using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace mouse_lighting.Models
{
    public class Setting
    {
        private readonly string pathToSetting = $"{AppDomain.CurrentDomain.BaseDirectory}//setting.json";
        InnerSetting? _innerSetting;
        private InnerSetting _InnerSetting => _innerSetting ??= LoadSetting();
        class InnerSetting
        {
            public string PathToXML { get; set; } = string.Empty;
            public string PathToDb { get; set; } = "Led.db";
        }

        public event Action? UpdateDbPathEvent;

        public string PathToXML
        {
            get => _InnerSetting.PathToXML;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _InnerSetting.PathToXML = value;
                    SaveSetting();
                }
            }
        }
        public string PathToDb
        {
            get => _InnerSetting.PathToDb;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _InnerSetting.PathToDb = value;
                    SaveSetting();
                    UpdateDbPathEvent?.Invoke();
                }
            }
        }

        private InnerSetting LoadSetting()
        {
            InnerSetting setting = new InnerSetting();
            if (File.Exists(pathToSetting))
            {
                using (StreamReader sr = new StreamReader(pathToSetting, Encoding.Unicode))
                {
                    string line = sr.ReadToEnd();
                    setting = JsonConvert.DeserializeObject<InnerSetting>(line) ?? setting;
                }
            }
            return setting;
        }
        private void SaveSetting()
        {
            using (StreamWriter sw = new StreamWriter(pathToSetting, false, Encoding.UTF8))
            {
                sw.Write(JsonConvert.SerializeObject(_InnerSetting));
            }
        }
    }
}
