Imports ApskaitaObjects.Workers
Imports ApskaitaObjects.HelperLists
Public Class F_WorkTimeSheet
    Implements ISupportsPrinting, IObjectEditForm

    Private Obj As WorkTimeSheet
    Private Loading As Boolean = True
    Private SheetID As Integer = -1

    Public ReadOnly Property ObjectID() As Integer Implements IObjectEditForm.ObjectID
        Get
            If Not Obj Is Nothing AndAlso Not Obj.IsNew Then Return Obj.ID
            Return 0
        End Get
    End Property

    Public ReadOnly Property ObjectType() As System.Type Implements IObjectEditForm.ObjectType
        Get
            Return GetType(WorkTimeSheet)
        End Get
    End Property


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal nSheetID As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        SheetID = nSheetID

    End Sub


    Private Sub F_WorkTimeSheet_Activated(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles Me.Activated

        If Me.WindowState = FormWindowState.Maximized AndAlso MyCustomSettings.AutoSizeForm Then _
            Me.WindowState = FormWindowState.Normal

        If Loading Then
            Loading = False
            Exit Sub
        End If

        If Not PrepareCache(Me, GetType(HelperLists.WorkTimeClassInfoList)) Then Exit Sub

    End Sub

    Private Sub F_WorkTimeSheet_FormClosing(ByVal sender As Object, _
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

        GetFormLayout(Me)

    End Sub

    Private Sub F_WorkTimeSheet_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Workers.WorkTimeSheet.CanGetObject Then
            MsgBox("Klaida. Jūsų teisių nepakanka šiai informacijai gauti.", _
                MsgBoxStyle.Exclamation, "Klaida.")
            DisableAllControls(Me)
            Exit Sub
        End If

        If Not SetDataSources() Then Exit Sub

        NewSheetYearNumericUpDown.Value = Today.Year
        NewSheetMonthNumericUpDown.Value = Today.Month

        If SheetID > 0 Then

            Try
                Obj = LoadObject(Of WorkTimeSheet)(Nothing, "GetWorkTimeSheet", False, SheetID)
            Catch ex As Exception
                ShowError(ex)
                DisableAllControls(Me)
                Exit Sub
            End Try

            WorkTimeSheetBindingSource.DataSource = Obj

        End If

        ConfigureButtons()

        SetFormLayout(Me)

        Dim h As New EditableDataGridViewHelper(Me.GeneralItemListDataGridView)
        Dim g As New EditableDataGridViewHelper(Me.SpecialItemListDataGridView)

    End Sub


    Private Sub OkButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles IOkButton.Click
        If Not SaveObj() Then Exit Sub
        Me.Hide()
        Me.Close()
    End Sub

    Private Sub ApplyButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles IApplyButton.Click
        If SaveObj() Then ConfigureButtons()
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ICancelButton.Click
        CancelObj()
    End Sub


    Private Sub RefreshButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles RefreshButton.Click

        If Not Obj Is Nothing AndAlso TypeOf Obj Is IIsDirtyEnough AndAlso _
            DirectCast(Obj, IIsDirtyEnough).IsDirtyEnough Then
            Dim answ As String = Ask("Išsaugoti duomenis?", New ButtonStructure("Taip"), _
                New ButtonStructure("Ne"), New ButtonStructure("Atšaukti"))
            If answ <> "Taip" AndAlso answ <> "Ne" Then Exit Sub
            If answ = "Taip" Then
                If Not SaveObj() Then Exit Sub
            End If
        End If

        Dim RestInfo As WorkTimeClassInfo = Nothing
        Try
            RestInfo = DirectCast(NewSheetRestTimeInfoAccGridComboBox.SelectedValue, WorkTimeClassInfo)
        Catch ex As Exception
        End Try
        Dim PublicHolidaysInfo As WorkTimeClassInfo = Nothing
        Try
            PublicHolidaysInfo = DirectCast(NewSheetPublicHolidaysInfoAccGridComboBox.SelectedValue, WorkTimeClassInfo)
        Catch ex As Exception
        End Try

        Using bm As New BindingsManager(WorkTimeSheetBindingSource, _
            GeneralItemListBindingSource, SpecialItemListBindingSource, False, True)

            Try
                Obj = LoadObject(Of WorkTimeSheet)(Nothing, "NewWorkTimeSheet", True, _
                    Convert.ToInt32(NewSheetYearNumericUpDown.Value), _
                    Convert.ToInt32(NewSheetMonthNumericUpDown.Value), RestInfo, PublicHolidaysInfo)
            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try

            bm.SetNewDataSource(Obj)

        End Using

        ConfigureButtons()

    End Sub

    Private Sub AddNewSpecialWorkTimeButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles AddNewSpecialWorkTimeButton.Click

        If Obj Is Nothing Then Exit Sub

        Dim RestInfo As WorkTimeClassInfo = Nothing
        Try
            RestInfo = DirectCast(NewSpecialWorkTimeTypeAccGridComboBox.SelectedValue, WorkTimeClassInfo)
        Catch ex As Exception
        End Try

        Dim baseItem As WorkTimeItem = Nothing
        Try
            baseItem = DirectCast(GeneralItemListDataGridView.Rows( _
                GeneralItemListDataGridView.SelectedCells(0).RowIndex).DataBoundItem, WorkTimeItem)
        Catch ex As Exception
        End Try

        Try
            Obj.SpecialItemList.AddNewSpecialWorkTimeItem(baseItem, RestInfo)
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

        If Obj Is Nothing Then Exit Sub

        Using frm As New F_SendObjToEmail(Obj, 1)
            frm.ShowDialog()
        End Using

    End Sub

    Public Sub OnPrintClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintClick

        If Obj Is Nothing Then Exit Sub

        Try
            PrintObject(Obj, False, 1)
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub

    Public Sub OnPrintPreviewClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintPreviewClick

        If Obj Is Nothing Then Exit Sub

        Try
            PrintObject(Obj, True, 1)
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub

    Public Function SupportsEmailing() As Boolean _
        Implements ISupportsPrinting.SupportsEmailing
        Return True
    End Function


    Private Function SaveObj() As Boolean

        If Not Obj.IsDirty Then Return True

        If Not Obj.IsValid Then
            MsgBox("Formoje yra klaidų:" & vbCrLf & Obj.GetAllBrokenRules, MsgBoxStyle.Exclamation, "Klaida.")
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

        Using bm As New BindingsManager(WorkTimeSheetBindingSource, _
            GeneralItemListBindingSource, SpecialItemListBindingSource, True, False)

            Try
                Obj = LoadObject(Of WorkTimeSheet)(Obj, "Save", False)
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
        If Obj Is Nothing OrElse Obj.IsNew OrElse Not Obj.IsDirty Then Exit Sub
        Using bm As New BindingsManager(WorkTimeSheetBindingSource, _
            GeneralItemListBindingSource, SpecialItemListBindingSource, True, True)
        End Using
    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, GetType(HelperLists.WorkTimeClassInfoList)) Then Return False

        TotalWorkTimeAccTextBox.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS

        DataGridViewTextBoxColumn10.AllowNegative = False
        DataGridViewTextBoxColumn10.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn12.AllowNegative = False
        DataGridViewTextBoxColumn12.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn14.AllowNegative = False
        DataGridViewTextBoxColumn14.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn16.AllowNegative = False
        DataGridViewTextBoxColumn16.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn18.AllowNegative = False
        DataGridViewTextBoxColumn18.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn20.AllowNegative = False
        DataGridViewTextBoxColumn20.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn22.AllowNegative = False
        DataGridViewTextBoxColumn22.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn24.AllowNegative = False
        DataGridViewTextBoxColumn24.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn26.AllowNegative = False
        DataGridViewTextBoxColumn26.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn28.AllowNegative = False
        DataGridViewTextBoxColumn28.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn30.AllowNegative = False
        DataGridViewTextBoxColumn30.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn32.AllowNegative = False
        DataGridViewTextBoxColumn32.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn34.AllowNegative = False
        DataGridViewTextBoxColumn34.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn36.AllowNegative = False
        DataGridViewTextBoxColumn36.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn38.AllowNegative = False
        DataGridViewTextBoxColumn38.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn40.AllowNegative = False
        DataGridViewTextBoxColumn40.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn42.AllowNegative = False
        DataGridViewTextBoxColumn42.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn44.AllowNegative = False
        DataGridViewTextBoxColumn44.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn46.AllowNegative = False
        DataGridViewTextBoxColumn46.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn48.AllowNegative = False
        DataGridViewTextBoxColumn48.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn50.AllowNegative = False
        DataGridViewTextBoxColumn50.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn52.AllowNegative = False
        DataGridViewTextBoxColumn52.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn54.AllowNegative = False
        DataGridViewTextBoxColumn54.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn56.AllowNegative = False
        DataGridViewTextBoxColumn56.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn58.AllowNegative = False
        DataGridViewTextBoxColumn58.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn60.AllowNegative = False
        DataGridViewTextBoxColumn60.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn62.AllowNegative = False
        DataGridViewTextBoxColumn62.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn64.AllowNegative = False
        DataGridViewTextBoxColumn64.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn66.AllowNegative = False
        DataGridViewTextBoxColumn66.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn68.AllowNegative = False
        DataGridViewTextBoxColumn68.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn70.AllowNegative = False
        DataGridViewTextBoxColumn70.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS

        DataGridViewTextBoxColumn83.AllowNegative = False
        DataGridViewTextBoxColumn83.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn84.AllowNegative = False
        DataGridViewTextBoxColumn84.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn85.AllowNegative = False
        DataGridViewTextBoxColumn85.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn86.AllowNegative = False
        DataGridViewTextBoxColumn86.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn87.AllowNegative = False
        DataGridViewTextBoxColumn87.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn88.AllowNegative = False
        DataGridViewTextBoxColumn88.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn89.AllowNegative = False
        DataGridViewTextBoxColumn89.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn90.AllowNegative = False
        DataGridViewTextBoxColumn90.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn91.AllowNegative = False
        DataGridViewTextBoxColumn91.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn92.AllowNegative = False
        DataGridViewTextBoxColumn92.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn93.AllowNegative = False
        DataGridViewTextBoxColumn93.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn94.AllowNegative = False
        DataGridViewTextBoxColumn94.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn95.AllowNegative = False
        DataGridViewTextBoxColumn95.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn96.AllowNegative = False
        DataGridViewTextBoxColumn96.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn97.AllowNegative = False
        DataGridViewTextBoxColumn97.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn98.AllowNegative = False
        DataGridViewTextBoxColumn98.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn99.AllowNegative = False
        DataGridViewTextBoxColumn99.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn100.AllowNegative = False
        DataGridViewTextBoxColumn100.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn101.AllowNegative = False
        DataGridViewTextBoxColumn101.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn102.AllowNegative = False
        DataGridViewTextBoxColumn102.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn103.AllowNegative = False
        DataGridViewTextBoxColumn103.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn104.AllowNegative = False
        DataGridViewTextBoxColumn104.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn105.AllowNegative = False
        DataGridViewTextBoxColumn105.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn106.AllowNegative = False
        DataGridViewTextBoxColumn106.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn107.AllowNegative = False
        DataGridViewTextBoxColumn107.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn108.AllowNegative = False
        DataGridViewTextBoxColumn108.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn109.AllowNegative = False
        DataGridViewTextBoxColumn109.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn110.AllowNegative = False
        DataGridViewTextBoxColumn110.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn111.AllowNegative = False
        DataGridViewTextBoxColumn111.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn112.AllowNegative = False
        DataGridViewTextBoxColumn112.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS
        DataGridViewTextBoxColumn113.AllowNegative = False
        DataGridViewTextBoxColumn113.DecimalLength = ApskaitaObjects.ROUNDWORKHOURS

        Try

            LoadWorkTimeClassInfoListToGridCombo(NewSheetRestTimeInfoAccGridComboBox, False, True, False)
            LoadWorkTimeClassInfoListToGridCombo(NewSheetPublicHolidaysInfoAccGridComboBox, _
                False, True, False)

            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn11, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn13, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn15, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn17, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn19, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn21, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn23, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn25, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn27, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn29, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn31, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn33, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn35, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn37, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn39, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn41, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn43, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn45, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn47, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn49, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn51, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn53, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn55, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn57, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn59, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn61, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn63, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn65, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn67, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn69, True, True, False)
            LoadWorkTimeClassInfoListToGridCombo(DataGridViewTextBoxColumn71, True, True, False)

            LoadWorkTimeClassInfoListToGridCombo(NewSpecialWorkTimeTypeAccGridComboBox, False, False, True)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Return False
        End Try

        Try
            Dim list As WorkTimeClassInfoList = WorkTimeClassInfoList.GetList
            NewSheetRestTimeInfoAccGridComboBox.SelectedValue = list.GetItemByCode("P")
            NewSheetPublicHolidaysInfoAccGridComboBox.SelectedValue = list.GetItemByCode("S")
        Catch ex As Exception
        End Try

        Return True

    End Function

    Private Sub ConfigureButtons()

        NumberTextBox.ReadOnly = (Obj Is Nothing)
        SubDivisionTextBox.ReadOnly = (Obj Is Nothing)
        PreparedByPositionTextBox.ReadOnly = (Obj Is Nothing)
        PreparedByNameTextBox.ReadOnly = (Obj Is Nothing)
        SignedByPositionTextBox.ReadOnly = (Obj Is Nothing)
        SignedByNameTextBox.ReadOnly = (Obj Is Nothing)
        DateDateTimePicker.Enabled = (Not Obj Is Nothing)
        NewSpecialWorkTimeTypeAccGridComboBox.Enabled = (Not Obj Is Nothing)
        AddNewSpecialWorkTimeButton.Enabled = (Not Obj Is Nothing)
        IOkButton.Enabled = (Not Obj Is Nothing)
        IApplyButton.Enabled = (Not Obj Is Nothing)
        ICancelButton.Enabled = (Not Obj Is Nothing AndAlso Not Obj.IsNew)
        RefreshButton.Enabled = (Obj Is Nothing OrElse Obj.IsNew)
        NewSheetYearNumericUpDown.Enabled = (Obj Is Nothing OrElse Obj.IsNew)
        NewSheetMonthNumericUpDown.Enabled = (Obj Is Nothing OrElse Obj.IsNew)

    End Sub

End Class