Imports CefSharp
Imports CefSharp.WinForms
Imports System.Net
Imports System.Threading
Imports System.Diagnostics
Imports AutoUpdaterDotNET

Public Class Form1
    Public Property tecla1 As Integer
    Public Property tecla2 As Integer

    Const MOUSEEVENTF_LEFTDOWN As UInteger = &H2
    Const MOUSEEVENTF_LEFTUP As UInteger = &H4
    Public Declare Sub mouse_event Lib "user32" (ByVal dwFlags As UInteger, ByVal dx As UInteger, ByVal dy As UInteger, ByVal dwData As UInteger, ByVal dwExtraInfo As Integer)
    'LEFTDOWN = &H2
    'LEFTUP = &H4
    'RIGHTDOWN = &H8
    'RIGHTUP = &H10
    'MIDDLEUP = &H40
    'MIDDLEDOWN = &H20

    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short
    Public Declare Auto Function GetCursorPos Lib "User32.dll" (ByRef lpPoint As Point) As Integer
    Dim mousepos As Point

    Private WithEvents navegador As ChromiumWebBrowser

    Dim Cliente As New WebClient
    Public Sub New()
        InitializeComponent()
        Dim config As New CefSettings
        CefSharp.Cef.Initialize(config)

        navegador = New ChromiumWebBrowser("https://adivinhadorgartic.tk/")

        AddHandler navegador.FrameLoadStart, AddressOf BrowserOnFrameLoadStart
        AddHandler navegador.FrameLoadEnd, AddressOf BrowserOnFrameLoadEnd

        Panel1.Controls.Add(navegador)
    End Sub

    Public Sub LeftClick()
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0)
        Thread.Sleep(100) 'Wait required
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Form1.ActiveForm.TopMost = True
        Else
            Form1.ActiveForm.TopMost = False
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        navegador.Load("https://adivinhadorgartic.tk/")
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AutoUpdater.Start("https://raw.githubusercontent.com/vin350/Adivinhador-do-Gartic/master/adivinhador-version.xml")

        ProgressBar1.Invoke(
            Sub()
                ProgressBar1.Style = ProgressBarStyle.Marquee
                ProgressBar1.Refresh()
            End Sub
            )

        Timer2.Start()

        Label3.Text = My.Settings.Tecla1
        Label4.Text = My.Settings.Tecla2
        TextBox1.Text = My.Settings.Xpos
        TextBox2.Text = My.Settings.Ypos
        If My.Settings.Tecla1 = "" Then
            Form2.TextBox1.Text = "113"
        Else
            Form2.TextBox1.Text = Char.ConvertFromUtf32(My.Settings.Tecla1)
        End If

        If My.Settings.Tecla2 = "" Then
            Form2.TextBox2.Text = "115"
        Else
            Form2.TextBox2.Text = Char.ConvertFromUtf32(My.Settings.Tecla2)
        End If

    End Sub

    Private Sub BrowserOnFrameLoadStart(sender As Object, e As FrameLoadStartEventArgs)
        ProgressBar1.Invoke(
            Sub()
                ProgressBar1.Style = ProgressBarStyle.Marquee
                ProgressBar1.Refresh()
            End Sub
            )

    End Sub

    Private Sub BrowserOnFrameLoadEnd(sender As Object, e As FrameLoadEndEventArgs)
        ProgressBar1.Invoke(
            Sub()
                ProgressBar1.Style = ProgressBarStyle.Blocks
                ProgressBar1.Refresh()
            End Sub
            )

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form2.ShowDialog()
        Form2.TextBox1.Text = Char.ConvertFromUtf32(My.Settings.Tecla1)
        Form2.TextBox2.Text = Char.ConvertFromUtf32(My.Settings.Tecla2)
    End Sub

    Private Sub Button4_Click(sender As Object, e As MouseEventArgs) Handles Button4.Click
        MsgBox("1- Primeiro mova o mouse em cima de 'Responda aqui...' (no gartic)
2- Depois clicke F2 para capturar a cordenada do mouse.
3- Em seguida copie a palavra que deseja responder.
4- Por ultimo clicke F4 para enviar a resposta.

TECLAS PADRÕES:
F2 para capturar cordenadas do mouse.
F4 envia a resposta.

Marque a caixa TOPO para o app ficar em cima da tela sempre.", , "Como Usar")
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If Label3.Text = "" Then
            Label3.Text = "113"
            My.Settings.Tecla1 = Label3.Text
        End If
        If Label4.Text = "" Then
            Label4.Text = "115"
            My.Settings.Tecla2 = Label4.Text
        End If
        If (GetAsyncKeyState(Label3.Text)) Then
            Dim R As Long = GetCursorPos(mousepos)
            TextBox1.Text = mousepos.X
            TextBox2.Text = mousepos.Y
            My.Settings.Xpos = mousepos.X
            My.Settings.Ypos = mousepos.Y
        ElseIf (GetAsyncKeyState(Label4.Text)) Then
            If TextBox1.Text <> "" And TextBox2.Text <> "" Then
                Dim R As Long = GetCursorPos(mousepos)
                Dim Xpos As String = mousepos.X
                Dim Ypos As String = mousepos.Y
                Me.Cursor = New Cursor(Cursor.Current.Handle)
                Cursor.Position = New Point(TextBox1.Text, TextBox2.Text)
                LeftClick()
                SendKeys.Send("^v")
                Thread.Sleep(200)
                SendKeys.Send("{ENTER}")
                Cursor.Position = New Point(Xpos, Ypos)
            Else
                MsgBox("NÃO se esqueça de usar o F2!", MsgBoxStyle.Critical, "Error!")
            End If
        End If
    End Sub
End Class