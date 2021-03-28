using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ConfigurationService
{
    public class FlightsManagmentSystemConfig
    {
        private static readonly log4net.ILog my_logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string m_file_name;
        private bool m_init = false;
        private JObject m_configRoot;

        public static readonly FlightsManagmentSystemConfig Instance = new FlightsManagmentSystemConfig();

        // member according to json file
        public string ConnectionString { get; set; }
        public int MaxConnections { get; set; }
        public string WorkTime { get; set; }

        private FlightsManagmentSystemConfig()
        {

        }

        public void Init(string file_name = null)
        {
            if (m_init)
                return;

            m_file_name = file_name != null ? file_name : "FlightsManagmentSystem.Config.json";
            m_init = true;

            if (!File.Exists(m_file_name))
            {
                my_logger.Fatal($"File {m_file_name} does not exist!");
                Environment.Exit(-1);
            }

            var reader = File.OpenText(m_file_name);
            string json_string = reader.ReadToEnd();

            JObject all = (JObject)JsonConvert.DeserializeObject(json_string);
            m_configRoot = (JObject)all["FlightsManagmentSystem"];
            ConnectionString = m_configRoot["ConnectionString"].Value<string>();
            MaxConnections = m_configRoot["MaxConnections"].Value<int>();
            WorkTime = m_configRoot["WorkTime"].Value<string>();
        }
    }
}
