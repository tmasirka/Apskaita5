Namespace Goods

    <Serializable()> _
    Public Class GoodsPriceCutItem
        Inherits BusinessBase(Of GoodsPriceCutItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private _Guid As Guid = Guid.NewGuid
        Private _ID As Integer = 0
        Private _GoodsInfo As GoodsSummary = Nothing
        Private _OperationLimitations As OperationalLimitList = Nothing
        Private _Remarks As String = ""
        Private _AmountInWarehouseAccounts As Double = 0
        Private _TotalValueInWarehouseAccounts As Double = 0
        Private _TotalValueCurrentPriceCut As Double = 0
        Private _UnitValueInWarehouseAccounts As Double = 0
        Private _TotalValuePriceCut As Double = 0
        Private _UnitValuePriceCut As Double = 0
        Private _TotalValueAfterPriceCut As Double = 0
        Private _UnitValueAfterPriceCut As Double = 0
        Private _AccountPriceCutCosts As Long = 0


        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property GoodsInfo() As GoodsSummary
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo
            End Get
        End Property

        Public ReadOnly Property OperationLimitations() As OperationalLimitList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OperationLimitations
            End Get
        End Property

        Public ReadOnly Property GoodsName() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo.Name
            End Get
        End Property

        Public ReadOnly Property GoodsMeasureUnit() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo.MeasureUnit
            End Get
        End Property

        Public ReadOnly Property GoodsAccountingMethod() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo.AccountingMethodHumanReadable
            End Get
        End Property

        Public ReadOnly Property GoodsValuationMethod() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo.ValuationMethodHumanReadable
            End Get
        End Property

        Public ReadOnly Property GoodsAccountPriceCut() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo.AccountValueReduction
            End Get
        End Property

        Public Property Remarks() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Remarks.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Remarks.Trim <> value.Trim Then
                    _Remarks = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public ReadOnly Property AmountInWarehouseAccounts() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_AmountInWarehouseAccounts, 6)
            End Get
        End Property

        Public ReadOnly Property TotalValueInWarehouseAccounts() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalValueInWarehouseAccounts)
            End Get
        End Property

        Public ReadOnly Property TotalValueCurrentPriceCut() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalValueCurrentPriceCut)
            End Get
        End Property

        Public ReadOnly Property UnitValueInWarehouseAccounts() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitValueInWarehouseAccounts, 6)
            End Get
        End Property

        Public Property TotalValuePriceCut() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalValuePriceCut)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If TotalValuePriceCutIsReadOnly Then Exit Property
                If CRound(_TotalValuePriceCut) <> CRound(value) Then
                    _TotalValuePriceCut = CRound(value)
                    PropertyHasChanged()
                    Recalculate(True)
                End If
            End Set
        End Property

        Public ReadOnly Property UnitValuePriceCut() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitValuePriceCut, 6)
            End Get
        End Property

        Public ReadOnly Property TotalValueAfterPriceCut() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalValueAfterPriceCut)
            End Get
        End Property

        Public ReadOnly Property UnitValueAfterPriceCut() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitValueAfterPriceCut, 6)
            End Get
        End Property

        Public Property AccountPriceCutCosts() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountPriceCutCosts
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If AccountPriceCutCostsIsReadOnly Then Exit Property
                If _AccountPriceCutCosts <> value Then
                    _AccountPriceCutCosts = value
                    PropertyHasChanged()
                End If
            End Set
        End Property


        Public ReadOnly Property TotalValuePriceCutIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _OperationLimitations.FinancialDataCanChange OrElse _
                    Not _OperationLimitations.BaseValidator.FinancialDataCanChange
            End Get
        End Property

        Public ReadOnly Property AccountPriceCutCostsIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _OperationLimitations.BaseValidator.FinancialDataCanChange
            End Get
        End Property



        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString
            If IsValid Then Return ""
            Return "Klaida (-os) eilutėje '" & _GoodsInfo.Name & "': " _
                & Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error)
        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString
            If BrokenRulesCollection.WarningCount < 1 Then Return ""
            Return "Eilutėje '" & _GoodsInfo.Name & "' gali būti klaida: " _
                & Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning)
        End Function

        Public Function ExplainFinancialLimitations() As String
            Return _OperationLimitations.FinancialDataCanChangeExplanation
        End Function

        Public Function ExplainAllLimitations() As String
            Return _OperationLimitations.LimitsExplanation
        End Function


        Private Sub RecalculateAll(ByVal RaisePropertyChanged As Boolean)

            If CRound(_AmountInWarehouseAccounts, 6) > 0 Then
                _UnitValueInWarehouseAccounts = CRound(CRound(_TotalValueInWarehouseAccounts _
                    - _TotalValueCurrentPriceCut) / _AmountInWarehouseAccounts, 6)
            Else
                _UnitValueInWarehouseAccounts = 0
            End If

            If RaisePropertyChanged Then PropertyHasChanged("UnitValueInWarehouseAccounts")

            Recalculate(RaisePropertyChanged)

        End Sub

        Private Sub Recalculate(ByVal RaisePropertyChanged As Boolean)

            _TotalValueAfterPriceCut = CRound(_TotalValueInWarehouseAccounts _
                - _TotalValueCurrentPriceCut - _TotalValuePriceCut)

            If CRound(_AmountInWarehouseAccounts, 6) > 0 Then
                _UnitValuePriceCut = CRound(_TotalValuePriceCut / _AmountInWarehouseAccounts, 6)
                _UnitValueAfterPriceCut = CRound(_TotalValueAfterPriceCut / _AmountInWarehouseAccounts, 6)
            Else
                _UnitValuePriceCut = 0
                _UnitValueAfterPriceCut = 0
            End If

            If RaisePropertyChanged Then
                PropertyHasChanged("TotalValueAfterPriceCut")
                PropertyHasChanged("UnitValuePriceCut")
                PropertyHasChanged("UnitValueAfterPriceCut")
            End If

        End Sub


        Friend Sub RefreshValuesInWarehouse(ByVal value As GoodsPriceInWarehouseItem)

            If value.GoodsID <> _GoodsInfo.ID Then Throw New ArgumentException( _
                "Klaida. Prekių verčių sandėlyje objekte yra ne šios prekės duomenys.")

            _AmountInWarehouseAccounts = value.AmountInWarehouseAccounts
            _TotalValueInWarehouseAccounts = value.TotalValueInWarehouseAccounts
            _TotalValueCurrentPriceCut = value.TotalValueCurrentPriceCut

            PropertyHasChanged("AmountInWarehouseAccounts")
            PropertyHasChanged("TotalValueInWarehouseAccounts")
            PropertyHasChanged("TotalValueCurrentPriceCut")

            RecalculateAll(True)

        End Sub

        Friend Function GetGoodsPriceInWarehouseParam() As GoodsPriceInWarehouseParam
            Return GoodsPriceInWarehouseParam.GetGoodsPriceCutParam(_GoodsInfo.ID, _ID)
        End Function

        Friend Sub ResetValuesInWarehouse()

            If CRound(_AmountInWarehouseAccounts, 6) > 0 OrElse _
                CRound(_TotalValueInWarehouseAccounts) > 0 OrElse _
                CRound(_TotalValueCurrentPriceCut) <> 0 Then

                _AmountInWarehouseAccounts = 0
                _TotalValueInWarehouseAccounts = 0
                _TotalValueCurrentPriceCut = 0

                PropertyHasChanged("AmountInWarehouseAccounts")
                PropertyHasChanged("TotalValueInWarehouseAccounts")
                PropertyHasChanged("TotalValueCurrentPriceCut")

                RecalculateAll(True)

            End If

        End Sub


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            Return _GoodsInfo.Name & " nukainojimas"
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("AmountInWarehouseAccounts", _
                "kiekis sandėlių sąskaitose (2 kl.)"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("TotalValueAfterPriceCut", _
                "bendra vertė po nukainojimo (atkūrimo)"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("AccountPriceCutCosts", _
                "nukainojimo sąnaudų sąskaita"))

            ValidationRules.AddRule(AddressOf TotalValuePriceCutValidation, _
                New Validation.RuleArgs("TotalValuePriceCut"))

        End Sub

        ''' <summary>
        ''' Rule ensuring that the value of property TotalValuePriceCut is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function TotalValuePriceCutValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As GoodsPriceCutItem = DirectCast(target, GoodsPriceCutItem)

            If ValObj.TotalValuePriceCut = 0 Then
                e.Description = "Nenurodyta nukainojimo (atstatymo) vertė."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            Return True

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

#End Region

#Region " Factory Methods "

        Public Shared Function NewGoodsPriceCutItem(ByVal GoodsID As Integer, _
            ByVal parentValidator As IChronologicValidator) As GoodsPriceCutItem
            Return DataPortal.Create(Of GoodsPriceCutItem)(New Criteria(GoodsID, parentValidator))
        End Function

        Friend Shared Function GetGoodsPriceCutItem(ByVal dr As DataRow, _
            ByVal LimitationsDataSource As DataTable, ByVal parentValidator As IChronologicValidator) As GoodsPriceCutItem
            Return New GoodsPriceCutItem(dr, LimitationsDataSource, parentValidator)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal LimitationsDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator)
            MarkAsChild()
            Fetch(dr, LimitationsDataSource, parentValidator)
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private mId As Integer
            Private mParentValidator As IChronologicValidator
            Public ReadOnly Property Id() As Integer
                Get
                    Return mId
                End Get
            End Property
            Public ReadOnly Property ParentValidator() As IChronologicValidator
                Get
                    Return mParentValidator
                End Get
            End Property
            Public Sub New(ByVal nid As Integer, ByVal nParentValidator As IChronologicValidator)
                mId = nid
                mParentValidator = nParentValidator
            End Sub
        End Class


        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)

            Dim myComm As New SQLCommand("CreateGoodsOperationPriceCut")
            myComm.AddParam("?DT", criteria.ParentValidator.CurrentOperationDate)
            myComm.AddParam("?GD", criteria.Id)

            Using myData As DataTable = myComm.Fetch

                If Not myData.Rows.Count > 0 Then Throw New Exception( _
                    "Klaida. Prekė, kurios ID='" & criteria.Id & "', nerasta.)")

                Dim dr As DataRow = myData.Rows(0)

                _AmountInWarehouseAccounts = CDblSafe(dr.Item(0), 6, 0)
                _TotalValueInWarehouseAccounts = CDblSafe(dr.Item(1), 2, 0)
                _TotalValueCurrentPriceCut = -CDblSafe(dr.Item(2), 2, 0)
                _GoodsInfo = GoodsSummary.GetGoodsSummary(dr, 3)

                RecalculateAll(False)

            End Using

            _OperationLimitations = OperationalLimitList.GetOperationalLimitList( _
                _GoodsInfo.ID, 0, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                _GoodsInfo.AccountingMethod, _GoodsInfo.Name, criteria.ParentValidator. _
                CurrentOperationDate, 0, criteria.ParentValidator)

            MarkNew()

            ValidationRules.CheckRules()

        End Sub

        Private Sub Fetch(ByVal dr As DataRow, ByVal LimitationsDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator)

            If ConvertEnumDatabaseCode(Of GoodsOperationType)(CIntSafe(dr.Item(2), 0)) _
                <> GoodsOperationType.PriceCut Then Throw New Exception("Klaida. Ši operacija " _
                & "yra ne prekių nukainojimo, o " & ConvertEnumHumanReadable( _
                ConvertEnumDatabaseCode(Of GoodsOperationType)(CIntSafe(dr.Item(2), 0))) & ".")

            _ID = CIntSafe(dr.Item(0), 0)
            _TotalValuePriceCut = -CDblSafe(dr.Item(6), 2, 0)
            _AccountPriceCutCosts = CLongSafe(dr.Item(9), 0)
            _AmountInWarehouseAccounts = CDblSafe(dr.Item(12), 6, 0)
            _TotalValueInWarehouseAccounts = CDblSafe(dr.Item(13), 2, 0)
            _TotalValueCurrentPriceCut = -CDblSafe(dr.Item(14), 2, 0)
            _GoodsInfo = GoodsSummary.GetGoodsSummary(dr, 15)

            RecalculateAll(False)

            _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                _ID, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                _GoodsInfo.AccountingMethod, _GoodsInfo.Name, parentValidator.CurrentOperationDate, _
                0, parentValidator)

            MarkOld()

            ValidationRules.CheckRules()

        End Sub


        Friend Sub Update(ByVal parent As GoodsComplexOperationPriceCut)

            Dim obj As OperationPersistenceObject = GetPersistenceObj(parent)

            obj = obj.Save(_OperationLimitations.FinancialDataCanChange)

            If IsNew Then
                _ID = obj.ID
            End If

            MarkOld()

        End Sub

        Private Function GetPersistenceObj(ByVal parent As GoodsComplexOperationPriceCut) As OperationPersistenceObject

            Dim operationContent As String = parent.Description
            If Not String.IsNullOrEmpty(_Remarks.Trim.Trim) Then operationContent = _
                operationContent & "<#>" & _Remarks.Trim

            Dim obj As OperationPersistenceObject
            If IsNew Then
                obj = OperationPersistenceObject.NewOperationPersistenceObject( _
                    GoodsOperationType.PriceCut, _GoodsInfo.ID)
            Else
                obj = OperationPersistenceObject.GetOperationPersistenceObject( _
                    _ID, GoodsOperationType.PriceCut)
            End If

            obj.AccountDiscounts = 0
            obj.AccountGeneral = 0
            obj.AccountPurchases = 0
            obj.AccountSalesNetCosts = 0
            obj.AccountPriceCut = -_TotalValuePriceCut
            obj.AccountOperation = _AccountPriceCutCosts
            obj.AccountOperationValue = _TotalValuePriceCut
            obj.Amount = 0
            obj.AmountInWarehouse = 0
            obj.AmountInPurchases = 0
            obj.TotalValue = 0
            obj.UnitValue = 0
            obj.Warehouse = Nothing
            obj.WarehouseID = 0
            obj.Content = operationContent
            obj.DocNo = parent.DocumentNumber
            obj.JournalEntryID = parent.JournalEntryID
            obj.OperationDate = parent.Date
            obj.ComplexOperationID = parent.ID

            Return obj

        End Function


        Friend Sub DeleteSelf()
            OperationPersistenceObject.Delete(_ID, False, False)
        End Sub


        Friend Function GetTotalBookEntryList() As BookEntryInternalList

            Dim result As BookEntryInternalList = _
               BookEntryInternalList.NewBookEntryInternalList(BookEntryType.Debetas)

            If CRound(_TotalValuePriceCut) > 0 Then

                Dim GoodsAccountBookEntry As BookEntryInternal = _
                BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
                GoodsAccountBookEntry.Account = _GoodsInfo.AccountValueReduction
                GoodsAccountBookEntry.Ammount = _TotalValuePriceCut

                result.Add(GoodsAccountBookEntry)

                Dim CostsAccountBookEntry As BookEntryInternal = _
                    BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
                CostsAccountBookEntry.Account = _AccountPriceCutCosts
                CostsAccountBookEntry.Ammount = _TotalValuePriceCut

                result.Add(CostsAccountBookEntry)

            Else

                Dim GoodsAccountBookEntry As BookEntryInternal = _
                BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
                GoodsAccountBookEntry.Account = _GoodsInfo.AccountValueReduction
                GoodsAccountBookEntry.Ammount = _TotalValuePriceCut

                result.Add(GoodsAccountBookEntry)

                Dim CostsAccountBookEntry As BookEntryInternal = _
                    BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
                CostsAccountBookEntry.Account = _AccountPriceCutCosts
                CostsAccountBookEntry.Ammount = _TotalValuePriceCut

                result.Add(CostsAccountBookEntry)

            End If

            Return result

        End Function

        Friend Function CheckIfCanDelete(ByVal LimitationsDataSource As DataTable, _
            ByVal ThrowOnInvalid As Boolean, ByVal parentValidator As IChronologicValidator) As String

            If IsNew Then Return ""

            If LimitationsDataSource Is Nothing Then
                _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                    _ID, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                    _GoodsInfo.AccountingMethod, _GoodsInfo.Name, parentValidator.CurrentOperationDate, _
                    0, parentValidator)
            Else
                _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                    _ID, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                    _GoodsInfo.AccountingMethod, _GoodsInfo.Name, parentValidator.CurrentOperationDate, _
                    0, parentValidator, LimitationsDataSource)
            End If

            If Not _OperationLimitations.FinancialDataCanChange Then
                If ThrowOnInvalid Then
                    Throw New Exception("Klaida. Negalima ištrinti prekių '" & _
                        _OperationLimitations.CurrentGoodsName & "' nukainojimo operacijos:" _
                        & vbCrLf & _OperationLimitations.FinancialDataCanChangeExplanation)
                Else
                    Return "Klaida. Negalima ištrinti prekių '" & _
                        _OperationLimitations.CurrentGoodsName & "' nukainojimo operacijos:" _
                        & vbCrLf & _OperationLimitations.FinancialDataCanChangeExplanation
                End If
            End If

            Return ""

        End Function

        Friend Function CheckIfCanUpdate(ByVal LimitationsDataSource As DataTable, _
            ByVal ThrowOnInvalid As Boolean, ByVal nDate As Date, _
            ByVal parentValidator As IChronologicValidator) As String

            If IsNew Then

                _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                    _ID, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                    _GoodsInfo.AccountingMethod, _GoodsInfo.Name, nDate, 0, parentValidator)

            Else

                If LimitationsDataSource Is Nothing Then
                    _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                        _ID, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                        _GoodsInfo.AccountingMethod, _GoodsInfo.Name, parentValidator.CurrentOperationDate, _
                        0, parentValidator)
                Else
                    _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                        _ID, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                        _GoodsInfo.AccountingMethod, _GoodsInfo.Name, parentValidator.CurrentOperationDate, _
                        0, parentValidator, LimitationsDataSource)
                End If

            End If

            ValidationRules.CheckRules()
            If Not IsValid Then
                If ThrowOnInvalid Then
                    Throw New Exception("Prekių '" & _GoodsInfo.Name _
                    & "' nukainojimo operacijoje yra klaidų: " & BrokenRulesCollection.ToString)
                Else
                    Return "Prekių '" & _GoodsInfo.Name _
                        & "' nukainojimo operacijoje yra klaidų: " & BrokenRulesCollection.ToString
                End If
            End If

            Return ""

        End Function

        Friend Sub ReloadLimitations(ByVal LimitationsDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator)

            If LimitationsDataSource Is Nothing Then Throw New ArgumentNullException( _
                "Klaida. Metodui GoodsOperationTransfer.ReloadLimitations " _
                & "nenurodytas LimitationsDataSource parametras.")
            If IsNew Then Throw New InvalidOperationException("Klaida. " _
                & "Metodas GoodsOperationTransfer.ReloadLimitations gali " _
                & "būti taikomas tik Old objektams.")

            _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                _ID, GoodsOperationType.Discard, _GoodsInfo.ValuationMethod, _
                _GoodsInfo.AccountingMethod, _GoodsInfo.Name, parentValidator.CurrentOperationDate, _
                0, parentValidator, LimitationsDataSource)

            ValidationRules.CheckRules()

        End Sub

#End Region

    End Class

End Namespace