Namespace HelperLists

    ''' <summary>
    ''' Represents an immutable collection of value objects for work (or rest) time class (type) for the company.
    ''' </summary>
    ''' <remarks>Corresponds to <see cref="Workers.WorkTimeClassList">Workers.WorkTimeClassList</see>.
    ''' Values are stored in the database table worktimeclasss.</remarks>
    <Serializable()> _
    Public NotInheritable Class WorkTimeClassInfoList
        Inherits ReadOnlyListBase(Of WorkTimeClassInfoList, WorkTimeClassInfo)

#Region " Business Methods "

        ''' <summary>
        ''' Finds an item with the code specified.
        ''' </summary>
        ''' <param name="typeCode">A code to find.</param>
        ''' <remarks></remarks>
        Public Function GetItemByCode(ByVal typeCode As String) As WorkTimeClassInfo
            If typeCode Is Nothing Then typeCode = ""
            For Each w As WorkTimeClassInfo In Me
                If w.Code.Trim.ToUpper = typeCode.Trim.ToUpper Then Return w
            Next
            Return Nothing
        End Function

#End Region

#Region " Authorization Rules "

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("HelperLists.WorkTimeClassInfoList1")
        End Function

#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets the WorkTimeClassInfoList for the company from a database.
        ''' </summary>
        ''' <remarks>Result is cached.
        ''' Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Public Shared Function GetList() As WorkTimeClassInfoList

            Dim result As WorkTimeClassInfoList = CacheManager.GetItemFromCache(Of WorkTimeClassInfoList)( _
                GetType(WorkTimeClassInfoList), Nothing)

            If result Is Nothing Then
                result = DataPortal.Fetch(Of WorkTimeClassInfoList)(New Criteria())
                CacheManager.AddCacheItem(GetType(WorkTimeClassInfoList), result, Nothing)
            End If

            Return result

        End Function

        ''' <summary>
        ''' Gets a filtered view of the work time class value object list.
        ''' </summary>
        ''' <param name="showEmpty">Wheather to include a placeholder object.</param>
        ''' <param name="showWithHours">Whether to include items where <see cref="WorkTimeClassInfo.WithoutWorkHours">WithoutWorkHours</see>=TRUE.</param>
        ''' <param name="showWithoutHours">Whether to include items where <see cref="WorkTimeClassInfo.WithoutWorkHours">WithoutWorkHours</see>=FALSE.</param>
        ''' <remarks>Result is cached.
        ''' Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Public Shared Function GetCachedFilteredList(ByVal showEmpty As Boolean, _
            ByVal showWithoutHours As Boolean, ByVal showWithHours As Boolean) _
            As Csla.FilteredBindingList(Of WorkTimeClassInfo)

            Dim filterToApply(2) As Object
            filterToApply(0) = ConvertDbBoolean(showEmpty)
            filterToApply(1) = ConvertDbBoolean(showWithoutHours)
            filterToApply(2) = ConvertDbBoolean(showWithHours)

            Dim result As Csla.FilteredBindingList(Of WorkTimeClassInfo) = _
                CacheManager.GetItemFromCache(Of Csla.FilteredBindingList(Of WorkTimeClassInfo)) _
                (GetType(WorkTimeClassInfoList), filterToApply)

            If result Is Nothing Then

                Dim baseList As WorkTimeClassInfoList = WorkTimeClassInfoList.GetList
                result = New Csla.FilteredBindingList(Of WorkTimeClassInfo) _
                    (baseList, AddressOf WorkTimeClassInfoFilter)
                result.ApplyFilter("", filterToApply)
                CacheManager.AddCacheItem(GetType(WorkTimeClassInfoList), result, filterToApply)

            End If

            Return result

        End Function

        Private Shared Function WorkTimeClassInfoFilter(ByVal item As Object, ByVal filterValue As Object) As Boolean

            If filterValue Is Nothing OrElse DirectCast(filterValue, Object()).Length < 1 Then Return True

            Dim showEmpty As Boolean = ConvertDbBoolean( _
                DirectCast(DirectCast(filterValue, Object())(0), Integer))
            Dim showWithoutHours As Boolean = ConvertDbBoolean( _
                DirectCast(DirectCast(filterValue, Object())(1), Integer))
            Dim showWithHours As Boolean = ConvertDbBoolean( _
                DirectCast(DirectCast(filterValue, Object())(2), Integer))

            If showEmpty AndAlso showWithoutHours AndAlso showWithHours Then Return True

            If Not showEmpty AndAlso Not DirectCast(item, WorkTimeClassInfo).ID > 0 Then Return False
            If DirectCast(item, WorkTimeClassInfo).ID > 0 Then
                If Not showWithoutHours AndAlso DirectCast(item, WorkTimeClassInfo).WithoutWorkHours Then Return False
                If Not showWithHours AndAlso Not DirectCast(item, WorkTimeClassInfo).WithoutWorkHours Then Return False
            End If

            Return True

        End Function

        ''' <summary>
        ''' Invalidates the current work time class value object list cache 
        ''' so that the next <see cref="GetList">GetList</see> call would hit the database.
        ''' </summary>
        ''' <remarks>Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Public Shared Sub InvalidateCache()
            CacheManager.InvalidateCache(GetType(WorkTimeClassInfoList))
        End Sub

        ''' <summary>
        ''' Returnes true if the cache does not contain a current work time class value object list.
        ''' </summary>
        ''' <remarks>Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Public Shared Function CacheIsInvalidated() As Boolean
            Return CacheManager.CacheIsInvalidated(GetType(WorkTimeClassInfoList))
        End Function

        ''' <summary>
        ''' Returns true if the collection is common across all the databases.
        ''' I.e. cache is not to be cleared on changing databases.
        ''' </summary>
        ''' <remarks>Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Private Shared Function IsApplicationWideCache() As Boolean
            Return False
        End Function

        ''' <summary>
        ''' Gets a current work time class value object list from database bypassing dataportal.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Should only be called server side.
        ''' Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Private Shared Function GetListOnServer() As WorkTimeClassInfoList
            Dim result As New WorkTimeClassInfoList
            result.DataPortal_Fetch(New Criteria)
            Return result
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

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecuritySelectDenied)

            Dim myComm As New SQLCommand("FetchWorkTimeClassInfoList")

            Using myData As DataTable = myComm.Fetch

                RaiseListChangedEvents = False
                IsReadOnly = False

                Add(WorkTimeClassInfo.Empty)

                For Each dr As DataRow In myData.Rows
                    Add(WorkTimeClassInfo.GetWorkTimeClassInfo(dr, 0))
                Next

                IsReadOnly = True
                RaiseListChangedEvents = True

            End Using

        End Sub

#End Region

    End Class

End Namespace