using tmath.algorithms.pso;

namespace tmath.algorithms.GAPSO.InformTopology
{
    public abstract class InformTopology<T> where T : IParticle
    {
        public abstract void Add(T particle);

        public abstract void Inform(T particle);

        public abstract void InformAll();
    }
}
