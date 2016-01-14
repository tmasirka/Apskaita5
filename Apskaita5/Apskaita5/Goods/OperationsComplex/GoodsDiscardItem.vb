Namespace Goods

    <Serializable()> _
    Public Class GoodsDiscardItem
        Inherits BusinessBase(Of GoodsDiscardItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private _Guid As Guid = Guid.NewGuid
        Private _GoodsInfo As GoodsSummary = Nothing
        Private _Discard As GoodsOperationDiscard = Nothing
        Private _Amount As Double = 0
        Private _UnitCost As Double = 0
        Private _TotalCost As Double = 0
        Private _AccountCost As Long = 0
        Private _Remarks As String = ""
        Private _FinancialDataCanChange As Boolean = True


        Public ReadOnly Property GoodsInfo() As GoodsSummary
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo
            End Get
        End Property

        Public ReadOnly Property GoodsName() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo.Name
            End Get
        End Property

        Public ReadOnly Property GoodsMeasureUnit() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo.MeasureUnit
            End Get
        End Property

        Public ReadOnly Property GoodsAccountingMethod() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo.AccountingMethodHumanReadable
            End Get
        End Property

        Public ReadOnly Property GoodsValuationMethod() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo.ValuationMethodHumanReadable
            End Get
        End Property

        Friend ReadOnly Property Discard() As GoodsOperationDiscard
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Discard
            End Get
        End Property

        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Discard.ID
            End Get
        End Property

        Public Property Amount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Amount, 6)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                If Not FinancialDataCanChange() Then Exit Property
                CanWriteProperty(True)
                If CRound(_Amount, 6) <> CRound(value, 6) Then
                    _Amount = CRound(value, 6)
                    PropertyHasChanged()
                    _Discard.Ammount = _Amount
                    _UnitCost = 0
                    _TotalCost = 0
                    PropertyHasChanged("UnitCost")
                    PropertyHasChanged("TotalCost")
                End If
            End Set
        End Property

        Public ReadOnly Property UnitCost() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitCost, 6)
            End Get
        End Property

        Public ReadOnly Property TotalCost() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalCost)
            End Get
        End Property

        Public Property AccountCost() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountCost
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If Not FinancialDataCanChange() Then Exit Property
                If _AccountCost <> value Then
                    _AccountCost = value
                    _Discard.AccountGoodsDiscardCosts = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property Remarks() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Remarks.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Remarks.Trim <> value.Trim Then
                    _Remarks = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property


        Public ReadOnly Property FinancialDataCanChange() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _FinancialDataCanChange AndAlso _
                    _Discard.OperationLimitations.FinancialDataCanChange
            End Get
        End Property


        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse _Discard.IsDirty
            End Get
        End Property


        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString
            If IsValid Then Return ""
            Return "Klaida (-os) eilutėje '" & _GoodsInfo.Name & "': " _
                & Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error)
        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString
            If BrokenRulesCollection.WarningCount < 1 Then Return ""
            Return "Eilutėje '" & _GoodsInfo.Name & "' gali būti klaida: " _
                & Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning)
        End Function

        Public Function ExplainFinancialLimitations() As String
            Return _Discard.OperationLimitations.FinancialDataCanChangeExplanation
        End Function

        Public Function ExplainAllLimitations() As String
            Return _Discard.OperationLimitations.LimitsExplanation
        End Function


        Public Function RequiresJournalEntry() As Boolean
            Return (_GoodsInfo.AccountingMethod = Goods.GoodsAccountingMethod.Persistent)
        End Function


        Public Function GetChronologyValidator() As OperationalLimitList
            Return _Discard.OperationLimitations
        End Function


        Public Sub ReloadCostInfo(ByVal CostInfo As GoodsCostItem)

            If Not FinancialDataCanChange() Then Exit Sub

            If CostInfo Is Nothing OrElse CostInfo.NotEnoughInStock Then
                _UnitCost = 0
                _TotalCost = 0
                PropertyHasChanged("UnitCost")
                PropertyHasChanged("TotalCost")
                If Not CostInfo Is Nothing AndAlso CostInfo.NotEnoughInStock Then _
                    Throw New Exception("Klaida. Nepakanka prekės """ & _GoodsInfo.Name _
                    & """ kiekio sandėlyje.")
                Exit Sub
            End If

            If CostInfo.GoodsID <> _GoodsInfo.ID Then Throw New Exception( _
                "Klaida. Nesutampa prekių ID ir prekių partijos vertės ID.")
            If Not _Discard.Warehouse Is Nothing AndAlso CostInfo.WarehouseID <> _Discard.Warehouse.ID Then _
                Throw New Exception("Klaida. Nesutampa prekių ir prekių partijos sandėliai.")
            If CostInfo.Amount <> CRound(_Amount, 6) Then _
                Throw New Exception("Klaida. Nesutampa nurodytas prekių kiekis " & _
                "ir prekių kiekis, kuriam paskaičiuota vertė.")
            If CostInfo.ValuationMethod <> _GoodsInfo.ValuationMethod Then Throw New Exception( _
                "Klaida. Nesutampa prekių ir prekių partijos vertinimo metodas.")

            _UnitCost = CostInfo.UnitCosts
            _TotalCost = CostInfo.TotalCosts
            PropertyHasChanged("UnitCost")
            PropertyHasChanged("TotalCost")

        End Sub


        Friend Sub SetDate(ByVal value As Date)
            If _Discard.Date.Date <> value.Date Then
                _Discard.SetDate(value)
                PropertyHasChanged()
            End If
        End Sub

        Friend Sub SetWarehouse(ByVal value As WarehouseInfo)

            If Not (_Discard.Warehouse Is Nothing AndAlso value Is Nothing) _
                AndAlso Not (Not _Discard.Warehouse Is Nothing AndAlso Not value Is Nothing _
                AndAlso _Discard.Warehouse.ID = value.ID) Then

                If Not _Discard.OperationLimitations.FinancialDataCanChange Then Throw New Exception( _
                    "Klaida. Finansinių vidinio judėjimo duomenų, įskaitant sandėlius, keisrti neleidžiama." _
                    & vbCrLf & _Discard.OperationLimitations.FinancialDataCanChangeExplanation)

                _Discard.Warehouse = value
                PropertyHasChanged()

            End If

        End Sub

        Friend Sub SetDescription(ByVal value As String)
            If Not String.IsNullOrEmpty(_Remarks.Trim) Then value = value & "<#>" & _Remarks
            If _Discard.Description.Trim <> value.Trim Then
                _Discard.Description = value
                PropertyHasChanged()
            End If
        End Sub


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            Return _GoodsInfo.Name & " nurašymas"
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf AmountValidation, New Validation.RuleArgs("Amount"))
            ValidationRules.AddRule(AddressOf AccountCostValidation, New Validation.RuleArgs("AccountCost"))

        End Sub

        ''' <summary>
        ''' Rule ensuring that the value of property Amount is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AmountValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As GoodsDiscardItem = DirectCast(target, GoodsDiscardItem)

            If Not CRound(ValObj._Amount, 6) > 0 Then
                e.Description = "Nenurodytas nurašomas kiekis prekei " _
                    & ValObj._GoodsInfo.Name & "."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that the value of property AccountCost is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AccountCostValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As GoodsDiscardItem = DirectCast(target, GoodsDiscardItem)

            If ValObj._GoodsInfo.AccountingMethod = Goods.GoodsAccountingMethod.Persistent AndAlso _
                Not ValObj._AccountCost > 0 Then
                e.Description = "Nenurodyta sąnaudų sąskaita prekei " _
                    & ValObj._GoodsInfo.Name & "."
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

        Public Shared Function NewGoodsDiscardItem(ByVal GoodsID As Integer, _
            ByVal parentValidator As IChronologicValidator) As GoodsDiscardItem
            Return DataPortal.Create(Of GoodsDiscardItem)(New Criteria(GoodsID, parentValidator))
        End Function

        Friend Shared Function GetGoodsDiscardItem(ByVal obj As OperationPersistenceObject, _
            ByVal parentValidator As IChronologicValidator, ByVal LimitationsDataSource As DataTable) As GoodsDiscardItem
            Return New GoodsDiscardItem(obj, parentValidator, LimitationsDataSource)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub

        Private Sub New(ByVal obj As OperationPersistenceObject, _
            ByVal parentValidator As IChronologicValidator, _
            ByVal LimitationsDataSource As DataTable)
            MarkAsChild()
            Fetch(obj, parentValidator, LimitationsDataSource)
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private mId As Integer
            Private mParentValidator As IChronologicValidator
            Public ReadOnly Property Id() As Integer
                Get
                    Return mId
                End Get
            End Property
            Public ReadOnly Property ParentValidator() As IChronologicValidator
                Get
                    Return mParentValidator
                End Get
            End Property
            Public Sub New(ByVal id As Integer, ByVal nParentValidator As IChronologicValidator)
                mId = id
                mParentValidator = nParentValidator
            End Sub
        End Class


        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)

            MarkAsChild()

            _GoodsInfo = GoodsSummary.NewGoodsSummary(criteria.Id)
            _Discard = GoodsOperationDiscard.NewGoodsOperationDiscardChild( _
                _GoodsInfo, criteria.ParentValidator)

            MarkNew()

            ValidationRules.CheckRules()

        End Sub

        Private Sub Fetch(ByVal obj As OperationPersistenceObject, _
            ByVal parentValidator As IChronologicValidator, _
            ByVal LimitationsDataSource As DataTable)

            _GoodsInfo = obj.GoodsInfo
            _Amount = -obj.Amount
            _UnitCost = obj.UnitValue
            _TotalCost = -obj.TotalValue
            _AccountCost = obj.AccountOperation
            If obj.Content.Split(New String() {"<#>"}, StringSplitOptions.RemoveEmptyEntries).Length > 1 Then _
                _Remarks = obj.Content.Split(New String() {"<#>"}, StringSplitOptions.RemoveEmptyEntries)(1)
            _Discard = GoodsOperationDiscard.GetGoodsOperationDiscardChild( _
                obj, parentValidator, LimitationsDataSource)

            If RequiresJournalEntry() AndAlso Not parentValidator.FinancialDataCanChange Then _
                _FinancialDataCanChange = False

            MarkOld()

            ValidationRules.CheckRules()

        End Sub


        Friend Sub Update(ByVal parent As GoodsComplexOperationDiscard)

            Dim operationContent As String = parent.Content
            If Not String.IsNullOrEmpty(_Remarks.Trim.Trim) Then operationContent = _
                operationContent & "<#>" & _Remarks.Trim

            _Discard.SaveChild(parent.JournalEntryID, parent.ID, parent.Date, _
                operationContent, parent.DocumentNumber)

            MarkOld()

        End Sub

        Friend Sub DeleteSelf()
            _Discard.DeleteGoodsOperationDiscardChild()
            MarkNew()
        End Sub


        Friend Function CheckIfCanUpdate(ByVal LimitationsDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator, ByVal ThrowOnInvalid As Boolean) As String
            Return _Discard.CheckIfCanUpdate(LimitationsDataSource, parentValidator, ThrowOnInvalid)
        End Function

        Friend Function CheckIfCanDelete(ByVal LimitationsDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator, ByVal ThrowOnInvalid As Boolean) As String
            If IsNew Then Return ""
            Return _Discard.CheckIfCanDelete(LimitationsDataSource, parentValidator, ThrowOnInvalid)
        End Function

        Friend Function GetBookEntryInternalList() As BookEntryInternalList
            Return _Discard.GetTotalBookEntryList
        End Function

        Friend Sub PrepareConsignements()

            If Not IsNew AndAlso Not _Discard.OperationLimitations.FinancialDataCanChange Then Exit Sub

            _Discard.GetDiscardList()

            _TotalCost = _Discard.TotalCost
            _UnitCost = _Discard.UnitCost

        End Sub

#End Region

    End Class

End Namespace