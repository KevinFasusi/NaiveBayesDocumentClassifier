Imports System.Reflection
Imports Microsoft.Win32

Public Class test

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim appName As String = ""
        Try
            appName = CStr(Assembly.GetEntryAssembly().Location)

            Const IE_EMULATION As String = "Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"
            Using fbeKey = Registry.CurrentUser.OpenSubKey(IE_EMULATION, True)
                fbeKey.SetValue(appName, 9000, RegistryValueKind.DWord)
            End Using

        Catch ex As Exception
            MessageBox.Show((appName & Convert.ToString(vbLf)) + ex.ToString(), "Unexpected error setting browser mode!")
        End Try

    End Sub
End Class
