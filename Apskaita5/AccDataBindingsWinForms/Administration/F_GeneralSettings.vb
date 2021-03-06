Imports ApskaitaObjects.Settings
Imports AccControlsWinForms

Friend Class F_GeneralSettings
    Implements ISingleInstanceForm

    Private WithEvents _FormManager As CslaActionExtenderEditForm(Of ApskaitaObjects.Settings.CommonSettings)
    Private _TaxRateListViewManager As DataListViewEditControlManager(Of TaxRate)
    Private _NameListViewManager As DataListViewEditControlManager(Of Name)
    Private _CodeListViewManager As DataListViewEditControlManager(Of Code)
    Private _DefaultWorkTimeListViewManager As DataListViewEditControlManager(Of DefaultWorkTime)
    Private _PublicHolidayListViewManager As DataListViewEditControlManager(Of PublicHoliday)
    Private _QueryManager As CslaActionExtenderQueryObject


    Private Sub F_GeneralSettings_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

            'ApskaitaObjects.Settings.CommonSettings.GetCommonSettings()
            _QueryManager.InvokeQuery(Of ApskaitaObjects.Settings.CommonSettings)(Nothing, _
                "GetCommonSettings", True, AddressOf OnDataSourceLoaded)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Exit Sub
        End Try

    End Sub

    Private Sub OnDataSourceLoaded(ByVal source As Object, ByVal exceptionHandled As Boolean)

        If exceptionHandled Then

            DisableAllControls(Me)
            Exit Sub

        ElseIf source Is Nothing Then

            ShowError(New Exception("Klaida. Nepavyko gauti bendrų nustatymų duomenų."))
            DisableAllControls(Me)
            Exit Sub

        End If

        Dim result As ApskaitaObjects.Settings.CommonSettings _
            = DirectCast(source, ApskaitaObjects.Settings.CommonSettings)

        Try

            _TaxRateListViewManager = New DataListViewEditControlManager(Of TaxRate) _
                (TaxRatesDataListView, Nothing, AddressOf OnTaxItemsDelete, _
                 AddressOf OnTaxItemAdd, Nothing, result)

            _NameListViewManager = New DataListViewEditControlManager(Of Name) _
                (NamesDataListView, Nothing, AddressOf OnNameItemsDelete, _
                 AddressOf OnNameItemAdd, Nothing, result)

            _CodeListViewManager = New DataListViewEditControlManager(Of Code) _
                (CodesDataListView, Nothing, AddressOf OnCodeItemsDelete, _
                 AddressOf OnCodeItemAdd, Nothing, result)

            _DefaultWorkTimeListViewManager = New DataListViewEditControlManager(Of DefaultWorkTime) _
                (DefaultWorkTimesDataListView, Nothing, AddressOf OnWorkTimeItemsDelete, _
                 AddressOf OnWorkTimeItemAdd, Nothing, result)

            _PublicHolidayListViewManager = New DataListViewEditControlManager(Of PublicHoliday) _
                (PublicHolidaysDataListView, Nothing, AddressOf OnPublicHolidayItemsDelete, _
                 AddressOf OnPublicHolidayItemAdd, Nothing, result)

            _FormManager = New CslaActionExtenderEditForm(Of ApskaitaObjects.Settings.CommonSettings) _
                (Me, CommonSettingsBindingSource, result, Nothing, nOkButton, _
                 ApplyButton, nCancelButton, Nothing, ProgressFiller1)

            _FormManager.ManageDataListViewStates(TaxRatesDataListView, CodesDataListView, _
                NamesDataListView, PublicHolidaysDataListView, DefaultWorkTimesDataListView)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Exit Sub
        End Try

        nOkButton.Enabled = ApskaitaObjects.Settings.CommonSettings.CanEditObject
        ApplyButton.Enabled = ApskaitaObjects.Settings.CommonSettings.CanEditObject

    End Sub


    Private Sub OnTaxItemsDelete(ByVal items As TaxRate())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As TaxRate In items
            _FormManager.DataSource.TaxRates.Remove(item)
        Next
    End Sub

    Private Sub OnTaxItemAdd()
        _FormManager.DataSource.TaxRates.AddNew()
    End Sub

    Private Sub OnNameItemsDelete(ByVal items As Name())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As Name In items
            _FormManager.DataSource.Names.Remove(item)
        Next
    End Sub

    Private Sub OnNameItemAdd()
        _FormManager.DataSource.Names.AddNew()
    End Sub

    Private Sub OnCodeItemsDelete(ByVal items As Code())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As Code In items
            _FormManager.DataSource.Codes.Remove(item)
        Next
    End Sub

    Private Sub OnCodeItemAdd()
        _FormManager.DataSource.Codes.AddNew()
    End Sub

    Private Sub OnWorkTimeItemsDelete(ByVal items As DefaultWorkTime())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As DefaultWorkTime In items
            _FormManager.DataSource.DefaultWorkTimes.Remove(item)
        Next
    End Sub

    Private Sub OnWorkTimeItemAdd()
        _FormManager.DataSource.DefaultWorkTimes.AddNew()
    End Sub

    Private Sub OnPublicHolidayItemsDelete(ByVal items As PublicHoliday())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As PublicHoliday In items
            _FormManager.DataSource.PublicHolidays.Remove(item)
        Next
    End Sub

    Private Sub OnPublicHolidayItemAdd()
        _FormManager.DataSource.PublicHolidays.AddNew()
    End Sub

End Class