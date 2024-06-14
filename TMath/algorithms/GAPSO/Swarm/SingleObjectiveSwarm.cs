namespace tmath.algorithms.pso
{
    public class SingleObjectiveSwarm<T> : ISwarm where T : IParticle
    {

        IParticle meta_particle; // 元祖粒子
        // statistic numbers;
        int Size;
        IParticle[] m_particles;
        int[,] LINKS;

        IParticle m_bestparticle = null;
        public IParticle BestParticle { get => m_bestparticle; } //TODO: 不可以被任意改变, 存值而非引用

        IFitness m_fitness;

        IGearbox m_gearbox;
        public IGearbox GearBox => m_gearbox;

        double m_bestfitness;
        public double Best_Fitness => m_bestfitness;

        double mean_fitness;

        public SingleObjectiveSwarm(int size, IParticle mp, IGearbox gearbox, IFitness fitness)
        {
            Size = size;
            meta_particle = mp;
            m_fitness = fitness;
            m_gearbox = gearbox;
            m_bestfitness = double.MaxValue;
        }

        public void Initialization()
        {
            ExceptionHelper.ThrowIfNull("meta_particle", meta_particle);
            ExceptionHelper.ThrowIfNull("gearbox", m_gearbox);

            m_particles = new IParticle[Size];
            LINKS = new int[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                m_particles[i] = meta_particle.CreateNew();

                double f = m_fitness.Evaluate(m_particles[i]);
                m_particles[i].Fitness = f;
                m_particles[i].BestFitness = f;

                if (f < m_bestfitness)
                {
                    m_bestfitness = f;
                    m_bestparticle = m_particles[i].Clone();
                }
            }
        }

        private void ResetLINKS()
        {
            int size = m_particles.Length;
            for (int i = 0; i < size; i++)
            {
                //LINKS[i] = new int[size];
                for (int j = 0; j < size; j++)
                {
                    LINKS[i, j] = 0;
                }
                LINKS[i, i] = 1;
            }
        }

        private void Inform()
        {
            ResetLINKS();
            int K = 3;
            int S = m_particles.Length;
            KISSRandomization randomization = new KISSRandomization();
            for (int m = 0; m < S; m++)
            {
                for (int i = 0; i < K; i++)
                {
                    int s = randomization.GetInt(0, S - 1);
                    LINKS[m, s] = 1;
                }
            }
        }

        public void Evaluate()
        {
            Inform();

            for (int s = 0; s < Size; s++)
            {
                var sp = m_particles[s];
                sp.GlobalBest = BestParticle.Position;

                IParticle best = m_particles[s];
                for (int m = 0; m < Size; m++)
                {
                    if (LINKS[s, m] == 1 && m_particles[m].Fitness < best.Fitness)
                        best = m_particles[m].Clone();
                }

                sp.BestPosition = best.BestPosition;

                // ... compute the new velocity, and move
                GearBox.Drive(ref sp);
                sp.Move();

                double f = m_fitness.Evaluate(sp);

                sp.Fitness = f;
                if (sp.Fitness < sp.BestFitness)
                {
                    sp.BestPosition = (double[])sp.Position.Clone();
                    if (sp.Fitness < best.Fitness)
                    {
                        best = sp.Clone();
                    }
                }

            }

            // statistic
        }
    }
}
