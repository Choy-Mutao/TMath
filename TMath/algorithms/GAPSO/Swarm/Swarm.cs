using System.Collections.Generic;
using tmath.algorithms.GAPSO.Fitness;
using tmath.algorithms.GAPSO.InformTopology;

namespace tmath.algorithms.pso
{
    public abstract class Swarm : ISwarm
    {
        InformTopology<IParticle> particles;
        IInformStrategy<IParticle> informStrategy;

        IFitness fitness;
        IGearbox gearbox;

        public void Evaluate()
        {
            throw new System.NotImplementedException();
        }

        public List<IParticle> GetNeighbours(in IParticle particle, double radius)
        {
            throw new System.NotImplementedException();
        }

        public void Initialization()
        {
            throw new System.NotImplementedException();
        }

        public void Move()
        {
            informStrategy.MakeDecision(particles);
        }
    }
}
