Imports System.Threading

Public Class Message

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        Dim screenWidth As Double = System.Windows.SystemParameters.PrimaryScreenWidth
        Dim screenHeight As Double = System.Windows.SystemParameters.PrimaryScreenHeight
        Dim windowWidth As Double = Me.Width
        Dim windowHeight As Double = Me.Height
        Me.Left = (screenWidth / 2) - (windowWidth / 2)
        Me.Top = (screenHeight / 2) - (windowHeight / 2)


        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class
