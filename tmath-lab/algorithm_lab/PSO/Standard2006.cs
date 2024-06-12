using tmath.algorithms.GAPSO.Fitness;
using tmath.algorithms.GAPSO.Termination;
using tmath.algorithms.pso;

namespace tmath_lab.algorithm_lab.PSO
{

    // Use Fitness to describe problem;
    internal class ParabolaFitnsss : IFitness
    {
        public double Evaluate(IParticle particle)
        {
            double f = 0, p = 0, xd;
            for (int d = 0; d < particle.Dimension; d++)
            {
                xd = particle.Position[d] - p;
                f = f + xd * xd;
            }
            return f;
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

        IRandomization randomization = new KISSRandomization();
        double m_min;
        double m_max;

        public DoubleParticle(int D, double min, double max)
        {
            m_dimension = D;
            m_min = min;
            m_max = max;
            m_position = randomization.GetDoubleArray(D, m_min, m_max);
            m_fitness = 0;
            m_prefitness = 0;
        }

        public IParticle CreateNew()
        {
            DoubleParticle dp = new DoubleParticle(m_dimension, m_min, m_max);
            return dp;
        }
    }


    [TestClass]
    public class Standard2006
    {
        [TestMethod]
        public void Parabola()
        {
            DoubleParticle doubleParticle = new DoubleParticle(2, -100, 100);
            uint S = 10 + (uint)(2 * Math.Sqrt(2));
            Swarm<DoubleParticle> swarm = new Swarm<DoubleParticle>(S);

            ExecuteLimtTermination termination = new ExecuteLimtTermination(100);
            ParabolaFitnsss fitness = new ParabolaFitnsss();


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
