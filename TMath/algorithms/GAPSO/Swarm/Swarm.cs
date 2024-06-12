using System.Collections.Generic;
using tmath.algorithms.GAPSO.Fitness;
using tmath.algorithms.GAPSO.InformTopology;

namespace tmath.algorithms.pso
{
    public class Swarm<T> : ISwarm where T: IParticle
    {
        InformTopology<T> particles;

        IFitness fitness;
        IGearbox gearbox;

        double mean_fitness;
        uint Size;

        public Swarm(uint size)
        {
            Size = size;
        }

        public void Evaluate()
        {

        }

        public List<IParticle> GetNeighbours(in IParticle particle, double radius)
        {
            throw new System.NotImplementedException();
        }

        public void Initialization()
        {


        }

        public void Move()
        {
        }
    }
}
