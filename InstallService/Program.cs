using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatsonFieldInstaller.ServiceTools;

namespace InstallService
{
    class Program
    {
        static string m_HttpServiceName = "pico TC08 Http server";
        static void Main(string[] args)
        {

            StopServiceAndUnInstall(m_HttpServiceName);

            InstallServiceAndStart(m_HttpServiceName,
                                   "Pico TC 08 Http server",
                                          @"C:\\PicoWCFHttpServer\WCFHttpServer\WCFHttpWinService\bin\Release\PicoTC08WCFHttpWinService.exe");
        }


        static string InstallServiceAndStart(string serviceName, string nameInServices, string serviceFullPath)
        {
            if (ServiceInstaller.ServiceIsInstalled(serviceName) == false)
            {
                ServiceInstaller.InstallAndStart(serviceName, nameInServices, serviceFullPath, true);
                for (int i = 0; i < 5; i++)
                {
                    if (ServiceInstaller.getStatus(serviceName) != "Running")
                    {
                        Thread.Sleep(200);
                    }
                    else
                    {
                        return "ok";
                    }
                }
            }
            else
            {
                ServiceInstaller.StartService(serviceName);
                for (int i = 0; i < 50; i++)
                {
                    if (ServiceInstaller.getStatus(serviceName) != "Running")
                    {
                        Thread.Sleep(700);
                    }
                    else
                    {
                        return "ok";
                    }
                }
            }
            return "failed";
        }
        static string StopServiceAndUnInstall(string serviceName)
        {

            if (ServiceInstaller.ServiceIsInstalled(serviceName) == true)
            {
                try
                {
                    ServiceInstaller.StopService(serviceName);
                    int count = 15;
                    while (ServiceInstaller.getStatus(serviceName) != "Stopped")
                    {
                        Thread.Sleep(700);
                        count--;
                        if (count == 0)
                        {
                            return "Failed to stop service";
                        }
                    }
                }
                catch (Exception err)
                {
                    return err.Message;
                }
                try
                {
                    ServiceInstaller.Uninstall(serviceName);
                    return "ok";
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }
            else
            {
                return "ok";
            }
        }
        static string InstallServiceDontStart(string serviceName, string nameInServices, string serviceFullPath)
        {
            if (ServiceInstaller.ServiceIsInstalled(serviceName) == false)
            {
                ServiceInstaller.InstallAndStart(serviceName, nameInServices, serviceFullPath, false);
                for (int i = 0; i < 50; i++)
                {
                    if (ServiceInstaller.getStatus(serviceName) != "Running")
                    {
                        Thread.Sleep(200);
                    }
                    else
                    {
                        return "ok";
                    }
                }
            }
            return "failed";
        }
    }
}
