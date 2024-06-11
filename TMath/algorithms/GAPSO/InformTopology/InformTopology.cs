using tmath.algorithms.pso;

namespace tmath.algorithms.GAPSO.InformTopology
{
    public abstract class InformTopology<T> where T : IParticle
    {


        public void Inform(T particle)
        {
            // Only Initial position;
            throw new System.NotImplementedException();
        }

        public void InformAll()
        {
            // Only Initial position;
            throw new System.NotImplementedException();
        }
    }
}
