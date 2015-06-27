Imports System.IO

Public Class RetrieveModel
    Private getFolderModel(,) As String
    Private Const processName = "Classifier"
    Private Const processStart = "Start"
    Private Const processFinish = "Finished"
    Private Const _TOTALID As String = "3063377A-000B-11E5-AEE4-FD367263C431"
    Private Const _MODELDATAFILEL0R0 = "ModelDataL0R0.txt"
    Private Const _MODELDATAFILEL0R1 = "ModelDataL0R1.txt"
    Private Const _MODELDATAFILEL0R2 = "ModelDataL0R2.txt"
    Private Const _MODELDATAFILEL0R3 = "ModelDataL0R3.txt"
    Private Const _MODELDATAFILEL0R4 = "ModelDataL0R4.txt"
    Private Const _MODELDATAFILEL0R5 = "ModelDataL0R5.txt"

    Public Enum FeatureSelection
        L0R0
        L0R1
        L0R2
        L0R3
        L0R4
        L0R5
    End Enum

    Public ReadOnly Property Model() As Array
        Get
            Return getFolderModel
        End Get
    End Property

    Public Sub setModel(pathToModel As String, selectModel As FeatureSelection)

        Dim sentenceArr() As String
        Dim corpusArr() As String
        Dim lineText As String
        Dim pathSep As String
        pathSep = Path.DirectorySeparatorChar

        Dim classifierModelFile As String
        Select Case selectModel
            Case FeatureSelection.L0R0
                classifierModelFile = _MODELDATAFILEL0R0
            Case FeatureSelection.L0R1
                classifierModelFile = _MODELDATAFILEL0R1
            Case FeatureSelection.L0R2
                classifierModelFile = _MODELDATAFILEL0R2
            Case FeatureSelection.L0R3
                classifierModelFile = _MODELDATAFILEL0R3
            Case FeatureSelection.L0R4
                classifierModelFile = _MODELDATAFILEL0R4
            Case FeatureSelection.L0R5
                classifierModelFile = _MODELDATAFILEL0R5
        End Select
        Dim filesInDirectory As String() = Directory.GetFiles(pathToModel, classifierModelFile)
        Dim fileName As String

        For Each fileName In filesInDirectory
            Dim textIn As New StreamReader(New FileStream(fileName, FileMode.Open, FileAccess.Read))


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
            textIn.Close()
        Next

        filesInDirectory = Nothing
    End Sub
End Class
