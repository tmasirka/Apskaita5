Imports ApskaitaObjects.Goods
Public Class F_GoodsOperationDiscard
    Implements IObjectEditForm, ISupportsPrinting

    Private Obj As GoodsOperationDiscard = Nothing
    Private Loading As Boolean = True
    Private OperationID As Integer = 0
    Private GoodsID As Integer = 0
    Private WarehouseID As Integer = 0


    Public ReadOnly Property ObjectID() As Integer Implements IObjectEditForm.ObjectID
        Get
            If Not Obj Is Nothing Then Return Obj.ID
            Return 0
        End Get
    End Property

    Public ReadOnly Property ObjectType() As System.Type Implements IObjectEditForm.ObjectType
        Get
            Return GetType(GoodsOperationDiscard)
        End Get
    End Property


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal nOperationID As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        OperationID = nOperationID

    End Sub

    Public Sub New(ByVal nGoodsID As Integer, ByVal nWarehouseID As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        GoodsID = nGoodsID
        WarehouseID = nWarehouseID

    End Sub


    Private Sub F_GoodsOperationDiscard_Activated(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles Me.Activated

        If Me.WindowState = FormWindowState.Maximized AndAlso MyCustomSettings.AutoSizeForm Then _
            Me.WindowState = FormWindowState.Normal

        If Loading Then
            Loading = False
            Exit Sub
        End If

        If Not PrepareCache(Me, GetType(HelperLists.WarehouseInfoList), _
            GetType(HelperLists.AccountInfoList)) Then Exit Sub

    End Sub

    Private Sub F_GoodsOperationDiscard_FormClosing(ByVal sender As Object, _
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

    End Sub

    Private Sub F_GoodsOperationDiscard_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If Not GoodsOperationDiscard.CanGetObject AndAlso OperationID > 0 Then
            MsgBox("Klaida. Jūsų teisių nepakanka šiai informacijai gauti.", _
                MsgBoxStyle.Exclamation, "Klaida.")
            DisableAllControls(Me)
            Exit Sub
        ElseIf Not GoodsOperationDiscard.CanAddObject AndAlso Not OperationID > 0 Then
            MsgBox("Klaida. Jūsų teisių nepakanka prekių duomenims tvarkyti.", _
                MsgBoxStyle.Exclamation, "Klaida.")
            DisableAllControls(Me)
            Exit Sub
        End If

        If Not SetDataSources() Then Exit Sub

        If OperationID > 0 Then

            Try
                Obj = LoadObject(Of GoodsOperationDiscard)(Nothing, "GetGoodsOperationDiscard", _
                    True, OperationID)
            Catch ex As Exception
                ShowError(ex)
                DisableAllControls(Me)
                Exit Sub
            End Try

        Else

            Try
                Obj = LoadObject(Of GoodsOperationDiscard)(Nothing, "NewGoodsOperationDiscard", _
                    True, GoodsID)
            Catch ex As Exception
                ShowError(ex)
                DisableAllControls(Me)
                Exit Sub
            End Try

        End If

        GoodsOperationDiscardBindingSource.DataSource = Obj

        ConfigureLimitationButton(Obj)

        SetFormLayout(Me)

        ConfigureButtons()

        If Not Obj Is Nothing AndAlso Not Obj.IsNew AndAlso Not GoodsOperationDiscard.CanEditObject Then
            MsgBox("Klaida. Jūsų teisių nepakanka prekių duomenims redaguoti.", _
                MsgBoxStyle.Exclamation, "Klaida.")
            DisableAllControls(Me)
            Exit Sub
        ElseIf Not Obj Is Nothing AndAlso Obj.ComplexOperationID > 0 Then
            MsgBox("Klaida. Ši operacija yra sudedamoji kompleksinės operacijos dalis. " _
                & "Ją redaguoti galima tik per atitinkamą kompleksinį dokumentą.", _
                MsgBoxStyle.Exclamation, "Klaida.")
            DisableAllControls(Me)
            Exit Sub
        End If

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


    Private Sub RefreshCostsInfoButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles RefreshCostsInfoButton.Click

        If Obj Is Nothing Then Exit Sub

        If Obj.Warehouse Is Nothing OrElse Not Obj.Warehouse.ID > 0 Then
            MsgBox("Klaida. Nepasirinktas sandėlis.", MsgBoxStyle.Exclamation, "Klaida")
        ElseIf Obj.GoodsInfo.AccountingMethod = GoodsAccountingMethod.Periodic Then
            MsgBox("Klaida. Taikant periodinį prekių apskaitos metodą operacijos " _
                & "savikaina neskaičiuojama.", MsgBoxStyle.Exclamation, "Klaida")
        End If

        Try
            Dim param As GoodsCostParam = GoodsCostParam.GetGoodsCostParam( _
                Obj.GoodsInfo.ID, Obj.Warehouse.ID, Obj.Ammount, Obj.ID, 0, _
                Obj.GoodsInfo.ValuationMethod, Obj.Date)
            Dim costItem As GoodsCostItem = LoadObject(Of GoodsCostItem)(Nothing, _
                "GetGoodsCostItem", True, param, True)
            Obj.ReloadCostInfo(costItem)
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub

    Private Sub LimitationsButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles LimitationsButton.Click

        If Obj Is Nothing Then Exit Sub

        MsgBox(Obj.OperationLimitations.LimitsExplanation & vbCrLf & vbCrLf & _
            Obj.OperationLimitations.GetBackgroundDescription, MsgBoxStyle.Information, "")

    End Sub

    Private Sub ViewJournalEntryButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ViewJournalEntryButton.Click
        If Obj Is Nothing OrElse Not Obj.JournalEntryID > 0 Then Exit Sub
        MDIParent1.LaunchForm(GetType(F_GeneralLedgerEntry), False, False, _
            Obj.JournalEntryID, Obj.JournalEntryID)
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
            MsgBox("Formoje yra klaidų:" & vbCrLf & Obj.BrokenRulesCollection.ToString( _
                Csla.Validation.RuleSeverity.Error), MsgBoxStyle.Exclamation, "Klaida.")
            Return False
        End If

        Dim Question, Answer As String
        If Obj.BrokenRulesCollection.WarningCount > 0 Then
            Question = "DĖMESIO. Duomenyse gali būti klaidų: " & vbCrLf _
                & Obj.BrokenRulesCollection.ToString(Csla.Validation.RuleSeverity.Warning) & vbCrLf
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

        Using bm As New BindingsManager(GoodsOperationDiscardBindingSource, Nothing, Nothing, True, False)

            Try
                Obj = LoadObject(Of GoodsOperationDiscard)(Obj, "Save", False)
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
        Using bm As New BindingsManager(GoodsOperationDiscardBindingSource, Nothing, Nothing, True, True)
        End Using
    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, GetType(HelperLists.WarehouseInfoList), _
            GetType(HelperLists.AccountInfoList)) Then Return False

        Try

            CachedListsLoaders.LoadWarehouseInfoListToGridCombo(WarehouseAccGridComboBox, False)
            CachedListsLoaders.LoadAccountInfoListToGridCombo(AccountGoodsDiscardCostsAccGridComboBox, True, 6)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Return False
        End Try

        Return True

    End Function

    Private Sub ConfigureButtons()

        If Obj Is Nothing Then Exit Sub

        WarehouseAccGridComboBox.Enabled = Not Obj Is Nothing AndAlso Not Obj.WarehouseIsReadOnly
        AccountGoodsDiscardCostsAccGridComboBox.Enabled = Not Obj Is Nothing AndAlso _
            Not Obj.AccountGoodsDiscardCostsIsReadOnly
        AmmountAccTextBox.ReadOnly = Obj Is Nothing OrElse Obj.AmmountIsReadOnly
        DateDateTimePicker.Enabled = Not Obj Is Nothing AndAlso Not Obj.DateIsReadOnly
        DocumentNumberTextBox.ReadOnly = Obj Is Nothing OrElse Obj.DocumentNumberIsReadOnly
        DescriptionTextBox.ReadOnly = Obj Is Nothing OrElse Obj.DescriptionIsReadOnly

        nCancelButton.Enabled = Not Obj Is Nothing AndAlso Not Obj.IsNew

        EditedBaner.Visible = Not Obj Is Nothing AndAlso Not Obj.IsNew

    End Sub

    Private Sub ConfigureLimitationButton(ByVal asset As GoodsOperationDiscard)

        If Not asset.OperationLimitations.FinancialDataCanChange OrElse _
            asset.OperationLimitations.MaxDateApplicable OrElse _
            asset.OperationLimitations.MinDateApplicable Then
            LimitationsButton.Visible = True
            LimitationsButton.Image = My.Resources.Action_lock_icon_32p
        Else
            LimitationsButton.Visible = False
        End If

    End Sub

End Class