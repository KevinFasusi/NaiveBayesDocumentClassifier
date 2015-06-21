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
    Private Const _MODELDATAFILE2 = "ModelData2.txt"

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

    Sub BuildTrainingModel(Optional pathToData As String = "E:\Testing1")
        'Dim sentenceArr() As String
        'Dim corpusArr() As String
        Dim pathSep As String
        Dim textLine As String
        Dim sentenceArr() As String
        Dim sentenceArr2() As String

        pathSep = Path.DirectorySeparatorChar
        Dim filesInDirectory As String() = Directory.GetFiles(pathToData, "*.txt")
        Dim fileName As String
        Dim corpusArr() As String
        Dim corpusArr2() As String
        Dim i, j, k, l As Long
        Dim counter As Integer
        counter = 0
        j = 0
        k = 0
        l = 0

        Try
            Dim textIn As StreamReader
            For Each fileName In filesInDirectory
                textIn = New StreamReader(New FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))

                If fileName <> _MODELDATAFILE Then
                    'read text file, chunk, tokenize add to corpus array

                    Do While textIn.Peek <> -1
                        textLine = textIn.ReadLine
                        'Console.WriteLine(textLine)
                        textLine = RemoveArtifacts(textLine)
                        ' Console.WriteLine(textLine)
                        If textLine <> "" Then
                            sentenceArr = CType(TokenizeSentence(textLine), String())
                            sentenceArr2 = CType(CollacateTwoWords(sentenceArr), String())
                        End If

                        ReDim Preserve corpusArr(CInt(j + UBound(sentenceArr)))
                        ReDim Preserve corpusArr2(CInt(k + UBound(sentenceArr2)))

                        For i = 0 To UBound(sentenceArr)
                            corpusArr(CInt(i + j)) = sentenceArr(CInt(i))
                            corpusArr2(CInt(i + k)) = sentenceArr2(CInt(i))
                        Next
                        j = j + UBound(sentenceArr)
                        k = k + UBound(sentenceArr2)
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

            Dim cleanCorpusArr(,) As String
            Dim cleanCorpusArr2(,) As String


            cleanCorpusArr = CType(CleanCorpus(corpusArr), String(,))
            cleanCorpusArr2 = CType(CleanCorpus(corpusArr2), String(,))

            Dim tallyCorpusArr(,) As String
            Dim tallyCorpusArr2(,) As String


            tallyCorpusArr = CType(TallyCorpus(cleanCorpusArr), String(,))
            tallyCorpusArr2 = CType(TallyCorpus(cleanCorpusArr2), String(,))


            Dim finalCorpusArr(,) As String
            Dim finalCorpusArr2(,) As String

            ReDim Preserve finalCorpusArr(UBound(tallyCorpusArr), 4)
            ReDim Preserve finalCorpusArr2(UBound(tallyCorpusArr2), 4)

            'Get cumulative total of words in bag and adding additive smoothing
            Dim cumulativeCountTotal As Long
            Dim cumulativeCountTotal2 As Long

            i = 0
            cumulativeCountTotal = TallyCumulativeTotal(tallyCorpusArr)
            cumulativeCountTotal2 = TallyCumulativeTotal(tallyCorpusArr2)

            Dim textout As New StreamWriter(New FileStream(pathToData & pathSep & _MODELDATAFILE, FileMode.OpenOrCreate, FileAccess.Write))
            Dim textout2 As New StreamWriter(New FileStream(pathToData & pathSep & _MODELDATAFILE2, FileMode.OpenOrCreate, FileAccess.Write))
            'probability and normal log

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
            For i = LBound(finalCorpusArr2) To UBound(finalCorpusArr2)
                If IsNumeric(tallyCorpusArr2(CInt(i), 1)) Then
                    finalCorpusArr2(CInt(i), 0) = tallyCorpusArr2(CInt(i), 0)
                    finalCorpusArr2(CInt(i), 1) = CStr(CInt(tallyCorpusArr2(CInt(i), 1)))
                    finalCorpusArr2(CInt(i), 2) = CStr(CInt(tallyCorpusArr2(CInt(i), 1)) + 1)
                    finalCorpusArr2(CInt(i), 3) = CStr(Math.Round(CDbl((CDbl(tallyCorpusArr2(CInt(i), 1)) + 1) / cumulativeCountTotal2), 10))
                    finalCorpusArr2(CInt(i), 4) = CStr(Math.Round(Math.Log10(CDbl(CDbl(tallyCorpusArr2(CInt(i), 1)) + 1 / cumulativeCountTotal2)), 10))
                    'write fully formated analysis to log file for specific artilce (Token, token count, smoothed count, probability, log probability
                    textout2.Write((tallyCorpusArr2(CInt(i), 0) & "|" & CInt(tallyCorpusArr2(CInt(i), 1)) & "|" & CInt(tallyCorpusArr2(CInt(i), 1)) + 1 & "|" _
                          & finalCorpusArr2(CInt(i), 3) & "|" & finalCorpusArr2(CInt(i), 4)) & "|" & vbCrLf)
                Else : End If
            Next
            textout2.Close()

        Catch ex As Exception
            Dim logError As New ErrorLogger("Error building training Model" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sentence"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RemoveArtifacts(sentence As String) As String

        RemoveArtifacts = Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(LCase(sentence), ". ", " "), ", ", " "), "; ", " "), ": ", " ") _
                                    , "' ", " "), "! ", " "), "# ", " "), "$", " "), "%", " "), "&", " "), "(", " "), ")", " "), " - ", " "), "_", " "), "--", " "), "+", " ") _
                                    , "=", " "), "/", " "), "\", " "), "{", " "), "}", " "), "[", " "), "]", " "), """ ", " "), "?", " "), "*", " "), " """, " "), """", " ")

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="arr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CollacateTwoWords(arr() As String) As Array
        Dim newArr(UBound(arr)) As String
        Dim p As Integer = 0
        For i As Integer = 0 To UBound(arr) Step 2
            If i < UBound(arr) Then
                newArr(p) = arr(i) & "-" & arr(i + 1)
                p = p + 1
            End If
        Next
        Return newArr
    End Function

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
    ''' <returns></returns>
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
