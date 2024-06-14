using tmath.algorithms.pso;

namespace tmath_lab.algorithm_lab.PSO
{

    // Use Fitness to describe problem;
    internal class ParabolaFitnsss : IFitness
    {
        double f_min = 0;

        public double Evaluate(IParticle particle)
        {
            double f = 0, p = 0, xd;
            for (int d = 0; d < particle.Dimension; d++)
            {
                xd = particle.Position[d] - p;
                f = f + xd * xd;
            }
            return Math.Abs(f - f_min);
        }
    }

    internal class DoubleParticle : IParticle
    {
        double[] m_position;
        public double[] Position { get => m_position; set => m_position = value; }
        int m_dimension;
        public int Dimension { get => m_dimension; set => m_dimension = value; }
        double m_fitness;
        public double Fitness { get => m_fitness; set => m_fitness = value; }
        double m_prefitness;
        public double BestFitness { get => m_prefitness; set => m_prefitness = value; }
        double[] m_velocity;
        public double[] Velocity { get => m_velocity; set => m_velocity = value; }
        public double[] m_bestposition;
        public double[] BestPosition { get => m_bestposition; set => m_bestposition = value; }

        int m_movecount;
        public int MoveCount => m_movecount;

        double[] m_globalbest;
        public double[] GlobalBest { get => m_globalbest; set => m_globalbest = value; }

        IRandomization randomization = new KISSRandomization();
        double m_min;
        double m_max;

        public DoubleParticle(int D, double min, double max)
        {
            m_dimension = D;
            m_min = min;
            m_max = max;

            m_position = new double[D];
            m_velocity = new double[D];
            m_bestposition = new double[D];
            m_globalbest = new double[D];

            m_fitness = 0;
            m_prefitness = 0;
            m_movecount = 0;
        }

        public IParticle CreateNew()
        {
            DoubleParticle dp = new DoubleParticle(m_dimension, m_min, m_max);
            dp.m_position = randomization.GetDoubleArray(m_dimension, m_min, m_max);
            dp.BestPosition = dp.Position;

            for (int i = 0; i < m_dimension; i++)
            {
                dp.m_velocity[i] = (randomization.GetDouble(m_min, m_max) - m_position[i]) / 2;
            }
            return dp;
        }

        public void Move()
        {
            for (int d = 0; d < m_dimension; d++)
                m_position[d] = m_position[d] + m_velocity[d];
            Confine();
            m_movecount++;
        }

        public void Confine()
        {
            for (int d = 0; d < m_dimension; d++)
            {
                if (m_position[d] < m_min)
                {
                    m_position[d] = m_min;
                    m_velocity[d] = 0;
                }
                if (m_position[d] > m_max)
                {
                    m_position[d] = m_max;
                    m_velocity[d] = 0;
                }
            }
        }

        public IParticle Clone()
        {
            DoubleParticle clone = new DoubleParticle(m_dimension, m_min, m_max);
            clone.m_position = m_position;
            clone.m_velocity = m_velocity;
            clone.m_fitness = m_fitness;
            clone.m_prefitness = m_prefitness;
            clone.m_bestposition = m_bestposition;
            clone.m_movecount = m_movecount;
            clone.m_globalbest = (double[])m_globalbest.Clone();

            return clone;
        }
    }


    [TestClass]
    public class Standard2006
    {
        [TestMethod]
        public void Parabola()
        {
            DoubleParticle doubleParticle = new DoubleParticle(2, -100, 100);
            var termination = new FitnessTermination(0, 0.9);
            var fitness = new ParabolaFitnsss();

            int S = 10 + (int)(2 * Math.Sqrt(2));
            var swarm = new SingleObjectiveSwarm<DoubleParticle>(S, doubleParticle, new SPSO2006Gearbox(), fitness);

            //var termination = new ExecuteLimtTermination(100);

            ParticleSwarmAlgorithm pso = new ParticleSwarmAlgorithm(swarm, termination);

            pso.Start();
        }
    }
}
