using Microsoft.Extensions.DependencyInjection;
using Models;
using mouse_lighting.Services.LightingHandlers;
using mouse_lighting.Services.Observer;
using mouse_lighting.Services.UserDialog;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace mouse_lighting.Services.DataService
{
    interface IExportService
    {
        public void ExportXmlAsync(Lighting? lighting, string? path);
    }
    internal class ExportService : IExportService
    {
        private readonly IUserDialog _UserDialog;
        private readonly IObserverStatusBar _StatusBar;
        //private ILightingConvert _Creator;

        static int i = 0;

        public ExportService(IObserverStatusBar observerStatusBar, ILightingConvert creator, IUserDialog userDialog)
        {
            i++;
            _UserDialog = userDialog;
            _StatusBar = observerStatusBar;
            //_Creator = creator;
        }
        private XDocument ConvertInXmlFile(Lighting lighting, List<FrameCycle> frames)
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

            return doc;
        }

        public void ExportXmlAsync(Lighting? lighting, string? path)
        {
            Debug.WriteLine("ExportService {0}", i);
            var _Creator = App.Services.GetRequiredService<ILightingConvert>();
            if (lighting == null || lighting.Cycles == null || lighting.Guid == default! || string.IsNullOrEmpty(lighting.Name)) { throw new ArgumentException(nameof(lighting)); }

            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) { _UserDialog.ShowNotification($"Path not found\n path:'{path}'"); return; }

            var fileName = $"File_{lighting.Guid.ToString().ToUpper()}";

            using (StreamWriter sw = new StreamWriter($"{path}\\{fileName}.xml", false, Encoding.Unicode))
            {
                List<FrameCycle> res = _Creator.Worker(lighting);
                if (res.Count == 0) { _UserDialog.ShowNotification("List is empty"); return; }
                var doc = ConvertInXmlFile(lighting, res);
            }
            _StatusBar.StatusBar($"Export {path}\\{fileName}.xml");
        }
    }
}
