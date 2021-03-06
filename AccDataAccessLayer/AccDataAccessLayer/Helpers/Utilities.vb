Imports System.Web.Services
Public Module Utilities

    ''' <summary>
    ''' Gets a substring from Tab (CHR9) delimited string.
    ''' </summary>
    ''' <param name="SourceString">Tab (CHR9) delimited string.</param>
    ''' <param name="index">Number (index) of substring to retrieve.</param>
    Friend Function GetElement(ByVal SourceString As String, ByVal index As Integer) As String
        Dim SubStrings As String() = SourceString.Split(Chr(9))
        If SubStrings.Length > index Then
            Return SubStrings(index)
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' Returns path to the folder where the program (.exe) is executing.
    ''' </summary>
    Public Function AppPath() As String
        'parodo .exe failo absoliucia vieta kompe
        Dim isserver As Boolean = False
        Try
            isserver = DatabaseAccess.GetCurrentIdentity.IsWebServerImpersonation
        Catch ex As Exception
        End Try
        If ApplicationContext.ExecutionLocation = ExecutionLocations.Server OrElse _
            Security.SecurityMethods.IsWebServer(isserver) Then

            Return System.Web.HttpContext.Current.Server.MapPath("App_Data")

        Else
            Try
                Return System.IO.Path.GetDirectoryName(Reflection.Assembly _
                    .GetEntryAssembly().Location)
            Catch ex As Exception
                Return System.Web.HttpContext.Current.Server.MapPath("App_Data")
            End Try
        End If
    End Function

    Public Function ConvertDbBoolean(ByVal DbBooleanValue As Integer) As Boolean
        Return DbBooleanValue > 0
    End Function

    Public Function ConvertDbBoolean(ByVal BooleanValue As Boolean) As Integer
        If BooleanValue Then Return 1
        Return 0
    End Function

    Friend Function HashPasswordMD5(ByVal s As String) As String

        Dim c As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim b As Byte()

        b = c.ComputeHash(Text.Encoding.Unicode.GetBytes(s))

        Dim result As New System.Text.StringBuilder("")
        For i As Integer = 1 To b.Length
            result.Append(b(i - 1).ToString("X2"))
        Next

        Return result.ToString

    End Function

    Friend Function HashPasswordSha256(ByVal SourceString As String) As String

        Dim c As New System.Security.Cryptography.SHA256Managed
        Dim b As Byte()

        b = c.ComputeHash(Text.Encoding.Unicode.GetBytes(SourceString))

        Dim result As New System.Text.StringBuilder("")
        For i As Integer = 1 To b.Length
            result.Append(b(i - 1).ToString("X2"))
        Next

        Return result.ToString

    End Function

    Friend Function ByteArrayToImage(ByVal source As Byte()) As System.Drawing.Image

        If source Is Nothing OrElse source.Length < 10 Then Return Nothing

        Dim result As System.Drawing.Image = Nothing

        Using MS As New IO.MemoryStream(source)
            Try
                result = System.Drawing.Image.FromStream(MS)
            Catch ex As Exception
            End Try
        End Using

        Return result

    End Function

    Friend Function ImageToByteArray(ByVal source As System.Drawing.Image) As Byte()

        If source Is Nothing Then Return Nothing

        Dim result As Byte() = Nothing

        Dim ImageToSave As System.Drawing.Bitmap = New System.Drawing.Bitmap( _
            source.Width, source.Height, source.PixelFormat)
        Using gr As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(ImageToSave)
            gr.DrawImage(source, New System.Drawing.PointF(0, 0))
        End Using

        Using ms As New IO.MemoryStream
            Try
                ImageToSave.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                result = ms.ToArray
            Catch ex As Exception
            End Try
        End Using

        ImageToSave.Dispose()
        GC.Collect()

        Return result

    End Function

    Public Function IsSqlException(ByVal e As Exception) As Boolean

        If TypeOf e Is MySql.Data.MySqlClient.MySqlException OrElse _
            TypeOf e Is SQLite.SQLiteException Then Return True

        If Not e.InnerException Is Nothing Then Return IsSqlException(e.InnerException)

        Return False

    End Function

#Region " Safe type cast "

    ''' <summary>
    ''' Gets a rounded value of d using Asymmetric Arithmetic Rounding algorithm
    ''' </summary>
    Private Function CRound(ByVal d As Double, ByVal r As Integer) As Double
        Dim i As Long = CLng(Math.Floor(d * Math.Pow(10, r)))
        If i + 0.5 > CType(d * Math.Pow(10, r), Decimal) Then
            Return i / Math.Pow(10, r)
        Else
            Return (i + 1) / Math.Pow(10, r)
        End If
    End Function
    Private Function CRound(ByVal d As Double) As Double
        Return CRound(d, 2)
    End Function

    Public Function CStrSafe(ByVal Obj As Object) As String

        If Obj Is Nothing OrElse TypeOf Obj Is DBNull Then Return ""

        If TypeOf Obj Is String OrElse TypeOf Obj Is Boolean OrElse _
            TypeOf Obj Is SByte OrElse TypeOf Obj Is Byte OrElse _
            TypeOf Obj Is Char OrElse TypeOf Obj Is Short OrElse _
            TypeOf Obj Is UShort OrElse TypeOf Obj Is Integer OrElse _
            TypeOf Obj Is UInteger OrElse TypeOf Obj Is Long OrElse _
            TypeOf Obj Is ULong OrElse TypeOf Obj Is Single OrElse _
            TypeOf Obj Is Double OrElse TypeOf Obj Is Decimal OrElse _
            TypeOf Obj Is Date Then Return Convert.ToString(Obj)

        Return Obj.ToString

    End Function

    Public Function CIntSafe(ByVal Obj As Object, Optional ByVal DefaultValue As Integer = 0) As Integer
        Dim result As Integer = DefaultValue
        If Integer.TryParse(CStrSafe(Obj), result) Then Return result
        Return DefaultValue
    End Function

    Public Function CByteSafe(ByVal Obj As Object, Optional ByVal DefaultValue As Byte = 0) As Integer
        Dim result As Byte = DefaultValue
        If Byte.TryParse(CStrSafe(Obj), result) Then Return result
        Return DefaultValue
    End Function

    Public Function CLongSafe(ByVal Obj As Object, Optional ByVal DefaultValue As Long = 0) As Long
        Dim result As Long = DefaultValue
        If Long.TryParse(CStrSafe(Obj), result) Then Return result
        Return DefaultValue
    End Function

    Public Function CDblSafe(ByVal Obj As Object, Optional ByVal RoundOrder As Integer = 2, _
        Optional ByVal DefaultValue As Double = 0) As Double
        Dim result As Double = DefaultValue
        If Double.TryParse(CStrSafe(Obj), result) Then Return CRound(result, RoundOrder)
        Return DefaultValue
    End Function

    Public Function CDecimalSafe(ByVal Obj As Object, Optional ByVal RoundOrder As Integer = 2, _
        Optional ByVal DefaultValue As Decimal = 0) As Double
        Dim result As Decimal = DefaultValue
        If Decimal.TryParse(CStrSafe(Obj), result) Then Return CRound(result, RoundOrder)
        Return DefaultValue
    End Function

    Public Function CDateSafe(ByVal Obj As Object, ByVal DefaultValue As Date) As Date
        Dim result As Date = DefaultValue
        If Date.TryParse(CStrSafe(Obj), result) Then Return result
        Return DefaultValue
    End Function

    Public Function CDateTimeSafe(ByVal Obj As Object, ByVal DefaultValue As Date) As Date
        Dim result As Date = DefaultValue
        If DateTime.TryParse(CStrSafe(Obj), result) Then Return result
        Return DefaultValue
    End Function

    Public Function CTimeStampSafe(ByVal Obj As Object) As DateTime
        Dim result As DateTime = CDateTimeSafe(Obj, DateTime.UtcNow)
        Return DateTime.SpecifyKind(result, DateTimeKind.Utc).ToLocalTime
    End Function

    Public Function CByteArraySafe(ByVal Obj As Object, ByVal MinLength As Integer) As Byte()
        If Obj Is Nothing OrElse TypeOf Obj Is DBNull OrElse Not TypeOf Obj Is Byte() OrElse _
            DirectCast(Obj, Byte()).Length < MinLength Then Return Nothing
        Return DirectCast(Obj, Byte())
    End Function

#End Region

End Module
