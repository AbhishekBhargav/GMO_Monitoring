using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Media;

namespace SystemTrayApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;
        public SolidColorBrush basecolour { get; set; }
        
        



        protected override void OnStartup(StartupEventArgs e)
        {
            
            base.OnStartup(e);
            this.Resources["defaultcolour"] = new SolidColorBrush(Colors.Wheat);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            };
            void Onchanged(object sender, FileSystemEventArgs fe)
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

            
            void RC()
            {
                string path = ConfigurationManager.AppSettings.Get("Summary Logs") + "\\Summary_" + DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".json";
                   
                if (File.Exists(path))
                {
                    string Task;
                    string Task_Status;
                    string Task_Type;
                    string data;
                    using (FileStream fs = new FileStream(path.ToString(),
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.ReadWrite))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            
                            data = sr.ReadToEndAsync().Result;
                        }
                    }
                    JObject Root = JsonConvert.DeserializeObject<JObject>(data);
                    string ms = Root.SelectTokens("$.EventList...[?(@.Start_time <> 'NULL')].Start_time").Max().ToObject<DateTime>().GetDateTimeFormats('o')[0];
                    string me = Root.SelectTokens("$.EventList...[?(@.End_time <> 'NULL')].End_time").Max().ToObject<DateTime>().GetDateTimeFormats('o')[0];
                    
                    if (DateTime.Parse(me) > DateTime.Parse(ms))
                    {
                        Task = Root.SelectToken("$.EventList...[?(@.End_time == '" + me + "')].TaskID").ToString();
                        Task_Status = Root.SelectToken("$.EventList...[?(@.End_time == '" + me + "')].Status").ToString();
                        Task_Type = ((JProperty)((JToken)((JToken)Root.SelectTokens("$.EventList...[?(@.End_time == '"+me+"')].End_time").Max()).Parent.Parent.Parent.Parent.Parent.Parent)).Name;
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
                    
                    this.Dispatcher.Invoke(() =>
                    {
                        if (Task_Status=="Started" || Task_Status== "Completed"){
                        notifyIcon.ShowBalloonTip(Task_Type + " Notification", Task + " " + Task_Status + " at " + (DateTime.Parse(me) > DateTime.Parse(ms) ? me : ms), BalloonIcon.Info);
                        }
                        else if (Task_Status== "Stop_Fail" || Task_Status== "File_Not_Found") {
                        notifyIcon.ShowBalloonTip(Task_Type + " Notification", Task + " " + Task_Status + " at " + (DateTime.Parse(me) > DateTime.Parse(ms) ? me : ms), BalloonIcon.Warning);
                        }
                        else
                        {
                        notifyIcon.ShowBalloonTip(Task_Type + " Notification", Task + " " + Task_Status + " at " + (DateTime.Parse(me) > DateTime.Parse(ms) ? me : ms), BalloonIcon.Error);
                        }
                    });   
                    
                    if (Task_Type == "NFS")
                    {
                        NFS_result(data);
                    }
                    else
                    {
                        Reading_result(data);
                    }
                }
            }
            void Reading_result(string data = null)
            {
                string path = ConfigurationManager.AppSettings.Get("Summary Logs") + "\\Summary_" + DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".json";
                if (data == null & File.Exists(path)) {

                    using (FileStream fs = new FileStream(path.ToString(),
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.ReadWrite))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {

                            data = sr.ReadToEndAsync().Result;
                        }
                    }

                }else if (data != null) {
                    JObject Reading_status = JsonConvert.DeserializeObject<JObject>(data);
                    IEnumerable<JToken> Failed = Reading_status.SelectTokens("$.EventList.ReadingCollection..[?(@.Status == 'Failed')].Start_time");
                    IEnumerable<JToken> Stop_Fail = Reading_status.SelectTokens("$.EventList.ReadingCollection..[?(@.Status == 'Stop_Fail')].Start_time");
                    IEnumerable<JToken> Started = Reading_status.SelectTokens("$.EventList.ReadingCollection..[?(@.Status == 'Started')].Start_time");
                    IEnumerable<JToken> Completed = Reading_status.SelectTokens("$.EventList.ReadingCollection..[?(@.Status == 'Completed')].Start_time");
                    if (Failed.Any())
                    {
                        this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.Red);                        
                    } else if (Stop_Fail.Any())
                    {
                        this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.Brown);                        
                    } else if (Started.Any())
                    {
                        this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.DeepSkyBlue);                                            
                    }else if (Completed.Any())
                    {
                        this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.Green);                                               
                    }else
                    {
                        this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.Yellow);                        
                    }
                }else
                {
                    this.Resources["ReadingCollectionColour"] = new SolidColorBrush(Colors.Plum);
                    this.Dispatcher.Invoke(() =>
                    {
                        notifyIcon.ToolTipText = $"Path not Found: {path}";
                        
                    });
                    
                }
            }

            void NFS_result(string data=null)
            {
                string path = ConfigurationManager.AppSettings.Get("Summary Logs") + "\\Summary_" + DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".json";
                if (data == null & File.Exists(path))
                {

                    using (FileStream fs = new FileStream(path.ToString(),
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.ReadWrite))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {

                            data = sr.ReadToEndAsync().Result;
                        }
                    }

                }
                else if (data != null)
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


            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            
            if (Directory.Exists(ConfigurationManager.AppSettings.Get("Summary Logs")))
            {
                var Watcher = new FileSystemWatcher(ConfigurationManager.AppSettings.Get("Summary Logs"));
                //Watcher.Filter = ("Summary_" + DateTime.Today.ToString("yyyy") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("dd") + ".json");
                Watcher.NotifyFilter = NotifyFilters.LastWrite;
                Watcher.Changed += Onchanged;
                Watcher.IncludeSubdirectories = true;
                Watcher.EnableRaisingEvents = true;
                Reading_result();
                NFS_result();


            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    notifyIcon.ToolTipText = $"Path not Found: {ConfigurationManager.AppSettings.Get("Summary Logs")}";

                });
            }
        }

        

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
