namespace tmath.algorithms.pso.Termination
{
    public interface ITermination
    {
        void Update();
        bool HasReached(ISwarm swarm);
    }
}
