namespace tmath.algorithms.pso
{
    public interface IRandomization
    {
        int GetInt(int min, int max);
        float GetFloat(float min, float max);
        double GetDouble();
        double GetDouble(double min, double max);

        int[] GetIntArray(int length, int min, int max);
        float[] GetFloatArray(int length, float min, float max);
        double[] GetDoubleArray(int length, double min, double max);
    }
}
