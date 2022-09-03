Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml

Public Class Form2

    Dim uName As String = Environment.UserName
    Dim appPath As String = My.Application.Info.DirectoryPath
    Dim ODIR As String = appPath + "/OutputDir.txt"

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If File.Exists("option.xml") Then
            Dim xmlDoc As New XmlDocument()
            xmlDoc.Load("option.xml")
            'フォルダパス
            TextBox2.Text = If(Not xmlDoc.DocumentElement.SelectSingleNode("options/folpath") Is Nothing, xmlDoc.DocumentElement.SelectSingleNode("options/folpath").InnerText, "")
            ComboBox4.SelectedIndex = Integer.Parse(If(Not xmlDoc.DocumentElement.SelectSingleNode("options/language") Is Nothing, xmlDoc.DocumentElement.SelectSingleNode("options/language").InnerText, ""))
            ComboBox3.SelectedIndex = Integer.Parse(If(Not xmlDoc.DocumentElement.SelectSingleNode("options/defab") Is Nothing, xmlDoc.DocumentElement.SelectSingleNode("options/defab").InnerText, ""))
            CheckBox3.Checked = Boolean.Parse(If(Not xmlDoc.DocumentElement.SelectSingleNode("options/hidetab") Is Nothing, xmlDoc.DocumentElement.SelectSingleNode("options/hidetab").InnerText, ""))
        End If
    End Sub
    Private Sub Form2_FormClosing(sender As Object, e As EventArgs) Handles MyBase.FormClosing
        Dim sw As New StreamWriter("option.xml", False,
        Encoding.GetEncoding("utf-8"))
        sw.Write("<?xml version=""1.0"" encoding=""utf-8"" ?>" & Chr(13) & "<configration>" & Chr(13) & "<options>" & Chr(13) & "<folpath>" & TextBox2.Text & "</folpath>" & Chr(13) & "<language>" & ComboBox4.SelectedIndex & "</language>" & Chr(13) & "<defab>" & ComboBox3.SelectedIndex & "</defab>" & Chr(13) & "<hidetab>" & CheckBox3.Checked & "</hidetab>" & Chr(13) & "</options>" & Chr(13) & "</configration>")
        sw.Close()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "ダウンロード先フォルダを指定してください。"
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        fbd.SelectedPath = "C:\Windows\" + uName + "\Desktop"
        If fbd.ShowDialog(Me) = DialogResult.OK Then
            TextBox2.Text = fbd.SelectedPath
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'サムネDL
        If (ComboBox2.Text = "サムネをダウンロード") Then
            TextBox3.Text = "--write-thumbnail --skip-download --convert-thumbnails png "
        Else
            '秒数指定
            If (TextBox6.Text <> "" And TextBox7.Text <> "") Then
                TextBox3.Text = "--download-sections *" + TextBox6.Text + "-" + TextBox7.Text + " "
            ElseIf (TextBox6.Text = "" Or TextBox7.Text = "") Then
                If (TextBox6.Text = "" And TextBox7.Text = "") Then

                Else
                    MsgBox("秒数はどちらも指定してください。")
                    Return
                End If
            End If
            If (ComboBox2.Text = "動画をダウンロード") Then
                '画質設定
                If (ComboBox1.SelectedIndex = -1) Then
                    MsgBox("画質設定が何も指定されていません。")
                    Return
                End If
                If (ComboBox1.SelectedIndex = 0) Then

                End If
                If (ComboBox1.SelectedIndex = 1) Then
                    TextBox3.Text = TextBox3.Text + "-f " + Chr(34) + "bv+ba/b" + Chr(34) + " --merge-output-format mp4 "
                End If
                If (ComboBox1.SelectedIndex = 2) Then
                    TextBox3.Text = TextBox3.Text + "-f " + Chr(34) + "137+ba" + Chr(34) + " --merge-output-format mp4 "
                End If
                If (ComboBox1.SelectedIndex = 3) Then
                    TextBox3.Text = TextBox3.Text + "-f " + Chr(34) + "136+ba" + Chr(34) + " --merge-output-format mp4 "
                End If
                If (ComboBox1.SelectedIndex = 4) Then
                    TextBox3.Text = TextBox3.Text + "-f " + Chr(34) + "135+ba" + Chr(34) + " --merge-output-format mp4 "
                End If
                If (ComboBox1.SelectedIndex = 5) Then
                    TextBox3.Text = TextBox3.Text + "-f " + Chr(34) + "134+ba" + Chr(34) + " --merge-output-format mp4 "
                End If
                If (ComboBox1.SelectedIndex = 6) Then
                    TextBox3.Text = TextBox3.Text + "-f " + Chr(34) + TextBox4.Text + Chr(34) + " --merge-output-format mp4 "
                End If

            End If
        End If
        '音声DL
        If (ComboBox2.Text = "音声をダウンロード") Then
            TextBox3.Text = TextBox3.Text + "-x --extract-audio --audio-format mp3 "
        End If
        '保存場所
        If (TextBox2.Text = "") Then
        Else
            TextBox3.Text = TextBox3.Text + "-o " + TextBox2.Text + "\%(title)s.%(ext)s "
        End If
        '動画URLの検問
        If (TextBox1.Text = "") Then
            MsgBox("動画のURLを指定してください。")
            Return
        End If
        '画質設定の検問
        If (ComboBox2.SelectedIndex = -1) Then
            MsgBox("なにをダウンロードするか指定してください。")
            Return
        End If
        If (CheckBox1.Checked = True) Then
            'ブラウザ設定検問
            If (ComboBox3.SelectedIndex = -1) Then
                MsgBox("あなたのブラウザを指定してください。")
                Return
            End If
        End If
        If (CheckBox2.Checked = True) Then
            'クッキー用でURL開く
            Process.Start(TextBox1.Text)
        End If
        'hidetab
        Dim tabop As Integer = ProcessWindowStyle.Normal
        If (CheckBox3.Checked = True) Then
            tabop = ProcessWindowStyle.Hidden
        Else
            tabop = ProcessWindowStyle.Normal
        End If
        If (CheckBox1.Checked = True) Then
            Dim DL As New ProcessStartInfo With {.FileName = Environment.GetEnvironmentVariable("ComSpec"), .WindowStyle = tabop, .Arguments = "/c yt-dlp --cookies-from-browser " + ComboBox3.Text + " " + TextBox3.Text + TextBox1.Text
        }
            Dim p1 As Process = Process.Start(DL)
        Else
            Dim DL As New ProcessStartInfo With {.FileName = Environment.GetEnvironmentVariable("ComSpec"), .WindowStyle = tabop, .Arguments = "/c yt-dlp " + TextBox3.Text + TextBox1.Text
            }
            Dim p1 As Process = Process.Start(DL)
        End If
        TextBox3.Text = ""
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RichTextBox1.Text = ""
        Dim p As New Process()
        p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec")
        p.StartInfo.UseShellExecute = False
        p.StartInfo.RedirectStandardOutput = True
        p.StartInfo.RedirectStandardInput = False
        p.StartInfo.CreateNoWindow = True
        If (TextBox5.Text = "") Then
            MsgBox("URLを指定してください")
        End If
        p.StartInfo.Arguments = "/c yt-dlp -F " + TextBox5.Text
        p.Start()
        Dim results As String = p.StandardOutput.ReadToEnd()
        p.WaitForExit()
        'p.Close()
        RichTextBox1.Text = RichTextBox1.Text + results
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If Clipboard.ContainsText() Then
            TextBox1.Text = Clipboard.GetText()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Clipboard.ContainsText() Then
            TextBox5.Text = Clipboard.GetText()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        TextBox1.Text = ""
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        TextBox5.Text = ""
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If (ComboBox1.SelectedIndex = 6) Then
            TextBox4.Enabled = True
        Else
            TextBox4.Enabled = False
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If (ComboBox2.SelectedIndex = 0) Then
            ComboBox1.Enabled = True
        Else
            ComboBox1.Enabled = False
            TextBox4.Enabled = False
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://discord.com/invite/28DTTBJ4w7")
    End Sub

End Class
