Imports ApskaitaObjects.Goods
Imports AccControlsWinForms
Imports AccDataBindingsWinForms.CachedInfoLists

Friend Class F_GoodsOperationValuationMethod
    Implements IObjectEditForm, ISupportsChronologicValidator

    Private WithEvents _FormManager As CslaActionExtenderEditForm(Of GoodsOperationValuationMethod)

    Private _DocumentToEdit As GoodsOperationValuationMethod = Nothing
    Private _GoodsID As Integer = 0


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
            Return GetType(GoodsOperationValuationMethod)
        End Get
    End Property


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal goodsID As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _GoodsID = goodsID

    End Sub

    Public Sub New(ByVal documentToEdit As GoodsOperationValuationMethod)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _DocumentToEdit = documentToEdit

    End Sub


    Private Sub F_GoodsOperationValuationMethod_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If _DocumentToEdit Is Nothing Then
            Using frm As New F_NewGoodsOperation(Of GoodsOperationValuationMethod)(_GoodsID)
                frm.ShowDialog()
                _DocumentToEdit = frm.Result
            End Using
        End If

        If _DocumentToEdit Is Nothing Then
            Me.BeginInvoke(New MethodInvoker(AddressOf Me.Close))
            Exit Sub
        End If

        Try

            SetupDefaultControls(Of GoodsOperationValuationMethod)(Me, _
                GoodsOperationValuationMethodBindingSource, _DocumentToEdit)

            _FormManager = New CslaActionExtenderEditForm(Of GoodsOperationValuationMethod) _
                (Me, GoodsOperationValuationMethodBindingSource, _DocumentToEdit, _
                Nothing, nOkButton, ApplyButton, nCancelButton, _
                Nothing, ProgressFiller1)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Exit Sub
        End Try

        ConfigureButtons()

    End Sub


    Public Function ChronologicContent() As String _
            Implements ISupportsChronologicValidator.ChronologicContent
        If _FormManager.DataSource Is Nothing Then Return ""
        Return _FormManager.DataSource.OperationLimitations.LimitsExplanation
    End Function

    Public Function HasChronologicContent() As Boolean _
        Implements ISupportsChronologicValidator.HasChronologicContent

        Return Not _FormManager.DataSource Is Nothing AndAlso _
            Not StringIsNullOrEmpty(_FormManager.DataSource.OperationLimitations.LimitsExplanation)

    End Function


    Private Sub _FormManager_DataSourceStateHasChanged(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles _FormManager.DataSourceStateHasChanged
        ConfigureButtons()
    End Sub

    Private Sub ConfigureButtons()

        NewMethodHumanReadableComboBox.Enabled = Not _FormManager.DataSource Is Nothing _
            AndAlso _FormManager.DataSource.OperationLimitations.FinancialDataCanChange

        nCancelButton.Enabled = Not _FormManager.DataSource Is Nothing AndAlso Not _FormManager.DataSource.IsNew
        EditedBaner.Visible = Not _FormManager.DataSource Is Nothing AndAlso Not _FormManager.DataSource.IsNew

    End Sub

End Class