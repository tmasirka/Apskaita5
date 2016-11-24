Imports ApskaitaObjects.HelperLists
Imports ApskaitaObjects.ActiveReports
Imports ApskaitaObjects.ActiveReports.Declarations
Imports ApskaitaObjects.Documents
Imports AccControlsWinForms
Imports AccDataBindingsWinForms.Printing
Imports AccDataBindingsWinForms.CachedInfoLists
Imports ApskaitaObjects.Attributes
Imports BrightIdeasSoftware
Imports System.IO
Imports System.IO.Compression

Friend Class F_InvoiceInfoList
    Implements ISupportsPrinting

    Private ReadOnly _RequiredCachedLists As Type() = New Type() _
        {GetType(HelperLists.PersonInfoList), GetType(AccDataAccessLayer.Security.UserProfile)}

    Private _FormManager As CslaActionExtenderReportForm(Of InvoiceInfoItemList)
    Private _ListViewManager As DataListViewEditControlManager(Of InvoiceInfoItem)
    Private _QueryManager As CslaActionExtenderQueryObject

    Private _PrintDropDown As Windows.Forms.ToolStripDropDown = Nothing
    Private _EmailDropDown As Windows.Forms.ToolStripDropDown = Nothing
    Private _PrintInvoices As Boolean = True
    Private _PrintVersion As Integer = 0

    Private _SelectedInvoices As List(Of InvoiceInfoItem) = Nothing
    Private _DateFrom As Date = Today
    Private _DateTo As Date = Today
    Private _OnlyAllowSingleSelection As Boolean = False


    Public ReadOnly Property SelectedInvoices() As List(Of InvoiceInfoItem)
        Get
            Return _SelectedInvoices
        End Get
    End Property


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    ''' <summary>
    ''' Use to show the form as modal dialog for invoice info selection.
    ''' </summary>
    ''' <param name="dateFrom">prefered period start date</param>
    ''' <param name="dateTo">prefered period end date</param>
    ''' <param name="onlyAllowSingleSelection">whether only to allow user to select a single invoice 
    ''' (not multiple)</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal dateFrom As Date, ByVal dateTo As Date, ByVal onlyAllowSingleSelection As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _DateFrom = dateFrom
        _DateTo = dateTo
        _OnlyAllowSingleSelection = onlyAllowSingleSelection

    End Sub


    Private Sub F_InvoiceInfoList_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If Not SetDataSources() Then Exit Sub

        Me.Panel1.Visible = Me.Modal
        Me.ExportFFDataButton.Visible = Not Me.Modal
        Me.ExportToSafTAccButton.Visible = Not Me.Modal
        Me.MinimizeBox = Not Me.Modal
        Me.MaximizeBox = Not Me.Modal

        If Me.Modal Then
            If _OnlyAllowSingleSelection Then
                Me.Text = "Pasirinkite sąskaitą faktūrą..."
            Else
                Me.Text = "Pasirinkite sąskaitas faktūras..."
            End If
        End If

    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, _RequiredCachedLists) Then Return False

        Try

            If Me.Modal Then

                _ListViewManager = New DataListViewEditControlManager(Of InvoiceInfoItem) _
                    (InvoiceInfoItemListDataListView, Nothing, Nothing, Nothing, Nothing, Nothing)

                _ListViewManager.AddCancelButton = False
                _ListViewManager.AddButtonHandler("Pasirinkti", "Pasirinkti", AddressOf SelectItem)

            Else

                _ListViewManager = New DataListViewEditControlManager(Of InvoiceInfoItem) _
                (InvoiceInfoItemListDataListView, ContextMenuStrip1, _
                 Nothing, Nothing, Nothing, Nothing)

                _ListViewManager.AddCancelButton = True
                _ListViewManager.AddButtonHandler("Keisti", "Keisti sąskaitos duomenis.", _
                    AddressOf ChangeItem)
                _ListViewManager.AddButtonHandler("Ištrinti", "Pašalinti sąskaitos duomenis iš duomenų bazės.", _
                    AddressOf DeleteItem)
                _ListViewManager.AddButtonHandler("Kopijuoti", "Sukurti sąskaitos faktūros kopiją.", _
                    AddressOf CopyItem)

                _ListViewManager.AddMenuItemHandler(ChangeItem_MenuItem, AddressOf ChangeItem)
                _ListViewManager.AddMenuItemHandler(DeleteItem_MenuItem, AddressOf DeleteItem)
                _ListViewManager.AddMenuItemHandler(CopyItem_MenuItem, AddressOf CopyItem)

            End If

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

            ' InvoiceInfoItemList.GetList(infoType, dateFrom, dateTo, personFilter)
            _FormManager = New CslaActionExtenderReportForm(Of InvoiceInfoItemList) _
                (Me, InvoiceInfoItemListBindingSource, Nothing, _RequiredCachedLists, RefreshButton, _
                 ProgressFiller1, "GetList", AddressOf GetReportParams)

            _FormManager.ManageDataListViewStates(InvoiceInfoItemListDataListView)

            PrepareControl(RegisterTypeComboBox, New LocalizedEnumFieldAttribute( _
                GetType(InvoiceInfoType), False, ""))
            PrepareControl(PersonAccGridComboBox, New PersonFieldAttribute( _
                ValueRequiredLevel.Optional, True, True, False))

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Return False
        End Try

        If Me.Modal Then

            DateFromDateTimePicker.Value = _DateFrom
            DateToDateTimePicker.Value = _DateTo

        Else

            DateFromDateTimePicker.Value = Today.AddMonths(-1)

            Dim CM As New ContextMenu()
            Dim CMItem1 As New MenuItem("v. 1", AddressOf ExportFFDataButton_Click)
            CM.MenuItems.Add(CMItem1)
            ExportFFDataButton.ContextMenu = CM

        End If

        RegisterTypeComboBox.SelectedIndex = 0

        Return True

    End Function


    Private Function GetReportParams() As Object()

        Dim personFilter As HelperLists.PersonInfo = Nothing
        Try
            personFilter = DirectCast(PersonAccGridComboBox.SelectedValue, HelperLists.PersonInfo)
        Catch ex As Exception
        End Try

        Dim registerType As InvoiceInfoType = InvoiceInfoType.InvoiceReceived
        Try
            registerType = Utilities.ConvertLocalizedName(Of InvoiceInfoType)(RegisterTypeComboBox.SelectedItem.ToString)
        Catch ex As Exception
        End Try

        'InvoiceInfoItemList.GetList(registerType, DateFromDateTimePicker.Value, _
        '    DateToDateTimePicker.Value, personFilter)
        Return New Object() {registerType, DateFromDateTimePicker.Value, _
            DateToDateTimePicker.Value, personFilter}

    End Function

    Private Sub ChangeItem(ByVal item As InvoiceInfoItem)

        If item Is Nothing Then Exit Sub

        If _FormManager.DataSource.InfoType = InvoiceInfoType.InvoiceMade Then
            'InvoiceMade.GetInvoiceMade(item.ID)
            _QueryManager.InvokeQuery(Of InvoiceMade)(Nothing, "GetInvoiceMade", True, _
                AddressOf OpenObjectEditForm, item.ID)
        Else
            'InvoiceReceived.GetInvoiceReceived(item.ID)
            _QueryManager.InvokeQuery(Of InvoiceReceived)(Nothing, "GetInvoiceReceived", True, _
                AddressOf OpenObjectEditForm, item.ID)

        End If

    End Sub

    Private Sub DeleteItem(ByVal item As InvoiceInfoItem)

        If item Is Nothing Then Exit Sub

        If _FormManager.DataSource.InfoType = InvoiceInfoType.InvoiceMade Then
            If CheckIfObjectEditFormOpen(Of InvoiceMade)(item.ID, True, True) Then Exit Sub
        Else
            If CheckIfObjectEditFormOpen(Of InvoiceReceived)(item.ID, True, True) Then Exit Sub
        End If

        If Not YesOrNo("Ar tikrai norite pašalinti dokumento duomenis iš duomenų bazės?") Then Exit Sub

        If _FormManager.DataSource.InfoType = InvoiceInfoType.InvoiceMade Then
            'InvoiceMade.DeleteInvoiceMade(item.ID)
            _QueryManager.InvokeQuery(Of InvoiceMade)(Nothing, "DeleteInvoiceMade", False, _
                AddressOf OnItemDeleted, item.ID)
        Else
            'InvoiceReceived.DeleteInvoiceReceived(item.ID)
            _QueryManager.InvokeQuery(Of InvoiceReceived)(Nothing, "DeleteInvoiceReceived", False, _
                AddressOf OnItemDeleted, item.ID)
        End If

    End Sub

    Private Sub OnItemDeleted(ByVal result As Object, ByVal exceptionHandled As Boolean)
        If exceptionHandled Then Exit Sub
        If Not YesOrNo("Dokumento duomenys sėkmingai pašalinti iš įmonės duomenų bazės. Atnaujinti sąrašą?") Then Exit Sub
        RefreshButton.PerformClick()
    End Sub

    Private Sub CopyItem(ByVal item As InvoiceInfoItem)

        If item Is Nothing Then Exit Sub

        If _FormManager.DataSource.InfoType = InvoiceInfoType.InvoiceMade Then
            'InvoiceMade.GetInvoiceMade(item.ID)
            _QueryManager.InvokeQuery(Of InvoiceMade)(Nothing, "GetInvoiceMade", True, _
                AddressOf OnInvoiceFetched, item.ID)
        Else
            'InvoiceReceived.GetInvoiceReceived(item.ID)
            _QueryManager.InvokeQuery(Of InvoiceReceived)(Nothing, "GetInvoiceReceived", True, _
                AddressOf OnInvoiceFetched, item.ID)

        End If

    End Sub

    Private Sub OnInvoiceFetched(ByVal result As Object, ByVal exceptionHandled As Boolean)

        If result Is Nothing Then Exit Sub

        Try
            If TypeOf result Is InvoiceMade Then
                OpenObjectEditForm(DirectCast(result, InvoiceMade).GetInvoiceMadeCopy())
            Else
                OpenObjectEditForm(DirectCast(result, InvoiceReceived).GetInvoiceReceivedCopy())
            End If
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub

    Private Sub SelectItem(ByVal item As InvoiceInfoItem)

        If item Is Nothing Then Exit Sub

        _SelectedInvoices = New List(Of InvoiceInfoItem)
        _SelectedInvoices.Add(item)

        Me.DialogResult = DialogResult.OK
        Me.Close()

    End Sub

    Private Sub ExportFFDataButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ExportFFDataButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        Dim fileName As String = ""

        Using sfd As New SaveFileDialog
            sfd.Filter = "FFData failai|*.ffdata|Visi failai|*.*"
            sfd.CheckFileExists = False
            sfd.AddExtension = True
            sfd.DefaultExt = ".ffdata"
            If sfd.ShowDialog() <> Windows.Forms.DialogResult.OK Then Exit Sub
            fileName = sfd.FileName.Trim
        End Using

        If StringIsNullOrEmpty(fileName) Then Exit Sub

        Dim version As Integer
        If GetSenderText(sender).Trim.ToLower.Contains("1") Then
            version = 1
        Else
            version = 2
        End If

        Dim declaration As IInvoiceRegisterDeclaration
        If _FormManager.DataSource.InfoType = InvoiceInfoType.InvoiceMade AndAlso version = 1 Then
            declaration = New InvoiceRegisterFR0672_1
        ElseIf _FormManager.DataSource.InfoType = InvoiceInfoType.InvoiceMade AndAlso version = 2 Then
            declaration = New InvoiceRegisterFR0672_2
        ElseIf _FormManager.DataSource.InfoType = InvoiceInfoType.InvoiceMade AndAlso version = 1 Then
            declaration = New InvoiceRegisterFR0671_1
        Else
            declaration = New InvoiceRegisterFR0671_2
        End If

        Try

            Dim warnings As String = ""

            Using busy As New StatusBusy
                _FormManager.DataSource.SaveToFfData(fileName, declaration, warnings)
            End Using

            Dim message As String
            If StringIsNullOrEmpty(warnings) Then
                message = "Failas sėkmingai išsaugotas. Atidaryti?"
            Else
                message = "DĖMESIO. Išsaugant failą pastebėtos klaidos:" _
                & vbCrLf & warnings & vbCrLf & vbCrLf & "Atidaryti išsaugotą failą?"
            End If

            If YesOrNo(message) Then
                System.Diagnostics.Process.Start(fileName)
            End If

        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub

    Private Sub ExportToSafTAccButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ExportToSafTAccButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        Dim selectedIds As Integer() = Nothing
        Try
            selectedIds = GetCheckedInvoicesIds(False)
        Catch ex As Exception
            ShowError(ex)
            Exit Sub
        End Try

        Dim fileName As String = ""

        Using sfd As New SaveFileDialog
            sfd.Filter = "GZip failai|*.gz|XML failai|*.xml|Visi failai|*.*"
            sfd.CheckFileExists = False
            sfd.AddExtension = True
            sfd.DefaultExt = ".gz"
            If sfd.ShowDialog() <> Windows.Forms.DialogResult.OK Then Exit Sub
            fileName = sfd.FileName.Trim
        End Using

        If StringIsNullOrEmpty(fileName) Then Exit Sub

        Dim version As Integer = 1
        'If GetSenderText(sender).Trim.ToLower.Contains("1") Then
        '    version = 1
        'Else
        '    version = 2
        'End If

        Dim declaration As IInvoiceRegisterSafT
        declaration = New InvoiceRegisterSafT_1()
        'If version = 1 Then
        '    declaration = New InvoiceRegisterSafT_1()
        'Else
        '    declaration = New InvoiceRegisterFR0671_2
        'End If

        If declaration.ValidTo < Today Then
            If Not YesOrNo(String.Format("Pasirinkta SAF-T duomenų versija nebegaioja. Ji galiojo nuo {0} iki {1}. {2} Ar tikrai norite išsaugoti duomenis šia versija?", _
                declaration.ValidFrom.ToString("yyyy-MM-dd"), declaration.ValidTo.ToString("yyyy-MM-dd"), vbCrLf)) Then
                Exit Sub
            End If
        End If

        Try

            Dim warningMessage As String = ""
            Dim result As String

            Using busy As New StatusBusy

                result = declaration.GetXmlString(_FormManager.DataSource, _
                    MyCustomSettings.LastUpdateDate.ToString("yyyyMMdd"), _
                    selectedIds, warningMessage)

            End Using

            If Not StringIsNullOrEmpty(warningMessage) Then
                If Not YesOrNo(warningMessage & vbCrLf & vbCrLf _
                    & "Ar tikrai norite tęsti?") Then Exit Sub
            End If

            Using busy As New StatusBusy

                If IO.Path.GetExtension(fileName).Trim.ToLower = ".xml" Then
                    IO.File.WriteAllText(fileName, result, New System.Text.UTF8Encoding(False))
                Else
                    fileName = fileName.Replace(IO.Path.GetExtension(fileName), _
                        ".xml" & IO.Path.GetExtension(fileName))
                    IO.File.WriteAllBytes(fileName, Compress(result))
                End If
            End Using

            If YesOrNo("Failas sėkmingai išsaugotas. Atidaryti?") Then
                System.Diagnostics.Process.Start(fileName)
            End If

        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub


    Private Sub nOkButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles nOkButton.Click

        If Not Me.Modal Then Exit Sub

        If Me.InvoiceInfoItemListDataListView.CheckedObjects Is Nothing _
            OrElse Me.InvoiceInfoItemListDataListView.CheckedObjects.Count < 1 Then
            MsgBox("Klaida. Nepasirinkta nė viena sąskaita.", MsgBoxStyle.Exclamation, "Klaida.")
            Exit Sub
        ElseIf _OnlyAllowSingleSelection AndAlso Me.InvoiceInfoItemListDataListView.CheckedObjects.Count > 1 Then
            MsgBox("Klaida. Leidžiama pasirinkti tik vieną sąskaitą.", MsgBoxStyle.Exclamation, "Klaida.")
            Exit Sub
        End If

        _SelectedInvoices = New List(Of InvoiceInfoItem)
        For Each selectedItem As Object In Me.InvoiceInfoItemListDataListView.CheckedObjects
            _SelectedInvoices.Add(DirectCast(selectedItem, InvoiceInfoItem))
        Next

        Me.DialogResult = DialogResult.OK
        Me.Close()

    End Sub

    Private Sub nCancelButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles nCancelButton.Click
        If Not Me.Modal Then Exit Sub
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub


    Public Function GetMailDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetMailDropDownItems

        If _EmailDropDown Is Nothing Then
            _EmailDropDown = New ToolStripDropDown
            _EmailDropDown.Items.Add("Siųsti pasirinktas", Nothing, AddressOf OnMailClick)
            _EmailDropDown.Items.Add("Siųsti pasirinktas lietuvių klb.", Nothing, AddressOf OnMailClick)
        End If

        Return _EmailDropDown

    End Function

    Public Function GetPrintDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetPrintDropDownItems

        If _PrintDropDown Is Nothing Then
            _PrintDropDown = New ToolStripDropDown
            _PrintDropDown.Items.Add("Spausdinti pasirinktas", Nothing, AddressOf OnPrintClick)
            _PrintDropDown.Items.Add("Spausdinti pasirinktas Lietuvių klb.", Nothing, AddressOf OnPrintClick)

        End If

        Return _PrintDropDown

    End Function

    Public Function GetPrintPreviewDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetPrintPreviewDropDownItems
        Return Nothing
    End Function

    Public Sub OnMailClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnMailClick

        If _FormManager.DataSource Is Nothing Then Exit Sub

        If GetSenderText(sender).ToLower.Contains("pasirinktas") Then

            If _FormManager.DataSource.InfoType <> InvoiceInfoType.InvoiceMade Then Exit Sub

            _PrintInvoices = False
            If GetSenderText(sender).ToLower.Contains("lietuvių") Then
                _PrintVersion = 1
            Else
                _PrintVersion = 0
            End If

            Try

                _QueryManager.InvokeQuery(Of BusinessObjectCollection(Of InvoiceMade))(Nothing, _
                    "GetBusinessObjectCollection", True, AddressOf OnInvoicesFetched, _
                    GetCheckedInvoicesIds(True))

            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try

        Else

            Using frm As New F_SendObjToEmail(_FormManager.DataSource, 0)
                frm.ShowDialog()
            End Using

        End If

    End Sub

    Public Sub OnPrintClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintClick

        If _FormManager.DataSource Is Nothing Then Exit Sub

        If GetSenderText(sender).ToLower.Contains("pasirinktas") Then

            If _FormManager.DataSource.InfoType <> InvoiceInfoType.InvoiceMade Then Exit Sub

            _PrintInvoices = True
            If GetSenderText(sender).ToLower.Contains("lietuvių") Then
                _PrintVersion = 1
            Else
                _PrintVersion = 0
            End If
            Try

                _QueryManager.InvokeQuery(Of BusinessObjectCollection(Of InvoiceMade))(Nothing, _
                    "GetBusinessObjectCollection", True, AddressOf OnInvoicesFetched, _
                    GetCheckedInvoicesIds(False))

            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try

        Else

            Try
                PrintObject(_FormManager.DataSource, False, 0, "SaskaituRegistras", Me, _
                    _ListViewManager.GetCurrentFilterDescription(), _
                    _ListViewManager.GetDisplayOrderIndexes())
            Catch ex As Exception
                ShowError(ex)
            End Try

        End If

    End Sub

    Private Sub OnInvoicesFetched(ByVal result As Object, ByVal exceptionHandled As Boolean)

        If result Is Nothing Then Exit Sub

        Dim list As BusinessObjectCollection(Of InvoiceMade) = DirectCast(result, BusinessObjectCollection(Of InvoiceMade))

        If _PrintInvoices Then

            Try
                PrintObjectList(list, _PrintVersion)
            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try

        Else

            Try

                Using busy As New StatusBusy

                    For Each item As Documents.InvoiceMade In list.Result

                        Dim mailSubject As String = String.Format("Saskaita - faktura (invoice) {0} Nr. {1}{2}", _
                            item.Date.ToShortDateString, item.Serial, item.FullNumber)
                        Dim fileName As String = String.Format("Invoice_{0}_{1}{2}", _
                            item.Date.ToString("yyyyMMdd"), item.Serial, item.FullNumber)

                        SendObjectToEmail(item, item.Payer.Email, mailSubject, _
                            MyCustomSettings.EmailMessageText, _PrintVersion, fileName, "", Nothing)

                    Next

                End Using
            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try

        End If

    End Sub

    Public Sub OnPrintPreviewClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintPreviewClick
        If _FormManager.DataSource Is Nothing Then Exit Sub
        Try
            PrintObject(_FormManager.DataSource, True, 0, "SaskaituRegistras", Me, _
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


    Private Function GetCheckedInvoicesIds(ByVal throwIfNoClientEmail As Boolean) As Integer()

        If _FormManager.DataSource Is Nothing Then Return Nothing

        If InvoiceInfoItemListDataListView.CheckedObjects Is Nothing Then
            Throw New Exception("Klaida. Nepasirinkta nė viena sąskaita.")
        End If

        Dim result As New List(Of Integer)

        For Each item As InvoiceInfoItem In InvoiceInfoItemListDataListView.CheckedObjects

            If throwIfNoClientEmail AndAlso StringIsNullOrEmpty(item.PersonEmail) Then
                Throw New Exception(String.Format("Klaida. Nenurodytas kliento '{0}' e-paštas.", _
                    item.PersonName))
            End If

            result.Add(item.ID)

        Next

        If result.Count < 1 Then Throw New Exception("Klaida. Nepasirinkta nė viena sąskaita.")

        Return result.ToArray

    End Function

    Private Sub InvoiceInfoItemListBindingSource_DataSourceChanged(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles InvoiceInfoItemListBindingSource.DataSourceChanged
        If _FormManager Is Nothing OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        InvoiceInfoItemListDataListView.Select()
        If CheckAllCheckBox.Checked Then InvoiceInfoItemListDataListView.CheckAll()
    End Sub

    Private Sub CheckAllCheckBox_CheckedChanged(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles CheckAllCheckBox.CheckedChanged
        If _FormManager Is Nothing OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        If CheckAllCheckBox.Checked Then
            InvoiceInfoItemListDataListView.CheckAll()
        Else
            InvoiceInfoItemListDataListView.UncheckAll()
        End If

    End Sub

    Private Function Compress(ByVal source As String) As Byte()

        Using ms As New MemoryStream
            Using gzip As New GZipStream(ms, CompressionMode.Compress, True)
                Dim encoding As New System.Text.UTF8Encoding(False)
                Dim sourceBytes As Byte() = encoding.GetBytes(source)
                gzip.Write(sourceBytes, 0, sourceBytes.Length)
            End Using
            Return ms.ToArray()
        End Using

    End Function

End Class