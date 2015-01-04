Imports System.Security
Namespace General

    <Serializable()> _
    Public Class Company
        Inherits BusinessBase(Of Company)
        Implements IIsDirtyEnough

#Region " Business Methods "

        Private _BaseCurrency As String = "LTL"
        Private _Code As String = ""
        Private _Name As String = ""
        Private _CodeVat As String = ""
        Private _Email As String = ""
        Private _HeadPerson As String = ""
        Private _Accountant As String = ""
        Private _Cashier As String = ""
        Private _CodeSODRA As String = ""
        Private _HeadPersonSignature As Byte() = Nothing
        Private _NumbersInInvoice As Integer = 0
        Private _AddDateToInvoiceNumber As Boolean = False
        Private _AddDateToTillIncomeOrderNumber As Boolean = False
        Private _AddDateToTillSpendingsOrderNumber As Boolean = False
        Private _DefaultInvoiceMadeContent As String = ""
        Private _DefaultInvoiceReceivedContent As String = ""
        Private _MeasureUnitInvoiceReceived As String = ""
        Private _AccountClassPrefix11 As Integer = 1
        Private _AccountClassPrefix12 As Integer = 0
        Private _AccountClassPrefix21 As Integer = 2
        Private _AccountClassPrefix22 As Integer = 0
        Private _AccountClassPrefix31 As Integer = 3
        Private _AccountClassPrefix32 As Integer = 0
        Private _AccountClassPrefix41 As Integer = 4
        Private _AccountClassPrefix42 As Integer = 0
        Private _AccountClassPrefix51 As Integer = 5
        Private _AccountClassPrefix52 As Integer = 0
        Private _AccountClassPrefix61 As Integer = 6
        Private _AccountClassPrefix62 As Integer = 0
        Private _DefaultTaxRates As CompanyRateList = Nothing
        Private _Accounts As CompanyAccountList = Nothing
        Private _DefaultTaxNpdFormula As String = "NPD-(imz(PAJ-290)*0,26)"
        Private _CompanyDatabaseName As String = ""


        Public Property BaseCurrency() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _BaseCurrency.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If Not IsNew Then Exit Property
                If value Is Nothing Then value = ""
                If _BaseCurrency.Trim <> value.Trim Then
                    _BaseCurrency = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

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

        Public Property CodeVat() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CodeVat.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _CodeVat.Trim <> value.Trim Then
                    _CodeVat = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

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

        Public Property HeadPerson() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _HeadPerson.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _HeadPerson.Trim <> value.Trim Then
                    _HeadPerson = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property Accountant() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Accountant.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Accountant.Trim <> value.Trim Then
                    _Accountant = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property Cashier() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Cashier.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Cashier.Trim <> value.Trim Then
                    _Cashier = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

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

        Public Property HeadPersonSignature() As System.Drawing.Image
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return ByteArrayToImage(_HeadPersonSignature)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As System.Drawing.Image)
                CanWriteProperty(True)

                If value Is Nothing AndAlso (_HeadPersonSignature Is Nothing OrElse _
                    Not _HeadPersonSignature.Length > 50) Then Exit Property

                If value Is Nothing Then
                    _HeadPersonSignature = Nothing
                    PropertyHasChanged()
                    Exit Property
                End If

                Dim valueArray As Byte() = ImageToByteArray(value)

                If _HeadPersonSignature Is Nothing OrElse valueArray Is Nothing OrElse _
                    Not _HeadPersonSignature.Equals(valueArray) Then
                    _HeadPersonSignature = valueArray
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property NumbersInInvoice() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _NumbersInInvoice
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 20 Then value = 20
                If _NumbersInInvoice <> value Then
                    _NumbersInInvoice = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AddDateToInvoiceNumber() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AddDateToInvoiceNumber
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _AddDateToInvoiceNumber <> value Then
                    _AddDateToInvoiceNumber = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AddDateToTillIncomeOrderNumber() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AddDateToTillIncomeOrderNumber
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _AddDateToTillIncomeOrderNumber <> value Then
                    _AddDateToTillIncomeOrderNumber = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AddDateToTillSpendingsOrderNumber() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AddDateToTillSpendingsOrderNumber
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _AddDateToTillSpendingsOrderNumber <> value Then
                    _AddDateToTillSpendingsOrderNumber = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property DefaultInvoiceMadeContent() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DefaultInvoiceMadeContent.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _DefaultInvoiceMadeContent.Trim <> value.Trim Then
                    _DefaultInvoiceMadeContent = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property DefaultInvoiceReceivedContent() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DefaultInvoiceReceivedContent.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _DefaultInvoiceReceivedContent.Trim <> value.Trim Then
                    _DefaultInvoiceReceivedContent = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property MeasureUnitInvoiceReceived() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _MeasureUnitInvoiceReceived.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _MeasureUnitInvoiceReceived.Trim <> value.Trim Then
                    _MeasureUnitInvoiceReceived = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix11() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix11
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix11 <> value Then
                    _AccountClassPrefix11 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix12() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix12
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix12 <> value Then
                    _AccountClassPrefix12 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix21() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix21
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix21 <> value Then
                    _AccountClassPrefix21 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix22() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix22
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix22 <> value Then
                    _AccountClassPrefix22 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix31() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix31
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix31 <> value Then
                    _AccountClassPrefix31 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix32() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix32
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix32 <> value Then
                    _AccountClassPrefix32 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix41() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix41
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix41 <> value Then
                    _AccountClassPrefix41 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix42() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix42
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix42 <> value Then
                    _AccountClassPrefix42 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix51() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix51
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix51 <> value Then
                    _AccountClassPrefix51 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix52() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix52
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix52 <> value Then
                    _AccountClassPrefix52 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix61() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix61
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix61 <> value Then
                    _AccountClassPrefix61 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountClassPrefix62() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix62
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If value < 0 Then value = 0
                If value > 9 Then value = 9
                If _AccountClassPrefix62 <> value Then
                    _AccountClassPrefix62 = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property DefaultTaxNpdFormula() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DefaultTaxNpdFormula.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _DefaultTaxNpdFormula.Trim <> value.Trim Then
                    _DefaultTaxNpdFormula = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public ReadOnly Property DefaultTaxRates() As CompanyRateList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DefaultTaxRates
            End Get
        End Property

        Public ReadOnly Property Accounts() As CompanyAccountList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Accounts
            End Get
        End Property

        Public ReadOnly Property CompanyDatabaseName() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CompanyDatabaseName
            End Get
        End Property


        Public ReadOnly Property IsDirtyEnough() As Boolean _
            Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Not IsNew Then Return IsDirty
                Return (Not String.IsNullOrEmpty(_Code.Trim) _
                    OrElse Not String.IsNullOrEmpty(_Name.Trim) _
                    OrElse Not String.IsNullOrEmpty(_CodeVat.Trim) _
                    OrElse Not String.IsNullOrEmpty(_Email.Trim) _
                    OrElse Not String.IsNullOrEmpty(_HeadPerson.Trim) _
                    OrElse Not String.IsNullOrEmpty(_CodeSODRA.Trim) _
                    OrElse Not String.IsNullOrEmpty(_DefaultInvoiceMadeContent.Trim) _
                    OrElse Not String.IsNullOrEmpty(_DefaultInvoiceReceivedContent.Trim) _
                    OrElse Not String.IsNullOrEmpty(_DefaultTaxNpdFormula.Trim))
            End Get
        End Property

        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse _Accounts.IsDirty OrElse _DefaultTaxRates.IsDirty
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return MyBase.IsValid AndAlso _Accounts.IsValid AndAlso _DefaultTaxRates.IsValid
            End Get
        End Property



        Public Overrides Function Save() As Company

            Me.ValidationRules.CheckRules()
            If Not IsValid Then Throw New Exception("Duomenyse yra klaidų: " & vbCrLf _
                & Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error))

            Dim result As Company = MyBase.Save
            Settings.CommonSettings.InvalidateCache()
            Return result

        End Function

        Public Function HasWarnings() As Boolean
            Return Me.BrokenRulesCollection.WarningCount > 0 OrElse _Accounts.HasWarnings _
                OrElse _DefaultTaxRates.HasWarnings
        End Function

        Public Function GetAllErrors() As String
            Dim result As String = ""
            If Not MyBase.IsValid Then result = MyBase.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error)
            If Not _DefaultTaxRates.IsValid Then
                If String.IsNullOrEmpty(result.Trim) Then
                    result = _DefaultTaxRates.GetAllBrokenRules
                Else
                    result = result & vbCrLf & _DefaultTaxRates.GetAllBrokenRules
                End If
            End If
            If Not _Accounts.IsValid Then
                If String.IsNullOrEmpty(result.Trim) Then
                    result = _Accounts.GetAllBrokenRules
                Else
                    result = result & vbCrLf & _Accounts.GetAllBrokenRules
                End If
            End If
            Return result
        End Function

        Public Function GetAllWarnings() As String
            Dim result As String = ""
            If MyBase.BrokenRulesCollection.WarningCount > 0 Then _
                result = MyBase.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning)
            If _DefaultTaxRates.HasWarnings Then
                If String.IsNullOrEmpty(result.Trim) Then
                    result = _DefaultTaxRates.GetAllWarnings
                Else
                    result = result & vbCrLf & _DefaultTaxRates.GetAllWarnings
                End If
            End If
            If _Accounts.HasWarnings Then
                If String.IsNullOrEmpty(result.Trim) Then
                    result = _Accounts.GetAllWarnings
                Else
                    result = result & vbCrLf & _Accounts.GetAllWarnings
                End If
            End If
            Return result
        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _Code
        End Function

        Public Overrides Function ToString() As String
            If _Code Is Nothing OrElse String.IsNullOrEmpty(_Code.Trim) Then Return ""
            Return _Name & ", " & _Code
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.CurrencyValid, _
                New CommonValidation.SimpleRuleArgs("BaseCurrency", "bazinė įmonės valiuta"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.SimpleRuleArgs("Code", "įmonės kodas"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.SimpleRuleArgs("Name", "įmonės pavadinimas"))
            ValidationRules.AddRule(AddressOf AccountClassValidation, _
                New Validation.RuleArgs("AccountClassPrefix12"))
            ValidationRules.AddRule(AddressOf NpdFormulaValidation, _
                New Validation.RuleArgs("DefaultTaxNpdFormula"))

            ValidationRules.AddDependantProperty("AccountClassPrefix11", "AccountClassPrefix12", False)
            ValidationRules.AddDependantProperty("AccountClassPrefix21", "AccountClassPrefix12", False)
            ValidationRules.AddDependantProperty("AccountClassPrefix22", "AccountClassPrefix12", False)
            ValidationRules.AddDependantProperty("AccountClassPrefix31", "AccountClassPrefix12", False)
            ValidationRules.AddDependantProperty("AccountClassPrefix32", "AccountClassPrefix12", False)
            ValidationRules.AddDependantProperty("AccountClassPrefix41", "AccountClassPrefix12", False)
            ValidationRules.AddDependantProperty("AccountClassPrefix42", "AccountClassPrefix12", False)
            ValidationRules.AddDependantProperty("AccountClassPrefix51", "AccountClassPrefix12", False)
            ValidationRules.AddDependantProperty("AccountClassPrefix52", "AccountClassPrefix12", False)
            ValidationRules.AddDependantProperty("AccountClassPrefix61", "AccountClassPrefix12", False)
            ValidationRules.AddDependantProperty("AccountClassPrefix62", "AccountClassPrefix12", False)

        End Sub

        ''' <summary>
        ''' Rule ensuring that the value of property AccountClassPrefix11 is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AccountClassValidation(ByVal target As Object, _
        ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As Company = DirectCast(target, Company)
            Dim l As New List(Of Integer)

            If Not ValObj._AccountClassPrefix11 > 0 Then
                e.Description = "Klaida. Nenurodytas nė vienas klasės prefiksas 1 klasei."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            ElseIf Not ValObj._AccountClassPrefix21 > 0 Then
                e.Description = "Klaida. Nenurodytas nė vienas klasės prefiksas 2 klasei."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            ElseIf Not ValObj._AccountClassPrefix31 > 0 Then
                e.Description = "Klaida. Nenurodytas nė vienas klasės prefiksas 3 klasei."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            ElseIf Not ValObj._AccountClassPrefix41 > 0 Then
                e.Description = "Klaida. Nenurodytas nė vienas klasės prefiksas 4 klasei."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            ElseIf Not ValObj._AccountClassPrefix51 > 0 Then
                e.Description = "Klaida. Nenurodytas nė vienas klasės prefiksas 5 klasei."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            ElseIf Not ValObj._AccountClassPrefix61 > 0 Then
                e.Description = "Klaida. Nenurodytas nė vienas klasės prefiksas 6 klasei."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            l.Add(valobj._AccountClassPrefix11)
            l.Add(valobj._AccountClassPrefix12)
            l.Add(valobj._AccountClassPrefix21)
            l.Add(valobj._AccountClassPrefix22)
            l.Add(valobj._AccountClassPrefix31)
            l.Add(valobj._AccountClassPrefix32)
            l.Add(valobj._AccountClassPrefix41)
            l.Add(valobj._AccountClassPrefix42)
            l.Add(valobj._AccountClassPrefix51)
            l.Add(valobj._AccountClassPrefix52)
            l.Add(valobj._AccountClassPrefix61)
            l.Add(valobj._AccountClassPrefix62)

            Dim i, j As Integer
            For i = l.Count To 1 Step -1
                If l(i - 1) = 0 Then l.RemoveAt(i - 1)
            Next

            For i = 1 To l.Count - 1
                For j = i + 1 To l.Count
                    If l(i - 1) = l(j - 1) Then
                        e.Description = "Klaida. Pasikartoja klasės prefiksas " & l(i - 1).ToString & "."
                        e.Severity = Validation.RuleSeverity.Error
                        Return False
                    End If
                Next
            Next

            Return True
        End Function

        ''' <summary>
        ''' Rule ensuring that the value of property DefaultTaxNpdFormula is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function NpdFormulaValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As Company = DirectCast(target, Company)

            If String.IsNullOrEmpty(ValObj._DefaultTaxNpdFormula.Trim) Then
                e.Description = "Nenurodyta NPD apskaičiavimo formulė."
                e.Severity = Validation.RuleSeverity.Warning
                Return False
            End If

            Dim FS As New FormulaSolver
            FS.Param("NPD") = 470
            FS.Param("PAJ") = 800
            If Not FS.Solved(ValObj._DefaultTaxNpdFormula.Trim, New Double) Then
                e.Description = "Klaidingai įvesta NPD formulė. Matematinės išraiškos klaida."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            Return True
        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
            AuthorizationRules.AllowWrite("General.Company3")
        End Sub

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.Company1")
        End Function

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole(Name_AdminRole)
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.Company3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole(Name_AdminRole)
        End Function

#End Region

#Region " Factory Methods "

        Public Shared Function NewCompany() As Company
            If Not CanAddObject() Then Throw New SecurityException( _
                "Klaida. Jūsų teisių nepakanka naujos įmonės duomenų bazei sukurti.")
            Dim result As New Company
            result._Accounts = CompanyAccountList.NewCompanyAccountList
            result._DefaultTaxRates = CompanyRateList.NewCompanyRateList
            result.ValidationRules.CheckRules()
            Return result
        End Function

        Public Shared Function GetCompany() As Company
            Return DataPortal.Fetch(Of Company)(New Criteria())
        End Function

        Private Sub New()
            ' require use of factory methods
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public Sub New()
            End Sub
        End Class

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanGetObject() Then Throw New SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims gauti.")

            Dim myComm As New SQLCommand("CompanyFetch")

            Using myData As DataTable = myComm.Fetch

                If Not myData.Rows.Count > 0 Then Throw New Exception( _
                    "Klaida. Nerasti bendri įmonės duomenys.)")

                Dim dr As DataRow = myData.Rows(0)

                _Code = CStrSafe(dr.Item(0)).Trim
                _Name = CStrSafe(dr.Item(1)).Trim
                _CodeVat = CStrSafe(dr.Item(2)).Trim
                _Email = CStrSafe(dr.Item(3)).Trim
                _HeadPerson = CStrSafe(dr.Item(4)).Trim
                _CodeSODRA = CStrSafe(dr.Item(5)).Trim
                _HeadPersonSignature = CByteArraySafe(dr.Item(6), 0)
                _NumbersInInvoice = CIntSafe(dr.Item(7), 0)
                _AddDateToInvoiceNumber = ConvertDbBoolean(CIntSafe(dr.Item(8), 0))
                _DefaultInvoiceMadeContent = CStrSafe(dr.Item(9)).Trim
                _DefaultInvoiceReceivedContent = CStrSafe(dr.Item(10)).Trim
                _AccountClassPrefix11 = CIntSafe(dr.Item(11), 0)
                _AccountClassPrefix12 = CIntSafe(dr.Item(12), 0)
                _AccountClassPrefix21 = CIntSafe(dr.Item(13), 0)
                _AccountClassPrefix22 = CIntSafe(dr.Item(14), 0)
                _AccountClassPrefix31 = CIntSafe(dr.Item(15), 0)
                _AccountClassPrefix32 = CIntSafe(dr.Item(16), 0)
                _AccountClassPrefix41 = CIntSafe(dr.Item(17), 0)
                _AccountClassPrefix42 = CIntSafe(dr.Item(18), 0)
                _AccountClassPrefix51 = CIntSafe(dr.Item(19), 0)
                _AccountClassPrefix52 = CIntSafe(dr.Item(20), 0)
                _AccountClassPrefix61 = CIntSafe(dr.Item(21), 0)
                _AccountClassPrefix62 = CIntSafe(dr.Item(22), 0)
                _DefaultTaxNpdFormula = CStrSafe(dr.Item(23)).Trim
                _MeasureUnitInvoiceReceived = CStrSafe(dr.Item(24)).Trim
                _AddDateToTillIncomeOrderNumber = ConvertDbBoolean(CIntSafe(dr.Item(25), 0))
                _AddDateToTillSpendingsOrderNumber = ConvertDbBoolean(CIntSafe(dr.Item(26), 0))
                _BaseCurrency = CStrSafe(dr.Item(27)).Trim
                If String.IsNullOrEmpty(_BaseCurrency.Trim) Then _BaseCurrency = "LTL"
                _Accountant = CStrSafe(dr.Item(28)).Trim
                _Cashier = CStrSafe(dr.Item(29)).Trim

                _CompanyDatabaseName = GetCurrentIdentity.Database

            End Using

            _Accounts = CompanyAccountList.GetCompanyAccountList
            _DefaultTaxRates = CompanyRateList.GetCompanyRateList

            ValidationRules.CheckRules()

            MarkOld()

        End Sub

        Protected Overrides Sub DataPortal_Insert()
            If Not CanAddObject() Then Throw New SecurityException( _
                "Klaida. Jūsų teisių nepakanka naujiems duomenims įvesti.")
            CreateCompany(_Name, AddressOf CustomCreateDatabaseMethod)
            MarkOld()
        End Sub

        Private Sub CustomCreateDatabaseMethod(ByVal NewDatabaseName As String)

            Dim myComm As New SQLCommand("CompanyInsert")
            AddWithParams(myComm)
            myComm.AddParam("?BB", "")
            myComm.AddParam("?BC", "")
            myComm.AddParam("?BD", "")
            myComm.AddParam("?BE", "")
            myComm.AddParam("?BF", "")
            myComm.AddParam("?BG", "")
            myComm.AddParam("?BH", "")
            myComm.AddParam("?BI", "")
            myComm.AddParam("?BK", "")
            myComm.AddParam("?BL", "")
            myComm.AddParam("?BO", _BaseCurrency)

            DatabaseAccess.TransactionBegin()

            Try
                myComm.Execute()
            Catch ex As Exception
                Throw New Exception("KRITINĖ KLAIDA. Nepavyko įrašyti įmonės " _
                    & "duomenų į sukurtą duomenų bazę.", ex)
            End Try

            DatabaseAccess.TransactionCommit()

            _CompanyDatabaseName = NewDatabaseName

        End Sub

        Protected Overrides Sub DataPortal_Update()

            If Not CanEditObject() Then Throw New SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims pakeisti.")

            Dim myComm As New SQLCommand("CompanyUpdate")
            AddWithParams(myComm)

            DatabaseAccess.TransactionBegin()

            myComm.Execute()

            _DefaultTaxRates.Update(Me)
            _Accounts.Update(Me)

            DatabaseAccess.TransactionCommit()

            ' Company identification info is part of CompanyInfo object in GlobalContext
            ApskaitaObjects.Settings.CompanyInfo.LoadCompanyInfoToGlobalContext("", "")

        End Sub

        Private Sub AddWithParams(ByRef myComm As SQLCommand)

            myComm.AddParam("?AA", _Code.Trim)
            myComm.AddParam("?AB", _Name.Trim)
            myComm.AddParam("?AC", _CodeVat.Trim)
            myComm.AddParam("?AD", _Email.Trim)
            myComm.AddParam("?AE", _HeadPerson.Trim)
            myComm.AddParam("?AF", _CodeSODRA.Trim)
            myComm.AddParam("?AG", _HeadPersonSignature, GetType(Byte()))
            myComm.AddParam("?AH", _NumbersInInvoice)
            myComm.AddParam("?AI", ConvertDbBoolean(_AddDateToInvoiceNumber))
            myComm.AddParam("?AJ", _DefaultInvoiceMadeContent.Trim)
            myComm.AddParam("?AK", _DefaultInvoiceReceivedContent.Trim)
            myComm.AddParam("?AL", _AccountClassPrefix11)
            myComm.AddParam("?AM", _AccountClassPrefix12)
            myComm.AddParam("?AN", _AccountClassPrefix21)
            myComm.AddParam("?AO", _AccountClassPrefix22)
            myComm.AddParam("?AQ", _AccountClassPrefix31)
            myComm.AddParam("?AP", _AccountClassPrefix32)
            myComm.AddParam("?AR", _AccountClassPrefix41)
            myComm.AddParam("?AT", _AccountClassPrefix42)
            myComm.AddParam("?AU", _AccountClassPrefix51)
            myComm.AddParam("?AV", _AccountClassPrefix52)
            myComm.AddParam("?AZ", _AccountClassPrefix61)
            myComm.AddParam("?AW", _AccountClassPrefix62)
            myComm.AddParam("?BA", _DefaultTaxNpdFormula.Trim)
            myComm.AddParam("?BJ", _MeasureUnitInvoiceReceived.Trim)
            myComm.AddParam("?BM", ConvertDbBoolean(_AddDateToTillIncomeOrderNumber))
            myComm.AddParam("?BN", ConvertDbBoolean(_AddDateToTillSpendingsOrderNumber))
            myComm.AddParam("?BQ", _Accountant.Trim)
            myComm.AddParam("?BP", _Cashier.Trim)

        End Sub

#End Region

    End Class

End Namespace