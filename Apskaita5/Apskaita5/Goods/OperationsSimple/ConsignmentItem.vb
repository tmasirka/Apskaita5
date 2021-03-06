Imports Csla.Validation
Imports ApskaitaObjects.My.Resources

Namespace Goods

    ''' <summary>
    ''' Represents a consignment value modification operation, i.e. discards
    ''' an available (not already discarded) consignment and creates a new 
    ''' modified consignment (with a reduced or increased value).
    ''' </summary>
    ''' <remarks>Used as a single operation or a part of <see cref="ConsignmentItemList">
    ''' operations collection</see> by goods operations that modify goods acquisition value,
    ''' e.g. <see cref="GoodsOperationAdditionalCosts">GoodsOperationAdditionalCosts</see>,
    ''' <see cref="GoodsOperationDiscount">GoodsOperationDiscount</see>.
    ''' Values are stored using a <see cref="ConsignmentPersistenceObject">ConsignmentPersistenceObject</see>
    ''' and a <see cref="ConsignmentDiscardPersistenceObject">ConsignmentDiscardPersistenceObject</see>.</remarks>
    <Serializable()> _
    Public NotInheritable Class ConsignmentItem
        Inherits BusinessBase(Of ConsignmentItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private _ID As Integer = 0
        Private _ParentID As Integer = 0
        Private _AcquisitionID As Integer = 0
        Private _WarehouseID As Integer = 0
        Private _UpdatedConsignmentID As Integer = 0
        Private _ConsignmentDiscardID As Integer = 0
        Private _Amount As Double = 0
        Private _UnitValue As Double = 0
        Private _TotalValue As Double = 0
        Private _AmountWithdrawn As Double = 0
        Private _TotalValueWithdrawn As Double = 0
        Private _AmountLeft As Double = 0
        Private _TotalValueLeft As Double = 0
        Private _WarehouseName As String = ""
        Private _AcquisitionDate As Date = Today
        Private _AcquisitionDocNo As String = ""
        Private _AcquisitionDocType As DocumentType = DocumentType.None
        Private _AcquisitionDocTypeHumanReadable As String = ""
        Private _UnitValueChange As Double = 0
        Private _TotalValueChange As Double = 0
        Private _ChangeIsNegative As Boolean = False
        Private _FinancialDataCanChange As Boolean = True


        ''' <summary>
        ''' Gets an <see cref="ConsignmentPersistenceObject.ID">ID of the replaceable 
        ''' consignment</see>.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.ID">ConsignmentPersistenceObject.ID</see>.</remarks>
        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Whether the change of the operation financial data is restricted by
        ''' the parent goods operation.
        ''' </summary>
        ''' <remarks>Is set when the operation is fetched or created.</remarks>
        Public ReadOnly Property FinancialDataCanChange() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _FinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Gets an <see cref="ConsignmentPersistenceObject.ParentID">ID of the
        ''' goods operation</see> that is a parent of the replaceable consignment.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.ParentID">ConsignmentPersistenceObject.ParentID</see>.</remarks>
        Public ReadOnly Property ParentID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ParentID
            End Get
        End Property

        ''' <summary>
        ''' Gets an <see cref="GoodsOperationAcquisition.ID">ID of the
        ''' goods acquisition operation</see> that the replaceable consignment 
        ''' was (originaly) acquired by.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.AcquisitionID">ConsignmentPersistenceObject.AcquisitionID</see>.</remarks>
        Public ReadOnly Property AcquisitionID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AcquisitionID
            End Get
        End Property

        ''' <summary>
        ''' Gets an original acquisition date of the replaceable consignment.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.AcquisitionID">ConsignmentPersistenceObject.AcquisitionID</see>.</remarks>
        Public ReadOnly Property AcquisitionDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AcquisitionDate
            End Get
        End Property

        ''' <summary>
        ''' Gets a number of the document that the replaceable consignment was originaly 
        ''' acquired by.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.AcquisitionID">ConsignmentPersistenceObject.AcquisitionID</see>.</remarks>
        Public ReadOnly Property AcquisitionDocNo() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AcquisitionDocNo.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a type of the document that the replaceable consignment was originaly 
        ''' acquired by.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.AcquisitionID">ConsignmentPersistenceObject.AcquisitionID</see>.</remarks>
        Public ReadOnly Property AcquisitionDocType() As DocumentType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AcquisitionDocType
            End Get
        End Property

        ''' <summary>
        ''' Gets a type of the document, that the replaceable consignment was originaly 
        ''' acquired by, as a localized human readable string.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.AcquisitionID">ConsignmentPersistenceObject.AcquisitionID</see>.</remarks>
        Public ReadOnly Property AcquisitionDocTypeHumanReadable() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AcquisitionDocTypeHumanReadable.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets an <see cref="Warehouse.ID">ID of the warehouse</see> where 
        ''' the replaceable (and modified) consignment is located.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.WarehouseID">ConsignmentPersistenceObject.WarehouseID</see>.</remarks>
        Public ReadOnly Property WarehouseID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WarehouseID
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="Warehouse.Name">name of the warehouse</see> where 
        ''' the replaceable (and modified) consignment is located.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.WarehouseID">ConsignmentPersistenceObject.WarehouseID</see>.</remarks>
        Public ReadOnly Property WarehouseName() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WarehouseName.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets an <see cref="ConsignmentPersistenceObject.ID">ID of the modified 
        ''' consignment</see>.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.ID">ConsignmentPersistenceObject.ID</see>.</remarks>
        Public ReadOnly Property UpdatedConsignmentID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UpdatedConsignmentID
            End Get
        End Property

        ''' <summary>
        ''' Gets an <see cref="ConsignmentDiscardPersistenceObject.ID">ID of the 
        ''' consignment discard operation</see> that discards the replaceable consignment.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentDiscardPersistenceObject.ID">ConsignmentDiscardPersistenceObject.ID</see>.</remarks>
        Public ReadOnly Property ConsignmentDiscardID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ConsignmentDiscardID
            End Get
        End Property

        ''' <summary>
        ''' Gets an (original) acquisition amount of the goods in the replaceable consignment.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.Amount">ConsignmentPersistenceObject.Amount</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, ROUNDAMOUNTGOODS)> _
        Public ReadOnly Property Amount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Amount, ROUNDAMOUNTGOODS)
            End Get
        End Property

        ''' <summary>
        ''' Gets a unit value of the goods in the replaceable consignment.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.UnitValue">ConsignmentPersistenceObject.UnitValue</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, ROUNDUNITGOODS)> _
        Public ReadOnly Property UnitValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitValue, ROUNDUNITGOODS)
            End Get
        End Property

        ''' <summary>
        ''' Gets an original total value of the goods in the replaceable consignment
        ''' (that was acquired originaly).
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.TotalValue">ConsignmentPersistenceObject.TotalValue</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, 2)> _
        Public ReadOnly Property TotalValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalValue, 2)
            End Get
        End Property

        ''' <summary>
        ''' Gets an amount of the goods that is already withdrawn 
        ''' from the replaceable consignment.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.AmountWithdrawn">ConsignmentPersistenceObject.AmountWithdrawn</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDAMOUNTGOODS)> _
        Public ReadOnly Property AmountWithdrawn() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_AmountWithdrawn, ROUNDAMOUNTGOODS)
            End Get
        End Property

        ''' <summary>
        ''' Gets a total value of the goods that is already withdrawn 
        ''' from the replaceable consignment.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.TotalValueWithdrawn">ConsignmentPersistenceObject.TotalValueWithdrawn</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, 2)> _
        Public ReadOnly Property TotalValueWithdrawn() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalValueWithdrawn)
            End Get
        End Property

        ''' <summary>
        ''' Gets an available (not withdrawn) amount of the goods in the replaceable consignment.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.AmountLeft">ConsignmentPersistenceObject.AmountLeft</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, ROUNDAMOUNTGOODS)> _
        Public ReadOnly Property AmountLeft() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_AmountLeft, ROUNDAMOUNTGOODS)
            End Get
        End Property

        ''' <summary>
        ''' Gets an available (not withdrawn) total value of the goods 
        ''' in the replaceable consignment.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="ConsignmentPersistenceObject.TotalValueLeft">ConsignmentPersistenceObject.TotalValueLeft</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, 2)> _
        Public ReadOnly Property TotalValueLeft() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalValueLeft)
            End Get
        End Property

        ''' <summary>
        ''' Gets a goods value change per unit.
        ''' </summary>
        ''' <remarks>Value is calculated as <see cref="TotalValueChange">TotalValueChange</see>
        ''' divided by <see cref="AmountLeft">AmountLeft</see>. </remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDUNITGOODS)> _
        Public ReadOnly Property UnitValueChange() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitValueChange, ROUNDUNITGOODS)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a total value change. The sign of the change is subject to
        ''' the <see cref="ChangeIsNegative">ChangeIsNegative</see> value.
        ''' </summary>
        ''' <remarks></remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, 2)> _
        Public Property TotalValueChange() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalValueChange)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If Not _FinancialDataCanChange Then Exit Property
                If CRound(_TotalValueChange) <> CRound(value) AndAlso _AmountLeft > 0 Then
                    _TotalValueChange = CRound(value)
                    PropertyHasChanged()
                    _UnitValueChange = CRound(_TotalValueChange / _AmountLeft, ROUNDAMOUNTGOODS)
                    PropertyHasChanged("UnitValueChange")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Whether the <see cref="TotalValueChange">TotalValueChange</see>
        ''' decreases the value of the replaceable consignment.
        ''' </summary>
        ''' <remarks>Value is set when fetching or creating the operation.</remarks>
        Public ReadOnly Property ChangeIsNegative() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ChangeIsNegative
            End Get
        End Property



        Public Function GetAllBrokenRules() As String
            If IsValid Then Return ""
            Return BrokenRulesCollection.ToString(RuleSeverity.Error)
        End Function

        Public Function GetAllWarnings() As String
            If Not BrokenRulesCollection.WarningCount > 0 Then Return ""
            Return BrokenRulesCollection.ToString(RuleSeverity.Warning)
        End Function

        Public Function HasWarnings() As Boolean
            Return MyBase.BrokenRulesCollection.WarningCount > 0
        End Function

        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString
            If IsValid Then Return ""
            Return String.Format(My.Resources.Common_ErrorInItem, Me.ToString, _
                vbCrLf, Me.GetAllBrokenRules())
        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString
            If Not HasWarnings() Then Return ""
            Return String.Format(My.Resources.Common_WarningInItem, Me.ToString, _
                vbCrLf, Me.GetAllWarnings())
        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _ID
        End Function

        Public Overrides Function ToString() As String
            If Not _ID > 0 Then Return ""
            Return String.Format(My.Resources.Goods_ConsignmentItem_ToString, _
                _AcquisitionDate.ToString("yyyy-MM-dd"), _AcquisitionDocNo, _ID.ToString)
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf TotalValueChangeValidation, _
                New Validation.RuleArgs("TotalValueChange"))

        End Sub

        ''' <summary>
        ''' Rule ensuring that the value of property TotalValueChange is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function TotalValueChangeValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As ConsignmentItem = DirectCast(target, ConsignmentItem)

            If valObj._ChangeIsNegative AndAlso CRound(valObj._TotalValueChange) _
                >= CRound(valObj._TotalValueLeft) Then
                e.Description = Goods_ConsignmentItem_TotalValueChangeInvalid
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            Return CommonValidation.DoubleFieldValidation(target, e)

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets a new ConsignmentItem instance by a database query result.
        ''' </summary>
        ''' <param name="dr">a database query result containing the operation data</param>
        ''' <param name="changeIsNegative">whether the <see cref="TotalValueChange">
        ''' TotalValueChange</see> decreases the value of the replaceable consignment</param>
        ''' <param name="financialDataCanChange">whether the change of the operation 
        ''' financial data is restricted by the parent goods operation</param>
        ''' <remarks>Both existing and new (available) consignment value change
        ''' operations are fetched.</remarks>
        Friend Shared Function GetConsignmentItem(ByVal dr As DataRow, _
            ByVal changeIsNegative As Boolean, ByVal financialDataCanChange As Boolean) As ConsignmentItem
            Return New ConsignmentItem(dr, changeIsNegative, financialDataCanChange)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal nChangeIsNegative As Boolean, _
            ByVal nFinancialDataCanChange As Boolean)
            MarkAsChild()
            Fetch(dr, nChangeIsNegative, nFinancialDataCanChange)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Fetch(ByVal dr As DataRow, ByVal nChangeIsNegative As Boolean, _
            ByVal nFinancialDataCanChange As Boolean)

            _ID = CIntSafe(dr.Item(0), 0)
            _ParentID = CIntSafe(dr.Item(1), 0)
            _AcquisitionID = CIntSafe(dr.Item(2), 0)
            _WarehouseID = CIntSafe(dr.Item(3), 0)
            _Amount = CDblSafe(dr.Item(4), ROUNDAMOUNTGOODS, 0)
            _UnitValue = CDblSafe(dr.Item(5), ROUNDUNITGOODS, 0)
            _TotalValue = CDblSafe(dr.Item(6), 2, 0)
            _AmountWithdrawn = CDblSafe(dr.Item(7), ROUNDAMOUNTGOODS, 0)
            _TotalValueWithdrawn = CDblSafe(dr.Item(8), 2, 0)
            _WarehouseName = CStrSafe(dr.Item(9)).Trim
            _AcquisitionDate = CDateSafe(dr.Item(10), Today)
            _AcquisitionDocNo = CStrSafe(dr.Item(11)).Trim
            _AcquisitionDocType = ConvertDatabaseCharID(Of DocumentType)(CStrSafe(dr.Item(12)))
            _AcquisitionDocTypeHumanReadable = ConvertLocalizedName(_AcquisitionDocType)
            _AmountLeft = CRound(_Amount - _AmountWithdrawn, ROUNDAMOUNTGOODS)
            _TotalValueLeft = CRound(_TotalValue - _TotalValueWithdrawn, 2)

            _ChangeIsNegative = nChangeIsNegative

            ' an old operation (contains consignment discard)
            If CIntSafe(dr.Item(15), 0) > 0 Then

                If CDblSafe(dr.Item(13), 2, 0) > CDblSafe(dr.Item(14), 2, 0) Then
                    _TotalValueChange = CRound(CDblSafe(dr.Item(13), 2, 0) - CDblSafe(dr.Item(14), 2, 0))
                Else
                    _TotalValueChange = CRound(CDblSafe(dr.Item(14), 2, 0) - CDblSafe(dr.Item(13), 2, 0))
                End If
                If CRound(_AmountLeft, ROUNDAMOUNTGOODS) <> 0 Then
                    _UnitValueChange = CRound(_TotalValueChange / _AmountLeft, ROUNDUNITGOODS)
                Else
                    _UnitValueChange = 0
                End If
                _ConsignmentDiscardID = CIntSafe(dr.Item(15), 0)
                _UpdatedConsignmentID = CIntSafe(dr.Item(16), 0)

                MarkOld()

            End If

            _FinancialDataCanChange = nFinancialDataCanChange

            ValidationRules.CheckRules()

        End Sub

        Friend Sub Insert(ByVal parentID As Integer)

            Dim consignmentDiscard As ConsignmentDiscardPersistenceObject = _
                ConsignmentDiscardPersistenceObject.NewConsignmentDiscardPersistenceObject(Me)

            Dim updatedConsignment As ConsignmentPersistenceObject = _
                ConsignmentPersistenceObject.NewConsignmentPersistenceObject(Me)

            If _ChangeIsNegative Then
                updatedConsignment.TotalValue = CRound(_TotalValueLeft - _TotalValueChange)
                updatedConsignment.UnitValue = CRound(_UnitValue - _UnitValueChange, ROUNDUNITGOODS)
            Else
                updatedConsignment.TotalValue = CRound(_TotalValueLeft + _TotalValueChange)
                updatedConsignment.UnitValue = CRound(_UnitValue + _UnitValueChange, ROUNDUNITGOODS)
            End If

            updatedConsignment.Insert(parentID, _WarehouseID)
            _UpdatedConsignmentID = updatedConsignment.ID
            consignmentDiscard.Insert(parentID)
            _ConsignmentDiscardID = consignmentDiscard.ID

            MarkOld()

        End Sub

        Friend Sub Update(ByVal parentID As Integer)

            Dim updatedConsignment As ConsignmentPersistenceObject = _
                ConsignmentPersistenceObject.GetConsignmentPersistenceObject(Me, parentID)

            If _ChangeIsNegative Then
                updatedConsignment.TotalValue = CRound(_TotalValueLeft - _TotalValueChange)
                updatedConsignment.UnitValue = CRound(_UnitValue - _UnitValueChange, ROUNDUNITGOODS)
            Else
                updatedConsignment.TotalValue = CRound(_TotalValueLeft + _TotalValueChange)
                updatedConsignment.UnitValue = CRound(_UnitValue + _UnitValueChange, ROUNDUNITGOODS)
            End If

            updatedConsignment.Update(_WarehouseID)

            MarkOld()

        End Sub

        Friend Sub DeleteSelf()

            ConsignmentPersistenceObject.DeleteSelf(_UpdatedConsignmentID)
            ConsignmentDiscardPersistenceObject.DeleteSelf(_ConsignmentDiscardID)
            _UpdatedConsignmentID = 0
            _ConsignmentDiscardID = 0

            MarkNew()

        End Sub

#End Region

    End Class

End Namespace