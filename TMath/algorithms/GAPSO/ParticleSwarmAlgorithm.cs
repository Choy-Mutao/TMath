using System;

namespace tmath.algorithms.pso
{
    public class ParticleSwarmAlgorithm
    {
        ISwarm Swarm;
        ITermination Termination;
        IRandomization randomization = new KISSRandomization();

        public event EventHandler InitializationHandler;
        public event EventHandler VelocityHandler;
        public event EventHandler InformHandler;
        public event EventHandler EvaluateHandler;
        public event EventHandler TerminateHandler;

        public ParticleSwarmAlgorithm(ISwarm swarm, ITermination termination)
        {
            Swarm = swarm;
            Termination = termination;
        }

        public void Start()
        {

            ExceptionHelper.ThrowIfNull("swarm", Swarm);
            ExceptionHelper.ThrowIfNull("randomization", randomization);
            ExceptionHelper.ThrowIfNull("termination", Termination);

            // Build
            Swarm.Initialization();
            // Evaluation
            do
            {
                Swarm.Evaluate();
            } while (!Termination.HasReached(Swarm));
        }
    }
}
