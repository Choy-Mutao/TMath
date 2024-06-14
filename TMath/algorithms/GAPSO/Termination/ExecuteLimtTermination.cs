namespace tmath.algorithms.pso
{
    public class ExecuteLimtTermination : ITermination
    {
        private int IterationCount = 0;
        private int MaxIterationCount = 0;

        public ExecuteLimtTermination(int max) => MaxIterationCount = max;

        public bool HasReached(ISwarm swarm)
        {
            return !(IterationCount++ < MaxIterationCount);
        }
    }
}
