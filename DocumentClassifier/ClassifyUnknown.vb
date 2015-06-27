Imports System.IO
Imports System.Xml

Public Class ClassifyUnknown
    Inherits TrainClassifier

    Private _modelArr1(,) As String
    Private _modelArr2(,) As String
    Private _compArr(,) As String
    Private wordArr() As String
    Private _classificationResult As String
    Private _LogProb As Double
    Private _FileName As String
    Private Const _TOTALID As String = "3063377A-000B-11E5-AEE4-FD367263C431"
    Private Const processName = "Training"
    Private Const processStart = "Start"
    Private Const processFinish = "Finished"
    Private _logResult As Double

    Public ReadOnly Property ClassificationResult() As String
        Get
            Return _classificationResult
        End Get

    End Property


    Public ReadOnly Property LogProb() As Double
        Get
            Return _LogProb
        End Get
    End Property

    Public ReadOnly Property FileName() As String
        Get
            Return _FileName
        End Get
    End Property

    Public WriteOnly Property Model1() As Array
        Set(value As Array)
            _modelArr1 = CType(value, String(,))
        End Set

    End Property

    Public WriteOnly Property Model2() As Array
        Set(value As Array)
            _modelArr2 = CType(value, String(,))
        End Set
    End Property


    'it would be safer and more sensible to store the results in a generic list here and then use this as the basis of the the log to be written to the xml file 
    'instead of having to read back from a text file
    'in next code review i will be removing the the dependece on text files to store the prediction data and using xml files only

    Public Sub ClassifyUnknown(pathToUnknown As String, featureToSelect As Feature)

        Dim pathSep As String
        Dim textLine As String
        Dim sentenceArr() As String
        pathSep = Path.DirectorySeparatorChar
        Dim filesInDirectory As String() = Directory.GetFiles(pathToUnknown, "*.txt")
        Dim fileName As String
        Dim corpusArr() As String
        Dim i, lnRn As Integer
        Dim counter As Integer
        counter = 0
        lnRn = 0
        Dim selectFeature As New FeatureSelection


        For Each fileName In filesInDirectory

            Dim textIn As New StreamReader(New FileStream(fileName, FileMode.Open, FileAccess.Read))

            Dim classifierModelFile As String

            If Right(fileName, 15) <> "-Prediction.txt" Then
                classifierModelFile = Replace(fileName, ".txt", "") & "-" & featureToSelect.ToString & "-Prediction.txt"

                Do While textIn.Peek <> -1
                    textLine = textIn.ReadLine
                    'Console.WriteLine(textLine)
                    textLine = MyBase.RemoveArtifacts(textLine)
                    ' Console.WriteLine(textLine)
                    If textLine <> "" Then
                        sentenceArr = CType(TokenizeSentence(textLine), String())

                        Select Case featureToSelect
                            Case Feature.L0R1
                                sentenceArr = CType(selectFeature.FeatureSelectionL0R1(sentenceArr), String())
                            Case Feature.L0R2
                                sentenceArr = CType(selectFeature.FeatureSelectionL0R2(sentenceArr), String())
                            Case Feature.L0R3
                                sentenceArr = CType(selectFeature.FeatureSelectionL0R3(sentenceArr), String())
                            Case Feature.L0R4
                                sentenceArr = CType(selectFeature.FeatureSelectionL0R4(sentenceArr), String())
                            Case Feature.L0R5
                                sentenceArr = CType(selectFeature.FeatureSelectionL0R5(sentenceArr), String())
                        End Select
                    End If


                    ReDim Preserve corpusArr(lnRn + UBound(sentenceArr))

                    For i = 0 To UBound(sentenceArr)
                        corpusArr(i + lnRn) = sentenceArr(i)
                    Next
                    lnRn = lnRn + UBound(sentenceArr)
                Loop


                'move to own function
                Dim cleanCorpusArr(,) As String

                cleanCorpusArr = CType(MyBase.CleanCorpus(corpusArr), String(,))

                Dim tallyCorpusArr(,) As String
                tallyCorpusArr = CType(MyBase.TallyCorpus(cleanCorpusArr), String(,))

                Dim comparisonArr(,) As String
                Dim comparisonIndex As Integer
                Dim wordFromArr As String
                ReDim comparisonArr(UBound(_modelArr1), 2)
                Dim p As Integer = 0
                For i = 0 To UBound(cleanCorpusArr)

                    wordFromArr = tallyCorpusArr(CInt(i), 0)
                    wordFromArr = Replace(wordFromArr, " ", "")

                    If wordFromArr <> "" Then


                        If chkArray(wordFromArr, _modelArr1) = True Then
                            comparisonArr(p, 0) = wordFromArr
                            comparisonArr(p, 1) = _modelArr1(comparisonIndex, 1)
                        Else
                            comparisonArr(p, 0) = wordFromArr
                            comparisonArr(p, 1) = CStr(0)
                        End If

                        If chkArray(wordFromArr, _modelArr2) = True Then
                            comparisonIndex = CInt(MyBase.chkArrayIndex(wordFromArr, _modelArr2))
                            comparisonArr(p, 0) = wordFromArr
                            comparisonArr(p, 2) = _modelArr2(comparisonIndex, 1) 'almost solved the problem is here. you do not know the index of the germodelarr where the word was found to then assign it to the third column. need a way to pass back that value from the function
                        Else
                            comparisonArr(p, 0) = wordFromArr
                            comparisonArr(p, 2) = CStr(0)
                        End If
                        p = p + 1
                    Else : End If

                Next
                'write the column headings in each file
                i = 0
                Dim textout As New StreamWriter(New FileStream(classifierModelFile, FileMode.OpenOrCreate, FileAccess.Write))

                Do While i < UBound(comparisonArr) And (comparisonArr(CInt(i), 0) <> "")
                    textout.Write(comparisonArr(CInt(i), 0) & "|" & comparisonArr(CInt(i), 1) & "|" & comparisonArr(CInt(i), 2) & "|" & vbCrLf)
                    i = i + 1
                Loop
                'Get cumulative total of words in bag and adding additive smoothing
                Dim cumulativeCountTotal1 As Double
                Dim cumulativeCountTotal2 As Double

                i = 0

                For i = LBound(comparisonArr) To UBound(comparisonArr)
                    If IsNumeric(comparisonArr(CInt(i), 1)) And IsNumeric(comparisonArr(CInt(i), 2)) Then
                        cumulativeCountTotal1 = cumulativeCountTotal1 + CDbl(comparisonArr(CInt(i), 1))
                        cumulativeCountTotal2 = cumulativeCountTotal2 + CDbl(comparisonArr(CInt(i), 2))
                    Else : End If
                Next

                textout.Write(_TOTALID & "|" & cumulativeCountTotal1 & "|" & cumulativeCountTotal2)

                Dim logArr(3) As String

                logArr(0) = fileName

                Dim resultsSession As SessionData
                resultsSession = New SessionData

                'instantiate session class and then write to list box, check original 
                'retrieve alias from xml document
                If cumulativeCountTotal1 > cumulativeCountTotal2 Then
                    logArr(2) = resultsSession.Folder1Alias.ToString
                    logArr(1) = CStr(cumulativeCountTotal1)
                ElseIf cumulativeCountTotal1 < cumulativeCountTotal2 Then
                    logArr(2) = resultsSession.Folder2Alias.ToString
                    logArr(1) = CStr(cumulativeCountTotal2)
                Else
                    logArr(2) = "Inconclusive"
                    logArr(1) = "0"
                End If

                AppendToLog(logArr)
                textout.Close()

                cumulativeCountTotal1 = 0
                cumulativeCountTotal2 = 0
            End If
        Next
    End Sub



    Public Sub AppendToLog(logging() As String)
        Const _XPATH As String = "Resources/LogResult.xml"
        Dim settings As New XmlWriterSettings
        settings.Indent = True
        settings.IndentChars = ("  ")

        Dim xmlOut As XmlWriter = XmlWriter.Create(_XPATH, settings)
        xmlOut.WriteStartElement("ArrayOfLogResults")
        xmlOut.WriteStartElement("LogResult")
        xmlOut.WriteElementString("Filename", logging(0).ToString)
        xmlOut.WriteElementString("Result", logging(1).ToString)
        xmlOut.WriteElementString("ResultAlias", logging(2).ToString)
        xmlOut.WriteEndElement()
        xmlOut.WriteEndElement()
        xmlOut.Close()

    End Sub

End Class
