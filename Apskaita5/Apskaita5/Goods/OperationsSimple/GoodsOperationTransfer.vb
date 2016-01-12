Namespace Goods

    <Serializable()> _
    Public Class GoodsOperationTransfer
        Inherits BusinessBase(Of GoodsOperationTransfer)
        Implements IIsDirtyEnough

#Region " Business Methods "

        Private _ID As Integer = 0
        Private _GoodsInfo As GoodsSummary = Nothing
        Private _OperationLimitations As OperationalLimitList = Nothing
        Private _ComplexOperationID As Integer = 0
        Private _ComplexOperationType As GoodsComplexOperationType = GoodsComplexOperationType.InternalTransfer
        Private _ComplexOperationHumanReadable As String = ""
        Private _JournalEntryID As Integer = 0
        Private _JournalEntryDate As Date = Today
        Private _JournalEntryContent As String = ""
        Private _JournalEntryCorrespondence As String = ""
        Private _JournalEntryRelatedPerson As String = ""
        Private _JournalEntryType As DocumentType = DocumentType.None
        Private _JournalEntryTypeHumanReadable As String = ""
        Private _DocumentNumber As String = ""
        Private _Warehouse As WarehouseInfo = Nothing
        Private _OldWarehouseID As Integer = 0
        Private _Date As Date = Today
        Private _OldDate As Date = Today
        Private _Description As String = ""
        Private _Amount As Double = 0
        Private _UnitCost As Double = 0
        Private _TotalCost As Double = 0
        Private _AccountGoodsCost As Long = 0
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now


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

        Public ReadOnly Property JournalEntryID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryID
            End Get
        End Property

        Public ReadOnly Property JournalEntryDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryDate
            End Get
        End Property

        Public ReadOnly Property JournalEntryContent() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryContent.Trim
            End Get
        End Property

        Public ReadOnly Property JournalEntryCorrespondence() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryCorrespondence.Trim
            End Get
        End Property

        Public ReadOnly Property JournalEntryRelatedPerson() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryRelatedPerson.Trim
            End Get
        End Property

        Public ReadOnly Property JournalEntryType() As DocumentType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryType
            End Get
        End Property

        Public ReadOnly Property JournalEntryTypeHumanReadable() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryTypeHumanReadable.Trim
            End Get
        End Property

        Public ReadOnly Property DocumentNumber() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DocumentNumber.Trim
            End Get
        End Property

        Public Property Warehouse() As WarehouseInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Warehouse
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WarehouseInfo)
                CanWriteProperty(True)
                If WarehouseIsReadOnly Then Exit Property
                If Not (_Warehouse Is Nothing AndAlso value Is Nothing) _
                    AndAlso Not (Not _Warehouse Is Nothing AndAlso Not value Is Nothing _
                    AndAlso _Warehouse.ID = value.ID) Then
                    _Warehouse = value
                    PropertyHasChanged()
                    PropertyHasChanged("AccountGoodsWarehouse")
                    _OperationLimitations.SetWarehouse(_Warehouse)
                End If
            End Set
        End Property

        Public ReadOnly Property OldWarehouseID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OldWarehouseID
            End Get
        End Property

        Public ReadOnly Property AccountGoodsWarehouse() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _GoodsInfo.AccountingMethod = GoodsAccountingMethod.Periodic OrElse _
                    _Warehouse Is Nothing OrElse Not _Warehouse.ID > 0 Then Return 0
                Return _Warehouse.WarehouseAccount
            End Get
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

        Public Property Amount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Amount, 6)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If AmountIsReadOnly Then Exit Property
                If CRound(_Amount, 6) <> CRound(value, 6) Then
                    _Amount = CRound(value, 6)
                    PropertyHasChanged()
                    _UnitCost = 0
                    _TotalCost = 0
                    PropertyHasChanged("UnitCost")
                    PropertyHasChanged("TotalCost")
                End If
            End Set
        End Property

        Public ReadOnly Property UnitCost() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitCost, 6)
            End Get
        End Property

        Public ReadOnly Property TotalCost() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalCost)
            End Get
        End Property

        Public Property AccountGoodsCost() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountGoodsCost
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If AccountGoodsCostIsReadOnly Then Exit Property
                If _AccountGoodsCost <> value Then
                    _AccountGoodsCost = value
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


        Public ReadOnly Property WarehouseIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _OperationLimitations.FinancialDataCanChange OrElse _
                    (Not Me.IsChild AndAlso Not Me.IsNew AndAlso (_ComplexOperationID > 0 _
                    OrElse (_JournalEntryID > 0 AndAlso ( _
                    _JournalEntryType = DocumentType.InvoiceMade OrElse _
                    _JournalEntryType = DocumentType.InvoiceReceived))))
            End Get
        End Property

        Public ReadOnly Property DateIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return IsChild OrElse (Not Me.IsNew AndAlso (_ComplexOperationID > 0 _
                    OrElse (_JournalEntryID > 0 AndAlso ( _
                    _JournalEntryType = DocumentType.InvoiceMade OrElse _
                    _JournalEntryType = DocumentType.InvoiceReceived))))
            End Get
        End Property

        Public ReadOnly Property DescriptionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not Me.IsChild AndAlso Not Me.IsNew AndAlso (_ComplexOperationID > 0 _
                    OrElse (_JournalEntryID > 0 AndAlso ( _
                    _JournalEntryType = DocumentType.InvoiceMade OrElse _
                    _JournalEntryType = DocumentType.InvoiceReceived)))
            End Get
        End Property

        Public ReadOnly Property AmountIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _OperationLimitations.FinancialDataCanChange OrElse _
                    (Not Me.IsChild AndAlso Not Me.IsNew AndAlso (_ComplexOperationID > 0 _
                    OrElse (_JournalEntryID > 0 AndAlso ( _
                    _JournalEntryType = DocumentType.InvoiceMade OrElse _
                    _JournalEntryType = DocumentType.InvoiceReceived))))
            End Get
        End Property

        Public ReadOnly Property AccountGoodsCostIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _GoodsInfo.AccountingMethod = GoodsAccountingMethod.Periodic OrElse _
                    Not _OperationLimitations.FinancialDataCanChange OrElse _
                    (Not Me.IsChild AndAlso Not Me.IsNew AndAlso (_ComplexOperationID > 0 _
                    OrElse (_JournalEntryID > 0 AndAlso ( _
                    _JournalEntryType = DocumentType.InvoiceMade OrElse _
                    _JournalEntryType = DocumentType.InvoiceReceived))))
            End Get
        End Property

        Public ReadOnly Property AssociatedJournalEntryIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Me.IsChild OrElse (Not Me.IsChild AndAlso Not Me.IsNew _
                    AndAlso (_ComplexOperationID > 0 OrElse (_JournalEntryID > 0 _
                    AndAlso (_JournalEntryType = DocumentType.InvoiceMade OrElse _
                    _JournalEntryType = DocumentType.InvoiceReceived))))
            End Get
        End Property


        Public ReadOnly Property IsDirtyEnough() As Boolean _
            Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Not IsNew Then Return IsDirty
                Return (Not String.IsNullOrEmpty(_Description.Trim) OrElse _
                    (Not _Warehouse Is Nothing AndAlso _Warehouse.ID > 0) OrElse _
                    CRound(_Amount, 6) > 0)
            End Get
        End Property



        Public Overrides Function Save() As GoodsOperationTransfer

            If IsNew AndAlso Not CanAddObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka naujiems duomenims įvesti.")
            If Not IsNew AndAlso Not CanEditObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims pakeisti.")

            If IsChild OrElse _ComplexOperationID > 0 Then Throw New InvalidOperationException( _
                "Klaida. Ši operacija yra sudedamoji kito dokumento dalis ir negali būti keičiama atskirai.")
            If _JournalEntryType = DocumentType.InvoiceMade Then Throw New Exception( _
                "Klaida. Ši operacija yra sudedamoji išrašytos sąskaitos faktūros dalis, " _
                & "ją keisti galima tik kartu su atitinkama sąskaita faktūra.")
            If _JournalEntryType = DocumentType.InvoiceReceived Then Throw New Exception( _
                "Klaida. Ši operacija yra sudedamoji gautos sąskaitos faktūros dalis, " _
                & "ją keisti galima tik kartu su atitinkama sąskaita faktūra.")

            Me.ValidationRules.CheckRules()
            If Not IsValid Then Throw New Exception("Duomenyse yra klaidų: " _
                & vbCrLf & Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error))
            Return MyBase.Save

        End Function


        Public Sub LoadAssociatedJournalEntry(ByVal Entry As ActiveReports.JournalEntryInfo)

            If AssociatedJournalEntryIsReadOnly Then Exit Sub

            If Entry Is Nothing OrElse Not Entry.Id > 0 Then Exit Sub

            If Entry.DocType = DocumentType.InvoiceMade OrElse Entry.DocType = DocumentType.InvoiceReceived Then
                Throw New Exception("Prekių perleidimas pagal sąskaitą faktūrą " _
                    & "gali būti registruojamas tik per atitinkamą sąskaitą faktūrą.")
            ElseIf Entry.DocType = DocumentType.Amortization Then
                Throw New Exception("Prekės negali būti perleidžiamos pagal ilgalaikio turto amortizacijos dokumentą.")
            ElseIf Entry.DocType = DocumentType.ClosingEntries Then
                Throw New Exception("Prekės negali būti perleidžiamos pagal 5/6 klasių uždarymo dokumentą.")
            ElseIf Entry.DocType = DocumentType.GoodsProduction Then
                Throw New Exception("Prekės negali būti perleidžiamos pagal prekių gamybos dokumentą.")
            ElseIf Entry.DocType = DocumentType.GoodsRevalue Then
                Throw New Exception("Prekės negali būti perleidžiamos pagal prekių nukainojimo dokumentą.")
            ElseIf Entry.DocType = DocumentType.GoodsWriteOff Then
                Throw New Exception("Prekės negali būti perleidžiamos pagal prekių nurašymo dokumentą.")
            ElseIf Entry.DocType = DocumentType.ImprestSheet Then
                Throw New Exception("Prekės negali būti perleidžiamos pagal darbo užmokesčio avanso žiniaraštį.")
            ElseIf Entry.DocType = DocumentType.LongTermAssetAccountChange Then
                Throw New Exception("Prekės negali būti perleidžiamos pagal ilgalaikio turto apskaitos sąskaitos pakeitimo dokumentą.")
            ElseIf Entry.DocType = DocumentType.Offset Then
                Throw New Exception("Prekės negali būti perleidžiamos pagal užskaitos dokumentą.")
            ElseIf Entry.DocType = DocumentType.WageSheet Then
                Throw New Exception("Prekės negali būti perleidžiamos pagal darbo užmokesčio žiniaraštį.")
            ElseIf Entry.Date.Date <> _Date.Date Then
                Throw New Exception("Klaida. Susietas bendrojo žurnalo įrašas privalo būti tos pačios datos kaip ir operacija.")
            End If

            _JournalEntryID = Entry.Id
            _JournalEntryDate = Entry.Date
            _JournalEntryContent = Entry.Content
            _JournalEntryCorrespondence = Entry.BookEntries
            _JournalEntryRelatedPerson = Entry.Person
            _JournalEntryType = Entry.DocType
            _JournalEntryTypeHumanReadable = Entry.DocTypeHumanReadable

            _DocumentNumber = Entry.DocNumber

            PropertyHasChanged("JournalEntryID")
            PropertyHasChanged("JournalEntryDate")
            PropertyHasChanged("JournalEntryContent")
            PropertyHasChanged("JournalEntryCorrespondence")
            PropertyHasChanged("JournalEntryRelatedPerson")
            PropertyHasChanged("JournalEntryType")
            PropertyHasChanged("JournalEntryTypeHumanReadable")
            PropertyHasChanged("DocumentNumber")

        End Sub

        Public Sub ReloadCostInfo(ByVal CostInfo As GoodsCostItem)

            If _GoodsInfo.AccountingMethod = GoodsAccountingMethod.Periodic Then Throw New Exception( _
                "Klaida. Šiai prekei taikoma periodinė apskaita. Jos partijų vertės neskaičiuojamos.")
            If CostInfo.GoodsID <> _GoodsInfo.ID Then Throw New Exception( _
                "Klaida. Nesutampa prekių ID ir prekių partijos vertės ID.")
            If Not _Warehouse Is Nothing AndAlso CostInfo.WarehouseID <> _Warehouse.ID Then _
                Throw New Exception("Klaida. Nesutampa prekių ir prekių partijos sandėliai.")
            If CostInfo.Amount <> CRound(_Amount, 6) Then _
                Throw New Exception("Klaida. Nesutampa nurodytas prekių kiekis " & _
                "ir prekių kiekis, kuriam paskaičiuota vertė.")
            If CostInfo.ValuationMethod <> _GoodsInfo.ValuationMethod Then Throw New Exception( _
                "Klaida. Nesutampa prekių ir prekių partijos vertinimo metodas.")
            If Not _OperationLimitations.FinancialDataCanChange Then Throw New Exception( _
                "Klaida. Operacijos su preke """ & _GoodsInfo.Name _
                & """ finansiniai duomenys, įskaitant savikainą, negali būti keičiami. Priežastis:" _
                & vbCrLf & _OperationLimitations.FinancialDataCanChangeExplanation)

            _UnitCost = CostInfo.UnitCosts
            _TotalCost = CostInfo.TotalCosts
            PropertyHasChanged("UnitCost")
            PropertyHasChanged("TotalCost")

        End Sub

        Friend Sub SetDate(ByVal value As Date)
            If Not IsChild Then Throw New InvalidOperationException( _
                "Klaida. Metodas SetDate gali būti naudojamas tik child objektui.")
            If _Date.Date <> value.Date Then
                _Date = value.Date
                PropertyHasChanged("Date")
            End If
        End Sub

        Friend Sub SetAssociatedJournalEntryID(ByVal EntryID As Integer)
            If Not IsChild Then Throw New InvalidOperationException( _
                "Klaida. Metodas SetAssociatedJournalEntryID gali būti naudojamas tik child objektui.")
            If _JournalEntryID <> EntryID Then _JournalEntryID = EntryID
        End Sub

        Friend Sub SetCosts(ByVal nUnitCost As Double, ByVal nTotalCost As Double)
            If CRound(_UnitCost, 6) <> CRound(nUnitCost, 6) Then
                _UnitCost = nUnitCost
                PropertyHasChanged("UnitCost")
            End If
            If CRound(_TotalCost, 6) <> CRound(nTotalCost, 6) Then
                _TotalCost = nTotalCost
                PropertyHasChanged("TotalCost")
            End If
        End Sub


        Protected Overrides Function GetIdValue() As Object
            Return _ID
        End Function

        Public Overrides Function ToString() As String
            Return "Goods.GoodsOperationTransfer"
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.InfoObjectRequired, _
                New CommonValidation.InfoObjectRequiredRuleArgs("Warehouse", "sandėlis", "ID"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("Amount", "perleidžiamas kiekis"))

            ValidationRules.AddRule(AddressOf DateValidation, New Validation.RuleArgs("Date"))
            ValidationRules.AddRule(AddressOf DescriptionValidation, _
                New CommonValidation.SimpleRuleArgs("Description", "operacijos aprašymas", _
                Validation.RuleSeverity.Warning))
            ValidationRules.AddRule(AddressOf JournalEntryValidation, _
                New Validation.RuleArgs("JournalEntryID"))
            ValidationRules.AddRule(AddressOf AccountGoodsCostValidation, _
                New Validation.RuleArgs("AccountGoodsCost"))

            ValidationRules.AddDependantProperty("Date", "JournalEntryID", False)
            ValidationRules.AddDependantProperty("Warehouse", "Date", False)

        End Sub

        ''' <summary>
        ''' Rule ensuring that the value of property Description is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function DescriptionValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As GoodsOperationTransfer = DirectCast(target, GoodsOperationTransfer)

            If valObj.IsChild Then Return True

            Return CommonValidation.StringRequired(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that the value of property Date is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function DateValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As GoodsOperationTransfer = DirectCast(target, GoodsOperationTransfer)

            If Not ValObj._OperationLimitations Is Nothing AndAlso _
                Not ValObj._OperationLimitations.ValidateOperationDate(ValObj._Date, _
                e.Description, e.Severity) Then Return False

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that associated journal entry is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function JournalEntryValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As GoodsOperationTransfer = DirectCast(target, GoodsOperationTransfer)

            If Not ValObj.IsChild AndAlso Not ValObj._JournalEntryID > 0 Then
                e.Description = "Nenurodytas susietas bendrojo žurnalo įrašas " & _
                    "(dokumentas, pagrindžiantis įsigijimą)."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            ElseIf Not ValObj.IsChild AndAlso ValObj._JournalEntryDate.Date <> ValObj._Date.Date Then
                e.Description = "Susieto bendrojo žurnalo įrašo (dokumento, " & _
                    "pagrindžiančio įsigijimą) data nesutampa su operacijos data."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that associated journal entry is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AccountGoodsCostValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As GoodsOperationTransfer = DirectCast(target, GoodsOperationTransfer)

            If ValObj._GoodsInfo.AccountingMethod = GoodsAccountingMethod.Persistent AndAlso _
                Not ValObj._AccountGoodsCost > 0 Then
                e.Description = "Nenurodyta savikainos nurašymo sąskaita."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            Return True

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
            AuthorizationRules.AllowWrite("Goods.GoodsOperationTransfer2")
        End Sub

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationTransfer1")
        End Function

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationTransfer2")
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationTransfer3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationTransfer3")
        End Function

#End Region

#Region " Factory Methods "

        Public Shared Function NewGoodsOperationTransfer(ByVal GoodsID As Integer) As GoodsOperationTransfer
            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka naujiems duomenims įvesti.")
            Return DataPortal.Create(Of GoodsOperationTransfer)(New Criteria(GoodsID))
        End Function

        Friend Shared Function NewGoodsOperationTransferChild(ByVal summary As GoodsSummary) As GoodsOperationTransfer
            Return New GoodsOperationTransfer(summary)
        End Function

        Friend Shared Function NewGoodsOperationTransferChild(ByVal nGoodsID As Integer, _
            ByVal nWarehouseID As Integer) As GoodsOperationTransfer
            Return New GoodsOperationTransfer(nGoodsID, nWarehouseID)
        End Function

        Public Shared Function GetGoodsOperationTransfer(ByVal nID As Integer) As GoodsOperationTransfer
            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka šiems duomenims gauti.")
            Return DataPortal.Fetch(Of GoodsOperationTransfer)(New Criteria(nID))
        End Function

        Friend Shared Function GetGoodsOperationTransferChild( _
            ByVal obj As OperationPersistenceObject, _
            ByVal LimitationsDataSource As DataTable) As GoodsOperationTransfer
            Return New GoodsOperationTransfer(obj, LimitationsDataSource)
        End Function

        Friend Shared Function GetGoodsOperationTransferChild(ByVal nOperationID As Integer) As GoodsOperationTransfer
            Return New GoodsOperationTransfer(nOperationID)
        End Function

        Public Shared Sub DeleteGoodsOperationTransfer(ByVal id As Integer)
            If Not CanDeleteObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenų ištrynimui.")
            DataPortal.Delete(New Criteria(id))
        End Sub

        Friend Sub DeleteGoodsOperationTransferChild()
            If Not IsChild Then Throw New InvalidOperationException( _
                "Method DeleteGoodsOperationTransferChild is only applicable to child object.")
            DoDelete()
        End Sub


        Private Sub New()
            ' require use of factory methods
        End Sub

        Private Sub New(ByVal summary As GoodsSummary)
            ' require use of factory methods
            MarkAsChild()
            Create(summary)
        End Sub

        Private Sub New(ByVal nGoodsID As Integer, ByVal nWarehouseID As Integer)
            ' require use of factory methods
            MarkAsChild()
            Create(nGoodsID)
        End Sub

        Private Sub New(ByVal obj As OperationPersistenceObject, ByVal LimitationsDataSource As DataTable)
            ' require use of factory methods
            MarkAsChild()
            Fetch(obj, LimitationsDataSource)
        End Sub

        Private Sub New(ByVal nOperationID As Integer)
            ' require use of factory methods
            MarkAsChild()
            Fetch(nOperationID)
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private mId As Integer
            Public ReadOnly Property Id() As Integer
                Get
                    Return mId
                End Get
            End Property
            Public Sub New(ByVal id As Integer)
                mId = id
            End Sub
        End Class


        <NonSerialized(), NotUndoable()> _
        Private DiscardList As ConsignmentDiscardPersistenceObjectList = Nothing

        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)
            Create(criteria.Id)
        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            Fetch(criteria.Id)
        End Sub

        Private Sub Fetch(ByVal nOperationID As Integer)
            Dim obj As OperationPersistenceObject = OperationPersistenceObject. _
                GetOperationPersistenceObject(nOperationID, GoodsOperationType.Transfer)
            Fetch(obj, Nothing)
        End Sub

        Private Sub Fetch(ByVal obj As OperationPersistenceObject, ByVal LimitationsDataSource As DataTable)

            If obj.OperationType <> GoodsOperationType.Transfer Then Throw New Exception( _
                "Operacijos, kurios ID=" & obj.ID.ToString & " tipas yra ne " & _
                "perleidimas, o " & ConvertEnumHumanReadable(obj.OperationType) & ".")

            _ID = obj.ID
            _ComplexOperationID = obj.ComplexOperationID
            _ComplexOperationType = obj.ComplexOperationType
            _ComplexOperationHumanReadable = obj.ComplexOperationHumanReadable
            _Date = obj.OperationDate
            _OldDate = _Date
            _Description = obj.Content
            _Amount = -obj.Amount
            _UnitCost = obj.UnitValue
            _TotalCost = -obj.TotalValue
            _Warehouse = obj.Warehouse
            _OldWarehouseID = obj.WarehouseID
            _JournalEntryID = obj.JournalEntryID
            _JournalEntryDate = obj.JournalEntryDate
            _JournalEntryContent = obj.JournalEntryContent
            _JournalEntryCorrespondence = obj.JournalEntryCorrespondence
            _JournalEntryRelatedPerson = obj.JournalEntryRelatedPerson
            _JournalEntryType = obj.JournalEntryType
            _JournalEntryTypeHumanReadable = obj.JournalEntryTypeHumanReadable
            _DocumentNumber = obj.DocNo
            _InsertDate = obj.InsertDate
            _UpdateDate = obj.UpdateDate
            _AccountGoodsCost = obj.AccountOperation
            _GoodsInfo = obj.GoodsInfo

            If LimitationsDataSource Is Nothing Then
                _OperationLimitations = OperationalLimitList.GetOperationalLimitList( _
                    obj.GoodsID, obj.ID, GoodsOperationType.Transfer, _GoodsInfo.ValuationMethod, _
                    _GoodsInfo.AccountingMethod, _GoodsInfo.Name, obj.OperationDate, obj.WarehouseID, Nothing)
            Else
                _OperationLimitations = OperationalLimitList.GetOperationalLimitList(obj.GoodsID, _
                    obj.ID, GoodsOperationType.Transfer, _GoodsInfo.ValuationMethod, _
                    _GoodsInfo.AccountingMethod, _GoodsInfo.Name, obj.OperationDate, _
                    obj.WarehouseID, Nothing, LimitationsDataSource)
            End If

            MarkOld()

            ValidationRules.CheckRules()

        End Sub

        Private Sub Create(ByVal nGoodsID As Integer)
            Create(GoodsSummary.GetGoodsSummary(nGoodsID))
        End Sub

        Private Sub Create(ByVal summary As GoodsSummary)

            _GoodsInfo = summary
            _Warehouse = _GoodsInfo.DefaultWarehouse
            If _GoodsInfo.AccountingMethod = GoodsAccountingMethod.Persistent Then _
                _AccountGoodsCost = _GoodsInfo.AccountSalesNetCosts
            Dim wid As Integer = 0
            If Not _Warehouse Is Nothing AndAlso _Warehouse.ID > 0 Then wid = _Warehouse.ID
            _OperationLimitations = OperationalLimitList.GetOperationalLimitList( _
                _GoodsInfo.ID, 0, GoodsOperationType.Transfer, _GoodsInfo.ValuationMethod, _
                _GoodsInfo.AccountingMethod, _GoodsInfo.Name, Today, wid, Nothing)

            MarkNew()

            ValidationRules.CheckRules()

        End Sub


        Protected Overrides Sub DataPortal_Insert()

            CheckIfCanUpdate(Nothing, True)

            GetDiscardList()

            DoSave(True, False, False)

            _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                _ID, GoodsOperationType.Acquisition, _GoodsInfo.ValuationMethod, _
                _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _OldDate, _OldWarehouseID, Nothing)

        End Sub

        Protected Overrides Sub DataPortal_Update()

            CheckIfCanUpdate(Nothing, True)

            GetDiscardList()

            DoSave(True, False, False)

            _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                _ID, GoodsOperationType.Acquisition, _GoodsInfo.ValuationMethod, _
                _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _OldDate, _OldWarehouseID, Nothing)

        End Sub

        Friend Sub SaveChild(ByVal ParentJournalEntryID As Integer, _
            ByVal ParentComplexOperationID As Integer, ByVal ParentDescription As String, _
            ByVal ParentDocNo As String, ByVal ManageConsignments As Boolean, _
            ByVal ForceFinancialChangeForPeriodic As Boolean, ByVal FinancialDataReadOnly As Boolean)
            _JournalEntryID = ParentJournalEntryID
            _Description = ParentDescription
            _DocumentNumber = ParentDocNo
            If ParentComplexOperationID > 0 Then _ComplexOperationID = ParentComplexOperationID
            DoSave(ManageConsignments, ForceFinancialChangeForPeriodic, FinancialDataReadOnly)
        End Sub

        Private Sub DoSave(ByVal ManageConsignments As Boolean, _
            ByVal ForceFinancialChangeForPeriodic As Boolean, _
            ByVal FinancialDataReadOnly As Boolean)

            If ManageConsignments AndAlso _OperationLimitations.FinancialDataCanChange AndAlso _
                Not FinancialDataReadOnly AndAlso DiscardList Is Nothing AndAlso _
                _GoodsInfo.AccountingMethod <> GoodsAccountingMethod.Periodic Then _
                Throw New InvalidOperationException("Klaida. Prieš išsaugant GoodsOperationTransfer " _
                & "būtina iškviesti GetDiscardList metodą.")

            Dim obj As OperationPersistenceObject = GetPersistenceObj(ForceFinancialChangeForPeriodic)

            Using transaction As New SqlTransaction

                Try

                    obj = obj.Save(_OperationLimitations.FinancialDataCanChange _
                        AndAlso Not FinancialDataReadOnly)

                    If IsNew Then
                        _ID = obj.ID
                        _InsertDate = obj.InsertDate
                    End If
                    _UpdateDate = obj.UpdateDate

                    If ManageConsignments AndAlso _OperationLimitations.FinancialDataCanChange _
                        AndAlso _GoodsInfo.AccountingMethod <> GoodsAccountingMethod.Periodic _
                        AndAlso Not FinancialDataReadOnly Then
                        DiscardList.Update(_ID)
                    End If

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

            _OldDate = _Date
            _OldWarehouseID = _Warehouse.ID

            MarkOld()

        End Sub

        Private Function GetPersistenceObj(ByVal ForceFinancialChangeForPeriodic As Boolean) As OperationPersistenceObject

            If Not IsChild Then ForceFinancialChangeForPeriodic = False

            Dim obj As OperationPersistenceObject
            If IsNew Then
                obj = OperationPersistenceObject.NewOperationPersistenceObject( _
                    GoodsOperationType.Transfer, _GoodsInfo.ID)
            Else
                obj = OperationPersistenceObject.GetOperationPersistenceObject( _
                    _ID, GoodsOperationType.Transfer)
                If obj.UpdateDate <> _UpdateDate Then Throw New Exception( _
                    My.Resources.Common_UpdateDateHasChanged)
            End If

            If _GoodsInfo.AccountingMethod = GoodsAccountingMethod.Persistent Then

                obj.AccountGeneral = -_TotalCost
                obj.AccountOperation = _AccountGoodsCost
                obj.AccountOperationValue = _TotalCost
                obj.AmountInWarehouse = -_Amount
                obj.AmountInPurchases = 0
                obj.TotalValue = -_TotalCost
                obj.UnitValue = _UnitCost

            Else

                If ForceFinancialChangeForPeriodic Then
                    obj.AmountInPurchases = -_Amount
                    obj.AccountPurchases = -_TotalCost
                    obj.TotalValue = -_TotalCost
                    obj.UnitValue = _UnitCost
                Else
                    obj.AmountInPurchases = 0
                    obj.AccountPurchases = 0
                    obj.TotalValue = 0
                    obj.UnitValue = 0
                End If
                obj.AmountInWarehouse = 0

            End If

            obj.Amount = -_Amount
            obj.Content = _Description
            obj.DocNo = _DocumentNumber
            obj.JournalEntryID = _JournalEntryID
            obj.OperationDate = _Date
            obj.Warehouse = _Warehouse
            obj.WarehouseID = _Warehouse.ID
            obj.ComplexOperationID = _ComplexOperationID

            Return obj

        End Function


        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_ID))
        End Sub

        Protected Overrides Sub DataPortal_Delete(ByVal criteria As Object)

            Dim OperationToDelete As GoodsOperationTransfer = New GoodsOperationTransfer
            OperationToDelete.Fetch(DirectCast(criteria, Criteria).Id)

            If OperationToDelete.ComplexOperationID > 0 Then Throw New Exception( _
                "Klaida. Ši operacija yra sudedamoji dalis kompleksinės operacijos - " _
                & ConvertEnumHumanReadable(OperationToDelete.ComplexOperationType) _
                & ". Ją pašalinti galima tik kartu su komplesine operacija.")

            If OperationToDelete.JournalEntryType = DocumentType.InvoiceMade Then _
                Throw New Exception("Klaida. Ši operacija yra sudedamoji dalis išrašytos " _
                & "sąskaitos faktūros. Ją pašalinti galima tik kartu su sąskaita faktūra.")

            If OperationToDelete.JournalEntryType = DocumentType.InvoiceReceived Then _
                Throw New Exception("Klaida. Ši operacija yra sudedamoji dalis gautos " _
                & "sąskaitos faktūros. Ją pašalinti galima tik kartu su sąskaita faktūra.")

            If Not OperationToDelete._OperationLimitations.FinancialDataCanChange Then _
                Throw New Exception("Klaida. Negalima ištrinti prekių '" & _
                    OperationToDelete._OperationLimitations.CurrentGoodsName & _
                    "' perleidimo operacijos:" & vbCrLf & OperationToDelete. _
                    _OperationLimitations.FinancialDataCanChangeExplanation)

            OperationToDelete.DoDelete()

        End Sub

        Private Sub DoDelete()

            Using transaction As New SqlTransaction

                Try

                    OperationPersistenceObject.Delete(_ID, False, True)

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

            MarkNew()

        End Sub


        Friend Function GetTotalBookEntryList(ByVal SalesIncomeAccount As Long, _
            ByVal SalesTotalSum As Double) As BookEntryInternalList

            Dim result As BookEntryInternalList = _
               BookEntryInternalList.NewBookEntryInternalList(BookEntryType.Debetas)

            If SalesIncomeAccount > 0 AndAlso CRound(SalesTotalSum) > 0 Then

                Dim SalesAccountBookEntry As BookEntryInternal = _
                    BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
                SalesAccountBookEntry.Account = SalesIncomeAccount
                SalesAccountBookEntry.Ammount = CRound(SalesTotalSum)

                result.Add(SalesAccountBookEntry)

            End If

            If _GoodsInfo.AccountingMethod = GoodsAccountingMethod.Periodic Then Return result

            Dim DiscardAccountBookEntry As BookEntryInternal = _
                BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas)
            DiscardAccountBookEntry.Account = _Warehouse.WarehouseAccount
            DiscardAccountBookEntry.Ammount = CRound(_TotalCost)

            result.Add(DiscardAccountBookEntry)

            Dim CostsAccountBookEntry As BookEntryInternal = _
                BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas)
            CostsAccountBookEntry.Account = _AccountGoodsCost
            CostsAccountBookEntry.Ammount = CRound(_TotalCost)

            result.Add(CostsAccountBookEntry)

            Return result

        End Function

        Friend Function CheckIfCanDelete(ByVal LimitationsDataSource As DataTable, _
            ByVal ThrowOnInvalid As Boolean) As String

            If IsNew Then Return ""

            If LimitationsDataSource Is Nothing Then
                _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                    _ID, GoodsOperationType.Transfer, _GoodsInfo.ValuationMethod, _
                    _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _OldDate, _OldWarehouseID, Nothing)
            Else
                _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                    _ID, GoodsOperationType.Transfer, _GoodsInfo.ValuationMethod, _
                    _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _OldDate, _OldWarehouseID, _
                    Nothing, LimitationsDataSource)
            End If

            If Not _OperationLimitations.FinancialDataCanChange Then
                If ThrowOnInvalid Then
                    Throw New Exception("Klaida. Negalima ištrinti prekių '" & _
                        _OperationLimitations.CurrentGoodsName & "' perleidimo operacijos: " _
                        & vbCrLf & _OperationLimitations.FinancialDataCanChangeExplanation)
                Else
                    Return "Klaida. Negalima ištrinti prekių '" & _
                        _OperationLimitations.CurrentGoodsName & "' perleidimo operacijos: " _
                        & vbCrLf & _OperationLimitations.FinancialDataCanChangeExplanation
                End If
            End If

            Return ""

        End Function

        Friend Function CheckIfCanUpdate(ByVal LimitationsDataSource As DataTable, _
            ByVal ThrowOnInvalid As Boolean) As String

            If IsNew Then

                _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                    _ID, GoodsOperationType.Transfer, _GoodsInfo.ValuationMethod, _
                    _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _Date, _Warehouse.ID, Nothing)

            Else

                If LimitationsDataSource Is Nothing Then
                    _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                        _ID, GoodsOperationType.Transfer, _GoodsInfo.ValuationMethod, _
                        _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _OldDate, _OldWarehouseID, Nothing)
                Else
                    _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                        _ID, GoodsOperationType.Transfer, _GoodsInfo.ValuationMethod, _
                        _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _OldDate, _OldWarehouseID, _
                        Nothing, LimitationsDataSource)
                End If

            End If

            ValidationRules.CheckRules()
            If Not IsValid Then
                If ThrowOnInvalid Then
                    Throw New Exception("Prekių '" & _GoodsInfo.Name _
                    & "' perleidimo operacijoje yra klaidų: " & BrokenRulesCollection.ToString)
                Else
                    Return "Prekių '" & _GoodsInfo.Name _
                        & "' perleidimo operacijoje yra klaidų: " & BrokenRulesCollection.ToString
                End If
            End If

            Return ""

        End Function

        Friend Sub ReloadLimitations(ByVal LimitationsDataSource As DataTable)

            If LimitationsDataSource Is Nothing Then Throw New ArgumentNullException( _
                "Klaida. Metodui GoodsOperationTransfer.ReloadLimitations " _
                & "nenurodytas LimitationsDataSource parametras.")
            If IsNew Then Throw New InvalidOperationException("Klaida. " _
                & "Metodas GoodsOperationTransfer.ReloadLimitations gali " _
                & "būti taikomas tik Old objektams.")

            _OperationLimitations = OperationalLimitList.GetOperationalLimitList(_GoodsInfo.ID, _
                _ID, GoodsOperationType.Transfer, _GoodsInfo.ValuationMethod, _
                _GoodsInfo.AccountingMethod, _GoodsInfo.Name, _OldDate, _OldWarehouseID, _
                Nothing, LimitationsDataSource)

            ValidationRules.CheckRules()

        End Sub

        Friend Sub GetDiscardList()

            If _GoodsInfo.AccountingMethod = GoodsAccountingMethod.Periodic Then Exit Sub

            Dim consignements As ConsignmentPersistenceObjectList = _
                ConsignmentPersistenceObjectList.GetConsignmentPersistenceObjectList( _
                _GoodsInfo.ID, _Warehouse.ID, _ID, 0, (_GoodsInfo.ValuationMethod = GoodsValuationMethod.LIFO))

            consignements.RemoveLateEntries(_Date)

            Dim discards As ConsignmentDiscardPersistenceObjectList = _
                ConsignmentDiscardPersistenceObjectList.NewConsignmentDiscardPersistenceObjectList( _
                consignements, _Amount, _GoodsInfo.Name)

            If Not IsNew Then

                Dim curDiscards As ConsignmentDiscardPersistenceObjectList = _
                    ConsignmentDiscardPersistenceObjectList. _
                    GetConsignmentDiscardPersistenceObjectList(_ID)

                curDiscards.MergeChangedList(discards)

                DiscardList = curDiscards

            Else

                DiscardList = discards

            End If

            _TotalCost = DiscardList.GetTotalValue
            _Amount = DiscardList.GetTotalAmount
            If Not CRound(_Amount, 6) > 0 Then Throw New Exception( _
                "Klaida. Kiekis negali būti lygus nuliui metode SetCostValues.")
            _UnitCost = CRound(_TotalCost / _Amount, 6)

        End Sub

#End Region

    End Class

End Namespace