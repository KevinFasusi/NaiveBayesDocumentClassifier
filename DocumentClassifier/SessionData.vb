Imports System.Xml
Imports System.Security.Principal
Imports System.IO

Public Class SessionData

    Public Enum SessionDataType
        folder1Path
        folder2Path
        unknownFolderPath
        folder1Alias
        folder2Alias
    End Enum

    Sub New()

        ReadSessionData()
    End Sub

    Private _folderPath1 As String
    Private _folderPath2 As String
    Private _unknownFolderPath As String
    Private _folder1Alias As String
    Private _folder2Alias As String
    Private Const _XPATH As String = "Resources/SessionData.xml"

    Public Property FolderPath1 As String
        Get
            Return _folderPath1
        End Get
        Set(value As String)
            _folderPath1 = value
        End Set
    End Property

    Public Property FolderPath2 As String
        Get
            Return _folderPath2
        End Get
        Set(value As String)
            _folderPath2 = value
        End Set
    End Property

    Public Property UnknownFolder As String
        Get
            Return _unknownFolderPath
        End Get
        Set(value As String)
            _unknownFolderPath = value
        End Set
    End Property

    Public Property Folder1Alias As String
        Get
            Return _folder1Alias
        End Get
        Set(value As String)
            _folder1Alias = value
        End Set
    End Property

    Public Property Folder2Alias As String
        Get
            Return _folder2Alias
        End Get
        Set(value As String)
            _folder2Alias = value
        End Set
    End Property

    Public Sub ReadSessionData()
        Try
            Dim settings As New XmlReaderSettings
            settings.IgnoreWhitespace = True
            settings.IgnoreComments = True
            Dim writer As New IO.StreamReader(_XPATH, False)
            Dim xmlIn As XmlReader = XmlReader.Create(writer, settings)
            xmlIn.Read()
            'should assign to actual property lose the underscore
            xmlIn.ReadStartElement("Profile")
            xmlIn.ReadStartElement("User")
            _folderPath1 = xmlIn.ReadElementString("FolderPath1").ToString
            _folderPath2 = xmlIn.ReadElementString("FolderPath2").ToString
            _unknownFolderPath = xmlIn.ReadElementString("UnknownFolderPath").ToString
            _folder1Alias = xmlIn.ReadElementString("Folder1Alias").ToString
            _folder2Alias = xmlIn.ReadElementString("Folder2Alias").ToString
            xmlIn.Close()
            writer.Close()
            xmlIn = Nothing
            writer = Nothing
        Catch ex As Exception
            Dim logError As New ErrorLogger(ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
    End Sub

    Public Function WriteSessionData(sessionData As String, selection As SessionDataType) As Boolean
        Dim settings As New XmlWriterSettings
        settings.Indent = True
        settings.IndentChars = ("  ")

        Dim userSystemIdentity As WindowsIdentity = WindowsIdentity.GetCurrent

        Dim xmlOut As XmlWriter = XmlWriter.Create(_XPATH, settings)
        xmlOut.WriteStartDocument()
        xmlOut.WriteStartElement("Profile")
        xmlOut.WriteStartElement("User")
        xmlOut.WriteAttributeString("SystemId", userSystemIdentity.Name.ToString)

        Select Case selection
            Case SessionDataType.folder1Path
                xmlOut.WriteElementString("FolderPath1", sessionData)
            Case SessionDataType.folder2Path
                xmlOut.WriteElementString("FolderPath2", sessionData)
            Case SessionDataType.unknownFolderPath
                xmlOut.WriteElementString("UnknownFolderPath", sessionData)
            Case SessionDataType.folder1Alias
                xmlOut.WriteElementString("Folder1Alias", sessionData)
            Case SessionDataType.folder2Alias
                xmlOut.WriteElementString("Folder2Alias", sessionData)
        End Select

        xmlOut.WriteEndElement()
        xmlOut.Close()
        Return True
    End Function

    Public Function WriteSessionData(folderPath1 As String, folderPath2 As String, unknownFolderPath As String, folder1Alias As String, folder2Alias As String) As Boolean
        Dim settings As New XmlWriterSettings
        settings.Indent = True
        settings.IndentChars = ("  ")
        Dim userSystemIdentity As WindowsIdentity = WindowsIdentity.GetCurrent
        Dim xmlOut As XmlWriter = XmlWriter.Create(_XPATH, settings)
        xmlOut.WriteStartDocument()
        xmlOut.WriteStartElement("Profile")
        xmlOut.WriteStartElement("User")
        xmlOut.WriteAttributeString("SystemId", userSystemIdentity.Name.ToString)
        xmlOut.WriteElementString("FolderPath1", folderPath1)
        xmlOut.WriteElementString("FolderPath2", folderPath2)
        xmlOut.WriteElementString("UnknownFolderPath", unknownFolderPath)
        xmlOut.WriteElementString("Folder1Alias", folder1Alias)
        xmlOut.WriteElementString("Folder2Alias", folder2Alias)
        xmlOut.WriteEndElement()
        xmlOut.Close()
        Return True
    End Function

End Class
