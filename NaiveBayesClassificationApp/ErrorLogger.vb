Imports DocumentClassifier
Imports System.Xml
Imports System.IO
Imports System.Security.Principal

Public Class ErrorLogger

    Enum ErrorType
        Warning
        FatalError
    End Enum

    Private _systemInfo As String
    Private _registeredUsername As String
    Private _datetime As Date
    Private _errorMessage As String
    Private _XPATH As String = "Resources/ErrorLog.txt"
    Private Const _TITLE = "Error"
    Public Sub New()

    End Sub

    Public Sub New(ErrorMessage As String, stackTrace As String, errorType As ErrorType)
        Me.errorMessage = ErrorMessage
        Me.datetime = Date.Now
        Me.SystemInfo = stackTrace
        WriteToErrorLog(_TITLE, ErrorMessage, stackTrace, errorType, datetime)
    End Sub

    Public Property SystemInfo As String
        Get
            Return _systemInfo
        End Get
        Set(value As String)
            _systemInfo = value
        End Set
    End Property

    Public Property ResgisteredUsername As String
        Get
            Return _registeredUsername
        End Get
        Set(value As String)
            _registeredUsername = value
        End Set
    End Property

    Public Property datetime As Date
        Get
            Return _datetime
        End Get
        Set(value As Date)
            _datetime = value
        End Set
    End Property

    Public Property errorMessage As String
        Get
            Return _errorMessage
        End Get
        Set(value As String)
            _errorMessage = value
        End Set
    End Property

    Public Sub LogError()

    End Sub

    Public Sub WriteToErrorLog(Title As String, msg As String, stackTrace As String, errorType As ErrorType, dateOfexception As Date)
        Dim writeToFile As New StreamWriter(New FileStream(_XPATH, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
        writeToFile.Write("Title: " & Title & vbCrLf)
        writeToFile.Write("Message: " & msg & vbCrLf)
        writeToFile.Write("StackTrace: " & stackTrace & vbCrLf)
        writeToFile.Write("Error Type: " & errorType.ToString & vbCrLf)
        writeToFile.Write("Date/Time: " & dateOfexception.ToString() & vbCrLf)
        writeToFile.Write("=========================================================================================================================" & vbCrLf)
        writeToFile.Close()

    End Sub


End Class

