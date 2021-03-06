﻿Imports System.Xml

Public Class ApplicationSettings
    Sub New()

        ReadSessionData()
    End Sub

    Public Enum Classifier
        Multinominal
        Bernoulli
        Ensemble
    End Enum

    Private _classification As String


    Private Const _XPATH = "Resources/ApplicationSettings.xml"

    Public Property Classification As String
        Get
            Return _classification
        End Get
        Set(value As String)
            _classification = value
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

            xmlIn.ReadStartElement("Application")
            ' xmlIn.ReadStartElement("Classification")
            _classification = xmlIn.ReadElementString("Classifier").ToString
            xmlIn.Close()
            writer.Close()
            xmlIn = Nothing
            writer = Nothing
        Catch ex As Exception
            Dim logError As New ErrorLogger(ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
        End Try
    End Sub

    Public Function WriteSessionData(model As Classifier) As Boolean


        Dim settings As New XmlWriterSettings
        settings.Indent = True
        settings.IndentChars = ("  ")
        Dim writer As New IO.StreamWriter(_XPATH, False)
        Dim xmlOut As XmlWriter = XmlWriter.Create(writer, settings)
        xmlOut.WriteStartDocument()
        xmlOut.WriteStartElement("Application")
        xmlOut.WriteElementString("Classifier", model.ToString)
        xmlOut.WriteEndElement()
        xmlOut.Close()
        writer.Close()

        xmlOut = Nothing
        writer = Nothing

        Return True
    End Function

End Class
