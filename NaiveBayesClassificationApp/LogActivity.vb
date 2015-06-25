Imports System.Xml.Serialization
Imports System.Xml
Imports System.Threading
Imports System.IO

Public Class LogActivity

    Private _activity As String
    Private _status As String
    Private _datetime As String
    Private _update As String
    Private Const _XPATH As String = "Resources\LogActivity.xml"

    Public Property Activity As String
        Get
            Activity = _activity
        End Get
        Set(value As String)

            _activity = value
        End Set
    End Property

    Public Property Status As String
        Get
            Status = _status
        End Get
        Set(value As String)
            _status = value
        End Set
    End Property

    Public Property Datetime As String
        Get
            Datetime = _datetime
        End Get
        Set(value As String)
            _datetime = value
        End Set
    End Property

    Public Property Update As String
        Get
            Update = _update
        End Get
        Set(value As String)
            _update = value
        End Set
    End Property

    Public ReadOnly Property XPath As String
        Get
            Return _XPATH
        End Get
    End Property

    Public Sub AppendToLog(logging As LogActivity)
        Dim activitiesLog As New List(Of LogActivity)
        Dim listActivity As New List(Of LogActivity)
        Dim rowActivity As New LogActivity
        Dim tryAgain As Boolean = True
        Dim settings As New XmlWriterSettings
        settings.Indent = True
        settings.IndentChars = ("  ")

        Dim tag As New IO.StreamReader(_XPATH, False)

        Dim xmlIn As XmlReader = XmlReader.Create(tag)
        Dim xmlSerialiser As XmlSerializer = New XmlSerializer(GetType(List(Of LogActivity)))

        Do While (tryAgain)

            Try
                If xmlSerialiser.CanDeserialize(xmlIn) = True Then
                    listActivity = CType(xmlSerialiser.Deserialize(xmlIn), Global.System.Collections.Generic.List(Of Global.NaiveBayesClassificationApp.LogActivity))
                    listActivity.Add(logging)
                    activitiesLog = listActivity
                    xmlIn.Close()
                    xmlIn = Nothing
                    xmlSerialiser = Nothing
                    tag.Close()
                    tag = Nothing

                End If

                Dim writer As New IO.StreamWriter(_XPATH, False)
                Dim xmlOut As XmlWriter = XmlWriter.Create(writer, settings)
                xmlOut.WriteStartDocument()
                xmlOut.WriteStartElement("ArrayOfLogActivity")
                For Each rowActivity In activitiesLog
                    xmlOut.WriteStartElement("LogActivity")
                    xmlOut.WriteElementString("Activity", rowActivity.Activity)
                    xmlOut.WriteElementString("Status", rowActivity.Status)
                    xmlOut.WriteElementString("Datetime", rowActivity.Datetime)
                    'xmlOut.WriteElementString("Status", logging.Status)
                    xmlOut.WriteElementString("Update", rowActivity.Update)
                    xmlOut.WriteEndElement()
                Next
                xmlOut.WriteEndElement()
                xmlOut.Close()
                writer.Close()
                tryAgain = False
                rowActivity = Nothing

            Catch ex As Exception
                Dim logError As New ErrorLogger("Error appending to log " & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
            End Try
        Loop
    End Sub
End Class
