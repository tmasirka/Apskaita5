Namespace Assets

    <Serializable()> _
Public Class LongTermAssetOperationChild
        Inherits BusinessBase(Of LongTermAssetOperationChild)

#Region " Business Methods "

        Private _AssetID As Integer
        Private _AssetName As String
        Private _AssetMeasureUnit As String
        Private _AccumulatedAmortizationBeforeAquisition As Double
        Private _AccumulatedAmortizationBeforeAquisitionRevaluedPortion As Double
        Private _AssetAcquiredAccount As Integer
        Private _AssetContraryAccount As Integer
        Private _AssetValueDecreaseAccount As Integer
        Private _AssetValueIncreaseAccount As Integer
        Private _AssetValueIncreaseAmortizationAccount As Integer
        Private _AssetDateAcquired As Date
        Private _AssetAquisitionOpID As Integer
        Private _AssetLiquidationValue As Double

        Private _ChronologyValidator As OperationChronologicValidator

        Private _CurrentAcquisitionAccountValue As Double
        Private _CurrentAcquisitionAccountValuePerUnit As Double
        Private _CurrentAmortizationAccountValue As Double
        Private _CurrentAmortizationAccountValuePerUnit As Double
        Private _CurrentValueDecreaseAccountValue As Double
        Private _CurrentValueDecreaseAccountValuePerUnit As Double
        Private _CurrentValueIncreaseAccountValue As Double
        Private _CurrentValueIncreaseAccountValuePerUnit As Double
        Private _CurrentValueIncreaseAmortizationAccountValue As Double
        Private _CurrentValueIncreaseAmortizationAccountValuePerUnit As Double
        Private _CurrentAssetAmmount As Integer
        Private _CurrentAssetValue As Double
        Private _CurrentAssetValuePerUnit As Double
        Private _CurrentAssetValueRevaluedPortion As Double
        Private _CurrentAssetValueRevaluedPortionPerUnit As Double

        Private _CurrentUsageTermMonths As Integer
        Private _CurrentAmortizationPeriod As Integer
        Private _CurrentUsageStatus As Boolean

        Private _IsChecked As Boolean = False
        Private _ID As Integer = -1
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now
        Private _Type As LtaOperationType = LtaOperationType.Discard
        Private _Date As Date = Today.Date
        Private _OldDate As Date = Today.Date
        Private _Content As String = ""
        Private _AccountCorresponding As Integer = 0
        Private _ActNumber As Integer = 0
        Private _JournalEntryID As Integer = -1
        Private _JournalEntryType As DocumentType = DocumentType.None
        Private _JournalEntryContent As String = ""
        Private _JournalEntryDocNumber As String = ""
        Private _UnitValueChange As Double = 0
        Private _TotalValueChange As Double = 0
        Private _AmmountChange As Integer = 0
        Private _RevaluedPortionUnitValueChange As Double = 0
        Private _RevaluedPortionTotalValueChange As Double = 0
        Private _AmortizationCalculations As String = ""
        Private _AmortizationCalculatedForMonths As Integer = 0

        Public ReadOnly Property AssetID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetID
            End Get
        End Property

        Public ReadOnly Property AssetName() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetName
            End Get
        End Property

        Public ReadOnly Property AssetMeasureUnit() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetMeasureUnit
            End Get
        End Property

        Public ReadOnly Property AccumulatedAmortizationBeforeAquisition() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccumulatedAmortizationBeforeAquisition
            End Get
        End Property

        Public ReadOnly Property AccumulatedAmortizationBeforeAquisitionRevaluedPortion() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccumulatedAmortizationBeforeAquisitionRevaluedPortion
            End Get
        End Property

        Public ReadOnly Property AssetAcquiredAccount() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetAcquiredAccount
            End Get
        End Property

        Public ReadOnly Property AssetContraryAccount() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetContraryAccount
            End Get
        End Property

        Public ReadOnly Property AssetValueDecreaseAccount() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetValueDecreaseAccount
            End Get
        End Property

        Public ReadOnly Property AssetValueIncreaseAccount() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetValueIncreaseAccount
            End Get
        End Property

        Public ReadOnly Property AssetValueIncreaseAmortizationAccount() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetValueIncreaseAmortizationAccount
            End Get
        End Property

        Public ReadOnly Property AssetDateAcquired() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetDateAcquired
            End Get
        End Property

        Public ReadOnly Property AssetAquisitionOpID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetAquisitionOpID
            End Get
        End Property

        Public ReadOnly Property AssetLiquidationValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AssetLiquidationValue
            End Get
        End Property

        Public ReadOnly Property ChronologyValidator() As OperationChronologicValidator
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ChronologyValidator
            End Get
        End Property


        Public ReadOnly Property CurrentAcquisitionAccountValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentAcquisitionAccountValue)
            End Get
        End Property

        Public ReadOnly Property CurrentAcquisitionAccountValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            End Get
        End Property

        Public ReadOnly Property CurrentAmortizationAccountValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentAmortizationAccountValue)
            End Get
        End Property

        Public ReadOnly Property CurrentAmortizationAccountValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            End Get
        End Property

        Public ReadOnly Property CurrentValueDecreaseAccountValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentValueDecreaseAccountValue)
            End Get
        End Property

        Public ReadOnly Property CurrentValueDecreaseAccountValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            End Get
        End Property

        Public ReadOnly Property CurrentValueIncreaseAccountValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentValueIncreaseAccountValue)
            End Get
        End Property

        Public ReadOnly Property CurrentValueIncreaseAccountValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            End Get
        End Property

        Public ReadOnly Property CurrentValueIncreaseAmortizationAccountValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentValueIncreaseAmortizationAccountValue)
            End Get
        End Property

        Public ReadOnly Property CurrentValueIncreaseAmortizationAccountValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            End Get
        End Property

        Public ReadOnly Property CurrentAssetAmmount() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CurrentAssetAmmount
            End Get
        End Property

        Public ReadOnly Property CurrentAssetValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentAssetValue)
            End Get
        End Property

        Public ReadOnly Property CurrentAssetValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentAssetValuePerUnit, ROUNDUNITASSET)
            End Get
        End Property

        Public ReadOnly Property CurrentAssetValueRevaluedPortion() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentAssetValueRevaluedPortion)
            End Get
        End Property

        Public ReadOnly Property CurrentAssetValueRevaluedPortionPerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            End Get
        End Property



        Public ReadOnly Property CurrentUsageTermMonths() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CurrentUsageTermMonths
            End Get
        End Property

        Public ReadOnly Property CurrentAmortizationPeriod() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CurrentAmortizationPeriod
            End Get
        End Property

        Public ReadOnly Property CurrentUsageStatus() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CurrentUsageStatus
            End Get
        End Property


        Public Property IsChecked() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsChecked
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _IsChecked <> value Then
                    _IsChecked = value
                    PropertyHasChanged()
                    ValidationRules.CheckRules()
                End If
            End Set
        End Property

        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property InsertDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _InsertDate
            End Get
        End Property

        Public ReadOnly Property UpdateDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UpdateDate
            End Get
        End Property

        Public Property [Type]() As LtaOperationType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Friend Set(ByVal value As LtaOperationType)
                CanWriteProperty(True)
                If _Type <> value Then

                    If Not IsNew Then Throw New Exception("Klaida. Įtrauktos į duomenų bazę " & _
                        "operacijos tipas negali būti keičiamas.")

                    _Type = value
                    PropertyHasChanged()
                    OnTypeChanged()

                End If
            End Set
        End Property

        Public Property [Date]() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Date
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Friend Set(ByVal value As Date)
                CanWriteProperty(True)
                If _Date <> value Then
                    _Date = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public ReadOnly Property OldDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OldDate
            End Get
        End Property

        Public Property Content() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Content
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Friend Set(ByVal value As String)
                CanWriteProperty(True)
                If _Content.Trim <> value.Trim Then
                    _Content = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property AccountCorresponding() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountCorresponding
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Friend Set(ByVal value As Integer)
                CanWriteProperty(True)
                If _AccountCorresponding <> value Then
                    _AccountCorresponding = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property ActNumber() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ActNumber
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Friend Set(ByVal value As Integer)
                CanWriteProperty(True)
                If _ActNumber <> value Then
                    _ActNumber = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public ReadOnly Property JournalEntryID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryID
            End Get
        End Property

        Public ReadOnly Property JournalEntryType() As DocumentType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryType
            End Get
        End Property

        Public ReadOnly Property JournalEntryContent() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryContent
            End Get
        End Property

        Public ReadOnly Property JournalEntryDocNumber() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryDocNumber.Trim
            End Get
        End Property

        Public Property UnitValueChange() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitValueChange, ROUNDUNITASSET)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If CRound(_UnitValueChange, ROUNDUNITASSET) <> CRound(value, ROUNDUNITASSET) AndAlso _
                    (_Type = LtaOperationType.AcquisitionValueIncrease OrElse _
                    _Type = LtaOperationType.Amortization OrElse _Type = LtaOperationType.ValueChange) Then

                    If Not _ChronologyValidator.FinancialDataCanChange Then Exit Property

                    _UnitValueChange = CRound(value, ROUNDUNITASSET)
                    _TotalValueChange = CRound(_UnitValueChange * _CurrentAssetAmmount, 2)
                    PropertyHasChanged()
                    PropertyHasChanged("TotalValueChange")

                    If _Type = LtaOperationType.ValueChange Then
                        _RevaluedPortionTotalValueChange = CRound(value * _CurrentAssetAmmount)
                        _RevaluedPortionUnitValueChange = CRound(value, ROUNDUNITASSET)
                        PropertyHasChanged("RevaluedPortionUnitValueChange")
                        PropertyHasChanged("RevaluedPortionTotalValueChange")
                    End If

                    AfterOperationPropertiesChanged()

                End If
            End Set
        End Property

        Public Property TotalValueChange() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalValueChange)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If CRound(_TotalValueChange) <> CRound(value) AndAlso _
                    _Type <> LtaOperationType.AccountChange AndAlso _
                    _Type <> LtaOperationType.AmortizationPeriod AndAlso _
                    _Type <> LtaOperationType.UsingEnd AndAlso _Type <> LtaOperationType.UsingStart Then

                    If Not _ChronologyValidator.FinancialDataCanChange Then Exit Property

                    _TotalValueChange = CRound(value)
                    PropertyHasChanged()

                    If _Type = LtaOperationType.ValueChange Then
                        _RevaluedPortionTotalValueChange = CRound(value)
                        PropertyHasChanged("RevaluedPortionTotalValueChange")
                    End If

                    AfterOperationPropertiesChanged()

                End If
            End Set
        End Property

        Public Property AmmountChange() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AmmountChange
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If _AmmountChange <> value AndAlso (_Type = LtaOperationType.Discard OrElse _
                    _Type = LtaOperationType.Transfer) Then

                    If Not _ChronologyValidator.FinancialDataCanChange Then Exit Property

                    _AmmountChange = value

                    If (_CurrentAssetAmmount - _AmmountChange) < 1 Then
                        _TotalValueChange = -CRound(_CurrentAssetValue)
                        _RevaluedPortionTotalValueChange = -CRound(_CurrentAssetValueRevaluedPortion)
                    Else
                        _TotalValueChange = -CRound(_AmmountChange * _CurrentAssetValuePerUnit)
                        _RevaluedPortionTotalValueChange = -CRound( _
                            _CurrentAssetValueRevaluedPortionPerUnit * _AmmountChange)
                    End If

                    PropertyHasChanged()
                    PropertyHasChanged("TotalValueChange")
                    PropertyHasChanged("RevaluedPortionTotalValueChange")

                    AfterOperationPropertiesChanged()

                End If
            End Set
        End Property

        Public Property RevaluedPortionUnitValueChange() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_RevaluedPortionUnitValueChange, ROUNDUNITASSET)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If CRound(_RevaluedPortionUnitValueChange, ROUNDUNITASSET) <> CRound(value, ROUNDUNITASSET) _
                    AndAlso (_Type = LtaOperationType.Amortization OrElse _
                    _Type = LtaOperationType.ValueChange) Then

                    If Not _ChronologyValidator.FinancialDataCanChange Then Exit Property

                    _RevaluedPortionUnitValueChange = CRound(value, ROUNDUNITASSET)

                    PropertyHasChanged()

                    If _Type = LtaOperationType.ValueChange Then
                        _UnitValueChange = CRound(value, ROUNDUNITASSET)
                        PropertyHasChanged("UnitValueChange")
                    End If

                    AfterOperationPropertiesChanged()

                End If
            End Set
        End Property

        Public Property RevaluedPortionTotalValueChange() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _RevaluedPortionTotalValueChange
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If CRound(_RevaluedPortionTotalValueChange) <> CRound(value) _
                    AndAlso (_Type = LtaOperationType.Amortization OrElse _Type = LtaOperationType.Discard _
                    OrElse _Type = LtaOperationType.Transfer OrElse _Type = LtaOperationType.ValueChange) Then

                    If Not _ChronologyValidator.FinancialDataCanChange Then Exit Property

                    _RevaluedPortionTotalValueChange = CRound(value)

                    PropertyHasChanged()

                    If _Type = LtaOperationType.ValueChange Then
                        _TotalValueChange = CRound(value)
                        PropertyHasChanged("TotalValueChange")
                    End If

                    AfterOperationPropertiesChanged()

                End If
            End Set
        End Property

        Public Property AmortizationCalculations() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AmortizationCalculations
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If _AmortizationCalculations.Trim <> value.Trim AndAlso _
                    _Type = LtaOperationType.Amortization Then

                    If Not _ChronologyValidator.FinancialDataCanChange Then Exit Property

                    _AmortizationCalculations = value.Trim
                    PropertyHasChanged()

                End If
            End Set
        End Property

        Public ReadOnly Property AmortizationCalculatedForMonths() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AmortizationCalculatedForMonths
            End Get
        End Property



        Public ReadOnly Property AfterOperationAcquisitionAccountValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.Transfer OrElse _Type = LtaOperationType.Discard Then

                    If _AmmountChange < 1 Then
                        Return CRound(_CurrentAcquisitionAccountValue)
                    ElseIf _AmmountChange = _CurrentAssetAmmount Then
                        Return 0
                    Else
                        Return CRound(_CurrentAcquisitionAccountValue _
                            - CRound(_AmmountChange * _CurrentAcquisitionAccountValuePerUnit))
                    End If

                ElseIf _Type = LtaOperationType.AcquisitionValueIncrease Then

                    Return CRound(_CurrentAcquisitionAccountValue + _TotalValueChange _
                        - _RevaluedPortionTotalValueChange)

                Else

                    Return CRound(_CurrentAcquisitionAccountValue)

                End If
            End Get
        End Property

        Public ReadOnly Property AfterOperationAcquisitionAccountValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.AcquisitionValueIncrease Then

                    Return CRound(_CurrentAcquisitionAccountValuePerUnit + _UnitValueChange _
                        - _RevaluedPortionUnitValueChange, ROUNDUNITASSET)

                Else

                    Return CRound(_CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)

                End If
            End Get
        End Property

        Public ReadOnly Property AfterOperationAmortizationAccountValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.Transfer OrElse _Type = LtaOperationType.Discard Then

                    If _AmmountChange < 1 Then
                        Return CRound(_CurrentAmortizationAccountValue)
                    ElseIf _AmmountChange = _CurrentAssetAmmount Then
                        Return 0
                    Else
                        Return CRound(_CurrentAmortizationAccountValue _
                            - CRound(_AmmountChange * _CurrentAmortizationAccountValuePerUnit))
                    End If

                ElseIf _Type = LtaOperationType.Amortization Then

                    Return CRound(_CurrentAmortizationAccountValue - (_TotalValueChange _
                        - _RevaluedPortionTotalValueChange))

                Else

                    Return CRound(_CurrentAmortizationAccountValue)

                End If
            End Get
        End Property

        Public ReadOnly Property AfterOperationAmortizationAccountValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.Amortization Then

                    Return CRound(_CurrentAmortizationAccountValuePerUnit - (_UnitValueChange _
                        - _RevaluedPortionUnitValueChange), ROUNDUNITASSET)

                Else

                    Return CRound(_CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)

                End If
            End Get
        End Property

        Public ReadOnly Property AfterOperationValueDecreaseAccountValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.Transfer OrElse _Type = LtaOperationType.Discard Then

                    If _AmmountChange < 1 OrElse Not _CurrentValueDecreaseAccountValue > 0 Then
                        Return CRound(_CurrentValueDecreaseAccountValue)
                    ElseIf _AmmountChange = _CurrentAssetAmmount Then
                        Return 0
                    Else
                        Return CRound(_CurrentValueDecreaseAccountValue _
                            - CRound(_AmmountChange * _CurrentValueDecreaseAccountValuePerUnit))
                    End If

                ElseIf _Type = LtaOperationType.ValueChange Then

                    If CRound(_CurrentValueIncreaseAccountValue _
                        - _CurrentValueIncreaseAmortizationAccountValue _
                        - _CurrentValueDecreaseAccountValue + _RevaluedPortionTotalValueChange) >= 0 Then

                        Return 0

                    Else

                        Return CRound(_CurrentValueDecreaseAccountValue _
                            - _CurrentValueIncreaseAccountValue _
                            + _CurrentValueIncreaseAmortizationAccountValue _
                            - _RevaluedPortionTotalValueChange)

                    End If

                Else

                    Return CRound(_CurrentValueDecreaseAccountValue)

                End If
            End Get
        End Property

        Public ReadOnly Property AfterOperationValueDecreaseAccountValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.ValueChange Then

                    If CRound(_CurrentValueIncreaseAccountValuePerUnit _
                        - _CurrentValueIncreaseAmortizationAccountValuePerUnit _
                        - _CurrentValueDecreaseAccountValuePerUnit _
                        + _RevaluedPortionUnitValueChange, ROUNDUNITASSET) >= 0 Then

                        Return 0

                    Else

                        Return CRound(_CurrentValueDecreaseAccountValuePerUnit _
                            - _CurrentValueIncreaseAccountValuePerUnit _
                            + _CurrentValueIncreaseAmortizationAccountValuePerUnit _
                            - _RevaluedPortionUnitValueChange, ROUNDUNITASSET)

                    End If

                Else

                    Return CRound(_CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)

                End If
            End Get
        End Property

        Public ReadOnly Property AfterOperationValueIncreaseAccountValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.Transfer OrElse _Type = LtaOperationType.Discard Then

                    If _AmmountChange < 1 OrElse Not _CurrentValueIncreaseAccountValue > 0 Then
                        Return CRound(_CurrentValueIncreaseAccountValue)
                    ElseIf _AmmountChange = _CurrentAssetAmmount Then
                        Return 0
                    Else
                        Return CRound(_CurrentValueIncreaseAccountValue _
                            - CRound(_AmmountChange * _CurrentValueIncreaseAccountValuePerUnit))
                    End If

                ElseIf _Type = LtaOperationType.ValueChange Then

                    If CRound(_CurrentValueIncreaseAccountValue _
                        - _CurrentValueIncreaseAmortizationAccountValue _
                        - _CurrentValueDecreaseAccountValue + _RevaluedPortionTotalValueChange) <= 0 Then

                        Return CRound(_CurrentValueIncreaseAmortizationAccountValue)

                    Else

                        Return CRound(_CurrentValueIncreaseAccountValue _
                            - _CurrentValueDecreaseAccountValue _
                            + _RevaluedPortionTotalValueChange)

                    End If

                Else

                    Return CRound(_CurrentValueIncreaseAccountValue)

                End If
            End Get
        End Property

        Public ReadOnly Property AfterOperationValueIncreaseAccountValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.ValueChange Then

                    If CRound(_CurrentValueIncreaseAccountValuePerUnit _
                        - _CurrentValueIncreaseAmortizationAccountValuePerUnit _
                        - _CurrentValueDecreaseAccountValuePerUnit _
                        + _RevaluedPortionUnitValueChange, ROUNDUNITASSET) <= 0 Then

                        Return CRound(_CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)

                    Else

                        Return CRound(_CurrentValueIncreaseAccountValuePerUnit _
                            - _CurrentValueDecreaseAccountValuePerUnit _
                            + _RevaluedPortionUnitValueChange, ROUNDUNITASSET)

                    End If

                Else

                    Return CRound(_CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)

                End If
            End Get
        End Property

        Public ReadOnly Property AfterOperationValueIncreaseAmortizationAccountValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.Transfer OrElse _Type = LtaOperationType.Discard Then

                    If _AmmountChange < 1 Then
                        Return CRound(_CurrentValueIncreaseAmortizationAccountValue)
                    ElseIf _AmmountChange = _CurrentAssetAmmount Then
                        Return 0
                    Else
                        Return CRound(_CurrentValueIncreaseAmortizationAccountValue _
                            - CRound(_AmmountChange * _CurrentValueIncreaseAmortizationAccountValuePerUnit))
                    End If

                ElseIf _Type = LtaOperationType.Amortization Then

                    Return CRound(_CurrentValueIncreaseAmortizationAccountValue _
                        - _RevaluedPortionTotalValueChange)

                Else

                    Return CRound(_CurrentValueIncreaseAmortizationAccountValue)

                End If
            End Get
        End Property

        Public ReadOnly Property AfterOperationValueIncreaseAmortizationAccountValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.Amortization Then

                    Return CRound(_CurrentValueIncreaseAmortizationAccountValuePerUnit _
                        - _RevaluedPortionUnitValueChange, ROUNDUNITASSET)

                Else

                    Return CRound(_CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)

                End If
            End Get
        End Property

        Public ReadOnly Property AfterOperationAssetAmmount() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.Discard OrElse _Type = LtaOperationType.Transfer Then
                    Return _CurrentAssetAmmount - _AmmountChange
                Else
                    Return _CurrentAssetAmmount
                End If
            End Get
        End Property

        Public Property AfterOperationAssetValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type <> LtaOperationType.AccountChange _
                    AndAlso _Type <> LtaOperationType.AmortizationPeriod _
                    AndAlso _Type <> LtaOperationType.UsingEnd _
                    AndAlso _Type <> LtaOperationType.UsingStart Then

                    Return CRound(_CurrentAssetValue + _TotalValueChange)

                Else

                    Return CRound(_CurrentAssetValue)

                End If
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If CRound(_CurrentAssetValue + _TotalValueChange) <> CRound(value) Then
                    If Not _ChronologyValidator.FinancialDataCanChange Then Exit Property
                    TotalValueChange = CRound(value - _CurrentAssetValue)
                End If
            End Set
        End Property

        Public Property AfterOperationAssetValuePerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.AcquisitionValueIncrease _
                    OrElse _Type = LtaOperationType.Amortization _
                    OrElse _Type = LtaOperationType.ValueChange Then

                    Return CRound(_CurrentAssetValuePerUnit + _UnitValueChange, ROUNDUNITASSET)

                Else

                    Return CRound(_CurrentAssetValuePerUnit, ROUNDUNITASSET)

                End If
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If CRound(_CurrentAssetValuePerUnit + _UnitValueChange, ROUNDUNITASSET) _
                    <> CRound(value, ROUNDUNITASSET) Then
                    If Not _ChronologyValidator.FinancialDataCanChange Then Exit Property
                    UnitValueChange = CRound(value - _CurrentAssetValuePerUnit, ROUNDUNITASSET)
                End If
            End Set
        End Property

        Public Property AfterOperationAssetValueRevaluedPortion() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.Amortization _
                    OrElse _Type = LtaOperationType.Discard _
                    OrElse _Type = LtaOperationType.Transfer _
                    OrElse _Type = LtaOperationType.ValueChange Then

                    Return CRound(_CurrentAssetValueRevaluedPortion + _RevaluedPortionTotalValueChange)

                Else

                    Return CRound(_CurrentAssetValueRevaluedPortion)

                End If
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If CRound(_CurrentAssetValueRevaluedPortion _
                    + _RevaluedPortionTotalValueChange) <> CRound(value) Then

                    If Not _ChronologyValidator.FinancialDataCanChange Then Exit Property
                    RevaluedPortionTotalValueChange = CRound(value - _CurrentAssetValueRevaluedPortion)

                End If
            End Set
        End Property

        Public Property AfterOperationAssetValueRevaluedPortionPerUnit() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _Type = LtaOperationType.Amortization OrElse _Type = LtaOperationType.ValueChange Then

                    Return CRound(_CurrentAssetValueRevaluedPortionPerUnit + _
                        _RevaluedPortionUnitValueChange, ROUNDUNITASSET)

                Else

                    Return CRound(_CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)

                End If
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If CRound(_CurrentAssetValueRevaluedPortionPerUnit _
                    + _RevaluedPortionUnitValueChange, ROUNDUNITASSET) <> CRound(value, ROUNDUNITASSET) Then

                    If Not _ChronologyValidator.FinancialDataCanChange Then Exit Property
                    RevaluedPortionUnitValueChange = CRound(value _
                        - _CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)

                End If
            End Set
        End Property



        Friend Sub SetAttachedJournalEntry(ByVal JournalEntryInfo As ActiveReports.JournalEntryInfo)
            SetAttachedJournalEntry(JournalEntryInfo.Id, JournalEntryInfo.Content, _
                JournalEntryInfo.DocType, JournalEntryInfo.DocNumber)
        End Sub

        Friend Sub SetAttachedJournalEntry(ByVal nJournalEntryID As Integer, _
            ByVal nJournalEntryContent As String, ByVal nJournalEntryType As DocumentType, _
            ByVal nJournalEntryDocNumber As String)

            If _Type <> LtaOperationType.AcquisitionValueIncrease AndAlso _
                _Type <> LtaOperationType.Transfer AndAlso _Type <> LtaOperationType.ValueChange _
                AndAlso _Type <> LtaOperationType.Discard AndAlso _Type <> LtaOperationType.Amortization Then _
                Throw New Exception("Klaida. Susietas bendrojo žurnalo įrašas gali būti " _
                & "priskiriamas tik įsigijimo savikainos padidinimo, pervertinimo ir perleidimo operacijoms.")

            If Not nJournalEntryID > 0 Then
                _JournalEntryType = DocumentType.None
                _JournalEntryContent = ""
                _JournalEntryID = -1
                _JournalEntryDocNumber = ""
                PropertyHasChanged("JournalEntryID")
                PropertyHasChanged("JournalEntryContent")
                PropertyHasChanged("JournalEntryType")
                PropertyHasChanged("JournalEntryDocNumber")
                Exit Sub
            End If


            If nJournalEntryID = _AssetAquisitionOpID Then Throw New Exception( _
                "Nurodyta bendrojo žurnalo operacija (dokumentas) yra " _
                & "šio turto įsigijimo pagrindas. Ta pati operacija negali pagrįsti ir " _
                & "turto įsigijimo, ir kitos operacijos su tuo turtu.")

            If _Type = LtaOperationType.Transfer AndAlso _
                (nJournalEntryType = DocumentType.InvoiceMade OrElse _
                nJournalEntryType = DocumentType.InvoiceReceived) Then _
                Throw New Exception("Turto perleidimo operacijas, pagrįstas sąskaitomis - faktūromis, " & _
                "galima tvarkyti tik išrašant arba keičiant sąskaitas - faktūras.")

            _JournalEntryType = nJournalEntryType
            _JournalEntryContent = nJournalEntryContent
            _JournalEntryID = nJournalEntryID
            _JournalEntryDocNumber = nJournalEntryDocNumber
            PropertyHasChanged("JournalEntryID")
            PropertyHasChanged("JournalEntryContent")
            PropertyHasChanged("JournalEntryType")
            PropertyHasChanged("JournalEntryDocNumber")

        End Sub

        Friend Sub ForceCheckRules()
            ValidationRules.CheckRules()
        End Sub

        ''' <summary>
        ''' Updates properties with the AmortizationCalculation data.
        ''' </summary>
        Friend Sub SetAmortizationCalculation(ByVal AmortizationCalculation _
            As LongTermAssetAmortizationCalculation)

            _UnitValueChange = -AmortizationCalculation.AmortizationValuePerUnit
            _TotalValueChange = -AmortizationCalculation.AmortizationValue
            _RevaluedPortionTotalValueChange = -AmortizationCalculation.AmortizationValueRevaluedPortion
            _RevaluedPortionUnitValueChange = -AmortizationCalculation.AmortizationValuePerUnitRevaluedPortion
            _AmortizationCalculatedForMonths = AmortizationCalculation.AmortizationCalculatedForMonths
            _AmortizationCalculations = AmortizationCalculation.CalculationDescription

            PropertyHasChanged("UnitValueChange")
            PropertyHasChanged("TotalValueChange")
            PropertyHasChanged("RevaluedPortionTotalValueChange")
            PropertyHasChanged("RevaluedPortionUnitValueChange")
            PropertyHasChanged("AmortizationCalculatedForMonths")
            PropertyHasChanged("AmortizationCalculations")
            AfterOperationPropertiesChanged()

        End Sub

        Private Sub OnTypeChanged()

            _AmmountChange = 0
            _UnitValueChange = 0
            _TotalValueChange = 0
            _RevaluedPortionUnitValueChange = 0
            _RevaluedPortionTotalValueChange = 0
            _AmortizationCalculations = ""
            _AccountCorresponding = 0
            _ActNumber = 0
            _JournalEntryID = -1
            _JournalEntryType = DocumentType.None
            _JournalEntryContent = ""
            _JournalEntryDocNumber = ""

            _ChronologyValidator.ReconfigureForType(_Type, LtaAccountChangeType.AcquisitionAccount)

            PropertyHasChanged("JournalEntryContent")
            PropertyHasChanged("JournalEntryID")
            PropertyHasChanged("JournalEntryDocNumber")
            PropertyHasChanged("AmortizationCalculations")
            PropertyHasChanged("AmmountChange")
            PropertyHasChanged("UnitValueChange")
            PropertyHasChanged("TotalValueChange")
            PropertyHasChanged("Ammount")
            PropertyHasChanged("UnitValue")
            PropertyHasChanged("TotalValue")
            PropertyHasChanged("RevaluedPortionUnitValueChange")
            PropertyHasChanged("RevaluedPortionTotalValueChange")
            PropertyHasChanged("AccountCorresponding")
            PropertyHasChanged("ActNumber")

            AfterOperationPropertiesChanged()

            ValidationRules.CheckRules()

        End Sub


        Protected Overrides Function GetIdValue() As Object
            Return _AssetID
        End Function

        Private Sub AfterOperationPropertiesChanged()
            PropertyHasChanged("AfterOperationAcquisitionAccountValue")
            PropertyHasChanged("AfterOperationAcquisitionAccountValuePerUnit")
            PropertyHasChanged("AfterOperationAmortizationAccountValue")
            PropertyHasChanged("AfterOperationAmortizationAccountValuePerUnit")
            PropertyHasChanged("AfterOperationAssetAmmount")
            PropertyHasChanged("AfterOperationAssetValue")
            PropertyHasChanged("AfterOperationAssetValuePerUnit")
            PropertyHasChanged("AfterOperationAssetValueRevaluedPortion")
            PropertyHasChanged("AfterOperationAssetValueRevaluedPortionPerUnit")
            PropertyHasChanged("AfterOperationValueDecreaseAccountValue")
            PropertyHasChanged("AfterOperationValueDecreaseAccountValuePerUnit")
            PropertyHasChanged("AfterOperationValueIncreaseAccountValue")
            PropertyHasChanged("AfterOperationValueIncreaseAccountValuePerUnit")
            PropertyHasChanged("AfterOperationValueIncreaseAmortizationAccountValue")
            PropertyHasChanged("AfterOperationValueIncreaseAmortizationAccountValuePerUnit")
        End Sub

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf JournalEntryIdValidation, "AssetName")
            ValidationRules.AddRule(AddressOf DateValidationForLTAOperation, "AssetName")
            ValidationRules.AddRule(AddressOf UnitValueValidation, "UnitValueChange")
            ValidationRules.AddRule(AddressOf TotalValueValidation, "TotalValueChange")
            ValidationRules.AddRule(AddressOf RevaluedPortionUnitValueValidation, "RevaluedPortionUnitValueChange")
            ValidationRules.AddRule(AddressOf RevaluedPortionTotalValueValidation, "RevaluedPortionTotalValueChange")
            ValidationRules.AddRule(AddressOf AmountValidation, "AmmountChange")
            ValidationRules.AddRule(AddressOf OperationTypeValidation, "AssetName")
            ValidationRules.AddRule(AddressOf AmortizationCalculatedForMonthsValidation, _
                "AmortizationCalculatedForMonths")

            ValidationRules.AddDependantProperty("JournalEntryID", "AssetName")
            ValidationRules.AddDependantProperty("Date", "AssetName")
            ValidationRules.AddDependantProperty("Type", "AssetName")

        End Sub

        ''' <summary>
        ''' Rule ensuring that the operation date meets special rules.
        ''' or changed before last closing day.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function DateValidationForLTAOperation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As LongTermAssetOperationChild = DirectCast(target, LongTermAssetOperationChild)

            If ValObj._ChronologyValidator Is Nothing Then Return True

            If Not ValObj._IsChecked AndAlso Not ValObj.IsNew AndAlso _
                Not ValObj._ChronologyValidator.FinancialDataCanChange Then

                e.Description = "Operacijos finansiniai duomenys negali būti keičiami (ar šalinami) " _
                    & " (" & ValObj._AssetName & "): " & ValObj._ChronologyValidator.FinancialDataCanChangeExplanation
                e.Severity = Csla.Validation.RuleSeverity.Error
                Return False

            ElseIf ValObj._IsChecked Then

                Return ValObj._ChronologyValidator.ValidateOperationDate(ValObj._Date, e.Description, e.Severity)

            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that the related general ledger entry is provided when necessary.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function JournalEntryIdValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As LongTermAssetOperationChild = DirectCast(target, LongTermAssetOperationChild)

            If Not ValObj._IsChecked Then Return True

            If ValObj._Type = LtaOperationType.ValueChange OrElse _
                ValObj._Type = LtaOperationType.AcquisitionValueIncrease OrElse _
                ValObj._Type = LtaOperationType.Transfer Then

                If ValObj._JournalEntryID = ValObj._AssetAquisitionOpID Then
                    e.Description = "Nurodyta bendrojo žurnalo operacija (dokumentas) yra " _
                    & "šio turto įsigijimo pagrindas. Ta pati operacija negali pagrįsti ir " _
                    & "turto įsigijimo, ir kitos operacijos su tuo turtu." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False
                End If

            End If

            Return True
        End Function

        ''' <summary>
        ''' Rule ensuring that the unit value is provided when necessary.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function UnitValueValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As LongTermAssetOperationChild = DirectCast(target, LongTermAssetOperationChild)

            If Not ValObj._IsChecked Then Return True

            If ValObj._Type = LtaOperationType.AcquisitionValueIncrease _
                OrElse ValObj._Type = LtaOperationType.Amortization OrElse _
                ValObj._Type = LtaOperationType.ValueChange Then

                If CRound(ValObj._UnitValueChange, ROUNDUNITASSET) = 0 Then
                    e.Description = "Nenurodytas turto vieneto vertės pokytis." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False
                ElseIf Not CRound(ValObj._UnitValueChange, ROUNDUNITASSET) < 0 AndAlso _
                    ValObj._Type = LtaOperationType.Amortization Then
                    e.Description = "Vieneto vertės pokytis po amortizacijos negali būti teigiamas." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False
                ElseIf Not CRound(ValObj._UnitValueChange, ROUNDUNITASSET) > 0 AndAlso _
                    ValObj._Type = LtaOperationType.AcquisitionValueIncrease Then
                    e.Description = "Vieneto vertės pokytis po įsigijimo savikainos " _
                    & "padidinimo negali būti neigiamas." & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False
                ElseIf ValObj.AfterOperationAssetValuePerUnit < CRound(ValObj._AssetLiquidationValue, ROUNDUNITASSET) Then
                    e.Description = "Turto vieneto vertė negali būti sumažinta žemiau likvidacinės." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False
                End If

            End If

            Return True
        End Function

        ''' <summary>
        ''' Rule ensuring that the total value is provided when necessary.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function TotalValueValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As LongTermAssetOperationChild = DirectCast(target, LongTermAssetOperationChild)

            If Not ValObj._IsChecked Then Return True

            If (ValObj.Type = LtaOperationType.AcquisitionValueIncrease OrElse _
                ValObj.Type = LtaOperationType.Amortization OrElse _
                ValObj.Type = LtaOperationType.Discard OrElse ValObj._Type = LtaOperationType.Transfer _
                OrElse ValObj._Type = LtaOperationType.ValueChange) Then

                If CRound(ValObj._TotalValueChange) = 0 Then

                    e.Description = "Nenurodytas bendras turto vertės pokytis." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False

                ElseIf Not CRound(ValObj._TotalValueChange) < 0 AndAlso _
                    (ValObj._Type = LtaOperationType.Amortization _
                    OrElse ValObj._Type = LtaOperationType.Discard _
                    OrElse ValObj._Type = LtaOperationType.Transfer) Then

                    e.Description = "Vertės pokytis po amortizacijos, perleidimo arba " _
                        & "nurašymo negali būti teigiamas." & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False

                ElseIf Not CRound(ValObj._TotalValueChange) > 0 AndAlso _
                    ValObj._Type = LtaOperationType.AcquisitionValueIncrease Then

                    e.Description = "Vertės pokytis po įsigijimo savikainos padidinimo negali būti neigiamas." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False

                End If

                Dim nAfterOperationValue As Double = ValObj.AfterOperationAssetValue

                If (ValObj.Type = LtaOperationType.AcquisitionValueIncrease OrElse _
                    ValObj.Type = LtaOperationType.Amortization OrElse _
                    ValObj._Type = LtaOperationType.ValueChange) _
                    AndAlso CRound(nAfterOperationValue) < _
                    CRound(ValObj._AssetLiquidationValue * ValObj.CurrentAssetAmmount) Then

                    e.Description = "Turto vertė po operacijos negali sumažėti mažiau likvidacinės." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False

                End If

                If (ValObj.Type = LtaOperationType.Discard OrElse ValObj._Type = LtaOperationType.Transfer) _
                    AndAlso CRound(nAfterOperationValue) < 0 Then

                    e.Description = "Turto vertė po operacijos negali sumažėti mažiau nulio." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False

                End If

            End If

            Return True
        End Function

        ''' <summary>
        ''' Rule ensuring that the revalued portion unit value is provided when necessary.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function RevaluedPortionUnitValueValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As LongTermAssetOperationChild = DirectCast(target, LongTermAssetOperationChild)

            If Not ValObj._IsChecked Then Return True

            If (ValObj._Type = LtaOperationType.Amortization AndAlso _
                CRound(ValObj._CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET) <> 0) _
                OrElse ValObj._Type = LtaOperationType.ValueChange Then

                If CRound(ValObj._RevaluedPortionUnitValueChange, ROUNDUNITASSET) = 0 Then
                    e.Description = "Nenurodytas turto perkainotos dalies vieneto vertės pokytis." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False
                ElseIf ValObj.AfterOperationAssetValuePerUnit < CRound(ValObj._AssetLiquidationValue, ROUNDUNITASSET) Then
                    e.Description = "Turto vieneto vertė negali būti sumažinta žemiau likvidacinės." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False
                ElseIf ValObj._Type = LtaOperationType.Amortization AndAlso _
                    Not CRound(ValObj._RevaluedPortionUnitValueChange, ROUNDUNITASSET) < 0 Then
                    e.Description = "Turto vieneto perkainotos dalies vertės pokytis po " _
                    & "amortizacijos negali būti teigiamas." & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False
                End If

            End If

            Return True
        End Function

        ''' <summary>
        ''' Rule ensuring that the revalued portion total value is provided when necessary.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function RevaluedPortionTotalValueValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As LongTermAssetOperationChild = DirectCast(target, LongTermAssetOperationChild)

            If Not ValObj._IsChecked Then Return True

            If ((ValObj._Type = LtaOperationType.Amortization _
                OrElse ValObj._Type = LtaOperationType.Discard _
                OrElse ValObj._Type = LtaOperationType.Transfer) AndAlso _
                CRound(ValObj._CurrentAssetValueRevaluedPortion) <> 0) _
                OrElse ValObj._Type = LtaOperationType.ValueChange Then

                If CRound(ValObj._RevaluedPortionTotalValueChange) = 0 Then

                    e.Description = "Nenurodytas bendras turto pervertintos dalies vertės pokytis." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False

                ElseIf Not CRound(ValObj._RevaluedPortionTotalValueChange) < 0 AndAlso _
                    ValObj._Type = LtaOperationType.Amortization Then

                    e.Description = "Bendras turto pervertintos dalies vertės pokytis po " _
                        & "amortizacijos negali būti teigiamas." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False

                ElseIf (ValObj._Type = LtaOperationType.Discard _
                    OrElse ValObj._Type = LtaOperationType.Transfer) AndAlso _
                    ((CRound(ValObj._CurrentAssetValueRevaluedPortion) > 0 AndAlso _
                    CRound(ValObj._RevaluedPortionTotalValueChange) > 0) OrElse _
                    (CRound(ValObj._CurrentAssetValueRevaluedPortion) < 0 AndAlso _
                    CRound(ValObj._RevaluedPortionTotalValueChange) < 0)) Then

                    e.Description = "Bendras turto pervertintos dalies vertės pokytis po " _
                        & "nurašymo arba perleidimo negali būti teigiamas." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False

                ElseIf (ValObj._Type = LtaOperationType.Amortization _
                    OrElse ValObj._Type = LtaOperationType.ValueChange) _
                    AndAlso ValObj.AfterOperationAssetValue < CRound(ValObj._AssetLiquidationValue _
                    * ValObj._CurrentAssetAmmount) Then

                    e.Description = "Turto vertė po operacijos negali sumažėti mažiau likvidacinės vertės." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False

                ElseIf (ValObj._Type = LtaOperationType.Transfer _
                    OrElse ValObj._Type = LtaOperationType.Discard) _
                    AndAlso ValObj.AfterOperationAssetValue < 0 Then

                    e.Description = "Turto vertė po operacijos negali sumažėti mažiau nulio." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False

                End If

            End If

            Return True
        End Function

        ''' <summary>
        ''' Rule ensuring that an operation can only be added or updated if current ammount
        ''' of the asset > 0.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AmountValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As LongTermAssetOperationChild = DirectCast(target, LongTermAssetOperationChild)

            If Not ValObj._IsChecked Then Return True

            If Not ValObj._CurrentAssetAmmount > 0 Then

                e.Description = "Visas ilgalaikis turtas yra perleistas (nurašytas) ." & _
                    "Operacijų su juo įvesti ar taisyti nėra leidžiama." _
                    & " (" & ValObj._AssetName & ")"
                e.Severity = Csla.Validation.RuleSeverity.Error
                Return False

            End If

            If (ValObj._Type = LtaOperationType.Discard OrElse ValObj.Type = LtaOperationType.Transfer) Then

                If Not ValObj._AmmountChange > 0 Then
                    e.Description = "Nenurodytas IT kiekis." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False
                ElseIf ValObj._AmmountChange > ValObj._CurrentAssetAmmount Then
                    e.Description = "Nepakanka IT kiekio." _
                    & " (" & ValObj._AssetName & ")"
                    e.Severity = Csla.Validation.RuleSeverity.Error
                    Return False
                End If

            End If

            Return True
        End Function

        ''' <summary>
        ''' Rule ensuring that an operation type is valid (for usage operation types).
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function OperationTypeValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As LongTermAssetOperationChild = DirectCast(target, LongTermAssetOperationChild)

            If Not ValObj._IsChecked Then Return True

            If ValObj._Type = LtaOperationType.UsingStart AndAlso ValObj._CurrentUsageStatus Then
                e.Description = "Turtas jau perduotas naudojimui." _
                    & " (" & ValObj._AssetName & ")"
                e.Severity = Csla.Validation.RuleSeverity.Error
                Return False
            End If

            If ValObj._Type = LtaOperationType.UsingEnd AndAlso Not ValObj._CurrentUsageStatus Then
                e.Description = "Turtas ir taip nėra naudojamas." _
                    & " (" & ValObj._AssetName & ")"
                e.Severity = Csla.Validation.RuleSeverity.Error
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' Rule ensuring that amortization can only be calculated for a month or more.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AmortizationCalculatedForMonthsValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As LongTermAssetOperationChild = DirectCast(target, LongTermAssetOperationChild)

            If Not ValObj._IsChecked Then Return True

            If ValObj._Type = LtaOperationType.Amortization AndAlso _
                Not ValObj._AmortizationCalculatedForMonths > 0 Then
                e.Description = "Amortizacija negali būti paskaičiuota mažesniam nei mėnesio laikotarpiui." _
                    & " (" & ValObj._AssetName & ")"
                e.Severity = Csla.Validation.RuleSeverity.Error
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

        Friend Shared Function NewLongTermAssetOperationChild(ByVal dr As DataRow, _
            ByVal ChronologyDataSource As DataTable, ByVal parentValidator As IChronologicValidator) As LongTermAssetOperationChild
            Return New LongTermAssetOperationChild(dr, ChronologyDataSource, parentValidator)
        End Function

        Friend Shared Function GetLongTermAssetOperationChild(ByVal dr As DataRow, _
            ByVal BackgroundDataRow As DataRow, ByVal ChronologyDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator) As LongTermAssetOperationChild
            Return New LongTermAssetOperationChild(dr, BackgroundDataRow, ChronologyDataSource, parentValidator)
        End Function


        Private Sub New()
            MarkAsChild()
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal ChronologyDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator)
            MarkAsChild()
            Create(dr, ChronologyDataSource, parentValidator)
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal BackgroundDataRow As DataRow, _
            ByVal ChronologyDataSource As DataTable, ByVal parentValidator As IChronologicValidator)
            MarkAsChild()
            Fetch(dr, BackgroundDataRow, ChronologyDataSource, parentValidator)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Create(ByVal dr As DataRow, ByVal ChronologyDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator)

            GetOperationBackgroundInfo(dr)
            _ChronologyValidator = OperationChronologicValidator.NewOperationChronologicValidator( _
                _AssetID, _AssetName, _Type, LtaAccountChangeType.AcquisitionAccount, _
                _AssetDateAcquired, parentValidator, ChronologyDataSource)

            MarkNew()
            ValidationRules.CheckRules()

        End Sub

        Private Sub GetOperationBackgroundInfo(ByVal dr As DataRow)

            _AssetID = CIntSafe(dr.Item(0))
            _AssetName = CStrSafe(dr.Item(1)).Trim
            _AssetMeasureUnit = CStrSafe(dr.Item(2)).Trim
            _AssetAcquiredAccount = CIntSafe(dr.Item(3))
            _AssetContraryAccount = CIntSafe(dr.Item(4))
            _AssetValueDecreaseAccount = CIntSafe(dr.Item(5), 0)
            _AssetValueIncreaseAccount = CIntSafe(dr.Item(6))
            _AssetValueIncreaseAmortizationAccount = CIntSafe(dr.Item(7), 0)
            _AssetDateAcquired = CDate(dr.Item(8))
            _AssetAquisitionOpID = CIntSafe(dr.Item(9), 0)
            _AssetLiquidationValue = CDblSafe(dr.Item(10), 2, 0)
            _CurrentUsageStatus = ConvertDbBoolean(CIntSafe(dr.Item(11), 0))
            _CurrentAssetAmmount = CIntSafe(dr.Item(12), 0)
            _CurrentAcquisitionAccountValuePerUnit = CDblSafe(dr.Item(13), ROUNDUNITASSET, 0)
            _CurrentAcquisitionAccountValue = CDblSafe(dr.Item(14), 2, 0)
            If CDbl(dr.Item(15)) < 0 Then
                _CurrentValueDecreaseAccountValuePerUnit = -CDblSafe(dr.Item(15), ROUNDUNITASSET, 0)
                _CurrentValueIncreaseAccountValuePerUnit = 0
            ElseIf CDbl(dr.Item(15)) > 0 Then
                _CurrentValueDecreaseAccountValuePerUnit = 0
                _CurrentValueIncreaseAccountValuePerUnit = CDblSafe(dr.Item(15), ROUNDUNITASSET, 0)
            Else
                _CurrentValueDecreaseAccountValuePerUnit = 0
                _CurrentValueIncreaseAccountValuePerUnit = 0
            End If
            If CDbl(dr.Item(16)) < 0 Then
                _CurrentValueDecreaseAccountValue = -CDblSafe(dr.Item(16), 2, 0)
                _CurrentValueIncreaseAccountValue = 0
            ElseIf CDbl(dr.Item(16)) > 0 Then
                _CurrentValueDecreaseAccountValue = 0
                _CurrentValueIncreaseAccountValue = CDblSafe(dr.Item(16), 2, 0)
            Else
                _CurrentValueDecreaseAccountValue = 0
                _CurrentValueIncreaseAccountValue = 0
            End If
            _CurrentAmortizationAccountValuePerUnit = CDblSafe(dr.Item(17), ROUNDUNITASSET, 0)
            _CurrentAmortizationAccountValue = CDblSafe(dr.Item(18), 2, 0)
            _CurrentValueIncreaseAmortizationAccountValuePerUnit = CDblSafe(dr.Item(19), ROUNDUNITASSET, 0)
            _CurrentValueIncreaseAmortizationAccountValue = CDblSafe(dr.Item(20), 2, 0)
            _CurrentAmortizationPeriod = CIntSafe(dr.Item(21), 0)
            _CurrentUsageTermMonths = CIntSafe(dr.Item(22), 0)

            _CurrentValueIncreaseAccountValue = CRound(_CurrentValueIncreaseAccountValue + _
                _CurrentValueIncreaseAmortizationAccountValue)
            _CurrentValueIncreaseAccountValuePerUnit = CRound(_CurrentValueIncreaseAccountValuePerUnit + _
                _CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)

            _CurrentAssetAmmount = _CurrentAssetAmmount + CIntSafe(dr.Item(23), 0)
            _CurrentAcquisitionAccountValue = _
                CRound(_CurrentAcquisitionAccountValue + CDblSafe(dr.Item(28), 2, 0))
            _CurrentAcquisitionAccountValuePerUnit = _
                CRound(_CurrentAcquisitionAccountValuePerUnit + _
                CDblSafe(dr.Item(29), ROUNDUNITASSET, 0), ROUNDUNITASSET)
            _CurrentAmortizationAccountValue = _
                CRound(_CurrentAmortizationAccountValue + CDblSafe(dr.Item(30), 2, 0))
            _CurrentAmortizationAccountValuePerUnit = _
                CRound(_CurrentAmortizationAccountValuePerUnit _
                + CDblSafe(dr.Item(31), ROUNDUNITASSET, 0), ROUNDUNITASSET)
            _CurrentValueDecreaseAccountValue = CRound(_CurrentValueDecreaseAccountValue + _
                CDblSafe(dr.Item(32), 2, 0))
            _CurrentValueDecreaseAccountValuePerUnit = _
                CRound(_CurrentValueDecreaseAccountValuePerUnit _
                + CDblSafe(dr.Item(33), ROUNDUNITASSET, 0), ROUNDUNITASSET)
            _CurrentValueIncreaseAccountValue = _
                CRound(_CurrentValueIncreaseAccountValue + CDblSafe(dr.Item(34), 2, 0))
            _CurrentValueIncreaseAccountValuePerUnit = _
                CRound(_CurrentValueIncreaseAccountValuePerUnit _
                + CDblSafe(dr.Item(35), ROUNDUNITASSET, 0), ROUNDUNITASSET)
            _CurrentValueIncreaseAmortizationAccountValue = _
                CRound(_CurrentValueIncreaseAmortizationAccountValue + CDblSafe(dr.Item(36), 2, 0))
            _CurrentValueIncreaseAmortizationAccountValuePerUnit = _
                CRound(_CurrentValueIncreaseAmortizationAccountValuePerUnit _
                + CDblSafe(dr.Item(37), ROUNDUNITASSET, 0), ROUNDUNITASSET)
            _CurrentUsageTermMonths = _CurrentUsageTermMonths + CIntSafe(dr.Item(38), 0)
            If (Math.Floor(CIntSafe(dr.Item(39), 0) / 2) <> Math.Ceiling(CIntSafe(dr.Item(39), 0) / 2)) Then _
                _CurrentUsageStatus = Not _CurrentUsageStatus

            _CurrentAssetValue = CRound(_CurrentAcquisitionAccountValue - _
                _CurrentAmortizationAccountValue - _CurrentValueDecreaseAccountValue + _
                _CurrentValueIncreaseAccountValue - _CurrentValueIncreaseAmortizationAccountValue)
            _CurrentAssetValuePerUnit = CRound(_CurrentAcquisitionAccountValuePerUnit - _
                _CurrentAmortizationAccountValuePerUnit - _CurrentValueDecreaseAccountValuePerUnit + _
                _CurrentValueIncreaseAccountValuePerUnit - _
                _CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            If _CurrentValueDecreaseAccountValue > 0 Then
                _CurrentAssetValueRevaluedPortion = -CRound(_CurrentValueDecreaseAccountValue)
                _CurrentAssetValueRevaluedPortionPerUnit = -CRound(_CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            ElseIf _CurrentValueIncreaseAccountValue > 0 Then
                _CurrentAssetValueRevaluedPortion = CRound(_CurrentValueIncreaseAccountValue _
                    - _CurrentValueIncreaseAmortizationAccountValue)
                _CurrentAssetValueRevaluedPortionPerUnit = CRound(_CurrentValueIncreaseAccountValuePerUnit _
                    - _CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            Else
                _CurrentAssetValueRevaluedPortion = 0
                _CurrentAssetValueRevaluedPortionPerUnit = 0
            End If

        End Sub


        Private Sub Fetch(ByVal dr As DataRow, ByVal BackgroundDataRow As DataRow, _
            ByVal ChronologyDataSource As DataTable, ByVal parentValidator As IChronologicValidator)

            _AssetID = CIntSafe(dr.Item(0), 0)
            _Type = ConvertEnumDatabaseStringCode(Of LtaOperationType)(CStrSafe(dr.Item(1)))
            '_AccountChangeType = ConvertAccountChangeType(dr.Item(2).ToString)
            _Date = CDateSafe(dr.Item(3), Today)
            _OldDate = _Date
            _JournalEntryID = CIntSafe(dr.Item(4), 0)
            _JournalEntryDocNumber = CStrSafe(dr.Item(5))
            _JournalEntryContent = CStrSafe(dr.Item(6))
            _JournalEntryType = ConvertEnumDatabaseStringCode(Of DocumentType)(CStrSafe(dr.Item(7)))
            '_IsComplexAct = ConvertDbBoolean(CInt(dr.Item(8)))
            _Content = CStrSafe(dr.Item(9))
            _AccountCorresponding = CIntSafe(dr.Item(10), 0)
            _ActNumber = CIntSafe(dr.Item(11), 0)
            _UnitValueChange = CDblSafe(dr.Item(12), ROUNDUNITASSET, 0)
            _AmmountChange = CIntSafe(dr.Item(13), 0)
            _TotalValueChange = CDblSafe(dr.Item(14), 2, 0)
            '_NewAmortizationPeriod = CInt(dr.Item(15))
            _AmortizationCalculations = CStrSafe(dr.Item(16))
            _RevaluedPortionUnitValueChange = CDblSafe(dr.Item(17), ROUNDUNITASSET, 0)
            _RevaluedPortionTotalValueChange = CDblSafe(dr.Item(18), 2, 0)
            _AmortizationCalculatedForMonths = CIntSafe(dr.Item(19), 0)
            _ID = CIntSafe(dr.Item(20), 0)
            _InsertDate = DateTime.SpecifyKind(CDateTimeSafe(dr.Item(21), Date.UtcNow), _
                DateTimeKind.Utc).ToLocalTime
            _UpdateDate = DateTime.SpecifyKind(CDateTimeSafe(dr.Item(22), Date.UtcNow), _
                DateTimeKind.Utc).ToLocalTime
            _IsChecked = True

            GetOperationBackgroundInfo(BackgroundDataRow)

            _ChronologyValidator = OperationChronologicValidator.GetOperationChronologicValidator( _
                _AssetID, _AssetName, _Type, LtaAccountChangeType.AcquisitionAccount, _
                _AssetDateAcquired, _ID, _Date, parentValidator, ChronologyDataSource)

            MarkOld()

            ValidationRules.CheckRules()

        End Sub


        Friend Sub Insert(ByVal parent As LongTermAssetComplexDocument)
            DoSave(parent)
            MarkOld()
            ValidationRules.CheckRules()
        End Sub

        Friend Sub Update(ByVal parent As LongTermAssetComplexDocument)
            DoSave(parent)
            MarkOld()
            ValidationRules.CheckRules()
        End Sub

        Private Sub DoSave(ByVal parent As LongTermAssetComplexDocument)

            _JournalEntryID = parent.JournalEntryID

            Dim myComm As SQLCommand
            If IsNew Then
                myComm = New SQLCommand("InsertLongTermAssetOperation")
                myComm.AddParam("?TD", _AssetID)
                myComm.AddParam("?OT", ConvertEnumDatabaseStringCode(_Type))
                myComm.AddParam("?AT", "")
                myComm.AddParam("?CA", parent.ID)
            Else
                myComm = New SQLCommand("UpdateLongTermAssetOperation")
                myComm.AddParam("?OD", _ID)
            End If

            AddParams(myComm)

            myComm.Execute()

            If IsNew Then _ID = Convert.ToInt32(myComm.LastInsertID)

            MarkOld()

        End Sub

        Private Sub AddParams(ByRef myComm As SQLCommand)

            myComm.AddParam("?DT", _Date.Date)
            If _Type <> LtaOperationType.AmortizationPeriod AndAlso _
                _Type <> LtaOperationType.UsingEnd AndAlso _Type <> LtaOperationType.UsingStart Then
                myComm.AddParam("?JD", _JournalEntryID)
            Else
                myComm.AddParam("?JD", 0)
            End If
            myComm.AddParam("?CN", _Content.Trim)
            If _Type = LtaOperationType.AccountChange OrElse _Type = LtaOperationType.Amortization _
                OrElse _Type = LtaOperationType.Discard Then
                myComm.AddParam("?AC", _AccountCorresponding)
            Else
                myComm.AddParam("?AC", 0)
            End If
            If _Type = LtaOperationType.Discard OrElse _Type = LtaOperationType.UsingEnd _
                OrElse _Type = LtaOperationType.UsingStart Then
                myComm.AddParam("?NM", _ActNumber)
            Else
                myComm.AddParam("?NM", 0)
            End If
            If _Type = LtaOperationType.AcquisitionValueIncrease OrElse _Type = LtaOperationType.Amortization _
                OrElse _Type = LtaOperationType.ValueChange Then
                myComm.AddParam("?UV", CRound(_UnitValueChange, ROUNDUNITASSET))
            Else
                myComm.AddParam("?UV", 0)
            End If
            If _Type = LtaOperationType.Discard OrElse _Type = LtaOperationType.Transfer Then
                myComm.AddParam("?AM", _AmmountChange)
            Else
                myComm.AddParam("?AM", 0)
            End If
            If _Type <> LtaOperationType.AccountChange AndAlso _Type <> LtaOperationType.AmortizationPeriod _
                AndAlso _Type <> LtaOperationType.UsingEnd AndAlso _Type <> LtaOperationType.UsingStart Then
                myComm.AddParam("?TV", CRound(_TotalValueChange))
            Else
                myComm.AddParam("?TV", 0)
            End If
            myComm.AddParam("?AP", 0)
            If _Type = LtaOperationType.Amortization Then
                myComm.AddParam("?CL", _AmortizationCalculations.Trim)
                myComm.AddParam("?UT", _AmortizationCalculatedForMonths)
            Else
                myComm.AddParam("?CL", "")
                myComm.AddParam("?UT", 0)
            End If
            If _Type = LtaOperationType.Amortization OrElse _Type = LtaOperationType.ValueChange Then
                myComm.AddParam("?RU", CRound(_RevaluedPortionUnitValueChange, ROUNDUNITASSET))
            Else
                myComm.AddParam("?RU", 0)
            End If
            If _Type = LtaOperationType.Amortization OrElse _Type = LtaOperationType.ValueChange _
                OrElse _Type = LtaOperationType.Discard OrElse _Type = LtaOperationType.Transfer Then
                myComm.AddParam("?RT", CRound(_RevaluedPortionTotalValueChange))
            Else
                myComm.AddParam("?RT", 0)
            End If

            myComm.AddParam("?DA", CRound(AfterOperationAcquisitionAccountValue _
                - _CurrentAcquisitionAccountValue))
            myComm.AddParam("?DB", CRound(AfterOperationAcquisitionAccountValuePerUnit _
                - _CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET))
            myComm.AddParam("?DC", CRound(AfterOperationAmortizationAccountValue _
                - _CurrentAmortizationAccountValue))
            myComm.AddParam("?DE", CRound(AfterOperationAmortizationAccountValuePerUnit _
                - _CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET))
            myComm.AddParam("?DF", CRound(AfterOperationValueDecreaseAccountValue _
                - _CurrentValueDecreaseAccountValue))
            myComm.AddParam("?DG", CRound(AfterOperationValueDecreaseAccountValuePerUnit _
                - _CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET))
            myComm.AddParam("?DH", CRound(AfterOperationValueIncreaseAccountValue _
                - _CurrentValueIncreaseAccountValue))
            myComm.AddParam("?DI", CRound(AfterOperationValueIncreaseAccountValuePerUnit _
                - _CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET))
            myComm.AddParam("?DJ", CRound(AfterOperationValueIncreaseAmortizationAccountValue _
                - _CurrentValueIncreaseAmortizationAccountValue))
            myComm.AddParam("?DK", CRound(AfterOperationValueIncreaseAmortizationAccountValuePerUnit _
                - _CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET))

            _UpdateDate = DateTime.Now
            _UpdateDate = New DateTime(Convert.ToInt64(Math.Floor(_UpdateDate.Ticks / TimeSpan.TicksPerSecond) _
                * TimeSpan.TicksPerSecond))
            If Me.IsNew Then _InsertDate = _UpdateDate
            myComm.AddParam("?UD", _UpdateDate.ToUniversalTime)

        End Sub


        ''' <summary>
        ''' Gets a book entry list that contains the book entries, needed to write off the asset from balance.
        ''' </summary>
        Friend Function GetBookEntryListForDiscard() As BookEntryInternalList

            If _AmmountChange < 1 Then Throw New Exception("Klaida. Nenurodytas turto kiekis.")
            If _CurrentAssetAmmount < 1 Then Throw New Exception("Klaida. Turto likutis lygus nuliui.")
            If _CurrentAssetAmmount < _AmmountChange Then Throw New Exception( _
                "Klaida. Nurodytas turto kiekis yra didesnis nei turto likutis.")

            Dim result As BookEntryInternalList = BookEntryInternalList. _
                NewBookEntryInternalList(BookEntryType.Debetas)

            If _CurrentAssetAmmount = _AmmountChange Then

                If CRound(_CurrentAmortizationAccountValue) > 0 Then
                    Dim AmortizationAccountBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
                    AmortizationAccountBookEntry.Account = _AssetContraryAccount
                    AmortizationAccountBookEntry.Ammount = CRound(Me._CurrentAmortizationAccountValue)
                    result.Add(AmortizationAccountBookEntry)
                End If

                If CRound(Me._CurrentValueDecreaseAccountValue) > 0 Then
                    Dim ValueDecreaseAccountBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
                    ValueDecreaseAccountBookEntry.Account = Me._AssetValueDecreaseAccount
                    ValueDecreaseAccountBookEntry.Ammount = CRound(_CurrentValueDecreaseAccountValue)
                    result.Add(ValueDecreaseAccountBookEntry)
                End If

                If CRound(Me._CurrentValueIncreaseAmortizationAccountValue) > 0 Then
                    Dim ValueIncreaseAmmortizationAccountBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
                    ValueIncreaseAmmortizationAccountBookEntry.Account = _
                        _AssetValueIncreaseAmortizationAccount
                    ValueIncreaseAmmortizationAccountBookEntry.Ammount = _
                        CRound(_CurrentValueIncreaseAmortizationAccountValue)
                    result.Add(ValueIncreaseAmmortizationAccountBookEntry)
                End If

                Dim AcquisitionAccountBookEntry As BookEntryInternal = _
                    BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
                AcquisitionAccountBookEntry.Account = _AssetAcquiredAccount
                AcquisitionAccountBookEntry.Ammount = CRound(_CurrentAcquisitionAccountValue)
                result.Add(AcquisitionAccountBookEntry)

                If CRound(Me._CurrentValueIncreaseAccountValue) > 0 Then
                    Dim ValueIncreaseAccountBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
                    ValueIncreaseAccountBookEntry.Account = Me._AssetValueIncreaseAccount
                    ValueIncreaseAccountBookEntry.Ammount = CRound(_CurrentValueIncreaseAccountValue)
                    result.Add(ValueIncreaseAccountBookEntry)
                End If

            Else

                If CRound(_CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET) > 0 Then
                    Dim AmortizationAccountBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
                    AmortizationAccountBookEntry.Account = _AssetContraryAccount
                    AmortizationAccountBookEntry.Ammount = _
                        CRound(_CurrentAmortizationAccountValuePerUnit * _AmmountChange)
                    result.Add(AmortizationAccountBookEntry)
                End If

                If CRound(_CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET) > 0 Then
                    Dim ValueDecreaseAccountBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
                    ValueDecreaseAccountBookEntry.Account = _AssetValueDecreaseAccount
                    ValueDecreaseAccountBookEntry.Ammount = _
                        CRound(_CurrentValueDecreaseAccountValuePerUnit * _AmmountChange)
                    result.Add(ValueDecreaseAccountBookEntry)
                End If

                If CRound(_CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET) > 0 Then
                    Dim ValueIncreaseAmmortizationAccountBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
                    ValueIncreaseAmmortizationAccountBookEntry.Account = _
                        _AssetValueIncreaseAmortizationAccount
                    ValueIncreaseAmmortizationAccountBookEntry.Ammount = _
                        CRound(_CurrentValueIncreaseAmortizationAccountValuePerUnit * _AmmountChange)
                    result.Add(ValueIncreaseAmmortizationAccountBookEntry)
                End If

                Dim AcquisitionAccountBookEntry As BookEntryInternal = _
                    BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
                AcquisitionAccountBookEntry.Account = _AssetAcquiredAccount
                AcquisitionAccountBookEntry.Ammount = _
                    CRound(_CurrentAcquisitionAccountValuePerUnit * _AmmountChange)
                result.Add(AcquisitionAccountBookEntry)

                If CRound(_CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET) > 0 Then
                    Dim ValueIncreaseAccountBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
                    ValueIncreaseAccountBookEntry.Account = _AssetValueIncreaseAccount
                    ValueIncreaseAccountBookEntry.Ammount = _
                        CRound(_CurrentValueIncreaseAccountValuePerUnit * _AmmountChange)
                    result.Add(ValueIncreaseAccountBookEntry)
                End If

            End If

            Return result

        End Function

        ''' <summary>
        ''' Gets a book entry list that contains the book entries, needed to calculate amortization.
        ''' </summary>
        Friend Function GetBookEntryListForAmortization() As BookEntryInternalList

            Dim result As BookEntryInternalList = BookEntryInternalList.NewBookEntryInternalList( _
                BookEntryType.Kreditas)

            If CRound(-_TotalValueChange) > 0 Then

                If CRound(-_TotalValueChange + _RevaluedPortionTotalValueChange) > 0 Then
                    Dim CreditBookEntry As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
                    CreditBookEntry.Account = _AssetContraryAccount
                    CreditBookEntry.Ammount = CRound(-_TotalValueChange + _RevaluedPortionTotalValueChange)
                    result.Add(CreditBookEntry)
                End If

                If CRound(-_RevaluedPortionTotalValueChange) > 0 Then
                    Dim CreditBookEntrySecondary As BookEntryInternal = _
                        BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
                    CreditBookEntrySecondary.Account = _AssetValueIncreaseAmortizationAccount
                    CreditBookEntrySecondary.Ammount = CRound(-_RevaluedPortionTotalValueChange)
                    result.Add(CreditBookEntrySecondary)
                End If

                If result.Count = 2 AndAlso result(0).Account = result(1).Account Then
                    result(0).Ammount = CRound(result(0).Ammount + result(1).Ammount)
                    result.RemoveAt(1)
                End If

            End If

            If result.Count < 1 Then Throw New Exception("Klaida. Neįmanoma suformuoti " _
                & "bendrojo žurnalo įrašo amortizacijai, kai bendras vertės " _
                & "pokytis lygus nuliui.")

            Return result

        End Function


        Friend Sub DeleteSelf()
            LongTermAssetOperation.DeleteLongTermAssetOperationChild(_ID)
            MarkNew()
        End Sub


        Friend Sub ReloadChronologyValidation(ByVal parent As LongTermAssetComplexDocument, _
            ByVal ChronologyDataSource As DataTable)

            If IsNew Then
                _ChronologyValidator = OperationChronologicValidator.NewOperationChronologicValidator( _
                    _AssetID, _AssetName, _Type, LtaAccountChangeType.AcquisitionAccount, _
                    _AssetDateAcquired, parent.ChronologicValidator.BaseValidator, ChronologyDataSource)
            Else
                _ChronologyValidator = OperationChronologicValidator.GetOperationChronologicValidator( _
                    _AssetID, _AssetName, _Type, LtaAccountChangeType.AcquisitionAccount, _
                    _AssetDateAcquired, _ID, _Date, parent.ChronologicValidator.BaseValidator, _
                    ChronologyDataSource)
            End If

            ValidationRules.CheckRules()

        End Sub

        Friend Sub CheckAllRules()

            If Not IsValid Then Throw New Exception("Turto '" & _AssetName _
                & "' operacijos duomenyse yra klaidų: " & Me.BrokenRulesCollection.ToString)

            If Not IsNew Then CheckIfUpdateDateChanged()

        End Sub

        Friend Sub CheckIfCanDelete(ByVal nJournalEntryID As Integer)

            If Not _ChronologyValidator.FinancialDataCanChange Then Throw New Exception( _
                "Klaida. Operacija su turtu """ & _AssetName & """ negali būti pašalinta: " _
                & _ChronologyValidator.FinancialDataCanChangeExplanation)

        End Sub

        Private Sub CheckIfUpdateDateChanged()

            Dim myComm As New SQLCommand("CheckIfLongTermAssetOperationUpdateDateChanged")
            myComm.AddParam("?CD", _ID)

            Using myData As DataTable = myComm.Fetch
                If myData.Rows.Count < 1 OrElse CDateTimeSafe(myData.Rows(0).Item(0), _
                    Date.MinValue) = Date.MinValue Then Throw New Exception( _
                    "Klaida. Objektas, kurio ID=" & _ID.ToString & ", nerastas.")
                If DateTime.SpecifyKind(CDateTimeSafe(myData.Rows(0).Item(0), DateTime.UtcNow), _
                    DateTimeKind.Utc).ToLocalTime <> _UpdateDate Then Throw New Exception( _
                    "Turto '" & _AssetName & "' operacijos duomenų atnaujinimo data pasikeitė. " _
                    & "Teigtina, kad kitas vartotojas redagavo šį objektą.")
            End Using

        End Sub

#End Region

    End Class

End Namespace