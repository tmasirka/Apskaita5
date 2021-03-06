Imports ApskaitaObjects.Documents
Imports ApskaitaObjects.HelperLists
Imports ApskaitaObjects.Documents.InvoiceAdapters
Imports AccControlsWinForms
Imports AccDataBindingsWinForms.CachedInfoLists
Imports AccControlsWinForms.WebControls

Friend Class F_InvoiceReceived
    Implements IObjectEditForm, ISupportsChronologicValidator

    Private ReadOnly _RequiredCachedLists As Type() = New Type() _
        {GetType(TaxRateInfoList), GetType(PersonInfoList), GetType(AccountInfoList), _
        GetType(RegionalInfoDictionary)}

    Private WithEvents _FormManager As CslaActionExtenderEditForm(Of InvoiceReceived)
    Private _ListViewManager As DataListViewEditControlManager(Of InvoiceReceivedItem)
    Private _QueryManager As CslaActionExtenderQueryObject

    Private _DocumentToEdit As InvoiceReceived = Nothing


    Public ReadOnly Property ObjectID() As Integer _
        Implements IObjectEditForm.ObjectID
        Get
            If _FormManager Is Nothing OrElse _FormManager.DataSource Is Nothing Then
                If _DocumentToEdit Is Nothing OrElse _DocumentToEdit.IsNew Then
                    Return Integer.MinValue
                Else
                    Return _DocumentToEdit.ID
                End If
            End If
            Return _FormManager.DataSource.ID
        End Get
    End Property

    Public ReadOnly Property ObjectType() As System.Type _
        Implements IObjectEditForm.ObjectType
        Get
            Return GetType(InvoiceReceived)
        End Get
    End Property


    Public Sub New(ByVal documentToEdit As InvoiceReceived)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        _DocumentToEdit = documentToEdit

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()


    End Sub


    Private Sub F_InvoiceReceived_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If Not SetDataSources() Then Exit Sub

        If _DocumentToEdit Is Nothing Then
            _DocumentToEdit = InvoiceReceived.NewInvoiceReceived()
        End If

        Try

            _FormManager = New CslaActionExtenderEditForm(Of InvoiceReceived) _
                (Me, InvoiceReceivedBindingSource, _DocumentToEdit, _
                _RequiredCachedLists, IOkButton, IApplyButton, ICancelButton, _
                Nothing, ProgressFiller1)

            _FormManager.AddCustomValidation(AddressOf NumberUniqueValidation)

            _FormManager.AddNewDataSourceButton(NewButton, "NewInvoiceReceived")

            _FormManager.ManageDataListViewStates(InvoiceItemsDataListView)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Exit Sub
        End Try

        ConfigureButtons()
        ActualDateSmartDateTimePicker.Enabled = _DocumentToEdit.ActualDateIsApplicable

    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, _RequiredCachedLists) Then Return False

        Try

            _ListViewManager = New DataListViewEditControlManager(Of InvoiceReceivedItem) _
                (InvoiceItemsDataListView, Nothing, AddressOf OnItemsDelete, _
                 AddressOf OnItemAdd, Nothing, _DocumentToEdit)

            _ListViewManager.AddCancelButton = False
            _ListViewManager.AddButtonHandler("Susietas Objektas", "Redaguoti susietą objektą.", _
                AddressOf OnItemClicked)

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

            SetupDefaultControls(Of InvoiceReceived)(Me, _
                InvoiceReceivedBindingSource, _DocumentToEdit)

            Dim adapters As List(Of TypeItem) = GetInvoiceAdapterTypes()
            NewAdapterTypeComboBox.DataSource = adapters

            Dim defaultAdapterType As Type = GetType(ServiceInvoiceAdapter)
            If MyCustomSettings.DefaultInvoiceReceivedItemIsGoods Then
                defaultAdapterType = GetType(GoodsAcquisitionInvoiceAdapter)
            End If
            For Each t As TypeItem In adapters
                If t.Type Is defaultAdapterType Then
                    NewAdapterTypeComboBox.SelectedItem = t
                    Exit For
                End If
            Next

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Return False
        End Try

        Return True

    End Function


    Private Sub OnItemsDelete(ByVal items As InvoiceReceivedItem())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        If Not _FormManager.DataSource.ChronologyValidator.FinancialDataCanChange Then
            MsgBox(String.Format("Klaida. Keisti dokumento finansinių duomenų negalima, įskaitant eilučių pridėjimą ar ištrynimą:{0}{1}", vbCrLf, _
                _FormManager.DataSource.ChronologyValidator.FinancialDataCanChangeExplanation), _
                MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        End If
        For Each item As InvoiceReceivedItem In items
            If Not item.AttachedObjectValue Is Nothing AndAlso Not item.AttachedObjectValue. _
            ChronologyValidator.FinancialDataCanChange Then
                MsgBox(String.Format("Klaida. Eilutės pašalinti neleidžiama:{0}{1}", vbCrLf, _
                    item.AttachedObjectValue.ChronologyValidator.FinancialDataCanChangeExplanation), _
                    MsgBoxStyle.Exclamation, "Klaida")
                Exit Sub
            End If
        Next
        For Each item As InvoiceReceivedItem In items
            _FormManager.DataSource.InvoiceItemsSorted.Remove(item)
        Next
    End Sub

    Private Sub OnItemAdd()
        If _FormManager.DataSource Is Nothing Then Exit Sub
        If Not _FormManager.DataSource.ChronologyValidator.FinancialDataCanChange Then
            MsgBox(String.Format("Klaida. Keisti dokumento finansinių duomenų negalima, įskaitant eilučių pridėjimą ar ištrynimą:{0}{1}", vbCrLf, _
                _FormManager.DataSource.ChronologyValidator.FinancialDataCanChangeExplanation), _
                MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        End If
        _FormManager.DataSource.InvoiceItemsSorted.AddNew()
    End Sub

    Private Sub OnItemClicked(ByVal item As InvoiceReceivedItem)

        If item Is Nothing OrElse item.AttachedObjectValue Is Nothing _
            OrElse item.AttachedObjectValue.Type = InvoiceAdapterType.Service Then Exit Sub

        OpenObjectEditForm(item.AttachedObjectValue.ValueObject)

    End Sub

    Private Sub GetCurrencyRatesButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles GetCurrencyRatesButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        If Not YesOrNo("Gauti valiutos kursą?") Then Exit Sub

        Dim factory As CurrencyRateFactoryBase = Nothing
        Dim result As CurrencyRate = Nothing
        Try
            factory = WebControls.GetCurrencyRateFactory(GetCurrentCompany.BaseCurrency)
            result = WebControls.GetCurrencyRateWithProgress(_FormManager.DataSource.CurrencyCode.Trim.ToUpper, _
                _FormManager.DataSource.Date, factory)
        Catch ex As Exception
            ShowError(ex)
            Exit Sub
        End Try

        If Not result Is Nothing Then
            _FormManager.DataSource.CurrencyRate = result.Rate
        End If

    End Sub

    Private Sub AddAttachedObjectButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles AddAttachedObjectButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        Dim selectedType As TypeItem = Nothing
        Try
            selectedType = DirectCast(NewAdapterTypeComboBox.SelectedItem, TypeItem)
        Catch ex As Exception
        End Try
        If selectedType Is Nothing Then Exit Sub

        Dim adapter As IInvoiceAdapter = GetNewInvoiceAdapter(selectedType.Type, True, _
            _FormManager.DataSource.ChronologyValidator.BaseValidator)
        If adapter Is Nothing Then Exit Sub

        Dim item As InvoiceReceivedItem = Nothing

        Dim regionalDictionary As HelperLists.RegionalInfoDictionary
        Try
            regionalDictionary = HelperLists.RegionalInfoDictionary.GetList()
        Catch ex As Exception
            ShowError(ex)
            Exit Sub
        End Try

        Try

            Using busy As New StatusBusy
                item = _FormManager.DataSource.AttachNewObject(adapter, regionalDictionary)
            End Using

        Catch ex As Exception
            ShowError(ex)
            Exit Sub
        End Try

        If item.AttachedObjectValue.Type <> InvoiceAdapterType.Service Then
            OpenObjectEditForm(item.AttachedObjectValue.ValueObject)
        End If

    End Sub

    Private Sub CopyInvoiceButton_Click(ByVal sender As System.Object, _
       ByVal e As System.EventArgs) Handles CopyInvoiceButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        Dim info As InvoiceInfo.InvoiceInfo = Nothing
        Try
            Using busy As New StatusBusy
                info = _FormManager.DataSource.GetInvoiceInfo(InstanceGuid.ToString)
            End Using
        Catch ex As Exception
            ShowError(New Exception(String.Format("Klaida. Nepavyko generuoti InvoiceInfo objekto:{0}{1}", _
                vbCrLf, ex.Message), ex))
            Exit Sub
        End Try

        If info Is Nothing Then
            MsgBox("Klaida. Dėl nežinomų priežasčių nepavyko generuoti InvoiceInfo objekto.", _
                MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        End If

        Try
            Using busy As New StatusBusy
                System.Windows.Forms.Clipboard.SetText(InvoiceInfo.Factory. _
                    ToXmlString(Of InvoiceInfo.InvoiceInfo)(info), TextDataFormat.UnicodeText)
            End Using
        Catch ex As Exception
            ShowError(New Exception(String.Format("Klaida. Nepavyko serializuoti InvoiceInfo objekto:{0}{1}", _
                vbCrLf, ex.Message), ex))
            Exit Sub
        End Try

        MsgBox("Sąskaita sėkmingai nukopijuota į ClipBoard'ą.", MsgBoxStyle.Information, "Info")

    End Sub

    Private Sub PasteInvoiceButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles PasteInvoiceButton.Click

        Dim clipboardText As String = System.Windows.Forms.Clipboard.GetText(TextDataFormat.UnicodeText)

        If StringIsNullOrEmpty(clipboardText) Then

            MsgBox("Klaida. ClipBoard'as tuščias, t.y. nebuvo nukopijuota jokia sąskaita.", _
                MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub

        End If

        Dim info As InvoiceInfo.InvoiceInfo = Nothing
        Try
            Using busy As New StatusBusy
                info = InvoiceInfo.Factory.FromXmlString(Of InvoiceInfo.InvoiceInfo)(clipboardText)
            End Using
        Catch ex As Exception
            ShowError(New Exception(String.Format("Klaida. Nepavyko atkurti sąskaitos objekto. " _
                & "Teigtina, kad prieš tai į ClipBoard'ą buvo nukopijuota ne sąskaita, " _
                & "o šiaip kažkoks tekstas.{0}Klaidos tekstas:{1}", vbCrLf, ex.Message), ex))
            Exit Sub
        End Try

        If info Is Nothing Then
            MsgBox("Klaida. Dėl nežinomų priežasčių nepavyko atkurti sąskaitos objekto.", _
                MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        End If

        Dim useExternalID As Boolean = False

        If InstanceGuid.ToString.Trim <> info.SystemGuid.Trim Then

            Dim answer As String

            If info.ExternalID Is Nothing OrElse String.IsNullOrEmpty(info.ExternalID.Trim) Then
                If Not MyCustomSettings.AlwaysUseExternalIdInvoicesMade Then
                    answer = Ask("Sąskaita yra kopijuojama iš išorinės sistemos. " _
                        & "Ką priskirti išoriniam ID?", New ButtonStructure("Nieko", _
                        "Nepriskirti jokio išorinio ID."), New ButtonStructure( _
                        "ID", "Sąskaitos ID laikyti išoriniu ID."), New ButtonStructure( _
                        "Atšaukti", "Atšaukti kopijavimą."))
                Else
                    answer = "ID"
                End If
            Else
                answer = Ask("Sąskaita yra kopijuojama iš išorinės sistemos. " _
                    & "Ką priskirti išoriniam ID?", New ButtonStructure("Nieko", _
                    "Nepriskirti jokio išorinio ID."), New ButtonStructure( _
                    "ID", "Sąskaitos ID laikyti išoriniu ID."), New ButtonStructure( _
                    "Išorinį ID", "Sąskaitos išorinį ID laikyti išoriniu ID."), _
                    New ButtonStructure("Atšaukti", "Atšaukti kopijavimą."))
            End If

            If answer = "Nieko" Then
                info.SystemGuid = InstanceGuid.ToString.Trim
            ElseIf answer = "Atšaukti" Then
                Exit Sub
            ElseIf answer = "Išorinį ID" Then
                useExternalID = True
            End If

        End If

        Dim newObj As InvoiceReceived = Nothing
        Dim newPerson As InvoiceInfo.ClientInfo = Nothing
        Try
            Using busy As New StatusBusy
                Dim personList As HelperLists.PersonInfoList = HelperLists.PersonInfoList.GetList
                newObj = InvoiceReceived.NewInvoiceReceived(info, InstanceGuid.ToString, _
                    useExternalID, personList, newPerson)
            End Using
        Catch ex As Exception
            ShowError(New Exception(String.Format("Klaida. Nepavyko įkrauti kopijuojamos sąskaitos duomenų:{0}{1}", _
                vbCrLf, ex.Message), ex))
            Exit Sub
        End Try

        If newObj Is Nothing Then
            MsgBox("Klaida. Dėl nežinomų priežasčių nepavyko įkrauti kopijuojamos sąskaitos duomenų.", _
                MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        End If

        If Not StringIsNullOrEmpty(newObj.ExternalID) Then

            Dim existingInvoiceID As Integer = 0

            Try
                Using busy As New StatusBusy
                    existingInvoiceID = CommandDocumentIdByExternalId.TheCommand(Of InvoiceReceived)( _
                        newObj.ExternalID)
                End Using
            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try

            If existingInvoiceID > 0 Then
                If YesOrNo(String.Format("DĖMESIO. Sąskaita su tokiu išoriniu ID jau egzistuoja. " _
                    & "Įvesti sąskaitą iš naujo nebus leidžiama.{0}{1}Atidaryti egzistuojančią sąskaitą?", _
                    vbCrLf, vbCrLf)) Then
                    ' InvoiceReceived.GetInvoiceReceived(existingInvoiceID)
                    _QueryManager.InvokeQuery(Of InvoiceReceived)(Nothing, "GetInvoiceReceived", True, _
                        AddressOf OpenObjectEditForm, existingInvoiceID)
                End If
                Exit Sub
            End If

        End If

        _FormManager.AddNewDataSource(newObj)

        If Not newPerson Is Nothing Then
            OpenObjectEditForm(newPerson)
        End If

    End Sub

    Private Sub ViewJournalEntryButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ViewJournalEntryButton.Click
        If _FormManager.DataSource Is Nothing OrElse _FormManager.DataSource.IsNew Then Exit Sub
        OpenJournalEntryEditForm(_QueryManager, _FormManager.DataSource.ID)
    End Sub

    Private Sub ActualDateIsApplicableCheckBox_CheckedChanged(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ActualDateIsApplicableCheckBox.CheckedChanged
        ActualDateSmartDateTimePicker.Enabled = ActualDateIsApplicableCheckBox.Checked
    End Sub

    Private Sub CalculateIndirectVatButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles CalculateIndirectVatButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        Try
            _FormManager.DataSource.CalculateIndirectVat()
        Catch ex As Exception
            ShowError(ex)
        End Try

    End Sub

    Private Function NumberUniqueValidation() As Boolean

        If _FormManager.DataSource Is Nothing OrElse Not _FormManager.DataSource.IsNew _
            OrElse Not MyCustomSettings.CheckInvoiceReceivedNumber Then Return True

        Dim uniqueInfo As InvoiceReceivedNumberUnique = Nothing

        Try

            Using busy As New StatusBusy
                uniqueInfo = InvoiceReceivedNumberUnique.GetInvoiceReceivedNumberUnique( _
                    _FormManager.DataSource, MyCustomSettings.CheckInvoiceReceivedNumberWithDate, _
                    MyCustomSettings.CheckInvoiceReceivedNumberWithSupplier)
            End Using

        Catch ex As Exception
            ShowError(ex)
            Return False
        End Try

        If Not uniqueInfo.IsUnique Then
            Return YesOrNo(String.Format("DĖMESIO. Duomenyse gali būti klaidų:{0}{1}{2}Ar tikrai norite išsaugoti naują sąskaitą?", _
                vbCrLf, uniqueInfo.ToString, vbCrLf))
        End If

        Return True

    End Function


    Public Function ChronologicContent() As String _
        Implements ISupportsChronologicValidator.ChronologicContent
        If _FormManager.DataSource Is Nothing Then Return ""
        Return _FormManager.DataSource.ChronologyValidator.LimitsExplanation
    End Function

    Public Function HasChronologicContent() As Boolean _
        Implements ISupportsChronologicValidator.HasChronologicContent

        Return Not _FormManager.DataSource Is Nothing AndAlso _
            Not StringIsNullOrEmpty(_FormManager.DataSource.ChronologyValidator.LimitsExplanation)

    End Function


    Private Sub _FormManager_DataSourceStateHasChanged(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles _FormManager.DataSourceStateHasChanged
        ConfigureButtons()
    End Sub

    Private Sub ConfigureButtons()

        If _FormManager.DataSource Is Nothing Then Exit Sub

        CurrencyCodeComboBox.Enabled = Not _FormManager.DataSource.CurrencyCodeIsReadOnly
        GetCurrencyRatesButton.Enabled = Not _FormManager.DataSource.CurrencyRateIsReadOnly
        CurrencyRateAccTextBox.ReadOnly = _FormManager.DataSource.CurrencyRateIsReadOnly

        AccountSupplierAccGridComboBox.Enabled = Not _FormManager.DataSource.AccountSupplierIsReadOnly
        AddAttachedObjectButton.Enabled = _FormManager.DataSource.ChronologyValidator.BaseValidator.FinancialDataCanChange
        IndirectVatAccountAccGridComboBox.Enabled = Not _FormManager.DataSource.IndirectVatAccountIsReadOnly
        IndirectVatCostsAccountAccGridComboBox.Enabled = Not _FormManager.DataSource.IndirectVatCostsAccountIsReadOnly
        IndirectVatSumAccTextBox.ReadOnly = _FormManager.DataSource.IndirectVatSumIsReadOnly

        ICancelButton.Enabled = Not _FormManager.DataSource.IsNew

    End Sub

End Class