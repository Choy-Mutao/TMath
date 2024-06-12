using System;
using tmath.algorithms.GAPSO.Fitness;
using tmath.algorithms.pso.Termination;

namespace tmath.algorithms.pso
{
    public class ParticleSwarmAlgorithm
    {
        ISwarm Swarm;
        ITermination Termination;
        IFitness Fitness;
        IRandomization randomization = new KISSRandomization();

        public event EventHandler InitializationHandler;
        public event EventHandler VelocityHandler;
        public event EventHandler InformHandler;
        public event EventHandler EvaluateHandler;
        public event EventHandler TerminateHandler;

        public ParticleSwarmAlgorithm(ISwarm swarm, ITermination termination, IFitness fitness)
        {
            Swarm = swarm;
            Termination = termination;
            Fitness = fitness;
        }

        public void Start()
        {
            // Initialization
            Swarm.Initialization();

            // Evaluation
            do
            {
                Swarm.Evaluate();
                Termination.Update();
            } while (Termination.HasReached(Swarm));
        }

        public void Stop()
        {
        }
    }
}
