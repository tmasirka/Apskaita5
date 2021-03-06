Imports ApskaitaObjects.My.Resources
Imports Csla.Validation
Namespace Goods

    ''' <summary>
    ''' Represents a complex goods discard operation, that is composed of a collection 
    ''' of <see cref="GoodsOperationDiscard">simple goods discard operations</see>, 
    ''' i.e. discards multiple goods.
    ''' </summary>
    ''' <remarks>See methodology for BAS No 9 ""Stores"" para. 40 for details.
    ''' Encapsulates a <see cref="General.JournalEntry">JournalEntry</see>
    ''' of type <see cref="DocumentType.GoodsWriteOff">DocumentType.GoodsWriteOff</see>.
    ''' Values are stored using <see cref="ComplexOperationPersistenceObject">ComplexOperationPersistenceObject</see>.</remarks>
    <Serializable()> _
    Public NotInheritable Class GoodsComplexOperationDiscard
        Inherits BusinessBase(Of GoodsComplexOperationDiscard)
        Implements IIsDirtyEnough, IValidationMessageProvider

#Region " Business Methods "

        Private _ID As Integer = 0
        Private _JournalEntryID As Integer = 0
        Private _OperationalLimit As ComplexChronologicValidator = Nothing
        Private _Date As Date = Today
        Private _DocumentNumber As String = ""
        Private _Content As String = ""
        Private _OldWarehouseID As Integer = 0
        Private _Warehouse As WarehouseInfo = Nothing
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now
        Private WithEvents _Items As GoodsDiscardItemList

        <NotUndoable()> _
        <NonSerialized()> _
        Private _ItemsSortedList As Csla.SortedBindingList(Of GoodsOperationDiscard) = Nothing


        ''' <summary>
        ''' Gets an ID of the operation that is assigned by a database (AUTOINCREMENT).
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ComplexOperationPersistenceObject.ID">ComplexOperationPersistenceObject.ID</see>.</remarks>
        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the operation was inserted into the database.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ComplexOperationPersistenceObject.InsertDate">ComplexOperationPersistenceObject.InsertDate</see>.</remarks>
        Public ReadOnly Property InsertDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _InsertDate
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the operation was last updated.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ComplexOperationPersistenceObject.UpdateDate">ComplexOperationPersistenceObject.UpdateDate</see>.</remarks>
        Public ReadOnly Property UpdateDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UpdateDate
            End Get
        End Property

        ''' <summary>
        ''' Gets an <see cref="General.JournalEntry.ID">ID of the journal entry</see>
        ''' that is encapsulated by the operation.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ComplexOperationPersistenceObject.JournalEntryID">ComplexOperationPersistenceObject.JournalEntryID</see>.</remarks>
        Public ReadOnly Property JournalEntryID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryID
            End Get
        End Property

        ''' <summary>
        ''' Gets <see cref="IChronologicValidator">IChronologicValidator</see> object 
        ''' that contains business restraints on updating the operation data.
        ''' </summary>
        ''' <remarks>A <see cref="ComplexChronologicValidator">ComplexChronologicValidator</see> 
        ''' is used to validate a complex goods operation chronological business rules.</remarks>
        Public ReadOnly Property OperationalLimit() As ComplexChronologicValidator
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OperationalLimit
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a date of the operation.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ComplexOperationPersistenceObject.OperationDate">ComplexOperationPersistenceObject.OperationDate</see>.</remarks>
        Public Property [Date]() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Date
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Date)
                CanWriteProperty(True)
                If _Date.Date <> value.Date Then
                    _Date = value.Date
                    _Items.SetParentDate(value.Date)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a document number of the operation.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ComplexOperationPersistenceObject.DocNo">ComplexOperationPersistenceObject.DocNo</see>.</remarks>
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
        ''' Gets or sets a content (description) of the operation.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ComplexOperationPersistenceObject.Content">ComplexOperationPersistenceObject.Content</see>.</remarks>
        <StringField(ValueRequiredLevel.Mandatory, 255)> _
        Public Property Content() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Content.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Content.Trim <> value.Trim Then
                    _Content = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public ReadOnly Property OldWarehouseID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OldWarehouseID
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a <see cref="Goods.Warehouse">warehouse</see> that the goods 
        ''' are discarded from.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ComplexOperationPersistenceObject.Warehouse">ComplexOperationPersistenceObject.Warehouse</see>.</remarks>
        <WarehouseField(ValueRequiredLevel.Mandatory)> _
        Public Property Warehouse() As WarehouseInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Warehouse
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WarehouseInfo)
                CanWriteProperty(True)
                If WarehouseIsReadOnly Then Exit Property

                If Not (_Warehouse Is Nothing AndAlso value Is Nothing) _
                    AndAlso Not (Not _Warehouse Is Nothing AndAlso Not value Is Nothing _
                    AndAlso _Warehouse.ID = value.ID) Then

                    If Not value Is Nothing AndAlso Not value.IsEmpty Then
                        _Items.SetParentWarehouse(value)
                        _OperationalLimit = ComplexChronologicValidator.GetComplexChronologicValidator( _
                            _OperationalLimit, _Items.GetLimitations())
                        PropertyHasChanged("OperationalLimit")
                    End If

                    _Warehouse = value
                    PropertyHasChanged()

                End If

            End Set
        End Property

        ''' <summary>
        ''' Gets a collection of goods discard items within the operation.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Items() As GoodsDiscardItemList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Items
            End Get
        End Property

        ''' <summary>
        ''' Gets a sortable collection of goods discard items within the operation.
        ''' </summary>
        ''' <remarks>Used to implement autosort in a datagridview.</remarks>
        Public ReadOnly Property ItemsSorted() As Csla.SortedBindingList(Of GoodsOperationDiscard)
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _ItemsSortedList Is Nothing Then
                    _ItemsSortedList = New Csla.SortedBindingList(Of GoodsOperationDiscard)(_Items)
                End If
                Return _ItemsSortedList
            End Get
        End Property


        ''' <summary>
        ''' Whether the <see cref="Warehouse">Warehouse</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property WarehouseIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _OperationalLimit.FinancialDataCanChange _
                    OrElse Not _OperationalLimit.ChildrenFinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="RefreshCosts">RefreshCosts</see> method could be invoked,
        ''' i.e. <see cref="GoodsOperationDiscard.UnitCost">UnitCost</see> and 
        ''' <see cref="GoodsOperationDiscard.TotalCost">TotalCost</see> can be set.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property RefreshCostsIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _OperationalLimit.FinancialDataCanChange
            End Get
        End Property


        Public ReadOnly Property IsDirtyEnough() As Boolean _
            Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Not IsNew Then Return IsDirty
                Return (Not String.IsNullOrEmpty(_DocumentNumber.Trim) _
                    OrElse Not String.IsNullOrEmpty(_Content.Trim) _
                    OrElse _Items.Count > 0)
            End Get
        End Property

        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse _Items.IsDirty
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean _
            Implements IValidationMessageProvider.IsValid
            Get
                Return MyBase.IsValid AndAlso _Items.IsValid
            End Get
        End Property



        Public Function GetAllBrokenRules() As String _
            Implements IValidationMessageProvider.GetAllBrokenRules
            Dim result As String = ""
            If Not MyBase.IsValid Then
                result = AddWithNewLine(result, _
                    Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error), False)
            End If
            If Not _Items.IsValid Then
                result = AddWithNewLine(result, _Items.GetAllBrokenRules(), False)
            End If
            Return result
        End Function

        Public Function GetAllWarnings() As String _
            Implements IValidationMessageProvider.GetAllWarnings
            Dim result As String = ""
            If MyBase.BrokenRulesCollection.WarningCount > 0 Then
                result = AddWithNewLine(result, _
                    Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning), False)
            End If
            If _Items.HasWarnings() Then
                result = AddWithNewLine(result, _Items.GetAllWarnings(), False)
            End If
            Return result
        End Function

        Public Function HasWarnings() As Boolean _
            Implements IValidationMessageProvider.HasWarnings
            Return (MyBase.BrokenRulesCollection.WarningCount > 0 OrElse _Items.HasWarnings())
        End Function


        ''' <summary>
        ''' Gets an array of param objects for a <see cref="GoodsCostItemList">query object</see>.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetCostsParams() As GoodsCostParam()
            If Not _OperationalLimit.FinancialDataCanChange Then
                Throw New Exception(String.Format(Goods_GoodsComplexOperationDiscard_CannotChangeFinancialDataParent, _
                    vbCrLf, _OperationalLimit.FinancialDataCanChangeExplanation))
            End If
            If _Warehouse Is Nothing OrElse _Warehouse.IsEmpty Then
                Throw New Exception(Goods_GoodsOperationDiscard_WarehouseNull)
            End If
            Return _Items.GetGoodsCostsParams()
        End Function

        ''' <summary>
        ''' Sets costs of the goods to discard using <see cref="GoodsCostItemList">
        ''' query object</see>.
        ''' </summary>
        ''' <param name="values">a query object containing information about the costs 
        ''' of the goods for a given amount, warehouse and date.</param>
        ''' <param name="warnings">an out parameter that returns a description of 
        ''' non critical errors encountered while seting the data</param>
        ''' <remarks></remarks>
        Public Sub RefreshCosts(ByVal values As GoodsCostItemList, _
            ByRef warnings As String)

            warnings = ""

            If Not _OperationalLimit.FinancialDataCanChange Then
                Throw New Exception(String.Format(Goods_GoodsComplexOperationDiscard_CannotChangeFinancialDataParent, _
                    vbCrLf, _OperationalLimit.FinancialDataCanChangeExplanation))
            End If

            _Items.RefreshCosts(values, warnings)

        End Sub

        ''' <summary>
        ''' Sets costs of the goods to discard using <see cref="GoodsCostItem">
        ''' query object</see>.
        ''' </summary>
        ''' <param name="value">a query object containing information about the costs 
        ''' of the given goods for a given amount, warehouse and date.</param>
        ''' <remarks></remarks>
        Public Sub RefreshCosts(ByVal value As GoodsCostItem)

            If Not _OperationalLimit.FinancialDataCanChange Then
                Throw New Exception(String.Format(Goods_GoodsComplexOperationDiscard_CannotChangeFinancialDataParent, _
                    vbCrLf, _OperationalLimit.FinancialDataCanChangeExplanation))
            End If

            _Items.RefreshCosts(value)

        End Sub

        ''' <summary>
        ''' Sets the same <see cref="GoodsOperationDiscard.AccountGoodsDiscardCosts">
        ''' goods discard costs account</see> for all the discard items within the document.
        ''' </summary>
        ''' <param name="accountCosts">an <see cref="General.Account.ID">account</see> to set</param>
        ''' <remarks></remarks>
        Public Sub SetCommonAccountDiscardCosts(ByVal accountCosts As Long)

            If Not _OperationalLimit.ChildrenFinancialDataCanChange Then
                Throw New Exception(String.Format(Goods_GoodsComplexOperationDiscard_CannotChangeFinancialDataChildren, _
                    vbCrLf, _OperationalLimit.ChildrenFinancialDataCanChangeExplanation))
            ElseIf Not _OperationalLimit.FinancialDataCanChange Then
                Throw New Exception(String.Format(Goods_GoodsComplexOperationDiscard_CannotChangeFinancialDataParent, _
                    vbCrLf, _OperationalLimit.FinancialDataCanChangeExplanation))
            End If

            _Items.SetCommonAccountDiscardCosts(accountCosts)

        End Sub

        ''' <summary>
        ''' Adds items in the list to the current collection.
        ''' </summary>
        ''' <param name="list"></param>
        ''' <remarks>Invoke <see cref="GoodsDiscardItemList.NewGoodsDiscardItemList">GoodsDiscardItemList.NewGoodsDiscardItemList</see>
        ''' to get a list of new child operations by goods ID's.</remarks>
        Public Sub AddRange(ByVal list As GoodsDiscardItemList)

            If Not _OperationalLimit.FinancialDataCanChange Then
                Throw New Exception(String.Format(Goods_GoodsComplexOperationDiscard_CannotChangeFinancialDataParent, _
                    vbCrLf, _OperationalLimit.FinancialDataCanChangeExplanation))
            End If

            list.SetParentDate(_Date)
            list.SetParentWarehouse(_Warehouse)

            _Items.AddRange(list)

            For Each i As GoodsOperationDiscard In list
                _OperationalLimit.MergeNewValidationItem(i.OperationLimitations)
            Next

        End Sub


        Private Sub Items_Changed(ByVal sender As Object, _
            ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _Items.ListChanged

            If e.ListChangedType = ComponentModel.ListChangedType.ItemAdded Then

                Try
                    _OperationalLimit.MergeNewValidationItem(_Items(e.NewIndex).OperationLimitations)
                    PropertyHasChanged("OperationalLimit")
                Catch ex As Exception
                End Try

            ElseIf e.ListChangedType = ComponentModel.ListChangedType.ItemDeleted Then

                _OperationalLimit = ComplexChronologicValidator.GetComplexChronologicValidator( _
                    _OperationalLimit, _Items.GetLimitations())

                PropertyHasChanged("OperationalLimit")

            End If

        End Sub

        ''' <summary>
        ''' Helper method. Takes care of child lists loosing their handlers.
        ''' </summary>
        Protected Overrides Function GetClone() As Object
            Dim result As GoodsComplexOperationDiscard = DirectCast(MyBase.GetClone(), GoodsComplexOperationDiscard)
            result.RestoreChildListsHandles()
            Return result
        End Function

        Protected Overrides Sub OnDeserialized(ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            RestoreChildListsHandles()
        End Sub

        Protected Overrides Sub UndoChangesComplete()
            MyBase.UndoChangesComplete()
            RestoreChildListsHandles()
        End Sub

        ''' <summary>
        ''' Helper method. Takes care of ReportItems loosing its handler. See GetClone method.
        ''' </summary>
        Friend Sub RestoreChildListsHandles()
            Try
                RemoveHandler _Items.ListChanged, AddressOf Items_Changed
            Catch ex As Exception
            End Try
            AddHandler _Items.ListChanged, AddressOf Items_Changed
        End Sub


        Public Overrides Function Save() As GoodsComplexOperationDiscard
            Return MyBase.Save
        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _ID
        End Function

        Public Overrides Function ToString() As String
            Return String.Format(My.Resources.Goods_GoodsComplexOperationDiscard_ToString, _
                _Date.ToString("yyyy-MM-dd"), _DocumentNumber, _ID.ToString)
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New RuleArgs("DocumentNumber"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New RuleArgs("Description"))
            ValidationRules.AddRule(AddressOf CommonValidation.ValueObjectFieldValidation, _
                New Csla.Validation.RuleArgs("Warehouse"))

            ValidationRules.AddRule(AddressOf CommonValidation.ChronologyValidation, _
                New CommonValidation.ChronologyRuleArgs("Date", "OperationalLimit"))

            ValidationRules.AddDependantProperty("Warehouse", "Date", False)
            ValidationRules.AddDependantProperty("OperationalLimit", "Date", False)

        End Sub

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
            AuthorizationRules.AllowWrite("Goods.GoodsOperationDiscard1")
        End Sub

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationDiscard1")
        End Function

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationDiscard2")
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationDiscard3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationDiscard3")
        End Function

#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets a new GoodsComplexOperationDiscard instance.
        ''' </summary>
        ''' <param name="warehouseID">an <see cref="Goods.Warehouse.ID">ID of the warehouse</see>
        ''' to discard the goods from</param>
        ''' <remarks></remarks>
        Public Shared Function NewGoodsComplexOperationDiscard(ByVal warehouseID As Integer) As GoodsComplexOperationDiscard
            Return DataPortal.Create(Of GoodsComplexOperationDiscard)(New Criteria(warehouseID))
        End Function

        ''' <summary>
        ''' Gets an existing GoodsComplexOperationDiscard instance from a database.
        ''' </summary>
        ''' <param name="id">an <see cref="ID">ID of the operation</see> to fetch</param>
        ''' <remarks></remarks>
        Public Shared Function GetGoodsComplexOperationDiscard(ByVal id As Integer) As GoodsComplexOperationDiscard
            Return DataPortal.Fetch(Of GoodsComplexOperationDiscard)(New Criteria(id))
        End Function

        ''' <summary>
        ''' Deletes an existing GoodsComplexOperationDiscard instance from a database.
        ''' </summary>
        ''' <param name="id">an <see cref="ID">ID of the operation</see> to delete</param>
        ''' <remarks></remarks>
        Public Shared Sub DeleteGoodsComplexOperationDiscard(ByVal id As Integer)
            DataPortal.Delete(New Criteria(id))
        End Sub


        Private Sub New()
            ' require use of factory methods
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


        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)

            Create(criteria.ID)

        End Sub

        Private Sub Create(ByVal warehouseID As Integer)

            If warehouseID > 0 Then
                _Warehouse = WarehouseInfoList.GetListChild.GetItem(warehouseID, False)
            End If

            _Items = GoodsDiscardItemList.NewGoodsDiscardItemList

            Dim baseValidator As SimpleChronologicValidator = _
                SimpleChronologicValidator.NewSimpleChronologicValidator( _
                ConvertLocalizedName(GoodsComplexOperationType.BulkDiscard), Nothing)

            _OperationalLimit = ComplexChronologicValidator.NewComplexChronologicValidator( _
                 baseValidator.CurrentOperationName, baseValidator, Nothing, Nothing)

            ValidationRules.CheckRules()

        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecuritySelectDenied)

            Fetch(criteria.ID)

        End Sub

        Private Sub Fetch(ByVal operationID As Integer)

            Dim obj As ComplexOperationPersistenceObject = ComplexOperationPersistenceObject. _
                GetComplexOperationPersistenceObject(operationID, _
                GoodsComplexOperationType.BulkDiscard, True)

            Fetch(obj)

        End Sub

        Private Sub Fetch(ByVal obj As ComplexOperationPersistenceObject)

            _Content = obj.Content
            _DocumentNumber = obj.DocNo
            _ID = obj.ID
            _InsertDate = obj.InsertDate
            _JournalEntryID = obj.JournalEntryID
            _Date = obj.OperationDate
            _UpdateDate = obj.UpdateDate
            _Warehouse = obj.Warehouse

            Dim baseValidator As IChronologicValidator = SimpleChronologicValidator. _
                GetSimpleChronologicValidator(_ID, _Date, _
                ConvertLocalizedName(GoodsComplexOperationType.BulkDiscard), Nothing)

            Using myData As DataTable = OperationalLimitList.GetDataSourceForComplexOperation( _
                _ID, _Date)
                Dim objList As List(Of OperationPersistenceObject) = _
                    OperationPersistenceObject.GetOperationPersistenceObjectList(_ID)
                _Items = GoodsDiscardItemList.GetGoodsDiscardItemList(objList, baseValidator, myData)
            End Using

            _OperationalLimit = ComplexChronologicValidator.GetComplexChronologicValidator( _
                _ID, _Date, baseValidator.CurrentOperationName, baseValidator, _
                Nothing, _Items.GetLimitations())

            _OldWarehouseID = _Warehouse.ID

            MarkOld()

            ValidationRules.CheckRules()

        End Sub


        Protected Overrides Sub DataPortal_Insert()

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)

            CheckIfCanUpdate()
            DoSave()

        End Sub

        Protected Overrides Sub DataPortal_Update()

            If Not CanEditObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityUpdateDenied)

            CheckIfCanUpdate()
            DoSave()

        End Sub

        Private Sub DoSave()

            Dim obj As ComplexOperationPersistenceObject = GetPersistenceObj()

            Dim entry As General.JournalEntry = GetJournalEntry()

            Using transaction As New SqlTransaction

                Try

                    If Not entry Is Nothing Then
                        entry = entry.SaveChild
                        _JournalEntryID = entry.ID
                        obj.JournalEntryID = _JournalEntryID
                    ElseIf entry Is Nothing AndAlso Not IsNew AndAlso _JournalEntryID > 0 Then
                        General.JournalEntry.DeleteJournalEntryChild(_JournalEntryID)
                        _JournalEntryID = 0
                        obj.JournalEntryID = 0
                    End If

                    obj = obj.SaveChild(_OperationalLimit.FinancialDataCanChange, _
                        _OperationalLimit.FinancialDataCanChange, False)

                    If IsNew Then
                        _ID = obj.ID
                        _InsertDate = obj.InsertDate
                    End If
                    _UpdateDate = obj.UpdateDate

                    _Items.Update(Me)

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

            _OldWarehouseID = _Warehouse.ID

            MarkOld()

        End Sub

        Private Function GetPersistenceObj() As ComplexOperationPersistenceObject

            Dim obj As ComplexOperationPersistenceObject
            If IsNew Then
                obj = ComplexOperationPersistenceObject.NewComplexOperationPersistenceObject( _
                    GoodsComplexOperationType.BulkDiscard, 0)
            Else
                obj = ComplexOperationPersistenceObject.GetComplexOperationPersistenceObject( _
                    _ID, GoodsComplexOperationType.BulkDiscard)
                If obj.UpdateDate <> _UpdateDate Then Throw New Exception( _
                    My.Resources.Common_UpdateDateHasChanged)
            End If

            obj.AccountOperation = 0
            obj.Warehouse = _Warehouse
            obj.SecondaryWarehouse = Nothing
            obj.Content = _Content
            obj.DocNo = _DocumentNumber
            obj.JournalEntryID = _JournalEntryID
            obj.OperationDate = _Date

            Return obj

        End Function


        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_ID))
        End Sub

        Protected Overrides Sub DataPortal_Delete(ByVal criteria As Object)

            If Not CanDeleteObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityUpdateDenied)

            Dim operationToDelete As GoodsComplexOperationDiscard = New GoodsComplexOperationDiscard
            operationToDelete.Fetch(DirectCast(criteria, Criteria).ID)

            If Not operationToDelete._OperationalLimit.FinancialDataCanChange Then
                Throw New Exception(String.Format(Goods_GoodsComplexOperationDiscard_InvalidDelete, _
                    vbCrLf, operationToDelete._OperationalLimit.FinancialDataCanChangeExplanation))
            ElseIf Not operationToDelete._OperationalLimit.ChildrenFinancialDataCanChange Then
                Throw New Exception(String.Format(Goods_GoodsComplexOperationDiscard_InvalidDelete, _
                    vbCrLf, operationToDelete._OperationalLimit.ChildrenFinancialDataCanChangeExplanation))
            End If

            If operationToDelete.JournalEntryID > 0 Then
                IndirectRelationInfoList.CheckIfJournalEntryCanBeDeleted( _
                    operationToDelete.JournalEntryID, DocumentType.GoodsWriteOff)
            End If

            Using transaction As New SqlTransaction

                Try

                    ComplexOperationPersistenceObject.Delete(operationToDelete.ID, _
                        True, False, True)

                    If operationToDelete.JournalEntryID > 0 Then General.JournalEntry. _
                        DeleteJournalEntryChild(operationToDelete.JournalEntryID)

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

        End Sub


        Private Sub CheckIfCanUpdate()

            If Not _Items.Count > 0 Then
                Throw New Exception(Goods_GoodsComplexOperationDiscard_ListEmpty)
            End If

            ' just in case
            _Items.SetParentDate(_Date)
            If _OperationalLimit.FinancialDataCanChange AndAlso _
                _OperationalLimit.ChildrenFinancialDataCanChange Then
                _Items.SetParentWarehouse(_Warehouse)
            End If

            If IsNew Then
                _Items.CheckIfCanUpdate(Nothing, Me)
            Else
                Using myData As DataTable = OperationalLimitList.GetDataSourceForComplexOperation( _
                    _ID, _OperationalLimit.CurrentOperationDate)
                    _Items.CheckIfCanUpdate(myData, Me)
                End Using
            End If

            Me.ValidationRules.CheckRules()
            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

        End Sub

        Private Function GetJournalEntry() As General.JournalEntry

            Dim result As General.JournalEntry
            If IsNew OrElse Not _JournalEntryID > 0 Then
                result = General.JournalEntry.NewJournalEntryChild(DocumentType.GoodsWriteOff)
            Else
                result = General.JournalEntry.GetJournalEntryChild(_JournalEntryID, DocumentType.GoodsWriteOff)
            End If

            result.Content = _Content
            result.Date = _Date
            result.DocNumber = _DocumentNumber

            If _OperationalLimit.FinancialDataCanChange Then

                Dim fullBookEntryList As BookEntryInternalList = _Items.GetTotalBookEntryList()

                fullBookEntryList.Aggregate()

                If Not fullBookEntryList.Count > 0 Then Return Nothing

                result.DebetList.Clear()
                result.CreditList.Clear()

                result.DebetList.LoadBookEntryListFromInternalList(fullBookEntryList, False, False)
                result.CreditList.LoadBookEntryListFromInternalList(fullBookEntryList, False, False)

            End If

            If Not result.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_FailedToCreateJournalEntry, _
                    vbCrLf, result.ToString, vbCrLf, result.GetAllBrokenRules))
            End If

            Return result

        End Function

#End Region

    End Class

End Namespace