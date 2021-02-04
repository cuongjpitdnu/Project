Option Explicit On

Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Module FamilyDraw

#Region "Draw Base Class"

    Public Class clsDraw

#Region "Properties"

        Dim mobjGraphic As Graphics
        Dim mPen As Pen
        Dim miPenSize As Integer
        Dim mColor As Color
        Dim mPenStyle As DashStyle

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PenSize() As Integer

            Get

                Return Me.miPenSize

            End Get

            Set(ByVal value As Integer)

                Me.miPenSize = value

            End Set

        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PenColor() As Color

            Get

                Return Me.mColor

            End Get

            Set(ByVal value As Color)

                Me.mColor = value

            End Set

        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PenStyle() As DashStyle

            Get

                Return Me.mPenStyle

            End Get

            Set(ByVal value As DashStyle)

                Me.mPenStyle = value

            End Set

        End Property

#End Region

#Region "Draw Line"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="vpSP"></param>
        ''' <param name="vpEP"></param>
        ''' <param name="vintPenSize"></param>
        ''' <param name="vobjPenStyle"></param>
        ''' <remarks></remarks>
        Public Sub DrawLine(ByVal vpSP As Point, _
                            ByVal vpEP As Point, _
                            Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                            Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Dim objPen As Pen

            Try

                Me.PenSize = vintPenSize
                Me.PenStyle = vobjPenStyle

                objPen = New Pen(Me.PenColor)
                objPen.DashStyle = Me.PenStyle
                objPen.Width = Me.PenSize

                Me.mobjGraphic.DrawLine(objPen, vpSP, vpEP)

            Catch ex As Exception

                Throw ex

            End Try

            If Not objPen Is Nothing Then objPen.Dispose()

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="vpSP"></param>
        ''' <param name="vpEP"></param>
        ''' <param name="vcPenColor"></param>
        ''' <param name="vintPenSize"></param>
        ''' <param name="vobjPenStyle"></param>
        ''' <remarks></remarks>
        Public Sub DrawLine(ByVal vpSP As Point, _
                            ByVal vpEP As Point, _
                            ByVal vcPenColor As Color, _
                            Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                            Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Try

                Me.PenColor = vcPenColor

                Me.DrawLine(vpSP, vpEP, vintPenSize, vobjPenStyle)

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="vintX1"></param>
        ''' <param name="vintY1"></param>
        ''' <param name="vintX2"></param>
        ''' <param name="vintY2"></param>
        ''' <param name="vintPenSize"></param>
        ''' <param name="vobjPenStyle"></param>
        ''' <remarks></remarks>
        Public Sub DrawLine(ByVal vintX1 As Integer, _
                            ByVal vintY1 As Integer, _
                            ByVal vintX2 As Integer, _
                            ByVal vintY2 As Integer, _
                            Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                            Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Try

                Dim pStartPoint As New Point(vintX1, vintY1)
                Dim pEndPoint As New Point(vintX2, vintY2)

                Me.DrawLine(pStartPoint, pEndPoint, vintPenSize, vobjPenStyle)

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="vintX1"></param>
        ''' <param name="vintY1"></param>
        ''' <param name="vintX2"></param>
        ''' <param name="vintY2"></param>
        ''' <param name="vcColor"></param>
        ''' <param name="vintPenSize"></param>
        ''' <param name="vobjPenStyle"></param>
        ''' <remarks></remarks>
        Public Sub DrawLine(ByVal vintX1 As Integer, _
                            ByVal vintY1 As Integer, _
                            ByVal vintX2 As Integer, _
                            ByVal vintY2 As Integer, _
                            ByVal vcColor As Color, _
                            Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                            Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Try

                Me.PenColor = vcColor

                Me.DrawLine(vintX1, vintY1, vintX2, vintY2, vintPenSize, vobjPenStyle)

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

#End Region

#Region "Draw Line By Multiple Points"

        Public Sub DrawLineMulPos(ByVal vpSP() As Point, _
                                  Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                                  Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Dim objPen As Pen

            Try

                Me.PenSize = vintPenSize
                Me.PenStyle = vobjPenStyle

                objPen = New Pen(Me.PenColor)
                objPen.DashStyle = Me.PenStyle
                objPen.Width = Me.PenSize

                'Me.mobjGraphic.DrawLine(objPen, vpSP, vpEP)
                
                Me.mobjGraphic.DrawLines(objPen, vpSP)

            Catch ex As Exception

                Throw ex

            End Try

            If Not objPen Is Nothing Then objPen.Dispose()

        End Sub

        Public Sub DrawLineMulPos(ByVal vparrSP() As Point, _
                                  ByVal vcPenColor As Color, _
                                  Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                                  Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Try

                Me.PenColor = vcPenColor

                Me.DrawLineMulPos(vparrSP, vintPenSize, vobjPenStyle)

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

#End Region

#Region "Draw Rectangle"

        Public Sub DrawRectangle(ByVal vrecValue As Rectangle, _
                                 Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                                 Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Dim objPen As Pen

            Try

                Me.PenSize = vintPenSize
                Me.PenStyle = vobjPenStyle

                objPen = New Pen(Me.PenColor)
                objPen.DashStyle = Me.PenStyle
                objPen.Width = Me.PenSize

                Me.mobjGraphic.DrawRectangle(objPen, vrecValue)

            Catch ex As Exception

                Throw ex

            End Try

            If Not objPen Is Nothing Then objPen.Dispose()

        End Sub

        Public Sub DrawRectangle(ByVal vpTopLeft As Point, _
                                 ByVal vszRecSize As Size, _
                                 Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                                 Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Try

                Dim recValue As Rectangle = New Rectangle(vpTopLeft, vszRecSize)

                Me.DrawRectangle(recValue, vintPenSize, vobjPenStyle)

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

        Public Sub DrawRectangle(ByVal vpTopLeft As Point, _
                                 ByVal viRecWidth As Integer, _
                                 ByVal viRecHeight As Integer, _
                                 Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                                 Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Try

                Dim recSize As Size = New Size(viRecWidth, viRecHeight)

                Me.DrawRectangle(vpTopLeft, recSize, vintPenSize, vobjPenStyle)

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

        Public Sub DrawRectangle(ByVal viTop As Integer, _
                                 ByVal viLeft As Integer, _
                                 ByVal vszRecSize As Size, _
                                 Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                                 Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Try

                Dim pPoint As Point = New Point(viTop, viLeft)

                Me.DrawRectangle(pPoint, vszRecSize, vintPenSize, vobjPenStyle)

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

        Public Sub DrawRectangle(ByVal viTop As Integer, _
                                 ByVal viLeft As Integer, _
                                 ByVal viRecWidth As Integer, _
                                 ByVal viRecHeight As Integer, _
                                 Optional ByVal vintPenSize As Integer = clsDefine.PENSIZE, _
                                 Optional ByVal vobjPenStyle As DashStyle = clsDefine.PENSTYLE)

            Try

                Dim pPoint As Point = New Point(viTop, viLeft)
                Dim recSize As Size = New Size(viRecWidth, viRecHeight)

                Me.DrawRectangle(pPoint, recSize, vintPenSize, vobjPenStyle)

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

#End Region

#Region "Initial"

        Private Sub New()
            Try

                'Me.miPenSize = 1
                'Me.mPen = New Pen(Color.Black, Me.miPenSize)
                'Debug.Assert(True, "Create by Private")
                'MsgBox("New Graphic")

            Catch ex As Exception

                Throw ex

            End Try
        End Sub

        Public Sub New(ByVal vgToDraw As Graphics)
            Try

                Me.mobjGraphic = vgToDraw
                Me.miPenSize = clsDefine.PENSIZE
                Me.PenColor = Color.Black
                Me.mPen = New Pen(Color.Black, Me.miPenSize)
                'MsgBox("New Graphic")

            Catch ex As Exception

                Throw ex

            End Try

        End Sub

#End Region

#Region "Destroy"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub Finalize()

            Try

                Me.mPen.Dispose()
                Me.mobjGraphic.Dispose()

            Catch ex As Exception

                Throw ex

            End Try

            MyBase.Finalize()

        End Sub

#End Region

    End Class

#End Region

End Module