using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace FOREL
{
    class Element : ICloneable
    {
        public Element()
        {
            DoNothing();
        }

        //inline-method
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DoNothing()
        {
            ;
        }

        public Element(Double mass)
        {
            Mass = mass;
        }

        public Element(Double mass, Double x, Double y)
        {
            Mass = mass;
            X = x;
            Y = y;
        }

        public Element(Double mass, Double x, Double y, SolidColorBrush color)
        {
            Mass = mass;
            X = x;
            Y = y;
            Color = color;
        }

        public Double X { get; set; }

        public Double Y { get; set; }

        public Double Mass { get; set; }

        public SolidColorBrush Color { get; set; }

        public Object Clone() =>
            new Element(Mass, X, Y, Color.Clone());
    }
}
