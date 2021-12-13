using InteractiveDataDisplay.Core;
using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FOREL
{
    public static class Representation
    {
        public static Ellipse GetCircle(Double x, Double y, Double radius, SolidColorBrush color, Double thickness)
        {
            Ellipse ellipse = new Ellipse();

            ellipse.Fill = color;
            ellipse.StrokeThickness = thickness;

            ellipse.SetValue(Plot.X1Property, x - radius);
            ellipse.SetValue(Plot.Y1Property, y - radius);
            ellipse.SetValue(Plot.X2Property, x + radius);
            ellipse.SetValue(Plot.Y2Property, y + radius);

            return ellipse;
        }
    }
}
