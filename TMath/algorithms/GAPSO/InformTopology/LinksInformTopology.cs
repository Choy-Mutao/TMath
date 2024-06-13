using System.Collections.Generic;

namespace tmath.algorithms.pso
{
    public class LinksInformTopology : IInformTopology
    {
        public int[,] LINKS;
        IParticle[] m_particles;
        int m_informcount;
        int K = 3;
        public IRandomization randomization;

        public LinksInformTopology(IParticle[] particles)
        {
            m_particles = particles;
            m_informcount = 0;
            randomization = new KISSRandomization();
        }

        public IParticle[] Particles => m_particles;

        public int InformCount => m_informcount;

        public void Build()
        {
            int size = m_particles.Length;
            LINKS = new int[size, size];
            Reset();
        }

        public void Reset()
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

        public void UpdateInformation()
        {
            Reset();
            int S = m_particles.Length;
            for (int m = 0; m < S; m++)
            {
                for (int i = 0; i < K; i++)
                {
                    int s = randomization.GetInt(0, S - 1);
                    LINKS[m, s] = 1;
                }
            }
            m_informcount++;
        }

        public IParticle[] GetInformedParticles()
        {
            int S = m_particles.Length;
            List<IParticle> fps = new List<IParticle>();
            for (int s = 0; s < S; s++)
            {
                for (int m = 0; m < S; m++)
                {
                    if (LINKS[s, m] == 1) fps.Add(m_particles[m]);
                }
            }
            return fps.ToArray();
        }
    }
}
