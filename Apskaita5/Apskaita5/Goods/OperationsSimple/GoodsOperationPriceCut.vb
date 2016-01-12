Namespace Goods

    <Serializable()> _
    Public Class GoodsOperationPriceCut
        Inherits BusinessBase(Of GoodsOperationPriceCut)
        Implements IIsDirtyEnough

#Region " Business Methods "

        Private _ID As Integer = 0
        Private _JournalEntryID As Integer = 0
        Private _GoodsInfo As GoodsSummary = Nothing
        Private _OperationLimitations As OperationalLimitList = Nothing
        Private _ComplexOperationID As Integer = 0
        Private _ComplexOperationType As GoodsComplexOperationType = GoodsComplexOperationType.BulkPriceCut
        Private _ComplexOperationHumanReadable As String = ""
        Private _DocumentNumber As String = ""
        Private _Date As Date = Today
        Private _OldDate As Date = Today
        Private _Description As String = ""
        Private _AmountInWarehouseAccounts As Double = 0
        Private _TotalValueInWarehouseAccounts As Double = 0
        Private _TotalValueCurrentPriceCut As Double = 0
        Private _UnitValueInWarehouseAccounts As Double = 0
        Private _TotalValuePriceCut As Double = 0
        Private _UnitValuePriceCut As Double = 0
        Private _TotalValueAfterPriceCut As Double = 0
        Private _UnitValueAfterPriceCut As Double = 0
        Private _AccountPriceCutCosts As Long = 0
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now


        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property JournalEntryID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryID
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

        Public ReadOnly Property ComplexOperationID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ComplexOperationID
            End Get
        End Property

        Public ReadOnly Property ComplexOperationType() As GoodsComplexOperationType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ComplexOperationType
            End Get
        End Property

        Public ReadOnly Property ComplexOperationHumanReadable() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ComplexOperationHumanReadable.Trim
            End Get
        End Property

        Public Property DocumentNumber() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DocumentNumber.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If DocumentNumberIsReadOnly Then Exit Property
                If value Is Nothing Then value = ""
                If _DocumentNumber.Trim <> value.Trim Then
                    _DocumentNumber = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property [Date]() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Date
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Date)
                CanWriteProperty(True)
                If DateIsReadOnly Then Exit Property
                If _Date.Date <> value.Date Then
                    _Date = value.Date
                    PropertyHasChanged()
                    ResetValuesInWarehouse()
                End If
            End Set
        End Property

        Public ReadOnly Property OldDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OldDate
            End Get
        End Property

        Public Property Description() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Description.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If DescriptionIsReadOnly Then Exit Property
                If value Is Nothing Then value = ""
                If _Description.Trim <> value.Trim Then
                    _Description = value.Trim
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

        Public ReadOnly Property InsertDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _InsertDate
            End Get
        End Property

        Public ReadOnly Property UpdateDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UpdateDate
            End Get
        End Property


        Public ReadOnly Property DocumentNumberIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not Me.IsChild AndAlso Not Me.IsNew AndAlso _ComplexOperationID > 0
            End Get
        End Property

        Public ReadOnly Property DateIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _OperationLimitations.FinancialDataCanChange OrElse _
                    (Not Me.IsChild AndAlso Not Me.IsNew AndAlso _ComplexOperationID > 0)
            End Get
        End Property

        Public ReadOnly Property DescriptionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not Me.IsChild AndAlso Not Me.IsNew AndAlso _ComplexOperationID > 0
            End Get
        End Property

        Public ReadOnly Property TotalValuePriceCutIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _OperationLimitations.FinancialDataCanChange OrElse _
                    (Not Me.IsChild AndAlso Not Me.IsNew AndAlso _ComplexOperationID > 0)
            End Get
        End Property

        Public ReadOnly Property AccountPriceCutCostsIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _OperationLimitations.FinancialDataCanChange OrElse _
                    (Not Me.IsChild AndAlso Not Me.IsNew AndAlso _ComplexOperationID > 0)
            End Get
        End Property


        Public ReadOnly Property IsDirtyEnough() As Boolean _
            Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Not IsNew Then Return IsDirty
                Return (Not String.IsNullOrEmpty(_DocumentNumber.Trim) _
                    OrElse Not String.IsNullOrEmpty(_Description.Trim) _
                    OrElse CRound(_TotalValuePriceCut) <> 0)
            End Get
        End Property



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


        Public Sub RefreshValuesInWarehouse(ByVal value As GoodsPriceInWarehouseItem)

            If Not _OperationLimitations.FinancialDataCanChange Then Throw New Exception( _
                "Klaida. Finansinių operacijos duomenų keisti neleidžiama:" & vbCrLf _
                & _OperationLimitations.FinancialDataCanChangeExplanation)
            If value.GoodsID <> _GoodsInfo.ID Then Throw New ArgumentException( _
                "Klaida. Prekių verčių sandėlyje objekte yra ne šios prekės duomenys.")
            If value.Date.Date <> _Date.Date Then Throw New ArgumentException( _
                "Klaida. Prekių verčių sandėlyje objekto data yra " & value.Date.ToString("yyyy-MM-dd") _
                & ", o dokumento data - " & _Date.ToString("yyyy-MM-dd") & ".")

            _AmountInWarehouseAccounts = value.AmountInWarehouseAccounts
            _TotalValueInWarehouseAccounts = value.TotalValueInWarehouseAccounts
            _TotalValueCurrentPriceCut = value.TotalValueCurrentPriceCut

            RecalculateAll(True)

        End Sub

        Private Sub ResetValuesInWarehouse()

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


        Public Overrides Function Save() As GoodsOperationPriceCut

            If (Me.IsNew AndAlso Not CanAddObject()) OrElse (Not Me.IsNew AndAlso Not CanEditObject()) Then _
                Throw New System.Security.SecurityException("Klaida. Jūsų teisių nepakanka šiai operacijai.")
            If IsChild OrElse _ComplexOperationID > 0 Then Throw New InvalidOperationException( _
                "Klaida. Ši operacija yra sudedamoji kito dokumento dalis ir negali būti keičiama atskirai.")

            Me.ValidationRules.CheckRules()
            If Not IsValid Then Throw New Exception("Duomenyse yra klaidų: " & vbCrLf _
                & Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error))
            Return MyBase.Save

        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _ID
        End Function

        Public Overrides Function ToString() As String
            Return "Goods.GoodsOperationPriceCut"
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.SimpleRuleArgs("DocumentNumber", "dokumento numeris"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.SimpleRuleArgs("Description", "operacijos aprašymas"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("AmountInWarehouseAccounts", _
                "kiekis sandėlių sąskaitose (2 kl.)"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("TotalValueAfterPriceCut", _
                "bendra vertė po nukainojimo (atkūrimo)"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("AccountPriceCutCosts", _
                "nukainojimo sąnaudų sąskaita"))

            ValidationRules.AddRule(AddressOf DateValidation, New Validation.RuleArgs("Date"))
            ValidationRules.AddRule(AddressOf TotalValuePriceCutValidation, _
                New Validation.RuleArgs("TotalValuePriceCut"))

        End Sub

        ''' <summary>
        ''' Rule ensuring that operation date is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function DateValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As GoodsOperationPriceCut = DirectCast(target, GoodsOperationPriceCut)

            If Not ValObj._OperationLimitations Is Nothing AndAlso _
                Not ValObj._OperationLimitations.ValidateOperationDate(ValObj._Date, _
                e.Description, e.Severity) Then Return False

            Return True

        End Function

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

            Dim ValObj As GoodsOperationPriceCut = DirectCast(target, GoodsOperationPriceCut)

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
            AuthorizationRules.AllowWrite("Goods.GoodsOperationPriceCut2")
        End Sub

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationPriceCut1")
        End Function

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationPriceCut2")
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationPriceCut3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationPriceCut3")
        End Function

#End Region

#Region " Factory Methods "

        Public Shared Function NewGoodsOperationPriceCut(ByVal GoodsID As Integer) As GoodsOperationPriceCut
            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka naujiems duomenims įvesti.")
            Return DataPortal.Create(Of GoodsOperationPriceCut)(New Criteria(GoodsID, False))
        End Function

        Public Shared Function GetGoodsOperationPriceCut(ByVal nID As Integer) As GoodsOperationPriceCut
            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka šiems duomenims gauti.")
            Return DataPortal.Fetch(Of GoodsOperationPriceCut)(New Criteria(nID, False))
        End Function

        Public Shared Sub DeleteGoodsOperationPriceCut(ByVal id As Integer)
            If Not CanDeleteObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenų ištrynimui.")
            DataPortal.Delete(New Criteria(id, False))
        End Sub


        Private Sub New()
            ' require use of factory methods
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private mId As Integer
            Private mChild As Boolean
            Public ReadOnly Property Id() As Integer
                Get
                    Return mId
                End Get
            End Property
            Public ReadOnly Property Child() As Boolean
                Get
                    Return mChild
                End Get
            End Property
            Public Sub New(ByVal id As Integer, ByVal fetchChild As Boolean)
                mId = id
                mChild = fetchChild
            End Sub
        End Class


        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)

            Dim myComm As New SQLCommand("CreateGoodsOperationPriceCut")
            myComm.AddParam("?DT", Today.Date)
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
                _GoodsInfo.AccountingMethod, _GoodsInfo.Name, Today, 0, Nothing)

            MarkNew()

            ValidationRules.CheckRules()

        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            Dim myComm As New SQLCommand("FetchGoodsOperationPriceCut")
            myComm.AddParam("?OD", criteria.Id)

            Using myData As DataTable = myComm.Fetch

                If Not myData.Rows.Count > 0 Then Throw New Exception( _
                    "Klaida. Objektas, kurio ID='" & criteria.Id & "', nerastas.)")

                Dim dr As DataRow = myData.Rows(0)

                If ConvertEnumDatabaseCode(Of GoodsOperationType)(CIntSafe(dr.Item(2), 0)) _
                    <> GoodsOperationType.PriceCut Then Throw New Exception("Klaida. Ši operacija " _
                    & "yra ne prekių nukainojimo, o " & ConvertEnumHumanReadable( _
                    ConvertEnumDatabaseCode(Of GoodsOperationType)(CIntSafe(dr.Item(2), 0))) & ".")

                _ID = CIntSafe(dr.Item(0), 0)
                _Date = CDateSafe(dr.Item(1), Today)
                _OldDate = _Date
                _JournalEntryID = CIntSafe(dr.Item(3), 0)
                _DocumentNumber = CStrSafe(dr.Item(4)).Trim
                _Description = CStrSafe(dr.Item(5)).Trim
                _TotalValuePriceCut = -CDblSafe(dr.Item(6), 2, 0)
                _ComplexOperationID = CIntSafe(dr.Item(7), 0)
                _ComplexOperationType = ConvertEnumDatabaseCode(Of GoodsComplexOperationType) _
                    (CIntSafe(dr.Item(8), 0))
                _ComplexOperationHumanReadable = ConvertEnumHumanReadable(_ComplexOperationType)
                _AccountPriceCutCosts = CLongSafe(dr.Item(9), 0)
                _InsertDate = DateTime.SpecifyKind(CDateTimeSafe(dr.Item(10), Date.UtcNow), _
                    DateTimeKind.Utc).ToLocalTime
                _UpdateDate = DateTime.SpecifyKind(CDateTimeSafe(dr.Item(11), Date.UtcNow), _
                    DateTimeKind.Utc).ToLocalTime
                _AmountInWarehouseAccounts = CDblSafe(dr.Item(12), 6, 0)
                _TotalValueInWarehouseAccounts = CDblSafe(dr.Item(13), 2, 0)
                _TotalValueCurrentPriceCut = -CDblSafe(dr.Item(14), 2, 0)
                _GoodsInfo = GoodsSummary.GetGoodsSummary(dr, 15)

                RecalculateAll(False)

            End Using

            _OperationLimitations = OperationalLimitList.GetOperationalLimitList( _
                _GoodsInfo.ID, _ID, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                _GoodsInfo.AccountingMethod, _GoodsInfo.Name, Today, 0, Nothing)

            MarkOld()

            ValidationRules.CheckRules()

        End Sub


        Protected Overrides Sub DataPortal_Insert()
            CheckIfCanUpdate(True)
            DoSave(True)
        End Sub

        Protected Overrides Sub DataPortal_Update()
            CheckIfCanUpdate(True)
            DoSave(True)
        End Sub

        Private Sub DoSave(ByVal SaveJournalEntry As Boolean)

            Dim obj As OperationPersistenceObject = GetPersistenceObj()

            Dim JE As General.JournalEntry = Nothing
            If SaveJournalEntry Then JE = GetJournalEntryForDocument()

            Using transaction As New SqlTransaction

                Try

                    If SaveJournalEntry Then
                        JE = JE.SaveChild()
                        If IsNew Then
                            _JournalEntryID = JE.ID
                            obj.JournalEntryID = _JournalEntryID
                        End If
                    End If

                    obj = obj.Save(_OperationLimitations.FinancialDataCanChange)

                    If IsNew Then
                        _ID = obj.ID
                        _InsertDate = obj.InsertDate
                    End If
                    _UpdateDate = obj.UpdateDate

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

            _OldDate = _Date

            MarkOld()

        End Sub

        Private Function GetPersistenceObj() As OperationPersistenceObject

            Dim obj As OperationPersistenceObject
            If IsNew Then
                obj = OperationPersistenceObject.NewOperationPersistenceObject( _
                    GoodsOperationType.PriceCut, _GoodsInfo.ID)
            Else
                obj = OperationPersistenceObject.GetOperationPersistenceObject( _
                    _ID, GoodsOperationType.PriceCut)
                If obj.UpdateDate <> _UpdateDate Then Throw New Exception( _
                    My.Resources.Common_UpdateDateHasChanged)
            End If

            obj.AccountDiscounts = 0
            obj.AccountGeneral = 0
            obj.AccountPurchases = 0
            obj.AccountSalesNetCosts = 0
            obj.AmountInPurchases = 0
            obj.AccountPriceCut = -_TotalValuePriceCut
            obj.AccountOperation = _AccountPriceCutCosts
            obj.AccountOperationValue = _TotalValuePriceCut
            obj.Amount = 0
            obj.AmountInWarehouse = 0
            obj.TotalValue = 0
            obj.UnitValue = 0
            obj.Warehouse = Nothing
            obj.WarehouseID = 0
            obj.Content = _Description
            obj.DocNo = _DocumentNumber
            obj.JournalEntryID = _JournalEntryID
            obj.OperationDate = _Date
            obj.ComplexOperationID = _ComplexOperationID

            Return obj

        End Function

        Private Function GetJournalEntryForDocument() As General.JournalEntry

            Dim result As General.JournalEntry

            If IsNew Then

                If _Date.Date <= GetCurrentCompany.LastClosingDate.Date Then Throw New Exception( _
                    "Klaida. Neleidžiama koreguoti operacijų po uždarymo (" _
                    & GetCurrentCompany.LastClosingDate & ").")

                result = General.JournalEntry.NewJournalEntryChild(DocumentType.GoodsRevalue)

            Else

                result = General.JournalEntry.GetJournalEntryChild(_JournalEntryID, _
                    DocumentType.GoodsRevalue)

            End If

            result.Date = _Date.Date
            result.Person = Nothing
            result.Content = _Description & " (prekių nukainojimas)"
            result.DocNumber = _DocumentNumber

            If _OperationLimitations.FinancialDataCanChange Then

                Dim CommonBookEntryList As BookEntryInternalList = GetTotalBookEntryList()

                result.DebetList.LoadBookEntryListFromInternalList(CommonBookEntryList, False, False)
                result.CreditList.LoadBookEntryListFromInternalList(CommonBookEntryList, False, False)

            End If

            If Not result.IsValid Then Throw New Exception("Klaida. Nekorektiškai generuotas " _
                & "bendrojo žurnalo įrašas: " & result.GetAllBrokenRules)

            Return result

        End Function


        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_ID, False))
        End Sub

        Protected Overrides Sub DataPortal_Delete(ByVal criteria As Object)

            Dim JEID As Integer = 0

            If Not DirectCast(criteria, Criteria).Child Then
                Dim OperationToDelete As GoodsOperationPriceCut = New GoodsOperationPriceCut
                OperationToDelete.DataPortal_Fetch(DirectCast(criteria, Criteria).Id)
                JEID = OperationToDelete.JournalEntryID
                OperationToDelete.CheckIfCanDelete(True)
                If JEID > 0 Then IndirectRelationInfoList.CheckIfJournalEntryCanBeDeleted( _
                    JEID, DocumentType.GoodsRevalue)
            End If

            Using transaction As New SqlTransaction

                Try

                    OperationPersistenceObject.Delete(DirectCast(criteria, Criteria).Id, _
                        False, False)

                    If Not DirectCast(criteria, Criteria).Child AndAlso JEID > 0 Then _
                        General.JournalEntry.DeleteJournalEntryChild(JEID)

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

        End Sub



        Private Function GetTotalBookEntryList() As BookEntryInternalList

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

        Private Function CheckIfCanDelete(ByVal ThrowOnInvalid As Boolean) As String

            If IsNew Then Return ""

            _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                _ID, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _OldDate, 0, Nothing)

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

        Private Function CheckIfCanUpdate(ByVal ThrowOnInvalid As Boolean) As String

            If IsNew Then

                _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                    _ID, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                    _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _Date, 0, Nothing)

            Else

                _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                    _ID, GoodsOperationType.PriceCut, _GoodsInfo.ValuationMethod, _
                    _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _OldDate, 0, Nothing)
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

#End Region

    End Class

End Namespace