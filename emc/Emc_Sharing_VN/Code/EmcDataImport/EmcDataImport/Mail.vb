Imports System
Imports System.Collections
Imports System.Text
Imports System.Text.RegularExpressions

''' <summary>
''' メールヘッダ部を取得するためのクラスです。
''' </summary>
Public Class MailHeader

    ''' <summary>メールヘッダ部</summary>
    Dim mailheader As String

    ''' <summary>
    ''' コンストラクタです。
    ''' </summary>
    ''' <param name="mail">メール本体。</param>
    Public Sub New(ByVal mail As String)
        ' メールのヘッダ部とボディ部は 1つ以上の空行でわけられています。
        ' 正規表現を使ってヘッダ部のみを取り出します。
        Dim reg As Regex = New Regex("^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline)
        Dim m As Match = reg.Match(mail)
        Me.mailheader = m.Groups("header").Value
    End Sub

    ''' <summary>
    ''' ヘッダ部全体を返します。
    ''' </summary>
    Public ReadOnly Property Text() As String
        Get
            Return Me.mailheader
        End Get
    End Property

    ''' <summary>
    ''' ヘッダの各行を返します。
    ''' name に null、もしくは、空文字列を渡した場合はすべてのヘッダを返します。
    ''' </summary>
    Default Public ReadOnly Property Item(ByVal name As String) As String()
        Get
            ' Subject: line1
            '          line2
            ' のように複数行に分かれているヘッダを
            ' Subject: line1 line2
            ' となるように 1行にまとめます。
            Dim header As String = Regex.Replace(Me.mailheader, "\r\n\s+", " ")

            If Not name Is Nothing AndAlso name <> "" Then
                If Not name.EndsWith(":") Then
                    name += ":"
                End If
                name = name.ToLower()
            Else
                name = Nothing
            End If

            ' name に一致するヘッダのみを抽出
            Dim ary As ArrayList = New ArrayList
            For Each line As String In header.Replace(vbCrLf, vbLf).Split(vbLf)
                If name Is Nothing OrElse line.ToLower().StartsWith(name) Then
                    ary.Add(line)
                End If
            Next

            Return CType(ary.ToArray(GetType(String)), String())
        End Get
    End Property


    ''' <summary>
    ''' デコードします。
    ''' </summary>
    ''' <param name="encodedtext">デコードする文字列。</param>
    ''' <returns>デコードした結果。</returns>
    Public Shared Function Decode(ByVal encodedtext As String) As String
        Dim decodedtext As String = ""
        Do While encodedtext <> ""
            Dim r As Regex = New Regex("^(?<preascii>.*?)(?:=\?(?<charset>.+?)\?(?<encoding>.+?)\?(?<encodedtext>.+?)\?=)+(?<postascii>.*?)$")
            Dim m As Match = r.Match(encodedtext)
            If m.Groups("charset").Value = "" OrElse m.Groups("encoding").Value = "" OrElse m.Groups("encodedtext").Value = "" Then
                ' エンコードされた文字列はない
                decodedtext += encodedtext
                encodedtext = ""
            Else
                decodedtext += m.Groups("preascii").Value
                If m.Groups("encoding").Value = "B" Then
                    Dim c As Char() = m.Groups("encodedtext").Value.ToCharArray()
                    Dim b As Byte() = Convert.FromBase64CharArray(c, 0, c.Length)
                    Dim s As String = Encoding.GetEncoding(m.Groups("charset").Value).GetString(b)
                    decodedtext += s
                Else
                    ' 未サポート
                    decodedtext += "=?" & m.Groups("charset").Value & "?" & m.Groups("encoding").Value & "?" & m.Groups("encodedtext").Value & "?="
                End If
                encodedtext = m.Groups("postascii").Value
            End If
        Loop
        Return decodedtext
    End Function

End Class

''' <summary>
''' メールボディ部を取得するためのクラスです。
''' </summary>
Public Class MailBody

    ''' <summary>メールボディ部</summary>
    Dim mailbody As String

    ''' <summary>各マルチパート部のコレクション</summary>
    Private _multiparts As MailMultipart()

    ''' <summary>
    ''' コンストラクタです。
    ''' </summary>
    ''' <param name="mail">メール本体。</param>
    Public Sub New(ByVal mail As String)
        ' メールのヘッダ部とボディ部は 1つ以上の空行でわけられています。
        ' 正規表現を使ってヘッダ部、ボディ部を取り出します。
        Dim reg As Regex = New Regex("^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline)
        Dim m As Match = reg.Match(mail)
        Dim mailheader As String = m.Groups("header").Value
        Me.mailbody = m.Groups("body").Value

        reg = New Regex("Content-Type:\s+multipart/mixed;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase)
        m = reg.Match(mailheader)

        Dim rs As String
        rs = m.Groups("boundary").Value
        If m.Groups("boundary").Value <> "" Then

            Dim reg1 As Regex = New Regex("Content-Type:\s+multipart/alternative;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase)
            Dim m1 As Match = reg1.Match(Me.mailbody)
            If m1.Groups("boundary").Value <> "" Then
                ' multipart
                Dim boundary As String = m1.Groups("boundary").Value
                reg1 = New Regex("^.*?--" & boundary & "\r\n(?:(?<multipart>.*?)" & "--" & boundary & "-*\r\n)+.*?$", RegexOptions.Singleline)
                m1 = reg1.Match(Me.mailbody)
                Dim ary As ArrayList = New ArrayList
                For i As Integer = 0 To m1.Groups("multipart").Captures.Count - 1
                    If m1.Groups("multipart").Captures(i).Value <> "" Then
                        Dim str1 As String = m1.Groups("multipart").Captures(i).Value
                        Dim b As MailMultipart = New MailMultipart(m1.Groups("multipart").Captures(i).Value)
                        ary.Add(b)
                    End If
                Next
                Me._multiparts = CType(ary.ToArray(GetType(MailMultipart)), MailMultipart())

            Else
                ' multipart
                Dim boundary As String = m.Groups("boundary").Value
                reg = New Regex("^.*?--" & boundary & "\r\n(?:(?<multipart>.*?)" & "--" & boundary & "-*\r\n)+.*?$", RegexOptions.Singleline)
                m = reg.Match(Me.mailbody)
                Dim ary As ArrayList = New ArrayList
                For i As Integer = 0 To m.Groups("multipart").Captures.Count - 1
                    If m.Groups("multipart").Captures(i).Value <> "" Then
                        Dim str1 As String = m.Groups("multipart").Captures(i).Value
                        Dim b As MailMultipart = New MailMultipart(m.Groups("multipart").Captures(i).Value)
                        ary.Add(b)
                    End If
                Next
                Me._multiparts = CType(ary.ToArray(GetType(MailMultipart)), MailMultipart())
            End If

        Else
            reg = New Regex("Content-Type:\s+multipart/alternative;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase)
            m = reg.Match(mailheader)

            Dim rs1 As String
            rs1 = m.Groups("boundary").Value
            If m.Groups("boundary").Value <> "" Then
                ' multipart
                Dim boundary As String = m.Groups("boundary").Value
                reg = New Regex("^.*?--" & boundary & "\r\n(?:(?<multipart>.*?)" & "--" & boundary & "-*\r\n)+.*?$", RegexOptions.Singleline)
                m = reg.Match(Me.mailbody)
                Dim ary As ArrayList = New ArrayList
                For i As Integer = 0 To m.Groups("multipart").Captures.Count - 1
                    If m.Groups("multipart").Captures(i).Value <> "" Then
                        Dim str1 As String = m.Groups("multipart").Captures(i).Value
                        Dim b As MailMultipart = New MailMultipart(m.Groups("multipart").Captures(i).Value)
                        ary.Add(b)
                    End If
                Next
                Me._multiparts = CType(ary.ToArray(GetType(MailMultipart)), MailMultipart())
            Else
                Me._multiparts = Array.CreateInstance(GetType(MailMultipart), 0)
            End If

        End If

    End Sub

    ''' <summary>
    ''' ボディ部全体を返します。
    ''' </summary>
    Public ReadOnly Property Text() As String
        Get
            Return Me.mailbody
        End Get
    End Property

    ''' <summary>
    ''' マルチパート部のコレクションを返します。
    ''' </summary>
    Public ReadOnly Property Multiparts() As MailMultipart()
        Get
            Return Me._multiparts
        End Get
    End Property

End Class

''' <summary>
''' ひとつのマルチパート部をあらわすクラスです。
''' </summary>
Public Class MailMultipart

    ''' <summary>メール本体</summary>
    Dim mail As String

    ''' <summary>
    ''' コンストラクタです。
    ''' </summary>
    ''' <param name="mail">メール本体。</param>
    Public Sub New(ByVal mail As String)
        Me.mail = mail
    End Sub

    ''' <summary>
    ''' ヘッダ部を取得します。
    ''' </summary>
    Public ReadOnly Property Header() As MailHeader
        Get
            Return New MailHeader(Me.mail)
        End Get
    End Property

    ''' <summary>
    ''' ボディ部を取得します。
    ''' </summary>
    Public ReadOnly Property Body() As MailBody
        Get
            Return New MailBody(Me.mail)
        End Get
    End Property

End Class

''' <summary>
''' メールを表すクラスです。
''' </summary>
Public Class Mail

    ''' <summary>メール本体</summary>
    Dim mail As String

    ''' <summary>
    ''' コンストラクタです。
    ''' </summary>
    ''' <param name="mail">メール本体。</param>
    Public Sub New(ByVal mail As String)
        ' 行頭のピリオド2つをピリオド1つに変換
        Me.mail = Regex.Replace(mail, "\r\n\.\.", vbCrLf & ".")
    End Sub

    ''' <summary>
    ''' ヘッダ部を取得します。
    ''' </summary>
    Public ReadOnly Property Header() As MailHeader
        Get
            Return New MailHeader(Me.mail)
        End Get
    End Property

    ''' <summary>
    ''' ボディ部を取得します。
    ''' </summary>
    Public ReadOnly Property Body() As MailBody
        Get
            Return New MailBody(Me.mail)
        End Get
    End Property

End Class
