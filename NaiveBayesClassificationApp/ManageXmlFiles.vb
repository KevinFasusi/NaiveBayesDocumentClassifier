Imports System.Xml

Public Class ManageXmlFiles
    Private Const _ACTIVITYLOGPATH As String = "Resources/LogActivity.xml"
    Private Const _SESSIONDATA As String = "Resources/SessionData.xml"

    Public ReadOnly Property ActivityLogPath As String
        Get
            ActivityLogPath = _ACTIVITYLOGPATH
        End Get
    End Property

    Public ReadOnly Property SessionData As String
        Get
            SessionData = _SESSIONDATA
        End Get
    End Property


    Public Function CheckXmlStatus(Path As String) As String

        Dim settings As New XmlWriterSettings
        settings.Indent = True
        settings.IndentChars = ("  ")
        Dim reader As New IO.StreamReader(Path, False)
        Dim xmlOut As XmlWriter = XmlWriter.Create(Path, settings)
        Return xmlOut.WriteState.ToString
        reader.Close()

    End Function

End Class
