Imports DocumentClassifier

Class AliasSettings

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        Dim folderDetails As New SessionData

        txtAlias1.Text = folderDetails.Folder1Alias
        txtFolder1.Text = folderDetails.FolderPath1

        txtAlias2.Text = folderDetails.Folder2Alias
        txtFolder2.Text = folderDetails.FolderPath2
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim folderDetails As New SessionData

        folderDetails.WriteSessionData(txtFolder1.Text, txtFolder2.Text, folderDetails.UnknownFolder.ToString, txtAlias1.Text, txtAlias2.Text)
    End Sub
End Class
