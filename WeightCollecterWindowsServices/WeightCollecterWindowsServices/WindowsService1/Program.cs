using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var type = System.Configuration.ConfigurationSettings.AppSettings["Type"].ToString();
            ServiceBase[] ServicesToRun;
            if (type == "TCP")
            {
                ServicesToRun = new ServiceBase[]
                {
                    new WeightCollectService_TCP()
                };
            }
            else 
            {
                ServicesToRun = new ServiceBase[]
                {
                    new WeughtCollectService()
                };               
            }
            ServiceBase.Run(ServicesToRun);
        }
    }
}
