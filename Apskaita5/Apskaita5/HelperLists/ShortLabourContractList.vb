Namespace HelperLists

    ''' <summary>
    ''' A value object list that provides serials, numbers and dates of existing labour contracts.
    ''' </summary>
    ''' <remarks>Should be used as a datasource for a labour contract identification.</remarks>
    <Serializable()> _
Public NotInheritable Class ShortLabourContractList
        Inherits ReadOnlyListBase(Of ShortLabourContractList, ShortLabourContract)

#Region " Business Methods "

        ''' <summary>
        ''' Finds a contract within the list. Returns nothing if no such contract within the list.
        ''' </summary>
        ''' <param name="serial">Serial to find.</param>
        ''' <param name="number">Number to find.</param>
        ''' <remarks></remarks>
        Public Function GetShortLabourContract(ByVal serial As String, _
            ByVal number As Integer) As ShortLabourContract

            For Each s As ShortLabourContract In Me
                If serial.Trim.ToLower = s.Serial.Trim.ToLower _
                    AndAlso number = s.Number Then Return s
            Next

            Return Nothing

        End Function

        ''' <summary>
        ''' Returns whether a labour contract with the required serial and number exists within the list.
        ''' </summary>
        ''' <param name="serial">Serial to find.</param>
        ''' <param name="number">Number to find.</param>
        ''' <remarks></remarks>
        Public Function Exists(ByVal serial As String, ByVal number As Integer) As Boolean
            For Each s As ShortLabourContract In Me
                If serial.Trim.ToLower = s.Serial.Trim.ToLower _
                    AndAlso number = s.Number Then Return True
            Next
            Return False
        End Function

#End Region

#Region " Authorization Rules "

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("HelperLists.ShortLabourContractList1")
        End Function

#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets a current labour contract value object list from database.
        ''' </summary>
        ''' <remarks>Result is cached.
        ''' Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Public Shared Function GetList() As ShortLabourContractList

            Dim result As ShortLabourContractList = _
                CacheManager.GetItemFromCache(Of ShortLabourContractList)( _
                GetType(ShortLabourContractList), Nothing)

            If result Is Nothing Then
                result = DataPortal.Fetch(Of ShortLabourContractList)(New Criteria())
                CacheManager.AddCacheItem(GetType(ShortLabourContractList), result, Nothing)
            End If

            Return result

        End Function

        ''' <summary>
        ''' Gets a filtered view of the current labour contract value object list.
        ''' </summary>
        ''' <param name="showEmpty">whether to include a placeholder object</param>
        ''' <param name="personID">an <see cref="General.Person.ID">ID of a worker</see>
        ''' to filter the list by (use 0 for all workers)</param>
        ''' <param name="beforeDate">a contract date to filter the list by
        ''' (excluding the contracts after the date)</param>
        ''' <remarks>Result is cached.
        ''' Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Public Shared Function GetCachedFilteredList(ByVal showEmpty As Boolean, _
            ByVal personID As Integer, ByVal beforeDate As Date) As Csla.FilteredBindingList(Of ShortLabourContract)

            Dim filterToApply(2) As Object
            filterToApply(0) = ConvertDbBoolean(showEmpty)
            filterToApply(1) = personID
            filterToApply(2) = beforeDate

            Dim result As Csla.FilteredBindingList(Of ShortLabourContract) = _
                CacheManager.GetItemFromCache(Of Csla.FilteredBindingList(Of ShortLabourContract)) _
                (GetType(ShortLabourContractList), filterToApply)

            If result Is Nothing Then

                Dim baseList As ShortLabourContractList = ShortLabourContractList.GetList
                result = New Csla.FilteredBindingList(Of ShortLabourContract) _
                    (baseList, AddressOf ShortLabourContractFilter)
                result.ApplyFilter("", filterToApply)
                CacheManager.AddCacheItem(GetType(ShortLabourContractList), _
                    result, filterToApply)

            End If

            Return result

        End Function

        ''' <summary>
        ''' Invalidates the current labour contract value object list cache 
        ''' so that the next <see cref="GetList">GetList</see> call would hit the database.
        ''' </summary>
        ''' <remarks>Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Public Shared Sub InvalidateCache()
            CacheManager.InvalidateCache(GetType(ShortLabourContractList))
        End Sub

        ''' <summary>
        ''' Returnes true if the cache does not contain a current 
        ''' labour contract value object list.
        ''' </summary>
        ''' <remarks>Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Public Shared Function CacheIsInvalidated() As Boolean
            Return CacheManager.CacheIsInvalidated(GetType(ShortLabourContractList))
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
        ''' Gets a current labour contract value object list from database 
        ''' bypassing dataportal.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Should only be called server side.
        ''' Required by <see cref="AccDataAccessLayer.CacheManager">AccDataAccessLayer.CacheManager</see>.</remarks>
        Private Shared Function GetListOnServer() As ShortLabourContractList
            Dim result As New ShortLabourContractList
            result.DataPortal_Fetch(New Criteria)
            Return result
        End Function

        ''' <summary>
        ''' Gets a current labour contract value object list from database bypassing dataportal.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Should only be invoked server side.</remarks>
        Friend Shared Function GetListChild() As ShortLabourContractList
            Dim result As New ShortLabourContractList
            result.DoFetch()
            Return result
        End Function


        Private Shared Function ShortLabourContractFilter(ByVal item As Object, ByVal filterValue As Object) As Boolean

            If filterValue Is Nothing OrElse DirectCast(filterValue, Object()).Length < 3 Then Return True

            Dim showEmpty As Boolean = ConvertDbBoolean( _
                DirectCast(DirectCast(filterValue, Object())(0), Integer))
            Dim personID As Integer = DirectCast(DirectCast(filterValue, Object())(1), Integer)
            Dim beforeDate As Date = DirectCast(DirectCast(filterValue, Object())(2), Date)

            Dim ci As ShortLabourContract = DirectCast(item, ShortLabourContract)

            If Not showEmpty AndAlso ci.IsEmpty Then Return False
            If personID > 0 AndAlso Not ci.IsEmpty AndAlso _
                ci.PersonID <> personID Then Return False
            If Not ci.IsEmpty AndAlso ci.Date.Date > beforeDate.Date Then Return False

            Return True

        End Function


        Private Sub New()
            ' require use of factory methods
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private _PersonID As Integer = 0
            Private _IsForPerson As Boolean = False
            Private _Date As Date = Today.AddYears(50)
            Public ReadOnly Property PersonID() As Integer
                Get
                    Return _PersonID
                End Get
            End Property
            Public ReadOnly Property IsForPerson() As Boolean
                Get
                    Return _IsForPerson
                End Get
            End Property
            Public ReadOnly Property [Date]() As Date
                Get
                    Return _Date
                End Get
            End Property
            Public Sub New(ByVal nPersonID As Integer)
                _PersonID = nPersonID
                _IsForPerson = True
            End Sub
            Public Sub New(ByVal nPersonID As Integer, ByVal nDate As Date)
                _PersonID = nPersonID
                _IsForPerson = True
                _Date = nDate.Date
            End Sub
            Public Sub New()
            End Sub
        End Class

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecuritySelectDenied)

            DoFetch()

        End Sub

        Private Sub DoFetch()

            Dim myComm As New SQLCommand("FetchShortLabourContractList")

            Using myData As DataTable = myComm.Fetch
                RaiseListChangedEvents = False
                IsReadOnly = False
                For Each dr As DataRow In myData.Rows
                    Add(ShortLabourContract.GetShortLabourContract(dr))
                Next
                IsReadOnly = True
                RaiseListChangedEvents = True
            End Using

        End Sub

#End Region

    End Class

End Namespace