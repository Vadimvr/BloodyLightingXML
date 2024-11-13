using Models;
using mouse_lighting.Services.db;
using mouse_lighting.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace mouse_lighting.Services
{
    internal class DataService : IDataService
    {
        private readonly string _path = "C:\\Program Files (x86)\\BloodyWorkShop8\\BloodyWorkShop8\\Accounts\\Guest\\Data\\English\\SLED\\Standard2_V8MMax";
        private readonly string _pathToDb = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SLED";
        private ApplicationContextSqLite _appContext;
        public ApplicationContextSqLite DB => _appContext;

        static string path_1 = string.Empty;
        static string path = string.Empty;
        private ILocalData _localData;
        public Setting Setting { get; set; }

        public DataService(ILocalData localData)
        {
            _localData = localData;
            Setting = _localData.LoadSetting();
            if (Setting == null)
            {
                Setting = new Setting()
                {
                    PathToXML = _path,
                    PathToDb = _pathToDb
                };
            }
            _appContext = new ApplicationContextSqLite(Setting.PathToDb);
            path = Setting.PathToXML;
            path_1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
        public void SaveSetting()
        {
            if (Setting != null)
            {
                _localData.SaveSetting(Setting);
            }
        }
        public void SaveToXML(Lighting lighting, List<FrameCycle> frames)
        {
            DataService.Save(lighting, frames);
        }
        private static void Save(Lighting lighting, List<FrameCycle> frames)
        {
            var fileName = $"File_{lighting.Guid.ToString().ToUpper()}";
            XDocument doc =
              new XDocument(
                    new XElement("SledAnimation",
                        new XElement("Name", lighting.Name),
                        new XElement("Guid", fileName),
                        new XElement("IsFolder", false),
                        new XElement("FolderGuid", "Folder_00000000-0000-0000-0000-000000000000"),
                        new XElement("Description", "ckAnimation:For custom-made each frame."),
                        new XElement("Time", 0),
                        new XElement("BackgroundColor", "000000"),
                        new XElement("FrameCount", frames.Count),
                        frames.Select(
                            (x, i) => new XElement("Frame" + (i + 1),
                              new XElement("ColorPicture", string.Join(",", x.ColorPicture)),
                              new XElement("DisplayTime", x.DisplayTime)
                              )
                            )
            )
            );
            SaveInFile(path_1, fileName, doc);
            SaveInFile(path, fileName, doc);
        }

        private static void SaveInFile(string path, string fileName, XDocument doc)
        {
            if (Path.Exists(path))
            {
                using (StreamWriter sw = new StreamWriter($"{path}\\{fileName}.xml", false, Encoding.Unicode))
                {
                    doc.Save(sw);
                }
            }
        }
    }
    public interface ILocalData
    {
        Setting LoadSetting();
        void SaveSetting(Setting setting);
    }

    public class LocalData : ILocalData
    {


        static string pathToSetting = $"{AppDomain.CurrentDomain.BaseDirectory}//setting.json";
        public Setting LoadSetting()
        {
            Setting setting = null;
            if (File.Exists(pathToSetting))
            {
                using (StreamReader sr = new StreamReader(pathToSetting, Encoding.Unicode))
                {
                    string line = sr.ReadToEnd();
                    setting = JsonConvert.DeserializeObject<Setting>(line);
                }
            }
            return setting;
        }
        public void SaveSetting(Setting setting)
        {
            using (StreamWriter sw = new StreamWriter(pathToSetting, false, Encoding.UTF8))
            {
                sw.Write(JsonConvert.SerializeObject(setting));
            }
        }
    }

    public class Setting
    {
        public string PathToXML { get; set; } = "C:\\Program Files (x86)\\BloodyWorkShop8\\BloodyWorkShop8\\Accounts\\Guest\\Data\\English\\SLED\\Standard2_V8MMax";
        public string PathToDb { get; set; }

    }
}
