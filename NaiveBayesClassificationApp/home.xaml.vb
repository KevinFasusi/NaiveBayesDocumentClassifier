Imports System.Xml.Serialization
Imports System.IO
Imports DocumentClassifier
Imports System.Xml

Class home

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        Try

            Dim listActivity As New List(Of LogActivity)

            Dim rowActivity As New LogActivity
            Const _XPATH As String = "Resources/LogActivity.xml"
            'Dim tag As New IO.StreamWriter(_XPATH, False)
            Dim tag As New IO.StreamReader(_XPATH, False)
            Dim reader As XmlReader = XmlReader.Create(_XPATH)
            'deserialise into this initialisation the saved state of the class for a project
            Dim xmlSerialiser As XmlSerializer = New XmlSerializer(GetType(List(Of LogActivity)))

            Dim currentModel As New ApplicationSettings
            lblCurrentModel.Content = "Classification Model: " & currentModel.Classification.ToString


            listActivity = CType(xmlSerialiser.Deserialize(tag), Global.System.Collections.Generic.List(Of Global.NaiveBayesClassificationApp.LogActivity))
            lstvwActivity.ItemsSource = listActivity

            'select last item in the list of items
            If lstvwActivity.Items.Count > 0 Then
                lstvwActivity.ScrollIntoView(lstvwActivity.Items(lstvwActivity.Items.Count - 1))
            End If

            tag.Close()
            reader.Close()
            tag = Nothing
            reader = Nothing

            Dim Session As New SessionData
            Dim Results As New LogResult

            txtModelAccuracy.Content = "89%"

            Dim listing As New List(Of LogResult)

            Results.Alias1 = Session.Folder1Alias
            Results.Alias2 = Session.Folder2Alias


            'listing = Results.PredictionResults
            Results.RetriverPredictionResults(Session.UnknownFolder)
            For Each Results In Results.PredictionResults
                lstvwPredictions.Items.Add(Results)
            Next
            Results = Nothing
            Session = Nothing

        Catch ex As Exception
            Dim logError As New ErrorLogger(ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
            Exit Sub
        End Try



    End Sub
End Class
