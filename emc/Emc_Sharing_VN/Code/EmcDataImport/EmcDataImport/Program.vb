Imports System.ServiceProcess

Module Program

    Public Sub main()

        'Dim servicesToRun As ServiceBase() = New ServiceBase() {New EmcDataImport()}
        'ServiceBase.Run(servicesToRun)

        AppManager.Start()
    End Sub
End Module
