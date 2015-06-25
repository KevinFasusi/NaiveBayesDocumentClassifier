Imports System.IO


Public Class TrainClassifier
    Public Enum FolderSelection
        folder1
        folder2
    End Enum
    Private _sentenceString As String '
    Private _wordCount As Integer
    Private _wordArr() As String
    Private _folderPath As String
    Private _trainingModel As List(Of String) 'was classification model
    Private Const _PROCESSNAME = "Training"
    Private Const _PROCESSSTART = "Start"
    Private Const _PROCESSFINISH = "Finished"
    Private Const _MODELDATAFILE = "ModelData.txt"
    Private Const _MODELDATAFILEL0R1 = "ModelDataL0R1.txt"
    Private Const _MODELDATAFILEL0R2 = "ModelDataL0R2.txt"
    Private Const _MODELDATAFILEL0R3 = "ModelDataL0R3.txt"
    Private Const _MODELDATAFILEL0R4 = "ModelDataL0R4.txt"
    Private Const _MODELDATAFILEL0R5 = "ModelDataL0R5.txt"

    Public Sub New()
        _trainingModel = New List(Of String)

    End Sub

    Public Property sentenceString As String 'was sentence to filter
        Get
            sentenceString = _sentenceString
        End Get
        Set(value As String)
            _sentenceString = value
        End Set
    End Property

    Public Property TrainingModel As List(Of String)
        Get
            TrainingModel = _trainingModel
        End Get
        Set(value As List(Of String))
            _trainingModel = value
        End Set
    End Property


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pathToData"></param>
    ''' <remarks></remarks>
    Sub BuildTrainingModel(pathToData As String)
        Dim pathSep, textLine, fileName As String
        pathSep = Path.DirectorySeparatorChar
        Dim sentenceArr(), sentenceArrL0R1(), sentenceArrL0R2(), sentenceArrL0R3(), sentenceArrL0R4(), sentenceArrL0R5() As String
        Dim filesInDirectory As String() = Directory.GetFiles(pathToData, "*.txt")
        Dim corpusArr(), corpusArrL0R1(), corpusArrL0R2(), corpusArrL0R3(), corpusArrL0R4(), corpusArrL0R5() As String
        Dim l0R0, l0R1, l0R2, l0R3, l0R4, l0R5, i As Integer
        l0R0 = 0 : l0R1 = 0 : l0R2 = 0 : l0R3 = 0 : l0R4 = 0 : l0R5 = 0 : i = 0


        Try
            Dim textIn As StreamReader
            Dim senLoop As Integer
            Dim selectModel As New FeatureSelection
            Dim selectFeature As New FeatureSelection

            For Each fileName In filesInDirectory
                textIn = New StreamReader(New FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))

                If fileName <> _MODELDATAFILE Then

                    'read text file, chunk, tokenize add to corpus array based on feature selection.

                    Do While textIn.Peek <> -1
                        textLine = textIn.ReadLine
                        textLine = RemoveArtifacts(textLine)
                        If textLine <> "" Then
                            sentenceArr = CType(TokenizeSentence(textLine), String())
                            sentenceArrL0R1 = CType(selectFeature.FeatureSelectionL0R1(sentenceArr), String())
                            sentenceArrL0R2 = CType(selectFeature.FeatureSelectionL0R2(sentenceArr), String())
                            sentenceArrL0R3 = CType(selectFeature.FeatureSelectionL0R3(sentenceArr), String())
                            sentenceArrL0R4 = CType(selectFeature.FeatureSelectionL0R4(sentenceArr), String())
                            sentenceArrL0R5 = CType(selectFeature.FeatureSelectionL0R5(sentenceArr), String())
                        End If

                        ReDim Preserve corpusArr(l0R0 + UBound(sentenceArr))
                        ReDim Preserve corpusArrL0R1(l0R1 + UBound(sentenceArrL0R1))
                        ReDim Preserve corpusArrL0R2(l0R2 + UBound(sentenceArrL0R2))
                        ReDim Preserve corpusArrL0R3(l0R3 + UBound(sentenceArrL0R3))
                        ReDim Preserve corpusArrL0R4(l0R4 + UBound(sentenceArrL0R4))
                        ReDim Preserve corpusArrL0R5(l0R5 + UBound(sentenceArrL0R5))

                        For i = 0 To UBound(sentenceArr)
                            corpusArr(i + l0R0) = sentenceArr(i)
                            corpusArrL0R1(i + l0R1) = sentenceArrL0R1(i)
                            corpusArrL0R2(i + l0R2) = sentenceArrL0R2(i)
                            corpusArrL0R3(i + l0R1) = sentenceArrL0R3(i)
                            corpusArrL0R4(i + l0R2) = sentenceArrL0R4(i)
                            corpusArrL0R5(i + l0R1) = sentenceArrL0R5(i)
                        Next

                        l0R0 = l0R0 + UBound(sentenceArr)
                        l0R1 = l0R1 + UBound(sentenceArrL0R1)
                        l0R2 = l0R2 + UBound(sentenceArrL0R2)
                        l0R3 = l0R3 + UBound(sentenceArrL0R3)
                        l0R4 = l0R4 + UBound(sentenceArrL0R4)
                        l0R5 = l0R5 + UBound(sentenceArrL0R5)
                    Loop
                Else : End If
            Next

            textIn.Close()

        Catch ex As EndOfStreamException
            Dim logError As New ErrorLogger("Error building training Model" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)

            Exit Sub
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error building training Model" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
            Exit Sub
        End Try

        Try
            Dim cleanCorpusArr(,), cleanCorpusArrL0R1(,), cleanCorpusArrL0R2(,), _
                cleanCorpusArrL0R3(,), cleanCorpusArrL0R4(,), cleanCorpusArrL0R5(,) As String


            cleanCorpusArr = CType(CleanCorpus(corpusArr), String(,))
            cleanCorpusArrL0R1 = CType(CleanCorpus(corpusArrL0R1), String(,))
            cleanCorpusArrL0R2 = CType(CleanCorpus(corpusArrL0R2), String(,))
            cleanCorpusArrL0R3 = CType(CleanCorpus(corpusArrL0R3), String(,))
            cleanCorpusArrL0R4 = CType(CleanCorpus(corpusArrL0R4), String(,))
            cleanCorpusArrL0R5 = CType(CleanCorpus(corpusArrL0R5), String(,))


            Dim tallyCorpusArr(,), tallyCorpusArrL0R1(,), tallyCorpusArrL0R2(,), tallyCorpusArrL0R3(,), _
                tallyCorpusArrL0R4(,), tallyCorpusArrL0R5(,) As String


            tallyCorpusArr = CType(TallyCorpus(cleanCorpusArr), String(,))
            tallyCorpusArrL0R1 = CType(TallyCorpus(cleanCorpusArrL0R1), String(,))
            tallyCorpusArrL0R2 = CType(TallyCorpus(cleanCorpusArrL0R2), String(,))
            tallyCorpusArrL0R3 = CType(TallyCorpus(cleanCorpusArrL0R3), String(,))
            tallyCorpusArrL0R4 = CType(TallyCorpus(cleanCorpusArrL0R4), String(,))
            tallyCorpusArrL0R5 = CType(TallyCorpus(cleanCorpusArrL0R5), String(,))


            Dim finalCorpusArr(,), finalCorpusArrL0R1(,), finalCorpusArrL0R2(,), finalCorpusArrL0R3(,), _
                finalCorpusArrL0R4(,), finalCorpusArrL0R5(,) As String

            ReDim Preserve finalCorpusArr(UBound(tallyCorpusArr), 4)
            ReDim Preserve finalCorpusArrL0R1(UBound(tallyCorpusArrL0R1), 4)
            ReDim Preserve finalCorpusArrL0R2(UBound(tallyCorpusArrL0R2), 4)
            ReDim Preserve finalCorpusArrL0R3(UBound(tallyCorpusArrL0R3), 4)
            ReDim Preserve finalCorpusArrL0R4(UBound(tallyCorpusArrL0R4), 4)
            ReDim Preserve finalCorpusArrL0R5(UBound(tallyCorpusArrL0R5), 4)

            'Get cumulative total of words in bag and adding additive smoothing
            Dim cumulativeCountTotal, cumulativeCountTotalL0R1, cumulativeCountTotalL0R2, _
                cumulativeCountTotalL0R3, cumulativeCountTotalL0R4, cumulativeCountTotalL0R5 As Long

            i = 0
            cumulativeCountTotal = TallyCumulativeTotal(tallyCorpusArr)
            cumulativeCountTotalL0R1 = TallyCumulativeTotal(tallyCorpusArrL0R1)
            cumulativeCountTotalL0R2 = TallyCumulativeTotal(tallyCorpusArrL0R2)
            cumulativeCountTotalL0R3 = TallyCumulativeTotal(tallyCorpusArrL0R3)
            cumulativeCountTotalL0R4 = TallyCumulativeTotal(tallyCorpusArrL0R4)
            cumulativeCountTotalL0R5 = TallyCumulativeTotal(tallyCorpusArrL0R5)

            Dim textout As New StreamWriter(New FileStream(pathToData & pathSep & _MODELDATAFILE, FileMode.OpenOrCreate, FileAccess.Write))
            Dim textoutL0R1 As New StreamWriter(New FileStream(pathToData & pathSep & _MODELDATAFILEL0R1, FileMode.OpenOrCreate, FileAccess.Write))
            Dim textoutL0R2 As New StreamWriter(New FileStream(pathToData & pathSep & _MODELDATAFILEL0R2, FileMode.OpenOrCreate, FileAccess.Write))
            Dim textoutL0R3 As New StreamWriter(New FileStream(pathToData & pathSep & _MODELDATAFILEL0R3, FileMode.OpenOrCreate, FileAccess.Write))
            Dim textoutL0R4 As New StreamWriter(New FileStream(pathToData & pathSep & _MODELDATAFILEL0R4, FileMode.OpenOrCreate, FileAccess.Write))
            Dim textoutL0R5 As New StreamWriter(New FileStream(pathToData & pathSep & _MODELDATAFILEL0R5, FileMode.OpenOrCreate, FileAccess.Write))

            Parallel.Invoke(
                Sub()
                    WriteModelToFile(textout, tallyCorpusArr, cumulativeCountTotal)
                End Sub,
                Sub()
                    WriteModelToFile(textoutL0R1, tallyCorpusArrL0R1, cumulativeCountTotalL0R1)
                End Sub,
                Sub()
                    WriteModelToFile(textoutL0R2, tallyCorpusArrL0R2, cumulativeCountTotalL0R2)
                End Sub,
                Sub()
                    WriteModelToFile(textoutL0R3, tallyCorpusArrL0R3, cumulativeCountTotalL0R3)
                End Sub,
                Sub()
                    WriteModelToFile(textoutL0R4, tallyCorpusArrL0R4, cumulativeCountTotalL0R4)
                End Sub,
                Sub()
                    WriteModelToFile(textoutL0R5, tallyCorpusArrL0R5, cumulativeCountTotalL0R5)
                End Sub
)

        Catch ex As Exception
            Dim logError As New ErrorLogger("Error building training Model" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
    End Sub

    ''' <summary>
    ''' Writes model to the model folder.
    ''' </summary>
    ''' <param name="textout"></param>
    ''' <param name="tallyCorpusArr"></param>
    ''' <param name="cumulativeCountTotal"></param>
    ''' <remarks></remarks>
    Public Sub WriteModelToFile(textout As StreamWriter, tallyCorpusArr(,) As String, cumulativeCountTotal As Long)
        Dim finalCorpusArr(,) As String
        ReDim Preserve finalCorpusArr(UBound(tallyCorpusArr), 4)
        Dim i As Integer = 0

        For i = LBound(finalCorpusArr) To UBound(finalCorpusArr)
            If IsNumeric(tallyCorpusArr(CInt(i), 1)) Then
                finalCorpusArr(CInt(i), 0) = tallyCorpusArr(CInt(i), 0)
                finalCorpusArr(CInt(i), 1) = CStr(CInt(tallyCorpusArr(CInt(i), 1)))
                finalCorpusArr(CInt(i), 2) = CStr(CInt(tallyCorpusArr(CInt(i), 1)) + 1)
                finalCorpusArr(CInt(i), 3) = CStr(Math.Round(CDbl((CDbl(tallyCorpusArr(CInt(i), 1)) + 1) / cumulativeCountTotal), 10))
                finalCorpusArr(CInt(i), 4) = CStr(Math.Round(Math.Log10(CDbl(CDbl(tallyCorpusArr(CInt(i), 1)) + 1 / cumulativeCountTotal)), 10))
                'write fully formated analysis to log file for specific artilce (Token, token count, smoothed count, probability, log probability
                textout.Write((tallyCorpusArr(CInt(i), 0) & "|" & CInt(tallyCorpusArr(CInt(i), 1)) & "|" & CInt(tallyCorpusArr(CInt(i), 1)) + 1 & "|" _
                      & finalCorpusArr(CInt(i), 3) & "|" & finalCorpusArr(CInt(i), 4)) & "|" & vbCrLf)
            Else : End If
        Next
        textout.Close()
    End Sub
    ''' <summary>
    ''' Removes all punctuation from a sentence
    ''' </summary>
    ''' <param name="sentence"></param>
    ''' <returns>a string without any punctuation</returns>
    ''' <remarks></remarks>
    Public Function RemoveArtifacts(sentence As String) As String

        RemoveArtifacts = Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(LCase(sentence), ". ", " "), ", ", " "), "; ", " "), ": ", " ") _
                                    , "' ", " "), "! ", " "), "# ", " "), "$", " "), "%", " "), "&", " "), "(", " "), ")", " "), " - ", " "), "_", " "), "--", " "), "+", " ") _
                                    , "=", " "), "/", " "), "\", " "), "{", " "), "}", " "), "[", " "), "]", " "), """ ", " "), "?", " "), "*", " "), " """, " "), """", " ")

    End Function


    ''' <summary>
    ''' Pairs two consecutive words to form one token
    ''' </summary>
    ''' <param name="arr"></param>
    ''' <returns>returns an array of string tokens</returns>
    ''' <remarks></remarks>
    Public Function CollacateTwoWords(arr() As String) As Array
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr) Step 4
                If i < UBound(arr) And i >= 3 Then
                    newArr(p) = arr(i - 1) & "-" & arr(i) & "-" & arr(i + 1)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error collocating words" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function

    Public Function CollacateThreeWords(arr() As String) As Array
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr) Step 5
                If i < UBound(arr) And i >= 5 Then
                    newArr(p) = arr(i - 2) & "-" & arr(i - 1) & "-" & arr(i) & "-" & arr(i + 1) & "-" & arr(i + 2)
                    p = p + 1
                End If
            Next
            Return newArr
        Catch ex As Exception
            Dim logError As New ErrorLogger("Error collocating words" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
        Return newArr
    End Function

    Public Function CollacateWord(arr() As String) As Array
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        Try
            For i As Integer = 0 To UBound(arr) Step 2
                If i < UBound(arr) And i >= 3 Then
                    newArr(p) = arr(i) & "-" & arr(i + 1)
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
    ''' Converts a line of text to an array of tokens
    ''' </summary>
    ''' <param name="sentence"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TokenizeSentence(sentence As String) As Object
        Dim i, counter As Integer
        Dim bagOfCharArr() As String
        Dim bagOfWordsArr() As String

        counter = 0
        bagOfCharArr = Split(sentence)

        ReDim bagOfWordsArr(UBound(bagOfCharArr))
        For i = 0 To UBound(bagOfCharArr)
            If Not Replace(Trim(bagOfCharArr(i)), " ", "") = "" Then
                bagOfWordsArr(counter) = bagOfCharArr(i)
                counter = counter + 1
            Else : End If
        Next
        If counter <> 0 Then
            ReDim Preserve bagOfWordsArr(counter - 1)
        Else : End If

        _wordArr = bagOfWordsArr
        TokenizeSentence = bagOfWordsArr
    End Function

    ''' <summary>
    ''' Check to see if string is presnet in array
    ''' </summary>
    ''' <param name="arrToCheckFor"></param>
    ''' <param name="arrToCheckagainst"></param>
    ''' <returns>boolean indicating wether the the arrToCheckFor String is present in the array arrToCheckagainst</returns>
    ''' <remarks></remarks>
    Public Function chkArray(arrToCheckFor As String, arrToCheckagainst(,) As String) As Boolean
        Dim i As Long
        For i = 0 To UBound(arrToCheckagainst, 1)
            If arrToCheckagainst(CInt(i), 0) = "" Then
                chkArray = False
                Exit Function
            ElseIf arrToCheckFor = arrToCheckagainst(CInt(i), 0) Then
                chkArray = True
                Exit Function
            Else
                chkArray = False
            End If
        Next
        chkArray = False
    End Function

    ''' <summary>
    ''' Returns the index location of the word found in the array.
    ''' </summary>
    ''' <param name="arrToCheckFor">string besing searched for</param>
    ''' <param name="arrToCheckagainst">array to find search for string within</param>
    ''' <returns>An integer as the index location</returns>
    ''' <remarks></remarks>
    Public Function chkArrayIndex(arrToCheckFor As String, arrToCheckagainst(,) As String) As Integer
        Dim i As Long
        For i = 0 To UBound(arrToCheckagainst, 1)
            If arrToCheckagainst(CInt(i), 0) = "" Then
                chkArrayIndex = 0
                Exit Function
            ElseIf arrToCheckFor = arrToCheckagainst(CInt(i), 0) Then
                chkArrayIndex = CInt(i)
                Exit Function
            Else
                chkArrayIndex = 0
            End If
        Next
        chkArrayIndex = 0
    End Function

    ''' <summary>
    ''' Removes tokens with less that 4 characters, cleans tken one last time and gets length of word
    ''' </summary>
    ''' <param name="corpus">String array</param>
    ''' <returns>returns an array of strings</returns>
    ''' <remarks></remarks>
    Public Function CleanCorpus(corpus() As String) As Array
        Dim j As Integer = 0
        Dim cleanCorpusArr(UBound(corpus), 1) As String

        For i = 0 To UBound(corpus)
            If Len(corpus(CInt(i))) > 4 And IsNumeric(corpus(CInt(i))) = False Then
                cleanCorpusArr(CInt(j), 0) = Replace(corpus(CInt(i)), "  """, " ")
                cleanCorpusArr(CInt(j), 1) = CStr(Len(corpus(CInt(i)))) 'should change this as this is showing the length of the string before replacing
                j = j + 1
            Else : End If
        Next
        Return cleanCorpusArr
    End Function


    ''' <summary>
    ''' Counts the number of unique tokens in the array
    ''' </summary>
    ''' <param name="cleanCorpusArr">An array of possibly repeating cleaned tokens</param>
    ''' <returns>An array of tokens with a count of their occurance in corpus</returns>
    ''' <remarks></remarks>
    Public Function TallyCorpus(cleanCorpusArr(,) As String) As Array
        Dim tallyCorpusArr(,) As String
        Dim tallyCount As Integer
        ReDim tallyCorpusArr(UBound(cleanCorpusArr), 1)
        Dim k, p As Integer
        Dim matchCondition As Boolean
        Dim wordFromArr As String
        Dim j As Integer

        p = 0
        For i = 0 To UBound(cleanCorpusArr)
            'Check if word in cleanCorpus is already in the tallcorpus
            wordFromArr = cleanCorpusArr(CInt(i), 0)
            If wordFromArr <> "" Then
                matchCondition = chkArray(wordFromArr, tallyCorpusArr)
                If matchCondition = False Then
                    tallyCorpusArr(p, 0) = cleanCorpusArr(CInt(i), 0)
                    j = 0
                    tallyCount = 0
                    Do Until j = UBound(cleanCorpusArr)
                        If tallyCorpusArr(p, 0) = cleanCorpusArr(CInt(j), 0) Then
                            tallyCount = tallyCount + 1
                            tallyCorpusArr(p, 1) = CStr(tallyCount)
                        Else : End If
                        j = j + 1
                    Loop
                    p = p + 1
                Else : End If
            Else : End If
        Next
        Return tallyCorpusArr
    End Function

    Public Function TallyCumulativeTotal(tallyCorpusArr(,) As String) As Long
        Dim cumulativeCountTotal As Long
        For i = LBound(tallyCorpusArr) To UBound(tallyCorpusArr)
            If IsNumeric(tallyCorpusArr(CInt(i), 1)) Then
                cumulativeCountTotal = cumulativeCountTotal + CInt(CDbl(tallyCorpusArr(CInt(i), 1)) + 1)
            Else : End If
        Next
        Return cumulativeCountTotal
    End Function


End Class
