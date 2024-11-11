using System.Text;
using System.Xml.Linq;
using System.Windows.Media;
using Models;
using System.Windows.Forms;
namespace BloodyLightingXML
{
    internal class Program
    {
        static string path = "C:\\Program Files (x86)\\BloodyWorkShop8\\BloodyWorkShop8\\Accounts\\Guest\\Data\\English\\SLED\\Standard2_V8MMax";
        static string path_1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static string guid = "File_514E3362-EAAF-4CA8-A193-57BACE408B72";
        //static byte step = 20;
        //static double displayTime = 0.050;
        static void Main(string[] args)
        {
            string[] colors = new string[] { "#CF46FF", "#FFF265", "#12FFD9", "#FF2676" };

            List<FrameCycle> frames = new List<FrameCycle>();
            LightingCycle lightingCycle1 = new LightingCycle()
            {
                ColorWheelStart = (Color)ColorConverter.ConvertFromString("#FF0000"),
                ColorWheelEnd = (Color)ColorConverter.ConvertFromString("#04FF08"),
                ColorSecondStart = (Color)ColorConverter.ConvertFromString("#04FFEE"),
                ColorSecondEnd = (Color)ColorConverter.ConvertFromString("#F30AFF"),
                Step = 10,
                DisplayTime = 1
            };
            LightingCycle lightingCycle2 = new LightingCycle()
            {
                ColorWheelStart = (Color)ColorConverter.ConvertFromString("#04FF08"),
                ColorWheelEnd = (Color)ColorConverter.ConvertFromString("#FF0000"),
                ColorSecondStart = (Color)ColorConverter.ConvertFromString("#F30AFF"),
                ColorSecondEnd = (Color)ColorConverter.ConvertFromString("#04FFEE"),
                Step = 10,
                DisplayTime = 1
            };
            LightingCycle lightingCycle3 = new LightingCycle()
            {
                ColorWheelStart = (Color)ColorConverter.ConvertFromString("#FF0000"),
                ColorWheelEnd = (Color)ColorConverter.ConvertFromString("#FF0000"),
                ColorSecondStart = (Color)ColorConverter.ConvertFromString("#000000"),
                ColorSecondEnd = (Color)ColorConverter.ConvertFromString("#000000"),
                Step = 1,
                DisplayTime = 2
            };
            List<LightingCycle> lightingCycles = new List<LightingCycle>() { lightingCycle1, lightingCycle2, lightingCycle3 };


            foreach (var item in lightingCycles)
            {
                FirstColorTransitionFormSecondColor(item, frames);
            }

            Save(frames);
        }

        private static void FirstColorTransitionFormSecondColor(LightingCycle lightingCycle, List<FrameCycle> frames)
        {
            List<List<string>> colors = new List<List<string>>();
            double displayTime = Math.Round(lightingCycle.DisplayTime / lightingCycle.Step, 3);
            //TODO: Надо обдумать если в мышке больше 2 светодиодов. 
            colors.Add(FirstColorTransitionFormSecondColor(lightingCycle.ColorWheelStart, lightingCycle.ColorWheelEnd, lightingCycle.Step));
            colors.Add(FirstColorTransitionFormSecondColor(lightingCycle.ColorSecondStart, lightingCycle.ColorSecondEnd, lightingCycle.Step));

            int start_I = frames.Count;
            int frameIndex = 0;
            for (int i = 0; i < colors[0].Count; i++)
            {
                frames.Add(new FrameCycle() { DisplayTime = displayTime });
            }
            for (int i = 0; i < colors.Count; i++)
            {
                for (int j = 0; j < colors[i].Count; j++)
                {
                    frameIndex = start_I + j;
                    frames[start_I + j].ColorPicture[i] = colors[i][j];
                }
            }
        }
        private static List<string> FirstColorTransitionFormSecondColor(Color color_1, Color color_2, int step)
        {
            List<string> result = new List<string>();
            float R_1 = color_1.R;
            float G_1 = color_1.G;
            float B_1 = color_1.B;


            float R_2 = color_2.R;
            float G_2 = color_2.G;
            float B_2 = color_2.B;


            float step_R = (R_1 - R_2) / step;
            float step_G = (G_1 - G_2) / step;
            float step_B = (B_1 - B_2) / step;
            Color color_Wheel = default;
            for (int i = 0; i < step; i++)
            {
                color_Wheel = Color.FromRgb((byte)R_1, (byte)G_1, (byte)B_1);
                R_1 -= step_R;
                G_1 -= step_G;
                B_1 -= step_B;
                result.Add(ToHex(color_Wheel));
            }
            return result;
        }


        private static void WheelToSecondToWheel(Color color_1, Color color_2, int step)
        {
            float R = color_1.R;
            float G = color_1.G;
            float B = color_1.B;

            float step_R = R / step;
            float step_G = G / step;
            float step_B = B / step;

            for (int i = 0; i < step; i++)
            {
                var color_Wheel = Color.FromRgb((byte)R, (byte)G, (byte)B);
                var color_S = Color.FromRgb((byte)(step_R * i), (byte)(step_G * i), (byte)(step_B * i));
                R -= step_R;
                G -= step_G;
                B -= step_B;
                Console.WriteLine($"{ToHex(color_Wheel)},{ToHex(color_S)}");
            }

            //  R = 0; G = 0; B = 0;
            Console.WriteLine("===================");
            for (int i = step; i > 0; i--)
            {
                var color_Wheel = Color.FromRgb((byte)R, (byte)G, (byte)B);
                var color_S = Color.FromRgb((byte)(step_R * i), (byte)(step_G * i), (byte)(step_B * i));
                R += step_R;
                G += step_G;
                B += step_B;
                Console.WriteLine($"{ToHex(color_Wheel)},{ToHex(color_S)}");
            }
        }

        private static string ToHex(Color c) => $"{c.B:X2}{c.G:X2}{c.R:X2}";
        private static string CWToHex(Color c) => $"{c.R:X2}{c.G:X2}{c.B:X2}";
        private static void Save(List<FrameCycle> frames)
        {
            XDocument doc =
              new XDocument(
                    new XElement("SledAnimation",
                        new XElement("Name", "animation 3"),
                        new XElement("Guid", guid),
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

                new StreamWriter($"{path}\\{guid}.xml", false, Encoding.Unicode))
            {
                doc.Save(sw);
            }
            using (StreamWriter sw =

              new StreamWriter($"{path_1}\\{guid}.xml", false, Encoding.Unicode))
            {
                doc.Save(sw);
            }
        }
    }
}
