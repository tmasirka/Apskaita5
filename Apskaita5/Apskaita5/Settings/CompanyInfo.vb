Imports ApskaitaObjects.General
Imports ApskaitaObjects.Documents

Namespace Settings

    ''' <summary>
    ''' Represents general current company's data, that is accessible for read for all objects 
    ''' both on server side and on user side. Is accessed by the method <see cref="GetCurrentCompany">GetCurrentCompany</see>.
    ''' </summary>
    ''' <remarks>Object is stored in the ApplicationContext.GlobalContext.</remarks>
    <Serializable()> _
    Public NotInheritable Class CompanyInfo
        Inherits Csla.ReadOnlyBase(Of CompanyInfo)

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
        Private _Address As String = ""
        Private _BankAccount As String = ""
        Private _Bank As String = ""

        Private _NumbersInInvoice As Integer = 0
        Private _AddDateToInvoiceNumber As Boolean = False
        Private _AddDateToTillIncomeOrderNumber As Boolean = False
        Private _AddDateToTillSpendingsOrderNumber As Boolean = False
        Private _UseVatDeclarationSchemas As Boolean = False
        Private _DeclarationSchemaSales As VatDeclarationSchemaInfo = Nothing
        Private _DeclarationSchemaPurchase As VatDeclarationSchemaInfo = Nothing
        Private _MainEconomicActivityCode As String = ""
        Private _VatDeductionPercentage As Double = 100
        Private _DefaultInvoiceMadeContent As String = ""
        Private _DefaultInvoiceReceivedContent As String = ""
        Private _MeasureUnitInvoiceMade As String = ""
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

        Private _DefaultTaxNpdFormula As String = ""

        Private _Accounts As CompanyAccountInfoList
        Private _Rates As CompanyRateInfoList

        Private _LastClosingDate As Date = Date.MinValue


        ''' <summary>
        ''' Represents a currency (code) that is used as a base for accounting.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.BaseCurrency.</remarks>
        Public ReadOnly Property BaseCurrency() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _BaseCurrency.Trim.ToUpper
            End Get
        End Property

        ''' <summary>
        ''' Company's registration code.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.I_Kodas.</remarks>
        Public ReadOnly Property Code() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Code.Trim
            End Get
        End Property

        ''' <summary>
        ''' Company's official name.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.I_Pavadinimas.</remarks>
        Public ReadOnly Property Name() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Name.Trim
            End Get
        End Property

        ''' <summary>
        ''' Company's VAT registration code.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.I_PVM_kodas.</remarks>
        Public ReadOnly Property CodeVat() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CodeVat.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a company address in base language.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.I_Adresas.</remarks>
        Public ReadOnly Property Address() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Address.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a company's bank account for base language.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.I_Banko_sask.</remarks>
        Public ReadOnly Property BankAccount() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _BankAccount.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a company's bank name for base language.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.I_Bankas.</remarks>
        Public ReadOnly Property Bank() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Bank.Trim
            End Get
        End Property

        ''' <summary>
        ''' An email address of a company.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.I_Mail.</remarks>
        Public ReadOnly Property Email() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Email.Trim
            End Get
        End Property

        ''' <summary>
        ''' Company's head person's name, surname.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.I_Vadas.</remarks>
        Public ReadOnly Property HeadPerson() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _HeadPerson.Trim
            End Get
        End Property

        ''' <summary>
        ''' Company's (chief) accountant's name, surname.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.Accountant.</remarks>
        Public ReadOnly Property Accountant() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Accountant.Trim
            End Get
        End Property

        ''' <summary>
        ''' Company's cashier's name, surname.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.Cashier.</remarks>
        Public ReadOnly Property Cashier() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Cashier.Trim
            End Get
        End Property

        ''' <summary>
        ''' Company's social security code.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.I_SD.</remarks>
        Public ReadOnly Property CodeSODRA() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CodeSODRA.Trim
            End Get
        End Property

        ''' <summary>
        ''' How many numeric positions are displayed for invoice number, e.g. use 5 for "00001". 
        ''' Use 0 for not enforcing fixed number.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.NumbersInInvoice.</remarks>
        Public ReadOnly Property NumbersInInvoice() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _NumbersInInvoice
            End Get
        End Property

        ''' <summary>
        ''' Wheather to display an invoice date as part of an invoice number.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>If true, overrides <see cref="NumbersInInvoice">NumbersInInvoice</see>.
        ''' Value is stored in the database field imone.AddDateToInvoiceNumber.</remarks>
        Public ReadOnly Property AddDateToInvoiceNumber() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AddDateToInvoiceNumber
            End Get
        End Property

        ''' <summary>
        ''' Wheather to display a TillIncomeOrder date as part of a TillIncomeOrder number.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.AddDateToTillIncomeOrderNumber.</remarks>
        Public ReadOnly Property AddDateToTillIncomeOrderNumber() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AddDateToTillIncomeOrderNumber
            End Get
        End Property

        ''' <summary>
        ''' Wheather to display a TillSpendingsOrder date as part of a TillSpendingsOrder number.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.AddDateToTillSpendingsOrderNumber.</remarks>
        Public ReadOnly Property AddDateToTillSpendingsOrderNumber() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AddDateToTillSpendingsOrderNumber
            End Get
        End Property

        ''' <summary>
        ''' Wheather to use <see cref="Documents.VatDeclarationSchema">VAT declaration schemas</see>
        ''' in invoices and other VAT documents.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.UseVatDeclarationSchemas.</remarks>
        Public ReadOnly Property UseVatDeclarationSchemas() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UseVatDeclarationSchemas
            End Get
        End Property

        ''' <summary>
        ''' Gets a default VAT declaration schema for the goods/services sold.
        ''' </summary>
        ''' <remarks>Value is stored in the database table paslaugos.DeclarationSchemaIDSales.</remarks>
        <VatDeclarationSchemaField(ValueRequiredLevel.Optional, TradedItemType.Sales)> _
        Public ReadOnly Property DeclarationSchemaSales() As VatDeclarationSchemaInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DeclarationSchemaSales
            End Get
        End Property

        ''' <summary>
        ''' Gets a default VAT declaration schema for the goods/services bought.
        ''' </summary>
        ''' <remarks>Value is stored in the database table paslaugos.DeclarationSchemaIDPurchase.</remarks>
        <VatDeclarationSchemaField(ValueRequiredLevel.Optional, TradedItemType.All)> _
        Public ReadOnly Property DeclarationSchemaPurchase() As VatDeclarationSchemaInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DeclarationSchemaPurchase
            End Get
        End Property

        ''' <summary>
        ''' Gets a main company's economic activity code (EVRK).
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.MainActivityCode.</remarks>
        <StringField(ValueRequiredLevel.Optional, 20, False)> _
        Public ReadOnly Property MainEconomicActivityCode() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _MainEconomicActivityCode Is Nothing Then Return ""
                Return _MainEconomicActivityCode.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a company's VAT deduction percentage.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.VatDeductionPercentage.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, 2, True, 0.0, 100.0)> _
        Public ReadOnly Property VatDeductionPercentage() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_VatDeductionPercentage)
            End Get
        End Property

        ''' <summary>
        ''' Default text that is set for a new <see cref="Documents.InvoiceMade.Content">InvoiceMade.Content</see>.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.DefaultInvoiceMadeContent.</remarks>
        Public ReadOnly Property DefaultInvoiceMadeContent() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DefaultInvoiceMadeContent.Trim
            End Get
        End Property

        ''' <summary>
        ''' Default text that is set for a new <see cref="Documents.InvoiceReceived.Content">InvoiceReceived.Content</see>.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.DefaultInvoiceReceivedContent.</remarks>
        Public ReadOnly Property DefaultInvoiceReceivedContent() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DefaultInvoiceReceivedContent.Trim
            End Get
        End Property

        ''' <summary>
        ''' Default text that is set for a new <see cref="Documents.InvoiceReceivedItem.MeasureUnit">InvoiceReceivedItem.MeasureUnit</see>.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.MeasureUnitInvoiceReceived.</remarks>
        Public ReadOnly Property MeasureUnitInvoiceReceived() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _MeasureUnitInvoiceReceived.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a default measure unit name for invoices made in base language.
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.MeasureUnitInvoiceMade.</remarks>
        Public ReadOnly Property MeasureUnitInvoiceMade() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _MeasureUnitInvoiceMade.Trim
            End Get
        End Property


        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the first class in traditional clasification (long term assets).
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.AccountClassPrefix11.</remarks>
        Public ReadOnly Property AccountClassPrefix11() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix11
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the first class in traditional clasification (long term assets).
        ''' </summary>
        ''' <remarks>Used if there are more than one prefix denoting the class, zero otherwise.
        ''' Value is stored in the database field imone.AccountClassPrefix12.</remarks>
        Public ReadOnly Property AccountClassPrefix12() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix12
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the second class in traditional clasification (short term assets).
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.AccountClassPrefix21.</remarks>
        Public ReadOnly Property AccountClassPrefix21() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix21
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the second class in traditional clasification (short term assets).
        ''' </summary>
        ''' <remarks>Used if there are more than one prefix denoting the class, zero otherwise.
        ''' Value is stored in the database field imone.AccountClassPrefix22.</remarks>
        Public ReadOnly Property AccountClassPrefix22() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix22
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the third class in traditional clasification (equity).
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.AccountClassPrefix31.</remarks>
        Public ReadOnly Property AccountClassPrefix31() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix31
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the third class in traditional clasification (equity).
        ''' </summary>
        ''' <remarks>Used if there are more than one prefix denoting the class, zero otherwise.
        ''' Value is stored in the database field imone.AccountClassPrefix32.</remarks>
        Public ReadOnly Property AccountClassPrefix32() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix32
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the fourth class in traditional clasification (liabilities).
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.AccountClassPrefix41.</remarks>
        Public ReadOnly Property AccountClassPrefix41() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix41
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the fourth class in traditional clasification (liabilities).
        ''' </summary>
        ''' <remarks>Used if there are more than one prefix denoting the class, zero otherwise.
        ''' Value is stored in the database field imone.AccountClassPrefix42.</remarks>
        Public ReadOnly Property AccountClassPrefix42() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix42
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the fifth class in traditional clasification (revenues).
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.AccountClassPrefix51.</remarks>
        Public ReadOnly Property AccountClassPrefix51() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix51
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the fifth class in traditional clasification (revenues).
        ''' </summary>
        ''' <remarks>Used if there are more than one prefix denoting the class, zero otherwise.
        ''' Value is stored in the database field imone.AccountClassPrefix52.</remarks>
        Public ReadOnly Property AccountClassPrefix52() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix52
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the sixth class in traditional clasification (expenses).
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.AccountClassPrefix61.</remarks>
        Public ReadOnly Property AccountClassPrefix61() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix61
            End Get
        End Property

        ''' <summary>
        ''' First number in an <see cref="Account.ID">account mumber</see> denoting 
        ''' that the account corresponds to the sixth class in traditional clasification (expenses).
        ''' </summary>
        ''' <remarks>Used if there are more than one prefix denoting the class, zero otherwise.
        ''' Value is stored in the database field imone.AccountClassPrefix62.</remarks>
        Public ReadOnly Property AccountClassPrefix62() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountClassPrefix62
            End Get
        End Property


        ''' <summary>
        ''' A list of <see cref="CompanyAccountList">default accounts</see> by type.
        ''' </summary>
        ''' <remarks>Values are stored in the database table companyaccounts.</remarks>
        Public ReadOnly Property Accounts() As CompanyAccountInfoList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Accounts
            End Get
        End Property

        ''' <summary>
        ''' A list of <see cref="CompanyRateList">default rates</see> (tax, wage, etc.) by type.
        ''' </summary>
        ''' <remarks>Values are stored in the database table companyrates.</remarks>
        Public ReadOnly Property Rates() As CompanyRateInfoList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Rates
            End Get
        End Property


        ''' <summary>
        ''' A formula used to calculate a (minimum) not-taxable personal income (NPD). 
        ''' </summary>
        ''' <remarks>Value is stored in the database field imone.DefaultTaxNpdFormula.</remarks>
        Public ReadOnly Property DefaultTaxNpdFormula() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DefaultTaxNpdFormula.Trim
            End Get
        End Property


        ''' <summary>
        ''' Gets a date when accounting info was transfered to the program or 5/6 classes were last closed.
        ''' </summary>
        Public ReadOnly Property LastClosingDate() As Date
            Get
                Return _LastClosingDate
            End Get
        End Property


        ''' <summary>
        ''' Gets a default <see cref="General.Account.ID">account</see> of the specified type.
        ''' Returns 0 if the requested type is not present in the list.
        ''' </summary>
        ''' <param name="accountType">Type of the default account to get.</param>
        ''' <remarks></remarks>
        Public Function GetDefaultAccount(ByVal accountType As DefaultAccountType) As Long
            For Each c As CompanyAccountInfo In _Accounts
                If c.Type = accountType Then Return c.Value
            Next
            Return 0
        End Function

        ''' <summary>
        ''' Gets a default rate of the requested type.
        ''' Returnes 0 if the requested type is not available in the list.
        ''' </summary>
        ''' <param name="requiredRateType">Type of the default rate to get.</param>
        ''' <remarks></remarks>
        Public Function GetDefaultRate(ByVal requiredRateType As DefaultRateType) As Double
            For Each c As CompanyRateInfo In _Rates
                If c.Type = requiredRateType Then Return c.Value
            Next
            Return 0
        End Function


        ''' <summary>
        ''' Returns TRUE if current settings contain enough info to create WageSheet.
        ''' </summary>
        Public Function IsSettingsReadyForWageSheet(ByRef message As String) As Boolean

            message = ""

            If _Rates.GetRate(DefaultRateType.WageRateDeviations) < 100 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_InvalidRate, _
                Utilities.ConvertLocalizedName(DefaultRateType.WageRateDeviations), 100.ToString()), False)

            If _Rates.GetRate(DefaultRateType.WageRateNight) < 150 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_InvalidRate, _
                Utilities.ConvertLocalizedName(DefaultRateType.WageRateNight), 150.ToString()), False)
            If _Rates.GetRate(DefaultRateType.WageRateOvertime) < 150 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_InvalidRate, _
                Utilities.ConvertLocalizedName(DefaultRateType.WageRateOvertime), 150.ToString()), False)
            If _Rates.GetRate(DefaultRateType.WageRatePublicHolidays) < 200 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_InvalidRate, _
                Utilities.ConvertLocalizedName(DefaultRateType.WageRatePublicHolidays), 200.ToString()), False)
            If _Rates.GetRate(DefaultRateType.WageRateRestTime) < 200 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_InvalidRate, _
                Utilities.ConvertLocalizedName(DefaultRateType.WageRateRestTime), 200.ToString()), False)
            If Not _Rates.GetRate(DefaultRateType.WageRateSickLeave) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultRateNull, _
                Utilities.ConvertLocalizedName(DefaultRateType.WageRateSickLeave)), False)
            If Not _Rates.GetRate(DefaultRateType.GpmWage) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultRateNull, _
                Utilities.ConvertLocalizedName(DefaultRateType.WageRateSickLeave)), False)
            If Not _Rates.GetRate(DefaultRateType.PsdEmployee) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultRateNull, _
                Utilities.ConvertLocalizedName(DefaultRateType.PsdEmployee)), False)
            If Not _Rates.GetRate(DefaultRateType.PsdEmployer) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultRateNull, _
                Utilities.ConvertLocalizedName(DefaultRateType.PsdEmployer)), False)
            If Not _Rates.GetRate(DefaultRateType.SodraEmployee) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultRateNull, _
                Utilities.ConvertLocalizedName(DefaultRateType.SodraEmployee)), False)
            If Not _Rates.GetRate(DefaultRateType.SodraEmployer) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultRateNull, _
                Utilities.ConvertLocalizedName(DefaultRateType.SodraEmployer)), False)

            If Not _Accounts.GetAccount(DefaultAccountType.WageGpmPayable) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultAccountNull, _
                Utilities.ConvertLocalizedName(DefaultAccountType.WageGpmPayable)), False)
            If Not _Accounts.GetAccount(DefaultAccountType.WageGuaranteeFundPayable) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultAccountNull, _
                Utilities.ConvertLocalizedName(DefaultAccountType.WageGuaranteeFundPayable)), False)
            If Not _Accounts.GetAccount(DefaultAccountType.WageImprestPayable) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultAccountNull, _
                Utilities.ConvertLocalizedName(DefaultAccountType.WageImprestPayable)), False)
            If Not _Accounts.GetAccount(DefaultAccountType.WagePayable) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultAccountNull, _
                Utilities.ConvertLocalizedName(DefaultAccountType.WagePayable)), False)
            If Not _Accounts.GetAccount(DefaultAccountType.WagePsdPayable) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultAccountNull, _
                Utilities.ConvertLocalizedName(DefaultAccountType.WagePsdPayable)), False)
            If Not _Accounts.GetAccount(DefaultAccountType.WageSodraPayable) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultAccountNull, _
                Utilities.ConvertLocalizedName(DefaultAccountType.WageSodraPayable)), False)
            If Not _Accounts.GetAccount(DefaultAccountType.WageWithdraw) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultAccountNull, _
                Utilities.ConvertLocalizedName(DefaultAccountType.WageWithdraw)), False)

            If StringIsNullOrEmpty(Me._DefaultTaxNpdFormula) Then message = AddWithNewLine(message, _
                My.Resources.Settings_CompanyInfo_NpdFormulaNull, False)

            Return String.IsNullOrEmpty(message.Trim)

        End Function

        ''' <summary>
        ''' Returns TRUE if current settings contain enough info to create ImprestSheet.
        ''' </summary>
        Public Function IsSettingsReadyForImprestSheet(ByRef message As String) As Boolean

            message = ""

            If Not _Accounts.GetAccount(DefaultAccountType.WageImprestPayable) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultAccountNull, _
                Utilities.ConvertLocalizedName(DefaultAccountType.WageImprestPayable)), False)
            If Not _Accounts.GetAccount(DefaultAccountType.WagePayable) > 0 Then message = AddWithNewLine(message, _
                String.Format(My.Resources.Settings_CompanyInfo_DefaultAccountNull, _
                Utilities.ConvertLocalizedName(DefaultAccountType.WagePayable)), False)

            Return String.IsNullOrEmpty(message.Trim)

        End Function



        Protected Overrides Function GetIdValue() As Object
            Return _Code
        End Function

        ''' <summary>
        ''' Returns short info about the company in format "Company name, [code]".
        ''' </summary>
        Public Overrides Function ToString() As String
            Return String.Format(My.Resources.Settings_CompanyInfo_ToString, _Name, _Code)
        End Function

#End Region

#Region " Factory Methods "

        Private Sub New(ByVal databaseName As String, ByVal password As String)
            Fetch(databaseName, password)
        End Sub

        Friend Shared Sub LoadCompanyInfoToGlobalContext(ByVal databaseName As String, _
            ByVal password As String)

            Dim newCompanyInfo As New CompanyInfo(databaseName, password)
            If ApplicationContext.GlobalContext.Contains(KEY_COMPANY_INFO) Then _
                ApplicationContext.GlobalContext.Remove(KEY_COMPANY_INFO)
            ApplicationContext.GlobalContext.Add(KEY_COMPANY_INFO, newCompanyInfo)

        End Sub

#End Region

#Region " Data Access "

        Private Sub Fetch(ByVal databaseName As String, ByVal password As String)
            FetchCompanyInfo(databaseName, password, AddressOf CustomFetchMethod)
        End Sub

        Private Sub CustomFetchMethod()

            Dim myComm As New SQLCommand("FetchCompanyInfo")

            Using myData As DataTable = myComm.Fetch

                If Not myData.Rows.Count > 0 Then
                    Throw New Exception(String.Format(My.Resources.Common_ObjectNotFound, _
                        My.Resources.Settings_CompanyInfo_TypeName, "N/A"))
                End If

                Dim dr As DataRow = myData.Rows(0)

                _Code = CStrSafe(dr.Item(0)).Trim
                _Name = CStrSafe(dr.Item(1)).Trim
                _CodeVat = CStrSafe(dr.Item(2)).Trim
                _Email = CStrSafe(dr.Item(3)).Trim
                _HeadPerson = CStrSafe(dr.Item(4)).Trim
                _CodeSODRA = CStrSafe(dr.Item(5)).Trim
                _NumbersInInvoice = CIntSafe(dr.Item(6), 0)
                _AddDateToInvoiceNumber = ConvertDbBoolean(CIntSafe(dr.Item(7), 0))
                _DefaultInvoiceMadeContent = CStrSafe(dr.Item(8)).Trim
                _DefaultInvoiceReceivedContent = CStrSafe(dr.Item(9)).Trim
                _AccountClassPrefix11 = CIntSafe(dr.Item(10), 0)
                _AccountClassPrefix12 = CIntSafe(dr.Item(11), 0)
                _AccountClassPrefix21 = CIntSafe(dr.Item(12), 0)
                _AccountClassPrefix22 = CIntSafe(dr.Item(13), 0)
                _AccountClassPrefix31 = CIntSafe(dr.Item(14), 0)
                _AccountClassPrefix32 = CIntSafe(dr.Item(15), 0)
                _AccountClassPrefix41 = CIntSafe(dr.Item(16), 0)
                _AccountClassPrefix42 = CIntSafe(dr.Item(17), 0)
                _AccountClassPrefix51 = CIntSafe(dr.Item(18), 0)
                _AccountClassPrefix52 = CIntSafe(dr.Item(19), 0)
                _AccountClassPrefix61 = CIntSafe(dr.Item(20), 0)
                _AccountClassPrefix62 = CIntSafe(dr.Item(21), 0)
                _DefaultTaxNpdFormula = CStrSafe(dr.Item(22)).Trim
                _MeasureUnitInvoiceReceived = CStrSafe(dr.Item(23)).Trim
                _MeasureUnitInvoiceMade = CStrSafe(dr.Item(24)).Trim
                _Address = CStrSafe(dr.Item(25)).Trim
                _Bank = CStrSafe(dr.Item(26)).Trim
                _BankAccount = CStrSafe(dr.Item(27)).Trim
                _AddDateToTillIncomeOrderNumber = ConvertDbBoolean(CIntSafe(dr.Item(28), 0))
                _AddDateToTillSpendingsOrderNumber = ConvertDbBoolean(CIntSafe(dr.Item(29), 0))
                _BaseCurrency = CStrSafe(dr.Item(30)).Trim
                If String.IsNullOrEmpty(_BaseCurrency.Trim) Then _BaseCurrency = "LTL"
                _Accountant = CStrSafe(dr.Item(31)).Trim
                _Cashier = CStrSafe(dr.Item(32)).Trim
                _UseVatDeclarationSchemas = ConvertDbBoolean(CIntSafe(dr.Item(33), 0))
                _LastClosingDate = CDateSafe(dr.Item(34), Date.MinValue)
                _MainEconomicActivityCode = CStrSafe(dr.Item(35)).Trim
                _VatDeductionPercentage = CDblSafe(dr.Item(36), 2, 100.0)
                _DeclarationSchemaSales = VatDeclarationSchemaInfo.GetVatDeclarationSchemaInfo(dr, 37)
                _DeclarationSchemaPurchase = VatDeclarationSchemaInfo.GetVatDeclarationSchemaInfo(dr, 44)

            End Using

            _Accounts = CompanyAccountInfoList.GetCompanyAccountInfoList
            _Rates = CompanyRateInfoList.GetCompanyRateInfoList

        End Sub

#End Region

    End Class

End Namespace
