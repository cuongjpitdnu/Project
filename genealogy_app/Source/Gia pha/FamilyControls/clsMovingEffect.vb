'   ******************************************************************
'      TITLE      : DRAW MOVING EFFECT
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2012/01/11　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On

'   ******************************************************************
'　　　FUNCTION   : clsMovingEffect, draw moving effet
'      MEMO       : 
'      CREATE     : 2012/01/11　AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class clsMovingEffect

    Private Const mcstrClsName As String = "clsMovingEffect"                                        'class name

    Private mobjTimer As Timer                              'timer 
    Private mobjDrawCard As clsDrawCard                     'draw card instance
    Private mpnDraw As Panel                                'panel to draw
    Private mintID As Integer                               'member id

    Private mptSource As Point                              'from point
    Private mptDestination As Point                         'to point

    Private mintX_offset As Integer                         '
    Private mintY_offset As Integer                         '
    Private mintW_increase As Integer                       '
    Private mintH_increase As Integer                       '
    Private mintCount As Integer                            '
    Private mintSW As Integer                               '
    Private mintSH As Integer                               '
    Private mintAnimateCount As Integer                     '

    Private mintDW As Integer                               '
    Private mintDH As Integer                               '

    Private mblnBegin As Boolean = True                     'start flag

    Public Event evnRefreshID(ByVal intID As Integer)


    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2012/01/11　AKB Quyet
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New(ByVal objDrawCard As clsDrawCard, ByVal pnDraw As Panel, ByVal intTimerInterval As Integer, ByVal intCounter As Integer)

        Me.mobjDrawCard = objDrawCard
        Me.mpnDraw = pnDraw
        Me.mintAnimateCount = intCounter

        'create object
        Me.mobjTimer = New Timer()
        Me.mobjTimer.Interval = intTimerInterval

        'add handler
        AddHandler mobjTimer.Tick, AddressOf xTimerTick

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : fncStartEffect, begin effect
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : ptStart Point,  start point
    '      PARAMS     : ptEnd Point,    end point
    '      PARAMS     : intID Integer,  member id  
    '      MEMO       : 
    '      CREATE     : 2012/01/11　AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncStartEffect(ByVal ptStart As Point, ByVal ptEnd As Point, ByVal intID As Integer) As Boolean

        fncStartEffect = False

        Try
            Me.mptSource = ptStart
            Me.mptDestination = ptEnd
            Me.mintID = intID

            'reset values
            mblnBegin = True
            mintCount = 0

            xCalculate()

            mobjTimer.Start()

            Return True

        Catch ex As Exception

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : xCalculate, calculate distance to move 
    '      MEMO       :  
    '      CREATE     : 2012/01/11　AKB Quyet
    '      UPDATE     :  
    '   ******************************************************************
    Private Sub xCalculate()

        Try
            mintSW = clsDefine.CARD_SMAL_W
            mintSH = clsDefine.CARD_SMAL_H

            mintDW = clsDefine.CARD_LARG_W
            mintDH = clsDefine.CARD_LARG_H

            mintX_offset = Math.Abs((mptSource.X - mptDestination.X) \ mintAnimateCount)
            mintY_offset = Math.Abs((mptSource.Y - mptDestination.Y) \ mintAnimateCount)

            mintW_increase = Math.Abs((mintSW - mintDW) \ mintAnimateCount)
            mintH_increase = Math.Abs((mintSH - mintDH) \ mintAnimateCount)

            'revert sign
            If mptSource.X > mptDestination.X Then mintX_offset *= -1
            If mptSource.Y > mptDestination.Y Then mintY_offset *= -1

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCalculate", ex)
        End Try

    End Sub


    '   ****************************************************************** 
    '      FUNCTION   : xTimerTick, start effect 
    '      MEMO       :  
    '      CREATE     : 2012/01/11　AKB Quyet
    '      UPDATE     :  
    '   ******************************************************************
    Private Sub xTimerTick(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
            xDrawBaseEffect()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xTimerTick", ex)
        End Try

    End Sub


    '   ****************************************************************** 
    '      FUNCTION   : xDrawBaseEffect, drawing 
    '      MEMO       :  
    '      CREATE     : 2012/01/11　AKB Quyet
    '      UPDATE     :  
    '   ******************************************************************
    Private Sub xDrawBaseEffect()

        Dim p As Pen = Nothing
        Dim g As Graphics = Nothing
        Dim desktopDC As IntPtr = Nothing

        Try
            'end timer and set active member
            If mintCount >= mintAnimateCount Then

                mblnBegin = False
                mobjTimer.Stop()

                'if there is an member - set active member and reload all
                mobjDrawCard.ActiveMemberID = mintID

                'raise event to change mintId in main form
                RaiseEvent evnRefreshID(mintID)

                Exit Sub

            End If

            'create object
            g = mpnDraw.CreateGraphics()
            p = New Pen(Color.Black, 1)

            'clear panel before drawing
            g.Clear(Color.White)

            mptSource.X += mintX_offset
            mptSource.Y += mintY_offset
            mintSW += mintW_increase
            mintSH += mintH_increase

            'use this when counter reaches to the end
            If mintCount = mintAnimateCount - 1 Then
                mptSource = mptDestination
            End If

            If mintCount = mintAnimateCount - 1 Then
                mintSW = mintDW
                mintSH = mintDH
            End If

            'draw rectangle
            g.DrawRectangle(p, mptSource.X, mptSource.Y, mintSW, mintSH)

            'increase counter
            mintCount += 1

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawBaseEffect", ex)
        Finally
            If p IsNot Nothing Then p.Dispose()
            If g IsNot Nothing Then g.Dispose()
        End Try

    End Sub

End Class
