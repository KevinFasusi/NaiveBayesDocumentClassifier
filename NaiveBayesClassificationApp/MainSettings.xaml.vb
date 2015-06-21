Class MainSettings

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        Dim selectedClassifier As New ApplicationSettings
        Dim classifier As String

        classifier = selectedClassifier.Classification

        Select Case classifier
            Case ApplicationSettings.Classifier.Multinominal.ToString
                rdoMultinominal.IsChecked = True
            Case ApplicationSettings.Classifier.Multinominal.ToString
                rdoBernoulli.IsChecked = True
            Case ApplicationSettings.Classifier.Multinominal.ToString
                rdoEnsemble.IsChecked = True
        End Select
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
