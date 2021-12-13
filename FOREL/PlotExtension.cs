using InteractiveDataDisplay.Core;
using System;

namespace FOREL
{
    static class PlotExtension
    {
        public static void ShowInLimitX(this Plot plot, Double min, Double max)
        {
            plot.PlotOriginX = min;
            plot.PlotWidth = Math.Abs(max - min);
        }

        public static void ShowInLimitY(this Plot plot, Double min, Double max)
        {
            plot.PlotOriginY = min;
            plot.PlotHeight = Math.Abs(max - min);
        }
    }
}
