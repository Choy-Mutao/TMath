namespace tmath.algorithms.pso
{
    public class Swarm<T> : ISwarm where T : IParticle
    {
        IParticle meta_particle;
        public IInformTopology topology;

        IFitness fitness;
        IGearbox gearbox;

        int Size;
        double m_bestfitness;
        double mean_fitness;

        public Swarm(int size, IParticle mp)
        {
            Size = size;
            meta_particle = mp;
        }

        public double Best_Fitness => m_bestfitness;

        public void Initialization()
        {
            ExceptionHelper.ThrowIfNull("meta_particle", meta_particle);
            ExceptionHelper.ThrowIfNull("topology", topology);

            IParticle[] particles = new IParticle[Size];
            for(int i = 0; i < Size; i++) particles[i] = meta_particle.CreateNew();
            topology.Build();
        }

        public void Move()
        {
        }

        public IParticle[] CurrentParticles() => topology.Particles;
    }
}
