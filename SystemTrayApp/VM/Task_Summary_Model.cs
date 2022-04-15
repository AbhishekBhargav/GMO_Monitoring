using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTrayApp.Model;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Configuration;


namespace SystemTrayApp.View_Model
{
    public class Task_Summary_Model
    {
        public List<Task_Summary> task;
        public Task_Summary_Model()
        {
            string path = ConfigurationManager.AppSettings.Get("Summary Logs") + "\\Summary_" + DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".json";
            using (FileStream f = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using(StreamReader s = new StreamReader(f))
            {
                var text = s.ReadToEnd();
                task = JsonConvert.DeserializeObject<List<Task_Summary>>(text);
            }
           
        }

    }
}
