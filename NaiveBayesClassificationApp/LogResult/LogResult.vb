Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO

Public Class LogResult
    Private _filename As String
    Private _probabilityLog As String
    Private _predictionResult As String
    Private _Alias1 As String
    Private _Alias2 As String
    Private Const _TOTALID As String = "3063377A-000B-11E5-AEE4-FD367263C431"


    Private _predictionResults As New List(Of LogResult)
    'create an interface for the append to log function

    Public ReadOnly Property PredictionResults As List(Of LogResult)
        Get
            Return _predictionResults
        End Get
    End Property

    Public WriteOnly Property Alias1 As String
        Set(value As String)
            _Alias1 = value
        End Set
    End Property

    Public WriteOnly Property Alias2 As String
        Set(value As String)
            _Alias2 = value
        End Set
    End Property

    Public Property FileName As String
        Get
            Return _filename
        End Get
        Set(value As String)
            _filename = value
        End Set
    End Property

    Public Property ProbabilityLog As String
        Get
            Return _probabilityLog
        End Get
        Set(value As String)
            _probabilityLog = value
        End Set
    End Property

    Public Property PredictionResult As String
        Get
            Return _predictionResult
        End Get
        Set(value As String)
            _predictionResult = value
        End Set
    End Property

    'Retrieve the log results convert them into xml and then append to xml file. Remember to do the data binding on the xaml table and deserialise on reload of the homepage
    Public Sub RetriverPredictionResults(pathToUnknown As String)

        Dim sentenceArr() As String
        Dim corpusArr() As String
        Dim lineText As String
        Dim pathSep As String
        pathSep = Path.DirectorySeparatorChar
        Dim filesInDirectory As String() = Directory.GetFiles(pathToUnknown, "*-Prediction.txt")
        Dim fileName As String
        Dim modProbArrTemp() As String

        Dim result As LogResult

        For Each fileName In filesInDirectory

            Dim textIn As New StreamReader(New FileStream(fileName, FileMode.Open, FileAccess.Read))
            Do While textIn.Peek <> -1
                modProbArrTemp = Split(textIn.ReadToEnd, "|")
            Loop
       
            result = New LogResult
            result.FileName = fileName

            result.FileName = Replace(Replace(fileName, pathToUnknown & "\", ""), "-Prediction", "")

            'how do i get the prediction alias for the correct result to be assigned to the prediction result? I used to do it with the sessiondata class
            If CDbl(modProbArrTemp(UBound(modProbArrTemp) - 1)) < CDbl(modProbArrTemp(UBound(modProbArrTemp))) Then
                result.ProbabilityLog = modProbArrTemp(UBound(modProbArrTemp)).ToString
                result.PredictionResult = _Alias2
            ElseIf CDbl(modProbArrTemp(UBound(modProbArrTemp) - 1).ToString) > CDbl(modProbArrTemp(UBound(modProbArrTemp)).ToString) Then
                result.ProbabilityLog = modProbArrTemp(UBound(modProbArrTemp) - 1).ToString
                result.PredictionResult = _Alias1
            End If
            _predictionResults.Add(result)
            modProbArrTemp = Nothing
        Next

    End Sub

    'make into interface and share with the activity class

    Public Sub AppendToLog(logging As LogResult)

        Dim activitiesLog As New List(Of LogResult)
        Dim listActivity As New List(Of LogResult)
        Dim rowActivity As New LogResult
        Const _XPATH As String = "Resources/LogResult.xml"
        Dim tag As New IO.StreamReader(_XPATH, False)
        'deserialise into this initialisation the saved state of the class for a project
        Dim xmlSerialiser As XmlSerializer = New XmlSerializer(GetType(List(Of LogResult)))
        listActivity = CType(xmlSerialiser.Deserialize(tag), Global.System.Collections.Generic.List(Of Global.NaiveBayesClassificationApp.LogResult))
        listActivity.Add(logging)
        activitiesLog = listActivity
        tag.Close()

        Dim settings As New XmlWriterSettings
        settings.Indent = True
        settings.IndentChars = ("  ")
        Dim writer As New IO.StreamWriter(_XPATH, False)
        Dim xmlOut As XmlWriter = XmlWriter.Create(writer, settings)
        xmlOut.WriteStartElement("ArrayOfLogResults")
        For Each logging In activitiesLog
            xmlOut.WriteStartElement("LogResult")
            xmlOut.WriteElementString("Filename", logging.FileName)
            xmlOut.WriteElementString("Probabilitylog", logging.ProbabilityLog)
            xmlOut.WriteElementString("Probabilityresult", logging.PredictionResult)
            xmlOut.WriteEndElement()
        Next
        xmlOut.WriteEndElement()
        xmlOut.Close()
        writer.Close()

    End Sub

    Public Function chkArrayIndex(arrToCheckFor As String, arrToCheckagainst() As String) As Integer
        Dim i As Long
        For i = 0 To UBound(arrToCheckagainst)
            If arrToCheckagainst(CInt(i)) = "" Then
                chkArrayIndex = 0
                Exit Function
            ElseIf arrToCheckFor = arrToCheckagainst(CInt(i)) Then
                chkArrayIndex = CInt(i)
                Exit Function
            Else
                chkArrayIndex = 0
            End If
        Next
        chkArrayIndex = 0
    End Function

End Class
