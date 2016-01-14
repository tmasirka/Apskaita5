Namespace Goods

    <Serializable()> _
    Public Class GoodsInternalTransferItem
        Inherits BusinessBase(Of GoodsInternalTransferItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private _Guid As Guid = Guid.NewGuid
        Private _GoodsInfo As GoodsSummary = Nothing
        Private _Acquisition As GoodsOperationAcquisition = Nothing
        Private _Transfer As GoodsOperationTransfer = Nothing
        Private _Remarks As String = ""
        Private _Amount As Double = 0
        Private _UnitCost As Double = 0
        Private _TotalCost As Double = 0
        Private _FinancialDataCanChange As Boolean = True


        Public ReadOnly Property GoodsInfo() As GoodsSummary
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo
            End Get
        End Property

        Friend ReadOnly Property Acquisition() As GoodsOperationAcquisition
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Acquisition
            End Get
        End Property

        Friend ReadOnly Property Transfer() As GoodsOperationTransfer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer
            End Get
        End Property

        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.ID
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

        Public Property Amount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Amount, 6)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If Not FinancialDataCanChange Then Exit Property
                If CRound(_Amount, 6) <> CRound(value, 6) Then
                    _Amount = CRound(value, 6)
                    PropertyHasChanged()
                    _Acquisition.Ammount = _Amount
                    _Transfer.Amount = _Amount
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

        Public ReadOnly Property FinancialDataCanChange() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _FinancialDataCanChange AndAlso _
                    _Acquisition.OperationLimitations.FinancialDataCanChange AndAlso _
                    _Transfer.OperationLimitations.FinancialDataCanChange
            End Get
        End Property


        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse _Acquisition.IsDirty OrElse _Transfer.IsDirty
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
            Dim result As String = ""
            result = AddWithNewLine(result, _Acquisition.OperationLimitations. _
                FinancialDataCanChangeExplanation, False)
            result = AddWithNewLine(result, _Transfer.OperationLimitations. _
                FinancialDataCanChangeExplanation, False)
            Return result
        End Function

        Public Function ExplainAllLimitations() As String

            Dim _ConsignmentWasUsed As Boolean = _Acquisition.OperationLimitations.ConsignmentWasUsed
            Dim _FinancialDataCanChange As Boolean = _Acquisition.OperationLimitations.FinancialDataCanChange
            Dim _FinancialDataCanChangeExplanation As String = _Acquisition.OperationLimitations.FinancialDataCanChangeExplanation
            Dim _MinDateApplicable As Boolean = _Acquisition.OperationLimitations.MinDateApplicable
            Dim _MinDateExplanation As String = _Acquisition.OperationLimitations.MinDateExplanation
            Dim _MaxDateApplicable As Boolean = _Acquisition.OperationLimitations.MaxDateApplicable
            Dim _MaxDateExplanation As String = _Acquisition.OperationLimitations.MaxDateExplanation
            Dim _MinDate As Date = _Acquisition.OperationLimitations.MinDate
            Dim _MaxDate As Date = _Acquisition.OperationLimitations.MaxDate


            If Not _Transfer.OperationLimitations.FinancialDataCanChange Then
                _FinancialDataCanChange = False
                _FinancialDataCanChangeExplanation = AddWithNewLine(_FinancialDataCanChangeExplanation, _
                    _Transfer.OperationLimitations.FinancialDataCanChangeExplanation, False)
            End If

            If _Transfer.OperationLimitations.MinDateApplicable Then
                If _MinDateApplicable Then
                    If _Transfer.OperationLimitations.MinDate.Date > _MinDate.Date Then
                        _MinDateExplanation = _Transfer.OperationLimitations.MinDateExplanation
                        _MinDate = _Transfer.OperationLimitations.MinDate
                    End If
                Else
                    _MinDateApplicable = True
                    _MinDateExplanation = _Transfer.OperationLimitations.MinDateExplanation
                    _MinDate = _Transfer.OperationLimitations.MinDate
                End If
            End If

            If _Transfer.OperationLimitations.MaxDateApplicable Then
                If _MaxDateApplicable Then
                    If _Transfer.OperationLimitations.MaxDate.Date < _MaxDate.Date Then
                        _MaxDateExplanation = _Transfer.OperationLimitations.MaxDateExplanation
                        _MaxDate = _Transfer.OperationLimitations.MaxDate
                    End If
                Else
                    _MaxDateApplicable = True
                    _MaxDateExplanation = _Transfer.OperationLimitations.MaxDateExplanation
                    _MaxDate = _Transfer.OperationLimitations.MaxDate
                End If
            End If

            Dim result As String = ""

            If Not _FinancialDataCanChange Then
                result = AddWithNewLine(result, "Negalima keisti finansinių operacijos duomenų:", False)
                result = AddWithNewLine(result, _FinancialDataCanChangeExplanation, False)
            End If

            If _MinDateApplicable Then
                result = AddWithNewLine(result, _
                    "Mažiausia leidžiama operacijos data " & _MinDate.ToString("yyyy-MM-dd") & ":", False)
                result = AddWithNewLine(result, _MinDateExplanation, False)
            End If

            If _MaxDateApplicable Then
                result = AddWithNewLine(result, _
                    "Didžiausia leidžiama operacijos data " & _MaxDate.ToString("yyyy-MM-dd") & ":", False)
                result = AddWithNewLine(result, _MaxDateExplanation, False)
            End If

            Return result

        End Function


        Public Sub ReloadCostInfo(ByVal CostInfo As GoodsCostItem)

            If Not FinancialDataCanChange Then Exit Sub

            If CostInfo Is Nothing OrElse CostInfo.NotEnoughInStock Then
                _UnitCost = 0
                _TotalCost = 0
                _Acquisition.UnitCost = 0
                _Acquisition.TotalCost = 0
                _Transfer.SetCosts(0, 0)
                PropertyHasChanged("UnitCost")
                PropertyHasChanged("TotalCost")
                If Not CostInfo Is Nothing AndAlso CostInfo.NotEnoughInStock Then _
                    Throw New Exception("Klaida. Nepakanka prekės """ & _GoodsInfo.Name _
                    & """ kiekio sandėlyje.")
                Exit Sub
            End If

            If CostInfo.GoodsID <> _GoodsInfo.ID Then Throw New Exception( _
                "Klaida. Nesutampa prekių ID ir prekių partijos vertės ID.")
            If Not _Transfer.Warehouse Is Nothing AndAlso CostInfo.WarehouseID <> _Transfer.Warehouse.ID Then _
                Throw New Exception("Klaida. Nesutampa prekių ir prekių partijos sandėliai.")
            If CostInfo.Amount <> CRound(_Amount, 6) Then _
                Throw New Exception("Klaida. Nesutampa nurodytas prekių kiekis " & _
                "ir prekių kiekis, kuriam paskaičiuota vertė.")
            If CostInfo.ValuationMethod <> _GoodsInfo.ValuationMethod Then Throw New Exception( _
                "Klaida. Nesutampa prekių ir prekių partijos vertinimo metodas.")

            _UnitCost = CostInfo.UnitCosts
            _TotalCost = CostInfo.TotalCosts
            _Acquisition.UnitCost = CostInfo.UnitCosts
            _Acquisition.TotalCost = CostInfo.TotalCosts
            _Transfer.SetCosts(CostInfo.UnitCosts, CostInfo.TotalCosts)
            PropertyHasChanged("UnitCost")
            PropertyHasChanged("TotalCost")

        End Sub

        Friend Function RequiresJournalEntry() As Boolean
            Return (_GoodsInfo.AccountingMethod = Goods.GoodsAccountingMethod.Persistent)
        End Function


        Friend Sub SetDate(ByVal value As Date)
            If _Transfer.Date.Date <> value.Date OrElse _Acquisition.Date.Date <> value.Date Then
                _Acquisition.SetDate(value)
                _Transfer.SetDate(value)
                PropertyHasChanged()
            End If
        End Sub

        Friend Sub SetWarehouseFrom(ByVal value As WarehouseInfo)

            If Not (_Transfer.Warehouse Is Nothing AndAlso value Is Nothing) _
                AndAlso Not (Not _Transfer.Warehouse Is Nothing AndAlso Not value Is Nothing _
                AndAlso _Transfer.Warehouse.ID = value.ID) Then
                If Not _Transfer.OperationLimitations.FinancialDataCanChange Then Throw New Exception( _
                    "Klaida. Finansinių vidinio judėjimo duomenų, įskaitant sandėlius, keisrti neleidžiama." _
                    & vbCrLf & _Transfer.OperationLimitations.FinancialDataCanChangeExplanation)
                _Transfer.Warehouse = value
                PropertyHasChanged()
            End If

        End Sub

        Friend Sub SetDescription(ByVal value As String)
            If Not String.IsNullOrEmpty(_Remarks.Trim) Then value = value & "<#>" & _Remarks
            If _Transfer.Description.Trim <> value.Trim OrElse _Acquisition.Description.Trim <> value.Trim Then
                _Transfer.Description = value
                _Acquisition.Description = value
                PropertyHasChanged()
            End If
        End Sub

        Friend Sub SetWarehouseTo(ByVal value As WarehouseInfo)

            If Not (_Acquisition.Warehouse Is Nothing AndAlso value Is Nothing) _
                AndAlso Not (Not _Acquisition.Warehouse Is Nothing AndAlso Not value Is Nothing _
                AndAlso _Acquisition.Warehouse.ID = value.ID) Then
                If Not _Transfer.OperationLimitations.FinancialDataCanChange Then Throw New Exception( _
                    "Klaida. Finansinių vidinio judėjimo duomenų, įskaitant sandėlius, keisrti neleidžiama." _
                    & vbCrLf & _Transfer.OperationLimitations.FinancialDataCanChangeExplanation)
                If Not _Acquisition.OperationLimitations.FinancialDataCanChange Then Throw New Exception( _
                    "Klaida. Finansinių vidinio judėjimo duomenų, įskaitant sandėlius, keisrti neleidžiama." _
                    & vbCrLf & _Acquisition.OperationLimitations.FinancialDataCanChangeExplanation)
                _Acquisition.Warehouse = value
                If Not value Is Nothing AndAlso value.ID > 0 Then _
                    _Transfer.AccountGoodsCost = value.WarehouseAccount
                PropertyHasChanged()
            End If

        End Sub


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            Return _GoodsInfo.Name & " vidinis judėjimas"
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()
            ValidationRules.AddRule(AddressOf AmountValidation, New Validation.RuleArgs("Amount"))
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

            Dim ValObj As GoodsInternalTransferItem = DirectCast(target, GoodsInternalTransferItem)

            If Not CRound(ValObj._Amount, 6) > 0 Then
                e.Description = "Nenurodytas perleidžiamas kiekis prekei " _
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

        Public Shared Function NewGoodsInternalTransferItem(ByVal GoodsID As Integer) _
            As GoodsInternalTransferItem
            Return DataPortal.Create(Of GoodsInternalTransferItem)(New Criteria(GoodsID))
        End Function

        Friend Shared Function GetGoodsInternalTransferItem(ByVal obj As OperationPersistenceObject, _
            ByVal objList As List(Of OperationPersistenceObject), _
            ByVal LimitationsDataSource As DataTable, ByVal nFinancialDataCanChange As Boolean) As GoodsInternalTransferItem
            Return New GoodsInternalTransferItem(obj, objList, LimitationsDataSource, nFinancialDataCanChange)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub

        Private Sub New(ByVal obj As OperationPersistenceObject, _
            ByVal objList As List(Of OperationPersistenceObject), _
            ByVal LimitationsDataSource As DataTable, ByVal nFinancialDataCanChange As Boolean)
            MarkAsChild()
            Fetch(obj, objList, LimitationsDataSource, nFinancialDataCanChange)
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private mId As Integer
            Public ReadOnly Property Id() As Integer
                Get
                    Return mId
                End Get
            End Property
            Public Sub New(ByVal id As Integer)
                mId = id
            End Sub
        End Class

        <NonSerialized(), NotUndoable()> _
        Private _Consignment As ConsignmentPersistenceObject = Nothing
        <NonSerialized(), NotUndoable()> _
        Private _ConsignmentDiscards As ConsignmentDiscardPersistenceObjectList = Nothing


        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)

            MarkAsChild()

            _GoodsInfo = GoodsSummary.NewGoodsSummary(criteria.Id)
            _Acquisition = GoodsOperationAcquisition.NewGoodsOperationAcquisitionChild(_GoodsInfo)
            _Transfer = GoodsOperationTransfer.NewGoodsOperationTransferChild(_GoodsInfo)

            MarkNew()

            ValidationRules.CheckRules()

        End Sub


        Private Sub Fetch(ByVal obj As OperationPersistenceObject, _
            ByVal objList As List(Of OperationPersistenceObject), _
            ByVal LimitationsDataSource As DataTable, ByVal nFinancialDataCanChange As Boolean)

            _GoodsInfo = obj.GoodsInfo
            _Amount = obj.Amount
            _UnitCost = obj.UnitValue
            _TotalCost = obj.TotalValue
            If obj.Content.Split(New String() {"<#>"}, StringSplitOptions.RemoveEmptyEntries).Length > 1 Then _
                _Remarks = obj.Content.Split(New String() {"<#>"}, StringSplitOptions.RemoveEmptyEntries)(1)

            If obj.OperationType = GoodsOperationType.Acquisition Then

                _Acquisition = GoodsOperationAcquisition.GetGoodsOperationAcquisitionChild( _
                    obj, LimitationsDataSource)

                For Each o As OperationPersistenceObject In objList
                    If o.GoodsID = obj.GoodsID AndAlso o.OperationType = GoodsOperationType.Transfer Then
                        _Transfer = GoodsOperationTransfer.GetGoodsOperationTransferChild( _
                            o, LimitationsDataSource)
                    End If
                Next

            ElseIf obj.OperationType = GoodsOperationType.Transfer Then

                _Transfer = GoodsOperationTransfer.GetGoodsOperationTransferChild( _
                    obj, LimitationsDataSource)

                For Each o As OperationPersistenceObject In objList
                    If o.GoodsID = obj.GoodsID AndAlso o.OperationType = GoodsOperationType.Acquisition Then
                        _Acquisition = GoodsOperationAcquisition.GetGoodsOperationAcquisitionChild( _
                            o, LimitationsDataSource)
                    End If
                Next

            Else

                Throw New Exception("Klaida. Sugadinti duomenys. Vidinio judėjimo kompleksinei " _
                    & "operacijai priskirta prekių operacija, kurios tipas " _
                    & ConvertEnumHumanReadable(obj.OperationType) & ".")

            End If

            If RequiresJournalEntry() AndAlso Not nFinancialDataCanChange Then _FinancialDataCanChange = False

            MarkOld()

            ValidationRules.CheckRules()

        End Sub

        Friend Sub Update(ByVal parent As GoodsComplexOperationInternalTransfer)

            Dim operationContent As String = parent.Content
            If Not String.IsNullOrEmpty(_Remarks.Trim.Trim) Then operationContent = _
                operationContent & "<#>" & _Remarks.Trim

            _Acquisition.SaveChild(parent.JournalEntryID, parent.ID, parent.DocumentNumber, _
                operationContent, False, False)
            _Transfer.SaveChild(parent.JournalEntryID, parent.ID, operationContent, _
                parent.DocumentNumber, False, True, False)

            If Not IsNew AndAlso _Acquisition.OperationLimitations.FinancialDataCanChange AndAlso _
                Not _Transfer.OperationLimitations.FinancialDataCanChange Then
                ConsignmentPersistenceObjectList.UpdateWarehouse(_Acquisition.ID, _
                    _Acquisition.Warehouse.ID)
            ElseIf Not (Not IsNew AndAlso Not _Acquisition.OperationLimitations.FinancialDataCanChange) Then
                If IsNew Then
                    _Consignment.Insert(_Acquisition.ID, _Acquisition.Warehouse.ID)
                Else
                    _Consignment.Update(_Acquisition.Warehouse.ID)
                End If
                _ConsignmentDiscards.Update(_Transfer.ID)
            End If

            MarkOld()

        End Sub

        Friend Sub DeleteSelf()

            _Acquisition.DeleteGoodsOperationAcquisitionChild()
            _Transfer.DeleteGoodsOperationTransferChild()

            MarkNew()

        End Sub


        Friend Function CheckIfCanUpdate(ByVal LimitationsDataSource As DataTable, _
            ByVal ThrowOnInvalid As Boolean) As String

            Dim result As String = ""

            result = AddWithNewLine(result, _Acquisition.CheckIfCanUpdate( _
                LimitationsDataSource, ThrowOnInvalid), False)
            result = AddWithNewLine(result, _Transfer.CheckIfCanUpdate( _
                LimitationsDataSource, ThrowOnInvalid), False)

            Return result

        End Function

        Friend Function CheckIfCanDelete(ByVal LimitationsDataSource As DataTable, _
            ByVal ThrowOnInvalid As Boolean) As String

            If IsNew Then Return ""

            Dim result As String = ""

            result = AddWithNewLine(result, _Acquisition.CheckIfCanDelete( _
                LimitationsDataSource, ThrowOnInvalid), False)
            result = AddWithNewLine(result, _Transfer.CheckIfCanDelete( _
                LimitationsDataSource, ThrowOnInvalid), False)

            Return result

        End Function

        Friend Function GetBookEntryInternalList() As BookEntryInternalList

            Dim result As BookEntryInternalList = _
               BookEntryInternalList.NewBookEntryInternalList(BookEntryType.Debetas)

            If _GoodsInfo.AccountingMethod = Goods.GoodsAccountingMethod.Periodic Then Return result

            Dim DiscardAccountBookEntry As BookEntryInternal = _
                BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
            DiscardAccountBookEntry.Account = _Transfer.Warehouse.WarehouseAccount
            DiscardAccountBookEntry.Ammount = CRound(_Acquisition.TotalCost)

            result.Add(DiscardAccountBookEntry)

            Dim CostsAccountBookEntry As BookEntryInternal = _
                BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
            CostsAccountBookEntry.Account = _Acquisition.Warehouse.WarehouseAccount
            CostsAccountBookEntry.Ammount = CRound(_Acquisition.TotalCost)

            result.Add(CostsAccountBookEntry)

            Return result

        End Function

        Friend Sub PrepareConsignements()

            If Not IsNew AndAlso Not _Acquisition.OperationLimitations.FinancialDataCanChange Then Exit Sub

            Dim AvailableConsignments As ConsignmentPersistenceObjectList = _
                ConsignmentPersistenceObjectList.NewConsignmentPersistenceObjectList( _
                _GoodsInfo.ID, _Transfer.Warehouse.ID, _Transfer.ID, _Acquisition.ID, _
                (_GoodsInfo.ValuationMethod = Goods.GoodsValuationMethod.LIFO))

            AvailableConsignments.RemoveLateEntries(_Transfer.Date)

            Dim NewConsignmentDiscards As ConsignmentDiscardPersistenceObjectList = _
                ConsignmentDiscardPersistenceObjectList.NewConsignmentDiscardPersistenceObjectList( _
                AvailableConsignments, _Amount, _GoodsInfo.Name)

            If IsNew Then
                _ConsignmentDiscards = NewConsignmentDiscards
            Else
                _ConsignmentDiscards = ConsignmentDiscardPersistenceObjectList. _
                    GetConsignmentDiscardPersistenceObjectList(_Transfer.ID)
                _ConsignmentDiscards.MergeChangedList(NewConsignmentDiscards)
            End If

            _TotalCost = _ConsignmentDiscards.GetTotalValue
            _UnitCost = CRound(_TotalCost / _Amount, 6)
            _Transfer.Amount = _Amount
            _Transfer.SetCosts(_UnitCost, _TotalCost)
            _Acquisition.Ammount = _Amount
            _Acquisition.UnitCost = _UnitCost
            _Acquisition.TotalCost = _TotalCost

            _Consignment = _Acquisition.GetConsignmentPersistenceObject

        End Sub

        Friend Sub SetFinancialDataCanChange(ByVal value As Boolean)

            If value AndAlso RequiresJournalEntry() Then
                _FinancialDataCanChange = False
            Else
                _FinancialDataCanChange = True
            End If

        End Sub

#End Region

    End Class

End Namespace