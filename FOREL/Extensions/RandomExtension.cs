using System;

namespace FOREL.Extensions
{
    static class RandomExtension
    {
        public static Double NextDouble(this Random random, Double lowerLimit, Double upperLimit) =>
            (upperLimit - lowerLimit) * random.NextDouble() + lowerLimit;
    }
}
