using tmath.algorithms.GAPSO.Termination;
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
        public double Previous_Fitness { get => m_prefitness; set => m_prefitness = value; }
        double[] m_velocity;
        public double[] Velocity { get => m_velocity; set => m_velocity = value; }
        public double[] m_bestposition;
        public double[] BestPosition { get => m_bestposition; set => m_bestposition = value; }

        int m_movecount;
        public int MoveCount => m_movecount;

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

            m_fitness = 0;
            m_prefitness = 0;
            m_movecount = 0;
        }

        public IParticle CreateNew()
        {
            DoubleParticle dp = new DoubleParticle(m_dimension, m_min, m_max);
            dp.m_position = randomization.GetDoubleArray(m_dimension, m_min, m_max);
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
            m_movecount++;
        }
    }


    [TestClass]
    public class Standard2006
    {
        [TestMethod]
        public void Parabola()
        {
            DoubleParticle doubleParticle = new DoubleParticle(2, -100, 100);

            int S = 10 + (int)(2 * Math.Sqrt(2));
            var swarm = new Swarm<DoubleParticle>(S, doubleParticle, new SPSO2006Gearbox());

            var termination = new ExecuteLimtTermination(100);
            var fitness = new ParabolaFitnsss();

            ParticleSwarmAlgorithm pso = new ParticleSwarmAlgorithm(swarm, termination, fitness);

            pso.InitializationHandler += (o, e) =>
            {
                Console.WriteLine();
            };

            pso.EvaluateHandler += (o, e) =>
            {
                Console.WriteLine();
            };

            pso.Start();
        }
    }
}
