namespace tmath.algorithms.pso
{
    public class FitnessTermination : ITermination
    {
        double m_target;
        double m_tolerance;
        public FitnessTermination(double target, double tolerance)
        {
            m_target = target;
            m_tolerance = tolerance;
        }

        public bool HasReached(ISwarm swarm)
        {
            return NumberUtils.CompValue(swarm.Best_Fitness, m_target, m_tolerance) == 0;
        }

        public void Update()
        {}
    }
}
