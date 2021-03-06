Imports AccControlsWinForms
Imports ApskaitaObjects.General

Friend Class F_ImportedPersonList
    Implements ISingleInstanceForm

    Private _FormManager As CslaActionExtenderEditForm(Of ImportedPersonList)
    Private _ListViewManager As DataListViewEditControlManager(Of ImportedPerson)
    Private _QueryManager As CslaActionExtenderQueryObject


    Private Sub F_ImportedPersonList_FormClosing(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        MyCustomSettings.GetListViewLayOut(ImportedPersonListDataListView)
        MyCustomSettings.GetFormLayout(Me)

    End Sub

    Private Sub F_ImportedPersonList_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            _FormManager = New CslaActionExtenderEditForm(Of ImportedPersonList) _
                (Me, ImportedPersonListBindingSource, Nothing, Nothing, _
                ApplyButton, Nothing, Nothing, Nothing, ProgressFiller1)

            _FormManager.ManageDataListViewStates(ImportedPersonListDataListView)

            _ListViewManager = New DataListViewEditControlManager(Of ImportedPerson) _
                (ImportedPersonListDataListView, Nothing, Nothing, Nothing, Nothing, Nothing)

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
        End Try

    End Sub


    Private Sub PasteButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles PasteButton.Click
        LoadData(Clipboard.GetText)
    End Sub

    Private Sub OpenFileButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles OpenFileButton.Click

        Dim fileName As String

        Using ofd As New OpenFileDialog
            ofd.Multiselect = False
            If ofd.ShowDialog(Me) <> System.Windows.Forms.DialogResult.OK Then Exit Sub
            fileName = ofd.FileName
        End Using
        If StringIsNullOrEmpty(fileName) OrElse Not IO.File.Exists(fileName) Then Exit Sub

        Dim source As String

        Try
            Using busy As New StatusBusy
                source = IO.File.ReadAllText(fileName, New System.Text.UnicodeEncoding)
            End Using
        Catch ex As Exception
            ShowError(New Exception("Klaida. Nepavyko atidaryti failo: " & ex.Message, ex))
            Exit Sub
        End Try

        LoadData(source)

    End Sub

    Private Sub HelpButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles nHelpButton.Click

        MsgBox("Importuojami duomenys, nepriklausomai nuo šaltinio, turi atitikti šiuos reikalavimus:" _
            & vbCrLf & "Duomenų eilutės turi būti atskirtos naudojant cr lf simbolius." _
            & vbCrLf & "Duomenų stulpeliai turi būti atskirti naudojant tab simbolį." _
            & vbCrLf & "Duomenyse negali būti simbolių, naudojamų kaip skirtukai, t.y. cr lf arba tab." _
            & vbCrLf & ImportedPerson.GetPasteStringColumnsDescription, MsgBoxStyle.Information, "Info")
        Clipboard.SetText(String.Join(vbTab, ImportedPerson.GetPasteStringColumns), TextDataFormat.UnicodeText)

    End Sub


    Private Sub LoadData(ByVal source As String)

        _QueryManager.InvokeQuery(Of ImportedPersonList)(Nothing, "GetImportedPersonList", _
            False, AddressOf OnDataLoaded, source)

    End Sub

    Private Sub OnDataLoaded(ByVal result As Object, ByVal exceptionHandled As Boolean)

        If result Is Nothing Then Exit Sub

        _FormManager.AddNewDataSource(DirectCast(result, ImportedPersonList))

    End Sub

End Class