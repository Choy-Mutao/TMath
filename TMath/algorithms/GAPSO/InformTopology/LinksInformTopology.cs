using System;

namespace tmath.algorithms.pso
{
    internal class LinksInformTopology : IInformTopology
    {
        public int[][] LINKS;
        IParticle[] m_particles;

        public LinksInformTopology(IParticle[] particles)
        {
            m_particles = particles;
        }

        public IParticle[] Particles => m_particles;

        public void Inform(IParticle particle)
        {
            throw new NotImplementedException();
        }

        public void InformAll()
        {
            throw new NotImplementedException();
        }

        public void Build()
        {
            int size = m_particles.Length;
            LINKS = new int[size][];
            for (int i = 0; i < size; i++)
            {
                LINKS[i] = new int[size];
                for (int j = 0; j < size; j++)
                {
                    LINKS[i][j] = 0;
                }
                LINKS[i][i] = 1;
            }
        }
    }
}
