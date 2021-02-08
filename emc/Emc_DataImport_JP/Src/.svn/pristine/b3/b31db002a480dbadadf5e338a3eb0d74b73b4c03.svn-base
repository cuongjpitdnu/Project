'******************************************************************
'   TITLE    : clsMail.vb
'   機能     : メールを表すクラス。
'   備考     : 無し.
'   CREATE   : 2020/02/17  AKB Cuong.
'   UPDATE   : .
'
'       2020 AKBSOFTWARE CORPORATION
'******************************************************************

Imports System.Text
Imports System.Text.RegularExpressions


'******************************************************************
'   機能     : メールヘッダ部を取得するためのクラスです。
'   備考     : 無し.
'   CREATE   : 2020/02/17  AKB Cuong.
'   UPDATE   : .
'******************************************************************
Public Class clsMailHeader

    Dim strMailHeader As String        'メールヘッダ部


    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです
    '　　　VALUE      : None.
    '      PARAMS     : (mail String, メール本体)
    '      MEMO       : None.
    '      CREATE     : 2020/02/28　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal mail As String)
        ' メールのヘッダ部とボディ部は 1つ以上の空行でわけられています。
        ' 正規表現を使ってヘッダ部のみを取り出します。
        Dim reg As Regex = New Regex("^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline)
        Dim match As Match = reg.Match(mail)
        Me.strMailHeader = match.Groups("header").Value
    End Sub

    Public ReadOnly Property Text() As String       'ヘッダ部全体を返します。
        Get
            Return Me.strMailHeader
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal name As String) As String()     'ヘッダの各行を返します。name に null、もしくは、空文字列を渡した場合はすべてのヘッダを返します。
        Get
            ' Subject: line1
            '          line2
            ' のように複数行に分かれているヘッダを
            ' Subject: line1 line2
            ' となるように 1行にまとめます。
            Dim strHeader As String = Regex.Replace(Me.strMailHeader, "\r\n\s+", " ")

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
            For Each line As String In strHeader.Replace(vbCrLf, vbLf).Split(vbLf)
                If name Is Nothing OrElse line.ToLower().StartsWith(name) Then
                    ary.Add(line)
                End If
            Next

            Return CType(ary.ToArray(GetType(String)), String())
        End Get
    End Property


    '   ******************************************************************
    '　　　FUNCTION   : デコードします。
    '　　　VALUE      : String デコードした結果
    '      PARAMS     : (encodedtext String, デコードする文字列)
    '      MEMO       : None.
    '      CREATE     : 2020/02/28　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Shared Function Decode(ByVal encodedtext As String) As String
        Dim strDecodedText As String = ""
        Do While encodedtext <> ""
            Dim r As Regex = New Regex("^(?<preascii>.*?)(?:=\?(?<charset>.+?)\?(?<encoding>.+?)\?(?<encodedtext>.+?)\?=)+(?<postascii>.*?)$")
            Dim match As Match = r.Match(encodedtext)
            If match.Groups("charset").Value = "" OrElse match.Groups("encoding").Value = "" OrElse match.Groups("encodedtext").Value = "" Then
                ' エンコードされた文字列はない
                strDecodedText += encodedtext
                encodedtext = ""
            Else
                strDecodedText += match.Groups("preascii").Value
                If match.Groups("encoding").Value = "B" Then
                    Dim c As Char() = match.Groups("encodedtext").Value.ToCharArray()
                    Dim bytResult As Byte() = Convert.FromBase64CharArray(c, 0, c.Length)
                    Dim strResult As String = Encoding.GetEncoding(match.Groups("charset").Value).GetString(bytResult)
                    strDecodedText += strResult
                Else
                    ' 未サポート
                    strDecodedText += "=?" & match.Groups("charset").Value & "?" & match.Groups("encoding").Value & "?" & match.Groups("encodedtext").Value & "?="
                End If
                encodedtext = match.Groups("postascii").Value
            End If
        Loop
        Return strDecodedText
    End Function

End Class


'******************************************************************
'   機能     : メールボディ部を取得するためのクラスです。
'   備考     : 無し.
'   CREATE   : 2020/02/17  AKB Cuong.
'   UPDATE   : .
'******************************************************************
Public Class clsMailBody

    Dim strMailBody As String       'メールボディ部

    Private _multiparts As clsMailMultipart()       '各マルチパート部のコレクション


    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです
    '　　　VALUE      : None.
    '      PARAMS     : (mail String, メール本体)
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal mail As String)
        ' メールのヘッダ部とボディ部は 1つ以上の空行でわけられています。
        ' 正規表現を使ってヘッダ部、ボディ部を取り出します。
        Dim reg As Regex = New Regex("^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline)
        Dim match As Match = reg.Match(mail)
        Dim strMailHeader As String = match.Groups("header").Value
        Me.strMailBody = match.Groups("body").Value

        reg = New Regex("Content-Type:\s+multipart/mixed;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase)
        match = reg.Match(strMailHeader)

        Dim strResul As String
        strResul = match.Groups("boundary").Value
        If match.Groups("boundary").Value <> "" Then

            Dim reg1 As Regex = New Regex("Content-Type:\s+multipart/alternative;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase)
            Dim match1 As Match = reg1.Match(Me.strMailBody)
            If match1.Groups("boundary").Value <> "" Then
                ' multipart
                Dim boundary As String = match1.Groups("boundary").Value
                reg1 = New Regex("^.*?--" & boundary & "\r\n(?:(?<multipart>.*?)" & "--" & boundary & "-*\r\n)+.*?$", RegexOptions.Singleline)
                match1 = reg1.Match(Me.strMailBody)
                Dim array As ArrayList = New ArrayList
                For i As Integer = 0 To match1.Groups("multipart").Captures.Count - 1
                    If match1.Groups("multipart").Captures(i).Value <> "" Then
                        Dim mailMultipart As clsMailMultipart = New clsMailMultipart(match1.Groups("multipart").Captures(i).Value)
                        array.Add(mailMultipart)
                    End If
                Next
                Me._multiparts = CType(array.ToArray(GetType(clsMailMultipart)), clsMailMultipart())

            Else
                ' multipart
                Dim boundary As String = match.Groups("boundary").Value
                reg = New Regex("^.*?--" & boundary & "\r\n(?:(?<multipart>.*?)" & "--" & boundary & "-*\r\n)+.*?$", RegexOptions.Singleline)
                match = reg.Match(Me.strMailBody)
                Dim array As ArrayList = New ArrayList
                For i As Integer = 0 To match.Groups("multipart").Captures.Count - 1
                    If match.Groups("multipart").Captures(i).Value <> "" Then
                        Dim mailMultipart As clsMailMultipart = New clsMailMultipart(match.Groups("multipart").Captures(i).Value)
                        array.Add(mailMultipart)
                    End If
                Next
                Me._multiparts = CType(array.ToArray(GetType(clsMailMultipart)), clsMailMultipart())
            End If

        Else
            reg = New Regex("Content-Type:\s+multipart/alternative;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase)
            match = reg.Match(strMailHeader)

            Dim strResult As String
            strResult = match.Groups("boundary").Value
            If match.Groups("boundary").Value <> "" Then
                ' multipart
                Dim boundary As String = match.Groups("boundary").Value
                reg = New Regex("^.*?--" & boundary & "\r\n(?:(?<multipart>.*?)" & "--" & boundary & "-*\r\n)+.*?$", RegexOptions.Singleline)
                match = reg.Match(Me.strMailBody)
                Dim array As ArrayList = New ArrayList
                For i As Integer = 0 To match.Groups("multipart").Captures.Count - 1
                    If match.Groups("multipart").Captures(i).Value <> "" Then
                        Dim mailMultipart As clsMailMultipart = New clsMailMultipart(match.Groups("multipart").Captures(i).Value)
                        array.Add(mailMultipart)
                    End If
                Next
                Me._multiparts = CType(array.ToArray(GetType(clsMailMultipart)), clsMailMultipart())
            Else
                Me._multiparts = Array.CreateInstance(GetType(clsMailMultipart), 0)
            End If

        End If

    End Sub

    Public ReadOnly Property Text() As String       'ボディ部全体を返します。
        Get
            Return Me.strMailBody
        End Get
    End Property

    Public ReadOnly Property Multiparts() As clsMailMultipart()     'マルチパート部のコレクションを返します。
        Get
            Return Me._multiparts
        End Get
    End Property

End Class


'******************************************************************
'   機能     : ひとつのマルチパート部をあらわすクラスです。
'   備考     : 無し.
'   CREATE   : 2020/02/17  AKB Cuong.
'   UPDATE   : .
'******************************************************************
Public Class clsMailMultipart

    Dim strMail As String       'メール本体

    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです
    '　　　VALUE      : None.
    '      PARAMS     : (mail String, メール本体)
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal mail As String)
        Me.strMail = mail
    End Sub

    Public ReadOnly Property Header() As clsMailHeader      'ヘッダ部を取得します。
        Get
            Return New clsMailHeader(Me.strMail)
        End Get
    End Property

    Public ReadOnly Property Body() As clsMailBody      'ボディ部を取得します。
        Get
            Return New clsMailBody(Me.strMail)
        End Get
    End Property

End Class


'******************************************************************
'   機能     : メールを表すクラスです。。
'   備考     : 無し.
'   CREATE   : 2020/02/17　AKB　Cuong
'   UPDATE   : .
'******************************************************************
Public Class clsMail

    Dim strMail As String       'メール本体


    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです
    '　　　VALUE      : None.
    '      PARAMS     : (mail String, メール本体)
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal mail As String)
        ' 行頭のピリオド2つをピリオド1つに変換
        Me.strMail = Regex.Replace(mail, "\r\n\.\.", vbCrLf & ".")
    End Sub

    Public ReadOnly Property Header() As clsMailHeader      'ヘッダ部を取得します。
        Get
            Return New clsMailHeader(Me.strMail)
        End Get
    End Property

    Public ReadOnly Property Body() As clsMailBody      ' ボディ部を取得します。
        Get
            Return New clsMailBody(Me.strMail)
        End Get
    End Property

End Class
