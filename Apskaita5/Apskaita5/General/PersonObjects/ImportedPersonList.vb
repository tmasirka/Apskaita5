Imports System.Text.RegularExpressions
Namespace General

    ''' <summary>
    ''' Represents a list of <see cref="Person">person's</see> data to be imported to the database.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public NotInheritable Class ImportedPersonList
        Inherits BusinessListBase(Of ImportedPersonList, ImportedPerson)
        Implements IValidationMessageProvider

#Region " Business Methods "

        Public Overrides ReadOnly Property IsValid() As Boolean _
            Implements IValidationMessageProvider.IsValid
            Get
                Return MyBase.IsValid
            End Get
        End Property


        Public Function HasWarnings() As Boolean _
            Implements IValidationMessageProvider.HasWarnings
            For Each p As ImportedPerson In Me
                If p.BrokenRulesCollection.WarningCount > 0 Then Return True
            Next
            Return False
        End Function

        Public Function GetAllBrokenRules() As String _
            Implements IValidationMessageProvider.GetAllBrokenRules
            Dim result As String = GetAllBrokenRulesForList(Me)

            'Dim GeneralErrorString As String = ""
            'SomeGeneralValidationSub(GeneralErrorString)
            'AddWithNewLine(result, GeneralErrorString, False)

            Return result
        End Function

        Public Function GetAllWarnings() As String _
            Implements IValidationMessageProvider.GetAllWarnings
            Dim result As String = GetAllWarningsForList(Me)
            'Dim GeneralErrorString As String = ""
            'SomeGeneralValidationSub(GeneralErrorString)
            'AddWithNewLine(result, GeneralErrorString, False)

            Return result
        End Function


        Public Overrides Function Save() As ImportedPersonList

            If Not Me.Count > 0 Then Throw New Exception(My.Resources.General_ImportedPersonList_ListEmpty)

            If Not Me.IsValid Then Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, _
                vbCrLf, GetAllBrokenRules()))

            Dim result As ImportedPersonList = MyBase.Save()
            HelperLists.PersonInfoList.InvalidateCache()
            Return result

        End Function

#End Region

#Region " Authorization Rules "

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.Person2")
        End Function

#End Region

#Region " Factory Methods "

        ' used to implement automatic sort in datagridview
        <NonSerialized()> _
        Private _SortedList As Csla.SortedBindingList(Of ImportedPerson) = Nothing

        ''' <summary>
        ''' Gets an ImportedPersonList from data in paste string.
        ''' </summary>
        ''' <param name="Source">Paste string, lines delimited by CrLf, fields - by Tab.</param>
        ''' <remarks></remarks>
        Public Shared Function GetImportedPersonList(ByVal Source As String) As ImportedPersonList
            Return DataPortal.Fetch(Of ImportedPersonList)(New Criteria( _
                Source, vbCrLf, vbTab, "", ""))
        End Function

        ''' <summary>
        ''' Gets a sorted view of ImportedPersonList.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetSortedList() As Csla.SortedBindingList(Of ImportedPerson)
            If _SortedList Is Nothing Then _SortedList = New Csla.SortedBindingList(Of ImportedPerson)(Me)
            Return _SortedList
        End Function


        Private Sub New()
            ' require use of factory methods
            Me.AllowEdit = False
            Me.AllowNew = False
            Me.AllowRemove = True
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private _Source As String
            Private _LineDelimiter As String
            Private _ColumnDelimiter As String
            Private _ColumnWrapper As String
            Private _EscapeString As String
            Public ReadOnly Property Source() As String
                Get
                    Return _Source
                End Get
            End Property
            Public ReadOnly Property LineDelimiter() As String
                Get
                    Return _LineDelimiter
                End Get
            End Property
            Public ReadOnly Property ColumnDelimiter() As String
                Get
                    Return _ColumnDelimiter
                End Get
            End Property
            Public ReadOnly Property ColumnWrapper() As String
                Get
                    Return _ColumnWrapper
                End Get
            End Property
            Public ReadOnly Property EscapeString() As String
                Get
                    Return _EscapeString
                End Get
            End Property
            Public Sub New(ByVal nSource As String, ByVal nLineDelimiter As String, _
                ByVal nColumnDelimiter As String, ByVal nColumnWrapper As String, _
                ByVal nEscapeString As String)
                _Source = nSource
                _LineDelimiter = nLineDelimiter
                _ColumnDelimiter = nColumnDelimiter
                _ColumnWrapper = nColumnWrapper
                _EscapeString = nEscapeString
            End Sub
        End Class

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)

            If StringIsNullOrEmpty(criteria.Source) Then _
                Throw New Exception(My.Resources.General_ImportedPersonList_PasteStringEmpty)
            ' do not use Trim on delimiters, because CrLf and Tab get removed.
            If String.IsNullOrEmpty(criteria.LineDelimiter) Then _
                Throw New Exception(My.Resources.General_ImportedPersonList_LineDelimiterNull)
            If String.IsNullOrEmpty(criteria.ColumnDelimiter) Then _
                Throw New Exception(My.Resources.General_ImportedPersonList_FieldDelimiterNull)

            Dim lineDelimiter As String = criteria.LineDelimiter
            If lineDelimiter <> vbCrLf AndAlso lineDelimiter <> vbCr AndAlso lineDelimiter <> vbLf Then
                lineDelimiter = lineDelimiter.Trim.ToUpper
                lineDelimiter = lineDelimiter.Replace("\R\N", vbCrLf)
                lineDelimiter = lineDelimiter.Replace("\R", vbCr)
                lineDelimiter = lineDelimiter.Replace("\N", vbLf)
            End If

            Dim columnDelimiter As String = criteria.ColumnDelimiter
            If columnDelimiter <> vbTab Then
                columnDelimiter = columnDelimiter.Trim.ToUpper
                columnDelimiter = columnDelimiter.Replace("\T", vbTab)
            End If

            Dim pil As PersonInfoList = PersonInfoList.GetListChild
            Dim personCodeList As New List(Of String)
            For Each pi As PersonInfo In pil
                personCodeList.Add(pi.Code.Trim.ToUpper)
            Next

            Dim ail As AccountInfoList = AccountInfoList.GetListChild
            Dim accountList As New List(Of Long)
            For Each pi As AccountInfo In ail
                accountList.Add(pi.ID)
            Next

            Dim alreadyPresentCodes As New List(Of String)

            RaiseListChangedEvents = True

            For Each s As String In criteria.Source.Split(New String() {lineDelimiter}, _
                StringSplitOptions.RemoveEmptyEntries)
                Add(ImportedPerson.NewImportedPerson(s.Split(New String() {columnDelimiter}, _
                    StringSplitOptions.None), accountList, personCodeList, alreadyPresentCodes))
            Next

            RaiseListChangedEvents = False

        End Sub

        Protected Overrides Sub DataPortal_Update()

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)

            Using transaction As New SqlTransaction

                Try

                    RaiseListChangedEvents = False

                    DeletedList.Clear()

                    For Each item As ImportedPerson In Me
                        If item.IsValid Then item.Insert()
                    Next

                    Me.Clear()
                    DeletedList.Clear()

                    RaiseListChangedEvents = True

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

        End Sub

#End Region

    End Class

End Namespace