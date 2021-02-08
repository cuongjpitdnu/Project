Imports System.ServiceProcess
Imports EmcDataImport.PcMoniter

Module Program

    Public Sub main()
        Dim servicesToRun As ServiceBase() = New ServiceBase() {New PcMoniterService()}
        ServiceBase.Run(servicesToRun)
    End Sub
End Module

