namespace tmath.algorithms.pso
{
    public class Swarm<T> : ISwarm where T : IParticle
    {
        IParticle meta_particle;
        IInformTopology m_topology;

        IFitness fitness;
        IGearbox m_gearbox;

        int Size;
        double m_bestfitness;
        double mean_fitness;

        public Swarm(int size, IParticle mp, IGearbox gearbox)
        {
            Size = size;
            meta_particle = mp;
            m_gearbox = gearbox;
        }

        public double Best_Fitness => m_bestfitness;

        public IGearbox GearBox => m_gearbox;

        public IInformTopology Topology => m_topology;

        public void Initialization()
        {
            ExceptionHelper.ThrowIfNull("meta_particle", meta_particle);
            ExceptionHelper.ThrowIfNull("gearbox", m_gearbox);

            IParticle[] particles = new IParticle[Size];
            for (int i = 0; i < Size; i++) particles[i] = meta_particle.CreateNew();

            m_topology = new LinksInformTopology(particles);
            m_topology.Build();
        }

        public void Move()
        {
            m_topology.UpdateInformation();
            var mps = m_topology.GetInformedParticles();
            for (int i = 0; i < mps.Length; i++)
            {
                m_gearbox.Drive(mps[i]);
                mps[i].Move();
            }
        }

        public IParticle[] CurrentParticles() => m_topology?.Particles;
    }
}
