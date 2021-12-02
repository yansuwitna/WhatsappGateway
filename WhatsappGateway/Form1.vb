Imports System.IO
Imports System.Net
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Form1
    Private Sub KeluarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KeluarToolStripMenuItem.Click
        Dim jwb = MsgBox("Yakin Keluar Aplikasi?", MsgBoxStyle.YesNo, "Whatsapp Gateway")
        If jwb = MsgBoxResult.Yes Then
            End
        End If
    End Sub

    Private Sub MulaiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MulaiToolStripMenuItem.Click


        If (Label2.Text = "") Then
            MsgBox("URL Masih Kosong", MsgBoxStyle.Critical, "Whatsapp Gateway")
        Else
            If (MulaiToolStripMenuItem.Text = "Mulai") Then
                Dim jwb = MsgBox("Yakin Mulai Proses?", MsgBoxStyle.YesNo, "Mulai")
                If (jwb = vbYes) Then
                    MulaiToolStripMenuItem.Text = "Stop"
                    Timer1.Enabled = True
                End If
            Else
                Label1.Text = "STOP"
                MulaiToolStripMenuItem.Text = "Mulai"
                Timer1.Enabled = False
                Button1.BackColor = Color.Red

            End If
        End If

    End Sub

    Private Sub URLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles URLToolStripMenuItem.Click
        Dim jwb = InputBox("Masukkan URL", "URL WEB", Label2.Text)
        If (jwb = "") Then
            MsgBox("URL Tidak Boleh Kosong", MsgBoxStyle.Critical, "URL")
        Else

            My.Computer.FileSystem.WriteAllText("config.ini", jwb, False)
            Using temp As New IO.StreamReader("config.ini")
                Label2.Text = temp.ReadLine
            End Using
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False

        Dim token As String
        token = Label5.Text

        Dim webClient As New System.Net.WebClient
        On Error GoTo Error_CEK
        Dim responseFromServer As String = webClient.DownloadString(Label2.Text & "?p=0&token=" & Label5.Text)




        If responseFromServer = "0" Then
            MsgBox("Token Anda Salah", MsgBoxStyle.Critical, "TOKEN")
            MulaiToolStripMenuItem.Text = "Mulai"
            Label1.Text = "STOP"
            Timer1.Enabled = False
        Else
            Button1.BackColor = Color.Yellow
            Label1.Text = "AKTIF"

            Dim json As String = responseFromServer
            On Error GoTo Error2
            Dim ser As JObject = JObject.Parse(json)
            Dim data As List(Of JToken) = ser.Children().ToList


            For Each item As JProperty In data
                item.CreateReader()
                Select Case item.Name
                    Case "isi"
                        For Each isi As JObject In item.Values
                            System.Threading.Thread.Sleep(10000)
                            'Proses Kirim Data
                            Button1.BackColor = Color.Green
                            Dim web As New WebBrowser
                            Dim id As String = isi("kode")
                            Dim hp As String = isi("hp")
                            Dim pesan As String = isi("pesan")
                            Dim url = "whatsapp://send?phone=" & hp & "&text=" & pesan
                            web.Navigate(url)
                            Label3.Text = hp
                            Label4.Text = pesan
                            System.Threading.Thread.Sleep(1000)
                            SendKeys.Send("{Enter}")
                            Button1.BackColor = Color.Yellow

                            'Proses Merubah Data Di Server
                            Dim webClient2 As New System.Net.WebClient
                            On Error Resume Next
                            webClient2.DownloadString(Label2.Text & "?p=1&token=" & Label5.Text & "&kode=" & id)
                        Next

                End Select
            Next
            Timer1.Enabled = True
        End If

        Exit Sub
Error_CEK:
        MulaiToolStripMenuItem.Text = "Mulai"
        Label1.Text = "STOP"
        Timer1.Enabled = False
        MsgBox("WEB TIDAK BISA DIAKSES, CEK KEMBALI URL-NYA", MsgBoxStyle.Critical, "ERROR")
        Exit Sub
Error2:
        Button1.BackColor = Color.Yellow
        Label3.Text = "Belum Ada Data"
        Label4.Text = ""
        Timer1.Enabled = True
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button1.BackColor = Color.Red
        Using temp As New IO.StreamReader("config.ini")
            Label2.Text = temp.ReadLine
        End Using
        On Error Resume Next
        Using temp2 As New IO.StreamReader("token.ini")
            Label5.Text = temp2.ReadLine
        End Using
    End Sub

    Private Sub TokenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TokenToolStripMenuItem.Click
        Dim jwb = InputBox("Masukkan TOKEN", "TOKEN WEB", Label5.Text)
        If (jwb = "") Then
            MsgBox("Token Tidak Boleh Kosong", MsgBoxStyle.Critical, "TOKEN")
        Else

            My.Computer.FileSystem.WriteAllText("token.ini", jwb, False)
            Using temp As New IO.StreamReader("token.ini")
                Label5.Text = temp.ReadLine
            End Using
        End If
    End Sub

    Private Sub TataCaraToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TataCaraToolStripMenuItem.Click
        Dim MyForm As New Form2
        MyForm.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If (Label2.Text = "") Then
            MsgBox("URL Masih Kosong", MsgBoxStyle.Critical, "Whatsapp Gateway")
        Else
            If (MulaiToolStripMenuItem.Text = "Mulai") Then
                Dim jwb = MsgBox("Yakin Mulai Proses?", MsgBoxStyle.YesNo, "Mulai")
                If (jwb = vbYes) Then
                    MulaiToolStripMenuItem.Text = "Stop"
                    Timer1.Enabled = True
                End If
            Else
                Label1.Text = "STOP"
                MulaiToolStripMenuItem.Text = "Mulai"
                Timer1.Enabled = False
                Button1.BackColor = Color.Red

            End If
        End If
    End Sub
End Class
