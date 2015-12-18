Imports ApskaitaObjects.Documents.InvoiceAdapters

Namespace Documents

    <Serializable()> _
    Public Class InvoiceAttachedObject
        Inherits BusinessBase(Of InvoiceAttachedObject)
        Implements IRegionalDataObject

#Region " Business Methods "

        Public Delegate Sub OnAttachedObjectDataChanged(ByVal InvoiceItemContent As String, _
            ByVal UpdateInvoiceItemContent As Boolean, ByVal InvoiceItemMeasureUnit As String, _
            ByVal UpdateInvoiceItemMeasureUnit As Boolean, ByVal InvoiceItemAccount As Long, _
            ByVal UpdateInvoiceItemAccount As Boolean, ByVal InvoiceItemAmount As Double, _
            ByVal UpdateInvoiceItemAmount As Boolean, ByVal InvoiceItemUnitValue As Double, _
            ByVal InvoiceItemSumCorrection As Integer, ByVal InvoiceItemSumVatCorrection As Integer, _
            ByVal InvoiceItemUnitValueLTLCorrection As Integer, ByVal InvoiceItemSumLTLCorrection As Integer, _
            ByVal InvoiceItemSumVatLTLCorrection As Integer, ByVal UpdateInvoiceItemUnitValue As Boolean)

        Private ReadOnly _Guid As Guid = Guid.NewGuid
        Private _ID As Integer = 0
        Private _Type As InvoiceAdapterType = InvoiceAdapterType.LongTermAssetPurchase
        Private _LongTermAsset As Assets.LongTermAsset = Nothing
        Private _LongTermSale As Assets.LongTermAssetOperation = Nothing
        Private _LongTermAcquisitionValueChange As Assets.LongTermAssetOperation = Nothing
        Private _GoodsAcquisition As Goods.GoodsOperationAcquisition
        Private _GoodsTransfer As Goods.GoodsOperationTransfer
        Private _GoodsDiscount As Goods.GoodsOperationDiscount
        Private _GoodsAdditionalCosts As Goods.GoodsOperationAdditionalCosts
        Private _Service As ServiceInfo = Nothing
        Private _ChronologyValidator As IChronologicValidator = Nothing


        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property [Type]() As InvoiceAdapterType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Type
            End Get
        End Property

        Public ReadOnly Property ValueObject() As Object
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                    Return _LongTermAsset
                ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                    Return _LongTermSale
                ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                    Return _LongTermAcquisitionValueChange
                ElseIf _Type = InvoiceAdapterType.Service Then
                    Return _Service
                ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                    Return _GoodsAcquisition
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                    Return _GoodsAdditionalCosts
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                    Return _GoodsDiscount
                ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                    Return _GoodsTransfer
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property ChronologyValidator() As IChronologicValidator
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                    Return _LongTermAsset.ChronologyValidator
                ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                    Return _LongTermSale.ChronologyValidator
                ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                    Return _LongTermAcquisitionValueChange.ChronologyValidator
                ElseIf _Type = InvoiceAdapterType.Service Then
                    If _ChronologyValidator Is Nothing Then _ChronologyValidator = _
                        EmptyChronologicValidator.NewEmptyChronologicValidator("Paslauga """ _
                        & _Service.NameShort & """.", Nothing)
                    Return _ChronologyValidator
                ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                    Return _GoodsAcquisition.OperationLimitations
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                    Return _GoodsAdditionalCosts.OperationLimitations
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                    Return _GoodsDiscount.OperationLimitations
                ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                    Return _GoodsTransfer.OperationLimitations
                Else
                    If _ChronologyValidator Is Nothing Then _ChronologyValidator = _
                        EmptyChronologicValidator.NewEmptyChronologicValidator( _
                        "Nenustatytas objektas", Nothing)
                    Return _ChronologyValidator
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                    Return _LongTermAsset.IsDirty
                ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                    Return _LongTermSale.IsDirty
                ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                    Return _LongTermAcquisitionValueChange.IsDirty
                ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                    Return _GoodsAcquisition.IsDirty
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                    Return _GoodsAdditionalCosts.IsDirty
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                    Return _GoodsDiscount.IsDirty
                ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                    Return _GoodsTransfer.IsDirty
                Else
                    Return False
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                    Return _LongTermAsset.IsValid
                ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                    Return _LongTermSale.IsValid
                ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                    Return _LongTermAcquisitionValueChange.IsValid
                ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                    Return _GoodsAcquisition.IsValid
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                    Return _GoodsAdditionalCosts.IsValid
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                    Return _GoodsDiscount.IsValid
                ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                    Return _GoodsTransfer.IsValid
                Else
                    Return True
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property BrokenRulesCollection() As Csla.Validation.BrokenRulesCollection
            Get
                If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                    Return _LongTermAsset.BrokenRulesCollection
                ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                    Return _LongTermSale.BrokenRulesCollection
                ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                    Return _LongTermAcquisitionValueChange.BrokenRulesCollection
                ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                    Return _GoodsAcquisition.BrokenRulesCollection
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                    Return _GoodsAdditionalCosts.BrokenRulesCollection
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                    Return _GoodsDiscount.BrokenRulesCollection
                ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                    Return _GoodsTransfer.BrokenRulesCollection
                Else
                    Return MyBase.BrokenRulesCollection
                End If
            End Get
        End Property

        Public ReadOnly Property IsRegionalizable() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return (_Type = InvoiceAdapterType.GoodsAcquisition OrElse _
                    _Type = InvoiceAdapterType.GoodsTransfer OrElse _
                    _Type = InvoiceAdapterType.Service)
            End Get
        End Property

        Public ReadOnly Property RegionalizableObjectType() As RegionalizedObjectType _
            Implements IRegionalDataObject.RegionalObjectType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = InvoiceAdapterType.GoodsAcquisition OrElse _
                    _Type = InvoiceAdapterType.GoodsTransfer Then
                    Return RegionalizedObjectType.Goods
                Else
                    Return RegionalizedObjectType.Service
                End If
            End Get
        End Property

        Public ReadOnly Property RegionalizableObjectID() As Integer _
            Implements IRegionalDataObject.RegionalObjectID
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = InvoiceAdapterType.GoodsTransfer Then
                    If _GoodsTransfer Is Nothing Then
                        Return 0
                    Else
                        Return _GoodsTransfer.GoodsInfo.ID
                    End If
                ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                    If _GoodsAcquisition Is Nothing Then
                        Return 0
                    Else
                        Return _GoodsAcquisition.GoodsInfo.ID
                    End If
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                    If Me._GoodsAdditionalCosts Is Nothing Then
                        Return 0
                    Else
                        Return _GoodsAdditionalCosts.GoodsInfo.ID
                    End If
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                    If Me._GoodsDiscount Is Nothing Then
                        Return 0
                    Else
                        Return _GoodsDiscount.GoodsInfo.ID
                    End If
                ElseIf _Type = InvoiceAdapterType.Service Then
                    If _Service Is Nothing Then
                        Return 0
                    Else
                        Return _Service.ID
                    End If
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property DefaultVatRateSales() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                    Return 0.0
                ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                    Return 0.0
                ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                    Return 0.0
                ElseIf _Type = InvoiceAdapterType.Service Then
                    Return _Service.RateVatSales
                ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                    Return _GoodsAcquisition.GoodsInfo.DefaultVatRateSales
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                    Return 0.0
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                    Return 0.0
                ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                    Return _GoodsTransfer.GoodsInfo.DefaultVatRateSales
                End If
                Return 0.0
            End Get
        End Property

        Public ReadOnly Property DefaultVatRatePurchases() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                    Return 0.0
                ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                    Return 0.0
                ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                    Return 0.0
                ElseIf _Type = InvoiceAdapterType.Service Then
                    Return _Service.RateVatPurchase
                ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                    Return _GoodsAcquisition.GoodsInfo.DefaultVatRatePurchase
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                    Return 0.0
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                    Return 0.0
                ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                    Return _GoodsTransfer.GoodsInfo.DefaultVatRatePurchase
                End If
                Return 0.0
            End Get
        End Property

        Public ReadOnly Property HasDefaultVatRates() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                    Return False
                ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                    Return False
                ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                    Return False
                ElseIf _Type = InvoiceAdapterType.Service Then
                    Return True
                ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                    Return True
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                    Return False
                ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                    Return False
                ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                    Return True
                End If
                Return False
            End Get
        End Property


        Friend Sub FillWithItemData(ByVal ReceivedItem As InvoiceReceivedItem)

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                If String.IsNullOrEmpty(_LongTermAsset.Name.Trim) Then _
                    _LongTermAsset.Name = ReceivedItem.NameInvoice
                If String.IsNullOrEmpty(_LongTermAsset.MeasureUnit.Trim) Then _
                    _LongTermAsset.MeasureUnit = ReceivedItem.MeasureUnit

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                If String.IsNullOrEmpty(_LongTermSale.Content.Trim) Then _
                    _LongTermSale.Content = ReceivedItem.NameInvoice

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                If String.IsNullOrEmpty(_LongTermAcquisitionValueChange.Content.Trim) Then _
                    _LongTermAcquisitionValueChange.Content = ReceivedItem.NameInvoice

            ElseIf _Type = InvoiceAdapterType.Service Then

                ' nothing to do for service

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then


            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                _GoodsAdditionalCosts.Description = ReceivedItem.NameInvoice

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                _GoodsDiscount.Description = ReceivedItem.NameInvoice

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then


            End If

            If Not ChronologyValidator.FinancialDataCanChange Then Exit Sub

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                If Not ReceivedItem.Ammount > 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then Exit Sub

                _LongTermAsset.AccountAcquisition = ReceivedItem.AccountCosts

                If ReceivedItem.IncludeVatInObject Then
                    _LongTermAsset.SetValues(Convert.ToInt32(ReceivedItem.Ammount), ReceivedItem.SumTotalLTL, _
                        CRound(ReceivedItem.UnitValueLTL + (ReceivedItem.SumVatLTL _
                        / ReceivedItem.Ammount), ROUNDUNITASSET))
                Else
                    _LongTermAsset.SetValues(Convert.ToInt32(ReceivedItem.Ammount), ReceivedItem.SumLTL, _
                        ReceivedItem.UnitValueLTL)
                End If

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                If Not ReceivedItem.Ammount < 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then Exit Sub

                _LongTermSale.AccountCorresponding = ReceivedItem.AccountCosts
                _LongTermSale.AmmountChange = -Convert.ToInt32(ReceivedItem.Ammount)

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                If Not ReceivedItem.Ammount > 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then Exit Sub

                If ReceivedItem.IncludeVatInObject Then
                    _LongTermAcquisitionValueChange.TotalValueChange = ReceivedItem.SumTotalLTL
                Else
                    _LongTermAcquisitionValueChange.TotalValueChange = ReceivedItem.SumLTL
                End If
                _LongTermAcquisitionValueChange.UnitValueChange = _
                    CRound(_LongTermAcquisitionValueChange.TotalValueChange / _
                    _LongTermAcquisitionValueChange.CurrentAssetAmmount, ROUNDUNITASSET)

            ElseIf _Type = InvoiceAdapterType.Service Then

                ' nothing to do for service

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then

                If Not ReceivedItem.Ammount > 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then Exit Sub

                _GoodsAcquisition.Ammount = ReceivedItem.Ammount

                If ReceivedItem.IncludeVatInObject Then
                    _GoodsAcquisition.UnitCost = CRound(ReceivedItem.UnitValueLTL + (ReceivedItem.SumVatLTL _
                        / ReceivedItem.Ammount), 6)
                    _GoodsAcquisition.TotalCost = ReceivedItem.SumTotalLTL
                Else
                    _GoodsAcquisition.UnitCost = ReceivedItem.UnitValueLTL
                    _GoodsAcquisition.TotalCost = ReceivedItem.SumLTL
                End If

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                If Not ReceivedItem.Ammount > 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then Exit Sub

                If ReceivedItem.IncludeVatInObject Then
                    _GoodsAdditionalCosts.SetTotalValueChange(ReceivedItem.SumTotalLTL)
                Else
                    _GoodsAdditionalCosts.SetTotalValueChange(ReceivedItem.SumLTL)
                End If

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                If Not ReceivedItem.Ammount > 0 OrElse Not ReceivedItem.UnitValueLTL < 0 Then Exit Sub

                If ReceivedItem.IncludeVatInObject Then
                    _GoodsDiscount.SetTotalValueChange(-ReceivedItem.SumTotalLTL)
                Else
                    _GoodsDiscount.SetTotalValueChange(-ReceivedItem.SumLTL)
                End If

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then

                If Not ReceivedItem.Ammount < 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then Exit Sub

                _GoodsTransfer.Amount = -ReceivedItem.Ammount

            End If

        End Sub

        Friend Sub FillWithItemData(ByVal MadeItem As InvoiceMadeItem)

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                If String.IsNullOrEmpty(_LongTermAsset.Name.Trim) Then _
                    _LongTermAsset.Name = MadeItem.NameInvoice
                If String.IsNullOrEmpty(_LongTermAsset.MeasureUnit.Trim) Then _
                    _LongTermAsset.MeasureUnit = MadeItem.MeasureUnit

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                If String.IsNullOrEmpty(_LongTermSale.Content.Trim) Then _
                    _LongTermSale.Content = MadeItem.NameInvoice

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                If String.IsNullOrEmpty(_LongTermAcquisitionValueChange.Content.Trim) Then _
                    _LongTermAcquisitionValueChange.Content = MadeItem.NameInvoice

            ElseIf _Type = InvoiceAdapterType.Service Then

                ' nothing to do for service

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then


            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                ' nothing to do additional costs can only be made by invoice received

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                _GoodsDiscount.Description = MadeItem.NameInvoice

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then


            End If

            If Not ChronologyValidator.FinancialDataCanChange Then Exit Sub

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                If Not MadeItem.Ammount < 0 OrElse Not MadeItem.UnitValueLTL > 0 Then Exit Sub

                _LongTermAsset.AccountAcquisition = MadeItem.AccountIncome

                If MadeItem.IncludeVatInObject Then
                    _LongTermAsset.SetValues(-Convert.ToInt32(MadeItem.Ammount), -MadeItem.SumTotalLTL, _
                        CRound(MadeItem.UnitValueLTL + (MadeItem.SumVatLTL / MadeItem.Ammount), ROUNDUNITASSET))
                Else
                    _LongTermAsset.SetValues(-Convert.ToInt32(MadeItem.Ammount), -MadeItem.SumLTL, MadeItem.UnitValueLTL)
                End If

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                If Not MadeItem.Ammount > 0 OrElse Not MadeItem.UnitValueLTL > 0 Then Exit Sub

                _LongTermSale.AccountCorresponding = MadeItem.AccountIncome
                _LongTermSale.AmmountChange = Convert.ToInt32(MadeItem.Ammount)

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                If Not MadeItem.Ammount > 0 OrElse Not MadeItem.UnitValueLTL < 0 Then Exit Sub

                If MadeItem.IncludeVatInObject Then
                    _LongTermAcquisitionValueChange.TotalValueChange = -MadeItem.SumTotalLTL
                Else
                    _LongTermAcquisitionValueChange.TotalValueChange = -MadeItem.SumLTL
                End If
                _LongTermAcquisitionValueChange.UnitValueChange = _
                    CRound(_LongTermAcquisitionValueChange.TotalValueChange / _
                    _LongTermAcquisitionValueChange.CurrentAssetAmmount, ROUNDUNITASSET)

            ElseIf _Type = InvoiceAdapterType.Service Then

                ' nothing to do for service

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then

                If Not MadeItem.Ammount < 0 OrElse Not MadeItem.UnitValueLTL > 0 Then Exit Sub

                _GoodsAcquisition.Ammount = -MadeItem.Ammount

                If MadeItem.IncludeVatInObject Then
                    _GoodsAcquisition.TotalCost = -MadeItem.SumTotalLTL
                    _GoodsAcquisition.UnitCost = CRound(MadeItem.UnitValueLTL _
                        + (MadeItem.SumVatLTL / MadeItem.Ammount), 6)
                Else
                    _GoodsAcquisition.TotalCost = -MadeItem.SumLTL
                    _GoodsAcquisition.UnitCost = MadeItem.UnitValueLTL
                End If

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                ' nothing to do additional costs can only be made by invoice received

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                If Not MadeItem.Ammount > 0 OrElse Not MadeItem.UnitValueLTL > 0 Then Exit Sub

                If MadeItem.IncludeVatInObject Then
                    _GoodsDiscount.SetTotalValueChange(MadeItem.SumTotalLTL)
                Else
                    _GoodsDiscount.SetTotalValueChange(MadeItem.SumLTL)
                End If

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then

                If Not MadeItem.Ammount > 0 OrElse Not MadeItem.UnitValueLTL > 0 Then Exit Sub

                _GoodsTransfer.Amount = MadeItem.Ammount

            End If

        End Sub

        Friend Sub FillWithInvoiceDate(ByVal InvoiceDate As Date)

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                _LongTermAsset.SetDate(InvoiceDate)

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                _LongTermSale.SetDate(InvoiceDate)

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                _LongTermAcquisitionValueChange.SetDate(InvoiceDate)

            ElseIf _Type = InvoiceAdapterType.Service Then

                ' nothing to do for service

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then

                _GoodsAcquisition.SetDate(InvoiceDate)

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                _GoodsAdditionalCosts.SetDate(InvoiceDate)

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                _GoodsDiscount.SetDate(InvoiceDate)

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then

                _GoodsTransfer.SetDate(InvoiceDate)

            End If

        End Sub

        Friend Function ValidateAmountAndUnitValue(ByVal ReceivedItem As InvoiceReceivedItem, _
            ByRef ErrorDescription As String, ByRef ErrorLevel As Csla.Validation.RuleSeverity) As Boolean

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                If Not ReceivedItem.Ammount > 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis ir vnt. kaina privalo būti teigiami. " _
                        & "T.y. įsigyti IT galima tik registruojant gautą paprastą arba debetinę sąskaitą faktūrą. "
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                If Not ReceivedItem.Ammount < 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis privalo būti neigiamas, o vnt. kaina teigiama. " _
                        & "T.y. perleisti IT galima tik registruojant gautą kreditinę sąskaitą faktūrą, " _
                        & "kuria tipiškai yra įforminamas pirkto IT turto grąžinimas."
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If


            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                If Not ReceivedItem.Ammount > 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis ir vnt. kaina privalo būti teigiami. T.y. padidinti IT savikainą " _
                        & "galima tik registruojant gautą paprastą arba debetinę sąskaitą faktūrą. "
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If


            ElseIf _Type = InvoiceAdapterType.Service Then

                ' nothing to do for service

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then

                If Not ReceivedItem.Ammount > 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis ir vnt. kaina privalo būti teigiami. " _
                        & "T.y. įsigyti prekes galima tik registruojant gautą paprastą arba debetinę sąskaitą faktūrą. "
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                If Not ReceivedItem.Ammount > 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis ir vnt. kaina privalo būti teigiami. T.y. padidinti prekių " _
                        & "savikainą galima tik registruojant gautą paprastą arba debetinę sąskaitą faktūrą. "
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                If Not ReceivedItem.Ammount > 0 OrElse Not ReceivedItem.UnitValueLTL < 0 Then

                    ErrorDescription = "Kiekis privalo būti teigiamas, o vnt. kaina neigiama. " _
                        & "T.y. registruoti gautą nuolaidą prekėms galima tik registruojant gautą " _
                        & "kreditinę sąskaitą faktūrą."
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then

                If Not ReceivedItem.Ammount < 0 OrElse Not ReceivedItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis privalo būti neigiamas, o vnt. kaina teigiama. " _
                        & "T.y. perleisti prekes galima tik registruojant gautą kreditinę sąskaitą faktūrą, " _
                        & "kuria tipiškai yra įforminamas pirktų prekių grąžinimas."
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            End If

        End Function

        Friend Function ValidateAmountAndUnitValue(ByVal MadeItem As InvoiceMadeItem, _
            ByRef ErrorDescription As String, ByRef ErrorLevel As Csla.Validation.RuleSeverity) As Boolean

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                If Not MadeItem.Ammount < 0 OrElse Not MadeItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis privalo būti neigiamas, o vnt. kaina teigiama. " _
                        & "T.y. įsigyti IT galima tik išrašant kreditinę sąskaitą faktūrą, " _
                        & "kuria tipiškai yra įforminamas parduoto IT turto grąžinimas."
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                If Not MadeItem.Ammount > 0 OrElse Not MadeItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis ir vnt. kaina privalo būti teigiami. T.y. parduoti IT galima " _
                        & "tik išrašant paprastą arba debetinę sąskaitą faktūrą."
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                If Not MadeItem.Ammount > 0 OrElse Not MadeItem.UnitValueLTL < 0 Then

                    ErrorDescription = "Kiekis privalo būti teigiamas, o vnt. kaina neigiama. " _
                        & "T.y. padidinti IT savikainą galima tik išrašant kreditinę sąskaitą. " _
                        & "Nors tokia operacija yra įtartina pati savaime."
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            ElseIf _Type = InvoiceAdapterType.Service Then

                ' nothing to do for service

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then

                If Not MadeItem.Ammount < 0 OrElse Not MadeItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis privalo būti neigiamas, o vnt. kaina teigiama. " _
                        & "T.y. įsigyti prekes galima tik išrašant kreditinę sąskaitą faktūrą, " _
                        & "kuria tipiškai yra įforminamas parduotų prekių grąžinimas."
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                ErrorDescription = "Padidinti prekių savikainą išrašant sąskaitą faktūrą neįmanoma."
                ErrorLevel = Validation.RuleSeverity.Error
                Return False

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                If Not MadeItem.Ammount > 0 OrElse Not MadeItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis ir vnt. kaina privalo būti teigimi. T.y. gauta prekių " _
                        & "nuolaida gali būti registruojama tik išrašant debetinę sąskaitą faktūrą. " _
                        & "Nors tokia operacija yra įtartina pati savaime."
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then

                If Not MadeItem.Ammount > 0 OrElse Not MadeItem.UnitValueLTL > 0 Then

                    ErrorDescription = "Kiekis ir vnt. kaina privalo būti teigiami. T.y. parduoti prekes galima " _
                        & "tik išrašant paprastą arba debetinę sąskaitą faktūrą."
                    ErrorLevel = Validation.RuleSeverity.Error
                    Return False

                End If

            End If

            Return True

        End Function

        Friend Sub UpdateItemData(ByVal InvoiceReceivedItem As InvoiceReceivedItem, _
            ByRef UpdateDelegate As OnAttachedObjectDataChanged)

            If _Type = InvoiceAdapterType.Service Then Exit Sub

            Dim InvoiceItemContent As String = ""
            Dim UpdateInvoiceItemContent As Boolean = False
            Dim InvoiceItemMeasureUnit As String = ""
            Dim UpdateInvoiceItemMeasureUnit As Boolean = False
            Dim InvoiceItemAccount As Long = 0
            Dim UpdateInvoiceItemAccount As Boolean = False
            Dim InvoiceItemAmount As Double = 0
            Dim UpdateInvoiceItemAmount As Boolean = False
            Dim InvoiceItemUnitValue As Double = 0
            Dim InvoiceItemSumCorrection As Integer = 0
            Dim InvoiceItemSumVatCorrection As Integer = 0
            Dim InvoiceItemUnitValueLTLCorrection As Integer = 0
            Dim InvoiceItemSumLTLCorrection As Integer = 0
            Dim InvoiceItemSumVatLTLCorrection As Integer = 0
            Dim UpdateInvoiceItemUnitValue As Boolean = False

            ' Do non financial updates

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                InvoiceItemContent = _LongTermAsset.Name
                InvoiceItemMeasureUnit = _LongTermAsset.MeasureUnit
                UpdateInvoiceItemContent = True
                UpdateInvoiceItemMeasureUnit = True

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                InvoiceItemContent = _LongTermSale.Content
                InvoiceItemMeasureUnit = _LongTermSale.AssetMeasureUnit
                UpdateInvoiceItemContent = True
                UpdateInvoiceItemMeasureUnit = True

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                InvoiceItemContent = _LongTermAcquisitionValueChange.Content
                UpdateInvoiceItemContent = True

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                ' nothing non financial?
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                ' nothing non financial?
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                ' nothing non financial?
            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                ' nothing non financial?
            End If

            ' Do financial updates if allowed

            If Not ChronologyValidator.FinancialDataCanChange Then
                UpdateDelegate.Invoke(InvoiceItemContent, UpdateInvoiceItemContent, _
                    InvoiceItemMeasureUnit, UpdateInvoiceItemMeasureUnit, 0, False, 0, _
                    False, 0, 0, 0, 0, 0, 0, False)
                Exit Sub
            End If

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                InvoiceItemAccount = _LongTermAsset.AccountAcquisition
                UpdateInvoiceItemAccount = True
                InvoiceItemAmount = _LongTermAsset.Ammount
                UpdateInvoiceItemAmount = True
                ReverseCalculateValues(_LongTermAsset.Ammount, _LongTermAsset.ValuePerUnit, _
                    _LongTermAsset.Value, InvoiceReceivedItem.GetCurrencyRate(0), _
                    InvoiceReceivedItem.IncludeVatInObject, InvoiceReceivedItem.VatRate, True, _
                    InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                    InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                    InvoiceItemSumVatLTLCorrection)
                UpdateInvoiceItemUnitValue = True

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                InvoiceItemAccount = _LongTermSale.AccountCorresponding
                UpdateInvoiceItemAccount = True
                InvoiceItemAmount = _LongTermSale.AmmountChange
                UpdateInvoiceItemAmount = True

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                InvoiceItemAmount = 1
                UpdateInvoiceItemAmount = True
                ReverseCalculateValues(1, _LongTermAcquisitionValueChange.TotalValueChange, _
                    _LongTermAcquisitionValueChange.TotalValueChange, InvoiceReceivedItem.GetCurrencyRate(0), _
                    InvoiceReceivedItem.IncludeVatInObject, InvoiceReceivedItem.VatRate, True, _
                    InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                    InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                    InvoiceItemSumVatLTLCorrection)
                UpdateInvoiceItemUnitValue = True

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then

                InvoiceItemAmount = CRound(_GoodsAcquisition.Ammount, ROUNDAMOUNTINVOICERECEIVED)
                UpdateInvoiceItemAmount = True
                ReverseCalculateValues(InvoiceItemAmount, _GoodsAcquisition.UnitCost, _
                    _GoodsAcquisition.TotalCost, InvoiceReceivedItem.GetCurrencyRate(0), _
                    InvoiceReceivedItem.IncludeVatInObject, InvoiceReceivedItem.VatRate, True, _
                    InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                    InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                    InvoiceItemSumVatLTLCorrection)
                UpdateInvoiceItemUnitValue = True

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                InvoiceItemAccount = _GoodsAdditionalCosts.AccountGoodsNetCosts
                UpdateInvoiceItemAccount = True
                InvoiceItemAmount = 1
                UpdateInvoiceItemAmount = True
                ReverseCalculateValues(1, _GoodsAdditionalCosts.TotalValueChange, _
                    _GoodsAdditionalCosts.TotalValueChange, InvoiceReceivedItem.GetCurrencyRate(0), _
                    InvoiceReceivedItem.IncludeVatInObject, InvoiceReceivedItem.VatRate, True, _
                    InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                    InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                    InvoiceItemSumVatLTLCorrection)
                UpdateInvoiceItemUnitValue = True

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                InvoiceItemAccount = _GoodsDiscount.AccountGoodsNetCosts
                UpdateInvoiceItemAccount = True
                InvoiceItemAmount = 1
                UpdateInvoiceItemAmount = True
                ReverseCalculateValues(1, -_GoodsDiscount.TotalValueChange, _
                    -_GoodsDiscount.TotalValueChange, InvoiceReceivedItem.GetCurrencyRate(0), _
                    InvoiceReceivedItem.IncludeVatInObject, InvoiceReceivedItem.VatRate, True, _
                    InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                    InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                    InvoiceItemSumVatLTLCorrection)
                UpdateInvoiceItemUnitValue = True

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then

                InvoiceItemAccount = _GoodsTransfer.AccountGoodsCost
                UpdateInvoiceItemAccount = True
                InvoiceItemAmount = -CRound(_GoodsTransfer.Amount, ROUNDAMOUNTINVOICERECEIVED)
                UpdateInvoiceItemAmount = True

            End If

            UpdateDelegate.Invoke(InvoiceItemContent, UpdateInvoiceItemContent, _
                InvoiceItemMeasureUnit, UpdateInvoiceItemMeasureUnit, InvoiceItemAccount, _
                UpdateInvoiceItemAccount, InvoiceItemAmount, UpdateInvoiceItemAmount, _
                InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                InvoiceItemSumVatLTLCorrection, UpdateInvoiceItemUnitValue)

        End Sub

        Friend Sub UpdateItemData(ByVal InvoiceMadeItem As InvoiceMadeItem, _
            ByRef UpdateDelegate As OnAttachedObjectDataChanged)

            If _Type = InvoiceAdapterType.Service OrElse _
                _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then Exit Sub

            Dim InvoiceItemContent As String = ""
            Dim UpdateInvoiceItemContent As Boolean = False
            Dim InvoiceItemMeasureUnit As String = ""
            Dim UpdateInvoiceItemMeasureUnit As Boolean = False
            Dim InvoiceItemAccount As Long = 0
            Dim UpdateInvoiceItemAccount As Boolean = False
            Dim InvoiceItemAmount As Double = 0
            Dim UpdateInvoiceItemAmount As Boolean = False
            Dim InvoiceItemUnitValue As Double = 0
            Dim InvoiceItemSumCorrection As Integer = 0
            Dim InvoiceItemSumVatCorrection As Integer = 0
            Dim InvoiceItemUnitValueLTLCorrection As Integer = 0
            Dim InvoiceItemSumLTLCorrection As Integer = 0
            Dim InvoiceItemSumVatLTLCorrection As Integer = 0
            Dim UpdateInvoiceItemUnitValue As Boolean = False

            ' Do non financial updates

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                InvoiceItemContent = _LongTermAsset.Name
                InvoiceItemMeasureUnit = _LongTermAsset.MeasureUnit
                UpdateInvoiceItemContent = True
                UpdateInvoiceItemMeasureUnit = True

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                InvoiceItemContent = _LongTermSale.Content
                InvoiceItemMeasureUnit = _LongTermSale.AssetMeasureUnit
                UpdateInvoiceItemContent = True
                UpdateInvoiceItemMeasureUnit = True

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                InvoiceItemContent = _LongTermAcquisitionValueChange.Content
                UpdateInvoiceItemContent = True

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                ' nothing non financial?
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                ' nothing non financial?
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                ' nothing non financial?
            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                ' nothing non financial?
            End If

            ' Do financial updates if allowed

            If Not ChronologyValidator.FinancialDataCanChange Then
                UpdateDelegate.Invoke(InvoiceItemContent, UpdateInvoiceItemContent, _
                    InvoiceItemMeasureUnit, UpdateInvoiceItemMeasureUnit, 0, False, 0, _
                    False, 0, 0, 0, 0, 0, 0, False)
                Exit Sub
            End If

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                ' to purchase an asset by issuing an invoice the invoice should be credit
                InvoiceItemAccount = _LongTermAsset.AccountAcquisition
                UpdateInvoiceItemAccount = True
                InvoiceItemAmount = -_LongTermAsset.Ammount
                UpdateInvoiceItemAmount = True
                ReverseCalculateValues(-_LongTermAsset.Ammount, _LongTermAsset.ValuePerUnit, _
                    -_LongTermAsset.Value, InvoiceMadeItem.GetCurrencyRate(0), _
                    InvoiceMadeItem.IncludeVatInObject, InvoiceMadeItem.VatRate, False, _
                    InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                    InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                    InvoiceItemSumVatLTLCorrection)
                UpdateInvoiceItemUnitValue = True

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                InvoiceItemAccount = _LongTermSale.AccountCorresponding
                UpdateInvoiceItemAccount = True
                InvoiceItemAmount = _LongTermSale.AmmountChange
                UpdateInvoiceItemAmount = True

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                InvoiceItemAmount = 1
                UpdateInvoiceItemAmount = True
                ReverseCalculateValues(1, _LongTermAcquisitionValueChange.TotalValueChange, _
                    _LongTermAcquisitionValueChange.TotalValueChange, InvoiceMadeItem.GetCurrencyRate(0), _
                    InvoiceMadeItem.IncludeVatInObject, InvoiceMadeItem.VatRate, False, _
                    InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                    InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                    InvoiceItemSumVatLTLCorrection)
                UpdateInvoiceItemUnitValue = True

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then

                InvoiceItemAmount = -CRound(_GoodsAcquisition.Ammount, ROUNDAMOUNTINVOICERECEIVED)
                UpdateInvoiceItemAmount = True
                ReverseCalculateValues(InvoiceItemAmount, _GoodsAcquisition.UnitCost, _
                    -_GoodsAcquisition.TotalCost, InvoiceMadeItem.GetCurrencyRate(0), _
                    InvoiceMadeItem.IncludeVatInObject, InvoiceMadeItem.VatRate, False, _
                    InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                    InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                    InvoiceItemSumVatLTLCorrection)
                UpdateInvoiceItemUnitValue = True

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                ' nothing to do additional costs can only be made by invoice received

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                InvoiceItemAccount = _GoodsDiscount.AccountGoodsNetCosts
                UpdateInvoiceItemAccount = True
                InvoiceItemAmount = 1
                UpdateInvoiceItemAmount = True
                ReverseCalculateValues(1, _GoodsDiscount.TotalValueChange, _
                    _GoodsDiscount.TotalValueChange, InvoiceMadeItem.GetCurrencyRate(0), _
                    InvoiceMadeItem.IncludeVatInObject, InvoiceMadeItem.VatRate, False, _
                    InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                    InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                    InvoiceItemSumVatLTLCorrection)
                UpdateInvoiceItemUnitValue = True

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then

                InvoiceItemAmount = CRound(_GoodsTransfer.Amount, ROUNDAMOUNTINVOICERECEIVED)
                UpdateInvoiceItemAmount = True

            End If

            UpdateDelegate.Invoke(InvoiceItemContent, UpdateInvoiceItemContent, _
                InvoiceItemMeasureUnit, UpdateInvoiceItemMeasureUnit, InvoiceItemAccount, _
                UpdateInvoiceItemAccount, InvoiceItemAmount, UpdateInvoiceItemAmount, _
                InvoiceItemUnitValue, InvoiceItemSumCorrection, InvoiceItemSumVatCorrection, _
                InvoiceItemUnitValueLTLCorrection, InvoiceItemSumLTLCorrection, _
                InvoiceItemSumVatLTLCorrection, UpdateInvoiceItemUnitValue)

        End Sub

        Private Sub ReverseCalculateValues(ByVal nAmount As Double, ByVal nUnitValueLTL As Double, _
            ByVal nSumLTL As Double, ByVal nCurrencyRate As Double, ByVal inclVat As Boolean, _
            ByVal nVatRate As Double, ByVal ItemIsInvoiceReceived As Boolean, ByRef InvoiceItemUnitValue As Double, _
            ByRef InvoiceItemSumCorrection As Integer, ByRef InvoiceItemSumVatCorrection As Integer, _
            ByRef InvoiceItemUnitValueLTLCorrection As Integer, ByRef InvoiceItemSumLTLCorrection As Integer, _
            ByRef InvoiceItemSumVatLTLCorrection As Integer)

            Dim UnitRound As Integer
            If ItemIsInvoiceReceived Then
                nAmount = CRound(nAmount, ROUNDAMOUNTINVOICERECEIVED)
                UnitRound = ROUNDUNITINVOICERECEIVED
            Else
                nAmount = CRound(nAmount, ROUNDAMOUNTINVOICEMADE)
                UnitRound = ROUNDUNITINVOICEMADE
            End If

            ' if no unit value -> nothing to calculate
            If CRound(nUnitValueLTL, UnitRound) = 0 Then Exit Sub

            If Not CRound(nCurrencyRate, 6) > 0 Then nCurrencyRate = 1

            ' if no amount -> only calculate unit value
            If CRound(nAmount, Math.Max(ROUNDAMOUNTINVOICERECEIVED, ROUNDAMOUNTINVOICEMADE)) = 0 Then

                InvoiceItemUnitValue = CRound(nUnitValueLTL / nCurrencyRate, UnitRound)
                InvoiceItemUnitValueLTLCorrection = Convert.ToInt32(CRound(nUnitValueLTL _
                    - CRound(InvoiceItemUnitValue * nCurrencyRate, UnitRound)) * 100)

                Exit Sub

            End If

            Dim SumLTL, SumVatLTL, UnitValueLTL, _Sum, _SumVat As Double

            If inclVat AndAlso nVatRate > 0 Then
                SumLTL = CRound(nSumLTL * 100 / (nVatRate + 100))
                SumVatLTL = CRound(nSumLTL - SumLTL)
                UnitValueLTL = CRound(nUnitValueLTL - (SumVatLTL / nAmount), UnitRound)
            Else
                SumLTL = CRound(nSumLTL)
                SumVatLTL = 0
                UnitValueLTL = CRound(nUnitValueLTL, UnitRound)
            End If

            InvoiceItemUnitValue = CRound(UnitValueLTL / nCurrencyRate, UnitRound)
            InvoiceItemUnitValueLTLCorrection = Convert.ToInt32(CRound(UnitValueLTL _
                - CRound(InvoiceItemUnitValue * nCurrencyRate, ROUNDUNITINVOICERECEIVED)) * 100)

            _Sum = CRound(SumLTL / nCurrencyRate)
            InvoiceItemSumCorrection = Convert.ToInt32(CRound(_Sum - CRound(InvoiceItemUnitValue * nAmount)) * 100)
            InvoiceItemSumLTLCorrection = Convert.ToInt32(CRound(SumLTL - CRound(_Sum * nCurrencyRate)) * 100)

            If inclVat AndAlso nVatRate > 0 Then
                _SumVat = CRound(SumVatLTL / nCurrencyRate)
                InvoiceItemSumVatLTLCorrection = Convert.ToInt32(CRound(SumVatLTL - CRound(_SumVat * nCurrencyRate)) * 100)
                InvoiceItemSumVatCorrection = Convert.ToInt32(CRound(_SumVat - CRound(_Sum * nVatRate / 100)))
            End If

        End Sub

        Friend Sub CheckDataSync(ByVal ReceivedItem As InvoiceReceivedItem)

            ' sync is not important if financial data is not updated
            If Not ChronologyValidator.FinancialDataCanChange Then Exit Sub

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                If _LongTermAsset.AccountAcquisition <> ReceivedItem.AccountCosts Then _
                    Throw New Exception("Klaida. Nesutampa įsigijimo sąskaita eilutėje " _
                    & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                If CRound(_LongTermAsset.Ammount, ROUNDAMOUNTINVOICERECEIVED) <> _
                    CRound(ReceivedItem.Ammount, ROUNDAMOUNTINVOICERECEIVED) Then _
                    Throw New Exception("Klaida. Nesutampa įsigijimo kiekis eilutėje " _
                    & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                If ReceivedItem.IncludeVatInObject Then

                    If _LongTermAsset.AcquisitionAccountValue <> ReceivedItem.SumTotalLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo suma eilutėje " _
                        & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                    If CRound(_LongTermAsset.AcquisitionAccountValuePerUnit, Math.Min( _
                        ROUNDUNITINVOICERECEIVED, ROUNDUNITASSET)) <> _
                        CRound(ReceivedItem.UnitValueLTL + (ReceivedItem.SumVatLTL _
                        / ReceivedItem.Ammount), Math.Min(ROUNDUNITINVOICERECEIVED, ROUNDUNITASSET)) Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo vnt. kaina eilutėje " _
                        & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                Else

                    If _LongTermAsset.AcquisitionAccountValue <> ReceivedItem.SumLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo suma eilutėje " _
                        & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                    If CRound(_LongTermAsset.AcquisitionAccountValuePerUnit, Math.Min( _
                        ROUNDUNITINVOICERECEIVED, ROUNDUNITASSET)) <> _
                        CRound(ReceivedItem.UnitValueLTL, Math.Min(ROUNDUNITINVOICERECEIVED, ROUNDUNITASSET)) Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo vnt. kaina eilutėje " _
                        & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                End If

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                If _LongTermSale.AccountCorresponding <> ReceivedItem.AccountCosts Then _
                    Throw New Exception("Klaida. Nesutampa sąnaudų sąskaita eilutėje " _
                    & "ir IT pardavimo objekte (" & _LongTermSale.AssetName & ").")

                If CRound(_LongTermSale.AmmountChange, ROUNDAMOUNTINVOICERECEIVED) <> _
                    -CRound(ReceivedItem.Ammount, ROUNDAMOUNTINVOICERECEIVED) Then _
                    Throw New Exception("Klaida. Nesutampa įsigijimo kiekis eilutėje " _
                    & "ir IT pardavimo objekte (" & _LongTermSale.AssetName & ").")

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                If ReceivedItem.IncludeVatInObject Then
                    If _LongTermAcquisitionValueChange.TotalValueChange <> ReceivedItem.SumTotalLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo savikainos bendro pokyčio " _
                        & "suma eilutėje ir IT savikainos pakeitimo objekte (" _
                        & _LongTermAcquisitionValueChange.AssetName & ").")
                Else
                    If _LongTermAcquisitionValueChange.TotalValueChange <> ReceivedItem.SumLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo savikainos bendro pokyčio " _
                        & "suma eilutėje ir IT savikainos pakeitimo objekte (" _
                        & _LongTermAcquisitionValueChange.AssetName & ").")
                End If

            ElseIf _Type = InvoiceAdapterType.Service Then

                ' nothing to do for service

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then

                If CRound(_GoodsAcquisition.Ammount, Math.Min(ROUNDAMOUNTINVOICERECEIVED, 6)) <> _
                    CRound(ReceivedItem.Ammount, Math.Min(ROUNDAMOUNTINVOICERECEIVED, 6)) Then _
                    Throw New Exception("Klaida. Nesutampa įsigijimo kiekis eilutėje " _
                    & "ir prekių įsigijimo objekte (" & _GoodsAcquisition.GoodsInfo.Name & ").")

                If ReceivedItem.IncludeVatInObject Then

                    If _GoodsAcquisition.TotalCost <> ReceivedItem.SumTotalLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo suma eilutėje " _
                        & "ir prekių įsigijimo objekte (" & _GoodsAcquisition.GoodsInfo.Name & ").")

                    If CRound(_GoodsAcquisition.UnitCost, Math.Min(ROUNDUNITINVOICERECEIVED, 6)) <> _
                        CRound(ReceivedItem.UnitValueLTL + (ReceivedItem.SumVatLTL _
                        / ReceivedItem.Ammount), Math.Min(ROUNDUNITINVOICERECEIVED, 6)) Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo vnt. kaina eilutėje " _
                        & "ir prekių įsigijimo objekte (" & _GoodsAcquisition.GoodsInfo.Name & ").")

                Else

                    If _GoodsAcquisition.TotalCost <> ReceivedItem.SumLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo suma eilutėje " _
                        & "ir prekių įsigijimo objekte (" & _GoodsAcquisition.GoodsInfo.Name & ").")

                    If CRound(_GoodsAcquisition.UnitCost, Math.Min(ROUNDUNITINVOICERECEIVED, 6)) <> _
                        CRound(ReceivedItem.UnitValueLTL, Math.Min(ROUNDUNITINVOICERECEIVED, 6)) Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo vnt. kaina eilutėje " _
                        & "ir prekių įsigijimo objekte (" & _GoodsAcquisition.GoodsInfo.Name & ").")

                End If

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                If ReceivedItem.IncludeVatInObject Then
                    If _GoodsAdditionalCosts.TotalValueChange <> ReceivedItem.SumTotalLTL Then _
                        Throw New Exception("Klaida. Nesutampa savikainos padidinimo suma eilutėje " _
                        & "ir prekių savikainos padidinimo objekte (" & _GoodsAdditionalCosts.GoodsInfo.Name & ")")
                Else
                    If _GoodsAdditionalCosts.TotalValueChange <> ReceivedItem.SumLTL Then _
                        Throw New Exception("Klaida. Nesutampa savikainos padidinimo suma eilutėje " _
                        & "ir prekių savikainos padidinimo objekte (" & _GoodsAdditionalCosts.GoodsInfo.Name & ")")
                End If

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                If ReceivedItem.IncludeVatInObject Then
                    If _GoodsDiscount.TotalValueChange <> -ReceivedItem.SumTotalLTL Then _
                        Throw New Exception("Klaida. Nesutampa nuolaidos suma eilutėje " _
                        & "ir prekių nuolaidos objekte (" & _GoodsDiscount.GoodsInfo.Name & ")")
                Else
                    If _GoodsDiscount.TotalValueChange <> -ReceivedItem.SumLTL Then _
                        Throw New Exception("Klaida. Nesutampa nuolaidos suma eilutėje " _
                        & "ir prekių nuolaidos objekte (" & _GoodsDiscount.GoodsInfo.Name & ")")
                End If

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then

                If CRound(_GoodsTransfer.Amount, Math.Min(ROUNDAMOUNTINVOICERECEIVED, 6)) <> _
                    -CRound(ReceivedItem.Ammount, Math.Min(ROUNDAMOUNTINVOICERECEIVED, 6)) Then _
                    Throw New Exception("Klaida. Nesutampa pardavimo kiekis eilutėje " _
                    & "ir prekių pardavimo objekte (" & _GoodsTransfer.GoodsInfo.Name & ").")

            End If

        End Sub

        Friend Sub CheckDataSync(ByVal MadeItem As InvoiceMadeItem)

            ' sync is not important if financial data is not updated
            If Not ChronologyValidator.FinancialDataCanChange Then Exit Sub

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then

                If _LongTermAsset.AccountAcquisition <> MadeItem.AccountIncome Then _
                    Throw New Exception("Klaida. Nesutampa įsigijimo sąskaita eilutėje " _
                    & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                If CRound(_LongTermAsset.Ammount, ROUNDAMOUNTINVOICEMADE) <> _
                    -CRound(MadeItem.Ammount, ROUNDAMOUNTINVOICEMADE) Then _
                    Throw New Exception("Klaida. Nesutampa įsigijimo kiekis eilutėje " _
                    & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                If MadeItem.IncludeVatInObject Then

                    If _LongTermAsset.AcquisitionAccountValue <> -MadeItem.SumTotalLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo suma eilutėje " _
                        & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                    If CRound(_LongTermAsset.AcquisitionAccountValuePerUnit, Math.Min( _
                        ROUNDUNITINVOICEMADE, ROUNDUNITASSET)) <> _
                        CRound(MadeItem.UnitValueLTL + (MadeItem.SumVatLTL _
                        / MadeItem.Ammount), Math.Min(ROUNDUNITINVOICEMADE, ROUNDUNITASSET)) Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo vnt. kaina eilutėje " _
                        & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                Else

                    If _LongTermAsset.AcquisitionAccountValue <> -MadeItem.SumLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo suma eilutėje " _
                        & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                    If CRound(_LongTermAsset.AcquisitionAccountValuePerUnit, Math.Min( _
                        ROUNDUNITINVOICEMADE, ROUNDUNITASSET)) <> _
                        CRound(MadeItem.UnitValueLTL, Math.Min(ROUNDUNITINVOICEMADE, ROUNDUNITASSET)) Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo vnt. kaina eilutėje " _
                        & "ir IT įsigijimo objekte (" & _LongTermAsset.Name & ").")

                End If

            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then

                If _LongTermSale.AccountCorresponding <> MadeItem.AccountIncome Then _
                    Throw New Exception("Klaida. Nesutampa pajamų sąskaita eilutėje " _
                    & "ir IT pardavimo objekte (" & _LongTermSale.AssetName & ").")

                If CRound(_LongTermSale.AmmountChange, ROUNDAMOUNTINVOICEMADE) <> _
                    CRound(MadeItem.Ammount, ROUNDAMOUNTINVOICEMADE) Then _
                    Throw New Exception("Klaida. Nesutampa įsigijimo kiekis eilutėje " _
                    & "ir IT pardavimo objekte (" & _LongTermSale.AssetName & ").")

            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then

                If MadeItem.IncludeVatInObject Then
                    If _LongTermAcquisitionValueChange.TotalValueChange <> -MadeItem.SumTotalLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo savikainos bendro pokyčio " _
                        & "suma eilutėje ir IT savikainos pakeitimo objekte (" _
                        & _LongTermAcquisitionValueChange.AssetName & ").")
                Else
                    If _LongTermAcquisitionValueChange.TotalValueChange <> -MadeItem.SumLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo savikainos bendro pokyčio " _
                        & "suma eilutėje ir IT savikainos pakeitimo objekte (" _
                        & _LongTermAcquisitionValueChange.AssetName & ").")
                End If

            ElseIf _Type = InvoiceAdapterType.Service Then

                ' nothing to do for service

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then

                If CRound(_GoodsAcquisition.Ammount, Math.Min(ROUNDAMOUNTINVOICEMADE, 6)) <> _
                    -CRound(MadeItem.Ammount, Math.Min(ROUNDAMOUNTINVOICEMADE, 6)) Then _
                    Throw New Exception("Klaida. Nesutampa įsigijimo kiekis eilutėje " _
                    & "ir prekių įsigijimo objekte (" & _GoodsAcquisition.GoodsInfo.Name & ").")

                If MadeItem.IncludeVatInObject Then

                    If _GoodsAcquisition.TotalCost <> -MadeItem.SumTotalLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo suma eilutėje " _
                        & "ir prekių įsigijimo objekte (" & _GoodsAcquisition.GoodsInfo.Name & ").")

                    If CRound(_GoodsAcquisition.UnitCost, Math.Min(ROUNDUNITINVOICEMADE, 6)) <> _
                        CRound(MadeItem.UnitValueLTL + (MadeItem.SumVatLTL _
                        / MadeItem.Ammount), Math.Min(ROUNDUNITINVOICEMADE, 6)) Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo vnt. kaina eilutėje " _
                        & "ir prekių įsigijimo objekte (" & _GoodsAcquisition.GoodsInfo.Name & ").")

                Else

                    If _GoodsAcquisition.TotalCost <> -MadeItem.SumLTL Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo suma eilutėje " _
                        & "ir prekių įsigijimo objekte (" & _GoodsAcquisition.GoodsInfo.Name & ").")

                    If CRound(_GoodsAcquisition.UnitCost, Math.Min(ROUNDUNITINVOICEMADE, 6)) <> _
                        CRound(MadeItem.UnitValueLTL, Math.Min(ROUNDUNITINVOICEMADE, 6)) Then _
                        Throw New Exception("Klaida. Nesutampa įsigijimo vnt. kaina eilutėje " _
                        & "ir prekių įsigijimo objekte (" & _GoodsAcquisition.GoodsInfo.Name & ").")

                End If

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then

                Throw New Exception("Klaida. Prekių savikainos padidinimo objekto negali būti " _
                    & "išrašomos sąskaitos faktūros eilutėje (" & _GoodsAdditionalCosts.GoodsInfo.Name & ").")

            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then

                If MadeItem.IncludeVatInObject Then
                    If _GoodsDiscount.TotalValueChange <> MadeItem.SumTotalLTL Then _
                        Throw New Exception("Klaida. Nesutampa nuolaidos suma eilutėje " _
                        & "ir prekių nuolaidos objekte (" & _GoodsDiscount.GoodsInfo.Name & ")")
                Else
                    If _GoodsDiscount.TotalValueChange <> MadeItem.SumLTL Then _
                        Throw New Exception("Klaida. Nesutampa nuolaidos suma eilutėje " _
                        & "ir prekių nuolaidos objekte (" & _GoodsDiscount.GoodsInfo.Name & ")")
                End If

            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then

                If CRound(_GoodsTransfer.Amount, Math.Min(ROUNDAMOUNTINVOICERECEIVED, 6)) <> _
                    CRound(MadeItem.Ammount, Math.Min(ROUNDAMOUNTINVOICERECEIVED, 6)) Then _
                    Throw New Exception("Klaida. Nesutampa pardavimo kiekis eilutėje " _
                    & "ir prekių pardavimo objekte (" & _GoodsTransfer.GoodsInfo.Name & ").")

            End If

        End Sub



        Friend Function VatIsHandledOnRequest() As Boolean
            Return (_Type = InvoiceAdapterType.LongTermAssetPurchase _
                OrElse _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange _
                OrElse _Type = InvoiceAdapterType.GoodsAcquisition _
                OrElse _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts _
                OrElse _Type = InvoiceAdapterType.GoodsConsignmentDiscount)
        End Function

        Friend Function DiscountIsAllowed() As Boolean
            Return _Type = InvoiceAdapterType.GoodsTransfer OrElse _
                _Type = InvoiceAdapterType.LongTermAssetSale OrElse _
                _Type = InvoiceAdapterType.Service
        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                Return "Ilgalaikio turto įsigijimas"
            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                Return "Ilgalaikio turto perleidimas (" & _LongTermSale.AssetName & ")"
            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                Return "Ilgalaikio turto įsigijimo savikainos pokytis (" _
                    & _LongTermAcquisitionValueChange.AssetName & ")"
            ElseIf _Type = InvoiceAdapterType.Service Then
                Return "Paslauga: " & _Service.ToString
            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                Return "Prekių įsigijimas: " & _GoodsAcquisition.GoodsInfo.Name
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                Return "Prekių savikainos padidinimas: " & _GoodsAdditionalCosts.GoodsInfo.Name
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                Return "Gauta nuolaida prekėms: " & _GoodsDiscount.GoodsInfo.Name
            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                Return "Prekių perleidimas: " & _GoodsTransfer.GoodsInfo.Name
            Else
                Return "Nenustatyta operacija"
            End If
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

        End Sub

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

        Public Shared Function CanGetObject() As Boolean
            Return InvoiceMade.CanAddObject OrElse InvoiceReceived.CanAddObject
        End Function

#End Region

#Region " Factory Methods "

        Public Shared Function NewInvoiceAttachedObject(ByVal nType As InvoiceAdapterType, _
            ByVal AttachedObjectParentID As Integer, ByVal WarehouseID As Integer, _
            ByVal parentChronologyValidator As IChronologicValidator) As InvoiceAttachedObject
            Return DataPortal.Create(Of InvoiceAttachedObject) _
                (New Criteria(nType, AttachedObjectParentID, WarehouseID, parentChronologyValidator))
        End Function

        Friend Shared Function NewInvoiceAttachedObject(ByVal nService As ServiceInfo, _
            ByVal parentChronologyValidator As IChronologicValidator) As InvoiceAttachedObject
            Return New InvoiceAttachedObject(nService, parentChronologyValidator)
        End Function

        Friend Shared Function GetInvoiceAttachedObject(ByVal nType As InvoiceAdapterType, _
            ByVal nAttachedObjectID As Integer, ByVal parentChronologyValidator As IChronologicValidator) As InvoiceAttachedObject
            Return New InvoiceAttachedObject(nType, nAttachedObjectID, parentChronologyValidator)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub

        Private Sub New(ByVal nService As ServiceInfo, ByVal parentChronologyValidator As IChronologicValidator)
            MarkAsChild()
            CreateServiceAttachedObject(nService, parentChronologyValidator)
        End Sub

        Private Sub New(ByVal nType As InvoiceAdapterType, ByVal nAttachedObjectID As Integer, _
            ByVal parentChronologyValidator As IChronologicValidator)
            MarkAsChild()
            Fetch(nType, nAttachedObjectID, parentChronologyValidator)
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private _Type As InvoiceAdapterType
            Private _ID As Integer
            Private _WarehouseID As Integer
            Private _ParentChronologyValidator As IChronologicValidator
            Public ReadOnly Property Type() As InvoiceAdapterType
                Get
                    Return _Type
                End Get
            End Property
            Public ReadOnly Property ID() As Integer
                Get
                    Return _ID
                End Get
            End Property
            Public ReadOnly Property WarehouseID() As Integer
                Get
                    Return _WarehouseID
                End Get
            End Property
            Public ReadOnly Property ParentChronologyValidator() As IChronologicValidator
                Get
                    Return _ParentChronologyValidator
                End Get
            End Property
            Public Sub New(ByVal nType As InvoiceAdapterType, ByVal nid As Integer, _
                ByVal nWarehouseID As Integer, ByVal nParentChronologyValidator As IChronologicValidator)
                _Type = nType
                _ID = nid
                _WarehouseID = nWarehouseID
                _ParentChronologyValidator = nParentChronologyValidator
            End Sub
        End Class


        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka naujiems duomenims įtraukti.")
            Create(criteria.Type, criteria.ID, criteria.WarehouseID, criteria.ParentChronologyValidator)
        End Sub


        Private Sub CreateServiceAttachedObject(ByVal nService As ServiceInfo, _
            ByVal parentChronologyValidator As IChronologicValidator)

            If nService Is Nothing OrElse Not nService.ID > 0 Then Throw New ArgumentException( _
                "Klaida. Nepriskirta paslauga.")
            _Type = InvoiceAdapterType.Service
            _Service = nService
            _ID = nService.ID
            _ChronologyValidator = parentChronologyValidator
            If _ChronologyValidator Is Nothing Then _ChronologyValidator = _
                EmptyChronologicValidator.NewEmptyChronologicValidator("Paslauga", Nothing)

            ValidationRules.CheckRules()

        End Sub

        Private Sub Create(ByVal nType As InvoiceAdapterType, _
            ByVal AttachedObjectParentID As Integer, ByVal WarehouseID As Integer, _
            ByVal parentChronologyValidator As IChronologicValidator)

            If nType = InvoiceAdapterType.LongTermAssetPurchase Then
                _LongTermAsset = Assets.LongTermAsset.NewLongTermAssetChild(parentChronologyValidator)
            ElseIf nType = InvoiceAdapterType.LongTermAssetSale Then
                _LongTermSale = Assets.LongTermAssetOperation.NewLongTermAssetOperationChild( _
                    AttachedObjectParentID, Assets.LtaOperationType.Transfer, parentChronologyValidator)
            ElseIf nType = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                _LongTermAcquisitionValueChange = Assets.LongTermAssetOperation. _
                    NewLongTermAssetOperationChild(AttachedObjectParentID, _
                    Assets.LtaOperationType.AcquisitionValueIncrease, parentChronologyValidator)
            ElseIf nType = InvoiceAdapterType.Service Then
                Throw New ArgumentException("Klaida. Paslaugoms naudotinas kitas overloadas.")
            ElseIf nType = InvoiceAdapterType.GoodsAcquisition Then
                _GoodsAcquisition = Goods.GoodsOperationAcquisition. _
                    NewGoodsOperationAcquisitionChild(AttachedObjectParentID)
            ElseIf nType = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                _GoodsAdditionalCosts = Goods.GoodsOperationAdditionalCosts. _
                    NewGoodsOperationAdditionalCostsChild(AttachedObjectParentID, WarehouseID)
            ElseIf nType = InvoiceAdapterType.GoodsConsignmentDiscount Then
                _GoodsDiscount = Goods.GoodsOperationDiscount. _
                    NewGoodsOperationDiscountChild(AttachedObjectParentID, WarehouseID)
            ElseIf nType = InvoiceAdapterType.GoodsTransfer Then
                _GoodsTransfer = Goods.GoodsOperationTransfer. _
                    NewGoodsOperationTransferChild(AttachedObjectParentID, WarehouseID)
            Else
                Throw New ArgumentException("Klaida. Objekto tipas '" & nType.ToString & "' nežinomas.")
            End If
            _Type = nType

            MarkNew()

            ValidationRules.CheckRules()

        End Sub

        Private Sub Fetch(ByVal nType As InvoiceAdapterType, ByVal nAttachedObjectID As Integer, _
            ByVal parentChronologyValidator As IChronologicValidator)

            If nType = InvoiceAdapterType.LongTermAssetPurchase Then
                _LongTermAsset = Assets.LongTermAsset.GetLongTermAssetChild(nAttachedObjectID, _
                    parentChronologyValidator)
            ElseIf nType = InvoiceAdapterType.LongTermAssetSale Then
                _LongTermSale = Assets.LongTermAssetOperation.GetLongTermAssetOperationChild( _
                    nAttachedObjectID, parentChronologyValidator)
            ElseIf nType = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                _LongTermAcquisitionValueChange = Assets.LongTermAssetOperation. _
                    GetLongTermAssetOperationChild(nAttachedObjectID, parentChronologyValidator)
            ElseIf nType = InvoiceAdapterType.Service Then
                _Service = HelperLists.ServiceInfo.GetServiceInfo(nAttachedObjectID)
            ElseIf nType = InvoiceAdapterType.GoodsAcquisition Then
                _GoodsAcquisition = Goods.GoodsOperationAcquisition.GetGoodsOperationAcquisitionChild(nAttachedObjectID)
            ElseIf nType = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                _GoodsAdditionalCosts = Goods.GoodsOperationAdditionalCosts.GetGoodsOperationAdditionalCostsChild(nAttachedObjectID)
            ElseIf nType = InvoiceAdapterType.GoodsConsignmentDiscount Then
                _GoodsDiscount = Goods.GoodsOperationDiscount.GetGoodsOperationDiscountChild(nAttachedObjectID)
            ElseIf nType = InvoiceAdapterType.GoodsTransfer Then
                _GoodsTransfer = Goods.GoodsOperationTransfer.GetGoodsOperationTransferChild(nAttachedObjectID)
            Else
                Throw New ArgumentException("Klaida. Objekto tipas '" & nType.ToString & "' nežinomas.")
            End If

            _ID = nAttachedObjectID
            _Type = nType

            MarkOld()

        End Sub

        Friend Sub Update(ByVal InvoiceID As Integer, ByVal InvoiceDate As Date, _
            ByVal InvoiceContent As String, ByVal InvoiceDocNo As String, ByVal FinancialDataReadOnly As Boolean)

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                _LongTermAsset.SaveChild(InvoiceID, FinancialDataReadOnly)
                If IsNew Then _ID = _LongTermAsset.ID
            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                _LongTermSale.SaveServerSide(InvoiceID, FinancialDataReadOnly)
                If IsNew Then _ID = _LongTermSale.ID
            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                _LongTermAcquisitionValueChange.SaveServerSide(InvoiceID, FinancialDataReadOnly)
                If IsNew Then _ID = _LongTermAcquisitionValueChange.ID
            ElseIf _Type = InvoiceAdapterType.Service Then
                ' nothing to do for service
            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                _GoodsAcquisition.SaveChild(InvoiceID, 0, InvoiceDocNo, InvoiceContent, _
                    True, FinancialDataReadOnly)
                If IsNew Then _ID = _GoodsAcquisition.ID
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                _GoodsAdditionalCosts.SaveChild(InvoiceID, InvoiceDate, InvoiceContent, _
                    InvoiceDocNo, FinancialDataReadOnly)
                If IsNew Then _ID = _GoodsAdditionalCosts.ID
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                _GoodsDiscount.SaveChild(InvoiceID, InvoiceDate, InvoiceContent, _
                    InvoiceDocNo, FinancialDataReadOnly)
                If IsNew Then _ID = _GoodsDiscount.ID
            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                _GoodsTransfer.SaveChild(InvoiceID, 0, InvoiceContent, InvoiceDocNo, _
                    True, False, FinancialDataReadOnly)
                If IsNew Then _ID = _GoodsTransfer.ID

            Else
                Throw New ArgumentException("Klaida. Objekto tipas '" & _Type.ToString & "' nežinomas.")
            End If

            MarkOld()

        End Sub

        Friend Sub DeleteSelf()

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                Assets.LongTermAsset.DeleteChild(_ID)
            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                Assets.LongTermAssetOperation.DeleteLongTermAssetOperationChild(_ID)
            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                Assets.LongTermAssetOperation.DeleteLongTermAssetOperationChild(_ID)
            ElseIf _Type = InvoiceAdapterType.Service Then
                ' nothing to do for service
            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                _GoodsAcquisition.DeleteGoodsOperationAcquisitionChild()
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                _GoodsAdditionalCosts.DeleteGoodsOperationAdditionalCostsChild()
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                _GoodsDiscount.DeleteGoodsOperationDiscountChild()
            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                _GoodsTransfer.DeleteGoodsOperationTransferChild()
            Else
                Throw New ArgumentException("Klaida. Objekto tipas '" & _Type.ToString & "' nežinomas.")
            End If

            MarkNew()

        End Sub


        Friend Sub CheckIfCanUpdate(ByVal FinancialDataReadOnly As Boolean, _
            ByVal parentChronologyValidator As IChronologicValidator)

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                _LongTermAsset.CheckIfCanUpdate()
            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                _LongTermSale.CheckAllRulesChild(parentChronologyValidator)
            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                _LongTermAcquisitionValueChange.CheckAllRulesChild(parentChronologyValidator)
            ElseIf _Type = InvoiceAdapterType.Service Then
                ' nothing to do for service
            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                If _GoodsAcquisition.OperationLimitations.FinancialDataCanChange _
                    AndAlso Not FinancialDataReadOnly Then _GoodsAcquisition.GetConsignment()
                _GoodsAcquisition.CheckIfCanUpdate(Nothing, True)
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                _GoodsAdditionalCosts.CheckIfCanUpdate()
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                _GoodsDiscount.CheckIfCanUpdate()
            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                If _GoodsTransfer.OperationLimitations.FinancialDataCanChange _
                    AndAlso Not FinancialDataReadOnly Then _GoodsTransfer.GetDiscardList()
                _GoodsTransfer.CheckIfCanUpdate(Nothing, True)
            Else
                Throw New ArgumentException("Klaida. Objekto tipas '" & _Type.ToString & "' nežinomas.")
            End If

        End Sub

        Friend Sub CheckIfCanDelete(ByVal parentChronologyValidator As IChronologicValidator)

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                _LongTermAsset.CheckIfCanUpdate()
            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                _LongTermSale.CheckIfCanDeleteChild(parentChronologyValidator)
            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                _LongTermAcquisitionValueChange.CheckIfCanDeleteChild(parentChronologyValidator)
            ElseIf _Type = InvoiceAdapterType.Service Then
                ' nothing to do for service
            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                _GoodsAcquisition.CheckIfCanDelete(Nothing, True)
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                _GoodsAdditionalCosts.CheckIfCanDelete()
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                _GoodsDiscount.CheckIfCanDelete()
            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                _GoodsTransfer.CheckIfCanDelete(Nothing, True)
            Else
                Throw New ArgumentException("Klaida. Objekto tipas '" & _Type.ToString & "' nežinomas.")
            End If

        End Sub


        Friend Function GetBookEntryList(ByVal SalesSum As Double, _
            ByVal CorrespondingAccount As Long, ByVal IsSale As Boolean) As BookEntryInternalList

            If CRound(SalesSum) < 0 Then SalesSum = -SalesSum

            If _Type = InvoiceAdapterType.LongTermAssetPurchase Then
                Return _LongTermAsset.GetTotalBookEntryList
            ElseIf _Type = InvoiceAdapterType.LongTermAssetSale Then
                Return _LongTermSale.GetTotalBookEntryListForTransfer(SalesSum)
            ElseIf _Type = InvoiceAdapterType.LongTermAssetAcquisitionValueChange Then
                Return _LongTermAcquisitionValueChange.GetTotalBookEntryListForAcquisitionValueChange()
            ElseIf _Type = InvoiceAdapterType.Service Then

                Dim result As BookEntryInternalList = BookEntryInternalList. _
                    NewBookEntryInternalList(BookEntryType.Debetas)

                If IsSale Then

                    Dim CreditBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
                    CreditBookEntry.Account = CorrespondingAccount
                    CreditBookEntry.Ammount = CRound(SalesSum)
                    result.Add(CreditBookEntry)

                Else

                    Dim DebitBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
                    DebitBookEntry.Account = CorrespondingAccount
                    DebitBookEntry.Ammount = CRound(SalesSum)
                    result.Add(DebitBookEntry)

                End If

                Return result

            ElseIf _Type = InvoiceAdapterType.GoodsAcquisition Then
                Return _GoodsAcquisition.GetTotalBookEntryList
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentAdditionalCosts Then
                Return _GoodsAdditionalCosts.GetTotalBookEntryList
            ElseIf _Type = InvoiceAdapterType.GoodsConsignmentDiscount Then
                Return _GoodsDiscount.GetTotalBookEntryList
            ElseIf _Type = InvoiceAdapterType.GoodsTransfer Then
                Return _GoodsTransfer.GetTotalBookEntryList(CorrespondingAccount, SalesSum)
            Else
                Throw New ArgumentException("Klaida. Objekto tipas '" & _Type.ToString & "' nežinomas.")
            End If

        End Function

#End Region

    End Class

End Namespace