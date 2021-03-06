Namespace DatabaseAccess.DatabaseStructure

    <Serializable()> _
Public Class DatabaseStoredProcedure
        Inherits BusinessBase(Of DatabaseStoredProcedure)

#Region " Business Methods "

        Private _GID As Guid = Guid.NewGuid
        Private _Name As String = ""
        Private _SourceCode As String = ""
        Private _SourceCodeComparable As String = ""
        Private _Description As String = ""


        Public Property Name() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Name.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Name.Trim <> value.Trim Then
                    _Name = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property SourceCode() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _SourceCode.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _SourceCode.Trim <> value.Trim Then
                    _SourceCode = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property SourceCodeComparable() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _SourceCodeComparable.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _SourceCodeComparable.Trim <> value.Trim Then
                    _SourceCodeComparable = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property Description() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Description.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Description.Trim <> value.Trim Then
                    _Description = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property


        Public Function GetDropProcedureStatement(ByVal DatabaseName As String, _
            ByVal SqlGenerator As SqlServerSpecificMethods.ISqlGenerator) As String
            Return SqlGenerator.GetDropProcedureStatement(DatabaseName, Me)
        End Function

        Public Function GetCreateProcedureStatement(ByVal DatabaseName As String, _
            ByVal SqlGenerator As SqlServerSpecificMethods.ISqlGenerator) As String
            Return SqlGenerator.GetCreateProcedureStatement(DatabaseName, Me)
        End Function

        Public Function GetUpdateProcedureStatement(ByVal DatabaseName As String, _
            ByVal SqlGenerator As SqlServerSpecificMethods.ISqlGenerator) As String
            Return SqlGenerator.GetUpdateProcedureStatement(DatabaseName, Me)
        End Function

        Public Shared Operator =(ByVal FirstDatabaseStoredProcedure As DatabaseStoredProcedure, _
            ByVal SecondDatabaseStoredProcedure As DatabaseStoredProcedure) As Boolean
            Return (FirstDatabaseStoredProcedure._Name.Trim.ToLower = _
                SecondDatabaseStoredProcedure._Name.Trim.ToLower AndAlso _
                FirstDatabaseStoredProcedure._SourceCodeComparable.Trim.ToLower = _
                SecondDatabaseStoredProcedure._SourceCodeComparable.Trim.ToLower)
        End Operator

        Public Shared Operator <>(ByVal FirstDatabaseStoredProcedure As DatabaseStoredProcedure, _
            ByVal SecondDatabaseStoredProcedure As DatabaseStoredProcedure) As Boolean
            Return (FirstDatabaseStoredProcedure._Name.Trim.ToLower <> _
                SecondDatabaseStoredProcedure._Name.Trim.ToLower OrElse _
                FirstDatabaseStoredProcedure._SourceCodeComparable.Trim.ToLower <> _
                SecondDatabaseStoredProcedure._SourceCodeComparable.Trim.ToLower)
        End Operator

        Protected Overrides Function GetIdValue() As Object
            Return _GID
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()
            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.StringRequiredRuleArgs("Name", "procedūros pavadinimas"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.StringRequiredRuleArgs("SourceCode", _
                "procedūros išeitinis kodas (source code)"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.StringRequiredRuleArgs("SourceCodeComparable", _
                "procedūros išeitinio kodo (source code) palyginamoji dalis (BEGIN ... END)"))
        End Sub

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
            AuthorizationRules.AllowWrite(Name_AdminRole)
        End Sub

#End Region

#Region " Factory Methods "

        Friend Shared Function NewDatabaseStoredProcedure() As DatabaseStoredProcedure
            Return New DatabaseStoredProcedure
        End Function

        Friend Shared Function GetDatabaseStoredProcedure(ByVal node As Xml.XmlNode) _
            As DatabaseStoredProcedure
            Return New DatabaseStoredProcedure(node)
        End Function

        Friend Shared Function GetDatabaseStoredProcedure(ByVal dr As DataRow, _
            ByVal SqlGenerator As SqlServerSpecificMethods.ISqlGenerator) As DatabaseStoredProcedure
            Return New DatabaseStoredProcedure(dr, SqlGenerator)
        End Function

        Private Sub New()
            MarkAsChild()
            ValidationRules.CheckRules()
        End Sub

        Private Sub New(ByVal node As Xml.XmlNode)
            MarkAsChild()
            Fetch(node)
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal SqlGenerator As SqlServerSpecificMethods.ISqlGenerator)
            MarkAsChild()
            Fetch(dr, SqlGenerator)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Fetch(ByVal node As Xml.XmlNode)

            _Name = node.Attributes.GetNamedItem("Name").Value
            _SourceCode = node.Attributes.GetNamedItem("SourceCode").Value
            Me._SourceCodeComparable = node.Attributes.GetNamedItem("SourceCodeComparable").Value
            _Description = node.Attributes.GetNamedItem("Description").Value

            MarkOld()

            ValidationRules.CheckRules()

        End Sub

        Private Sub Fetch(ByVal dr As DataRow, ByVal SqlGenerator As SqlServerSpecificMethods.ISqlGenerator)

            If Not SqlGenerator.StoredProcedureNameItemInQueryResults < 0 Then _
                _Name = GetSafeString(dr.Item(SqlGenerator.StoredProcedureNameItemInQueryResults))

            SqlGenerator.FetchStoredProcedureFromDbDefinition(GetSafeString( _
                dr.Item(SqlGenerator.StoredProcedureSourceItemInQueryResults)), _
                _Name, _SourceCode, _SourceCodeComparable)

        End Sub

        Private Function GetSafeString(ByVal DataRowValue As Object) As String
            Try
                Return System.Text.Encoding.ASCII.GetString(CType(DataRowValue, Byte())).Trim
            Catch ex As Exception
                Return DataRowValue.ToString.Trim
            End Try
        End Function


        Friend Sub Insert(ByVal writer As Xml.XmlWriter)

            writer.WriteStartElement("DatabaseStoredProcedure")

            writer.WriteStartAttribute("Name")
            writer.WriteValue(_Name)
            writer.WriteEndAttribute()

            writer.WriteStartAttribute("SourceCode")
            writer.WriteValue(_SourceCode)
            writer.WriteEndAttribute()

            writer.WriteStartAttribute("SourceCodeComparable")
            writer.WriteValue(_SourceCodeComparable)
            writer.WriteEndAttribute()

            writer.WriteStartAttribute("Description")
            writer.WriteValue(_Description)
            writer.WriteEndAttribute()

            writer.WriteEndElement()

            MarkOld()
        End Sub

#End Region

    End Class

End Namespace