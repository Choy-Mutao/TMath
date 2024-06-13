using System;
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

            ExceptionHelper.ThrowIfNull("swarm", Swarm);
            ExceptionHelper.ThrowIfNull("fitness", Fitness);
            ExceptionHelper.ThrowIfNull("randomization", randomization);
            ExceptionHelper.ThrowIfNull("termination", Termination);

            // Build
            Swarm.Initialization();

            // Evaluation
            do
            {
                Swarm.Move();
                Evaluate();
                Termination.Update();
            } while (!Termination.HasReached(Swarm));
        }

        void Evaluate()
        {
            var particles = Swarm.CurrentParticles();
            int S = particles.Length;
            for (int s = 0; s < S; s++)
            {
                particles[s].Fitness = Fitness.Evaluate(particles[s]);
            }
        }
    }
}
