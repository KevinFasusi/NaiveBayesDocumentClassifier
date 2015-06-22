Class MainSettings

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        Dim selectedClassifier As New ApplicationSettings
        Dim classifier As String
        Try
            classifier = selectedClassifier.Classification
            Select Case classifier
                Case ApplicationSettings.Classifier.Multinominal.ToString
                    rdoMultinominal.IsChecked = True
                Case ApplicationSettings.Classifier.Bernoulli.ToString
                    rdoBernoulli.IsChecked = True
                Case ApplicationSettings.Classifier.Ensemble.ToString
                    rdoEnsemble.IsChecked = True
            End Select
        Catch ex As Exception
            Dim logError As New ErrorLogger(ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim selectModel As New ApplicationSettings

        Try
            If rdoEnsemble.IsChecked = True Then
                selectModel.WriteSessionData(ApplicationSettings.Classifier.Ensemble)
            ElseIf rdoBernoulli.IsChecked = True Then
                selectModel.WriteSessionData(ApplicationSettings.Classifier.Bernoulli)
            ElseIf rdoMultinominal.IsChecked = True Then
                selectModel.WriteSessionData(ApplicationSettings.Classifier.Multinominal)
            Else
                MessageBox.Show("Please select a classification model to proceed.")
            End If
        Catch ex As Exception
            Dim logError As New ErrorLogger(ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
            Exit Sub
        End Try

    End Sub
End Class
