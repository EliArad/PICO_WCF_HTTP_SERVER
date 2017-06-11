using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PicoUSB_TC_08Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PicoConnector
{
  
 
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
     [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
     ConcurrencyMode = ConcurrencyMode.Multiple,
     Name = "PicoTS08", Namespace = "PicoConnector"
     )]
    public class PicoTS08 : IPicoTS08
    {

        USBTC08 m_tc = new USBTC08();
 
        public PicoTS08()
        {
            try
            {
                m_tc.Open();
                WriteLog("Pico opened ok");
                m_tc.SetChannels();
                WriteLog("Pico Set channles ok");
            }
            catch (Exception err)
            {
                WriteLog(err.Message);
            }             
        }
        static Object m_lock = new Object();

        void WriteLog(string msg)
        {
            using (System.IO.StreamWriter sw = System.IO.File.AppendText("c:\\picolog.txt"))
            {
                sw.WriteLine("{0},{1}" ,DateTime.Now,msg);
            }
        }
           
        Stream PrepareResponse(JObject jsonObject)
        {
            var s = JsonSerializer.Create();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                s.Serialize(sw, jsonObject);
            }
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        Stream PrepareResponseOk()
        {
            dynamic jsonObject = new JObject();
            jsonObject.Result = "ok";

            var s = JsonSerializer.Create();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                s.Serialize(sw, jsonObject);
            }
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
            return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        Stream PrepareResponseMsg(string msg, bool ok = false)
        {
            dynamic jsonObject = new JObject();
            jsonObject.Result = msg;

            var s = JsonSerializer.Create();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                s.Serialize(sw, jsonObject);
            }

            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            if (ok == true)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        public Stream ReadChannels()
        {
            lock (m_lock)
            {
                bool ok;
                float[]  x = m_tc.TC08GetSingle2(out ok);
                var result = string.Join(",", x);
                return PrepareResponseMsg(result, true);
            }
        }

        public Stream Close()
        {
            lock (m_lock)
            {
                 
                m_tc.Dispose();
                return PrepareResponseOk();
            }
        }          
    }
}
