Imports MySql.Data.MySqlClient
Imports AccDataAccessLayer.DatabaseAccess
Imports System.Configuration
Namespace SqlServerSpecificMethods

    Friend Class MySqlCommandManager
        Implements ISqlCommandManager

        ' All paths are relative to:
        ' In case of winforms - to program instalation folder.
        ' In case of webservice - to App_Data. (see Helpers.Utilities.AppPath)
        Private Const Path_SecurityDatabaseScriptMySql As String = "\DbStructure\MySQL_accsecurity.sql"
        Private Const Path_MySqlDepositoryFile As String = "\SqlDepositories\MySQL_Depository.sqld"

        Public Sub TransactionBegin(Optional ByVal ThrowOnTransactionExists As Boolean = True) _
            Implements ISqlCommandManager.TransactionBegin

            Dim Conn As MySqlConnection = OpenConnection()
            Dim myTrans As MySqlTransaction = Conn.BeginTransaction()
            Dim myComm As New MySqlCommand
            myComm.Connection = Conn
            myComm.CommandTimeout = GetSqlQueryTimeOut()
            myComm.Transaction = myTrans

            Csla.ApplicationContext.LocalContext.Add(LC_TransactionObjectKey, myComm)
            Csla.ApplicationContext.LocalContext.Add(LC_TransactionStatementListKey, New List(Of String))

        End Sub

        Public Sub TransactionCommit() _
            Implements ISqlCommandManager.TransactionCommit

            Dim myTrans As MySqlTransaction = TransactionGetObject()

            Try
                myTrans.Commit()
            Catch ex As Exception
                Try
                    TransactionRollBack(ex)
                Catch e As Exception
                    TransactionEnd()
                    Throw
                End Try
            End Try

            TransactionEnd()

        End Sub

        Friend Function ExecuteCommand(ByVal CommandToExecute As DatabaseAccess.SQLCommand, _
            Optional ByRef RowsAffected As Integer = 0) As Long _
            Implements ISqlCommandManager.ExecuteCommand

            Dim Conn As MySqlConnection = Nothing

            Dim myCommand As MySqlCommand = GetCommand(Conn, CommandToExecute)

            Dim result As Long = -1

            Try
                RowsAffected = myCommand.ExecuteNonQuery()
                result = myCommand.LastInsertedId
            Catch ex As Exception
                If TransactionExists() Then
                    TransactionRollBack(New Exception("Klaida vykdant SQL komandą: " _
                        & CommandToExecute.ToString & vbCrLf & "Klaidos turinys: '" & _
                        ex.Message & "'.", ex))
                Else
                    Try
                        If Conn.State = ConnectionState.Open Then Conn.Close()
                        Conn.Dispose()
                    Catch e As Exception
                    End Try
                    Throw New Exception("Klaida vykdant SQL komandą: " & CommandToExecute.ToString _
                        & vbCrLf & "Klaidos turinys: '" & ex.Message & "'.", ex)
                End If
            End Try

            If Not TransactionExists() Then
                Try
                    If Conn.State = ConnectionState.Open Then Conn.Close()
                    Conn.Dispose()
                    myCommand.Dispose()
                Catch e As Exception
                End Try
            End If

            Return result

        End Function

        Friend Function FetchCommand(ByVal SelectCommand As DatabaseAccess.SQLCommand) _
            As System.Data.DataTable Implements ISqlCommandManager.FetchCommand

            Dim result As DataTable = Nothing

            Dim Conn As MySqlConnection = Nothing

            Dim myCommand As MySqlCommand = GetCommand(Conn, SelectCommand)

            Try

                Using myAdapter As New MySqlDataAdapter

                    myAdapter.SelectCommand = myCommand

                    result = New DataTable

                    myAdapter.Fill(result)

                End Using

            Catch ex As Exception

                If Not result Is Nothing Then result.Dispose()

                If TransactionExists() Then

                    TransactionRollBack(New Exception("Klaida vykdant SQL komandą: " _
                        & SelectCommand.ToString & vbCrLf & "Klaidos turinys: '" & _
                        ex.Message & "'.", ex))

                Else

                    Try
                        If Conn.State = ConnectionState.Open Then Conn.Close()
                        Conn.Dispose()
                    Catch e As Exception
                    End Try
                    Try
                        myCommand.Dispose()
                    Catch exd As Exception
                    End Try

                    Throw New Exception("Klaida vykdant SELECT sakinį (statement): " & _
                        SelectCommand.ToString & vbCrLf & "Klaidos turinys: '" & _
                        ex.Message & "'.", ex)

                End If

            End Try

            If Not TransactionExists() Then

                Try
                    If Conn.State = ConnectionState.Open Then Conn.Close()
                    Conn.Dispose()
                Catch e As Exception
                End Try
                Try
                    myCommand.Dispose()
                Catch exd As Exception
                End Try

            End If

            Return result

        End Function

        Friend Sub TransactionRollBack(ByVal e As System.Exception) _
            Implements ISqlCommandManager.TransactionRollBack

            Dim tmpList As List(Of String) = GetExecutedTransactionStatementsList()

            Dim currentQuery As String = "unidentified query"
            If tmpList.Count > 0 Then
                currentQuery = tmpList.Item(tmpList.Count - 1)
            End If

            Dim myError As New Exception("Klaida transakcijoje vykdant SQL sakinį " & _
                currentQuery & vbCrLf & "Klaidos turinys: '" & e.Message & "'.", e)

            Dim myTrans As MySqlTransaction = TransactionGetObject()

            Try
                myTrans.Rollback()
            Catch ex As MySqlException
                If Not myTrans.Connection Is Nothing Then
                    ' Jei nepavyko rollback reiskiasi labai labai negerai
                    ' Formuojam klaidos loga
                    Dim Tmp As String = vbCrLf & "KRITINĖ DUOMENŲ BAZĖS KLAIDA. Nepavyko atkurti " _
                        & "pradinių duomenų bazės įrašų. Siūlome šios žinutės pilną tekstą " _
                        & "perduoti programuotojui."
                    If tmpList.Count > 1 Then
                        Tmp = Tmp & vbCrLf & "ĮVYKDYTI SAKINIAI: " & vbCrLf
                    Else
                        Tmp = Tmp & vbCrLf & "Jokie sakiniai nebuvo (be klaidos) įvykdyti."
                    End If
                    For i As Integer = 1 To tmpList.Count - 1
                        Tmp = Tmp & vbCrLf & tmpList.Item(i - 1)
                    Next
                    Tmp = Tmp & vbCrLf & "Kritinės klaidos turinys: '" & ex.Message & "'."
                    Tmp = myError.Message & vbCrLf & Tmp
                    myError = New Exception(Tmp, e)
                End If
            End Try

            TransactionEnd()

            Throw myError

        End Sub

        Friend Sub TryLogin(ByVal CurrentIdentity As Security.AccIdentity, _
            ByRef cRoles As System.Collections.Generic.List(Of String)) _
            Implements ISqlCommandManager.TryLogin

            If cRoles Is Nothing Then
                cRoles = New List(Of String)
            Else
                cRoles.Clear()
            End If

            AccDataAccessLayer.DatabaseAccess.GetSQLDepository(CurrentIdentity.SqlServerType)

            Using Conn As New MySqlConnection(CurrentIdentity.ConnString)

                Try
                    Conn.Open()
                Catch ex As Exception

                    If Not Conn Is Nothing AndAlso Not Conn.State = ConnectionState.Closed Then Conn.Close()

                    If CurrentIdentity.IsSecuritySystemInternal Then

                        Throw New Exception("Klaida. Nepavyko prisijungti " _
                            & "prie SQL serverio, kreipkitės į serverio administratorių.", ex)

                    ElseIf TypeOf ex Is MySql.Data.MySqlClient.MySqlException AndAlso _
                        (CType(ex, MySql.Data.MySqlClient.MySqlException).ErrorCode = _
                        MySql.Data.MySqlClient.MySqlErrorCode.AccessDenied OrElse _
                        CType(ex, MySql.Data.MySqlClient.MySqlException).ErrorCode = _
                        MySql.Data.MySqlClient.MySqlErrorCode.PasswordNoMatch) Then

                        Throw New System.Security.SecurityException("Klaida. Jūs neturite teisės prisijungti " _
                            & "prie serverio arba nurodytas klaidingas slaptažodis.", ex)

                    ElseIf TypeOf ex Is MySql.Data.MySqlClient.MySqlException AndAlso _
                        CType(ex, MySql.Data.MySqlClient.MySqlException).ErrorCode = _
                        MySql.Data.MySqlClient.MySqlErrorCode.UnableToConnectToHost Then

                        Throw New Exception("Klaida. Išjungtas SQL serverio servisas " _
                            & "arba klaidingai nurodytas SQL serverio adresas ar portas.", ex)

                    Else

                        Throw New Exception("Klaida. Nepavyko prisijungti prie duomenų bazės: " _
                            & ex.Message, ex)

                    End If

                End Try

                Dim myComm As SQLCommand

                If CurrentIdentity.IsSecuritySystemInternal Then

                    If String.IsNullOrEmpty(CurrentIdentity.Database.Trim) Then
                        myComm = New SQLCommand("RawSQL", "CALL " & CurrentIdentity.SecurityDatabase _
                            & ".AccLoginInternal('" & CurrentIdentity.Name & "', '" _
                            & HashPasswordSha256(CurrentIdentity.Password) & "');")
                    Else
                        myComm = New SQLCommand("RawSQL", "CALL " & CurrentIdentity.SecurityDatabase & _
                            ".AccLoginDatabaseInternal('" & CurrentIdentity.Database & "', '" _
                            & CurrentIdentity.Name & "', '" & HashPasswordSha256(CurrentIdentity.Password) & "');")
                    End If

                Else
                    If String.IsNullOrEmpty(CurrentIdentity.Database.Trim) Then
                        myComm = New SQLCommand("RawSQL", "CALL " & CurrentIdentity.SecurityDatabase & ".AccLogin();")
                    Else
                        myComm = New SQLCommand("RawSQL", "CALL " & CurrentIdentity.SecurityDatabase & _
                            ".AccLoginDatabase('" & CurrentIdentity.Database & "');")
                    End If

                End If

                Try
                    Using myData As DataTable = FetchUsingConnection(Conn, myComm)

                        If String.IsNullOrEmpty(CurrentIdentity.Database.Trim) Then

                            If myData.Rows.Count < 1 OrElse IsDBNull(myData.Rows(0).Item(1)) _
                                OrElse Not Integer.TryParse(myData.Rows(0).Item(1).ToString, New Integer) Then

                                If Not Conn Is Nothing AndAlso Conn.State <> ConnectionState.Closed Then Conn.Close()
                                Throw New System.Security.SecurityException("Klaida. Jūs neturite teisės prisijungti " _
                                    & "prie serverio arba klaidingas slaptažodis.")

                            End If

                            If Convert.ToInt32(myData.Rows(0).Item(1).ToString) > 0 Then cRoles.Add(Name_AdminRole)

                        Else

                            If myData.Rows.Count < 1 Then

                                If Not Conn Is Nothing AndAlso Not Conn.State = ConnectionState.Closed Then _
                                    Conn.Close()
                                Throw New System.Security.SecurityException("Klaida. Jūs neturite teisės prisijungti " _
                                    & "prie šios įmonės duomenų bazės.")

                            End If


                            For Each dr As DataRow In myData.Rows
                                If dr.Item(0).ToString.Trim.ToLower = Name_AdminRole.ToLower.Trim Then
                                    cRoles.Clear()
                                    cRoles.Add(Name_AdminRole)
                                    Exit For
                                Else
                                    cRoles.Add(dr.Item(0).ToString.Trim & dr.Item(1).ToString.Trim)
                                End If
                            Next

                        End If

                        If String.IsNullOrEmpty(CurrentIdentity.Database.Trim) Then _
                            CurrentIdentity.SetUserRealName(CStrSafe(myData.Rows(0).Item(2)))

                    End Using

                Catch ex As Exception

                    If Not Conn Is Nothing AndAlso Conn.State <> ConnectionState.Closed Then Conn.Close()

                    If ex.Message.ToLower.Contains("denied") Then

                        Throw New Exception("Vartotojui nesuteikta prieiga prie vartotojų duomenų bazės. " _
                            & "Kreipkitės į serverio administratorių.")

                    ElseIf ex.Message.ToLower.Contains("does not exist") Then

                        If CurrentIdentity.IsSecuritySystemInternal OrElse _
                            CurrentIdentity.Name.Trim.ToLower = "root" Then

                            Using newConn As New MySqlConnection(CurrentIdentity.ConnString & ";Allow User Variables=True;")
                                Try
                                    newConn.Open()
                                    CreateUserDatabase(newConn, CurrentIdentity)
                                    If newConn.State <> ConnectionState.Closed Then newConn.Close()
                                Catch e As Exception
                                    If Not newConn Is Nothing AndAlso newConn.State <> ConnectionState.Closed Then _
                                        newConn.Close()
                                    Throw New Exception("Klaida. Nepavyko sukurti vartotojų duomenų bazės.", e)
                                End Try
                                cRoles.Add(Name_AdminRole)
                            End Using

                        Else

                            Throw New Exception("Nėra vartotojų duomenų bazės (nesukurta). " _
                                & "Norint sukurti vartotojų duomenų bazę, reikia prisijungti " _
                                & "'root' vardu. Kreipkitės į serverio administratorių.", ex)

                        End If

                    Else

                        Throw New Exception("Klaida. Nepavyko gauti vartotojo statuso: " & ex.Message, ex)

                    End If

                End Try

                If Not Conn Is Nothing AndAlso Not Conn.State = ConnectionState.Closed Then Conn.Close()

            End Using

        End Sub

        Public Sub FetchCompanyFromDbFile(ByVal DbFilePath As String, _
            ByVal cPassword As String, ByRef cCompanyName As String, _
            ByRef cCompanyCode As String) _
            Implements ISqlCommandManager.FetchCompanyFromDbFile
            Throw New NotSupportedException("Klaida. MySQL serveris nėra " & _
                "failinis serveris, gauti įmonės duomenis iš failo neįmanoma.")
        End Sub

        Private Function FetchUsingConnection(ByVal Conn As MySqlConnection, _
            ByVal myComm As SQLCommand) As DataTable

            Dim result As DataTable = New DataTable

            Using myCommand As New MySqlCommand

                myCommand.Connection = Conn
                myCommand.CommandText = myComm.Sentence
                myCommand.CommandTimeout = GetSqlQueryTimeOut()

                Using myAdapter As New MySqlDataAdapter

                    myAdapter.SelectCommand = myCommand

                    Try
                        myAdapter.Fill(result)
                    Catch ex As Exception
                        If Not Conn Is Nothing AndAlso Conn.State <> ConnectionState.Closed Then Conn.Close()
                        result.Dispose()
                        Throw New Exception("Klaida vykdant SELECT sakinį (statement): " & _
                            myCommand.CommandText & vbCrLf & "Klaidos turinys: '" & ex.Message & "'.", ex)
                    End Try

                End Using

            End Using

            Return result

        End Function

        Private Function ExecuteUsingConnection(ByVal Conn As MySqlConnection, _
            ByVal myComm As SQLCommand) As Integer

            Dim result As Integer = 0

            Using myCommand As New MySqlCommand

                myCommand.Connection = Conn
                myCommand.CommandText = myComm.Sentence
                myCommand.CommandTimeout = GetSqlQueryTimeOut()

                Try
                    result = myCommand.ExecuteNonQuery
                Catch ex As Exception
                    If Not Conn Is Nothing AndAlso Conn.State <> ConnectionState.Closed Then Conn.Close()
                    Throw New Exception("Klaida vykdant sakinį (statement): " & _
                        myCommand.CommandText & vbCrLf & "Klaidos turinys: '" & ex.Message & "'.", ex)
                End Try

            End Using

            Return result

        End Function

        Private Sub CreateUserDatabase(ByVal Conn As MySqlConnection, _
            ByVal CurrentIdentity As Security.AccIdentity)

            Dim SqlScript As String
            Try
                SqlScript = IO.File.ReadAllText(AppPath() & Path_SecurityDatabaseScriptMySql)
            Catch ex As Exception
                If Not Conn Is Nothing AndAlso Conn.State <> ConnectionState.Closed Then Conn.Close()
                Throw New Exception("Klaida. Nepavyko gauti vartototojų duomenų " _
                    & "bazės sukūrimo skripto: " & ex.Message, ex)
            End Try

            Dim myComm As SQLCommand

            For Each statement As String In SqlScript.Split(New String() {"<|>"}, _
                StringSplitOptions.RemoveEmptyEntries)

                myComm = New SQLCommand("RawSQL", statement.Replace("**Pav", CurrentIdentity.SecurityDatabase).Trim)
                Try
                    ExecuteUsingConnection(Conn, myComm)
                Catch ex As Exception
                    Throw New Exception("KRITINĖ DUOMENŲ BAZĖS KLAIDA. Nebaigta kurti vartotojų duomenų bazė.", ex)
                End Try

            Next

            If CurrentIdentity.IsSecuritySystemInternal Then
                myComm = New SQLCommand("RawSQL", "CALL " & CurrentIdentity.SecurityDatabase _
                    & ".AccLoginInternal('" & CurrentIdentity.Name & "', '" _
                    & HashPasswordSha256(CurrentIdentity.Password) & "');")
            Else
                myComm = New SQLCommand("RawSQL", "CALL " & CurrentIdentity.SecurityDatabase & ".AccLogin();")
            End If

            Try
                ExecuteUsingConnection(Conn, myComm)
            Catch ex As Exception
                Throw New Exception("KRITINĖ DUOMENŲ BAZĖS KLAIDA. Nebaigta kurti vartotojų duomenų bazė.", ex)
            End Try

        End Sub

        ''' <summary>
        ''' Tryes to open the connection. Returns open connection if succeded.
        ''' </summary>
        Private Function OpenConnection() As MySqlConnection
            Return OpenConnection(CType(ApplicationContext.User.Identity, _
                Security.AccIdentity).ConnString)
        End Function

        ''' <summary>
        ''' Tries to open the connection. Returns open connection if succeded.
        ''' </summary>
        Friend Shared Function OpenConnection(ByVal connectionString As String) As MySqlConnection

            Dim result As New MySqlConnection(connectionString)

            Try
                result.Open()
            Catch ex As Exception

                If Not result Is Nothing AndAlso Not result.State = ConnectionState.Closed Then result.Close()
                If Not result Is Nothing Then result.Dispose()

                If TypeOf ex Is MySql.Data.MySqlClient.MySqlException AndAlso _
                    (CType(ex, MySql.Data.MySqlClient.MySqlException).ErrorCode = _
                    MySql.Data.MySqlClient.MySqlErrorCode.AccessDenied OrElse _
                    CType(ex, MySql.Data.MySqlClient.MySqlException).ErrorCode = _
                    MySql.Data.MySqlClient.MySqlErrorCode.PasswordNoMatch) Then

                    Throw New Exception("Klaida. Jūs neturite teisės prisijungti " _
                        & "prie serverio arba nurodytas klaidingas slaptažodis.")

                ElseIf TypeOf ex Is MySql.Data.MySqlClient.MySqlException AndAlso _
                    CType(ex, MySql.Data.MySqlClient.MySqlException).ErrorCode = _
                    MySql.Data.MySqlClient.MySqlErrorCode.UnableToConnectToHost Then

                    Throw New Exception("Klaida. Išjungtas SQL serverio servisas " _
                        & "arba klaidingai nurodytas SQL serverio adresas ar portas.")

                Else

                    Throw New Exception("Klaida. Nepavyko prisijungti prie duomenų bazės: " _
                        & ex.Message, ex)

                End If

            End Try

            Return result

        End Function

        ''' <summary>
        ''' If a transaction exists returns the MySqlCommand object from Transaction object.
        ''' Else - returns a new MySqlCommand object.
        ''' </summary>
        ''' <param name="Conn">MySqlConnection object: if a transaction exists 
        ''' the existing object is used; else a new connection is opened.</param>
        ''' <param name="CurrentSqlCommand">SqlCommand object containing the SQL statement.</param>
        Private Function GetCommand(ByRef Conn As MySqlConnection, _
            ByVal CurrentSqlCommand As SQLCommand) As MySqlCommand

            Dim result As MySqlCommand

            If TransactionExists() Then
                result = TransactionGetCommand()
                result.Parameters.Clear()
                CType(ApplicationContext.LocalContext.Item(LC_TransactionStatementListKey), _
                    List(Of String)).Add(CurrentSqlCommand.ToString)
                Conn = TransactionGetConnection()
            Else
                If Conn Is Nothing Then Conn = OpenConnection()
                result = New MySqlCommand
                result.Connection = Conn
                result.CommandTimeout = GetSqlQueryTimeOut()
            End If

            AddWithSqlCommand(result, CurrentSqlCommand)

            Return result

        End Function

        ''' <summary>
        ''' Returns a new MySqlCommand object using the connection provided 
        ''' and data from SqlCommand provided.
        ''' </summary>
        ''' <param name="Conn">Connection to use for the MySqlCommand object.</param>
        ''' <param name="CurrentSqlCommand">SqlCommand object containing the SQL statement.</param>
        Private Function GetNewCommand(ByVal Conn As MySqlConnection, _
            ByVal CurrentSqlCommand As SQLCommand) As MySqlCommand

            Dim result As MySqlCommand

            result = New MySqlCommand
            result.Connection = Conn
            result.CommandTimeout = GetSqlQueryTimeOut()

            AddWithSqlCommand(result, CurrentSqlCommand)

            Return result

        End Function

        ''' <summary>
        ''' Adds the MySqlCommand object with the statement and params from SqlCommand object provided.
        ''' </summary>
        ''' <param name="myComm">The MySqlCommand object to which statement and params should be added.</param>
        ''' <param name="CurrentSqlCommand">SqlCommand object containing the statement and the params.</param>
        ''' <remarks></remarks>
        Private Sub AddWithSqlCommand(ByRef myComm As MySqlCommand, _
            ByVal CurrentSqlCommand As SQLCommand)

            myComm.CommandText = CurrentSqlCommand.Sentence
            myComm.Parameters.Clear()
            For i As Integer = 1 To CurrentSqlCommand.ParamsCount

                If Not CurrentSqlCommand.Params(i - 1).ToBeReplaced Then

                    'If CurrentSqlCommand.Params(i - 1).ValueType Is GetType(DateTime) Then

                    '    If DirectCast(CurrentSqlCommand.Params(i - 1).Value, DateTime).TimeOfDay.Ticks = 0 Then

                    '        myComm.Parameters.AddWithValue(CurrentSqlCommand.Params(i - 1).Name, _
                    '            DateToDbString(DirectCast(CurrentSqlCommand.Params(i - 1).Value, DateTime)))

                    '    Else

                    '        myComm.Parameters.AddWithValue(CurrentSqlCommand.Params(i - 1).Name, _
                    '            DateTimeToDbString(CType(CurrentSqlCommand.Params(i - 1).Value, Date)))

                    '    End If

                    'Else

                    '    myComm.Parameters.AddWithValue(CurrentSqlCommand.Params(i - 1).Name, _
                    '        CurrentSqlCommand.Params(i - 1).Value)

                    'End If

                    myComm.Parameters.AddWithValue(CurrentSqlCommand.Params(i - 1).Name, _
                        CurrentSqlCommand.Params(i - 1).Value)

                End If

            Next

        End Sub

        ''' <summary>
        ''' Ends distributed (interobject) MySQL transaction. I.e. disposes of connection and other objects.
        ''' </summary>
        Private Sub TransactionEnd()
            Try
                Dim myConn As MySqlConnection = TransactionGetConnection()
                Dim myComm As MySqlCommand = TransactionGetCommand()
                Dim myTrans As MySqlTransaction = TransactionGetObject()
                If myConn.State = ConnectionState.Open Then myConn.Close() 'issivalom
                myComm.Dispose()
                myTrans.Dispose()
                myConn.Dispose()
            Catch ex As Exception
            End Try
            Try
                ApplicationContext.LocalContext.Remove(LC_TransactionObjectKey)
                ApplicationContext.LocalContext.Remove(LC_TransactionStatementListKey)
            Catch ex As Exception
            End Try
        End Sub

        ''' <summary>
        ''' Returns MySQL transaction object.
        ''' </summary>
        Private Function TransactionGetObject() As MySqlTransaction
            Dim tmp As MySqlCommand = CType(Csla.ApplicationContext.LocalContext.Item( _
                LC_TransactionObjectKey), MySqlCommand)
            Return tmp.Transaction
        End Function

        ''' <summary>
        ''' Returns MySQL transaction command object.
        ''' </summary>
        Private Function TransactionGetCommand() As MySqlCommand
            Return CType(Csla.ApplicationContext.LocalContext.Item(LC_TransactionObjectKey), MySqlCommand)
        End Function

        ''' <summary>
        ''' Returns MySQL transaction connection object.
        ''' </summary>
        Private Function TransactionGetConnection() As MySqlConnection
            Dim tmp As MySqlCommand = CType(Csla.ApplicationContext.LocalContext.Item( _
                LC_TransactionObjectKey), MySqlCommand)
            Return tmp.Connection
        End Function

        Public Function DateTimeToDbString(ByVal nDate As Date) As String _
            Implements ISqlCommandManager.DateTimeToDbString
            Return nDate.ToString("yyyy'-'MM'-'dd HH':'mm':'ss")
        End Function

        Public Function DateToDbString(ByVal nDate As Date) As String _
            Implements ISqlCommandManager.DateToDbString
            Return nDate.ToString("yyyy'-'MM'-'dd")
        End Function

        Public Function TimeToDbString(ByVal nDate As Date) As String _
            Implements ISqlCommandManager.TimeToDbString
            Return nDate.ToString("HH':'mm':'ss")
        End Function

        Public Sub ChangePassword(ByVal NewPassword As String) _
            Implements ISqlCommandManager.ChangePassword

            If GetCurrentIdentity.IsSecuritySystemInternal Then

                Dim myComm As New SQLCommand("RawSQL", "UPDATE users SET Password=?PS " _
                    & "WHERE TRIM(LOWER(Name))=TRIM(LOWER(?NM));")
                myComm.AddParam("?PS", HashPasswordSha256(NewPassword.Trim))
                myComm.AddParam("?NM", GetCurrentIdentity.Name.Trim.ToLower)

                Using ChangedDb As New ChangedDatabase(GetCurrentIdentity.SecurityDatabase)
                    myComm.Execute()
                End Using

            Else

                Dim myComm As New SQLCommand("RawSQL", "SET PASSWORD = PASSWORD(?PS);")
                myComm.AddParam("?PS", NewPassword.Trim)
                myComm.Execute()

            End If

        End Sub

        Public Sub DropDatabase(ByVal DatabaseName As String) _
            Implements ISqlCommandManager.DropDatabase

            Dim myComm As New SQLCommand("RawSQL", "DROP DATABASE " & DatabaseName.Trim & ";")
            myComm.Execute()

        End Sub

        Public Sub FetchCompanyInfoData(ByVal DatabaseName As String, _
            ByRef CompanyName As String, ByRef CompanyID As String) _
            Implements ISqlCommandManager.FetchCompanyInfoData

            Dim myComm As New SQLCommand("GetCompanyByDatabase")
            myComm.AddParam("**DBName", DatabaseName.Trim)

            Using CompanyData As DataTable = myComm.Fetch

                If CompanyData.Rows.Count < 1 Then Throw New Exception( _
                    "Duomenų bazėje nėra įmonės pagrindinių duomenų.")

                CompanyName = CompanyData.Rows(0).Item(0).ToString.Trim
                CompanyID = CompanyData.Rows(0).Item(1).ToString.Trim

            End Using

        End Sub

        Public Function GetAccesibleDatabaseList() As System.Collections.Generic.List(Of String) _
            Implements ISqlCommandManager.GetAccesibleDatabaseList

            Dim result As List(Of String) = Nothing

            Dim myComm As SQLCommand

            If GetCurrentIdentity.IsSecuritySystemInternal Then
                myComm = New SQLCommand("RawSQL", "CALL " & GetCurrentIdentity.SecurityDatabase _
                    & ".GetAccessibleDatabasesInternal(?UN, ?UP, ?NC);")
                myComm.AddParam("?UN", GetCurrentIdentity.Name)
                myComm.AddParam("?UP", HashPasswordSha256(GetCurrentIdentity.Password))
                myComm.AddParam("?NC", GetCurrentIdentity.DatabaseNamingConvention)
            Else
                myComm = New SQLCommand("RawSQL", "SHOW DATABASES LIKE '" _
                    & GetCurrentIdentity.DatabaseNamingConvention & "%';")
            End If

            Using myData As DataTable = myComm.Fetch
                result = New List(Of String)
                For Each dr As DataRow In myData.Rows
                    If Not dr.Item(0) Is Nothing AndAlso Not IsDBNull(dr.Item(0)) AndAlso _
                        Not String.IsNullOrEmpty(dr.Item(0).ToString.Trim) Then _
                        result.Add(dr.Item(0).ToString.Trim)
                Next
            End Using

            Return result

        End Function

        Public Function GetConnectionString(ByVal TargetIdentity As Security.AccIdentity) As String _
            Implements ISqlCommandManager.GetConnectionString

            If TargetIdentity.IsSecuritySystemInternal Then

                If String.IsNullOrEmpty(TargetIdentity.Database.Trim) Then
                    Return ConfigurationManager.ConnectionStrings( _
                        AppSettingsKey_SqlConnectionString).ConnectionString
                Else
                    Return ConfigurationManager.ConnectionStrings( _
                        AppSettingsKey_SqlConnectionString).ConnectionString & ";database=" _
                        & TargetIdentity.Database.Trim
                End If

            Else

                If String.IsNullOrEmpty(TargetIdentity.SQLServer.Trim) OrElse _
                    String.IsNullOrEmpty(TargetIdentity.SQLPort.Trim) OrElse _
                    String.IsNullOrEmpty(TargetIdentity.Name.Trim) OrElse _
                    String.IsNullOrEmpty(TargetIdentity.Password.Trim) Then _
                    Throw New Exception("Klaida. Nenurodytas vartotojo vardas, slaptažodis, " _
                    & "serverio adresas arba portas.")

                Dim SslString As String = ""
                If TargetIdentity.UseSSL Then
                    If TargetIdentity.SslCertificateInstalled Then
                        SslString = ";Certificate Store Location=CurrentUser"
                    ElseIf Not TargetIdentity.SslCertificateFile Is Nothing AndAlso _
                        Not String.IsNullOrEmpty(TargetIdentity.SslCertificateFile.Trim) Then
                        SslString = ";CertificateFile=" & TargetIdentity.SslCertificateFile.Trim
                        If Not TargetIdentity.SslCertificatePassword Is Nothing AndAlso _
                            Not String.IsNullOrEmpty(TargetIdentity.SslCertificatePassword.Trim) Then
                            SslString = SslString & "CertificatePassword=" _
                                & TargetIdentity.SslCertificatePassword.Trim
                        End If
                    End If
                    SslString = SslString & ";SSL Mode=Preferred"
                End If

                Dim charsetString As String = ""
                If Not TargetIdentity.SqlConnectionCharSet Is Nothing AndAlso _
                        Not String.IsNullOrEmpty(TargetIdentity.SqlConnectionCharSet.Trim) Then
                    charsetString = ";CharSet=" & TargetIdentity.SqlConnectionCharSet.Trim
                Else
                    charsetString = ";CharSet=cp1257"
                End If

                If String.IsNullOrEmpty(TargetIdentity.Database.Trim) Then
                    Return "Server=" & TargetIdentity.SQLServer & ";" & "Port=" _
                        & TargetIdentity.SQLPort & ";" & "user id=" & TargetIdentity.Name _
                        & ";" & "password=" & TargetIdentity.Password & charsetString & SslString
                Else
                    Return "Server=" & TargetIdentity.SQLServer & ";" & "Port=" _
                        & TargetIdentity.SQLPort & ";" & "user id=" & TargetIdentity.Name & _
                        ";" & "password=" & TargetIdentity.Password & ";" & "database=" _
                        & TargetIdentity.Database & charsetString & SslString
                End If

            End If

        End Function

        Public Function GetSqlDepositoryFileName() As String _
            Implements ISqlCommandManager.GetSqlDepositoryFileName
            Return AppPath() & Path_MySqlDepositoryFile
        End Function

    End Class

End Namespace