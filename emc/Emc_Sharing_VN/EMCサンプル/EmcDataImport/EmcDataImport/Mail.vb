Imports System
Imports System.Collections
Imports System.Text
Imports System.Text.RegularExpressions

''' <summary>
''' ���[���w�b�_�����擾���邽�߂̃N���X�ł��B
''' </summary>
Public Class MailHeader

    ''' <summary>���[���w�b�_��</summary>
    Dim mailheader As String

    ''' <summary>
    ''' �R���X�g���N�^�ł��B
    ''' </summary>
    ''' <param name="mail">���[���{�́B</param>
    Public Sub New(ByVal mail As String)
        ' ���[���̃w�b�_���ƃ{�f�B���� 1�ȏ�̋�s�ł킯���Ă��܂��B
        ' ���K�\�����g���ăw�b�_���݂̂����o���܂��B
        Dim reg As Regex = New Regex("^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline)
        Dim m As Match = reg.Match(mail)
        Me.mailheader = m.Groups("header").Value
    End Sub

    ''' <summary>
    ''' �w�b�_���S�̂�Ԃ��܂��B
    ''' </summary>
    Public ReadOnly Property Text() As String
        Get
            Return Me.mailheader
        End Get
    End Property

    ''' <summary>
    ''' �w�b�_�̊e�s��Ԃ��܂��B
    ''' name �� null�A�������́A�󕶎����n�����ꍇ�͂��ׂẴw�b�_��Ԃ��܂��B
    ''' </summary>
    Default Public ReadOnly Property Item(ByVal name As String) As String()
        Get
            ' Subject: line1
            '          line2
            ' �̂悤�ɕ����s�ɕ�����Ă���w�b�_��
            ' Subject: line1 line2
            ' �ƂȂ�悤�� 1�s�ɂ܂Ƃ߂܂��B
            Dim header As String = Regex.Replace(Me.mailheader, "\r\n\s+", " ")

            If Not name Is Nothing AndAlso name <> "" Then
                If Not name.EndsWith(":") Then
                    name += ":"
                End If
                name = name.ToLower()
            Else
                name = Nothing
            End If

            ' name �Ɉ�v����w�b�_�݂̂𒊏o
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
    ''' �f�R�[�h���܂��B
    ''' </summary>
    ''' <param name="encodedtext">�f�R�[�h���镶����B</param>
    ''' <returns>�f�R�[�h�������ʁB</returns>
    Public Shared Function Decode(ByVal encodedtext As String) As String
        Dim decodedtext As String = ""
        Do While encodedtext <> ""
            Dim r As Regex = New Regex("^(?<preascii>.*?)(?:=\?(?<charset>.+?)\?(?<encoding>.+?)\?(?<encodedtext>.+?)\?=)+(?<postascii>.*?)$")
            Dim m As Match = r.Match(encodedtext)
            If m.Groups("charset").Value = "" OrElse m.Groups("encoding").Value = "" OrElse m.Groups("encodedtext").Value = "" Then
                ' �G���R�[�h���ꂽ������͂Ȃ�
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
                    ' ���T�|�[�g
                    decodedtext += "=?" & m.Groups("charset").Value & "?" & m.Groups("encoding").Value & "?" & m.Groups("encodedtext").Value & "?="
                End If
                encodedtext = m.Groups("postascii").Value
            End If
        Loop
        Return decodedtext
    End Function

End Class

''' <summary>
''' ���[���{�f�B�����擾���邽�߂̃N���X�ł��B
''' </summary>
Public Class MailBody

    ''' <summary>���[���{�f�B��</summary>
    Dim mailbody As String

    ''' <summary>�e�}���`�p�[�g���̃R���N�V����</summary>
    Private _multiparts As MailMultipart()

    ''' <summary>
    ''' �R���X�g���N�^�ł��B
    ''' </summary>
    ''' <param name="mail">���[���{�́B</param>
    Public Sub New(ByVal mail As String)
        ' ���[���̃w�b�_���ƃ{�f�B���� 1�ȏ�̋�s�ł킯���Ă��܂��B
        ' ���K�\�����g���ăw�b�_���A�{�f�B�������o���܂��B
        Dim reg As Regex = New Regex("^(?<header>.*?)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline)
        Dim m As Match = reg.Match(mail)
        Dim mailheader As String = m.Groups("header").Value
        Me.mailbody = m.Groups("body").Value

        reg = New Regex("Content-Type:\s+multipart/mixed;\s+boundary=""(?<boundary>.*?)""", RegexOptions.IgnoreCase)
        m = reg.Match(mailheader)
        If m.Groups("boundary").Value <> "" Then
            ' multipart
            Dim boundary As String = m.Groups("boundary").Value
            reg = New Regex("^.*?--" & boundary & "\r\n(?:(?<multipart>.*?)" & "--" & boundary & "-*\r\n)+.*?$", RegexOptions.Singleline)
            m = reg.Match(Me.mailbody)
            Dim ary As ArrayList = New ArrayList
            For i As Integer = 0 To m.Groups("multipart").Captures.Count - 1
                If m.Groups("multipart").Captures(i).Value <> "" Then
                    Dim b As MailMultipart = New MailMultipart(m.Groups("multipart").Captures(i).Value)
                    ary.Add(b)
                End If
            Next
            Me._multiparts = CType(ary.ToArray(GetType(MailMultipart)), MailMultipart())
        Else
            ' single
            Me._multiparts = Array.CreateInstance(GetType(MailMultipart), 0)
        End If
    End Sub

    ''' <summary>
    ''' �{�f�B���S�̂�Ԃ��܂��B
    ''' </summary>
    Public ReadOnly Property Text() As String
        Get
            Return Me.mailbody
        End Get
    End Property

    ''' <summary>
    ''' �}���`�p�[�g���̃R���N�V������Ԃ��܂��B
    ''' </summary>
    Public ReadOnly Property Multiparts() As MailMultipart()
        Get
            Return Me._multiparts
        End Get
    End Property

End Class

''' <summary>
''' �ЂƂ̃}���`�p�[�g��������킷�N���X�ł��B
''' </summary>
Public Class MailMultipart

    ''' <summary>���[���{��</summary>
    Dim mail As String

    ''' <summary>
    ''' �R���X�g���N�^�ł��B
    ''' </summary>
    ''' <param name="mail">���[���{�́B</param>
    Public Sub New(ByVal mail As String)
        Me.mail = mail
    End Sub

    ''' <summary>
    ''' �w�b�_�����擾���܂��B
    ''' </summary>
    Public ReadOnly Property Header() As MailHeader
        Get
            Return New MailHeader(Me.mail)
        End Get
    End Property

    ''' <summary>
    ''' �{�f�B�����擾���܂��B
    ''' </summary>
    Public ReadOnly Property Body() As MailBody
        Get
            Return New MailBody(Me.mail)
        End Get
    End Property

end class

''' <summary>
''' ���[����\���N���X�ł��B
''' </summary>
Public Class Mail

    ''' <summary>���[���{��</summary>
    Dim mail As String

    ''' <summary>
    ''' �R���X�g���N�^�ł��B
    ''' </summary>
    ''' <param name="mail">���[���{�́B</param>
    Public Sub New(ByVal mail As String)
        ' �s���̃s���I�h2���s���I�h1�ɕϊ�
        Me.mail = Regex.Replace(mail, "\r\n\.\.", vbcrlf & ".")
    End Sub

    ''' <summary>
    ''' �w�b�_�����擾���܂��B
    ''' </summary>
    Public ReadOnly Property Header() As MailHeader
        Get
            Return New MailHeader(Me.mail)
        End Get
    End Property

    ''' <summary>
    ''' �{�f�B�����擾���܂��B
    ''' </summary>
    Public ReadOnly Property Body() As MailBody
        Get
            Return New MailBody(Me.mail)
        End Get
    End Property

End Class
