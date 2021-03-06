Imports ApskaitaObjects.ActiveReports
Imports AccControlsWinForms
Imports AccDataBindingsWinForms.Printing
Imports AccDataBindingsWinForms.CachedInfoLists

Friend Class F_LongTermAssetOperationInfoListParent
    Implements ISupportsPrinting

    Private _FormManager As CslaActionExtenderReportForm(Of LongTermAssetOperationInfoListParent)
    Private _ListViewManager As DataListViewEditControlManager(Of LongTermAssetOperationInfo)
    Private _QueryManager As CslaActionExtenderQueryObject

    Private _LongTermAssetID As Integer = 0
    Private _OperationListChanged As Boolean = False


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.


    End Sub

    Public Sub New(ByVal longTermAssetID As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _LongTermAssetID = longTermAssetID

    End Sub


    Public Sub NotifyAboutOperationListChanges(ByVal assetID As Integer)
        If Not _FormManager.DataSource Is Nothing AndAlso assetID = _FormManager.DataSource.ID Then
            _OperationListChanged = True
        End If
    End Sub


    Private Sub F_LongTermAssetOperationInfoListParent_Activated(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles Me.Activated
        If _OperationListChanged Then
            _OperationListChanged = False
            If Not YesOrNo("Pasikeitė operacijų su šiuo turtu duomenys. Atnaujinti sąrašą?") Then Exit Sub
            RefreshButton.PerformClick()
        End If
    End Sub

    Private Sub F_LongTermAssetOperationInfoListParent_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If Not SetDataSources() Then Exit Sub

    End Sub

    Private Sub OnDataSourceFetched(ByVal result As Object, ByVal exceptionHandled As Boolean)

        If exceptionHandled Then
            DisableAllControls(Me)
            Exit Sub
        ElseIf result Is Nothing Then
            MsgBox("Klaida. Dėl nežinomų priežasčių nepavyko gauti ilgalaikio turto sąrašo.", _
                MsgBoxStyle.Exclamation, "Klaida")
            DisableAllControls(Me)
            Exit Sub
        End If

        Dim list As LongTermAssetInfoList

        Try

            list = DirectCast(result, LongTermAssetInfoList)
            Dim nestedList As New LongTermAssetInfoListControl
            nestedList.DataSource = list
            nestedList.AcceptSingleClick = True
            AssetInfoListAccListComboBox.AddDataListView(nestedList)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Exit Sub
        End Try

        If _LongTermAssetID > 0 Then
            For Each item As LongTermAssetInfo In list
                If item.ID = _LongTermAssetID Then
                    AssetInfoListAccListComboBox.SelectedValue = item
                    RefreshButton.PerformClick()
                    Exit Sub
                End If
            Next
        End If

    End Sub

    Private Function SetDataSources() As Boolean

        Try

            _ListViewManager = New DataListViewEditControlManager(Of LongTermAssetOperationInfo) _
                (OperationListDataListView, ContextMenuStrip1, Nothing, _
                Nothing, Nothing, Nothing)

            _ListViewManager.AddCancelButton = True
            _ListViewManager.AddButtonHandler("Keisti", "Keisti operacijos duomenis.", _
                AddressOf ChangeItem)
            _ListViewManager.AddButtonHandler("Ištrinti", "Pašalinti operacijos duomenis iš duomenų bazės.", _
                AddressOf DeleteItem)


            _ListViewManager.AddMenuItemHandler(ChangeItem_MenuItem, AddressOf ChangeItem)
            _ListViewManager.AddMenuItemHandler(DeleteItem_MenuItem, AddressOf DeleteItem)

            NewOperation_MenuItem.DropDownItems.Clear()
            For Each t As TypeItem In AssetOperationManager.GetSimpleOperationTypes()
                Dim menuItem As New ToolStripMenuItem(t.Name)
                menuItem.Tag = t.Type
                AddHandler menuItem.Click, AddressOf NewOperationMenuItem_Click
                NewOperation_MenuItem.DropDownItems.Add(menuItem)
            Next

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

            ' LongTermAssetOperationInfoListParent.GetLongTermAssetOperationInfoListParent(_LongTermAssetID)
            _FormManager = New CslaActionExtenderReportForm(Of LongTermAssetOperationInfoListParent) _
                (Me, LongTermAssetOperationInfoListParentBindingSource, Nothing, Nothing, RefreshButton, _
                 ProgressFiller1, "GetLongTermAssetOperationInfoListParent", AddressOf GetReportParams)

            _FormManager.ManageDataListViewStates(OperationListDataListView)

            'LongTermAssetInfoList.GetLongTermAssetInfoList(Today, Today.AddYears(50), Nothing)
            _QueryManager.InvokeQuery(Of LongTermAssetInfoList)(Nothing, _
                "GetLongTermAssetInfoList", True, AddressOf OnDataSourceFetched, Today, Today.AddYears(50), Nothing)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Return False
        End Try

        Return True

    End Function


    Private Function GetReportParams() As Object()

        Dim currentAssetID As Integer = 0
        Try
            currentAssetID = DirectCast(AssetInfoListAccListComboBox.SelectedValue, LongTermAssetInfo).ID
        Catch ex As Exception
        End Try

        If Not currentAssetID > 0 Then
            MsgBox("Klaida. Nepasirinktas ilgalaikis turtas.", MsgBoxStyle.Exclamation, "Klaida")
        End If

        'LongTermAssetOperationInfoListParent.GetLongTermAssetOperationInfoListParent(currentAssetID)
        Return New Object() {currentAssetID}

    End Function


    Private Sub ChangeItem(ByVal item As LongTermAssetOperationInfo)
        If item Is Nothing Then Exit Sub
        'AssetOperationManager.GetAssetOperation(item)
        _QueryManager.InvokeQuery(Of AssetOperationManager)(Nothing, "GetAssetOperation", True, _
            AddressOf OpenObjectEditForm, item)
    End Sub

    Private Sub DeleteItem(ByVal item As LongTermAssetOperationInfo)

        If item Is Nothing Then Exit Sub

        If item.ComplexActID > 0 Then
            If AssetOperationManager.CheckIfAssetOperationEditFormOpen(item.ComplexActID, _
                item.Type, True, True, True) Then Exit Sub
        Else
            If AssetOperationManager.CheckIfAssetOperationEditFormOpen(item.ID, _
                item.Type, False, True, True) Then Exit Sub
        End If

        If item.ComplexActID > 0 Then
            If Not YesOrNo("DĖMESIO. Pasirinkta operacija yra kompleksinė, sudaryta iš keleto paprastų operacijų, įskaitant bet neapsiribojant pasirinkta. Ištrynus šią operaciją kartu bus ištrinta ir kitos jai priklausančios operacijos. Ar tikrai norite pašalinti pasirinktos operacijos duomenis iš duomenų bazės?") Then Exit Sub
        Else
            If Not YesOrNo("Ar tikrai norite pašalinti pasirinktos operacijos duomenis iš duomenų bazės?") Then Exit Sub
        End If

        'AssetOperationManager.DeleteAssetOperation(item)
        _QueryManager.InvokeQuery(Of AssetOperationManager)(Nothing, "DeleteAssetOperation", False, _
            AddressOf OnOperationDeleted, item)

    End Sub

    Private Sub OnOperationDeleted(ByVal result As Object, ByVal exceptionHandled As Boolean)

        If exceptionHandled Then Exit Sub

        If Not YesOrNo("Operacijos duomenys sėkmingai pašalinti iš duomenų bazės." _
            & vbCrLf & "Atnaujinti ataskaitą?") Then Exit Sub

        RefreshButton.PerformClick()

    End Sub

    Private Sub NewOperationMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If _FormManager.DataSource Is Nothing OrElse Not _FormManager.DataSource.ID > 0 Then Exit Sub

        Dim operationType As Type = Nothing
        Try
            operationType = DirectCast(DirectCast(sender, ToolStripMenuItem).Tag, Type)
        Catch ex As Exception
        End Try

        If operationType Is Nothing Then Exit Sub

        AssetOperationManager.StartNewAssetOperation(_FormManager.DataSource.ID, operationType)

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
            PrintObject(_FormManager.DataSource, False, 0, "ITOperacijuSuvestine", Me, _
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
            PrintObject(_FormManager.DataSource, True, 0, "ITOperacijuSuvestine", Me, _
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

End Class