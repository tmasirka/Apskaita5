Imports ApskaitaObjects.General
Namespace ActiveReports

    ''' <summary>
    ''' Represents an account turnover report (part of <see cref="ActiveReports.FinancialStatementsInfo">FinancialStatementsInfo</see> report).
    ''' </summary>
    ''' <remarks>Should only be used as a child of <see cref="ActiveReports.FinancialStatementsInfo">FinancialStatementsInfo</see>.</remarks>
    <Serializable()> _
    Public NotInheritable Class AccountTurnoverInfoList
        Inherits ReadOnlyListBase(Of AccountTurnoverInfoList, AccountTurnoverInfo)

#Region " Business Methods "

        ''' <summary>
        ''' Gets an associated <see cref="AccountTurnoverInfo.FinancialStatementItem">FinancialStatementItem</see> 
        ''' for the account where the ID equals <paramref name="closingSummaryAccount">closingSummaryAccount</paramref>.
        ''' </summary>
        ''' <param name="closingSummaryAccount">An account ID to find.</param>
        ''' <param name="throwOnNotFound">Whether to throw an exception if no account is found.</param>
        ''' <remarks></remarks>
        Friend Function GetClosingSummaryBalanceItem(ByVal closingSummaryAccount As Long, _
            ByVal throwOnNotFound As Boolean) As String

            If Not closingSummaryAccount > 0 Then Throw New ArgumentNullException("closingSummaryAccount")

            For Each t As AccountTurnoverInfo In Me
                If t.ID = closingSummaryAccount Then

                    If throwOnNotFound AndAlso StringIsNullOrEmpty(t.FinancialStatementItem) Then
                        Throw New Exception(String.Format( _
                            My.Resources.ActiveReports_AccountTurnoverInfoList_FinancialStatementItemNullForClosingSummary, _
                            closingSummaryAccount.ToString))
                    End If

                    Return t.FinancialStatementItem

                End If
            Next

            If throwOnNotFound Then
                Throw New Exception(My.Resources.ActiveReports_AccountTurnoverInfoList_FinancialStatementItemNotFoundForClosingSummary)
            End If

            Return ""

        End Function

        ''' <summary>
        ''' Gets an ID of the associated <see cref="AccountTurnoverInfo.FinancialStatementItem">FinancialStatementItem</see> 
        ''' for the account where the ID equals <paramref name="closingSummaryAccount">closingSummaryAccount</paramref>.
        ''' </summary>
        ''' <param name="closingSummaryAccount">an account ID to find</param>
        ''' <param name="throwOnNotFound">whether to throw an exception if no account is found</param>
        ''' <remarks></remarks>
        Friend Function GetClosingSummaryBalanceItemId(ByVal closingSummaryAccount As Long, _
            ByVal throwOnNotFound As Boolean) As Integer

            If Not closingSummaryAccount > 0 Then Throw New ArgumentNullException("closingSummaryAccount")

            For Each t As AccountTurnoverInfo In Me
                If t.ID = closingSummaryAccount Then

                    If throwOnNotFound AndAlso t.FinancialStatementItemId < 1 Then
                        Throw New Exception(String.Format( _
                            My.Resources.ActiveReports_AccountTurnoverInfoList_FinancialStatementItemNullForClosingSummary, _
                            closingSummaryAccount.ToString))
                    End If

                    Return t.FinancialStatementItemId

                End If
            Next

            If throwOnNotFound Then
                Throw New Exception(My.Resources.ActiveReports_AccountTurnoverInfoList_FinancialStatementItemNotFoundForClosingSummary)
            End If

            Return 0

        End Function

        ''' <summary>
        ''' Gets a sum of transactions that would be produced 
        ''' by <see cref="General.ClosingEntriesCommand">ClosingEntriesCommand</see> 
        ''' at <see cref="FinancialStatementsInfo.SecondPeriodDateStart">FinancialStatementsInfo.SecondPeriodDateStart</see>.
        ''' </summary>
        ''' <remarks>Simulates closing at the begining of the first period.
        ''' Positive number represents debit type balance, negative number - credit type balance.</remarks>
        Friend Function GetFormerPeriodClosingSum() As Double

            Dim result As Double = 0

            For Each t As AccountTurnoverInfo In Me
                If t.FinancialStatementItemType = FinancialStatementItemType.StatementOfComprehensiveIncome Then _
                    result = CRound(result + t.DebitBalanceFormerPeriodStart _
                        + t.DebitTurnoverFormerPeriod - t.CreditBalanceFormerPeriodStart _
                        - t.CreditTurnoverFormerPeriod)
            Next

            Return result

        End Function

        ''' <summary>
        ''' Gets a sum of transactions that would be produced 
        ''' by <see cref="General.ClosingEntriesCommand">ClosingEntriesCommand</see> 
        ''' at <see cref="FinancialStatementsInfo.SecondPeriodDateEnd">FinancialStatementsInfo.SecondPeriodDateEnd</see>.
        ''' </summary>
        ''' <remarks>Simulates closing at the end of the second period.
        ''' Positive number represents debit type balance, negative number - credit type balance.</remarks>
        Friend Function GetCurrentPeriodClosingSum() As Double

            Dim result As Double = 0

            For Each t As AccountTurnoverInfo In Me
                If t.FinancialStatementItemType = FinancialStatementItemType.StatementOfComprehensiveIncome Then _
                    result = CRound(result + t.DebitBalanceFormerPeriodStart _
                        + t.DebitTurnoverFormerPeriod - t.CreditBalanceFormerPeriodStart _
                        - t.CreditTurnoverFormerPeriod + t.DebitTurnoverCurrentPeriod _
                        - t.CreditTurnoverCurrentPeriod)
            Next

            Return result

        End Function

#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets an account turnover report by a database query result.
        ''' </summary>
        ''' <param name="myData">A database query result</param>
        ''' <param name="includeWithoutTurnover">Whether to include accounts without turnover 
        ''' as defined by method <see cref="AccountTurnoverInfo.HasTurnover">AccountTurnoverInfo.HasTurnover</see>.</param>
        ''' <param name="closingSummaryAccount">An <see cref="General.Account.ID">ID</see> of the closing summary account.</param>
        ''' <remarks></remarks>
        Friend Shared Function GetAccountTurnoverInfoList(ByVal myData As DataTable, _
            ByVal includeWithoutTurnover As Boolean, ByVal closingSummaryAccount As Long) As AccountTurnoverInfoList
            Return New AccountTurnoverInfoList(myData, includeWithoutTurnover, closingSummaryAccount)
        End Function


        Private Sub New()
            ' require use of factory methods
        End Sub

        Private Sub New(ByVal myData As DataTable, ByVal includeWithoutTurnover As Boolean, _
            ByVal closingSummaryAccount As Long)
            ' require use of factory methods
            Fetch(myData, includeWithoutTurnover, closingSummaryAccount)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Fetch(ByVal myData As DataTable, ByVal includeWithoutTurnover As Boolean, _
            ByVal closingSummaryAccount As Long)

            RaiseListChangedEvents = False
            IsReadOnly = False

            For Each dr As DataRow In myData.Rows
                Dim newItem As AccountTurnoverInfo = AccountTurnoverInfo.GetAccountTurnoverInfo(dr)
                If includeWithoutTurnover OrElse newItem.ID = closingSummaryAccount _
                    OrElse newItem.HasTurnover Then Add(newItem)
            Next

            IsReadOnly = True
            RaiseListChangedEvents = True

        End Sub

#End Region

    End Class

End Namespace
