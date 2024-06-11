namespace tmath.algorithms.pso
{
    public struct Veolcity
    {
        double[] V;
    }

    public interface IGearbox
    {
        void Drive(IParticle particle);
    }
}
