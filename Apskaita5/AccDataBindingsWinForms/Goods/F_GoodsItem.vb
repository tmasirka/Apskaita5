Imports ApskaitaObjects.Goods
Imports ApskaitaObjects.HelperLists
Imports ApskaitaObjects.Settings
Imports AccControlsWinForms
Imports AccDataBindingsWinForms.CachedInfoLists
Imports ApskaitaObjects.Documents

Friend Class F_GoodsItem
    Implements IObjectEditForm

    Private ReadOnly _RequiredCachedLists As Type() = New Type() _
        {GetType(WarehouseInfoList), GetType(GoodsGroupInfoList), GetType(AccountInfoList), _
        GetType(CompanyRegionalInfoList), GetType(TaxRateInfoList)}

    Private WithEvents _FormManager As CslaActionExtenderEditForm(Of GoodsItem)
    Private _PricesListViewManager As DataListViewEditControlManager(Of RegionalPrice)
    Private _ContentsListViewManager As DataListViewEditControlManager(Of RegionalContent)

    Private _DocumentToEdit As GoodsItem = Nothing


    Public ReadOnly Property ObjectID() As Integer Implements IObjectEditForm.ObjectID
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

    Public ReadOnly Property ObjectType() As System.Type Implements IObjectEditForm.ObjectType
        Get
            Return GetType(GoodsItem)
        End Get
    End Property


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal documentToEdit As GoodsItem)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _DocumentToEdit = documentToEdit

    End Sub


    Private Sub F_GoodsItem_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If _DocumentToEdit Is Nothing Then
            _DocumentToEdit = GoodsItem.NewGoodsItem()
        End If

        If Not SetDataSources() Then Exit Sub

        Try


            _FormManager = New CslaActionExtenderEditForm(Of GoodsItem)(Me, GoodsItemBindingSource, _
                _DocumentToEdit, _RequiredCachedLists, nOkButton, ApplyButton, nCancelButton, _
                Nothing, ProgressFiller1)

            _FormManager.ManageDataListViewStates(RegionalPricesDataListView, RegionalContentsDataListView)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Exit Sub
        End Try

        AccountValueReductionAccGridComboBox.Enabled = Not _FormManager.DataSource.PriceCutsExist
        AccountPurchasesAccGridComboBox.Enabled = (_FormManager.DataSource.OldAccountingMethod <> _
            GoodsAccountingMethod.Periodic OrElse Not _FormManager.DataSource.IsInUse)
        AccountSalesNetCostsAccGridComboBox.Enabled = (_FormManager.DataSource.OldAccountingMethod <> _
            GoodsAccountingMethod.Periodic OrElse Not _FormManager.DataSource.IsInUse)
        AccountDiscountsAccGridComboBox.Enabled = (_FormManager.DataSource.OldAccountingMethod <> _
            GoodsAccountingMethod.Periodic OrElse Not _FormManager.DataSource.IsInUse)
        AccountingMethodHumanReadableComboBox.Enabled = Not _FormManager.DataSource.IsInUse
        DefaultValuationMethodHumanReadableComboBox.Enabled = Not _FormManager.DataSource.IsInUse
        EditedBaner.Visible = Not _FormManager.DataSource.IsNew

    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, _RequiredCachedLists) Then Return False

        Try

            _ContentsListViewManager = New DataListViewEditControlManager(Of RegionalContent) _
                (RegionalContentsDataListView, Nothing, AddressOf OnContentItemsDelete, _
                 AddressOf OnContentItemAdd, Nothing, _DocumentToEdit)

            _PricesListViewManager = New DataListViewEditControlManager(Of RegionalPrice) _
                (RegionalPricesDataListView, Nothing, AddressOf OnPriceItemsDelete, _
                 AddressOf OnPriceItemAdd, Nothing, _DocumentToEdit)

            SetupDefaultControls(Of GoodsItem)(Me, GoodsItemBindingSource, _DocumentToEdit)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Return False
        End Try

        Return True

    End Function


    Private Sub OnContentItemsDelete(ByVal items As RegionalContent())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As RegionalContent In items
            _FormManager.DataSource.RegionalContents.Remove(item)
        Next
    End Sub

    Private Sub OnContentItemAdd()
        If _FormManager.DataSource Is Nothing Then Exit Sub
        _FormManager.DataSource.RegionalContents.AddNew()
    End Sub

    Private Sub OnPriceItemsDelete(ByVal items As RegionalPrice())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As RegionalPrice In items
            _FormManager.DataSource.RegionalPrices.Remove(item)
        Next
    End Sub

    Private Sub OnPriceItemAdd()
        If _FormManager.DataSource Is Nothing Then Exit Sub
        _FormManager.DataSource.RegionalPrices.AddNew()
    End Sub

End Class