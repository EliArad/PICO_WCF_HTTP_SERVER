using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PicoConnector
{
    [ServiceContract]
    public interface IPicoTS08
    {
        
        [OperationContract]
        [WebGet(), CorsEnabled]
        Stream ReadChannels();

        [OperationContract]
        [WebGet(), CorsEnabled]
        Stream Close();


        [OperationContract]
        [WebGet(), CorsEnabled]
        Stream Open();
    }      
}
