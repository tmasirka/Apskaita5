Imports ApskaitaObjects.HelperLists
Imports ApskaitaObjects.Workers
Imports AccControlsWinForms
Imports ApskaitaObjects.Settings
Imports AccDataBindingsWinForms.CachedInfoLists

Friend Class F_PayOutNaturalPerson
    Implements IObjectEditForm

    Private ReadOnly _RequiredCachedLists As Type() = New Type() _
        {GetType(TaxRateInfoList), GetType(CodeInfoList)}

    Private WithEvents _FormManager As CslaActionExtenderEditForm(Of PayOutNaturalPerson)
    Private _QueryManager As CslaActionExtenderQueryObject
    Private _PayOutToEdit As PayOutNaturalPerson = Nothing


    Public ReadOnly Property ObjectID() As Integer _
        Implements IObjectEditForm.ObjectID
        Get
            If _FormManager Is Nothing OrElse _FormManager.DataSource Is Nothing Then
                If _PayOutToEdit Is Nothing OrElse _PayOutToEdit.IsNew Then
                    Return Integer.MinValue
                Else
                    Return _PayOutToEdit.ID
                End If
            ElseIf _FormManager.DataSource.IsNew Then
                Return Integer.MinValue
            End If
            Return _FormManager.DataSource.ID
        End Get
    End Property

    Public ReadOnly Property ObjectType() As System.Type _
        Implements IObjectEditForm.ObjectType
        Get
            Return GetType(PayOutNaturalPerson)
        End Get
    End Property


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal payOutToEdit As PayOutNaturalPerson)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        _PayOutToEdit = payOutToEdit

    End Sub


    Private Sub F_PayOutNaturalPerson_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If Not SetDataSources() Then Exit Sub

        Try

            If _PayOutToEdit Is Nothing Then
                _PayOutToEdit = PayOutNaturalPerson.NewPayOutNaturalPerson()
            End If

            _FormManager = New CslaActionExtenderEditForm(Of PayOutNaturalPerson)(Me, _
                PayOutNaturalPersonBindingSource, _PayOutToEdit, _RequiredCachedLists, _
                IOkButton, IApplyButton, ICancelButton, Nothing, ProgressFiller1)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Exit Sub
        End Try

        IOkButton.Enabled = ((_PayOutToEdit.IsNew AndAlso PayOutNaturalPerson.CanAddObject) _
            OrElse (Not _PayOutToEdit.IsNew AndAlso PayOutNaturalPerson.CanEditObject))
        IApplyButton.Enabled = ((_PayOutToEdit.IsNew AndAlso PayOutNaturalPerson.CanAddObject) _
            OrElse (Not _PayOutToEdit.IsNew AndAlso PayOutNaturalPerson.CanEditObject))
        LoadAssociatedJournalEntryButton.Enabled = ((_PayOutToEdit.IsNew AndAlso PayOutNaturalPerson.CanAddObject) _
            OrElse (Not _PayOutToEdit.IsNew AndAlso PayOutNaturalPerson.CanEditObject))

    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, _RequiredCachedLists) Then Return False

        Try

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

            SetupDefaultControls(Of PayOutNaturalPerson)(Me, _
                PayOutNaturalPersonBindingSource, _PayOutToEdit)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Return False
        End Try

        Return True

    End Function


    Private Sub LoadAssociatedJournalEntry_Click(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles LoadAssociatedJournalEntryButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        Using dlg As New F_JournalEntryInfoList(_FormManager.DataSource.Date.AddMonths(-1), _
            _FormManager.DataSource.Date, True)

            If dlg.ShowDialog() <> DialogResult.OK OrElse dlg.SelectedEntries Is Nothing _
                OrElse dlg.SelectedEntries.Count < 1 Then Exit Sub

            Try
                _FormManager.DataSource.LoadAssociatedJournalEntry(dlg.SelectedEntries(0))
            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try

        End Using

    End Sub

    Private Sub ViewJournalEntryButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ViewJournalEntryButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        If _FormManager.DataSource.JournalEntryID > 0 Then

            OpenJournalEntryEditForm(_QueryManager, _FormManager.DataSource.JournalEntryID)

        Else

            OpenObjectEditForm(_FormManager.DataSource.GetNewJournalEntry())

        End If

    End Sub

End Class