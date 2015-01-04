Imports ApskaitaObjects.Goods
Imports ApskaitaObjects.HelperLists
Public Class F_GoodsComplexOperationTransferOfBalance
    Implements IObjectEditForm, ISupportsPrinting

    Private Obj As GoodsComplexOperationTransferOfBalance = Nothing
    Private Loading As Boolean = True


    Public ReadOnly Property ObjectID() As Integer Implements IObjectEditForm.ObjectID
        Get
            If Not Obj Is Nothing Then Return Obj.ID
            Return 0
        End Get
    End Property

    Public ReadOnly Property ObjectType() As System.Type Implements IObjectEditForm.ObjectType
        Get
            Return GetType(GoodsComplexOperationTransferOfBalance)
        End Get
    End Property


    Private Sub F_GoodsComplexOperationTransferOfBalance_Activated(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles Me.Activated

        If Me.WindowState = FormWindowState.Maximized AndAlso MyCustomSettings.AutoSizeForm Then _
            Me.WindowState = FormWindowState.Normal

        If Loading Then
            Loading = False
            Exit Sub
        End If

        If Not PrepareCache(Me, GetType(HelperLists.GoodsInfoList), _
            GetType(HelperLists.WarehouseInfoList)) Then Exit Sub

    End Sub

    Private Sub F_GoodsComplexOperationTransferOfBalance_FormClosing(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Not Obj Is Nothing AndAlso TypeOf Obj Is IIsDirtyEnough AndAlso _
            DirectCast(Obj, IIsDirtyEnough).IsDirtyEnough Then
            Dim answ As String = Ask("Išsaugoti duomenis?", New ButtonStructure("Taip"), _
                New ButtonStructure("Ne"), New ButtonStructure("Atšaukti"))
            If answ <> "Taip" AndAlso answ <> "Ne" Then
                e.Cancel = True
                Exit Sub
            End If
            If answ = "Taip" Then
                If Not SaveObj() Then
                    e.Cancel = True
                    Exit Sub
                End If
            End If
        End If

        If Not Obj Is Nothing AndAlso Obj.IsDirty Then CancelObj()

        GetFormLayout(Me)
        GetDataGridViewLayOut(ItemsDataGridView)

    End Sub

    Private Sub F_GoodsComplexOperationTransferOfBalance_Load(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles Me.Load

        If Not GoodsComplexOperationTransferOfBalance.CanGetObject Then
            MsgBox("Klaida. Jūsų teisių nepakanka šiai informacijai gauti.", _
                MsgBoxStyle.Exclamation, "Klaida.")
            DisableAllControls(Me)
            Exit Sub
        End If

        If Not SetDataSources() Then Exit Sub

        Try
            Obj = LoadObject(Of GoodsComplexOperationTransferOfBalance)(Nothing, _
                "GetGoodsComplexOperationTransferOfBalance", True)
        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Exit Sub
        End Try

        GoodsComplexOperationTransferOfBalanceBindingSource.DataSource = Obj

        AddDGVColumnSelector(ItemsDataGridView)

        SetDataGridViewLayOut(ItemsDataGridView)
        SetFormLayout(Me)

        If (Not GoodsComplexOperationPriceCut.CanAddObject AndAlso Obj.IsNew) OrElse _
            (Not GoodsComplexOperationPriceCut.CanEditObject AndAlso Not Obj.IsNew) Then
            MsgBox("Klaida. Jūsų teisių nepakanka prekių likučių perkėlimo duomenims tvarkyti.", _
                MsgBoxStyle.Exclamation, "Klaida.")
            DisableAllControls(Me)
            Exit Sub
        End If

        ConfigureLimitationButton()
        ConfigureButtons()

        Dim CM2 As New ContextMenu()
        Dim CMItem2 As New MenuItem("Informacija apie formatą", AddressOf PasteAccButton_Click)
        CM2.MenuItems.Add(CMItem2)
        PasteAccButton.ContextMenu = CM2

    End Sub


    Private Sub OkButton_Click(ByVal sender As System.Object, _
       ByVal e As System.EventArgs) Handles nOkButton.Click
        If Obj Is Nothing Then Exit Sub
        If SaveObj() Then Me.Close()
    End Sub

    Private Sub ApplyButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ApplyButton.Click
        If Obj Is Nothing Then Exit Sub
        If SaveObj() Then ConfigureButtons()
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles nCancelButton.Click
        If Obj Is Nothing Then Exit Sub
        CancelObj()
    End Sub


    Private Sub AddNewItemButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles AddNewItemButton.Click

        Dim newGoodsInfo As HelperLists.GoodsInfo = Nothing
        Try
            newGoodsInfo = DirectCast(GoodsInfoListAccGridComboBox.SelectedValue, HelperLists.GoodsInfo)
        Catch ex As Exception
        End Try

        Dim newWarehouseInfo As HelperLists.WarehouseInfo = Nothing
        Try
            newWarehouseInfo = DirectCast(WarehouseInfoListAccGridComboBox.SelectedValue, HelperLists.WarehouseInfo)
        Catch ex As Exception
        End Try

        If newGoodsInfo Is Nothing OrElse Not newGoodsInfo.ID > 0 Then
            MsgBox("Klaida. Nepasirinkta prekė.", MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        ElseIf newWarehouseInfo Is Nothing OrElse Not newWarehouseInfo.ID > 0 Then
            MsgBox("Klaida. Nepasirinktas sandėlys.", MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        ElseIf Obj.Items.ContainsGood(newGoodsInfo.ID, newWarehouseInfo.ID) Then
            MsgBox("Klaida. Tokia prekė pasirinktame sandėlyje jau yra įtraukta.", _
                MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        End If

        Try
            Dim newItem As GoodsTransferOfBalanceItem = LoadObject(Of GoodsTransferOfBalanceItem) _
                (Nothing, "NewGoodsTransferOfBalanceItem", True, newGoodsInfo.ID, newWarehouseInfo)
            Obj.AddNewGoodsItem(newItem)
            ConfigureLimitationButton()
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub

    Private Sub ItemsDataGridView_CellBeginEdit(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles ItemsDataGridView.CellBeginEdit

        If Obj Is Nothing OrElse e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Exit Sub

        Dim currentItem As GoodsTransferOfBalanceItem = Nothing
        Try
            currentItem = DirectCast(ItemsDataGridView.Rows(e.RowIndex).DataBoundItem, GoodsTransferOfBalanceItem)
        Catch ex As Exception
        End Try
        If currentItem Is Nothing Then
            MsgBox("Klaida.Nepavyko nustatyti pasirinktos eilutės.", MsgBoxStyle.Exclamation, "Klaida")
            e.Cancel = True
            Exit Sub
        End If

        If (ItemsDataGridView.Columns(e.ColumnIndex) Is DataGridViewTextBoxColumn16 AndAlso _
            currentItem.AmmountIsReadOnly) OrElse _
            (ItemsDataGridView.Columns(e.ColumnIndex) Is DataGridViewTextBoxColumn17 AndAlso _
            currentItem.AmountInWarehouseIsReadOnly) OrElse _
            (ItemsDataGridView.Columns(e.ColumnIndex) Is DataGridViewTextBoxColumn18 AndAlso _
            currentItem.UnitCostIsReadOnly) OrElse _
            (ItemsDataGridView.Columns(e.ColumnIndex) Is DataGridViewTextBoxColumn19 AndAlso _
            currentItem.TotalValueIsReadOnly) OrElse _
            (ItemsDataGridView.Columns(e.ColumnIndex) Is DataGridViewTextBoxColumn20 AndAlso _
            currentItem.TotalValueInWarehouseIsReadOnly) OrElse _
            (ItemsDataGridView.Columns(e.ColumnIndex) Is DataGridViewTextBoxColumn21 AndAlso _
            currentItem.SalesNetCostsIsReadOnly) OrElse _
            (ItemsDataGridView.Columns(e.ColumnIndex) Is DataGridViewTextBoxColumn22 AndAlso _
            currentItem.DiscountsIsReadOnly) OrElse _
            (ItemsDataGridView.Columns(e.ColumnIndex) Is DataGridViewTextBoxColumn23 AndAlso _
            currentItem.PriceCutIsReadOnly) Then
            e.Cancel = True
        End If

    End Sub

    Private Sub ItemsDataGridView_UserDeletingRow(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles ItemsDataGridView.UserDeletingRow

        If Obj Is Nothing Then Exit Sub

        Dim currentItem As GoodsTransferOfBalanceItem = Nothing
        Try
            currentItem = DirectCast(e.Row.DataBoundItem, GoodsTransferOfBalanceItem)
        Catch ex As Exception
        End Try
        If currentItem Is Nothing Then
            MsgBox("Klaida.Nepavyko nustatyti pasirinktos eilutės.", MsgBoxStyle.Exclamation, "Klaida")
            e.Cancel = True
            Exit Sub
        End If

        If Not currentItem.OperationLimitations.FinancialDataCanChange Then
            MsgBox("Klaida. Eilutės pašalinti neleidžiama:" & vbCrLf _
                & currentItem.OperationLimitations.FinancialDataCanChangeExplanation, _
                MsgBoxStyle.Exclamation, "Klaida")
            e.Cancel = True
        End If

    End Sub

    Private Sub ViewJournalEntryButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ViewJournalEntryButton.Click
        If Obj Is Nothing OrElse Not Obj.JournalEntryID > 0 Then Exit Sub
        MDIParent1.LaunchForm(GetType(F_TransferOfBalance), True, False, Obj.JournalEntryID)
    End Sub

    Private Sub PasteAccButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles PasteAccButton.Click

        If GetSenderText(sender).Trim.ToLower.Contains("informacija") Then
            MsgBox(Goods.GoodsTransferOfBalanceItem.ColumnSequence, MsgBoxStyle.Information, "Info")
            Exit Sub
        End If

        If Not Obj Is Nothing AndAlso TypeOf Obj Is IIsDirtyEnough AndAlso _
            DirectCast(Obj, IIsDirtyEnough).IsDirtyEnough Then
            Dim answ As String = Ask("Išsaugoti duomenis?", New ButtonStructure("Taip"), _
                New ButtonStructure("Ne"), New ButtonStructure("Atšaukti"))
            If answ <> "Taip" AndAlso answ <> "Ne" Then
                Exit Sub
            End If
            If answ = "Taip" Then
                If Not SaveObj() Then
                    Exit Sub
                End If
            End If
        End If

        Using bm As New BindingsManager(GoodsComplexOperationTransferOfBalanceBindingSource, _
            ItemsBindingSource, Nothing, True, True)

            Try
                Obj = LoadObject(Of GoodsComplexOperationTransferOfBalance)(Nothing, _
                    "GetGoodsComplexOperationTransferOfBalance", True, Clipboard.GetText)
            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try

            bm.SetNewDataSource(Obj)

        End Using

        MsgBox("Likučių perkėlimas sėkmingai papildytas iš paste'o duomenų.", MsgBoxStyle.Information, "Info")

        If (Not GoodsComplexOperationPriceCut.CanAddObject AndAlso Obj.IsNew) OrElse _
            (Not GoodsComplexOperationPriceCut.CanEditObject AndAlso Not Obj.IsNew) Then
            MsgBox("Klaida. Jūsų teisių nepakanka prekių likučių perkėlimo duomenims tvarkyti.", _
                MsgBoxStyle.Exclamation, "Klaida.")
            DisableAllControls(Me)
            Exit Sub
        End If

        ConfigureLimitationButton()
        ConfigureButtons()

    End Sub

    Private Sub LimitationsButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles LimitationsButton.Click

        If Obj Is Nothing Then Exit Sub

        MsgBox(Obj.OperationalLimit.LimitsExplanation & vbCrLf & vbCrLf & _
            Obj.OperationalLimit.BackgroundExplanation, MsgBoxStyle.Information, "")

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
        If Obj Is Nothing Then Exit Sub

        Using frm As New F_SendObjToEmail(Obj, 0)
            frm.ShowDialog()
        End Using

    End Sub

    Public Sub OnPrintClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintClick
        If Obj Is Nothing Then Exit Sub
        Try
            PrintObject(Obj, False, 0)
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Public Sub OnPrintPreviewClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintPreviewClick
        If Obj Is Nothing Then Exit Sub
        Try
            PrintObject(Obj, True, 0)
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Public Function SupportsEmailing() As Boolean _
        Implements ISupportsPrinting.SupportsEmailing
        Return True
    End Function


    Private Function SaveObj() As Boolean

        If Obj Is Nothing OrElse Not Obj.IsDirty Then Return True

        If Not Obj.IsValid Then
            MsgBox("Formoje yra klaidų:" & vbCrLf & Obj.GetAllBrokenRules, _
                MsgBoxStyle.Exclamation, "Klaida.")
            Return False
        End If

        Dim Question, Answer As String
        If Not String.IsNullOrEmpty(Obj.GetAllWarnings.Trim) Then
            Question = "DĖMESIO. Duomenyse gali būti klaidų: " & vbCrLf & Obj.GetAllWarnings & vbCrLf
        Else
            Question = ""
        End If
        If Obj.IsNew Then
            Question = Question & "Ar tikrai norite įtraukti naujus duomenis?"
            Answer = "Nauji duomenys sėkmingai įtraukti."
        Else
            Question = Question & "Ar tikrai norite pakeisti duomenis?"
            Answer = "Duomenys sėkmingai pakeisti."
        End If

        If Not YesOrNo(Question) Then Return False

        Using bm As New BindingsManager(GoodsComplexOperationTransferOfBalanceBindingSource, _
            ItemsBindingSource, Nothing, True, False)

            Try
                Obj = LoadObject(Of GoodsComplexOperationTransferOfBalance)(Obj, "Save", False)
            Catch ex As Exception
                ShowError(ex)
                Return False
            End Try

            bm.SetNewDataSource(Obj)

        End Using

        MsgBox(Answer, MsgBoxStyle.Information, "Info")

        Return True

    End Function

    Private Sub CancelObj()
        If Obj Is Nothing OrElse Obj.IsNew Then Exit Sub
        Using bm As New BindingsManager(GoodsComplexOperationTransferOfBalanceBindingSource, _
            ItemsBindingSource, Nothing, True, True)
        End Using
    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, GetType(HelperLists.GoodsInfoList), _
            GetType(HelperLists.WarehouseInfoList)) Then Return False

        Try

            LoadGoodsInfoListToGridCombo(GoodsInfoListAccGridComboBox, False, Documents.TradedItemType.All)
            LoadWarehouseInfoListToGridCombo(WarehouseInfoListAccGridComboBox, False)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Return False
        End Try

        Return True

    End Function

    Private Sub ConfigureButtons()

        If Obj Is Nothing Then Exit Sub

        AddNewItemButton.Enabled = Not Obj Is Nothing
        DataGridViewTextBoxColumn16.ReadOnly = Obj Is Nothing OrElse Not Obj.OperationalLimit.FinancialDataCanChange

        nCancelButton.Enabled = Not Obj Is Nothing AndAlso Not Obj.IsNew
        nOkButton.Enabled = Not Obj Is Nothing
        ApplyButton.Enabled = Not Obj Is Nothing

        EditedBaner.Visible = Not Obj Is Nothing AndAlso Not Obj.IsNew

    End Sub

    Private Sub ConfigureLimitationButton()
        LimitationsButton.Visible = Not Obj Is Nothing AndAlso _
            Not String.IsNullOrEmpty(Obj.OperationalLimit.LimitsExplanation.Trim)
    End Sub

End Class