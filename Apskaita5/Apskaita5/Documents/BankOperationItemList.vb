Imports ApskaitaObjects.HelperLists
Namespace Documents

    <Serializable()> _
    Public Class BankOperationItemList
        Inherits BusinessListBase(Of BankOperationItemList, BankOperationItem)
        Implements IIsDirtyEnough

#Region " Business Methods "

        Private Const LITAS_ESIS_ENCODING As String = "windows-1257" ' "ISO-8859-13"
        Private Const LITAS_ESIS_OPERATION_LINE_CODE As String = "010"
        Private Const LITAS_ESIS_HEADER_LINE_CODE As String = "000"
        Private Const LITAS_ESIS_SUMMARY_LINE_CODE As String = "020"
        Private Const LITAS_ESIS_BALANCE_AT_START As String = "likutispr"
        Private Const LITAS_ESIS_BALANCE_AT_END As String = "likutispb"

        Private Const OPERATION_STRING_DELIMITER As String = "##%%OpSeparator%%##"

        Private _Account As CashAccountInfo = Nothing
        Private _ImportsSource As BankOperationImportSourceType = BankOperationImportSourceType.PasteString
        Private _BalanceStart As Double = 0
        Private _BalanceEnd As Double = 0
        Private _Income As Double = 0
        Private _Spendings As Double = 0
        Private _DateStart As Date = Today
        Private _DateEnd As Date = Today


        ''' <summary>
        ''' The bank account which the data is imported to.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Account() As CashAccountInfo
            Get
                Return _Account
            End Get
        End Property

        ''' <summary>
        ''' The source of the data.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property ImportsSource() As String
            Get
                Return ConvertEnumHumanReadable(_ImportsSource)
            End Get
        End Property

        ''' <summary>
        ''' The bank account balance at the begining of the data period (as specified in the imported data).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property BalanceStart() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_BalanceStart)
            End Get
        End Property

        ''' <summary>
        ''' The bank account balance at the end of the data period (as specified in the imported data).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property BalanceEnd() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_BalanceEnd)
            End Get
        End Property

        ''' <summary>
        ''' Total sum of money received within the imported data.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Income() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Income)
            End Get
        End Property

        ''' <summary>
        ''' Total sum of money transfered (spent) within the imported data.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Spendings() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Spendings)
            End Get
        End Property

        ''' <summary>
        ''' The begining of the data period (as specified in the imported data).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property DateStart() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DateStart
            End Get
        End Property

        ''' <summary>
        ''' The end of the data period (as specified in the imported data).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property DateEnd() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DateEnd
            End Get
        End Property


        ''' <summary>
        ''' Returnes TRUE if the object contains some user provided data.
        ''' </summary>
        Public ReadOnly Property IsDirtyEnough() As Boolean Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Me.Count < 1 Then Return False
                For Each i As BankOperationItem In Me
                    If Not i.ExistsInDatabase Then Return True
                Next
                Return False
            End Get
        End Property


        ''' <summary>
        ''' Gets a human readable (localized) description of the import source data.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetDescription() As String
            If _ImportsSource = BankOperationImportSourceType.LITAS_ESIS Then
                Return String.Format(My.Resources.Documents_BankOperationItemList_DescriptionStringLitasEsis, _
                    vbCrLf, _DateStart.ToString("yyyy-MM-dd"), _DateEnd.ToString("yyyy-MM-dd"), _
                    _Account.CurrencyCode.Trim.ToUpper, vbCrLf, DblParser(_BalanceStart), _
                    vbCrLf, DblParser(_Income), vbCrLf, DblParser(_Spendings), vbCrLf, DblParser(_BalanceEnd))
            Else
                Return My.Resources.Documents_BankOperationItemList_DescriptionStringPasteString
            End If
        End Function


        Public Function GetAllBrokenRules() As String
            Dim result As String = GetAllBrokenRulesForList(Me)
            Return result
        End Function

        Public Function GetAllWarnings() As String
            Dim result As String = GetAllWarningsForList(Me)
            Return result
        End Function

        Public Function HasWarnings() As Boolean
            For Each i As BankOperationItem In Me
                If i.BrokenRulesCollection.WarningCount > 0 Then Return True
            Next
            Return False
        End Function


        ''' <summary>
        ''' Updates data within the list with a saved bank operation data.
        ''' </summary>
        ''' <param name="savedBankOperation">A <see cref="BankOperation">BankOperation</see>
        ''' that was created using the imported data and was saved to a database.</param>
        ''' <remarks></remarks>
        Public Sub UpdateWithBankOperation(ByVal savedBankOperation As BankOperation)
            If savedBankOperation Is Nothing OrElse savedBankOperation.IsNew Then Exit Sub
            For Each i As BankOperationItem In Me
                i.IdentifyWithBankOperation(savedBankOperation, _Account)
            Next
        End Sub


        Public Overrides Function Save() As BankOperationItemList
            Return MyBase.Save()
        End Function

#End Region

#Region " Authorization Rules "

        Public Shared Function CanGetObject() As Boolean
            Return False
        End Function

        Public Shared Function CanAddObject() As Boolean
            Return BankOperation.CanAddObject
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return False
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return False
        End Function

#End Region

#Region " Factory Methods "

        ' used to implement automatic sort in datagridview
        <NonSerialized()> _
        Private _SortedList As Csla.SortedBindingList(Of BankOperationItem) = Nothing

        ''' <summary>
        ''' Creates a new list of imported bank operation data using a LITAS-ESIS file.
        ''' </summary>
        ''' <param name="fileName">a path to the LITAS-ESIS file</param>
        ''' <param name="nAccount">the account that the operations are ment for</param>
        ''' <param name="bankDocumentPrefix">a user defined prefix for the 
        ''' <see cref="BankOperation.DocumentNumber">DocumentNumber</see> property</param>
        ''' <param name="ignoreWrongIBAN">whether to ignore the fact, that the IBAN numbers
        ''' in the file and the account do not match</param>
        ''' <remarks></remarks>
        Public Shared Function GetListFromLitasEsisFile(ByVal fileName As String, _
            ByVal nAccount As CashAccountInfo, ByVal bankDocumentPrefix As String, _
            ByVal ignoreWrongIBAN As Boolean) As BankOperationItemList

            If StringIsNullOrEmpty(fileName) Then
                Throw New Exception(My.Resources.Documents_BankOperationItemList_FileNameNull)
            ElseIf Not IO.File.Exists(fileName) Then
                Throw New Exception(String.Format(My.Resources.Documents_BankOperationItemList_FileNotFound, fileName))
            ElseIf nAccount Is Nothing OrElse nAccount.IsEmpty Then
                Throw New Exception(My.Resources.Documents_BankOperationItemList_AccountNull)
            ElseIf nAccount.Type <> CashAccountType.PseudoBankAccount AndAlso _
                nAccount.Type <> CashAccountType.BankAccount Then
                Throw New Exception(My.Resources.Documents_BankOperationItemList_AccountInvalid)
            End If

            Dim readText() As String = IO.File.ReadAllLines(fileName, _
                System.Text.Encoding.GetEncoding(LITAS_ESIS_ENCODING))

            Dim nBalanceStart As Double = 0
            Dim nBalanceEnd As Double = 0
            Dim nDateStart As Date = Today
            Dim nDateEnd As Date = Today

            Dim operations As New List(Of String)

            For Each s As String In readText

                If GetElement(s, 0).Trim = LITAS_ESIS_OPERATION_LINE_CODE Then

                    operations.Add(s)

                ElseIf GetElement(s, 0).Trim = LITAS_ESIS_HEADER_LINE_CODE Then

                    If Not CurrenciesEquals(GetElement(s, 17), nAccount.CurrencyCode, _
                        GetCurrentCompany().BaseCurrency) Then

                        Throw New Exception(String.Format(My.Resources.Documents_BankOperationItemList_InvalidCurrency, _
                            GetElement(s, 17).Trim.ToUpper, nAccount.CurrencyCode.Trim.ToUpper))

                    End If

                    If Not ignoreWrongIBAN AndAlso Not StringIsNullOrEmpty(GetElement(s, 16)) AndAlso _
                       GetElement(s, 16).Trim.ToUpper <> nAccount.BankAccountNumber.Trim.ToUpper Then

                        Throw New Exception(String.Format(My.Resources.Documents_BankOperationItemList_InvalidIBAN, _
                            GetElement(s, 16).Trim.ToUpper, nAccount.BankAccountNumber.Trim.ToUpper))

                    End If

                ElseIf GetElement(s, 0).Trim = LITAS_ESIS_SUMMARY_LINE_CODE Then

                    If GetElement(s, 1).Trim.ToLower = LITAS_ESIS_BALANCE_AT_START Then

                        nBalanceStart = CRound(CLongSafe(GetElement(s, 4), 0) / 100)

                        Dim tm As String = GetElement(s, 2).Trim
                        If tm.Length = 8 Then
                            nDateStart = New Date(CIntSafe(tm.Substring(0, 4)), _
                                CIntSafe(tm.Substring(4, 2)), CIntSafe(tm.Substring(6, 2)))
                        End If

                    ElseIf GetElement(s, 1).Trim.ToLower = LITAS_ESIS_BALANCE_AT_END Then

                        nBalanceEnd = CRound(CLongSafe(GetElement(s, 4), 0) / 100)

                        Dim tm As String = GetElement(s, 2).Trim
                        If tm.Length = 8 Then
                            nDateEnd = New Date(CIntSafe(tm.Substring(0, 4)), _
                                CIntSafe(tm.Substring(4, 2)), CIntSafe(tm.Substring(6, 2)))
                        End If

                    End If

                End If

            Next

            If operations.Count < 1 Then
                Throw New Exception(My.Resources.Documents_BankOperationitemlist_Invalidfilecontent)
            End If

            Return DataPortal.Fetch(Of BankOperationItemList)(New Criteria(nAccount, _
                String.Join(OPERATION_STRING_DELIMITER, operations.ToArray()), _
                BankOperationImportSourceType.LITAS_ESIS, bankDocumentPrefix, nBalanceStart, _
                nBalanceEnd, nDateStart, nDateEnd))

        End Function

        ''' <summary>
        ''' Creates a new list of imported bank operation data using a paste string 
        ''' (new line delimited operations, tab delimited fields).
        ''' </summary>
        ''' <param name="pasteString">a paste string to parse</param>
        ''' <param name="nAccount">the account that the operations are ment for</param>
        ''' <param name="bankDocumentPrefix">a user defined prefix for the 
        ''' <see cref="BankOperation.DocumentNumber">DocumentNumber</see> property</param>
        ''' <remarks></remarks>
        Public Shared Function GetListFromPasteString(ByVal pasteString As String, _
            ByVal nAccount As CashAccountInfo, ByVal bankDocumentPrefix As String) As BankOperationItemList

            If nAccount Is Nothing OrElse Not nAccount.ID > 0 Then
                Throw New Exception(My.Resources.Documents_BankOperationItemList_AccountNull)
            ElseIf nAccount.Type <> CashAccountType.PseudoBankAccount AndAlso _
                nAccount.Type <> CashAccountType.BankAccount Then
                Throw New Exception(My.Resources.Documents_BankOperationItemList_AccountInvalid)
            ElseIf StringIsNullOrEmpty(pasteString) Then
                Throw New Exception(String.Format(My.Resources.Documents_BankOperationItemList_PasteStringNull, _
                    vbCrLf, BankOperationItem.GetPasteStringColumnsDescription()))
            End If

            Dim lineDelim As Char() = {ControlChars.Cr, ControlChars.Lf}
            Dim colDelim As Char() = {ControlChars.Tab}

            Dim lines As String() = pasteString.Split(lineDelim, StringSplitOptions.None)

            If lines(0).Split(colDelim, StringSplitOptions.None).Length <> _
               BankOperationItem.GetPasteStringColumnCount() Then
                Throw New Exception(String.Format(My.Resources.Documents_BankOperationItemList_InvalidColumnCountInPasteString, _
                    BankOperationItem.GetPasteStringColumnCount().ToString(), _
                    lines(0).Split(colDelim, StringSplitOptions.None).Length.ToString, vbCrLf, _
                    BankOperationItem.GetPasteStringColumnsDescription()))
            End If

            Dim operations As New List(Of String)

            For Each s As String In lines

                If s.Split(colDelim, StringSplitOptions.None).Length = 13 Then
                    operations.Add(s)
                End If

            Next

            Return DataPortal.Fetch(Of BankOperationItemList)(New Criteria(nAccount, _
                String.Join(OPERATION_STRING_DELIMITER, operations.ToArray()), _
                BankOperationImportSourceType.PasteString, bankDocumentPrefix, 0, 0, Today, Today))

        End Function


        ''' <summary>
        ''' Gets a sortable view of the list.
        ''' </summary>
        ''' <remarks>Used to implement auto sort in a grid.</remarks>
        Public Function GetSortedList() As Csla.SortedBindingList(Of BankOperationItem)
            If _SortedList Is Nothing Then _SortedList = New Csla.SortedBindingList(Of BankOperationItem)(Me)
            Return _SortedList
        End Function


        Private Sub New()
            ' require use of factory methods
            Me.AllowEdit = True
            Me.AllowNew = False
            Me.AllowRemove = True
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private _Account As CashAccountInfo = Nothing
            Private _ImportsSource As BankOperationImportSourceType = BankOperationImportSourceType.PasteString
            Private _OperationString As String = ""
            Private _BankDocumentPrefix As String = ""
            Private _BalanceStart As Double = 0
            Private _BalanceEnd As Double = 0
            Private _DateStart As Date = Today
            Private _DateEnd As Date = Today
            Public ReadOnly Property Account() As CashAccountInfo
                Get
                    Return _Account
                End Get
            End Property
            Public ReadOnly Property ImportsSource() As BankOperationImportSourceType
                Get
                    Return _ImportsSource
                End Get
            End Property
            Public ReadOnly Property OperationString() As String
                Get
                    Return _OperationString
                End Get
            End Property
            Public ReadOnly Property BankDocumentPrefix() As String
                Get
                    Return _BankDocumentPrefix
                End Get
            End Property
            Public ReadOnly Property BalanceStart() As Double
                Get
                    Return CRound(_BalanceStart)
                End Get
            End Property
            Public ReadOnly Property BalanceEnd() As Double
                Get
                    Return CRound(_BalanceEnd)
                End Get
            End Property
            Public ReadOnly Property DateStart() As Date
                Get
                    Return _DateStart
                End Get
            End Property
            Public ReadOnly Property DateEnd() As Date
                Get
                    Return _DateEnd
                End Get
            End Property
            Public Sub New(ByVal nAccount As CashAccountInfo, ByVal nOperationString As String, _
                ByVal nImportsSource As BankOperationImportSourceType, _
                ByVal nBankDocumentPrefix As String, ByVal nBalanceStart As Double, _
                ByVal nBalanceEnd As Double, ByVal nDateStart As Date, ByVal nDateEnd As Date)
                _Account = nAccount
                _OperationString = nOperationString
                _ImportsSource = nImportsSource
                _BankDocumentPrefix = nBankDocumentPrefix
                _BalanceStart = nBalanceStart
                _BalanceEnd = nBalanceEnd
                _DateStart = nDateStart
                _DateEnd = nDateEnd
            End Sub
        End Class


        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)

            If criteria.Account Is Nothing OrElse criteria.Account.IsEmpty Then
                Throw New Exception(My.Resources.Documents_BankOperationItemList_AccountNull)
            ElseIf criteria.Account.Type <> CashAccountType.PseudoBankAccount AndAlso _
                criteria.Account.Type <> CashAccountType.BankAccount Then
                Throw New Exception(My.Resources.Documents_BankOperationItemList_AccountInvalid)
            ElseIf StringIsNullOrEmpty(criteria.OperationString) Then
                If criteria.ImportsSource = BankOperationImportSourceType.LITAS_ESIS Then
                    Throw New Exception(My.Resources.Documents_BankOperationItemList_InvalidFileContent)
                Else
                    Throw New Exception(String.Format(My.Resources.Documents_BankOperationItemList_PasteStringNull, _
                        vbCrLf, BankOperationItem.GetPasteStringColumnsDescription()))
                End If
            End If

            Dim operationStrings() As String = criteria.OperationString.Split( _
                New String() {OPERATION_STRING_DELIMITER}, StringSplitOptions.RemoveEmptyEntries)

            RaiseListChangedEvents = False

            _Account = criteria.Account
            _ImportsSource = criteria.ImportsSource
            _BalanceStart = criteria.BalanceStart
            _BalanceEnd = criteria.BalanceEnd
            _DateStart = criteria.DateStart
            _DateEnd = criteria.DateEnd

            For Each s As String In operationStrings
                Add(BankOperationItem.GetBankOperationItem(s, criteria.Account, _
                    criteria.ImportsSource, criteria.BankDocumentPrefix))
            Next

            TryFixUniqueCodes()

            Dim bankPersonInfo As PersonInfo = Nothing

            If criteria.Account.ManagingPersonID > 0 Then

                Try
                    bankPersonInfo = PersonInfo.GetPersonInfoChild(criteria.Account.ManagingPersonID, True)
                Catch ex As Exception
                    ' error is not essential, not worth to interrupt the process
                End Try

            End If

            _Income = 0
            _Spendings = 0
            For Each i As BankOperationItem In Me
                i.RecognizeItem(criteria.Account, bankPersonInfo)
                If i.Inflow Then
                    _Income += i.SumInAccount
                Else
                    _Spendings += i.SumInAccount
                End If
            Next

            _Income = CRound(_Income)
            _Spendings = CRound(_Spendings)

            RaiseListChangedEvents = True

        End Sub

        Protected Overrides Sub DataPortal_Update()

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)

            If Not Me.Count > 0 Then Throw New Exception(My.Resources.Documents_BankOperationItemList_ListEmpty)

            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            CheckUniqueCodeConstraintsWithinList()

            For Each item As BankOperationItem In Me
                item.PrepareForInsert(_Account)
            Next

            RaiseListChangedEvents = False

            DeletedList.Clear()

            DatabaseAccess.TransactionBegin()

            For Each item As BankOperationItem In Me
                item.Insert()
            Next

            DatabaseAccess.TransactionCommit()

            RaiseListChangedEvents = True

        End Sub


        Private Sub TryFixUniqueCodes()

            If Not _Account.EnforceUniqueOperationID Then Exit Sub

            Dim i, j As Integer

            Dim sameUniqueCounter As Integer

            For i = 1 To Me.Count

                If StringIsNullOrEmpty(Item(i - 1).UniqueCode) Then Throw New Exception( _
                    My.Resources.Documents_BankOperationItem_UniqueCodeNotSpecified)

                ' some banks use the same unique code for the operation and the related bank fee
                ' these duplicate entries could be fixed, thus exception is thrown 
                ' only if more than two entries have the same unique code
                sameUniqueCounter = 0

                For j = i + 1 To Me.Count
                    If Item(i - 1).UniqueCode.Trim.ToUpper = Item(j - 1).UniqueCode.Trim.ToUpper Then
                        sameUniqueCounter += 1
                    End If
                Next

                If sameUniqueCounter > 1 Then
                    Throw New Exception(My.Resources.Documents_BankOperationItem_UniqueCodesInvalid)
                End If

            Next

            For i = 1 To Me.Count
                For j = i + 1 To Me.Count
                    If Item(i - 1).UniqueCode.Trim.ToUpper = Item(j - 1).UniqueCode.Trim.ToUpper Then
                        Item(i - 1).FixUniqueCodes(Item(j - 1))
                    End If
                Next
            Next

        End Sub

        Private Sub CheckUniqueCodeConstraintsWithinList()

            If Not _Account.EnforceUniqueOperationID Then Exit Sub

            Dim i, j As Integer
            Dim operationsWithoutUniqueCode As New List(Of String)
            Dim operationsWithDuplicateUniqueCode As New List(Of String)

            For i = 1 To Me.Count

                If StringIsNullOrEmpty(Me.Item(i - 1).UniqueCode) Then

                    operationsWithoutUniqueCode.Add(Me.Item(i - 1).ToString)

                Else

                    For j = i + 1 To Me.Count

                        If Me.Item(i - 1).UniqueCode.Trim.ToUpper = Me.Item(j - 1).UniqueCode.Trim.ToUpper Then
                            operationsWithDuplicateUniqueCode.Add(String.Format("{0} -> {1}", _
                                Me.Item(i - 1).ToString, Me.Item(j - 1).ToString))
                        End If

                    Next

                End If

            Next

            Dim exceptionMessage As String = ""

            If operationsWithoutUniqueCode.Count > 0 Then
                exceptionMessage = String.Format(My.Resources.Documents_BankOperationItemList_UniqueCodesMissing, _
                    vbCrLf, String.Join(vbCrLf, operationsWithoutUniqueCode.ToArray))
            End If

            If operationsWithDuplicateUniqueCode.Count > 0 Then
                exceptionMessage = AddWithNewLine(exceptionMessage, _
                    String.Format(My.Resources.Documents_BankOperationItemList_UniqueCodeDuplicates, _
                    vbCrLf, String.Join(vbCrLf, operationsWithDuplicateUniqueCode.ToArray)), False)
            End If

            If Not StringIsNullOrEmpty(exceptionMessage) Then
                Throw New Exception(exceptionMessage)
            End If

        End Sub

#End Region

    End Class

End Namespace