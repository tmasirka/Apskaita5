Imports ApskaitaObjects.HelperLists
Imports ApskaitaObjects.Attributes
Imports Csla.Validation
Imports ApskaitaObjects.Documents.BankDataExchangeProviders
Namespace Documents

    ''' <summary>
    ''' Represents a bank operation (transaction) data that is imported from an external source.
    ''' </summary>
    ''' <remarks>Uses a <see cref="BankOperation">BankOperation</see> to persist data.
    ''' Values are stored in the database table bankoperations.</remarks>
    <Serializable()> _
    Public NotInheritable Class BankOperationItem
        Inherits BusinessBase(Of BankOperationItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private ReadOnly _Guid As Guid = Guid.NewGuid
        Private _ChronologicValidator As SimpleChronologicValidator
        Private _Date As Date = Today
        Private _DocumentNumber As String = ""
        Private _PersonCode As String = ""
        Private _PersonName As String = ""
        Private _PersonBankAccount As String = ""
        Private _PersonBankName As String = ""
        Private _Content As String = ""
        Private _Inflow As Boolean = True
        Private _Currency As String = GetCurrentCompany.BaseCurrency
        Private _CurrencyRate As Double = 1
        Private _CurrencyRateInAccount As Double = 1
        Private _OriginalSum As Double = 0
        Private _SumLTLBank As Double = 0
        Private _SumLTL As Double = 0
        Private _SumInAccount As Double = 0
        Private _UniqueCode As String = ""
        Private _IsBankCosts As Boolean = False
        Private _Person As PersonInfo = Nothing
        Private _AccountCorresponding As Long = 0
        Private _ExistsInDatabase As Boolean = False
        Private _ProbablyExistsInDatabase As Boolean = False
        Private _OperationDatabaseID As Integer = 0
        Private _AccountBankCurrencyConversionCosts As Long = 0
        Private _BankCurrencyConversionCosts As Double = 0


        ''' <summary>
        ''' Gets an operation GUID.
        ''' </summary>
        ''' <remarks>Used for data exchange between this object and a <see cref="BankOperation">BankOperation</see>.</remarks>
        Public ReadOnly Property ItemGuid() As Guid
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Guid
            End Get
        End Property

        ''' <summary>
        ''' Gets <see cref="IChronologicValidator">IChronologicValidator</see> object that contains business restraints on inserting the operation.
        ''' </summary>
        ''' <remarks>A <see cref="SimpleChronologicValidator">SimpleChronologicValidator</see> is used to validate an bank operation chronological business rules.</remarks>
        Public ReadOnly Property ChronologicValidator() As SimpleChronologicValidator
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ChronologicValidator
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether the item represents a bank fee.
        ''' </summary>
        ''' <remarks>Bank fee transactions are handled in accordance with the properties :
        ''' <see cref="CashAccount.ManagingPerson">CashAccount.ManagingPerson</see> as <see cref="Person">Person</see> and
        ''' <see cref="CashAccount.BankFeeCostsAccount">CashAccount.BankFeeCostsAccount</see> as <see cref="AccountCorresponding">AccountCorresponding</see>.</remarks>
        Public Property IsBankCosts() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsBankCosts
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)

                CanWriteProperty(True)

                If IsBankCostsIsReadOnly Then Exit Property

                If _IsBankCosts <> value Then

                    _IsBankCosts = value

                    PropertyHasChanged()

                    If _IsBankCosts AndAlso Not Parent Is Nothing AndAlso _
                        TypeOf Parent Is BankOperationItemList Then

                        _AccountCorresponding = DirectCast(Parent, BankOperationItemList).Account.BankFeeCostsAccount
                        PropertyHasChanged("AccountCorresponding")

                        If _Person Is Nothing OrElse _Person.IsEmpty Then
                            _Person = DirectCast(Parent, BankOperationItemList).Bank
                            PropertyHasChanged("Person")
                        End If

                    End If

                End If

            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a <see cref="General.Person">person</see> who transfers money into the 
        ''' bank account or whom money is transfered to.
        ''' </summary>
        ''' <remarks>Use <see cref="HelperLists.PersonInfoList">PersonInfoList</see> as a datasource.
        ''' Value is handled by the encapsulated <see cref="General.JournalEntry.Person">journal entry Person property</see>.</remarks>
        <PersonField(ValueRequiredLevel.Mandatory)> _
        Public Property Person() As PersonInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Person
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As PersonInfo)
                CanWriteProperty(True)
                If Not (_Person Is Nothing AndAlso value Is Nothing) _
                    AndAlso Not (Not _Person Is Nothing AndAlso Not value Is Nothing _
                    AndAlso _Person = value) Then
                    _Person = value
                    PropertyHasChanged()
                    If Not _IsBankCosts Then SetAccountCorresponding(Nothing, True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a currency rate for the money transfered.
        ''' </summary>
        ''' <remarks>Due to automatic conversions by a bank 
        ''' one can transfer USD from EUR bank account and vice versa.
        ''' Value is stored in the database field bankoperations.CurrencyRate.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, ROUNDCURRENCYRATE)> _
        Public Property CurrencyRate() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrencyRate, ROUNDCURRENCYRATE)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)

                CanWriteProperty(True)

                If CurrencyRateIsReadOnly Then Exit Property

                If CRound(_CurrencyRate, ROUNDCURRENCYRATE) <> CRound(value, ROUNDCURRENCYRATE) Then

                    _CurrencyRate = CRound(value, ROUNDCURRENCYRATE)
                    PropertyHasChanged()

                    Recalculate("", True)

                End If

            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a currency rate for the <see cref="BankOperationItemList.Account">cash account</see>.
        ''' </summary>
        ''' <remarks>Value is stored in the database field bankoperations.CurrencyRateInAccount.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, ROUNDCURRENCYRATE)> _
        Public Property CurrencyRateInAccount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrencyRateInAccount, ROUNDCURRENCYRATE)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)

                CanWriteProperty(True)

                If CurrencyRateInAccountIsReadOnly Then Exit Property

                If CRound(_CurrencyRateInAccount, ROUNDCURRENCYRATE) <> CRound(value, ROUNDCURRENCYRATE) Then

                    _CurrencyRateInAccount = CRound(value, ROUNDCURRENCYRATE)
                    PropertyHasChanged()

                    Recalculate("", True)

                End If

            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an <see cref="General.Account.ID">account</see> 
        ''' that shall be debited or credited by the bank operation.
        ''' </summary>
        ''' <remarks>Correcponds to a single entry in <see cref="BankOperation.BookEntryItems">BankOperation.BookEntryItems</see>.</remarks>
        <AccountField(ValueRequiredLevel.Mandatory, False, 1, 2, 3, 4, 5, 6)> _
        Public Property AccountCorresponding() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountCorresponding
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If _AccountCorresponding <> value Then
                    _AccountCorresponding = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the <see cref="General.Account.ID">account</see> for the currency conversion
        ''' costs incured by a bank when converting currency from <see cref="Currency">Currency</see>
        ''' to <see cref="BankOperationItemList.Account">BankOperationItemList.Account</see> currency and vice versa.
        ''' </summary>
        ''' <remarks>Due to automatic conversions by a bank 
        ''' one can transfer USD from EUR bank account and vice versa.
        ''' Value is stored in the database field bankoperations.AccountBankCurrencyConversionCosts.</remarks>
        <AccountField(ValueRequiredLevel.Mandatory, True, 5, 6)> _
        Public Property AccountBankCurrencyConversionCosts() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountBankCurrencyConversionCosts
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If _AccountBankCurrencyConversionCosts <> value Then
                    _AccountBankCurrencyConversionCosts = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the costs of the currency conversion incured by a bank 
        ''' when converting currency from <see cref="Currency">Currency</see>
        ''' to <see cref="BankOperationItemList.Account">BankOperationItemList.Account</see> currency and vice versa.
        ''' </summary>
        ''' <remarks>Due to automatic conversions by a bank 
        ''' one can transfer USD from EUR bank account and vice versa.
        ''' Value is stored in the database field bankoperations.???.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, 2)> _
        Public ReadOnly Property BankCurrencyConversionCosts() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_BankCurrencyConversionCosts)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the bank operation number.
        ''' </summary>
        ''' <remarks>Value is handled by the encapsulated <see cref="General.JournalEntry.DocNumber">journal entry DocNumber property</see>.</remarks>
        <StringField(ValueRequiredLevel.Mandatory, 30)> _
        Public Property DocumentNumber() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DocumentNumber.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _DocumentNumber.Trim <> value.Trim Then
                    _DocumentNumber = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property


        ''' <summary>
        ''' Gets the date of the bank operation.
        ''' </summary>
        ''' <remarks>Value is handled by the encapsulated <see cref="General.JournalEntry.Date">journal entry Date property</see>.</remarks>
        Public ReadOnly Property [Date]() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Date
            End Get
        End Property

        ''' <summary>
        ''' Gets an original person code as it was specified in the import source.
        ''' </summary>
        ''' <remarks>Value is stored in the database field bankoperations.OriginalPerson.</remarks>
        Public ReadOnly Property PersonCode() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _PersonCode.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets an original person name as it was specified in the import source.
        ''' </summary>
        ''' <remarks>Value is stored in the database field bankoperations.OriginalPerson.</remarks>
        Public ReadOnly Property PersonName() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _PersonName.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets an original person bank account number as it was specified in the import source.
        ''' </summary>
        ''' <remarks>Value is not persisted, only used to import person data when necessary.</remarks>
        Public ReadOnly Property PersonBankAccount() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _PersonBankAccount.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets an original person bank name as it was specified in the import source.
        ''' </summary>
        ''' <remarks>Value is not persisted, only used to import person data when necessary.</remarks>
        Public ReadOnly Property PersonBankName() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _PersonBankName.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets an original content data (description) as it was specified in the import source.
        ''' </summary>
        ''' <remarks>Value is stored in the database field bankoperations.OriginalContent.</remarks>
        <StringField(ValueRequiredLevel.Mandatory, 255)> _
        Public ReadOnly Property Content() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Content.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the money is transfered into the bank account.
        ''' </summary>
        ''' <remarks>Value is stored as a sign of sum values in the database sum fields.
        ''' If the sum is positive, the operation is debit and vice versa.</remarks>
        Public ReadOnly Property Inflow() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Inflow
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether the money is transfered from the bank account.
        ''' </summary>
        ''' <remarks>Value is stored as a sign of sum values in the database sum fields.
        ''' If the sum is negative, the operation is credit and vice versa.</remarks>
        Public ReadOnly Property Payout() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _Inflow
            End Get
        End Property

        ''' <summary>
        ''' Gets a currency of the money transfered.
        ''' </summary>
        ''' <remarks>Due to automatic conversions by a bank 
        ''' one can transfer USD from EUR bank account and vice versa.
        ''' Value is stored in the database field bankoperations.CurrencyCode.</remarks>
        <CurrencyFieldAttribute(ValueRequiredLevel.Mandatory)> _
        Public ReadOnly Property Currency() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Currency.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets the (original) sum of bank transfer in <see cref="Currency">Currency</see>.
        ''' </summary>
        ''' <remarks>Value is stored in the database field bankoperations.SumOriginal.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, 2)> _
        Public ReadOnly Property OriginalSum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_OriginalSum)
            End Get
        End Property

        ''' <summary>
        ''' Gets the sum of bank transfer in base currency as indicated in the imported data.
        ''' </summary>
        ''' <remarks>Value is not stored, for general background info purposes only.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, 2)> _
        Public ReadOnly Property SumLTLBank() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumLTLBank)
            End Get
        End Property

        ''' <summary>
        ''' Gets the sum of bank transfer in base currency.
        ''' </summary>
        ''' <remarks>Value is stored in the database field bankoperations.SumLTL.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, 2)> _
        Public ReadOnly Property SumLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumLTL)
            End Get
        End Property

        ''' <summary>
        ''' Gets the sum of bank transfer in <see cref="BankOperationItemList.Account">Account Currency</see>,
        ''' i.e. the sum that was actualy added to the acount balance in the account currency.
        ''' </summary>
        ''' <remarks>Value is stored in the database field bankoperations.SumInAccount.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, 2)> _
        Public ReadOnly Property SumInAccount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumInAccount)
            End Get
        End Property

        ''' <summary>
        ''' Gets a unique code of the bank operation (specified by the bank in the imported data).
        ''' </summary>
        ''' <remarks>Used to identify already imported operations when importing data if the 
        ''' <see cref="CashAccount.EnforceUniqueOperationID">CashAccount.EnforceUniqueOperationID</see> 
        ''' property is set to true.
        ''' Value is stored in the database field bankoperations.U_ID.</remarks>
        Public ReadOnly Property UniqueCode() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UniqueCode.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the bank operation with the same <see cref="UniqueCode">UniqueCode</see>
        ''' already exists in a database.
        ''' </summary>
        ''' <remarks>Only applicable if the current <see cref="BankOperationItemList.Account">cash account</see>
        ''' property <see cref="CashAccount.EnforceUniqueOperationID">EnforceUniqueOperationID</see> is set to TRUE.</remarks>
        Public Property ExistsInDatabase() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ExistsInDatabase
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Friend Set(ByVal value As Boolean)
                _ExistsInDatabase = value
            End Set
        End Property

        ''' <summary>
        ''' Gets whether the bank operation with the similar data (same date, original person and sum in account) 
        ''' already exists in a database.
        ''' </summary>
        ''' <remarks>Only applicable if the current <see cref="BankOperationItemList.Account">cash account</see>
        ''' property <see cref="CashAccount.EnforceUniqueOperationID">EnforceUniqueOperationID</see> is set to FALSE.</remarks>
        Public ReadOnly Property ProbablyExistsInDatabase() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ProbablyExistsInDatabase
            End Get
        End Property

        ''' <summary>
        ''' Gets the <see cref="BankOperation.ID">BankOperation.ID</see> if the operation 
        ''' is identified with an operation that already exists in a database, i.e.
        ''' <see cref="ExistsInDatabase">ExistsInDatabase</see> equals true.
        ''' </summary>
        ''' <remarks>Only applicable if the current <see cref="BankOperationItemList.Account">cash account</see>
        ''' property <see cref="CashAccount.EnforceUniqueOperationID">EnforceUniqueOperationID</see> is set to TRUE.</remarks>
        Public ReadOnly Property OperationDatabaseID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OperationDatabaseID
            End Get
        End Property


        ''' <summary>
        ''' Whether the <see cref="IsBankCosts">IsBankCosts</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property IsBankCostsIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Inflow
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="CurrencyRate">CurrencyRate</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrencyRateIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return IsBaseCurrency(_Currency, GetCurrentCompany().BaseCurrency)
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="CurrencyRateInAccount">CurrencyRateInAccount</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrencyRateInAccountIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return (Not Parent Is Nothing AndAlso TypeOf Parent Is BankOperationItemList AndAlso _
                    Not DirectCast(Parent, BankOperationItemList).Account Is Nothing AndAlso _
                    Not IsBaseCurrency(DirectCast(Parent, BankOperationItemList).Account.CurrencyCode, _
                    GetCurrentCompany().BaseCurrency))
            End Get
        End Property


        Private Sub SetAccountCorresponding(ByVal nAccount As CashAccountInfo, ByVal raisePropertyHasChanged As Boolean)

            If _IsBankCosts Then

                If Not nAccount Is Nothing AndAlso Not nAccount.IsEmpty Then
                    _AccountCorresponding = nAccount.BankFeeCostsAccount
                ElseIf Not Parent Is Nothing AndAlso TypeOf Parent Is BankOperationItemList Then
                    _AccountCorresponding = DirectCast(Parent, BankOperationItemList).Account.BankFeeCostsAccount
                Else
                    _AccountCorresponding = 0
                End If

            Else

                If Not _Person Is Nothing AndAlso Not _Person.IsEmpty Then
                    If _Inflow Then
                        _AccountCorresponding = _Person.AccountAgainstBankBuyer
                    Else
                        _AccountCorresponding = _Person.AccountAgainstBankSupplyer
                    End If
                Else
                    _AccountCorresponding = 0
                End If

            End If

            If raisePropertyHasChanged Then PropertyHasChanged("AccountCorresponding")

        End Sub

        Private Sub Recalculate(ByVal nCurrencyCodeInAccount As String, ByVal raisePropertyChangedEvents As Boolean)

            Dim nAccountCurrency As String = GetCurrencyInAccount(nCurrencyCodeInAccount)
            Dim baseCurrency As String = GetCurrentCompany().BaseCurrency

            _SumLTL = ConvertToBaseCurrency(_OriginalSum, _Currency, _CurrencyRate, _
                GetCurrentCompany().BaseCurrency, 2, ROUNDCURRENCYRATE)

            If Not CurrenciesEquals(nAccountCurrency, _Currency, baseCurrency) AndAlso _
                CRound(_SumInAccount) > 0 AndAlso (CRound(_CurrencyRateInAccount, ROUNDCURRENCYRATE) > 0 _
                OrElse IsBaseCurrency(nAccountCurrency, baseCurrency)) Then

                Dim diff As Double = CRound(_SumLTL - ConvertToBaseCurrency(_SumInAccount, _
                    nAccountCurrency, _CurrencyRateInAccount, baseCurrency, 2, ROUNDCURRENCYRATE), 2)

                If _Inflow Then

                    _BankCurrencyConversionCosts = diff

                Else

                    _BankCurrencyConversionCosts = -diff

                End If

            Else

                _BankCurrencyConversionCosts = 0

            End If

            If raisePropertyChangedEvents Then
                PropertyHasChanged("SumLTL")
                PropertyHasChanged("BankCurrencyConversionCosts")
            End If

        End Sub

        Private Function GetCurrencyInAccount(ByVal nCurrencyInAccount As String) As String

            If StringIsNullOrEmpty(nCurrencyInAccount) Then

                If Parent Is Nothing OrElse Not TypeOf Parent Is BankOperationItemList OrElse _
                    DirectCast(Parent, BankOperationItemList).Account Is Nothing OrElse _
                    DirectCast(Parent, BankOperationItemList).Account.IsEmpty OrElse _
                    StringIsNullOrEmpty(DirectCast(Parent, BankOperationItemList).Account.CurrencyCode) Then

                    Return GetCurrentCompany.BaseCurrency

                Else

                    Return DirectCast(Parent, BankOperationItemList).Account.CurrencyCode.Trim.ToUpper

                End If

            Else

                Return nCurrencyInAccount.Trim.ToUpper

            End If

        End Function



        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString
            If IsValid Then Return ""
            Return String.Format(My.Resources.Common_ErrorInItem, Me.ToString, _
                vbCrLf, Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error))
        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString
            If BrokenRulesCollection.WarningCount < 1 Then Return ""
            Return String.Format(My.Resources.Common_WarningInItem, Me.ToString, _
                vbCrLf, Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning))
        End Function


        Friend Function IdentifyWithBankOperation(ByVal savedBankOperation As BankOperation, _
            ByVal account As HelperLists.CashAccountInfo) As Boolean

            If savedBankOperation Is Nothing OrElse savedBankOperation.IsNew OrElse _
                savedBankOperation.ImportedItemGuid = Guid.Empty OrElse _
                savedBankOperation.ImportedItemGuid <> Me._Guid Then Return False

            _Content = savedBankOperation.Content
            _OperationDatabaseID = savedBankOperation.ID
            _Date = savedBankOperation.Date
            _DocumentNumber = savedBankOperation.DocumentNumber
            _AccountBankCurrencyConversionCosts = savedBankOperation.AccountBankCurrencyConversionCosts
            _BankCurrencyConversionCosts = savedBankOperation.BankCurrencyConversionCosts

            If savedBankOperation.IsTransferBetweenAccounts AndAlso _
                savedBankOperation.CreditCashAccount.ID = account.ID Then

                _Inflow = False
                _Person = Nothing
                _IsBankCosts = False
                _UniqueCode = savedBankOperation.UniqueCodeInCreditAccount
                _Currency = savedBankOperation.AccountCurrency
                _CurrencyRate = savedBankOperation.CurrencyRateInAccount
                _CurrencyRateInAccount = savedBankOperation.CurrencyRate
                _OriginalSum = savedBankOperation.SumInAccount
                _SumInAccount = savedBankOperation.Sum
                _SumLTL = savedBankOperation.SumLTL
                _AccountCorresponding = savedBankOperation.Account.Account

            Else

                _Inflow = savedBankOperation.IsDebit
                _Person = savedBankOperation.Person
                _IsBankCosts = (Not savedBankOperation.IsTransferBetweenAccounts AndAlso _
                    Not savedBankOperation.Person Is Nothing AndAlso _
                    savedBankOperation.Person.ID = account.ManagingPersonID)
                _UniqueCode = savedBankOperation.UniqueCode
                _Currency = savedBankOperation.CurrencyCode
                _CurrencyRate = savedBankOperation.CurrencyRate
                _CurrencyRateInAccount = savedBankOperation.CurrencyRateInAccount
                _OriginalSum = savedBankOperation.Sum
                _SumInAccount = savedBankOperation.SumInAccount
                _SumLTL = savedBankOperation.SumLTL

                If savedBankOperation.BookEntryItems.Count > 0 Then
                    _AccountCorresponding = savedBankOperation.BookEntryItems(0).Account
                End If

            End If

            _ExistsInDatabase = True
            _ProbablyExistsInDatabase = True

            Me.ValidationRules.CheckRules()

            Me.OnUnknownPropertyChanged()

            Return True

        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            Return String.Format(My.Resources.Documents_BankOperationItem_ToString, _Date.ToString("yyyy-MM-dd"), _
                _DocumentNumber, _PersonName, _PersonCode)
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean

            If obj Is Nothing OrElse (Not TypeOf obj Is BankOperation AndAlso _
                Not TypeOf obj Is BankOperationItem) Then
                Return False
            End If

            If TypeOf obj Is BankOperation Then

                If DirectCast(obj, BankOperation).ID = _OperationDatabaseID Then
                    Return True
                End If

            Else

                If DirectCast(obj, BankOperationItem)._Guid = _Guid Then
                    Return True
                ElseIf _OperationDatabaseID > 0 AndAlso _OperationDatabaseID = _
                    DirectCast(obj, BankOperationItem)._OperationDatabaseID Then
                    Return True
                End If

            End If

            Return False

        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf AccountRequiredWhenNewValidation, _
                New Validation.RuleArgs("AccountCorresponding"))
            ValidationRules.AddRule(AddressOf StringRequiredWhenNewValidation, _
                New Validation.RuleArgs("Content"))
            ValidationRules.AddRule(AddressOf StringRequiredWhenNewValidation, _
                New Validation.RuleArgs("DocumentNumber"))
            ValidationRules.AddRule(AddressOf DoubleRequiredWhenNewValidation, _
                New Validation.RuleArgs("OriginalSum"))
            ValidationRules.AddRule(AddressOf DoubleRequiredWhenNewValidation, _
                New Validation.RuleArgs("CurrencyRate"))
            ValidationRules.AddRule(AddressOf DoubleRequiredWhenNewValidation, _
                New Validation.RuleArgs("CurrencyRateInAccount"))
            ValidationRules.AddRule(AddressOf DoubleRequiredWhenNewValidation, _
                New Validation.RuleArgs("SumInAccount"))
            ValidationRules.AddRule(AddressOf CurrencyRequiredWhenNewValidation, _
                New Validation.RuleArgs("Currency"))
            ValidationRules.AddRule(AddressOf DateWhenNewValidation, _
                New CommonValidation.CommonValidation.ChronologyRuleArgs("Date", "ChronologicValidator"))

            ValidationRules.AddRule(AddressOf ProbabilityOfBankCostsValidation, _
                New Validation.RuleArgs("OriginalSum"))
            ValidationRules.AddRule(AddressOf ClientValidation, New Validation.RuleArgs("Person"))
            ValidationRules.AddRule(AddressOf AccountBankCurrencyConversionCostsValidation, _
                New Validation.RuleArgs("AccountBankCurrencyConversionCosts"))

            ValidationRules.AddDependantProperty("BankCurrencyConversionCosts", _
                "AccountBankCurrencyConversionCosts", False)
            ValidationRules.AddDependantProperty("IsBankCosts", "Person", False)

        End Sub

        ''' <summary>
        ''' Rule ensuring that required and recommended double values are set when the item is new (not already in database).
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function DoubleRequiredWhenNewValidation( _
            ByVal target As Object, ByVal e As Validation.RuleArgs) As Boolean

            If DirectCast(target, BankOperationItem).ExistsInDatabase Then Return True

            Return DoubleFieldValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that required and recommended account values are set when the item is new (not already in database).
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AccountRequiredWhenNewValidation( _
            ByVal target As Object, ByVal e As Validation.RuleArgs) As Boolean

            If DirectCast(target, BankOperationItem).ExistsInDatabase Then Return True

            Return AccountFieldValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that required and recommended string values are set when the item is new (not already in database).
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function StringRequiredWhenNewValidation( _
            ByVal target As Object, ByVal e As Validation.RuleArgs) As Boolean

            If DirectCast(target, BankOperationItem).ExistsInDatabase Then Return True

            Return StringFieldValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that required and recommended currency values are set when the item is new (not already in database).
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function CurrencyRequiredWhenNewValidation( _
            ByVal target As Object, ByVal e As Validation.RuleArgs) As Boolean

            If DirectCast(target, BankOperationItem).ExistsInDatabase Then Return True

            Return CurrencyFieldValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that operation date is valid when the item is new (not already in database).
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function DateWhenNewValidation( _
            ByVal target As Object, ByVal e As Validation.RuleArgs) As Boolean

            If DirectCast(target, BankOperationItem).ExistsInDatabase Then Return True

            Return ChronologyValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that the value of property Sum is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function ProbabilityOfBankCostsValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As BankOperationItem = DirectCast(target, BankOperationItem)

            If valObj.ExistsInDatabase Then Return True

            If valObj.Parent Is Nothing OrElse Not TypeOf valObj.Parent Is BankOperationItemList _
                OrElse Not DirectCast(valObj.Parent, BankOperationItemList).Account.BankFeeLimit > 0 Then _
                Return True

            If CRound(valObj._SumInAccount) <= DirectCast(valObj.Parent,  _
                BankOperationItemList).Account.BankFeeLimit AndAlso Not valObj._IsBankCosts Then
                e.Description = My.Resources.Documents_BankOperationItem_BankFeeProbability
                e.Severity = Validation.RuleSeverity.Warning
                Return False
            End If

            If CRound(valObj._SumInAccount) > DirectCast(valObj.Parent,  _
                BankOperationItemList).Account.BankFeeLimit AndAlso valObj.IsBankCosts Then
                e.Description = My.Resources.Documents_BankOperationItem_NotBankFeeProbability
                e.Severity = Validation.RuleSeverity.Warning
                Return False
            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that the client is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function ClientValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As BankOperationItem = DirectCast(target, BankOperationItem)

            If valObj._ExistsInDatabase Then Return True

            If valObj._Person Is Nothing OrElse valObj._Person.IsEmpty Then

                e.Description = String.Format(My.Resources.Common_FieldValueNull, _
                    My.Resources.Documents_BankOperationItem_Person)
                If valObj._IsBankCosts Then
                    e.Severity = RuleSeverity.Warning
                Else
                    e.Severity = RuleSeverity.Error
                End If

                Return False

            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that AccountBankCurrencyConversionCosts is set when needed.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AccountBankCurrencyConversionCostsValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As BankOperationItem = DirectCast(target, BankOperationItem)

            If valObj.ExistsInDatabase OrElse CRound(valObj._BankCurrencyConversionCosts) = 0.0 Then Return True

            Return CommonValidation.CommonValidation.AccountFieldValidation(target, e)

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

#End Region

#Region " Factory Methods "

        Friend Shared Function GetBankOperationItem(ByVal operationData As BankAccountStatementItem, _
            ByVal account As CashAccountInfo, ByVal bankDocumentPrefix As String) As BankOperationItem
            Return New BankOperationItem(operationData, account, bankDocumentPrefix)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub

        Private Sub New(ByVal operationData As BankAccountStatementItem, _
            ByVal account As CashAccountInfo, ByVal bankDocumentPrefix As String)
            MarkAsChild()
            Fetch(operationData, account, bankDocumentPrefix)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Fetch(ByVal operationData As BankAccountStatementItem, _
            ByVal account As CashAccountInfo, ByVal bankDocumentPrefix As String)

            _Date = operationData.Date
            _DocumentNumber = GetLimitedLengthString(((bankDocumentPrefix _
                & " " & operationData.DocumentNumber).Trim).Trim, 30)
            _PersonCode = operationData.PersonCode
            _PersonName = operationData.PersonName
            _Content = GetLimitedLengthString(operationData.Content, 255)
            _Inflow = operationData.Inflow
            _Currency = operationData.Currency
            If Not IsValidCurrency(_Currency, True) Then
                _Currency = GetCurrentCompany.BaseCurrency
            End If
            _OriginalSum = operationData.OriginalSum
            _SumLTLBank = operationData.SumLTL
            _SumInAccount = operationData.SumInAccount
            If Not CRound(_OriginalSum) > 0 Then
                _OriginalSum = _SumInAccount
            End If
            _UniqueCode = operationData.UniqueCode
            _PersonBankAccount = operationData.PersonBankAccount
            _PersonBankName = operationData.PersonBankName

            If StringIsNullOrEmpty(_Content) Then
                _Content = My.Resources.Documents_BankOperationItem_DefaultContent
            End If

            If IsBaseCurrency(_Currency, GetCurrentCompany.BaseCurrency) Then
                _CurrencyRate = 1
            Else
                _CurrencyRate = 0
            End If
            If IsBaseCurrency(account.CurrencyCode, GetCurrentCompany.BaseCurrency) Then
                _CurrencyRateInAccount = 1
            Else
                _CurrencyRateInAccount = 0
            End If

            _ChronologicValidator = SimpleChronologicValidator. _
                NewSimpleChronologicValidator(My.Resources.Documents_BankOperation_TypeName, Nothing)

            Recalculate(account.CurrencyCode, False)

        End Sub


        <NonSerialized(), NotUndoable()> _
        Private _UnderlyingBankOperation As BankOperation = Nothing

        Friend Sub PrepareForInsert(ByVal bankAccount As HelperLists.CashAccountInfo)

            If _ExistsInDatabase Then Exit Sub

            Try
                _UnderlyingBankOperation = BankOperation.NewBankOperationForInsert(Me, bankAccount)
            Catch ex As Exception
                Throw New Exception(String.Format(My.Resources.Documents_BankOperation_FailedCreateBankOperation, _
                    Me.ToString, vbCrLf, ex.Message), ex)
            End Try

        End Sub

        Friend Sub Insert()

            If _ExistsInDatabase Then Exit Sub

            If _UnderlyingBankOperation Is Nothing Then
                Throw New Exception("Underlying Bank Operation should be prepared before invoking Insert BankOperationItem.")
            ElseIf Not _UnderlyingBankOperation Is Nothing AndAlso Not _UnderlyingBankOperation.IsNew Then
                Throw New Exception("Underlying Bank Operation should be new when invoking Insert BankOperationItem.")
            End If

            _UnderlyingBankOperation.DoInsert()

            _ExistsInDatabase = True
            _ProbablyExistsInDatabase = True
            _OperationDatabaseID = _UnderlyingBankOperation.ID

        End Sub


        Friend Sub FixUniqueCodes(ByRef item As BankOperationItem)

            If item._Guid <> Me._Guid AndAlso item._UniqueCode.Trim.ToUpper = Me._UniqueCode.Trim.ToUpper Then
                If CRound(item._SumInAccount) > CRound(Me._SumInAccount) Then
                    Me._UniqueCode = Me._UniqueCode.Trim & "1"
                Else
                    item._UniqueCode = item._UniqueCode.Trim & "1"
                End If
            End If

        End Sub

        Friend Sub RecognizeItem(ByVal nAccount As CashAccountInfo, ByVal bankPersonInfo As PersonInfo)

            If Parent Is Nothing OrElse Not TypeOf Parent Is BankOperationItemList Then
                Throw New InvalidOperationException(My.Resources.Documents_BankOperationItem_InvalidInvokeRecognizeItem)
            End If

            Dim myComm As SQLCommand
            If nAccount.EnforceUniqueOperationID Then
                myComm = New SQLCommand("RecognizeBankOperationItemByUniqueCode")
                myComm.AddParam("?UC", Me._UniqueCode.Trim)
            Else
                myComm = New SQLCommand("RecognizeBankOperationItemByBestGuess")
                myComm.AddParam("?DT", Me._Date.Date)
                myComm.AddParam("?BF", nAccount.BankFeeLimit)
                myComm.AddParam("?SM", CRound(Me._SumInAccount))
                myComm.AddParam("?OC", Me.PersonName.Trim & " (" & Me._PersonCode.Trim & ")")
            End If
            myComm.AddParam("?DC", Me._PersonBankAccount.Trim)
            myComm.AddParam("?CD", Me._PersonCode.Trim)
            myComm.AddParam("?AD", nAccount.ID)

            Using myData As DataTable = myComm.Fetch

                If myData.Rows.Count > 0 Then

                    Dim dr As DataRow = myData.Rows(0)
                    If myData.Rows.Count > 1 AndAlso CIntSafe(myData.Rows(1).Item(0), 0) > 0 Then _
                        dr = myData.Rows(1)

                    _ProbablyExistsInDatabase = ConvertDbBoolean(CIntSafe(dr.Item(0), 0))
                    _AccountCorresponding = CLongSafe(dr.Item(1), 0)
                    _Person = PersonInfo.GetPersonInfo(dr, 2)

                    If nAccount.EnforceUniqueOperationID Then
                        _ExistsInDatabase = _ProbablyExistsInDatabase
                        _OperationDatabaseID = CIntSafe(myData.Rows(0).Item(0), 0)
                    End If

                End If

            End Using

            If Not _ExistsInDatabase Then

                _IsBankCosts = ((_Person Is Nothing OrElse _Person.IsEmpty OrElse _
                    (Not bankPersonInfo Is Nothing AndAlso bankPersonInfo = _Person)) AndAlso _
                    CRound(_SumInAccount) > 0 AndAlso CRound(_SumInAccount) <= nAccount.BankFeeLimit)

                If _IsBankCosts AndAlso Not bankPersonInfo Is Nothing AndAlso Not bankPersonInfo.IsEmpty _
                    AndAlso (_Person Is Nothing OrElse _Person.IsEmpty) Then
                    _Person = bankPersonInfo
                End If

                If Not _AccountCorresponding > 0 Then SetAccountCorresponding(nAccount, False)

                ValidationRules.CheckRules()

            End If

        End Sub

#End Region

    End Class

End Namespace