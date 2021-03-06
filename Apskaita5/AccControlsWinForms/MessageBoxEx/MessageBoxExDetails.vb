''' <summary>
''' (c) http://www.codeproject.com/Articles/9656/Dissecting-the-MessageBox
''' </summary>
Friend Class MessageBoxExDetails

    Friend _Exception As Exception = Nothing

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Me.Hide()
        Me.Close()
    End Sub

    Private Sub MessageBoxExDetails_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load
        If _Exception Is Nothing Then Exit Sub
        ExDetails.Text = FormatExceptionString(_Exception, False)
    End Sub

    Private Function FormatExceptionString(ByVal ex As Exception, ByVal Internal As Boolean) As String
        Dim result As String = ""
        If Internal Then
            result = "Vidinės klaidos (internal exception) duomenys: " & vbCrLf
        Else
            result = "Klaidos duomenys: " & vbCrLf
        End If

        result = result & "Klaidos tekstas: " & vbCrLf & ex.Message & vbCrLf

        If ex.Source IsNot Nothing Then _
            result = result & "Klaidos šaltinis(Ex.Source): " & vbCrLf & ex.Source & vbCrLf

        If ex.TargetSite IsNot Nothing Then _
                result = result & "Klaidos metodas (Ex.TargetSite): " & vbCrLf & ex.TargetSite.Name & vbCrLf

        If ex.StackTrace IsNot Nothing AndAlso Not String.IsNullOrEmpty(ex.StackTrace) Then _
            result = result & "Klaidos stekas: " & vbCrLf & ex.StackTrace & vbCrLf

        If ex.InnerException IsNot Nothing Then
            result = result & vbCrLf & "----------------------------" & vbCrLf
            result = result & FormatExceptionString(ex.InnerException, True)
        End If

        Return result

    End Function

End Class