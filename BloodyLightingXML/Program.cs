using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace BloodyLightingXML
{
    internal class Program
    {
        static string path = "C:\\Program Files (x86)\\BloodyWorkShop8\\BloodyWorkShop8\\Accounts\\Guest\\Data\\English\\SLED\\Standard2_V8MMax";
        static string path_1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static string guid = "File_E252B9B3-3FFF-4C57-A1AF-520935AC25F8";
        static byte step =100;
        static double displayTime = 0.050;
        static void Main(string[] args)
        {
            string[] colors = new string[] { "#CF46FF", "#FFF265", "#12FFD9", "#FF2676" };

            List<Frame> frames = new List<Frame>();
            foreach (var item in colors)
            {
                Color color = System.Drawing.ColorTranslator.FromHtml(item);
                AddedFrames(color, frames);
            }

            Save(frames);
        }

        private static void AddedFrames(Color color_1, List<Frame> frames)
        {
            float R = color_1.R;
            float G = color_1.G;
            float B = color_1.B;

            float step_R = R / step;
            float step_G = G / step;
            float step_B = B / step;

            for (int i = 0; i < step; i++)
            {
                var color_Wheel = Color.FromArgb((byte)R, (byte)G, (byte)B);
                var color_S = Color.FromArgb((byte)(step_R * i), (byte)(step_G * i), (byte)(step_B * i));
                R -= step_R;
                G -= step_G;
                B -= step_B;
                Console.WriteLine($"{ToHex(color_Wheel)},{ToHex(color_S)}");
                Frame frame = new Frame();
                frame.ColorPicture[0] = ToHex(color_Wheel);
                frame.ColorPicture[1] = ToHex(color_S);
                frame.DisplayTime = displayTime;
                frames.Add(frame);
            }

            //  R = 0; G = 0; B = 0;
            Console.WriteLine("===================");
            for (int i = step; i > 0; i--)
            {
                var color_Wheel = Color.FromArgb((byte)R, (byte)G, (byte)B);
                var color_S = Color.FromArgb((byte)(step_R * i), (byte)(step_G * i), (byte)(step_B * i));
                R += step_R;
                G += step_G;
                B += step_B;
                Console.WriteLine($"{ToHex(color_Wheel)},{ToHex(color_S)}");
                Frame frame = new Frame();
                frame.ColorPicture[0] = ToHex(color_Wheel);
                frame.ColorPicture[1] = ToHex(color_S);
                frame.DisplayTime = displayTime;
                frames.Add(frame);
            }
        }

        private static string ToHex(Color c) => $"{c.R:X2}{c.G:X2}{c.B:X2}";
        private static void Save(List<Frame> frames)
        {
            XDocument doc =
              new XDocument(
                    new XElement("SledAnimation",
                        new XElement("Name", "my Animation"),
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
