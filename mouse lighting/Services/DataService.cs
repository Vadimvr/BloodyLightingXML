using mouse_lighting.Models;
using mouse_lighting.Services.db;
using mouse_lighting.Services.Interfaces;
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
        private ApplicationContextSqLite _appContext;
        public ApplicationContextSqLite DB => _appContext;
      
        public DataService()
        {
            _appContext = new ApplicationContextSqLite();
            _appContext.Database.EnsureCreated();
        }
        static string path = "C:\\Program Files (x86)\\BloodyWorkShop8\\BloodyWorkShop8\\Accounts\\Guest\\Data\\English\\SLED\\Standard2_V8MMax";
        static string path_1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public void Save(Lighting lighting, List<FrameCycle> frames)
        {
            SaveToXML(lighting, frames);
        }
        private static void SaveToXML(Lighting lighting, List<FrameCycle> frames)
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
            using (StreamWriter sw =

                new StreamWriter($"{path}\\{fileName}.xml", false, Encoding.Unicode))
            {
                doc.Save(sw);
            }
            using (StreamWriter sw =

              new StreamWriter($"{path_1}\\{fileName}.xml", false, Encoding.Unicode))
            {
                doc.Save(sw);
            }
        }


    }
}
