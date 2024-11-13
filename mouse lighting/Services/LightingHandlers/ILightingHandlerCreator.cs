using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace mouse_lighting.Services.LightingHandlers
{
    internal interface ILightingHandlerCreator
    {
        public List<FrameCycle> Worker(Lighting lighting);
    }

    internal class LightingHandlerCreator : ILightingHandlerCreator
    {
        public List<FrameCycle> Worker(Lighting lighting)
        {
            List<FrameCycle> frames = new List<FrameCycle>();
            var cycles = lighting.Cycles.OrderBy(c => c.IndexNumber);
            foreach (var item in cycles)
            {
                FirstColorTransitionFormSecondColor(item, frames);
            }
            return frames;
        }

        private void FirstColorTransitionFormSecondColor(LightingCycle lightingCycle, List<FrameCycle> frames)
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
        private List<string> FirstColorTransitionFormSecondColor(Color color_1, Color color_2, int step)
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
        private static string ToHex(Color c) => $"{c.B:X2}{c.G:X2}{c.R:X2}";
    }
}
