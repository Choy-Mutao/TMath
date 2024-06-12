using System;
using tmath.algorithms.pso;

namespace tmath.algorithms.GAPSO.Randomization
{
    public abstract class BaseRandomization : IRandomization
    {
        // returen 0 to 1;
        public abstract double GetDouble();

        public double GetDouble(double min, double max) => min + (max - min) * GetDouble();

        public double[] GetDoubleArray(int length, double min, double max)
        {
            double[] doubles = new double[length];
            for (int i = 0; i < length; i++) doubles[i] = GetDouble(min, max);
            return doubles;
        }

        public float GetFloat() => (float)GetDouble();

        public float GetFloat(float min, float max) => min + (max - min) * GetFloat();

        public float[] GetFloatArray(int length, float min, float max)
        {
            float[] floats = new float[length];
            for (int i = 0; i < length; i++) floats[i] = GetFloat(min, max);
            return floats;
        }

        public int GetInt(int min, int max)
        {
            if (min == max) return min;
            double r = GetDouble((double)min, (double)max + 1);
            int ir = (int)Math.Floor(r);
            if (ir > max) ir = max;
            return ir;
        }

        public int[] GetIntArray(int length, int min, int max)
        {
            int[] ints = new int[length];
            for (int i = 0; i < length; i++) ints[i] = GetInt(min, max);
            return ints;
        }
    }
}
