Imports System.Threading
Imports DocumentClassifier
Imports System.Threading.Tasks
Imports System.Windows.Threading

Public Class MainWindow
    'lambda task created as global values so the click button event would only create on instance before halting the user and waiting till current
    'classification is complete
    Dim pBar As New ProgressBar
    Dim activityLog As New List(Of LogActivity)

    Dim t As New Task(Sub()
                          Parallel.Invoke(Sub()
                                              TrainClassifier()

                                          End Sub,
                         Sub()
                             TrainClassfier2()
                         End Sub)
                      End Sub)
    Dim t2 As Task = t.ContinueWith(Sub()

                                        Try
                                            WriteToLog()
                                            My.Computer.Audio.Play("Resources\glass ping.wav", AudioPlayMode.Background)

                                            frmfoot.Visibility = Windows.Visibility.Hidden
                                            t.Dispose()
                                        Catch ex As Exception
                                            Dim logError As New ErrorLogger(ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
                                            t.Dispose()
                                            Exit Sub
                                        End Try
                                        Dim homeMain As New home
                                        frmMain.NavigationService.Navigate(homeMain)
                                    End Sub, TaskScheduler.FromCurrentSynchronizationContext())

    Dim t3 As New Task(Sub()
                           ClassifyUnknownDocument()
                       End Sub)
    Dim t4 As Task = t3.ContinueWith(Sub()

                                         Dim homeMain As New home
                                         frmMain.NavigationService.Navigate(homeMain)
                                         frmfoot.Visibility = Windows.Visibility.Hidden
                                         t3.Dispose()
                                    
                                     End Sub, TaskScheduler.FromCurrentSynchronizationContext())
    'Dim threadA As New Thread(AddressOf TrainClassfier2)
    Public Sub New()
        ' This call is required by the designer.
        Try
            InitializeComponent()
            Dim screenWidth As Double = System.Windows.SystemParameters.PrimaryScreenWidth
            Dim screenHeight As Double = System.Windows.SystemParameters.PrimaryScreenHeight
            Dim windowWidth As Double = Me.Width
            Dim windowHeight As Double = Me.Height
            Me.Left = (screenWidth / 2) - (windowWidth / 2)
            Me.Top = (screenHeight / 2) - (windowHeight / 2)

            ' Add any initialization after the InitializeComponent() call.
            Dim splashScreen As New Splash
            splashScreen.Show()
            Thread.Sleep(2000)
            splashScreen.Close()
            Dim homeMain As New home
            frmMain.NavigationService.Navigate(homeMain)
        Catch ex As Exception
            Dim logError As New ErrorLogger(ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
            Try
                InitializeComponent()
            Catch ex2 As Exception
                Dim logError2 As New ErrorLogger(ex2.Message.ToString, ex2.StackTrace.ToString, ErrorLogger.ErrorType.FatalError)
            End Try
        End Try
    End Sub

    Private Sub button_Click(sender As Object, e As RoutedEventArgs) Handles button.Click
        Dim frmSetupProject As New SetupProject
        frmMain.NavigationService.Navigate(frmSetupProject)

    End Sub

    Public Sub RefreshFrame()
        frmMain.Refresh()

    End Sub

    Private Sub button1_Click(sender As Object, e As RoutedEventArgs) Handles button1.Click

        Dim mainPage As New home
        frmMain.NavigationService.Navigate(mainPage)


        Dim currentSettings As New ApplicationSettings
        Dim selectedModel As String

        selectedModel = currentSettings.Classification.ToString

        Dim result As MessageBoxResult = MessageBox.Show("Do you wish to continue building a " & selectedModel & " classification model?", "Confirm Continuation", MessageBoxButton.YesNo, MessageBoxImage.Question)
      
        If result = MessageBoxResult.No Then
            Exit Sub
        End If


        Try

            If t.Status.ToString = TaskStatus.Running.ToString Then
                MsgBox("Classifier training is already engaged!")
                Exit Sub
            End If
            frmfoot.NavigationService.Navigate(pBar)
            frmfoot.Visibility = Windows.Visibility.Visible
            If t.Status.ToString = TaskStatus.RanToCompletion.ToString Then
                t = Nothing
                t = New Task(Sub()
                                 Parallel.Invoke(Sub()
                                                     TrainClassifier()
                                                 End Sub,
                                                 Sub()
                                                         TrainClassfier2()
                                                     End Sub)
                             End Sub)
                Dim t2 As Task = t.ContinueWith(Sub()
                                                    Dim homeMain As New home
                                                    frmMain.Refresh()
                                                    frmMain.NavigationService.Navigate(homeMain)
                                                    frmfoot.Visibility = Windows.Visibility.Hidden
                                                    Try
                                                        My.Computer.Audio.Play("Resources\glass ping.wav", AudioPlayMode.Background)
                                                        t.Dispose()
                                                    Catch ex As Exception
                                                        t.Dispose()
                                                        Exit Sub
                                                    End Try


                                                End Sub, TaskScheduler.FromCurrentSynchronizationContext())

            End If
            t.Start()

        Catch ex As Exception
            MessageBox.Show("NBC is experiencing difficulty training your classifier." _
                            & "Please make sure that the folders set in the project have the necessary files." & ex.Message)
            Exit Sub
        End Try

    End Sub

    Public Sub TrainClassifier()
        Dim classify As New TrainClassifier
        Dim session As New SessionData
        Dim rowActivity As New LogActivity
        Dim rowActivity2 As New LogActivity

        Try
            rowActivity.Activity = "Build Model" 'Add variable using the xml
            rowActivity.Datetime = CStr(Now)
            rowActivity.Status = "Start"
            rowActivity.Update = "Building training model in folder: " & session.FolderPath1.ToString
            activityLog.Add(rowActivity)
            'rowActivity.AppendToLog(rowActivity)
            classify.BuildTrainingModel(session.FolderPath1)
            rowActivity2.Activity = "Build Model"
            rowActivity2.Datetime = CStr(Now)
            rowActivity2.Status = "Finished"
            rowActivity2.Update = "Training model built in folder: " & session.FolderPath1.ToString
            activityLog.Add(rowActivity2)
            classify = Nothing
            session = Nothing
            rowActivity = Nothing

        Catch ex As Exception
            Dim logError As New ErrorLogger("First pass exception" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
            Exit Sub
        End Try
    End Sub

    Public Sub TrainClassfier2()
        Dim classify As New TrainClassifier
        Dim session As New SessionData
        Dim rowActivity As New LogActivity
        Dim rowActivity2 As New LogActivity

        Try
            rowActivity.Activity = "Build Model" 'Add variable using the xml
            rowActivity.Datetime = CStr(Now)
            rowActivity.Status = "Start"
            rowActivity.Update = "Building training model in folder: " & session.FolderPath2.ToString
            classify.BuildTrainingModel(session.FolderPath2)
            activityLog.Add(rowActivity)
            'rowActivity.AppendToLog(rowActivity)
            rowActivity2.Activity = "Build Model"
            rowActivity2.Datetime = CStr(Now)
            rowActivity2.Status = "Finished"
            rowActivity2.Update = "Training model built in folder: " & session.FolderPath2.ToString
            activityLog.Add(rowActivity2)
            'rowActivity.AppendToLog(rowActivity)
            classify = Nothing
            session = Nothing
            rowActivity = Nothing
        Catch ex As Exception
            Dim logError As New ErrorLogger("second pass exception" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
            Exit Sub
        End Try
    End Sub

    Public Sub WriteToLog()
        Dim acLog As New LogActivity
        For Each acLog In activityLog
            acLog.AppendToLog(acLog)
        Next



        'Do Until t3.Status = TaskStatus.RanToCompletion
        '    frmMain.NavigationService.Navigate(homeMain)
        '    frmfoot.Visibility = Windows.Visibility.Hidden
        '    ' Me.Dispatcher.BeginInvoke(DispatcherPriority.Normal, New ThreadStart(AddressOf RefreshFrame))
        'Loop
        activityLog = Nothing
      
    End Sub

    Private Sub home_Click(sender As Object, e As RoutedEventArgs) Handles home.Click
        Dim homeMain As New home
        frmMain.NavigationService.Navigate(homeMain)
    End Sub

    Private Sub button3_Click(sender As Object, e As RoutedEventArgs) Handles button3.Click
        If t3.Status.ToString = TaskStatus.Running.ToString Then
            MsgBox("Classifier training is already engaged!")
            Exit Sub
        End If
        If t3.Status.ToString = TaskStatus.RanToCompletion.ToString Then
            t3 = Nothing
            t3 = New Task(Sub()
                              ClassifyUnknownDocument()
                          End Sub)
            Dim t4 As Task = t3.ContinueWith(Sub()


                                                 frmfoot.Visibility = Windows.Visibility.Hidden
                                                 t3.Dispose()
                                             End Sub, TaskScheduler.FromCurrentSynchronizationContext())
        End If
        frmfoot.NavigationService.Navigate(pBar)
        '  frmfoot.Visibility = Windows.Visibility.Visible
        t3.Start()

    End Sub

    Private Sub ClassifyUnknownDocument()
        Dim Session As New SessionData
        Dim Classify As New ClassifyUnknown

        Dim Retrieve1 As New RetrieveModel
        Dim Retrieve2 As New RetrieveModel
        Dim mod1(,) As String
        Dim mod2(,) As String
        Dim activity As New LogActivity

        activity.Activity = "Classify Unknown" 'Add variable using the xml
        activity.Datetime = CStr(Now)
        activity.Status = "Start"
        activity.Update = "Classified all Unknown files in folder: " & Session.UnknownFolder.ToString
        activity.AppendToLog(activity)
        Retrieve1.setModel(Session.FolderPath1)
        Retrieve2.setModel(Session.FolderPath2)
        mod1 = CType(Retrieve1.Model, String(,))
        mod2 = CType(Retrieve2.Model, String(,))

        Classify.Model1 = mod1
        Classify.Model2 = mod2

        Try
            Classify.ClassifyUnknown(Session.UnknownFolder)
        Catch ex As Exception
            Dim logError As New ErrorLogger("User prompted to make sure they have added unclassified files for classification" & ex.Message.ToString, ex.StackTrace.ToString, ErrorLogger.ErrorType.Warning)
            MsgBox("Please make sure you have added unclassified files for classification.")
            activity = Nothing
            Retrieve1 = Nothing
            Retrieve2 = Nothing
            Classify = Nothing
            mod1 = Nothing
            mod2 = Nothing
            Exit Sub
        End Try

        activity.Activity = "Classify Unknown" 'Add variable using the xml
        activity.Datetime = CStr(Now)
        activity.Status = "Finish"
        activity.Update = "Classified all Unknown files in folder: " & Session.UnknownFolder.ToString
        activity.AppendToLog(activity)
        activity = Nothing
        Retrieve1 = Nothing
        Retrieve2 = Nothing
        Classify = Nothing
        mod1 = Nothing
        mod2 = Nothing

    End Sub

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()

    End Sub

    Private Sub MenuItem_Click_1(sender As Object, e As RoutedEventArgs)
        Dim settingConfig As New ProjectConfig
        settingConfig.ShowDialog()

    End Sub

    Private Sub button6_Click(sender As Object, e As RoutedEventArgs) Handles button6.Click
    End Sub
End Class
