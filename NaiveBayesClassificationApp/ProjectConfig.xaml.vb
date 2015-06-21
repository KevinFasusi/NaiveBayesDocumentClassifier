Imports DocumentClassifier

Public Class ProjectConfig


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        Dim folderDetails As New SessionData

        lblFolder1Path.Content = folderDetails.FolderPath1
        lblAlias1.Content = folderDetails.Folder1Alias
        lblFolder2Path.Content = folderDetails.FolderPath2
        lblAlias2.Content = folderDetails.Folder2Alias

        Dim classification As New ApplicationSettings
        Dim classifier As String
        classifier = classification.Classification


        lblClassificationModel.Content = classifier

        Dim mainSettingsPage As New MainSettings
        frmSettings.NavigationService.Navigate(mainSettingsPage)
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim settingsPage As New AliasSettings
        frmSettings.NavigationService.Navigate(settingsPage)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim mainSettingsPage As New MainSettings
        frmSettings.NavigationService.Navigate(mainSettingsPage)

    End Sub
End Class
