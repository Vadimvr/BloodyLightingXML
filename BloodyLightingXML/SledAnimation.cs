using Models;
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
        public List<FrameCycle>? Frames { get; set; } = new List<FrameCycle>();

        public int FrameCount { get; set; }
    }
 
}
