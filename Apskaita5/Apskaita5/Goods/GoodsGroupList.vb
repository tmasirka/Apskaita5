Namespace Goods

    ''' <summary>
    ''' Represents a custom goods group list for the company.
    ''' </summary>
    ''' <remarks>Exists a single instance per company.
    ''' Values are stored in the database table prekes_gr.</remarks>
    <Serializable()> _
    Public NotInheritable Class GoodsGroupList
        Inherits BusinessListBase(Of GoodsGroupList, GoodsGroup)
        Implements IIsDirtyEnough, IValidationMessageProvider

#Region " Business Methods "

        Public Overrides ReadOnly Property IsValid() As Boolean _
            Implements IValidationMessageProvider.IsValid
            Get
                Return MyBase.IsValid
            End Get
        End Property

        Public ReadOnly Property IsDirtyEnough() As Boolean _
            Implements IIsDirtyEnough.IsDirtyEnough
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Me.IsDirty
            End Get
        End Property


        Protected Overrides Function AddNewCore() As Object
            Dim newItem As GoodsGroup = GoodsGroup.NewGoodsGroup
            Me.Add(newItem)
            Return newItem
        End Function


        Public Function GetAllBrokenRules() As String _
            Implements IValidationMessageProvider.GetAllBrokenRules
            Dim result As String = GetAllBrokenRulesForList(Me)
            Return result
        End Function

        Public Function GetAllWarnings() As String _
            Implements IValidationMessageProvider.GetAllWarnings
            Dim result As String = GetAllWarningsForList(Me)
            Return result
        End Function

        Public Function HasWarnings() As Boolean _
            Implements IValidationMessageProvider.HasWarnings
            For Each i As GoodsGroup In Me
                If i.HasWarnings() Then Return True
            Next
            Return False
        End Function


        Protected Overrides Sub ClearItems()
            For Each w As GoodsGroup In Me
                If Not w.IsNew AndAlso w.IsInUse Then Throw New Exception(String.Format( _
                    My.Resources.Goods_GoodsGroupList_InvalidDelete, w.ToString))
            Next
            MyBase.ClearItems()
        End Sub

        Protected Overrides Sub RemoveItem(ByVal index As Integer)
            If Item(index).IsInUse Then Throw New Exception(String.Format( _
                My.Resources.Goods_GoodsGroupList_InvalidDelete, Item(index).ToString))
            MyBase.RemoveItem(index)
        End Sub


        Public Overrides Function Save() As GoodsGroupList
            Dim result As GoodsGroupList = MyBase.Save()
            HelperLists.GoodsGroupInfoList.InvalidateCache()
            Return result
        End Function

#End Region

#Region " Authorization Rules "

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsGroupList1")
        End Function

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsGroupList2")
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsGroupList3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsGroupList3")
        End Function

#End Region

#Region " Factory Methods "

        ' used to implement automatic sort in datagridview
        <NonSerialized()> _
        Private _SortedList As Csla.SortedBindingList(Of GoodsGroup) = Nothing

        ''' <summary>
        ''' Gets a current goods group list from a database.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function GetGoodsGroupList() As GoodsGroupList
            Return DataPortal.Fetch(Of GoodsGroupList)(New Criteria())
        End Function

        ''' <summary>
        ''' Gets a sortable view of the goods group list.
        ''' </summary>
        ''' <remarks>Used to implement auto sort in a datagridview.</remarks>
        Public Function GetSortedList() As Csla.SortedBindingList(Of GoodsGroup)
            If _SortedList Is Nothing Then
                _SortedList = New Csla.SortedBindingList(Of GoodsGroup)(Me)
            End If
            Return _SortedList
        End Function


        Private Sub New()
            ' require use of factory methods
            Me.AllowEdit = True
            Me.AllowNew = True
            Me.AllowRemove = True
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public Sub New()
            End Sub
        End Class


        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecuritySelectDenied)

            Dim myComm As New SQLCommand("FetchGoodsGroupList")

            Using myData As DataTable = myComm.Fetch

                RaiseListChangedEvents = False

                For Each dr As DataRow In myData.Rows
                    Add(GoodsGroup.GetGoodsGroup(dr))
                Next

                RaiseListChangedEvents = True

            End Using

        End Sub

        Protected Overrides Sub DataPortal_Update()

            If Not CanEditObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityUpdateDenied)

            Dim deletedCount As Integer = 0
            For Each g As GoodsGroup In Me.DeletedList
                If Not g.IsNew Then deletedCount += 1
            Next

            If Not Me.Count > 0 AndAlso Not deletedCount > 0 Then
                Throw New Exception(My.Resources.Goods_GoodsGroupList_ListEmpty)
            End If

            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            CheckIfCanDelete()

            Using transaction As New SqlTransaction

                Try

                    RaiseListChangedEvents = False

                    For Each g As GoodsGroup In DeletedList
                        If Not g.IsNew Then g.DeleteSelf()
                    Next
                    DeletedList.Clear()

                    For Each g As GoodsGroup In Me
                        If g.IsNew Then
                            g.Insert()
                        ElseIf g.IsDirty Then
                            g.Update()
                        End If
                    Next

                    RaiseListChangedEvents = True

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

        End Sub


        Private Sub CheckIfCanDelete()

            Dim myComm As New SQLCommand("CheckIfCanDeleteGoodsInfoList")

            Dim list As New List(Of Integer)

            Using myData As DataTable = myComm.Fetch

                For Each dr As DataRow In myData.Rows
                    If CIntSafe(dr.Item(0), 0) > 0 Then list.Add(CIntSafe(dr.Item(0), 0))
                Next

            End Using

            For Each g As GoodsGroup In DeletedList
                If Not g.IsNew AndAlso list.Contains(g.ID) Then
                    Throw New Exception(String.Format( _
                        My.Resources.Goods_GoodsGroupList_InvalidDelete, g.ToString))
                End If
            Next

        End Sub

#End Region

    End Class

End Namespace