Public Class Form2

    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.Show()
        Me.Close()
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox1.KeyDown
        TextBox1.Text = Char.ConvertFromUtf32(e.KeyData)
        Form1.Label3.Text = e.KeyCode
        My.Settings.Tecla1 = e.KeyCode
        MsgBox("1ª Tecla definida para " + Char.ConvertFromUtf32(e.KeyData))
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox2.KeyDown
        TextBox2.Text = Char.ConvertFromUtf32(e.KeyData)
        Form1.Label4.Text = e.KeyCode
        My.Settings.Tecla2 = e.KeyCode
        MsgBox("2ª Tecla definida para " + Char.ConvertFromUtf32(e.KeyData))
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form1.Label3.Text = "113"
        Form1.Label4.Text = "115"
        TextBox1.Text = "F2"
        TextBox2.Text = "F4"
        My.Settings.Tecla1 = "113"
        My.Settings.Tecla2 = "115"
        MsgBox("Teclas definidas para F2 e F4!")
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Form1.Timer2.Stop()
    End Sub

    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Form1.Timer2.Start()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        MsgBox("Clicke na linha da tecla que vc quer trocar
depois pressione a tecla que vc quiser

OBS: teclas como F1 F2 ... funcionam mas aparece que vc clickou em outra tecla", , "Como Trocar")
    End Sub
End Class