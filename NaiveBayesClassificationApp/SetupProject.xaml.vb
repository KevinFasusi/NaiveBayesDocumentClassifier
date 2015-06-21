Imports System
Imports System.IO
Imports DocumentClassifier
Imports System.Threading

Class SetupProject

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Dim systemDirectory As String
        'systemDirectory = System.Environment.SystemDirectory
        Dim strSystemDir As String = Environment.GetEnvironmentVariable("SystemDrive")
        strSystemDir = strSystemDir & "\"
        Dim allDrives() As DriveInfo = DriveInfo.GetDrives
        Dim driveData As DriveInfo

        For Each driveData In allDrives
            If driveData.Name.ToString <> strSystemDir.ToString Then
                lstDrives.Items.Add(driveData.Name.ToString)
            End If
        Next
    End Sub



    Private Function IsValidData() As Boolean

        Return IsPresent(txtFirstFolder, "First Folder") _
            AndAlso IsPresent(txtFirstFolderAlias, "First Folder Alias") _
            AndAlso IsPresent(txtSecondFolder, "Second Folder") _
            AndAlso IsPresent(txtSecondFolderAlias, "Second Folder Alias")
    End Function

    Private Function IsPresent(input As TextBox, fieldName As String) As Boolean
        If input.Text = Nothing Then
            MessageBox.Show(fieldName & " is a requires field.", "Entry Error")
            input.Select(1, 1)
            Return False
        Else
            Return True
        End If
    End Function



    Private Sub btnCreateProject_Click(sender As Object, e As RoutedEventArgs) Handles btnCreateProject.Click
        If IsValidData() = False And lstDrives.SelectedItem.ToString = Nothing Then
            Exit Sub
        End If

        Dim folderPath1, folderPath2, unknownFolderPath As String

        folderPath1 = lstDrives.SelectedItem.ToString & txtFirstFolder.Text
        folderPath2 = lstDrives.SelectedItem.ToString & txtSecondFolder.Text
        unknownFolderPath = lstDrives.SelectedItem.ToString & "unknown"

        Dim sGUID As String

        sGUID = System.Guid.NewGuid.ToString()
        unknownFolderPath = unknownFolderPath & sGUID

        If (Not Directory.Exists(folderPath1) AndAlso Not Directory.Exists(folderPath2)) Then
            Try
                Dim createSession As New SessionData
                createSession.WriteSessionData(folderPath1, folderPath2, unknownFolderPath, txtFirstFolderAlias.Text, txtSecondFolderAlias.Text)
                Directory.CreateDirectory(folderPath1)
                Directory.CreateDirectory(folderPath2)
                Directory.CreateDirectory(unknownFolderPath)
            Catch ex As Exception
                Dim response As Integer

                response = MsgBox(ex.Message, MsgBoxStyle.AbortRetryIgnore, "Error Creating Project Structure")
                If response = 4 Then
                    Directory.CreateDirectory(folderPath1)
                    Directory.CreateDirectory(folderPath2)
                    Directory.CreateDirectory(unknownFolderPath)
                ElseIf response = 3 Then
                    Exit Sub
                ElseIf response = 5 Then
                    MsgBox("The necessary folder structure required for this classification project was not created please check that the path specified is correct.")
                    Exit Sub
                End If
            End Try


        Else
            MsgBox("One of the folders already exists," & vbCrLf & " please choose an alternative folder structure for your project.")
            Exit Sub
        End If
        Dim successMessage As New Message
        successMessage.Show()
        NavigationService.GoBack()


    End Sub
End Class
