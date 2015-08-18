Imports ApskaitaObjects.HelperLists
Imports ApskaitaObjects.General

Namespace Documents

    <Serializable()> _
    Public Class InvoiceReceivedItem
        Inherits BusinessBase(Of InvoiceReceivedItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private _Guid As Guid = Guid.NewGuid
        Private _ID As Integer = -1
        Private _FinancialDataCanChange As Boolean = True
        Private _NameInvoice As String = ""
        Private _MeasureUnit As String = ""
        Private _AccountCosts As Long = 0
        Private _AccountVat As Long = 0
        Private _Ammount As Double = 0
        Private _UnitValueLTL As Double = 0
        Private _UnitValueLTLCorrection As Integer = 0
        Private _SumLTL As Double = 0
        Private _SumLTLCorrection As Integer = 0
        Private _VatRate As Double = 0
        Private _SumVatLTL As Double = 0
        Private _SumVatLTLCorrection As Integer = 0
        Private _SumTotalLTL As Double = 0
        Private _UnitValue As Double = 0
        Private _Sum As Double = 0
        Private _SumCorrection As Integer = 0
        Private _SumVat As Double = 0
        Private _SumVatCorrection As Integer = 0
        Private _SumTotal As Double = 0
        Private _IncludeVatInObject As Boolean = False
        Private _AttachedObject As InvoiceAttachedObject = Nothing


        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property FinancialDataCanChange() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If Not _AttachedObject Is Nothing AndAlso Not _AttachedObject. _
                    ChronologyValidator.FinancialDataCanChange Then Return False
                Return _FinancialDataCanChange
            End Get
        End Property

        Public Property NameInvoice() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _NameInvoice.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _NameInvoice.Trim <> value.Trim Then
                    _NameInvoice = value.Trim
                    PropertyHasChanged()
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public Property MeasureUnit() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _MeasureUnit.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _MeasureUnit.Trim <> value.Trim Then
                    _MeasureUnit = value.Trim
                    PropertyHasChanged()
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public Property AccountCosts() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountCosts
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If AccountCostsIsReadOnly Then Exit Property
                If _AccountCosts <> value Then
                    _AccountCosts = value
                    PropertyHasChanged()
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public Property AccountVat() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountVat
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If AccountVatIsReadOnly Then Exit Property
                If _AccountVat <> value Then
                    _AccountVat = value
                    PropertyHasChanged()
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public Property Ammount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If AmmountIsReadOnly Then Exit Property
                If CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED) <> CRound(value, ROUNDAMOUNTINVOICERECEIVED) Then
                    _Ammount = CRound(value, ROUNDAMOUNTINVOICERECEIVED)
                    PropertyHasChanged()
                    CalculateSum(0)
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public Property UnitValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitValue, ROUNDUNITINVOICERECEIVED)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If UnitValueIsReadOnly Then Exit Property
                If CRound(_UnitValue, ROUNDUNITINVOICERECEIVED) <> CRound(value, ROUNDUNITINVOICERECEIVED) Then
                    _UnitValue = CRound(value, ROUNDUNITINVOICERECEIVED)
                    CalculateSum(0)
                    PropertyHasChanged()
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public ReadOnly Property Sum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Sum)
            End Get
        End Property

        Public Property SumCorrection() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _SumCorrection
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If SumCorrectionIsReadOnly Then Exit Property
                If _SumCorrection <> value Then
                    _SumCorrection = value
                    PropertyHasChanged()
                    CalculateSum(0)
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public Property VatRate() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_VatRate)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If VatRateIsReadOnly Then Exit Property
                If CRound(_VatRate) <> CRound(value) Then
                    _VatRate = CRound(value)
                    PropertyHasChanged()
                    CalculateSumVat(0)
                    CalculateSumVatLTL(0)
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public ReadOnly Property SumVat() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumVat)
            End Get
        End Property

        Public Property SumVatCorrection() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _SumVatCorrection
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If SumVatCorrectionIsReadOnly Then Exit Property
                If _SumVatCorrection <> value Then
                    _SumVatCorrection = value
                    PropertyHasChanged()
                    CalculateSumVat(0)
                    CalculateSumVatLTL(0)
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public ReadOnly Property SumTotal() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumTotal)
            End Get
        End Property

        Public ReadOnly Property UnitValueLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitValueLTL, ROUNDUNITINVOICERECEIVED)
            End Get
        End Property

        Public Property UnitValueLTLCorrection() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UnitValueLTLCorrection
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If UnitValueLTLCorrectionIsReadOnly Then Exit Property
                If _UnitValueLTLCorrection <> value Then
                    _UnitValueLTLCorrection = value
                    PropertyHasChanged()
                    CalculateSumLTL(0)
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public ReadOnly Property SumLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumLTL)
            End Get
        End Property

        Public Property SumLTLCorrection() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _SumLTLCorrection
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If SumLTLCorrectionIsReadOnly Then Exit Property
                If _SumLTLCorrection <> value Then
                    _SumLTLCorrection = value
                    PropertyHasChanged()
                    CalculateSumLTL(0)
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public ReadOnly Property SumVatLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumVatLTL)
            End Get
        End Property

        Public Property SumVatLTLCorrection() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _SumVatLTLCorrection
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If SumVatLTLCorrectionIsReadOnly Then Exit Property
                If _SumVatLTLCorrection <> value Then
                    _SumVatLTLCorrection = value
                    PropertyHasChanged()
                    CalculateSumVatLTL(0)
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public ReadOnly Property SumTotalLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumTotalLTL)
            End Get
        End Property

        Public Property IncludeVatInObject() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IncludeVatInObject
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If IncludeVatInObjectIsReadOnly Then Exit Property
                If _IncludeVatInObject <> value Then
                    _IncludeVatInObject = value
                    PropertyHasChanged()
                    SetAttachedObjectData()
                End If
            End Set
        End Property

        Public ReadOnly Property AttachedObject() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _AttachedObject Is Nothing Then Return ""
                Return _AttachedObject.ToString
            End Get
        End Property

        Public ReadOnly Property AttachedObjectValue() As InvoiceAttachedObject
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AttachedObject
            End Get
        End Property


        Public ReadOnly Property AccountCostsIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange OrElse (Not _AttachedObject Is Nothing _
                    AndAlso _AttachedObject.Type = InvoiceAttachedObjectType.LongTermAssetAcquisitionValueChange)
            End Get
        End Property

        Public ReadOnly Property AccountVatIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        Public ReadOnly Property AmmountIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        Public ReadOnly Property UnitValueIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        Public ReadOnly Property SumCorrectionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        Public ReadOnly Property VatRateIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        Public ReadOnly Property SumVatCorrectionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        Public ReadOnly Property UnitValueLTLCorrectionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        Public ReadOnly Property SumLTLCorrectionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        Public ReadOnly Property SumVatLTLCorrectionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        Public ReadOnly Property IncludeVatInObjectIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange OrElse _AttachedObject Is Nothing _
                    OrElse Not _AttachedObject.VatIsHandledOnRequest
            End Get
        End Property


        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse (Not _AttachedObject Is Nothing AndAlso _AttachedObject.IsDirty)
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return MyBase.IsValid AndAlso (_AttachedObject Is Nothing OrElse _AttachedObject.IsValid)
            End Get
        End Property


        Private Sub CalculateSumVat(ByVal pCurrencyRate As Double)
            _SumVat = CRound(CRound(_Sum * _VatRate / 100) + _SumVatCorrection / 100)
            _SumTotal = CRound(_Sum + _SumVat)
            PropertyHasChanged("SumVat")
            PropertyHasChanged("SumTotal")
        End Sub

        Friend Sub CalculateSum(ByVal pCurrencyRate As Double)
            _Sum = CRound(CRound(_UnitValue * _Ammount) + _SumCorrection / 100)
            PropertyHasChanged("Sum")
            CalculateSumVat(pCurrencyRate)
            CalculateSumLTL(pCurrencyRate)
        End Sub

        Private Sub CalculateSumLTL(ByVal pCurrencyRate As Double)

            _UnitValueLTL = CRound(CRound(_UnitValue * GetCurrencyRate(pCurrencyRate), ROUNDUNITINVOICERECEIVED) _
                + _UnitValueLTLCorrection / 100, ROUNDUNITINVOICERECEIVED)
            _UnitValueLTLCorrection = Convert.ToInt32(CRound(_UnitValueLTL _
                - CRound(_UnitValue * GetCurrencyRate(pCurrencyRate), ROUNDUNITINVOICERECEIVED)) * 100)
            _SumLTL = CRound(CRound(_Sum * GetCurrencyRate(pCurrencyRate)) + _SumLTLCorrection / 100)

            PropertyHasChanged("UnitValueLTL")
            PropertyHasChanged("SumLTL")

            CalculateSumVatLTL(pCurrencyRate)

        End Sub

        Private Sub CalculateSumVatLTL(ByVal pCurrencyRate As Double)
            _SumVatLTL = CRound(CRound(_SumVat * GetCurrencyRate(pCurrencyRate)) + _SumVatLTLCorrection / 100)
            _SumTotalLTL = CRound(_SumLTL + _SumVatLTL)
            PropertyHasChanged("SumVatLTL")
            PropertyHasChanged("SumTotalLTL")
        End Sub

        Friend Function GetCurrencyRate(ByVal pCurrencyRate As Double) As Double
            If CRound(pCurrencyRate, 6) > 0 Then Return pCurrencyRate
            If Parent Is Nothing Then Return 1
            If DirectCast(Parent, InvoiceReceivedItemList).CurrencyRate > 0 Then _
                Return DirectCast(Parent, InvoiceReceivedItemList).CurrencyRate
            Return 1
        End Function


        Friend Sub UpdateCurrencyRate(ByVal nCurrencyRate As Double)
            If Not _AttachedObject Is Nothing AndAlso Not _AttachedObject.ChronologyValidator. _
                FinancialDataCanChange Then
                Throw New Exception("Eilutės """ & _NameInvoice & """ finansiniai duomenys " _
                    & "negali būti keičiami: " & _AttachedObject.ChronologyValidator. _
                    FinancialDataCanChangeExplanation)
            ElseIf Not _FinancialDataCanChange Then
                Throw New Exception("Jokie sąskaitos faktūros finansiniai duomenys negali būti " _
                    & "keičiami, žr. dokumento apribojimus.")
            End If
            CalculateSum(nCurrencyRate)
            SetAttachedObjectData()
        End Sub


        Friend Sub MarkAsCopy()
            _ID = -1
            _Guid = Guid.NewGuid
            _FinancialDataCanChange = True
            MarkNew()
        End Sub

        Friend Function CanBeCopied() As Boolean
            Return (_AttachedObject Is Nothing OrElse _AttachedObject.Type = _
                InvoiceAttachedObjectType.Service)
        End Function


        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString
            If IsValid Then Return ""
            Return "Klaida (-os) eilutėje '" & _NameInvoice & "': " & _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error)
        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString
            If BrokenRulesCollection.WarningCount < 1 Then Return ""
            Return "Sąskaitos eilutėje '" & _NameInvoice & "' gali būti klaida: " & _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning)
        End Function


        Public Sub AttachedObjectChanged()
            If _AttachedObject Is Nothing Then Exit Sub
            _AttachedObject.UpdateItemData(Me, AddressOf OnAttachedObjectDataChanged)
            PropertyHasChanged("AttachedObject")
        End Sub

        Private Sub OnAttachedObjectDataChanged(ByVal InvoiceItemContent As String, _
            ByVal UpdateInvoiceItemContent As Boolean, ByVal InvoiceItemMeasureUnit As String, _
            ByVal UpdateInvoiceItemMeasureUnit As Boolean, ByVal InvoiceItemAccount As Long, _
            ByVal UpdateInvoiceItemAccount As Boolean, ByVal InvoiceItemAmount As Double, _
            ByVal UpdateInvoiceItemAmount As Boolean, ByVal InvoiceItemUnitValue As Double, _
            ByVal InvoiceItemSumCorrection As Integer, ByVal InvoiceItemSumVatCorrection As Integer, _
            ByVal InvoiceItemUnitValueLTLCorrection As Integer, ByVal InvoiceItemSumLTLCorrection As Integer, _
            ByVal InvoiceItemSumVatLTLCorrection As Integer, ByVal UpdateInvoiceItemUnitValue As Boolean)

            If UpdateInvoiceItemContent AndAlso _NameInvoice.Trim <> InvoiceItemContent.Trim Then
                _NameInvoice = InvoiceItemContent.Trim
                PropertyHasChanged("NameInvoice")
            End If

            If UpdateInvoiceItemMeasureUnit AndAlso _MeasureUnit.Trim <> InvoiceItemMeasureUnit.Trim Then
                _MeasureUnit = InvoiceItemMeasureUnit.Trim
                PropertyHasChanged("MeasureUnit")
            End If

            If Not FinancialDataCanChange() Then Exit Sub

            If UpdateInvoiceItemAccount AndAlso _AccountCosts <> InvoiceItemAccount Then
                _AccountCosts = InvoiceItemAccount
                PropertyHasChanged("AccountCosts")
            End If

            If UpdateInvoiceItemAmount AndAlso CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED) _
                <> CRound(InvoiceItemAmount, ROUNDAMOUNTINVOICERECEIVED) Then
                _Ammount = InvoiceItemAmount
                PropertyHasChanged("Ammount")
            End If

            If UpdateInvoiceItemUnitValue Then

                _UnitValue = CRound(InvoiceItemUnitValue, ROUNDUNITINVOICERECEIVED)
                _SumCorrection = InvoiceItemSumCorrection
                _SumVatCorrection = InvoiceItemSumVatCorrection
                _UnitValueLTLCorrection = InvoiceItemUnitValueLTLCorrection
                _SumLTLCorrection = InvoiceItemSumLTLCorrection
                _SumVatLTLCorrection = InvoiceItemSumVatLTLCorrection

                CalculateSum(0)

                PropertyHasChanged("UnitValue")
                PropertyHasChanged("UnitValueLTLCorrection")
                PropertyHasChanged("SumCorrection")
                PropertyHasChanged("SumLTLCorrection")
                PropertyHasChanged("SumVatLTLCorrection")
                PropertyHasChanged("SumVatCorrection")

            End If

        End Sub

        Friend Sub SetAttachedObjectData()
            If _AttachedObject Is Nothing Then Exit Sub
            _AttachedObject.FillWithItemData(Me)
            PropertyHasChanged("AttachedObject")
        End Sub

        Friend Sub SetAttachedObjectInvoiceDate(ByVal InvoiceDate As Date)
            If _AttachedObject Is Nothing Then Exit Sub
            _AttachedObject.FillWithInvoiceDate(InvoiceDate)
            PropertyHasChanged("AttachedObject")
        End Sub

        Friend Sub UpdateServiceInfo(ByVal doRecalculation As Boolean)

            If _AttachedObject Is Nothing OrElse _AttachedObject.Type _
                <> InvoiceAttachedObjectType.Service Then Exit Sub

            Dim s As ServiceInfo = DirectCast(_AttachedObject.ValueObject, ServiceInfo)

            If Not _FinancialDataCanChange Then Exit Sub

            _VatRate = s.RateVatPurchase

            PropertyHasChanged("VatRate")

            If Not CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED) > 0 Then
                _Ammount = CRound(s.Amount, ROUNDAMOUNTINVOICERECEIVED)
                PropertyHasChanged("Ammount")
            End If

            If doRecalculation Then CalculateSum(0)

        End Sub


        Friend Function GetInvoiceItemInfo() As InvoiceInfo.InvoiceItemInfo

            Dim result As New InvoiceInfo.InvoiceItemInfo

            result.Ammount = Me._Ammount
            result.Comments = ""
            result.Discount = 0
            result.DiscountCorrection = 0
            result.DiscountLTL = 0
            result.DiscountLTLCorrection = 0
            result.DiscountVat = 0
            result.DiscountVatCorrection = 0
            result.DiscountVatLTL = 0
            result.DiscountVatLTLCorrection = 0
            result.ID = _ID.ToString
            result.MeasureUnit = _MeasureUnit
            result.MeasureUnitAltLng = ""
            result.NameInvoice = _NameInvoice
            result.NameInvoiceAltLng = ""
            result.ProjectCode = ""
            result.Sum = _Sum
            result.SumCorrection = _SumCorrection
            result.SumLTL = _SumLTL
            result.SumLTLCorrection = _SumLTLCorrection
            result.SumReceived = 0
            result.SumTotal = _SumTotal
            result.SumTotalLTL = _SumTotalLTL
            result.SumVat = _SumVat
            result.SumVatCorrection = _SumVatCorrection
            result.SumVatLTL = _SumVatLTL
            result.SumVatLTLCorrection = _SumVatLTLCorrection
            result.UnitValue = _UnitValue
            result.UnitValueCorrection = 0
            result.UnitValueLTL = _UnitValueLTL
            result.UnitValueLTLCorrection = _UnitValueLTLCorrection
            result.VatRate = _VatRate

            Return result

        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            If Not _ID > 0 Then Return ""
            Return _NameInvoice
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.SimpleRuleArgs("NameInvoice", "eilutės turinys"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.SimpleRuleArgs("MeasureUnit", "mato vienetas"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("AccountCosts", "koresponduojanti (sąnaudų) sąskaita"))

            ValidationRules.AddRule(AddressOf AmountAndUnitValueValidation, "Ammount")
            ValidationRules.AddRule(AddressOf AmountAndUnitValueValidation, "UnitValue")
            ValidationRules.AddRule(AddressOf AccountVatValidation, "AccountVat")
            ValidationRules.AddRule(AddressOf AttachedObjectValidation, "AttachedObject")

            ValidationRules.AddDependantProperty("VatRate", "AccountVat", False)
            ValidationRules.AddDependantProperty("IncludeVatInObject", "AccountVat", False)
            ValidationRules.AddDependantProperty("Ammount", "UnitValue", True)

        End Sub


        ''' <summary>
        ''' Rule ensuring that Amount and UnitValue are valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AmountAndUnitValueValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If CRound(ValObj._Ammount, ROUNDAMOUNTINVOICERECEIVED) = 0 AndAlso e.PropertyName = "Ammount" Then

                e.Description = "Nenurodyta kiekis."
                e.Severity = Validation.RuleSeverity.Error
                Return False

            ElseIf CRound(ValObj._UnitValue, ROUNDUNITINVOICERECEIVED) = 0 AndAlso e.PropertyName = "UnitValue" Then

                e.Description = "Nenurodyta vieneto kaina."
                e.Severity = Validation.RuleSeverity.Error
                Return False

            ElseIf CRound(ValObj._UnitValue, ROUNDUNITINVOICERECEIVED) < 0 AndAlso _
                CRound(ValObj._Ammount, ROUNDAMOUNTINVOICERECEIVED) < 0 Then

                e.Description = "Kiekis ir vieneto kaina negali abu būti neigiami."
                e.Severity = Validation.RuleSeverity.Error
                Return False

            ElseIf Not ValObj._AttachedObject Is Nothing Then

                If ValObj._AttachedObject.Type = InvoiceAttachedObjectType.LongTermAssetPurchase _
                    AndAlso Not CRound(ValObj._Ammount, ROUNDAMOUNTINVOICERECEIVED) > 0 _
                    AndAlso e.PropertyName = "Ammount" Then

                    e.Description = "Įsigyjant ilgalaikį turtą per gautą sąskaitą faktūrą " _
                        & "kiekis turi būti teigiamas. Neįmanoma įsigyti turto pagal gautą " _
                        & "kreditinę sąskaitą."
                    e.Severity = Validation.RuleSeverity.Error
                    Return False

                ElseIf ValObj._AttachedObject.Type = InvoiceAttachedObjectType.LongTermAssetPurchase _
                    AndAlso Not CRound(ValObj._UnitValue, ROUNDUNITINVOICERECEIVED) > 0 _
                    AndAlso e.PropertyName = "UnitValue" Then

                    e.Description = "Įsigyjant ilgalaikį turtą per gautą sąskaitą faktūrą " _
                        & "vieneto kaina turi būti teigiama. Neįmanoma įsigyti turto pagal gautą " _
                        & "kreditinę sąskaitą."
                    e.Severity = Validation.RuleSeverity.Error
                    Return False

                ElseIf ValObj._AttachedObject.Type = InvoiceAttachedObjectType.LongTermAssetSale _
                    AndAlso Not CRound(ValObj._Ammount, ROUNDAMOUNTINVOICERECEIVED) < 0 _
                    AndAlso e.PropertyName = "Ammount" Then

                    e.Description = "Perleidžiant ilgalaikį turtą per gautą sąskaitą faktūrą " _
                        & "kiekis turi būti neigiamas. Turtas gali būti perleidžiamas " _
                        & "(grąžinamas) tik pagal gautą kreditinę sąskaitą."
                    e.Severity = Validation.RuleSeverity.Error
                    Return False

                ElseIf ValObj._AttachedObject.Type = InvoiceAttachedObjectType.LongTermAssetSale _
                    AndAlso Not CRound(ValObj._UnitValue, ROUNDUNITINVOICERECEIVED) > 0 _
                    AndAlso e.PropertyName = "UnitValue" Then

                    e.Description = "Perleidžiant ilgalaikį turtą per gautą sąskaitą faktūrą " _
                        & "vieneto kaina turi būti teigiama. Neįmanoma parleidžiant (grąžinant) " _
                        & "turtą per kreditinę sąskaitą dar ir sumažinti jo įsigijimo vertės."
                    e.Severity = Validation.RuleSeverity.Error
                    Return False

                End If


            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that AccountVat is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AccountVatValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If CRound(ValObj._VatRate) <> 0 AndAlso Not ValObj._AccountVat > 0 AndAlso _
                (Not ValObj._IncludeVatInObject OrElse ValObj._AttachedObject Is Nothing _
                OrElse Not ValObj._AttachedObject.VatIsHandledOnRequest) Then

                e.Description = "Nenurodyta PVM koresponduojanti sąskaita."
                e.Severity = Validation.RuleSeverity.Error
                Return False

            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring AttachedObject is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AttachedObjectValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If Not ValObj._AttachedObject Is Nothing AndAlso Not ValObj._AttachedObject.IsValid Then

                e.Description = ValObj._AttachedObject.BrokenRulesCollection.ToString( _
                    Validation.RuleSeverity.Error)
                e.Severity = Validation.RuleSeverity.Error
                Return False

            End If

            Return True

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

#End Region

#Region " Factory Methods "

        Friend Shared Function NewInvoiceReceivedItem(ByVal pDefaultVatRate As Double, _
            ByVal pDefaultMeasureUnit As String) As InvoiceReceivedItem
            Return New InvoiceReceivedItem(pDefaultVatRate, pDefaultMeasureUnit)
        End Function

        Friend Shared Function NewInvoiceReceivedItem(ByVal nAttachedObject As InvoiceAttachedObject) As InvoiceReceivedItem
            Return New InvoiceReceivedItem(nAttachedObject)
        End Function

        Friend Shared Function NewInvoiceReceivedItem(ByVal info As InvoiceInfo.InvoiceItemInfo, _
            ByVal pCurrencyRate As Double) As InvoiceReceivedItem
            Return New InvoiceReceivedItem(info, pCurrencyRate)
        End Function


        Friend Shared Function GetInvoiceReceivedItem(ByVal dr As DataRow, ByVal pCurrencyRate As Double, _
            ByVal baseChronologyValidator As SimpleChronologicValidator) As InvoiceReceivedItem
            Return New InvoiceReceivedItem(dr, pCurrencyRate, baseChronologyValidator)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
            ValidationRules.CheckRules()
        End Sub


        Private Sub New(ByVal DefaultVatRate As Double, ByVal DefaultMeasureUnit As String)
            _VatRate = DefaultVatRate
            _MeasureUnit = DefaultMeasureUnit
            _AccountVat = GetCurrentCompany.Accounts.GetAccount(General.DefaultAccountType.VatReceivable)
            MarkAsChild()
            ValidationRules.CheckRules()
        End Sub

        Private Sub New(ByVal nAttachedObject As InvoiceAttachedObject)
            Create(nAttachedObject)
        End Sub

        Private Sub New(ByVal info As InvoiceInfo.InvoiceItemInfo, ByVal pCurrencyRate As Double)
            MarkAsChild()
            FetchInvoiceItemInfo(info, pCurrencyRate)
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal pCurrencyRate As Double, _
            ByVal baseChronologyValidator As SimpleChronologicValidator)
            MarkAsChild()
            Fetch(dr, pCurrencyRate, baseChronologyValidator)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Create(ByVal nAttachedObject As InvoiceAttachedObject)

            _AttachedObject = nAttachedObject
            If nAttachedObject.Type = InvoiceAttachedObjectType.Service Then
                _AccountCosts = DirectCast(nAttachedObject.ValueObject, ServiceInfo).AccountPurchase
                _AccountVat = DirectCast(nAttachedObject.ValueObject, ServiceInfo).AccountVatPurchase
                _NameInvoice = DirectCast(nAttachedObject.ValueObject, ServiceInfo).NameInvoice
                _MeasureUnit = DirectCast(nAttachedObject.ValueObject, ServiceInfo).MeasureUnit
            ElseIf nAttachedObject.Type = InvoiceAttachedObjectType.LongTermAssetAcquisitionValueChange Then
                _AccountCosts = DirectCast(nAttachedObject.ValueObject,  _
                    Assets.LongTermAssetOperation).AssetAcquiredAccount
            ElseIf nAttachedObject.Type = InvoiceAttachedObjectType.GoodsAcquisition Then
                _AccountCosts = DirectCast(nAttachedObject.ValueObject,  _
                    Goods.GoodsOperationAcquisition).AcquisitionAccount
                _NameInvoice = DirectCast(nAttachedObject.ValueObject,  _
                    Goods.GoodsOperationAcquisition).GoodsInfo.Name
                _MeasureUnit = DirectCast(nAttachedObject.ValueObject,  _
                    Goods.GoodsOperationAcquisition).GoodsInfo.MeasureUnit
            ElseIf nAttachedObject.Type = InvoiceAttachedObjectType.GoodsConsignmentAdditionalCosts Then
                _AccountCosts = DirectCast(nAttachedObject.ValueObject,  _
                    Goods.GoodsOperationAdditionalCosts).AccountGoodsNetCosts
            ElseIf nAttachedObject.Type = InvoiceAttachedObjectType.GoodsConsignmentDiscount Then
                _AccountCosts = DirectCast(nAttachedObject.ValueObject,  _
                    Goods.GoodsOperationDiscount).AccountGoodsNetCosts
                _NameInvoice = "Nuolaida - " & DirectCast(nAttachedObject.ValueObject,  _
                    Goods.GoodsOperationDiscount).GoodsInfo.Name
            ElseIf nAttachedObject.Type = InvoiceAttachedObjectType.GoodsTransfer Then
                _NameInvoice = DirectCast(nAttachedObject.ValueObject,  _
                    Goods.GoodsOperationTransfer).GoodsInfo.Name
                _MeasureUnit = DirectCast(nAttachedObject.ValueObject,  _
                    Goods.GoodsOperationTransfer).GoodsInfo.MeasureUnit
            ElseIf nAttachedObject.Type = InvoiceAttachedObjectType.LongTermAssetSale Then
                _NameInvoice = DirectCast(nAttachedObject.ValueObject,  _
                    Assets.LongTermAssetOperation).AssetName
                _MeasureUnit = DirectCast(nAttachedObject.ValueObject,  _
                    Assets.LongTermAssetOperation).AssetMeasureUnit
            End If

            If nAttachedObject.Type <> InvoiceAttachedObjectType.Service Then
                _AccountVat = GetCurrentCompany.GetDefaultAccount(DefaultAccountType.VatReceivable)
            End If

            If nAttachedObject.HasDefaultVatRates Then
                _VatRate = nAttachedObject.DefaultVatRatePurchases
            Else
                _VatRate = GetCurrentCompany.GetDefaultRate(DefaultRateType.Vat)
            End If

            MarkAsChild()

            ValidationRules.CheckRules()

        End Sub

        Private Sub Fetch(ByVal dr As DataRow, ByVal pCurrencyRate As Double, _
            ByVal baseChronologyValidator As SimpleChronologicValidator)

            _ID = CIntSafe(dr.Item(0), 0)
            _NameInvoice = CStrSafe(dr.Item(1)).Trim
            _Ammount = CDblSafe(dr.Item(2), ROUNDAMOUNTINVOICERECEIVED, 0)
            _UnitValueLTL = CDblSafe(dr.Item(3), ROUNDUNITINVOICERECEIVED, 0)
            _SumLTL = CDblSafe(dr.Item(4), 2, 0)
            _VatRate = CDblSafe(dr.Item(5), 2, 0)
            _SumVatLTL = CDblSafe(dr.Item(6), 2, 0)
            _UnitValue = CDblSafe(dr.Item(7), ROUNDUNITINVOICERECEIVED, 0)
            _Sum = CDblSafe(dr.Item(8), 2, 0)
            _SumVat = CDblSafe(dr.Item(9), 2, 0)
            _MeasureUnit = CStrSafe(dr.Item(10)).Trim
            _AccountVat = CLongSafe(dr.Item(13), 0)
            _AccountCosts = CLongSafe(dr.Item(14), 0)
            _IncludeVatInObject = ConvertDbBoolean(CIntSafe(dr.Item(15), 0))

            CalculateCorrections(pCurrencyRate)

            _SumTotal = CRound(_Sum + _SumVat)
            _SumTotalLTL = CRound(_SumLTL + _SumVatLTL)

            If CIntSafe(dr.Item(11), 0) > 0 AndAlso CIntSafe(dr.Item(12), 0) > 0 Then
                _AttachedObject = InvoiceAttachedObject.GetInvoiceAttachedObject( _
                    ConvertEnumDatabaseCode(Of InvoiceAttachedObjectType) _
                    (CIntSafe(dr.Item(11), 0)), CIntSafe(dr.Item(12), 0), baseChronologyValidator)
            End If

            _FinancialDataCanChange = baseChronologyValidator.FinancialDataCanChange

            MarkOld()
            ValidationRules.CheckRules()

        End Sub

        Private Sub FetchInvoiceItemInfo(ByVal info As InvoiceInfo.InvoiceItemInfo, _
            ByVal pCurrencyRate As Double)

            Me._Ammount = info.Ammount
            Me._MeasureUnit = info.MeasureUnit
            Me._NameInvoice = info.NameInvoice
            Me._Sum = info.Sum
            Me._SumLTL = info.SumLTL
            Me._SumTotal = info.SumTotal
            Me._SumTotalLTL = info.SumTotalLTL
            Me._SumVat = info.SumVat
            Me._SumVatLTL = info.SumVatLTL
            Me._UnitValue = info.UnitValue
            Me._UnitValueLTL = info.UnitValueLTL
            Me._VatRate = info.VatRate

            CalculateCorrections(pCurrencyRate)

            ValidationRules.CheckRules()

        End Sub

        Private Sub CalculateCorrections(ByVal pCurrencyRate As Double)

            '_UnitValueLTL = CRound(CRound(_UnitValue * GetCurrencyRate(pCurrencyRate), 4) _
            '   + _UnitValueLTLCorrection / 100, 4)
            _UnitValueLTLCorrection = Convert.ToInt32(Math.Floor(CRound(_UnitValueLTL - _
                CRound(_UnitValue * GetCurrencyRate(pCurrencyRate), ROUNDUNITINVOICERECEIVED)) * 100))
            ' _SumLTL = CRound(CRound(_Sum * GetCurrencyRate(pCurrencyRate)) + _SumLTLCorrection / 100)
            _SumLTLCorrection = Convert.ToInt32(Math.Floor(CRound(_SumLTL - CRound(_Sum * GetCurrencyRate(pCurrencyRate))) * 100))
            ' _SumVatLTL = CRound(CRound(_SumVat * GetCurrencyRate(pCurrencyRate)) + _SumVatLTLCorrection / 100)
            _SumVatLTLCorrection = Convert.ToInt32(Math.Floor(CRound(_SumVatLTL - CRound(_SumVat * GetCurrencyRate(pCurrencyRate))) * 100))
            ' _Sum = CRound(CRound(_UnitValue * _Ammount) + _SumCorrection / 100)
            _SumCorrection = Convert.ToInt32(Math.Floor(CRound(_Sum - CRound(_UnitValue * _Ammount)) * 100))
            ' _SumVat = CRound(CRound(_Sum * _VatRate / 100) + _SumVatCorrection / 100)
            _SumVatCorrection = Convert.ToInt32(Math.Floor(CRound(_SumVat - CRound(_Sum * _VatRate / 100)) * 100))

        End Sub


        Friend Sub Insert(ByVal parent As InvoiceReceived)

            If Not _AttachedObject Is Nothing Then _AttachedObject.Update(parent.ID, _
                parent.Date, parent.Content, parent.Number, False)

            Dim myComm As New SQLCommand("InsertInvoiceReceivedItem")
            myComm.AddParam("?AA", parent.ID)
            If Not _AttachedObject Is Nothing Then
                myComm.AddParam("?AB", ConvertEnumDatabaseCode(_AttachedObject.Type))
                myComm.AddParam("?AC", _AttachedObject.ID)
            Else
                myComm.AddParam("?AB", 0)
                myComm.AddParam("?AC", 0)
            End If
            AddWithParamsGeneral(myComm)
            AddWithParamsFinancial(myComm)

            myComm.Execute()

            _ID = Convert.ToInt32(myComm.LastInsertID)

            MarkOld()

        End Sub

        Friend Sub Update(ByVal parent As InvoiceReceived)

            If Not _AttachedObject Is Nothing Then _AttachedObject.Update(parent.ID, _
                parent.Date, parent.Content, parent.Number, Not FinancialDataCanChange())

            Dim myComm As SQLCommand
            If FinancialDataCanChange() Then
                myComm = New SQLCommand("UpdateInvoiceReceivedItem")
                AddWithParamsFinancial(myComm)
            Else
                myComm = New SQLCommand("UpdateInvoiceReceivedItemGeneral")
            End If
            myComm.AddParam("?MD", _ID)
            AddWithParamsGeneral(myComm)

            myComm.Execute()

            MarkOld()

        End Sub

        Private Sub AddWithParamsGeneral(ByRef myComm As SQLCommand)
            myComm.AddParam("?AD", _NameInvoice.Trim)
            myComm.AddParam("?AE", _MeasureUnit.Trim)
        End Sub

        Private Sub AddWithParamsFinancial(ByRef myComm As SQLCommand)
            myComm.AddParam("?AF", CRound(_UnitValue, ROUNDUNITINVOICERECEIVED))
            myComm.AddParam("?AG", CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED))
            myComm.AddParam("?AH", CRound(_VatRate))
            myComm.AddParam("?AI", _AccountVat)
            myComm.AddParam("?AJ", _AccountCosts)
            myComm.AddParam("?AK", CRound(_Sum))
            myComm.AddParam("?AL", CRound(_SumVat))
            myComm.AddParam("?AM", CRound(_UnitValueLTL, ROUNDUNITINVOICERECEIVED))
            myComm.AddParam("?AN", CRound(_SumLTL))
            myComm.AddParam("?AO", CRound(_SumVatLTL))
            myComm.AddParam("?AQ", ConvertDbBoolean(_IncludeVatInObject))
        End Sub


        Friend Sub DeleteSelf()

            If Not _AttachedObject Is Nothing Then _AttachedObject.DeleteSelf()

            Dim myComm As New SQLCommand("DeleteInvoiceReceivedItem")
            myComm.AddParam("?MD", _ID)

            myComm.Execute()

            MarkNew()

        End Sub


        Friend Sub CheckIfCanUpdate(ByVal FinancialDataReadOnly As Boolean, _
            ByVal parentChronologyValidator As IChronologicValidator)
            If Not _AttachedObject Is Nothing Then
                _AttachedObject.CheckDataSync(Me)
                _AttachedObject.CheckIfCanUpdate(FinancialDataReadOnly, parentChronologyValidator)
            End If
        End Sub

        Friend Sub CheckIfCanDelete(ByVal parentChronologyValidator As IChronologicValidator)
            If Not parentChronologyValidator.FinancialDataCanChange Then Throw New Exception( _
                "Klaida. Sąskaitos faktūros eilučių pašalinti neleidžiama:" & vbCrLf _
                & parentChronologyValidator.FinancialDataCanChangeExplanation)
            If Not _AttachedObject Is Nothing Then _AttachedObject.CheckIfCanDelete(parentChronologyValidator)
        End Sub


        Friend Function GetBookEntryInternalList(ByVal AccountSupplier As Long) As BookEntryInternalList

            Dim result As BookEntryInternalList = BookEntryInternalList. _
                NewBookEntryInternalList(BookEntryType.Debetas)

            ' normal or debit invoice
            If CRound(_SumLTL) > 0 Then

                ' without attached object
                If _AttachedObject Is Nothing Then

                    result.Add(BookEntryInternal.NewBookEntryInternal( _
                        BookEntryType.Debetas, _AccountCosts, CRound(_SumLTL), Nothing))

                    If CRound(_SumVatLTL) > 0 Then result.Add(BookEntryInternal.NewBookEntryInternal( _
                        BookEntryType.Debetas, _AccountVat, CRound(_SumVatLTL), Nothing))

                    ' with delegation to attached object
                Else

                    result.AddRange(_AttachedObject.GetBookEntryList(CRound(_SumLTL), _AccountCosts, False))
                    If (Not _AttachedObject.VatIsHandledOnRequest OrElse _
                        Not _IncludeVatInObject) AndAlso CRound(_SumVatLTL) > 0 Then _
                        result.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas, _
                        _AccountVat, CRound(_SumVatLTL), Nothing))

                End If

                result.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas, _
                    AccountSupplier, CRound(_SumTotalLTL), Nothing))

                ' Credit invoice
            Else

                ' without attached object
                If _AttachedObject Is Nothing Then

                    result.Add(BookEntryInternal.NewBookEntryInternal( _
                        BookEntryType.Kreditas, _AccountCosts, CRound(-_SumLTL), Nothing))

                    If CRound(-_SumVatLTL) > 0 Then result.Add(BookEntryInternal.NewBookEntryInternal( _
                        BookEntryType.Kreditas, _AccountVat, CRound(-_SumVatLTL), Nothing))

                    ' with delegation to attached object
                Else

                    result.AddRange(_AttachedObject.GetBookEntryList(CRound(-_SumLTL), _AccountCosts, True))
                    If (Not _AttachedObject.VatIsHandledOnRequest OrElse _
                        Not _IncludeVatInObject) AndAlso CRound(-_SumVatLTL) > 0 Then _
                        result.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas, _
                        _AccountVat, CRound(-_SumVatLTL), Nothing))

                End If

                result.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas, _
                    AccountSupplier, CRound(-_SumTotalLTL), Nothing))

            End If

            Return result

        End Function

#End Region

    End Class

End Namespace