Imports System.Runtime.InteropServices
Imports ApskaitaObjects.Attributes
Imports Csla.Validation
Imports ApskaitaObjects.Settings.XmlProxies

Namespace Settings

    ''' <summary>
    ''' Represents a rate of some tax.
    ''' </summary>
    ''' <remarks>Should only be used as a child of <see cref="TaxRateList">TaxRateList</see>.
    ''' Persisted using xml proxies as a part of <see cref="CommonSettings">CommonSettings</see>.</remarks>
    <Serializable()> _
    Public NotInheritable Class TaxRate
        Inherits BusinessBase(Of TaxRate)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private ReadOnly _Guid As Guid = Guid.NewGuid
        Private _TaxType As TaxRateType = TaxRateType.Vat
        Private _TaxRate As Double = 0
        Private _IsObsolete As Boolean = False

        ''' <summary>
        ''' Gets or sets a type of the tax.
        ''' </summary>
        ''' <value></value>
        Public Property TaxType() As TaxRateType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _TaxType
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As TaxRateType)
                If _TaxType <> value Then
                    _TaxType = value
                    PropertyHasChanged()
                    PropertyHasChanged("TaxTypeHumanReadable")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a type of the tax as a human readable (localized) string.
        ''' </summary>
        ''' <value></value>
        <LocalizedEnumField(GetType(TaxRateType), False, "")> _
        Public Property TaxTypeHumanReadable() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Utilities.ConvertLocalizedName(_TaxType)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                Dim typedValue As TaxRateType = TaxRateType.Vat
                Try
                    typedValue = Utilities.ConvertLocalizedName(Of TaxRateType)(value)
                Catch ex As Exception
                End Try
                If _TaxType <> typedValue Then
                    _TaxType = typedValue
                    PropertyHasChanged()
                    PropertyHasChanged("TaxType")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a tax rate.
        ''' </summary>
        ''' <remarks></remarks>
        <DoubleField(ValueRequiredLevel.Recommended, False, 2, True, 0.01, 99.0, False)> _
        Public Property TaxRate() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _TaxRate
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                If CRound(_TaxRate) <> CRound(value) Then
                    _TaxRate = CRound(value)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether the tax rate is obsolete, not longer in use.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property IsObsolete() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsObsolete
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                If _IsObsolete <> value Then
                    _IsObsolete = value
                    PropertyHasChanged()
                End If
            End Set
        End Property


        Public Function HasWarnings() As Boolean
            Return (Me.BrokenRulesCollection.WarningCount > 0)
        End Function

        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString
            If IsValid Then Return ""
            Return String.Format(My.Resources.Common_ErrorInItem, Me.ToString, _
                vbCrLf, Me.BrokenRulesCollection.ToString(RuleSeverity.Error))
        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString
            If BrokenRulesCollection.WarningCount < 1 Then Return ""
            Return String.Format(My.Resources.Common_WarningInItem, Me.ToString, _
                vbCrLf, Me.BrokenRulesCollection.ToString(RuleSeverity.Warning))
        End Function



        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            Return String.Format(My.Resources.HelperLists_TaxRateInfo_ToString, _
                Me.TaxTypeHumanReadable, DblParser(_TaxRate, 2))
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()
            ValidationRules.AddRule(AddressOf CommonValidation.CommonValidation.DoubleFieldValidation, _
                New RuleArgs("TaxRate"))
        End Sub

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

#End Region

#Region " Factory Methods "

        Friend Shared Function NewTaxRate() As TaxRate
            Dim result As New TaxRate
            result.ValidationRules.CheckRules()
            Return result
        End Function

        Friend Shared Function GetTaxRate(ByVal proxy As TaxRateProxy) As TaxRate
            Return New TaxRate(proxy)
        End Function


        Private Sub New()
            MarkAsChild()
        End Sub

        Private Sub New(ByVal proxy As TaxRateProxy)
            MarkAsChild()
            Fetch(proxy)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Fetch(ByVal proxy As TaxRateProxy)

            _TaxType = proxy.TaxType
            _TaxRate = proxy.TaxRate
            _IsObsolete = proxy.IsObsolete

            MarkOld()

            ValidationRules.CheckRules()

        End Sub

        Friend Function GetProxy(ByVal markItemAsOld As Boolean) As TaxRateProxy

            Dim result As New TaxRateProxy

            result.TaxType = _TaxType
            result.TaxRate = _TaxRate
            result.IsObsolete = _IsObsolete

            If markItemAsOld Then MarkOld()

            Return result

        End Function

#End Region

    End Class

End Namespace