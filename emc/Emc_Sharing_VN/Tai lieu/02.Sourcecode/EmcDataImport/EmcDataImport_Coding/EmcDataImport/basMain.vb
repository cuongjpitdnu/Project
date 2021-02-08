'   ******************************************************************
'      TITLE      : AKBカスタムコントロール
'      FUNCTION   : Call to run the service.
'      MEMO       : None.
'      CREATE     : 2020/02/18　AKB　Cuong.
'      UPDATE     : .
'
'           2020 AKBSOFTWARE CORPORATION
'   ******************************************************************

Imports System.ServiceProcess

'   ******************************************************************
'      FUNCTION   : Call to the service.
'      MEMO       : None.
'      CREATE     : 2020/02/18　AKB　Cuong.
'      UPDATE     : 
'   ******************************************************************
Module basMain

    Public Sub main()

        Dim servicesToRun As ServiceBase() = New ServiceBase() {New clsEmcDataImport()}
        ServiceBase.Run(servicesToRun)

    End Sub
End Module
