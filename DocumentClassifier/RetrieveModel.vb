Imports System.IO

Public Class RetrieveModel
    Private getFolderModel(,) As String
    Private Const processName = "Classifier"
    Private Const processStart = "Start"
    Private Const processFinish = "Finished"
    Private Const _TOTALID As String = "3063377A-000B-11E5-AEE4-FD367263C431"


    Public ReadOnly Property Model() As Array
        Get
            Return getFolderModel
        End Get
    End Property

    Public Sub setModel(pathToModel As String)

        Dim sentenceArr() As String
        Dim corpusArr() As String
        Dim lineText As String
        Dim pathSep As String
        pathSep = Path.DirectorySeparatorChar
        Dim filesInDirectory As String() = Directory.GetFiles(pathToModel, "ModelData.txt")
        Dim fileName As String

        Dim textIn As StreamReader

        For Each fileName In filesInDirectory
            textIn = New StreamReader(New FileStream(fileName, FileMode.Open, FileAccess.Read))
            Dim classifierModelFile As String
            classifierModelFile = "ModelData.txt"
            Dim modProbArrTemp() As String

            Do While textIn.Peek <> -1
                modProbArrTemp = Split(textIn.ReadToEnd, "|")
            Loop

            'Read into array irrelevant and relevant probabilities
            Dim modelArr(,) As String
            Dim modProbArr(,) As String
            ReDim Preserve modProbArr(UBound(modProbArrTemp), 1)

            ' remove token and log probability from temp array. Using multiplication to navigate the 1D array created from the fsoReadStrem.ReadAll Method
            Dim m As Long
            Dim j As Long
            m = 0
            j = 0

            Do
                j = m
                modProbArr(CInt(m), 0) = Replace(Trim(modProbArrTemp(CInt((5 * j)))), vbCrLf, "")
                modProbArr(CInt(m), 1) = modProbArrTemp(CInt((((j * 5) + 4))))
                m = m + 1
            Loop Until (j * 5) + 4 > UBound(modProbArrTemp) - 5
            getFolderModel = modProbArr
        Next
        textIn.Close()
        filesInDirectory = Nothing
    End Sub
End Class
