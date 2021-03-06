Imports ApskaitaObjects.Attributes

Namespace Workers

    ''' <summary>
    ''' Represents a general (standard/normal) type of work hours 
    ''' for a specific labour contract for a specific month.
    ''' </summary>
    ''' <remarks>Should only be used as a child of <see cref="WorkTimeItemList">WorkTimeItemList</see>.
    ''' Values are stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object
    ''' and a databse table worktimeitems.</remarks>
    <Serializable()> _
    Public NotInheritable Class WorkTimeItem
        Inherits BusinessBase(Of WorkTimeItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private ReadOnly _Guid As Guid = Guid.NewGuid
        Private _IsChecked As Boolean = True
        Private _ID As Integer = 0
        Private _WorkerID As Integer = 0
        Private _Worker As String = ""
        Private _WorkerPosition As String = ""
        Private _WorkerLoad As Double = 0
        Private _ContractSerial As String = ""
        Private _ContractNumber As Integer = 0
        Private _QuotaDays As Integer = 0
        Private _QuotaHours As Double = 0
        Private _TotalDays As Integer = 0
        Private _TotalHours As Double = 0
        Private _DayList As DayWorkTimeList


        ''' <summary>
        ''' Gets an ID of the special work time item that is assigned by a database (AUTOINCREMENT).
        ''' </summary>
        ''' <remarks>Value is stored in the database table worktimeitems.ID.</remarks>
        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Gets an ID of the worker.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="General.Person.ID">Person.ID</see>.
        ''' Value is stored in the database table SpecialWorkTimeItems.WorkerID.</remarks>
        Public ReadOnly Property WorkerID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WorkerID
            End Get
        End Property

        ''' <summary>
        ''' Gets a name of the worker.
        ''' </summary>
        ''' <remarks>Corresponds to <see cref="General.Person.Name">Person.Name</see>.</remarks>
        Public ReadOnly Property Worker() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Worker.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets the current worker position as specified in the current labour contract.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property WorkerPosition() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WorkerPosition.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets the current worker work load (ratio between contractual work hours and gauge work hours (40H/Week)) as specified in the current labour contract.
        ''' </summary>
        ''' <remarks></remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKLOAD)> _
        Public ReadOnly Property WorkerLoad() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_WorkerLoad, ROUNDWORKLOAD)
            End Get
        End Property

        ''' <summary>
        ''' Gets the serial number (code) of the current labour contract.
        ''' </summary>
        ''' <remarks>A labour contract is identified by a <see cref="SpecialWorkTimeItem.ContractSerial">serial number</see> 
        ''' and <see cref="SpecialWorkTimeItem.ContractNumber">running number</see> pair.
        ''' Value is stored in the database table worktimeitems.ContractSerial.</remarks>
        Public ReadOnly Property ContractSerial() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ContractSerial.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets the running number of the current labour contract.
        ''' </summary>
        ''' <remarks>A labour contract is identified by a <see cref="SpecialWorkTimeItem.ContractSerial">serial number</see> 
        ''' and <see cref="SpecialWorkTimeItem.ContractNumber">running number</see> pair.
        ''' Value is stored in the database table worktimeitems.ContractNumber.</remarks>
        Public ReadOnly Property ContractNumber() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ContractNumber
            End Get
        End Property


        ''' <summary>
        ''' Whether to include the item in the work time sheet.
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
                If _IsChecked <> value Then
                    _IsChecked = value
                    PropertyHasChanged()
                End If
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets a number of work days for the worker's schedule for the month.
        ''' </summary>
        ''' <remarks>Value is stored in the database table SpecialWorkTimeItems.QuotaDays.</remarks>
        <IntegerField(ValueRequiredLevel.Mandatory, False, True, 0, 31, True)> _
        Public Property QuotaDays() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _QuotaDays
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If _QuotaDays <> value Then
                    _QuotaDays = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a number of work hours for the worker's schedule for the month.
        ''' </summary>
        ''' <remarks>Value is stored in the database table SpecialWorkTimeItems.QuotaHours.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, ROUNDWORKHOURS, True, 0.0, 744.0, True)> _
        Public Property QuotaHours() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_QuotaHours, ROUNDWORKHOURS)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If CRound(_QuotaHours, ROUNDWORKHOURS) <> CRound(value, ROUNDWORKHOURS) Then
                    _QuotaHours = CRound(value, ROUNDWORKHOURS)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 1 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day1() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(1).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(1, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 1 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType1() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(1).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(1, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day1")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 2 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day2() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(2).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(2, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 2 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType2() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
             Get
                Return _DayList.GetItemForDay(2).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(2, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day2")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 3 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day3() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(3).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(3, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 3 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType3() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(3).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(3, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day3")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 4 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day4() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(4).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(4, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 4 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType4() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(4).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(4, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day4")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 5 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day5() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(5).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(5, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 5 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType5() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(5).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(5, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day5")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 6 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day6() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(6).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(6, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 6 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType6() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(6).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(6, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day6")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 7 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day7() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(7).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(7, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 7 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType7() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(7).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(7, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day7")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 8 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day8() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(8).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(8, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 8 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType8() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(8).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(8, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day8")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 9 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day9() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(9).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(9, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 9 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType9() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(9).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(9, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day9")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 10 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day10() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(10).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(10, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 10 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType10() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(10).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(10, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day10")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 11 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day11() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(11).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(11, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 11 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType11() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(11).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(11, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day11")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 12 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day12() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(12).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(12, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 12 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType12() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(12).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(12, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day12")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 13 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day13() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(13).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(13, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 13 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType13() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(13).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(13, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day13")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 14 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day14() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(14).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(14, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 14 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType14() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(14).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(14, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day14")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 15 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
          Public Property Day15() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(15).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(15, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 15 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType15() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(15).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(15, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day15")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 16 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day16() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(16).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(16, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 16 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType16() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(16).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(16, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day16")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 17 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day17() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(17).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(17, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 17 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType17() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(17).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(17, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day17")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 18 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day18() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(18).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(18, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 18 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType18() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(18).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(18, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day18")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 19 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day19() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(19).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(19, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 19 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType19() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(19).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(19, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day19")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 20of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day20() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(20).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(20, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 20 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType20() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(20).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(20, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day20")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 21 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day21() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(21).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(21, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 21 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType21() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(21).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(21, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day21")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 22 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day22() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(22).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(22, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 22 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType22() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(22).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(22, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day22")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 23 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day23() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(23).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(23, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 23 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType23() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(23).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(23, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day23")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 24 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day24() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(24).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(24, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 24 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType24() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(24).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(24, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day24")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 25 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day25() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(25).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(25, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 25 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType25() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(25).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(25, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day25")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 26 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day26() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(26).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(26, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 26 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType26() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(26).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(26, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day26")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 27 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day27() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(27).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(27, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 27 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType27() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(27).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(27, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day27")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 28 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day28() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(28).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(28, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 28 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType28() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(28).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(28, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day28")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 29 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day29() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(29).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(29, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 29 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType29() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(29).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(29, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day29")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 30 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day30() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(30).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(30, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 30 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType30() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(30).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(30, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day30")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the number of work hours for the day 31 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, ROUNDWORKHOURS, True, 0.0, 24.0, True)> _
        Public Property Day31() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(31).Length
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If _DayList.TrySetLengthForDay(31, value) Then
                    PropertyHasChanged()
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of work and (or) rest hours for the day 31 of the month.
        ''' </summary>
        ''' <remarks>Value is stored by an encapsulated <see cref="DayWorkTimeList">DayWorkTimeList</see> object.</remarks>
        <WorkTimeClassField(ValueRequiredLevel.Recommended, False, True)> _
        Public Property DayType31() As WorkTimeClassInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DayList.GetItemForDay(31).Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WorkTimeClassInfo)
                CanWriteProperty(True)
                If _DayList.TrySetTypeForDay(31, value) Then
                    PropertyHasChanged()
                    PropertyHasChanged("Day31")
                    RecalculateTotals(True)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets a total number of work days within the item.
        ''' </summary>
        ''' <remarks>Value is stored in the database table SpecialWorkTimeItems.TotalDays.</remarks>
        <IntegerField(ValueRequiredLevel.Recommended, False, True, 0, 31, True)> _
        Public ReadOnly Property TotalDays() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _TotalDays
            End Get
        End Property

        ''' <summary>
        ''' Gets a total number of work hours within the item.
        ''' </summary>
        ''' <remarks>Value is stored in the database table SpecialWorkTimeItems.TotalHours.</remarks>
        <DoubleField(ValueRequiredLevel.Recommended, False, ROUNDWORKHOURS, True, 0.0, 744.0, True)> _
        Public ReadOnly Property TotalHours() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalHours, ROUNDWORKHOURS)
            End Get
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


        Private Sub RecalculateTotals(ByVal raisePropertyHasChanged As Boolean)
            _TotalDays = _DayList.GetTotalWorkDays
            _TotalHours = _DayList.GetTotalWorkHours
            If raisePropertyHasChanged Then
                PropertyHasChanged("TotalDays")
                PropertyHasChanged("TotalHours")
            End If
        End Sub

        Friend Function GetTotalAbsenceDays(ByVal defaultRestTimeClass As WorkTimeClassInfo, _
            ByVal defaultPublicHolidaysClass As WorkTimeClassInfo) As Integer
            Return _DayList.GetTotalAbsenceDays(defaultRestTimeClass, defaultPublicHolidaysClass)
        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            Return String.Format(My.Resources.Workers_WorkTimeItem_ToString, _
                _Worker, _ContractSerial, _ContractNumber.ToString)
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.IntegerFieldValidation, _
                New Csla.Validation.RuleArgs("QuotaDays"))
            ValidationRules.AddRule(AddressOf CommonValidation.DoubleFieldValidation, _
                New Csla.Validation.RuleArgs("QuotaHours"))

            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day1"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day2"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day3"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day4"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day5"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day6"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day7"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day8"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day9"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day10"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day11"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day12"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day13"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day14"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day15"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day16"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day17"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day18"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day19"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day20"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day21"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day22"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day23"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day24"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day25"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day26"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day27"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day28"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day29"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day30"))
            ValidationRules.AddRule(AddressOf DayValueValidation, New Validation.RuleArgs("Day31"))

            ValidationRules.AddRule(AddressOf CommonValidation.CommonValidation.IntegerFieldValidation, _
                New Csla.Validation.RuleArgs("TotalDays"))
            ValidationRules.AddRule(AddressOf CommonValidation.CommonValidation.DoubleFieldValidation, _
                New Csla.Validation.RuleArgs("TotalHours"))

        End Sub

        ''' <summary>
        ''' Rule ensuring that the value of property Day n is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function DayValueValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As WorkTimeItem = DirectCast(target, WorkTimeItem)

            Dim dayNumber As Integer = 0
            If Not Integer.TryParse(e.PropertyName.Substring(3), dayNumber) _
                OrElse Not dayNumber > 0 Then Return True
            Dim curDayItem As DayWorkTime = valObj._DayList.GetItemForDay(dayNumber)
            If curDayItem Is Nothing Then Return True

            If Not curDayItem.IsValid Then
                e.Description = curDayItem.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error)
                e.Severity = Validation.RuleSeverity.Error
                Return False
            ElseIf curDayItem.BrokenRulesCollection.WarningCount > 0 Then
                e.Description = curDayItem.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning)
                e.Severity = Validation.RuleSeverity.Warning
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

        Friend Shared Function NewWorkTimeItem(ByVal dr As DataRow, _
            ByVal cYear As Integer, ByVal cMonth As Integer, _
            ByVal restDayInfo As WorkTimeClassInfo, ByVal publicHolydaysInfo As WorkTimeClassInfo, _
            ByVal cWorkTime As DefaultWorkTimeInfo, _
            ByVal cWorkTimeList As DefaultWorkTimeInfoList) As WorkTimeItem
            Return New WorkTimeItem(dr, cYear, cMonth, restDayInfo, _
                publicHolydaysInfo, cWorkTime, cWorkTimeList)
        End Function

        Friend Shared Function GetWorkTimeItem(ByVal dr As DataRow, _
            ByVal dayWorkTimeDataTable As DataTable, ByVal cYear As Integer, _
            ByVal cMonth As Integer, ByVal cWorkTimeList As DefaultWorkTimeInfoList) As WorkTimeItem
            Return New WorkTimeItem(dr, dayWorkTimeDataTable, cYear, cMonth, cWorkTimeList)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal cYear As Integer, _
            ByVal cMonth As Integer, ByVal restDayInfo As WorkTimeClassInfo, _
            ByVal publicHolydaysInfo As WorkTimeClassInfo, _
            ByVal cWorkTime As DefaultWorkTimeInfo, _
            ByVal cWorkTimeList As DefaultWorkTimeInfoList)
            MarkAsChild()
            Create(dr, cYear, cMonth, restDayInfo, publicHolydaysInfo, cWorkTime, cWorkTimeList)
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal dayWorkTimeDataTable As DataTable, _
            ByVal cYear As Integer, ByVal cMonth As Integer, _
            ByVal cWorkTimeList As DefaultWorkTimeInfoList)
            MarkAsChild()
            Fetch(dr, dayWorkTimeDataTable, cYear, cMonth, cWorkTimeList)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Create(ByVal dr As DataRow, ByVal cYear As Integer, _
            ByVal cMonth As Integer, ByVal restDayInfo As WorkTimeClassInfo, _
            ByVal publicHolydaysInfo As WorkTimeClassInfo, _
            ByVal cWorkTime As DefaultWorkTimeInfo, _
            ByVal cWorkTimeList As DefaultWorkTimeInfoList)

            _WorkerID = CIntSafe(dr.Item(0), 0)
            _Worker = CStrSafe(dr.Item(1))
            _WorkerPosition = CStrSafe(dr.Item(2))
            _ContractSerial = CStrSafe(dr.Item(3))
            _ContractNumber = CIntSafe(dr.Item(4), 0)
            _WorkerLoad = CDblSafe(dr.Item(5), ROUNDWORKLOAD, 0)
            _DayList = DayWorkTimeList.NewDayWorkTimeList(cYear, cMonth, _WorkerLoad, _
                CDateSafe(dr.Item(6), Date.MinValue), CDateSafe(dr.Item(7), Date.MaxValue), _
                restDayInfo, publicHolydaysInfo, cWorkTimeList)
            _QuotaDays = cWorkTime.WorkDaysFor5WorkDayWeek
            _QuotaHours = cWorkTime.WorkHoursFor5WorkDayWeek

            RecalculateTotals(False)

            ValidationRules.CheckRules()

        End Sub

        Private Sub Fetch(ByVal dr As DataRow, ByVal dayWorkTimeDataTable As DataTable, _
            ByVal cYear As Integer, ByVal cMonth As Integer, _
            ByVal cWorkTimeList As DefaultWorkTimeInfoList)

            _ID = CIntSafe(dr.Item(0), 0)
            _WorkerID = CIntSafe(dr.Item(1), 0)
            _Worker = CStrSafe(dr.Item(2)).Trim
            _WorkerPosition = CStrSafe(dr.Item(3)).Trim
            _WorkerLoad = CDblSafe(dr.Item(4), ROUNDWORKLOAD, 0)
            _ContractSerial = CStrSafe(dr.Item(5)).Trim
            _ContractNumber = CIntSafe(dr.Item(6), 0)
            _QuotaDays = CIntSafe(dr.Item(7), 0)
            _QuotaHours = CDblSafe(dr.Item(8), ROUNDWORKHOURS, 0)
            _TotalDays = CIntSafe(dr.Item(9), 0)
            _TotalHours = CDblSafe(dr.Item(10), ROUNDWORKHOURS, 0)

            _DayList = DayWorkTimeList.GetDayWorkTimeList(dayWorkTimeDataTable, Me, _
                cYear, cMonth, cWorkTimeList)

            ValidationRules.CheckRules()

            MarkOld()

        End Sub

        Friend Sub Insert(ByVal parent As WorkTimeSheet)

            Dim myComm As New SQLCommand("InsertWorkTimeItem")
            AddWithParams(myComm)
            myComm.AddParam("?AA", _WorkerID)
            myComm.AddParam("?AB", _ContractSerial.Trim)
            myComm.AddParam("?AC", _ContractNumber)
            myComm.AddParam("?PD", parent.ID)

            myComm.Execute()

            _ID = Convert.ToInt32(myComm.LastInsertID)

            _DayList.Update(Me)

            MarkOld()

        End Sub

        Friend Sub Update(ByVal parent As WorkTimeSheet)

            Dim myComm As New SQLCommand("UpdateWorkTimeItem")
            myComm.AddParam("?CD", _ID)
            AddWithParams(myComm)

            myComm.Execute()

            _DayList.Update(Me)

            MarkOld()

        End Sub

        Friend Sub DeleteSelf()

            Dim myComm As New SQLCommand("DeleteWorkTimeItem")
            myComm.AddParam("?CD", _ID)

            myComm.Execute()

            myComm = New SQLCommand("DeleteDayWorkTimeListForWorkTimeItem")
            myComm.AddParam("?CD", _ID)

            myComm.Execute()

            MarkNew()

        End Sub

        Private Sub AddWithParams(ByRef myComm As SQLCommand)

            myComm.AddParam("?AD", _QuotaDays)
            myComm.AddParam("?AE", CRound(_QuotaHours, ROUNDWORKHOURS))
            myComm.AddParam("?AF", _TotalDays)
            myComm.AddParam("?AG", CRound(_TotalHours, ROUNDWORKHOURS))

        End Sub

#End Region

    End Class

End Namespace