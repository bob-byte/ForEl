using System;

namespace FOREL
{
    static class RandomExtension
    {
        public static Double NextDouble(this Random random, Double lowerLimit, Double upperLimit) =>
            (upperLimit - lowerLimit) * random.NextDouble() + lowerLimit;
    }
}
