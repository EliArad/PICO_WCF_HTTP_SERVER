using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectonRestApi;
using VectonRestApi.HttpUtils;

namespace PicoHttpClient
{
    public class PicoClientApi
    {
        object m_lock = new object();
        
        RestClient m_restClient = new RestClient(); 
        string m_url;
        float[] m_channles = new float[8]; 

        public PicoClientApi(string ip, int port = 8022)
        {
            m_url = "http://" + ip + ":" + port;
        }

      
        public string ReadChannels(out float [] result)
        {

            lock (m_lock)
            {
                result = null;
                try
                {
                    m_restClient.EndPoint = m_url + "/ReadChannels";
                    m_restClient.Method = HttpVerb.GET;
                    m_restClient.PostData = null;
                    var json = m_restClient.MakeRequest();
                    dynamic d = JObject.Parse(json);
                    string r = d.Result;
                    m_channles = Array.ConvertAll(r.Split(','), float.Parse);
                    result = m_channles;
                    return "ok";
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }
        }
  

        public string Close()
        {

            lock (m_lock)
            {
                try
                {
                    m_restClient.EndPoint = m_url + "/Close";
                    m_restClient.Method = HttpVerb.GET;
                    m_restClient.PostData = null;
                    var json = m_restClient.MakeRequest();
                    dynamic d = JObject.Parse(json);
                    string r = d.Result;
                    return r;
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }
        }

        public string Open()
        {

            lock (m_lock)
            {
                try
                {
                    m_restClient.EndPoint = m_url + "/Open";
                    m_restClient.Method = HttpVerb.GET;
                    m_restClient.PostData = null;
                    var json = m_restClient.MakeRequest();
                    dynamic d = JObject.Parse(json);
                    string r = d.Result;
                    return r;
                }
                catch (Exception err)
                {
                    return err.Message;
                }
            }
        }


    }
}
