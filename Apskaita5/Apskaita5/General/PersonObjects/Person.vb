Imports ApskaitaObjects.Attributes

Namespace General

    ''' <summary>
    ''' Represents a person that the company operates with (client, supplier, worker, etc.).
    ''' </summary>
    ''' <remarks>Related helper object (value object list) is <see cref="HelperLists.PersonInfoList">PersonInfoList</see>.
    ''' Values are stored in the database table asmenys.</remarks>
    <Serializable()> _
    Public NotInheritable Class Person
        Inherits BusinessBase(Of Person)
        Implements IIsDirtyEnough, IValidationMessageProvider

#Region " Business Methods "

        Friend Const SodraCodeMaxLength As Integer = 20

        Private _Guid As Guid = Guid.NewGuid
        Private _ID As Integer = 0
        Private _Name As String = ""
        Private _Code As String = ""
        Private _CodeIsNotReal As Boolean = False
        Private _Address As String = ""
        Private _Bank As String = ""
        Private _BankAccount As String = ""
        Private _CodeVAT As String = ""
        Private _CodeSODRA As String = ""
        Private _Email As String = ""
        Private _ContactInfo As String = ""
        Private _InternalCode As String = ""
        Private _LanguageCode As String = LanguageCodeLith
        Private _LanguageName As String = GetLanguageName(LanguageCodeLith, False)
        Private _CurrencyCode As String = GetCurrentCompany.BaseCurrency
        Private _StateCode As String = StateCodeLith
        Private _IsNaturalPerson As Boolean = False
        Private _IsObsolete As Boolean = False
        Private _AccountAgainstBankBuyer As Long = 0
        Private _AccountAgainstBankSupplyer As Long = 0
        Private _MustBeAssignedToWorkers As Boolean = False
        Private _AssignedToGroups As PersonGroupAssignmentList
        Private _IsClient As Boolean = False
        Private _IsSupplier As Boolean = False
        Private _IsWorker As Boolean = False
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now


        ''' <summary>
        ''' Gets an ID of the person (assigned automaticaly by DB AUTOINCREMENT).
        ''' Returns 0 for a new person.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.ID.</remarks>
        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the person data was inserted into the database.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.InsertDate.</remarks>
        Public ReadOnly Property InsertDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _InsertDate
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the person data was last updated.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.UpdateDate.</remarks>
        Public ReadOnly Property UpdateDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UpdateDate
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets an official name of the person.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.Pavad.</remarks>
        <StringField(ValueRequiredLevel.Mandatory, 100, False)> _
        Public Property Name() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Name.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Name.Trim <> value.Trim Then
                    _Name = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an official registration/personal code of the person.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.Kodas.</remarks>
        <StringField(ValueRequiredLevel.Mandatory, 20, False)> _
        Public Property Code() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Code.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Code.Trim <> value.Trim Then
                    _Code = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether the <see cref="Code">Code</see> is not real, 
        ''' i.e. assigned by the company (e.g. natural persons are not required
        ''' to provide their personal identification code).
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.CodeIsNotReal.</remarks>
        Public Property CodeIsNotReal() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CodeIsNotReal
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _CodeIsNotReal <> value Then
                    _CodeIsNotReal = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an address of the person.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.Adresas.</remarks>
        <StringField(ValueRequiredLevel.Mandatory, 255, False)> _
        Public Property Address() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Address.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Address.Trim <> value.Trim Then
                    _Address = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a name of the bank used by the person.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.Bank.</remarks>
        <StringField(ValueRequiredLevel.Optional, 100, False)> _
        Public Property Bank() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Bank.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Bank.Trim <> value.Trim Then
                    _Bank = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a bank account number used by the person.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.B_Sask.</remarks>
        <StringField(ValueRequiredLevel.Optional, 50, False)> _
        Public Property BankAccount() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _BankAccount.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _BankAccount.Trim <> value.Trim Then
                    _BankAccount = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a VAT payer code of the person.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.SP_kodas.</remarks>
        <StringField(ValueRequiredLevel.Optional, 20, False)> _
        Public Property CodeVAT() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CodeVAT.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _CodeVAT.Trim <> value.Trim Then
                    _CodeVAT = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a SODRA (social security) code of the person.
        ''' </summary>
        ''' <remarks>Only applicable to natural persons.
        ''' Value is stored in the database field asmenys.SD_kodas.</remarks>
        <StringField(ValueRequiredLevel.Optional, SodraCodeMaxLength, False)> _
        Public Property CodeSODRA() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CodeSODRA.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _CodeSODRA.Trim <> value.Trim Then
                    _CodeSODRA = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an email address of the person.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.E_Mail.</remarks>
        <StringField(ValueRequiredLevel.Optional, 40, False)> _
        Public Property Email() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Email.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Email.Trim <> value.Trim Then
                    _Email = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets any other person info, e.g. phone number, etc.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.ContactInfo.</remarks>
        <StringField(ValueRequiredLevel.Optional, 255, False)> _
        Public Property ContactInfo() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ContactInfo.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _ContactInfo.Trim <> value.Trim Then
                    _ContactInfo = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an internal code of the person for company's uses.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.InternalCode.</remarks>
        <StringField(ValueRequiredLevel.Optional, 255, False)> _
        Public Property InternalCode() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _InternalCode.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _InternalCode.Trim <> value.Trim Then
                    _InternalCode = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets default language ISO 639-1 code used by the person.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.LanguageCode.</remarks>
        <LanguageCodeField(ValueRequiredLevel.Mandatory, True)> _
        Public Property LanguageCode() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _LanguageCode.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _LanguageCode.Trim <> value.Trim Then
                    _LanguageCode = value.Trim
                    _LanguageName = GetLanguageName(_LanguageCode, False)
                    PropertyHasChanged()
                    PropertyHasChanged("LanguageName")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets default language used by the person.
        ''' </summary>
        ''' <remarks>Use <see cref="HelperLists.CompanyRegionalInfoList">CompanyRegionalInfoList</see> to get available languages.
        ''' Value is stored in the database field asmenys.LanguageCode.</remarks>
        <LanguageNameField(ValueRequiredLevel.Mandatory, True)> _
        Public Property LanguageName() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _LanguageName.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If GetLanguageCode(value.Trim, False) <> _LanguageCode Then
                    _LanguageName = value.Trim
                    _LanguageCode = GetLanguageCode(value.Trim, False)
                    PropertyHasChanged()
                    PropertyHasChanged("LanguageCode")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a state of the origin of the person (ISO 3166–1 alpha 2 code).
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.StateCode.</remarks>
        <CodeField(ValueRequiredLevel.Mandatory, ApskaitaObjects.Settings.CodeType.VmiState)> _
        Public Property StateCode() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _StateCode.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _StateCode.Trim <> value.Trim Then
                    _StateCode = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets default currency code used by the person.
        ''' </summary>
        ''' <remarks>Use <see cref="CurrencyCodes">CurrencyCodes()</see> for a datasource.
        ''' Value is stored in the database field asmenys.CurrencyCode.</remarks>
        <CurrencyField(ValueRequiredLevel.Mandatory)> _
        Public Property CurrencyCode() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CurrencyCode.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _CurrencyCode.Trim <> value.Trim Then
                    _CurrencyCode = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an account for buyers' debts.
        ''' </summary>
        ''' <remarks>Used when importing bank operations of type 'money received'. Credits this account, debits bank account.
        ''' Value is stored in the database field asmenys.B_Kor.</remarks>
        <AccountField(ValueRequiredLevel.Recommended, False, 1, 2, 3, 4, 5, 6)> _
        Public Property AccountAgainstBankBuyer() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountAgainstBankBuyer
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If _AccountAgainstBankBuyer <> value Then
                    _AccountAgainstBankBuyer = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an account for suppliers' debts.
        ''' </summary>
        ''' <remarks>Used when importing bank operations of type 'money transfered'. Debits this account, credits bank account.
        ''' Value is stored in the database field asmenys.B_Kor_Tiek.</remarks>
        <AccountField(ValueRequiredLevel.Recommended, False, 1, 2, 3, 4, 5, 6)> _
        Public Property AccountAgainstBankSupplyer() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountAgainstBankSupplyer
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If _AccountAgainstBankSupplyer <> value Then
                    _AccountAgainstBankSupplyer = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets if the person is a natural person, i.e. not a company.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.IsNaturalPerson.</remarks>
        Public Property IsNaturalPerson() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsNaturalPerson
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _IsNaturalPerson <> value Then
                    _IsNaturalPerson = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Whether a person is a client of the company.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.IsClient.</remarks>
        Public Property IsClient() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsClient
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _IsClient <> value Then
                    _IsClient = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Whether a person is a supplier of the company.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.IsSupplier.</remarks>
        Public Property IsSupplier() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsSupplier
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _IsSupplier <> value Then
                    _IsSupplier = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Whether a person is a worker of the company.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.IsWorker.</remarks>
        Public Property IsWorker() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsWorker
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                ' If _MustBeAssignedToWorkers AndAlso _IsWorker Then Exit Property
                If _IsWorker <> value Then
                    _IsWorker = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets if the person is no longer in use, i.e. not supposed to be displayed in combos.
        ''' </summary>
        ''' <remarks>Value is stored in the database field asmenys.IsObsolete.</remarks>
        Public Property IsObsolete() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsObsolete
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _IsObsolete <> value Then
                    _IsObsolete = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets a PersonGroupInfoList object associated with the person.
        ''' I.e. info about the groups to which the person is assigned.
        ''' </summary>
        ''' <remarks>Values are stored in the database table persons_group_assignments.</remarks>
        Public ReadOnly Property AssignedToGroups() As PersonGroupAssignmentList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssignedToGroups
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the person must be assigned to employees, i.e. its data was used in employee module.
        ''' </summary>
        Public ReadOnly Property MustBeAssignedToWorkers() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _MustBeAssignedToWorkers
            End Get
        End Property


        ''' <summary>
        ''' Returnes TRUE if the object is new and contains some user provided data 
        ''' OR
        ''' object is not new and was changed by the user.
        ''' </summary>
        Public ReadOnly Property IsDirtyEnough() As Boolean _
            Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Not IsNew Then Return IsDirty
                Return (Not String.IsNullOrEmpty(_Name.Trim) _
                    OrElse Not String.IsNullOrEmpty(_Code.Trim) _
                    OrElse Not String.IsNullOrEmpty(_Address.Trim) _
                    OrElse Not String.IsNullOrEmpty(_Bank.Trim) _
                    OrElse Not String.IsNullOrEmpty(_BankAccount.Trim) _
                    OrElse Not String.IsNullOrEmpty(_CodeVAT.Trim) _
                    OrElse Not String.IsNullOrEmpty(_CodeSODRA.Trim) _
                    OrElse Not String.IsNullOrEmpty(_Email.Trim) _
                    OrElse Not String.IsNullOrEmpty(_ContactInfo.Trim) _
                    OrElse Not String.IsNullOrEmpty(_InternalCode.Trim))
            End Get
        End Property


        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                If _AssignedToGroups Is Nothing Then
                    Return MyBase.IsDirty
                Else
                    Return MyBase.IsDirty OrElse _AssignedToGroups.IsDirty
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean _
            Implements IValidationMessageProvider.IsValid
            Get
                If _AssignedToGroups Is Nothing Then
                    Return MyBase.IsValid
                Else
                    Return MyBase.IsValid AndAlso _AssignedToGroups.IsValid
                End If
            End Get
        End Property



        Public Overrides Function Save() As Person

            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            Dim result As Person = MyBase.Save
            PersonInfoList.InvalidateCache()
            Return result

        End Function

        Friend Function SaveChild() As Person

            If Not Me.IsValid Then
                Throw New InvalidOperationException(String.Format(My.Resources.Common_ErrorInItem, _
                    Me.ToString, vbCrLf, GetAllBrokenRules()))
            End If

            Dim result As Person = Me.Clone

            If result.IsNew Then
                result.DoInsert()
            Else
                result.DoUpdate()
            End If

            Return result

        End Function


        Public Function GetAllBrokenRules() As String _
            Implements IValidationMessageProvider.GetAllBrokenRules
            Dim result As String = ""
            If Not MyBase.IsValid Then result = AddWithNewLine(result, _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error), False)
            If Not _AssignedToGroups Is Nothing AndAlso Not _AssignedToGroups.IsValid Then _
                result = AddWithNewLine(result, _AssignedToGroups.GetAllBrokenRules, False)
            Return result
        End Function

        Public Function GetAllWarnings() As String _
            Implements IValidationMessageProvider.GetAllWarnings
            Dim result As String = ""
            If MyBase.BrokenRulesCollection.WarningCount > 0 Then result = AddWithNewLine(result, _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning), False)
            If Not _AssignedToGroups Is Nothing AndAlso _AssignedToGroups.HasWarnings Then _
                result = AddWithNewLine(result, _AssignedToGroups.GetAllWarnings, False)
            Return result
        End Function

        Public Function HasWarnings() As Boolean _
            Implements IValidationMessageProvider.HasWarnings
            Return Me.BrokenRulesCollection.WarningCount > 0 OrElse _
                (Not _AssignedToGroups Is Nothing AndAlso _AssignedToGroups.HasWarnings)
        End Function


        ''' <summary>
        ''' Imports person data from an external <see cref="InvoiceInfo.ClientInfo">ClientInfo</see> object.
        ''' </summary>
        ''' <param name="info">An external <see cref="InvoiceInfo.ClientInfo">ClientInfo</see> object.</param>
        ''' <remarks>Used for integration purposes, e.g. copy/paste functionality for base business objects.</remarks>
        Public Sub AddWithPersonInfoData(ByVal info As InvoiceInfo.ClientInfo)

            _Address = info.Address
            _Code = info.Code
            _CodeVAT = info.CodeVAT
            _ContactInfo = info.Contacts
            _CurrencyCode = info.CurrencyCode
            _Email = info.Email
            _InternalCode = info.BreedCode
            _IsClient = info.IsClient
            _IsNaturalPerson = info.IsNaturalPerson
            _IsObsolete = info.IsObsolete
            _IsSupplier = info.IsSupplier
            _IsWorker = info.IsWorker
            _LanguageCode = info.LanguageCode
            _Name = info.Name

            PropertyHasChanged()

            ValidationRules.CheckRules()

        End Sub

        ''' <summary>
        ''' Exports person data to an external <see cref="InvoiceInfo.ClientInfo">ClientInfo</see> object.
        ''' </summary>
        ''' <remarks>Used for integration purposes, e.g. copy/paste functionality for base business objects.</remarks>
        Public Function GetPersonInfo() As InvoiceInfo.ClientInfo

            Dim result As New InvoiceInfo.ClientInfo

            result.Address = _Address
            result.Code = _Code
            result.CodeVAT = _CodeVAT
            result.Contacts = _ContactInfo
            result.CurrencyCode = _CurrencyCode
            result.Email = _Email
            result.BreedCode = _InternalCode
            result.IsClient = _IsClient
            result.IsNaturalPerson = _IsNaturalPerson
            result.IsObsolete = _IsObsolete
            result.IsSupplier = _IsSupplier
            result.IsWorker = _IsWorker
            result.LanguageCode = _LanguageCode
            result.Name = _Name

            Return result

        End Function


        Protected Overrides Function GetIdValue() As Object
            If IsNew Then
                Return _Guid.ToString()
            Else
                Return _ID.ToString
            End If
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("{0} ({1}), {2}", _Name, _Code, _Address)
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Csla.Validation.RuleArgs("Name"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Csla.Validation.RuleArgs("Code"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Csla.Validation.RuleArgs("Address"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Csla.Validation.RuleArgs("Bank"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Csla.Validation.RuleArgs("BankAccount"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Csla.Validation.RuleArgs("CodeVAT"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Csla.Validation.RuleArgs("Email"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Csla.Validation.RuleArgs("ContactInfo"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Csla.Validation.RuleArgs("InternalCode"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Csla.Validation.RuleArgs("StateCode"))
            ValidationRules.AddRule(AddressOf CommonValidation.AccountFieldValidation, _
                New Csla.Validation.RuleArgs("AccountAgainstBankBuyer"))
            ValidationRules.AddRule(AddressOf CommonValidation.AccountFieldValidation, _
                New Csla.Validation.RuleArgs("AccountAgainstBankSupplyer"))
            ValidationRules.AddRule(AddressOf CommonValidation.LanguageNameValidation, _
                New Csla.Validation.RuleArgs("LanguageName"))
            ValidationRules.AddRule(AddressOf CommonValidation.CurrencyFieldValidation, _
                New Csla.Validation.RuleArgs("CurrencyCode"))

            ValidationRules.AddRule(AddressOf CodeSODRAValidation, New Validation.RuleArgs("CodeSODRA"))
            ValidationRules.AddRule(AddressOf IsWorkerValidation, New Validation.RuleArgs("IsWorker"))
            ValidationRules.AddRule(AddressOf BaseGroupAssignmentValidation, _
                New Validation.RuleArgs("IsClient"))

            ValidationRules.AddDependantProperty("IsWorker", "CodeSODRA", False)
            ValidationRules.AddDependantProperty("IsWorker", "IsClient", False)
            ValidationRules.AddDependantProperty("IsSupplier", "IsClient", False)

        End Sub

        Private Shared Function CodeSODRAValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As Person = DirectCast(target, Person)

            If StringIsNullOrEmpty(valObj._CodeSODRA) AndAlso valObj._IsWorker Then
                e.Description = My.Resources.General_Person_SodraCodeRequired
                e.Severity = Validation.RuleSeverity.Warning
                Return False
            ElseIf valObj._IsWorker AndAlso valObj._CodeSODRA.Trim.Length > SodraCodeMaxLength Then
                e.Description = String.Format(My.Resources.Common_StringExceedsMaxLength, _
                    My.Resources.General_Person_CodeSODRA, SodraCodeMaxLength.ToString)
                e.Severity = Validation.RuleSeverity.Warning
                Return False
            End If

            Return True

        End Function

        Private Shared Function IsWorkerValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As Person = DirectCast(target, Person)

            If ValObj._MustBeAssignedToWorkers AndAlso Not ValObj._IsWorker Then
                e.Description = My.Resources.General_Person_IsWorkerRequired
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            Return True

        End Function

        Private Shared Function BaseGroupAssignmentValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As Person = DirectCast(target, Person)

            If Not ValObj._IsWorker AndAlso Not ValObj._IsClient AndAlso Not ValObj._IsSupplier Then
                e.Description = My.Resources.General_Person_NoBaseGroupAssignement
                e.Severity = Validation.RuleSeverity.Warning
                Return False
            End If

            Return True

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
            AuthorizationRules.AllowWrite("General.Person2")
        End Sub

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.Person2")
        End Function

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.Person1")
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.Person3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.Person3")
        End Function

#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets a new Person object.
        ''' </summary>
        Public Shared Function NewPerson() As Person
            Return DataPortal.Create(Of Person)()
        End Function

        ''' <summary>
        ''' Gets an existing Person.
        ''' </summary>
        ''' <param name="personID"><see cref="ID">ID</see> of the Person.</param>
        Public Shared Function GetPerson(ByVal personID As Integer) As Person
            Return DataPortal.Fetch(Of Person)(New Criteria(personID))
        End Function

        ''' <summary>
        ''' Deletes an existing Person from database.
        ''' </summary>
        ''' <param name="personID"><see cref="ID">ID</see> of the Person.</param>
        Public Shared Sub DeletePerson(ByVal personID As Integer)
            DataPortal.Delete(New Criteria(personID))
            PersonInfoList.InvalidateCache()
        End Sub

        ''' <summary>
        ''' Gets a new Person object as a child of a collection.
        ''' </summary>
        ''' <remarks>Should only be invoked server side.</remarks>
        Friend Shared Function NewPersonChild() As Person
            Return New Person(True)
        End Function


        Private Sub New()
            ' require use of factory methods
        End Sub

        Private Sub New(ByVal createChild As Boolean)
            ' require use of factory methods
            If createChild Then
                MarkAsChild()
                DoCreate()
            End If
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private _ID As Integer
            Public ReadOnly Property ID() As Integer
                Get
                    Return _ID
                End Get
            End Property
            Public Sub New(ByVal nID As Integer)
                _ID = nID
            End Sub
        End Class


        Protected Overrides Sub DataPortal_Create()
            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)
            DoCreate()
        End Sub

        Private Sub DoCreate()
            _AssignedToGroups = PersonGroupAssignmentList.GetPersonGroupAssignmentList(Me)
            _AccountAgainstBankBuyer = GetCurrentCompany.GetDefaultAccount(DefaultAccountType.Buyers)
            _AccountAgainstBankSupplyer = GetCurrentCompany.GetDefaultAccount(DefaultAccountType.Suppliers)
            ValidationRules.CheckRules()
        End Sub


        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecuritySelectDenied)

            Dim myComm As New SQLCommand("FetchPerson")
            myComm.AddParam("?PD", criteria.ID)

            Using myData As DataTable = myComm.Fetch

                If Not myData.Rows.Count > 0 Then
                    Throw New Exception(String.Format(My.Resources.Common_ObjectNotFound, _
                        My.Resources.General_Person_TypeName, criteria.ID.ToString))
                End If

                Dim dr As DataRow = myData.Rows(0)

                _ID = CIntSafe(dr.Item(0), 0)
                _Name = CStrSafe(dr.Item(1)).Trim
                _Code = CStrSafe(dr.Item(2)).Trim
                _Address = CStrSafe(dr.Item(3)).Trim
                _CodeVAT = CStrSafe(dr.Item(4)).Trim
                _BankAccount = CStrSafe(dr.Item(5)).Trim
                _Bank = CStrSafe(dr.Item(6)).Trim
                _AccountAgainstBankBuyer = CLongSafe(dr.Item(7), 0)
                _AccountAgainstBankSupplyer = CLongSafe(dr.Item(8), 0)
                _Email = CStrSafe(dr.Item(9)).Trim
                _CodeSODRA = CStrSafe(dr.Item(10)).Trim
                _MustBeAssignedToWorkers = ConvertDbBoolean(CIntSafe(dr.Item(11), 0))
                _ContactInfo = CStrSafe(dr.Item(12)).Trim
                _InternalCode = CStrSafe(dr.Item(13)).Trim
                _IsObsolete = ConvertDbBoolean(CIntSafe(dr.Item(14), 0))
                _IsNaturalPerson = ConvertDbBoolean(CIntSafe(dr.Item(15), 0))
                _LanguageCode = CStrSafe(dr.Item(16)).Trim
                _LanguageName = GetLanguageName(_LanguageCode, False)
                _CurrencyCode = CStrSafe(dr.Item(17)).Trim
                _IsClient = ConvertDbBoolean(CIntSafe(dr.Item(18), 0))
                _IsSupplier = ConvertDbBoolean(CIntSafe(dr.Item(19), 0))
                _IsWorker = ConvertDbBoolean(CIntSafe(dr.Item(20), 0))
                _InsertDate = DateTime.SpecifyKind(CDateTimeSafe(dr.Item(21), Date.UtcNow), _
                    DateTimeKind.Utc).ToLocalTime
                _UpdateDate = DateTime.SpecifyKind(CDateTimeSafe(dr.Item(22), Date.UtcNow), _
                    DateTimeKind.Utc).ToLocalTime
                _StateCode = CStrSafe(dr.Item(23)).Trim
                _CodeIsNotReal = ConvertDbBoolean(CIntSafe(dr.Item(24), 0))

            End Using

            _AssignedToGroups = PersonGroupAssignmentList.GetPersonGroupAssignmentList(Me)

            ValidationRules.CheckRules()

            MarkOld()

        End Sub


        Protected Overrides Sub DataPortal_Insert()

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)

            Me.ValidationRules.CheckRules()
            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            CheckIfPersonCodeUnique()

            DoInsert()

        End Sub

        Private Sub DoInsert()

            Dim myComm As New SQLCommand("AddPerson")
            AddWithParams(myComm)

            Using transaction As New SqlTransaction

                Try

                    myComm.Execute()

                    _ID = Convert.ToInt32(myComm.LastInsertID)

                    AssignedToGroups.Update(Me)

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

            MarkOld()

        End Sub

        Protected Overrides Sub DataPortal_Update()

            If Not CanEditObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityUpdateDenied)

            Me.ValidationRules.CheckRules()
            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            CheckIfPersonCodeUnique()
            CheckIfPersonMustBeEmployee()
            CheckIfUpdateDateChanged()

            DoUpdate()

        End Sub

        Private Sub DoUpdate()

            Dim myComm As New SQLCommand("UpdatePerson")
            AddWithParams(myComm)
            myComm.AddParam("?PD", _ID)

            Using transaction As New SqlTransaction

                Try

                    myComm.Execute()

                    _AssignedToGroups.Update(Me)

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using


            MarkOld()

        End Sub


        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_ID))
        End Sub

        Protected Overrides Sub DataPortal_Delete(ByVal criteria As Object)

            If Not CanDeleteObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityUpdateDenied)

            CheckIfPersonCouldBeDeleted(DirectCast(criteria, Criteria).ID)

            DoDelete(DirectCast(criteria, Criteria).ID)

        End Sub

        Private Sub DoDelete(ByVal personToDeleteID As Integer)

            Using transaction As New SqlTransaction

                Try

                    Dim myComm2 As New SQLCommand("DeletePerson")
                    myComm2.AddParam("?PD", personToDeleteID)
                    myComm2.Execute()

                    Dim myComm3 As New SQLCommand("DeleteAllPersonAssignments")
                    myComm3.AddParam("?PD", personToDeleteID)
                    myComm3.Execute()

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

            MarkNew()

        End Sub


        Private Sub AddWithParams(ByRef myComm As SQLCommand)

            myComm.AddParam("?PN", _Name.Trim)
            myComm.AddParam("?PC", _Code.Trim)
            myComm.AddParam("?PA", _Address.Trim)
            myComm.AddParam("?PB", _Bank.Trim)
            myComm.AddParam("?PT", _BankAccount.Trim)
            myComm.AddParam("?PV", _CodeVAT.Trim)
            myComm.AddParam("?PS", _CodeSODRA.Trim)
            myComm.AddParam("?PE", _Email.Trim)
            myComm.AddParam("?PQ", _AccountAgainstBankBuyer)
            myComm.AddParam("?PW", _AccountAgainstBankSupplyer)
            myComm.AddParam("?AI", _ContactInfo.Trim)
            myComm.AddParam("?AJ", ConvertDbBoolean(_IsObsolete))
            myComm.AddParam("?AK", _InternalCode.Trim)
            myComm.AddParam("?AL", ConvertDbBoolean(_IsNaturalPerson))
            myComm.AddParam("?AM", _LanguageCode.Trim)
            myComm.AddParam("?AN", _CurrencyCode.Trim)
            myComm.AddParam("?IC", ConvertDbBoolean(_IsClient))
            myComm.AddParam("?IP", ConvertDbBoolean(_IsSupplier))
            myComm.AddParam("?IW", ConvertDbBoolean(_IsWorker))
            myComm.AddParam("?CR", ConvertDbBoolean(_CodeIsNotReal))
            myComm.AddParam("?SC", _StateCode.Trim)

            _UpdateDate = DateTime.Now
            _UpdateDate = New DateTime(Convert.ToInt64(Math.Floor(_UpdateDate.Ticks / TimeSpan.TicksPerSecond) _
                * TimeSpan.TicksPerSecond))
            If Me.IsNew Then _InsertDate = _UpdateDate
            myComm.AddParam("?UD", _UpdateDate.ToUniversalTime)

        End Sub


        Private Sub CheckIfPersonCodeUnique()

            Dim myComm As New SQLCommand("GetPersonExists")
            myComm.AddParam("?PC", _Code)
            myComm.AddParam("?PD", _ID)
            Using myData As DataTable = myComm.Fetch
                If myData.Rows.Count > 0 AndAlso ConvertDbBoolean(CIntSafe(myData.Rows(0).Item(0), 0)) Then
                    Throw New Exception(My.Resources.General_Person_CodeNotUnique)
                End If
            End Using

        End Sub

        Private Sub CheckIfPersonMustBeEmployee()

            If IsNew Then Exit Sub

            Dim myComm As New SQLCommand("CheckIfPersonMustBeEmployee")
            myComm.AddParam("?PD", _ID)
            Using myData As DataTable = myComm.Fetch
                If myData.Rows.Count > 0 AndAlso ConvertDbBoolean(CIntSafe(myData.Rows(0).Item(0), 0)) _
                    AndAlso Not _IsWorker Then
                    _IsWorker = True
                End If
            End Using

        End Sub

        Private Shared Sub CheckIfPersonCouldBeDeleted(ByVal PersonID As Integer)

            Dim myComm As New SQLCommand("PersonCanBeDeleted")
            myComm.AddParam("?PD", PersonID)

            Using myData As DataTable = myComm.Fetch
                If myData.Rows.Count > 0 Then
                    If CIntSafe(myData.Rows(0).Item(0), 0) > 0 Then
                        Throw New Exception(My.Resources.General_Person_IsInUseGeneral)
                    ElseIf CIntSafe(myData.Rows(0).Item(1), 0) > 0 Then
                        Throw New Exception(My.Resources.General_Person_IsInUseAsWorker)
                    End If
                End If
            End Using

        End Sub

        Private Sub CheckIfUpdateDateChanged()

            Dim myComm As New SQLCommand("CheckIfPersonUpdateDateChanged")
            myComm.AddParam("?CD", _ID)

            Using myData As DataTable = myComm.Fetch

                If myData.Rows.Count < 1 OrElse CDateTimeSafe(myData.Rows(0).Item(0), _
                    Date.MinValue) = Date.MinValue Then

                    Throw New Exception(String.Format(My.Resources.Common_ObjectNotFound, _
                        My.Resources.General_Person_TypeName, _ID.ToString))

                End If

                If CTimeStampSafe(myData.Rows(0).Item(0)) <> _UpdateDate Then

                    Throw New Exception(My.Resources.Common_UpdateDateHasChanged)

                End If

            End Using

        End Sub

#End Region

    End Class

End Namespace