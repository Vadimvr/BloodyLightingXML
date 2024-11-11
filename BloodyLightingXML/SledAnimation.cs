using System.Windows.Media;
using System.Xml.Serialization;

namespace BloodyLightingXML
{
    public class SledAnimation
    {
        public string? Name { get; set; }
        public Guid? Guid { get; set; }
        public string Description { get; set; } = "ckAnimation:For custom-made each frame。";
        public int Time { get; set; } = 0;
        public string BackgroundColor = "000000";
        [XmlIgnoreAttribute]
        public List<Frame>? Frames { get; set; } = new List<Frame>();

        public int FrameCount { get; set; }
    }

    public class Frame
    {
        public List<string> ColorPicture { get; set; } = (new string[18]).Select(x => x = "000000").ToList();
        public double DisplayTime { get; set; } = 0.005;
    }

 
}
