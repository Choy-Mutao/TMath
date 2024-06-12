using tmath.algorithms.pso;
using tmath.algorithms.pso.Termination;

namespace tmath.algorithms.GAPSO.Termination
{
    public class ExecuteLimtTermination : ITermination
    {
        private int IterationCount = 0;
        private int MaxIterationCount = 0;

        public ExecuteLimtTermination(int max) => MaxIterationCount = max;

        public void Update() => IterationCount++;

        public bool HasReached(ISwarm swarm)
        {
            return !(IterationCount < MaxIterationCount);
        }
    }
}
