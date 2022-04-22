using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Threading;
using BarChart;
using CredentialManagement;


namespace SystemTrayApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        public TaskbarIcon notifyIcon;
        public SolidColorBrush basecolour { get; set; }

        DateTime lastwrite = DateTime.MinValue;

        public FileSystemWatcher Watcher;

        public async Task<Dictionary<string,string>> Run_FilewatcherAsync()
        {
            return await Task.Run(() =>
            {
                
                try
                {
                    File_Watcher();                             
                    return new Dictionary<string, string>()
                    {

                        ["Status"] = "true"
                    };
                }
                catch (Exception e)
                {
                    return new Dictionary<string, string>()
                    {

                        ["Status"] = "false",
                        ["Exception"] = e.Message
                    };
                }                
            });
        }

        public void RC()
        {

            string path = ConfigurationManager.AppSettings.Get("Summary_Logs") + "\\Summary_" + DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".json";
            DateTime writetime = File.GetLastWriteTime(path);
            if (writetime.Ticks - lastwrite.Ticks > 10000)
            {
                if (File.Exists(path))
                {
                    string Task;
                    string Task_Status;
                    string Task_Type;
                    string data;
                    string ms;
                    string me;
                    using (FileStream fs = new FileStream(path.ToString(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        data = sr.ReadToEnd();
                    }

                    JObject Root = JsonConvert.DeserializeObject<JObject>(data);
                    try { ms = Root.SelectTokens("$.EventList...[?(@.Start_time <> 'NULL')].Start_time").Max().ToObject<DateTime>().GetDateTimeFormats('o')[0]; } catch (System.NullReferenceException) { ms = DateTime.MinValue.ToString(); }
                    try { me = Root.SelectTokens("$.EventList...[?(@.End_time <> 'NULL')].End_time").Max().ToObject<DateTime>().GetDateTimeFormats('o')[0]; } catch (System.NullReferenceException) { me = DateTime.MinValue.ToString(); }

                    if (DateTime.Parse(me) > DateTime.Parse(ms))
                    {
                        Task = Root.SelectToken("$.EventList...[?(@.End_time == '" + me + "')].TaskID").ToString();
                        Task_Status = Root.SelectToken("$.EventList...[?(@.End_time == '" + me + "')].Status").ToString();
                        Task_Type = ((JProperty)((JToken)((JToken)Root.SelectTokens("$.EventList...[?(@.End_time == '" + me + "')].End_time").Max()).Parent.Parent.Parent.Parent.Parent.Parent)).Name;
                    }
                    else
                    {
                        Task = Root.SelectToken("$.EventList...[?(@.Start_time == '" + ms + "')].TaskID").ToString();
                        Task_Status = Root.SelectToken("$.EventList...[?(@.Start_time == '" + ms + "')].Status").ToString();
                        Task_Type = ((JProperty)((JToken)((JToken)Root.SelectTokens("$.EventList...[?(@.Start_time == '" + ms + "')].Start_time").Max()).Parent.Parent.Parent.Parent.Parent.Parent)).Name;
                    }
                    //string Task = Root.SelectToken("$.EventList...[?(@.Start_time == '" + Root.SelectTokens("$.EventList...Start_time").Max() + "')].TaskID").ToString();
                    //var Task = Root.EventList.ReadingCollection.Task.Take(Root.EventList.ReadingCollection.Task.Count()).Where(x => x.Start_time == Root.EventList.ReadingCollection.Task.Take(Root.EventList.ReadingCollection.Task.Count()).Select(y => y.Start_time).Max()).Select(x => x.TaskID).First();
                    //string Task_Status = Root.SelectToken("$.EventList...[?(@.Start_time == '" + Root.SelectTokens("$.EventList...Start_time").Max() + "')].Status").ToString();

                    if (Task_Type != "PE")
                    {

                        this.Dispatcher.Invoke(() =>
                        {
                            if (Task_Status == "Started" || Task_Status == "Completed")
                            {
                                notifyIcon.ShowBalloonTip(Task_Type + " Notification", Task + " " + Task_Status + " at " + (DateTime.Parse(me) > DateTime.Parse(ms) ? me : ms), BalloonIcon.Info);
                            }
                            else if (Task_Status == "Stop_Fail" || Task_Status == "File_Not_Found")
                            {
                                notifyIcon.ShowBalloonTip(Task_Type + " Notification", Task + " " + Task_Status + " at " + (DateTime.Parse(me) > DateTime.Parse(ms) ? me : ms), BalloonIcon.Warning);
                            }
                            else
                            {
                                notifyIcon.ShowBalloonTip(Task_Type + " Notification", Task + " " + Task_Status + " at " + (DateTime.Parse(me) > DateTime.Parse(ms) ? me : ms), BalloonIcon.Error);
                            }
                        });
                    }
                    else
                    {
                        int value = (int)Root.SelectToken("$.EventList...[?(@.Start_time == '" + ms + "')].Value");
                        if (Task_Status == "Warning")
                        {
                            notifyIcon.ShowBalloonTip(Task_Type + " Notification", "The Number of Bound States for " + Task.Split(':')[1] + " in the server  " + Task.Split(':')[0] + " are " + value.ToString(), BalloonIcon.Warning);
                        }
                        else if (Task_Status == "Error")
                        {
                            notifyIcon.ShowBalloonTip(Task_Type + " Notification", "The Number of Bound States for " + Task.Split(':')[1] + " in the server  " + Task.Split(':')[0] + " are " + value.ToString(), BalloonIcon.Error);
                        }
                    }


                    if (Task_Type == "NFS")
                    {
                        NFS_result(data);
                    }
                    else if (Task_Type == "ReadingCollection")
                    {
                        Reading_result(data);
                    }
                    else { PE_result(data); }
                }
                lastwrite = writetime;
            }
        }

        public void Refresh()
        {
            Reading_result();
            NFS_result();
            PE_result();
        }
        public void Reading_result(string data = null)
        {

            string path = ConfigurationManager.AppSettings.Get("Summary_Logs") + "\\Summary_" + DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".json";
            if (data == null & File.Exists(path))
            {

                using (FileStream fs = new FileStream(path.ToString(),
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {

                        data = sr.ReadToEnd();
                    }
                }

            }
            if (data != null)
            {
                JObject Reading_status = JsonConvert.DeserializeObject<JObject>(data);
                IEnumerable<JToken> Failed = Reading_status.SelectTokens("$.EventList.ReadingCollection..[?(@.Status == 'Failed')].Start_time");
                IEnumerable<JToken> Stop_Fail = Reading_status.SelectTokens("$.EventList.ReadingCollection..[?(@.Status == 'Stop_Fail')].Start_time");
                IEnumerable<JToken> Started = Reading_status.SelectTokens("$.EventList.ReadingCollection..[?(@.Status == 'Started')].Start_time");
                IEnumerable<JToken> Completed = Reading_status.SelectTokens("$.EventList.ReadingCollection..[?(@.Status == 'Completed')].Start_time");
                if (Failed.Any())
                {
                    this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.Red);
                }
                else if (Stop_Fail.Any())
                {
                    this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.Brown);
                }
                else if (Started.Any())
                {
                    this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.DeepSkyBlue);
                }
                else if (Completed.Any())
                {
                    this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.Yellow);
                }
            }
            else
            {
                this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.Plum);
                this.Dispatcher.Invoke(() =>
                {
                    notifyIcon.ToolTipText = $"Path not Found: {path}";

                });

            }
        }

        public void NFS_result(string data = null)
        {

            string path = ConfigurationManager.AppSettings.Get("Summary_Logs") + "\\Summary_" + DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".json";
            if (data == null & File.Exists(path))
            {

                using (FileStream fs = new FileStream(path.ToString(),
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {

                        data = sr.ReadToEnd();
                    }
                }

            }
            if (data != null)
            {
                JObject NFS_status = JsonConvert.DeserializeObject<JObject>(data);
                IEnumerable<JToken> Failed = NFS_status.SelectTokens("$.EventList.NFS..[?(@.Status == 'Failed')].Start_time");
                IEnumerable<JToken> File_Not_Found = NFS_status.SelectTokens("$.EventList.NFS..[?(@.Status == 'File_Not_Found')].Start_time");
                IEnumerable<JToken> Started = NFS_status.SelectTokens("$.EventList.NFS..[?(@.Status == 'Started')].Start_time");
                IEnumerable<JToken> Completed = NFS_status.SelectTokens("$.EventList.NFS..[?(@.Status == 'Completed')].Start_time");
                if (Failed.Any())
                {
                    this.Resources["NFSColour"] = new SolidColorBrush(Colors.Red);
                }
                else if (File_Not_Found.Any())
                {
                    this.Resources["NFSColour"] = new SolidColorBrush(Colors.Brown);
                }
                else if (Started.Any())
                {
                    this.Resources["NFSColour"] = new SolidColorBrush(Colors.DeepSkyBlue);
                }
                else if (Completed.Any())
                {
                    this.Resources["NFSColour"] = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    this.Resources["NFSColour"] = new SolidColorBrush(Colors.Yellow);
                }
            }
            else
            {
                this.Resources["NFSColour"] = new SolidColorBrush(Colors.Plum);
                this.Dispatcher.Invoke(() =>
                {
                    notifyIcon.ToolTipText = $"Path not Found: {path}";

                });

            }
        }

        public void PE_result(string data = null)
        {

            string path = ConfigurationManager.AppSettings.Get("Summary_Logs") + "\\Summary_" + DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".json";
            if (data == null & File.Exists(path))
            {

                using (FileStream fs = new FileStream(path.ToString(),
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {

                        data = sr.ReadToEnd();
                    }
                }

            }
            if (data != null)
            {
                JObject PE_status = JsonConvert.DeserializeObject<JObject>(data);
                string PETask = PE_status.SelectToken("$.EventList.PE.Task").ToString();
                PEDate PEdata = JsonConvert.DeserializeObject<PEDate>(PETask);
                this.Resources["PEBarData"] = PEdata;
                IEnumerable<JToken> Info = PE_status.SelectTokens("$.EventList.PE..[?(@.Status == 'Info')].Start_time");
                IEnumerable<JToken> Warning = PE_status.SelectTokens("$.EventList.PE..[?(@.Status == 'Warning')].Start_time");
                IEnumerable<JToken> Error = PE_status.SelectTokens("$.EventList.PE..[?(@.Status == 'Error')].Start_time");
                if (Error.Any())
                {
                    this.Resources["PEColour"] = new SolidColorBrush(Colors.Red);
                }
                else if (Warning.Any())
                {
                    this.Resources["PEColour"] = new SolidColorBrush(Colors.Brown);
                }
                else if (Info.Any())
                {
                    this.Resources["PEColour"] = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    this.Resources["PEColour"] = new SolidColorBrush(Colors.Yellow);
                }
            }
            else
            {
                this.Resources["PEColour"] = new SolidColorBrush(Colors.Plum);
                this.Dispatcher.Invoke(() =>
                {
                    notifyIcon.ToolTipText = $"Path not Found: {path}";

                });

            }
        }

        public void Onchanged(object sender, FileSystemEventArgs fe)
        {

            try
            {
                if (fe.ChangeType != WatcherChangeTypes.Changed)
                {
                    return;
                }


                this.Dispatcher.Invoke(() =>
                {
                    notifyIcon.ToolTipText = $"Changed: {fe.FullPath} , {fe.ChangeType}";
                });


                RC();
            }
            catch (Exception error)
            {
                this.Dispatcher.Invoke(() =>
                {
                    notifyIcon.ToolTipText = error.ToString();
                });
            }
        }

        public void Onerror(object sender, ErrorEventArgs fe)
        {
            if (fe.GetException().Message == "The specified network name is no longer available")
            {
                Watcher.Dispose();


                this.Dispatcher.Invoke(() =>
                {
                    notifyIcon.ToolTipText = "Error Caught: \n " + fe.GetException().GetType() + "\n" + fe.GetException().ToString() + "\n" + fe.GetException().Message;
                });

                if (CanEnableNotification().Result)
                {
                    File_Watcher();
                    this.Dispatcher.Invoke(() =>
                    {
                        notifyIcon.ToolTipText = "Network Connection is back now. Notifications are Enabled";
                    });
                }
            }

        }

        public void File_Watcher()
        {

            if (Directory.Exists(ConfigurationManager.AppSettings.Get("Summary_Logs")))
            {
                Watcher = new FileSystemWatcher(ConfigurationManager.AppSettings.Get("Summary_Logs"));
                //Watcher.Filter = ("Summary_" + DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".json");
                Watcher.NotifyFilter = NotifyFilters.LastWrite;
                Watcher.Changed += Onchanged;
                Watcher.Error += Onerror;
                Watcher.IncludeSubdirectories = true;
                Watcher.EnableRaisingEvents = true;
                RC();
                Reading_result();
                NFS_result();
                PE_result();


            }
            else
            {
                Watcher = null;
                this.Dispatcher.Invoke(() =>
                {
                    notifyIcon.ToolTipText = $"Path not Found: {ConfigurationManager.AppSettings.Get("Summary_Logs")}";

                });
            }

        }

        public void WhichButon(string btn)
        {
            Uri RP = new Uri("ReadingsMonitoring.xaml", UriKind.Relative);
            Uri NFS = new Uri("NFS_Cleanup.xaml", UriKind.Relative);
            Uri PE = new Uri("PE_Visualization.xaml", UriKind.Relative);

            ///(sender as Path).Fill = shadeColor((sender as Path).Fill as SolidColorBrush, 20);
            if (btn == "RCM") { ((MainWindow)(Application.Current).MainWindow).MainFrame_Source(RP); };
            if (btn == "NFSC") { ((MainWindow)(Application.Current).MainWindow).MainFrame_Source(NFS); };
            if (btn == "PE") { ((MainWindow)(Application.Current).MainWindow).MainFrame_Source(PE); };
        }

        public Boolean LoopCheck()
        {
            Boolean IsLooping = true;

            while (IsLooping)
            {
                IsLooping = !(((App)Application.Current).Watcher == null ? Directory.Exists(ConfigurationManager.AppSettings.Get("Summary_Logs")) : Directory.Exists(ConfigurationManager.AppSettings.Get("Summary_Logs")) & ((App)Application.Current).Watcher.EnableRaisingEvents == false);

                Thread.Sleep(5000);
            }

            return !IsLooping;
        }

        public async Task<Boolean> CanEnableNotification()
        {

            return await Task.Run(() => LoopCheck());

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            };
            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            //File_Watcher();
        }



        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            List<string> Access_Paths = ConfigurationManager.AppSettings.Get("Access_Path").Split(';').ToList<string>();
            foreach (string path in Access_Paths)
            {
                using (var cred = new Credential())
                {
                    cred.Target = path;
                    cred.Type = CredentialType.DomainPassword;
                    if (cred.Exists())
                    {
                        cred.Delete();
                    }
                }
            }
            base.OnExit(e);
        }
    }
}
