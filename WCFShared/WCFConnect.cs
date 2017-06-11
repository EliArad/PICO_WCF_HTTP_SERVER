﻿using PicoConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
 

namespace WCFShared
{
    public class WCFConnect
    {
        static ServiceHost host = null;
        public static void Close()
        {
           
            if (host != null)
                host.Close();
        }   
        public static void OpenService()
        {

            try
            {

                WebServiceHost host = new WebServiceHost(typeof(PicoTS08), new Uri("http://localhost:8022/"));

                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IPicoTS08), new WebHttpBinding(), "");

                ep.Behaviors.Add(new EnableCorsEndpointBehavior());

                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                host.Open();
            }
            catch (Exception err)
            {
                throw (new SystemException(err.Message));
            }
        }
    }
}
