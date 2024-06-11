using tmath.algorithms.GAPSO.InformTopology;

namespace tmath.algorithms.pso
{
    public interface IInformStrategy<T> where T : IParticle
    {
        void MakeDecision(InformTopology<T> particles);
    }
}
