Imports AccControlsWinForms
Imports AccDataBindingsWinForms.Printing
Imports AccDataBindingsWinForms.CachedInfoLists
Imports ApskaitaObjects.General

Friend Class F_AccountList
    Implements ISupportsPrinting, ISingleInstanceForm

    Private ReadOnly _RequiredCachedLists As Type() = New Type() _
        {GetType(HelperLists.AssignableCRItemList)}

    Private _FormManager As CslaActionExtenderEditForm(Of AccountList)
    Private _ListViewManager As DataListViewEditControlManager(Of Account)
    Private _QueryManager As CslaActionExtenderQueryObject


    Private Sub F_Accounts_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

            ' AccountList.GetAccountList()
            _QueryManager.InvokeQuery(Of AccountList)(Nothing, "GetAccountList", _
                True, AddressOf OnDataSourceLoaded)

        Catch ex As Exception
            ShowError(New Exception("Klaida. Nepavyko gauti sąskaitų plano duomenų.", ex))
            DisableAllControls(Me)
        End Try

    End Sub

    Private Function SetDataSources(ByVal currentSource As AccountList) As Boolean

        If Not PrepareCache(Me, _RequiredCachedLists) Then Return False

        Try

            _ListViewManager = New DataListViewEditControlManager(Of Account) _
                (AccountListDataListView, Nothing, AddressOf DeleteAccounts, _
                 AddressOf AddNewAccount, Nothing, currentSource)

        Catch ex As Exception
            ShowError(New Exception("Klaida. Nepavyko gauti sąskaitų plano duomenų.", ex))
            DisableAllControls(Me)
            Return False
        End Try

        Dim CM2 As New ContextMenu()
        Dim CMItem1 As New MenuItem("Overwrite", AddressOf PasteAccButton_Click)
        CM2.MenuItems.Add(CMItem1)
        Dim CMItem2 As New MenuItem("Informacija apie formatą", AddressOf PasteAccButton_Click)
        CM2.MenuItems.Add(CMItem2)
        PasteAccButton.ContextMenu = CM2

        Dim CM1 As New ContextMenu()
        Dim CMItem3 As New MenuItem("Overwrite", AddressOf OpenFileButton_Click)
        CM1.MenuItems.Add(CMItem3)
        OpenFileAccButton.ContextMenu = CM1

        nOkButton.Enabled = ApskaitaObjects.General.AccountList.CanEditObject
        ApplyButton.Enabled = ApskaitaObjects.General.AccountList.CanEditObject

        Return True

    End Function

    Private Sub OnDataSourceLoaded(ByVal source As Object, ByVal exceptionHandled As Boolean)

        If exceptionHandled Then

            DisableAllControls(Me)
            Exit Sub

        ElseIf source Is Nothing Then

            ShowError(New Exception("Klaida. Nepavyko gauti sąskaitų plano duomenų."))
            DisableAllControls(Me)
            Exit Sub

        End If

        If Not SetDataSources(DirectCast(source, AccountList)) Then Exit Sub

        Try

            _FormManager = New CslaActionExtenderEditForm(Of AccountList) _
                (Me, AccountListBindingSource, DirectCast(source, AccountList), _
                 _RequiredCachedLists, nOkButton, ApplyButton, nCancelButton, _
                 Nothing, ProgressFiller1)

        Catch ex As Exception
            ShowError(New Exception("Klaida. Nepavyko gauti sąskaitų plano duomenų.", ex))
            DisableAllControls(Me)
            Exit Sub
        End Try

        _FormManager.ManageDataListViewStates(Me.AccountListDataListView)

    End Sub


    Private Sub DeleteAccounts(ByVal items As Account())
        If items Is Nothing OrElse items.Length < 1 Then Exit Sub
        For Each item As Account In items
            _FormManager.DataSource.Remove(item)
        Next
    End Sub

    Private Sub AddNewAccount()
        Dim newItem As Account = _FormManager.DataSource.AddNew()
        Try
            AccountListDataListView.EnsureVisible(AccountListDataListView.IndexOf(newItem))
        Catch ex As Exception
        End Try
    End Sub


    Private Sub CopyToClipboardButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles CopyToClipboardButton.Click

        Try
            Using busy As New StatusBusy
                Clipboard.SetText(_FormManager.DataSource.SaveToString(), TextDataFormat.UnicodeText)
            End Using
            MsgBox("Duomenys sėkminga nukopijuotį į clipboard'ą.", MsgBoxStyle.Information, "Info")
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub

    Private Sub PasteAccButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles PasteAccButton.Click

        If _FormManager.DataSource Is Nothing OrElse Not ApskaitaObjects.General.AccountList.CanEditObject Then Exit Sub

        If GetSenderText(sender).Trim.ToLower.Contains("informacija") Then
            MsgBox(ApskaitaObjects.General.Account.GetPasteStringColumnsDescription, MsgBoxStyle.Information, "Info")
            Clipboard.SetText(String.Join(vbTab, ApskaitaObjects.General.Account.GetPasteStringColumns))
            Exit Sub
        End If

        If GetSenderText(sender).Trim.ToLower.Contains("overwrite") _
           AndAlso _FormManager.DataSource.Count > 0 AndAlso _
           Not YesOrNo("Ar tikrai norite perrašyti visą sąskaitų planą?") Then
            Exit Sub
        End If

        Try
            Using busy As New StatusBusy
                _FormManager.DataSource.LoadAccountListFromString(Clipboard.GetText, _
                    GetSenderText(sender).Trim.ToLower.Contains("overwrite"))
            End Using
        Catch ex As Exception
            ShowError(ex)
            Exit Sub
        End Try

    End Sub

    Private Sub OpenFileButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles OpenFileAccButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        Dim filePath As String

        Using openFile As New OpenFileDialog
            openFile.InitialDirectory = AppPath()
            openFile.Filter = "Sąskaitų plano duomenys (*.txt)|*.txt|Visi failai|*.*"
            openFile.Multiselect = False
            If openFile.ShowDialog() <> Windows.Forms.DialogResult.OK Then Exit Sub
            filePath = openFile.FileName
        End Using

        If StringIsNullOrEmpty(filePath) Then Exit Sub

        If Not IO.File.Exists(filePath) Then
            MsgBox(String.Format("Klaida. Failas '{0}' neegzistuoja.", filePath), _
                MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        End If

        Try
            Using busy As New StatusBusy
                _FormManager.DataSource.LoadAccountListFromFile(filePath, _
                    GetSenderText(sender).Trim.ToLower.Contains("overwrite"))
            End Using
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub

    Private Sub SaveFileButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles SaveFileButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        Dim filePath As String

        Using saveFile As New SaveFileDialog
            saveFile.Filter = "Sąskaitų plano duomenys (*.txt)|*.txt|Visi failai|*.*"
            saveFile.AddExtension = True
            saveFile.DefaultExt = ".txt"
            If saveFile.ShowDialog() <> Windows.Forms.DialogResult.OK Then Exit Sub
            filePath = saveFile.FileName
        End Using

        If StringIsNullOrEmpty(filePath) Then Exit Sub

        Try
            Using busy As New StatusBusy
                _FormManager.DataSource.SaveToFile(filePath)
            End Using
            MsgBox("Failas sėkmingai išsaugotas.", MsgBoxStyle.Information, "Info")
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub


    Public Function GetMailDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetMailDropDownItems
        Return Nothing
    End Function

    Public Function GetPrintDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetPrintDropDownItems
        Return Nothing
    End Function

    Public Function GetPrintPreviewDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetPrintPreviewDropDownItems
        Return Nothing
    End Function

    Public Sub OnMailClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnMailClick
        If _FormManager.DataSource Is Nothing Then Exit Sub

        Using frm As New F_SendObjToEmail(_FormManager.DataSource, 0)
            frm.ShowDialog()
        End Using

    End Sub

    Public Sub OnPrintClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintClick
        If _FormManager.DataSource Is Nothing Then Exit Sub
        Try
            PrintObject(_FormManager.DataSource, False, 0, "SaskaituPlanas", Me, _
                _ListViewManager.GetCurrentFilterDescription(), _
                _ListViewManager.GetDisplayOrderIndexes())
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Public Sub OnPrintPreviewClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintPreviewClick
        If _FormManager.DataSource Is Nothing Then Exit Sub
        Try
            PrintObject(_FormManager.DataSource, True, 0, "SaskaituPlanas", Me, _
                _ListViewManager.GetCurrentFilterDescription(), _
                _ListViewManager.GetDisplayOrderIndexes())
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Public Function SupportsEmailing() As Boolean _
        Implements ISupportsPrinting.SupportsEmailing
        Return True
    End Function

    Private Sub AddFromTypicalAccountsAccButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles AddFromTypicalAccountsAccButton.Click

        Dim newItem As Account = Nothing

        Using dlg As New F_TypicalAccountInfoDialog
            If dlg.ShowDialog() <> DialogResult.OK Then Exit Sub
            If dlg.SelectedItems Is Nothing OrElse dlg.SelectedItems.Length < 1 Then Exit Sub
            For Each item As TypicalAccountInfo In dlg.SelectedItems
                Dim newAcount As Account = _FormManager.DataSource.AddNew()
                newAcount.ID = item.AccountNo
                newAcount.Name = item.AccountName
                newItem = newAcount
            Next
        End Using

        If Not newItem Is Nothing Then
            Try
                AccountListDataListView.EnsureVisible(AccountListDataListView.IndexOf(newItem))
            Catch ex As Exception
            End Try
        End If

    End Sub

End Class