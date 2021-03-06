Imports ApskaitaObjects.Attributes

Namespace Workers

    ''' <summary>
    ''' Represents an imprest calculation for a particular labour contract for a particular month.
    ''' </summary>
    ''' <remarks>Should only be used as a child of a <see cref="ImprestItemList">ImprestItemList</see>.
    ''' Values are stored in the database table d_avansai_d.</remarks>
    <Serializable()> _
    Public NotInheritable Class ImprestItem
        Inherits BusinessBase(Of ImprestItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private ReadOnly _Guid As Guid = Guid.NewGuid
        Private _ID As Integer = 0
        Private _FinancialDataCanChange As Boolean = True
        Private _PersonID As Integer = 0
        Private _PersonName As String = ""
        Private _PersonCode As String = ""
        Private _PersonCodeSodra As String = ""
        Private _ContractSerial As String = ""
        Private _ContractNumber As Integer = 0
        Private _IsChecked As Boolean = False
        Private _PayOffSumTotal As Double = 0
        Private _PayedOutDate As Csla.SmartDate = New Csla.SmartDate(False)


        ''' <summary>
        ''' Gets an ID of the imprest calculation item that is assigned by a database (AUTOINCREMENT).
        ''' </summary>
        ''' <remarks>Value is stored in the database table d_avansai_d.ID.</remarks>
        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Returnes TRUE if the parent imprest sheet allows financial changes due to business restrains.
        ''' </summary>
        ''' <remarks>Chronologic business restrains are provided by a <see cref="SheetChronologicValidator">SheetChronologicValidator</see>.</remarks>
        Public ReadOnly Property FinancialDataCanChange() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _FinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Gets an <see cref="General.Person.ID">ID of the person</see> (worker) that is assigned to the current labour contract.
        ''' </summary>
        ''' <remarks>Value is stored in the database table d_avansai_d.AK.</remarks>
        Public ReadOnly Property PersonID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _PersonID
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="General.Person.Name">name of the person</see> (worker) that is assigned to the current labour contract.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="General.Person.Name">General.Person.Name</see>.</remarks>
        Public ReadOnly Property PersonName() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _PersonName.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="General.Person.Code">personal code of the person</see> (worker) that is assigned to the current labour contract.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="General.Person.Code">General.Person.Code</see>.</remarks>
        Public ReadOnly Property PersonCode() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _PersonCode.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="General.Person.CodeSODRA">social security code of the person</see> (worker) that is assigned to the current labour contract.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="General.Person.CodeSODRA">General.Person.CodeSODRA</see>.</remarks>
        Public ReadOnly Property PersonCodeSodra() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _PersonCodeSodra.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets the serial number (code) of the current labour contract.
        ''' </summary>
        ''' <remarks>A labour contract is identified by a <see cref="WageItem.ContractSerial">serial number</see> 
        ''' and <see cref="WageItem.ContractNumber">running number</see> pair.
        ''' Value is stored in the database table d_avansai_d.DS_Serija.</remarks>
        Public ReadOnly Property ContractSerial() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ContractSerial.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets the running number of the current labour contract.
        ''' </summary>
        ''' <remarks>A labour contract is identified by a <see cref="WageItem.ContractSerial">serial number</see> 
        ''' and <see cref="WageItem.ContractNumber">running number</see> pair.
        ''' Value is stored in the database table d_avansai_d.DS_NR.</remarks>
        Public ReadOnly Property ContractNumber() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ContractNumber
            End Get
        End Property

        ''' <summary>
        ''' Whether to include the item in the imprest sheet.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property IsChecked() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsChecked
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If Not _FinancialDataCanChange Then Exit Property
                If _IsChecked <> value Then
                    _IsChecked = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' The amount of the imprest to pay.
        ''' </summary>
        ''' <remarks>Value is stored in the database table d_avansai_d.Suma.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, 2)> _
        Public Property PayOffSumTotal() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_PayOffSumTotal)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If Not _FinancialDataCanChange Then Exit Property
                If CRound(_PayOffSumTotal) <> CRound(value) Then
                    _PayOffSumTotal = CRound(value)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the date when the imprest within the current item has been payed.
        ''' Use an empty string if the imprest is not payed.
        ''' </summary>
        ''' <remarks>Value is stored in the database table d_avansai_d.Ismok.</remarks>
        Public Property PayedOutDate() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _PayedOutDate.ToString()
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If Not _FinancialDataCanChange Then Exit Property
                If value Is Nothing Then value = ""
                If _PayedOutDate <> New Csla.SmartDate(value.Trim, False).Date Then
                    _PayedOutDate = New Csla.SmartDate(value.Trim, False)
                    PropertyHasChanged()
                End If
            End Set
        End Property



        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString
            If IsValid Then Return ""
            Return String.Format(My.Resources.Common_ErrorInItem, Me.ToString, _
                vbCrLf, Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error))
        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString
            If BrokenRulesCollection.WarningCount < 1 Then Return ""
            Return String.Format(My.Resources.Common_WarningInItem, Me.ToString, _
                vbCrLf, Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning))
        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            Return String.Format(My.Resources.Workers_ImprestItem_ToString, _PersonName, _
                _ContractSerial, _ContractNumber)
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf DoubleRequiredWhenCheckedValidation, _
                New Csla.Validation.RuleArgs("PayOffSumTotal"))

            ValidationRules.AddDependantProperty("IsChecked", "PayOffSumTotal", False)

        End Sub

        ''' <summary>
        ''' Rule ensuring imprest amount is entered.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function DoubleRequiredWhenCheckedValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            If Not DirectCast(target, ImprestItem).IsChecked Then Return True

            Return DoubleFieldValidation(target, e)

        End Function


#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

#End Region

#Region " Factory Methods "

        Friend Shared Function NewImprestItem(ByVal dr As DataRow) As ImprestItem
            Return New ImprestItem(dr)
        End Function

        Friend Shared Function GetImprestItem(ByVal dr As DataRow, _
            ByVal nFinancialDataCanChange As Boolean) As ImprestItem
            Return New ImprestItem(dr, nFinancialDataCanChange)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub


        Private Sub New(ByVal dr As DataRow)
            MarkAsChild()
            Create(dr)
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal nFinancialDataCanChange As Boolean)
            MarkAsChild()
            Fetch(dr, nFinancialDataCanChange)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Create(ByVal dr As DataRow)

            _ID = CIntSafe(dr.Item(0), 0)

            _PersonID = CIntSafe(dr.Item(0), 0)
            _PersonName = CStrSafe(dr.Item(1)).Trim
            _PersonCode = CStrSafe(dr.Item(2)).Trim
            _PersonCodeSodra = CStrSafe(dr.Item(3)).Trim
            _ContractSerial = CStrSafe(dr.Item(4)).Trim
            _ContractNumber = CIntSafe(dr.Item(5), 0)

            _FinancialDataCanChange = True
            _PayOffSumTotal = 0
            _ID = -1
            _IsChecked = False

            MarkNew()

            ValidationRules.CheckRules()

        End Sub

        Private Sub Fetch(ByVal dr As DataRow, ByVal nFinancialDataCanChange As Boolean)

            _ID = CIntSafe(dr.Item(0), 0)

            _PersonID = CIntSafe(dr.Item(0), 0)
            _PersonName = CStrSafe(dr.Item(1)).Trim
            _PersonCode = CStrSafe(dr.Item(2)).Trim
            _PersonCodeSodra = CStrSafe(dr.Item(3)).Trim
            _ContractSerial = CStrSafe(dr.Item(4)).Trim
            _ContractNumber = CIntSafe(dr.Item(5), 0)
            _PayOffSumTotal = CDblSafe(dr.Item(6), 2, 0)
            If CDateSafe(dr.Item(7), Date.MaxValue) <> Date.MaxValue Then
                _PayedOutDate = New Csla.SmartDate(CDateSafe(dr.Item(7), Date.MinValue), False)
            End If
            _ID = CIntSafe(dr.Item(8), 0)

            _FinancialDataCanChange = nFinancialDataCanChange

            _IsChecked = True

            MarkOld()

            ValidationRules.CheckRules()

        End Sub

        Friend Sub Insert(ByVal parent As ImprestSheet)

            Dim myComm As New SQLCommand("InsertImprestItem")
            myComm.AddParam("?PD", _PersonID)
            myComm.AddParam("?DS", _ContractSerial.Trim)
            myComm.AddParam("?DN", _ContractNumber)
            myComm.AddParam("?AD", parent.ID)
            myComm.AddParam("?SM", CRound(_PayOffSumTotal))
            If _PayedOutDate.IsEmpty Then
                myComm.AddParam("?PO", Nothing, GetType(Date))
            Else
                myComm.AddParam("?PO", _PayedOutDate.Date)
            End If

            myComm.Execute()

            _ID = Convert.ToInt32(myComm.LastInsertID)

            MarkOld()

        End Sub

        Friend Sub Update(ByVal parent As ImprestSheet)

            Dim myComm As New SQLCommand("UpdateImprestItem")
            myComm.AddParam("?NR", _ID)
            myComm.AddParam("?SM", CRound(_PayOffSumTotal))
            If _PayedOutDate.IsEmpty Then
                myComm.AddParam("?PO", Nothing, GetType(Date))
            Else
                myComm.AddParam("?PO", _PayedOutDate.Date)
            End If

            myComm.Execute()

            MarkOld()

        End Sub

        Friend Sub DeleteSelf()

            If _PayedOutDate <> Date.MaxValue Then Exit Sub

            Dim myComm As New SQLCommand("DeleteImprestItem")
            myComm.AddParam("?NR", _ID)

            myComm.Execute()

            MarkNew()

        End Sub

#End Region

    End Class

End Namespace