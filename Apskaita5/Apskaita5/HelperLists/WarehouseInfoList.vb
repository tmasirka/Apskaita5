Namespace HelperLists

    <Serializable()> _
    Public Class WarehouseInfoList
        Inherits ReadOnlyListBase(Of WarehouseInfoList, WarehouseInfo)

#Region " Business Methods "

        Public Function GetItem(ByVal id As Integer) As WarehouseInfo
            For Each i As WarehouseInfo In Me
                If i.ID = id Then Return i
            Next
            Return Nothing
        End Function

        Public Function GetItem(ByVal name As String) As WarehouseInfo
            For Each i As WarehouseInfo In Me
                If i.Name.Trim.ToLower = name.Trim.ToLower Then Return i
            Next
            Return Nothing
        End Function

#End Region

#Region " Authorization Rules "

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("HelperLists.WarehouseInfoList1")
        End Function

#End Region

#Region " Factory Methods "

        Public Shared Function GetList() As WarehouseInfoList

            Dim result As WarehouseInfoList = CacheManager.GetItemFromCache(Of WarehouseInfoList)( _
                GetType(WarehouseInfoList), Nothing)

            If result Is Nothing Then
                result = DataPortal.Fetch(Of WarehouseInfoList)(New Criteria())
                CacheManager.AddCacheItem(GetType(WarehouseInfoList), result, Nothing)
            End If

            Return result

        End Function

        Public Shared Function GetCachedFilteredList(ByVal ShowEmpty As Boolean, _
            ByVal ShowObsolete As Boolean) As Csla.FilteredBindingList(Of WarehouseInfo)

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka šiai informacijai gauti.")

            Dim FilterToApply(1) As Object
            FilterToApply(0) = ConvertDbBoolean(ShowEmpty)
            FilterToApply(1) = ConvertDbBoolean(ShowObsolete)

            Dim result As Csla.FilteredBindingList(Of WarehouseInfo) = _
                CacheManager.GetItemFromCache(Of Csla.FilteredBindingList(Of WarehouseInfo)) _
                (GetType(WarehouseInfoList), FilterToApply)

            If result Is Nothing Then

                Dim BaseList As WarehouseInfoList = WarehouseInfoList.GetList
                result = New Csla.FilteredBindingList(Of WarehouseInfo)(BaseList, _
                AddressOf WarehouseInfoListFilter)
                result.ApplyFilter("", FilterToApply)
                CacheManager.AddCacheItem(GetType(WarehouseInfoList), result, FilterToApply)

            End If

            Return result

        End Function

        Public Shared Sub InvalidateCache()
            CacheManager.InvalidateCache(GetType(WarehouseInfoList))
        End Sub

        Public Shared Function CacheIsInvalidated() As Boolean
            Return CacheManager.CacheIsInvalidated(GetType(WarehouseInfoList))
        End Function

        Private Shared Function WarehouseInfoListFilter(ByVal item As Object, _
            ByVal filterValue As Object) As Boolean

            If filterValue Is Nothing OrElse Not TypeOf filterValue Is Object() _
                OrElse Not DirectCast(filterValue, Object()).Length > 0 Then Return True

            Dim ShowEmpty As Boolean = ConvertDbBoolean( _
                DirectCast(DirectCast(filterValue, Object())(0), Integer))
            Dim ShowObsolete As Boolean = ConvertDbBoolean( _
                DirectCast(DirectCast(filterValue, Object())(1), Integer))

            ' no criteria to apply
            If ShowEmpty AndAlso ShowObsolete Then Return True

            Dim CI As WarehouseInfo = DirectCast(item, WarehouseInfo)

            If Not ShowEmpty AndAlso Not CI.ID > 0 Then Return False
            If Not ShowObsolete AndAlso CI.ID > 0 AndAlso CI.IsObsolete Then Return False

            Return True

        End Function

        Private Shared Function IsApplicationWideCache() As Boolean
            Return False
        End Function

        Private Shared Function GetListOnServer() As WarehouseInfoList
            Dim result As New WarehouseInfoList
            result.DataPortal_Fetch(New Criteria)
            Return result
        End Function

        Friend Shared Function GetListServerSide() As WarehouseInfoList
            Dim result As New WarehouseInfoList
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
                "Klaida. Jūsų teisių nepakanka šiems duomenims gauti.")

            Dim myComm As New SQLCommand("FetchWarehouseInfoList")

            Using myData As DataTable = myComm.Fetch

                RaiseListChangedEvents = False
                IsReadOnly = False

                Add(WarehouseInfo.NewWarehouseInfo)

                For Each dr As DataRow In myData.Rows
                    Add(WarehouseInfo.GetWarehouseInfo(dr, 0))
                Next

                IsReadOnly = True
                RaiseListChangedEvents = True

            End Using

        End Sub

#End Region

    End Class

End Namespace