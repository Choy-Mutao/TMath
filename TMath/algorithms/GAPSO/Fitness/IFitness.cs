using tmath.algorithms.pso;

namespace tmath.algorithms.GAPSO.Fitness
{
    public interface IFitness
    {
        double Evaluate(IParticle particle);
    }
}
