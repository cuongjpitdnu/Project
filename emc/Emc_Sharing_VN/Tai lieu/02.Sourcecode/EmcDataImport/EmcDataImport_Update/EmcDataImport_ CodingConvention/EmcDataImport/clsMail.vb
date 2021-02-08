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

    Dim mstrMailHeader As String        'メールヘッダ部


    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです
    '　　　VALUE      : None.
    '      PARAMS     : (strMail String, メール本体)
    '      MEMO       : None.
    '      CREATE     : 2020/02/28　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal strMail As String)
        ' メールのヘッダ部とボディ部は 1つ以上の空行でわけられています。
        ' 正規表現を使ってヘッダ部のみを取り出します。
        Dim reg As Regex = New Regex("^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline)
        Dim match As Match = reg.Match(strMail)
        Me.mstrMailHeader = match.Groups("header").Value
    End Sub

    Public ReadOnly Property Text() As String       'ヘッダ部全体を返します。
        Get
            Return Me.mstrMailHeader
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal strName As String) As String()     'ヘッダの各行を返します。strName に null、もしくは、空文字列を渡した場合はすべてのヘッダを返します。
        Get
            ' Subject: line1
            '          line2
            ' のように複数行に分かれているヘッダを
            ' Subject: line1 line2
            ' となるように 1行にまとめます。
            Dim strHeader As String = Regex.Replace(Me.mstrMailHeader, "\r\n\s+", " ")

            If Not strName Is Nothing AndAlso strName <> "" Then
                If Not strName.EndsWith(":") Then
                    strName += ":"
                End If
                strName = strName.ToLower()
            Else
                strName = Nothing
            End If

            ' strName に一致するヘッダのみを抽出
            Dim array As ArrayList = New ArrayList
            For Each strline As String In strHeader.Replace(vbCrLf, vbLf).Split(vbLf)
                If strName Is Nothing OrElse strline.ToLower().StartsWith(strName) Then
                    array.Add(strline)
                End If
            Next

            Return CType(array.ToArray(GetType(String)), String())
        End Get
    End Property


    '   ******************************************************************
    '　　　FUNCTION   : デコードします。
    '　　　VALUE      : String デコードした結果
    '      PARAMS     : (strEncodedtext String, デコードする文字列)
    '      MEMO       : None.
    '      CREATE     : 2020/02/28　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Shared Function Decode(ByVal strEncodedtext As String) As String
        Dim strDecodedText As String = ""
        Do While strEncodedtext <> ""
            Dim regex As Regex = New Regex("^(?<preascii>.*?)(?:=\?(?<charset>.+?)\?(?<encoding>.+?)\?(?<encodedtext>.+?)\?=)+(?<postascii>.*?)$")
            Dim match As Match = regex.Match(strEncodedtext)
            If match.Groups("charset").Value = "" OrElse match.Groups("encoding").Value = "" OrElse match.Groups("encodedtext").Value = "" Then
                ' エンコードされた文字列はない
                strDecodedText += strEncodedtext
                strEncodedtext = ""
            Else
                strDecodedText += match.Groups("preascii").Value
                If match.Groups("encoding").Value = "B" Then
                    Dim charArray As Char() = match.Groups("encodedtext").Value.ToCharArray()
                    Dim bytResult As Byte() = Convert.FromBase64CharArray(charArray, 0, charArray.Length)
                    Dim strResult As String = Encoding.GetEncoding(match.Groups("charset").Value).GetString(bytResult)
                    strDecodedText += strResult
                Else
                    ' 未サポート
                    strDecodedText += "=?" & match.Groups("charset").Value & "?" & match.Groups("encoding").Value & "?" & match.Groups("encodedtext").Value & "?="
                End If
                strEncodedtext = match.Groups("postascii").Value
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

    Dim mstrMailBody As String       'メールボディ部

    Private mclsMailMultiparts As clsMailMultipart()       '各マルチパート部のコレクション


    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです
    '　　　VALUE      : None.
    '      PARAMS     : (strMail String, メール本体)
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal strMail As String)
        ' メールのヘッダ部とボディ部は 1つ以上の空行でわけられています。
        ' 正規表現を使ってヘッダ部、ボディ部を取り出します。
        Dim reg As Regex = New Regex("^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline)
        Dim match As Match = reg.Match(strMail)
        Dim strMailHeader As String = match.Groups("header").Value
        Me.mstrMailBody = match.Groups("body").Value

        reg = New Regex("Content-Type:\s+multipart/mixed;\s+boundary=""(?<boundary>.*?)""|Content-Type:\s+multipart/related;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase)
        match = reg.Match(strMailHeader)

        Dim strResul As String
        strResul = match.Groups("boundary").Value
        If match.Groups("boundary").Value <> "" Then

            Dim reg1 As Regex = New Regex("Content-Type:\s+multipart/alternative;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase)
            Dim match1 As Match = reg1.Match(Me.mstrMailBody)
            If match1.Groups("boundary").Value <> "" Then
                ' multipart
                Dim strBoundary As String = match1.Groups("boundary").Value
                reg1 = New Regex("^.*?--" & strBoundary & "\r\n(?:(?<multipart>.*?)" & "--" & strBoundary & "-*\r\n)+.*?$", RegexOptions.Singleline)
                match1 = reg1.Match(Me.mstrMailBody)
                Dim array As ArrayList = New ArrayList
                For i As Integer = 0 To match1.Groups("multipart").Captures.Count - 1
                    If match1.Groups("multipart").Captures(i).Value <> "" Then
                        Dim mailMultipart As clsMailMultipart = New clsMailMultipart(match1.Groups("multipart").Captures(i).Value)
                        array.Add(mailMultipart)
                    End If
                Next
                Me.mclsMailMultiparts = CType(array.ToArray(GetType(clsMailMultipart)), clsMailMultipart())

            Else
                ' multipart
                Dim strBoundary As String = match.Groups("boundary").Value
                reg = New Regex("^.*?--" & strBoundary & "\r\n(?:(?<multipart>.*?)" & "--" & strBoundary & "-*\r\n)+.*?$", RegexOptions.Singleline)
                match = reg.Match(Me.mstrMailBody)
                Dim array As ArrayList = New ArrayList
                For i As Integer = 0 To match.Groups("multipart").Captures.Count - 1
                    If match.Groups("multipart").Captures(i).Value <> "" Then
                        Dim mailMultipart As clsMailMultipart = New clsMailMultipart(match.Groups("multipart").Captures(i).Value)
                        array.Add(mailMultipart)
                    End If
                Next
                Me.mclsMailMultiparts = CType(array.ToArray(GetType(clsMailMultipart)), clsMailMultipart())
            End If

        Else
            reg = New Regex("Content-Type:\s+multipart/alternative;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase)
            match = reg.Match(strMailHeader)

            Dim strResult As String
            strResult = match.Groups("boundary").Value
            If match.Groups("boundary").Value <> "" Then
                ' multipart
                Dim strBoundary As String = match.Groups("boundary").Value
                reg = New Regex("^.*?--" & strBoundary & "\r\n(?:(?<multipart>.*?)" & "--" & strBoundary & "-*\r\n)+.*?$", RegexOptions.Singleline)
                match = reg.Match(Me.mstrMailBody)
                Dim array As ArrayList = New ArrayList
                For i As Integer = 0 To match.Groups("multipart").Captures.Count - 1
                    If match.Groups("multipart").Captures(i).Value <> "" Then
                        Dim clsMailMulti As clsMailMultipart = New clsMailMultipart(match.Groups("multipart").Captures(i).Value)
                        array.Add(clsMailMulti)
                    End If
                Next
                Me.mclsMailMultiparts = CType(array.ToArray(GetType(clsMailMultipart)), clsMailMultipart())
            Else
                Me.mclsMailMultiparts = Array.CreateInstance(GetType(clsMailMultipart), 0)
            End If

        End If

    End Sub

    Public ReadOnly Property Text() As String       'ボディ部全体を返します。
        Get
            Return Me.mstrMailBody
        End Get
    End Property

    Public ReadOnly Property Multiparts() As clsMailMultipart()     'マルチパート部のコレクションを返します。
        Get
            Return Me.mclsMailMultiparts
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

    Dim mstrMail As String       'メール本体

    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです
    '　　　VALUE      : None.
    '      PARAMS     : (strMail String, メール本体)
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal strMail As String)
        Me.mstrMail = strMail
    End Sub

    Public ReadOnly Property Header() As clsMailHeader      'ヘッダ部を取得します。
        Get
            Return New clsMailHeader(Me.mstrMail)
        End Get
    End Property

    Public ReadOnly Property Body() As clsMailBody      'ボディ部を取得します。
        Get
            Return New clsMailBody(Me.mstrMail)
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

    Dim mstrMail As String       'メール本体


    '   ******************************************************************
    '　　　FUNCTION   : コンストラクタです
    '　　　VALUE      : None.
    '      PARAMS     : (strMail String, メール本体)
    '      MEMO       : None.
    '      CREATE     : 2020/02/20　AKB　Cuong
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New(ByVal strMail As String)
        ' 行頭のピリオド2つをピリオド1つに変換
        Me.mstrMail = Regex.Replace(strMail, "\r\n\.\.", vbCrLf & ".")
    End Sub

    Public ReadOnly Property Header() As clsMailHeader      'ヘッダ部を取得します。
        Get
            Return New clsMailHeader(Me.mstrMail)
        End Get
    End Property

    Public ReadOnly Property Body() As clsMailBody      ' ボディ部を取得します。
        Get
            Return New clsMailBody(Me.mstrMail)
        End Get
    End Property

End Class
