using Models;
using mouse_lighting.Services.DataService;
using System.IO;

namespace mouse_lighting.Models
{
    /// <summary>
    /// Search for a folder with XML files
    /// </summary>
    internal class FindFolder
    {
        private readonly IExportService _IExport;

        public FindFolder(IExportService exportService)
        {
            _IExport = exportService;
        }
        public void Find()
        {
            // TODO
            string pathAccount = "C:\\Program Files (x86)\\BloodyWorkShop8\\BloodyWorkShop8\\Accounts\\Guest\\Data";
            if (Path.Exists(pathAccount))
            {
                var directories = Directory.GetDirectories(pathAccount);
                foreach (var item in directories)
                {
                    var locate = item.Substring(pathAccount.Length).Replace("\\", string.Empty);
                    var sLed = $"{item}\\SLED";
                    if (Path.Exists(sLed))
                    {
                        var sLedDirectories = Directory.GetDirectories(sLed);
                        foreach (var pa in sLedDirectories)
                        {
                            var dir = pa.Substring(sLed.Length).Replace("\\", string.Empty);

                            Lighting lighting = new Lighting()
                            {
                                Name = $"{locate} {dir}",
                                Guid = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                                Cycles = new List<LightingCycle> { new LightingCycle() }
                            };
                            _IExport.ExportXmlAsync(lighting, pa);
                        }
                    }
                }
            }
        }
    }
}
