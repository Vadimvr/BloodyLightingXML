using mouse_lighting.Models;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace mouse_lighting.Services.LightingHandlers
{
    internal interface ILightingHandlerCreator
    {

        public List<FrameCycle> Worker(Lighting lighting);
    }

    internal class LightingHandlerCreator : ILightingHandlerCreator
    {
        Dictionary<LightingHandlersEnum, ILightingHandlerCycle> lightingHandlers;
        public LightingHandlerCreator()
        {
            lightingHandlers = new();
            lightingHandlers.Add(LightingHandlersEnum.ShowHide, new LightingHandler_ShowHide());
        }
        public List<FrameCycle> Worker(Lighting lighting)
        {
            List<FrameCycle> frames = new List<FrameCycle>();
            foreach (var item in lighting.Cycles)
            {
                if (lightingHandlers.ContainsKey(item.Handler))
                {
                    lightingHandlers[item.Handler].AddedFrames(item, frames);
                }
            }
            return frames;
        }
    }

    enum LightingHandlersEnum
    {
        ShowHide,
        Hide,
        Show,
    }

    internal interface ILightingHandlerCycle
    {
        public void AddedFrames(LightingCycle lightingCycle, List<FrameCycle> frames);
    }

    internal class LightingHandler_ShowHide : ILightingHandlerCycle
    {
        public void AddedFrames(LightingCycle lightingCycle, List<FrameCycle> frames)
        {
            System.Windows.Media.Color color_1 = lightingCycle.ColorWheelStart;
            int step = lightingCycle.Step;
            var displayTime = Math.Round( lightingCycle.DisplayTime / step,3);

            double R = color_1.R;
            double G = color_1.G;
            double B = color_1.B;

            double step_R = R / step;
            double step_G = G / step;
            double step_B = B / step;

            for (int i = 0; i < step; i++)
            {
                var color_Wheel = Color.FromRgb((byte)R, (byte)G, (byte)B);
                var color_S = Color.FromRgb((byte)(step_R * i), (byte)(step_G * i), (byte)(step_B * i));
                R -= step_R;
                G -= step_G;
                B -= step_B;
                Console.WriteLine($"{ToHex(color_Wheel)},{ToHex(color_S)}");
                FrameCycle frame = new FrameCycle();
                frame.ColorPicture[0] = ToHex(color_Wheel);
                frame.ColorPicture[1] = ToHex(color_S);
                frame.DisplayTime = displayTime;
                frames.Add(frame);
            }

            for (int i = step; i > 0; i--)
            {
                var color_Wheel = Color.FromRgb((byte)R, (byte)G, (byte)B);
                var color_S = Color.FromRgb((byte)(step_R * i), (byte)(step_G * i), (byte)(step_B * i));
                R += step_R;
                G += step_G;
                B += step_B;
                Console.WriteLine($"{ToHex(color_Wheel)},{ToHex(color_S)}");
                FrameCycle frame = new FrameCycle();
                frame.ColorPicture[0] = ToHex(color_Wheel);
                frame.ColorPicture[1] = ToHex(color_S);
                frame.DisplayTime = displayTime;
                frames.Add(frame);
            }
        }
        // R <==> B haha ;) 
        private string ToHex(Color c) => $"{c.B:X2}{c.G:X2}{c.R:X2}";
    }
}
