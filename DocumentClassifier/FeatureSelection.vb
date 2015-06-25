Public Class FeatureSelection

    'overload addition operator to be able to add feature sets. Not sure why I would want to yet. If I can't find a reason then YANGNI Bitch!

    ''Public Shared Operator +(ByVal arr1() As String, arr2() As String) As Array
    ''    Dim arr3() As String
    ''    Try
    ''        If arr1.Length = 0 And arr2.Length = 0 Then
    ''            Dim ex As New System.Exception("Attempt to add empty array.")
    ''            Throw ex
    ''        End If

    ''        For i As Integer = 0 To UBound(arr1)
    ''            arr3(i) = arr1(i)
    ''        Next
    ''        For i = UBound(arr1) To UBound(arr1) + UBound(arr2)
    ''            arr3(i) = arr2(i - UBound(arr1))
    ''        Next
    ''        Return arr3
    ''    Catch ex As Exception
    ''        Dim logError As New ErrorLogger("Error building training Model" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)

    ''    End Try
    ''    Return arr3
    ''End Operator
    ''' <summary>
    ''' Feature selection for this model is only the ambiguous word, 0 words to the left and 0 words to the right (L0R0)
    ''' </summary>
    ''' <param name="arr"></param>
    ''' <returns>an array of strings as selected features for model building.</returns>
    ''' <remarks></remarks>
    Public Function FeatureSelectionL0R0(arr() As String) As String()
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr)
                If i < UBound(arr) Then
                    newArr(p) = arr(i)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error collocating words" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function

    ''' <summary>
    ''' Feature selection for this model is the ambiguous word and 1 word to the Right (L0R1)
    ''' </summary>
    ''' <param name="arr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FeatureSelectionL0R1(arr() As String) As String()
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr)
                If i + 1 < UBound(arr) And i >= 2 Then
                    newArr(p) = arr(i) & "-" & arr(i + 1)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error generating model using feature selection L0R1" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function

    Public Function FeatureSelectionL0R2(arr() As String) As String()
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr)
                If i + 2 < UBound(arr) And i >= 3 Then
                    newArr(p) = arr(i) & "-" & arr(i + 1) & "-" & arr(i + 2)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error generating model using feature selection L0R2" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function

    Public Function FeatureSelectionL0R3(arr() As String) As String()
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr)
                If i + 3 < UBound(arr) And i >= 4 Then
                    newArr(p) = arr(i) & "-" & arr(i + 1) & "-" & arr(i + 2) & "-" & arr(i + 3)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error collocating words" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function

    Public Function FeatureSelectionL0R4(arr() As String) As String()
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr)
                If i + 4 < UBound(arr) And i >= 5 Then
                    newArr(p) = arr(i) & "-" & arr(i + 1) & "-" & arr(i + 2) & "-" & arr(i + 3) & "-" & arr(i + 4)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error collocating words" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function

    Public Function FeatureSelectionL0R5(arr() As String) As String()
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr)
                If i + 5 < UBound(arr) And i >= 6 Then
                    newArr(p) = arr(i) & "-" & arr(i + 1) & "-" & arr(i + 2) & "-" & arr(i + 3) & "-" & arr(i + 4) & "-" & arr(i + 5)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error collocating words" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function















    Public Function FeatureSelectionL0R10(arr() As String) As Array
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr)
                If i < UBound(arr) And i >= 11 Then
                    newArr(p) = arr(i) & "-" & arr(i + 1) & "-" & arr(i + 2) & "-" & arr(i + 3) & "-" & arr(i + 4) & "-" & arr(i + 5) _
                        & arr(i + 6) & "-" & arr(i + 7) & "-" & arr(i + 8) & "-" & arr(i + 9) & "-" & arr(i + 10)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error collocating words" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function

    Public Function FeatureSelectionL0R25(arr() As String) As Array
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr)
                If i < UBound(arr) And i >= 11 Then
                    newArr(p) = arr(i) & "-" & arr(i + 1) & "-" & arr(i + 2) & "-" & arr(i + 3) & "-" & arr(i + 4) & "-" & arr(i + 5) _
                        & arr(i + 6) & "-" & arr(i + 7) & "-" & arr(i + 8) & "-" & arr(i + 9) & "-" & arr(i + 10) _
                         & arr(i + 11) & "-" & arr(i + 12) & "-" & arr(i + 13) & "-" & arr(i + 14) & "-" & arr(i + 15) _
                        & arr(i + 16) & "-" & arr(i + 17) & "-" & arr(i + 18) & "-" & arr(i + 19) & "-" & arr(i + 20) _
                         & arr(i + 21) & "-" & arr(i + 22) & "-" & arr(i + 23) & "-" & arr(i + 24) & "-" & arr(i + 25)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error collocating words" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function


    Public Function FeatureSelectionL0R50(arr() As String) As Array
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr)
                If i + 48 < UBound(arr) And i >= 51 Then
                    newArr(p) = arr(i) & "-" & arr(i + 1) & "-" & arr(i + 2) & "-" & arr(i + 3) & "-" & arr(i + 4) & "-" & arr(i + 5) _
                        & arr(i + 6) & "-" & arr(i + 7) & "-" & arr(i + 8) & "-" & arr(i + 9) & "-" & arr(i + 10) _
                        & arr(i + 11) & "-" & arr(i + 12) & "-" & arr(i + 13) & "-" & arr(i + 14) & "-" & arr(i + 15) _
                        & arr(i + 16) & "-" & arr(i + 17) & "-" & arr(i + 18) & "-" & arr(i + 19) & "-" & arr(i + 20) _
                        & arr(i + 21) & "-" & arr(i + 22) & "-" & arr(i + 23) & "-" & arr(i + 24) & "-" & arr(i + 25) _
                        & arr(i + 26) & "-" & arr(i + 27) & "-" & arr(i + 28) & "-" & arr(i + 29) & "-" & arr(i + 30) _
                        & arr(i + 31) & "-" & arr(i + 32) & "-" & arr(i + 33) & "-" & arr(i + 34) & "-" & arr(i + 35) _
                        & arr(i + 36) & "-" & arr(i + 37) & "-" & arr(i + 38) & "-" & arr(i + 39) & "-" & arr(i + 40) _
                        & arr(i + 41) & "-" & arr(i + 42) & "-" & arr(i + 43) & "-" & arr(i + 44) & "-" & arr(i + 45) _
                        & arr(i + 46) & "-" & arr(i + 47) & "-" & arr(i + 48) & "-" & arr(i + 49) & "-" & arr(i + 50)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error collocating words" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function

End Class
