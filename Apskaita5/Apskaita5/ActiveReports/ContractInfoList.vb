Namespace ActiveReports

    ''' <summary>
    ''' Represents a labour contract report. Describes parameters of <see cref="Workers.Contract">labour contracts</see>.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public NotInheritable Class ContractInfoList
        Inherits ReadOnlyListBase(Of ContractInfoList, ContractInfo)

#Region " Business Methods "

        Private _IsOriginalContractData As Boolean = True
        Private _OnlyInOperationAtDate As Boolean = True
        Private _AtDate As Date = Today
        Private _AddAllWorkers As Boolean = False


        ''' <summary>
        ''' Whether to display the original labour contract params or the actual labour contract params (including amendments).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property IsOriginalContractData() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsOriginalContractData
            End Get
        End Property

        ''' <summary>
        ''' Whether to display only the labour contracts that are valid at <see cref="AtDate">AtDate</see>.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property OnlyInOperationAtDate() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OnlyInOperationAtDate
            End Get
        End Property

        ''' <summary>
        ''' A Date for which the actual labour contract params (including amendments) are displayed. 
        ''' Only has an effect if <see cref="IsOriginalContractData">IsOriginalContractData</see> is set to False
        ''' or <see cref="OnlyInOperationAtDate">OnlyInOperationAtDate</see> is set to True.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property AtDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AtDate
            End Get
        End Property

        ''' <summary>
        ''' Whether to include workers without a labour contract.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property AddAllWorkers() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AddAllWorkers
            End Get
        End Property

#End Region

#Region " Authorization Rules "

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("Workers.ContractInfoList1")
        End Function

#End Region

#Region " Factory Methods "

        ' used to implement automatic sort in datagridview
        <NonSerialized()> _
        Private _SortedList As Csla.SortedBindingList(Of ContractInfo) = Nothing

        ''' <summary>
        ''' Gets a new labour contract report instance.
        ''' </summary>
        ''' <param name="nIsOriginalContractData">Whether to display the original labour contract params or the actual labour contract params (including amendments)</param>
        ''' <param name="nOnlyInOperationAtDate">Whether to display only the labour contracts that are valid at <see cref="AtDate">AtDate</see>.</param>
        ''' <param name="nAtDate">A Date for which the actual labour contract params (including amendments) are displayed.</param>
        ''' <param name="nAddAllWorkers">Whether to include workers without a labour contract.</param>
        ''' <remarks></remarks>
        Public Shared Function GetContractInfoList(ByVal nIsOriginalContractData As Boolean, _
                ByVal nOnlyInOperationAtDate As Boolean, ByVal nAtDate As Date, _
                ByVal nAddAllWorkers As Boolean) As ContractInfoList
            Return DataPortal.Fetch(Of ContractInfoList)(New Criteria( _
                nIsOriginalContractData, nOnlyInOperationAtDate, nAtDate, nAddAllWorkers))
        End Function

        ''' <summary>
        ''' Gets a sortable view of the report.
        ''' </summary>
        ''' <remarks>Used for auto sort in a datagridview.</remarks>
        Public Function GetSortedList() As Csla.SortedBindingList(Of ContractInfo)
            If _SortedList Is Nothing Then _SortedList = New Csla.SortedBindingList(Of ContractInfo)(Me)
            Return _SortedList
        End Function


        Private Sub New()
            ' require use of factory methods
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private _IsOriginalContractData As Boolean
            Private _OnlyInOperationAtDate As Boolean
            Private _AtDate As Date
            Private _AddAllWorkers As Boolean
            Public ReadOnly Property IsOriginalContractData() As Boolean
                Get
                    Return _IsOriginalContractData
                End Get
            End Property
            Public ReadOnly Property OnlyInOperationAtDate() As Boolean
                Get
                    Return _OnlyInOperationAtDate
                End Get
            End Property
            Public ReadOnly Property AtDate() As Date
                Get
                    Return _AtDate
                End Get
            End Property
            Public ReadOnly Property AddAllWorkers() As Boolean
                Get
                    Return _AddAllWorkers
                End Get
            End Property
            Public Sub New(ByVal nIsOriginalContractData As Boolean, _
                ByVal nOnlyInOperationAtDate As Boolean, ByVal nAtDate As Date, _
                ByVal nAddAllWorkers As Boolean)
                _IsOriginalContractData = nIsOriginalContractData
                _OnlyInOperationAtDate = nOnlyInOperationAtDate
                _AtDate = nAtDate
                _AddAllWorkers = nAddAllWorkers
            End Sub
        End Class

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
               My.Resources.Common_SecuritySelectDenied)

            Dim myComm As SQLCommand
            If criteria.IsOriginalContractData Then
                myComm = New SQLCommand("FetchContractInfoList")
            Else
                myComm = New SQLCommand("FetchContractInfoListConsolidated")
            End If
            myComm.AddParam("?DT", criteria.AtDate.Date)
            myComm.AddParam("?AL", ConvertDbBoolean(Not criteria.OnlyInOperationAtDate))

            Using myData As DataTable = myComm.Fetch

                myComm = New SQLCommand("FetchLabourContractUpdateInfoList")
                If criteria.OnlyInOperationAtDate Then
                    myComm.AddParam("?DT", criteria.AtDate)
                Else
                    myComm.AddParam("?DT", Today.AddYears(100))
                End If

                Using myDataUpdates As DataTable = myComm.Fetch

                    RaiseListChangedEvents = False
                    IsReadOnly = False

                    For Each dr As DataRow In myData.Rows
                        Add(ContractInfo.GetContractInfo(dr, myDataUpdates))
                    Next

                    _IsOriginalContractData = criteria.IsOriginalContractData
                    _OnlyInOperationAtDate = criteria.OnlyInOperationAtDate
                    _AtDate = criteria.AtDate
                    _AddAllWorkers = criteria.AddAllWorkers

                    IsReadOnly = True
                    RaiseListChangedEvents = True

                End Using

            End Using

            If criteria.AddAllWorkers Then

                myComm = New SQLCommand("FetchContractInfoListWorkersWithoutContract")

                Using myData As DataTable = myComm.Fetch

                    RaiseListChangedEvents = False
                    IsReadOnly = False

                    For Each dr As DataRow In myData.Rows
                        Add(ContractInfo.GetContractInfo(dr, Nothing))
                    Next

                    IsReadOnly = True
                    RaiseListChangedEvents = True

                End Using

            End If

        End Sub

#End Region

    End Class

End Namespace