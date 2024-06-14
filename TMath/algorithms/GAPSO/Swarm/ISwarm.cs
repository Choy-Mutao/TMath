namespace tmath.algorithms.pso
{
    /// <summary>
    /// The set of m_particles
    /// </summary>
    public interface ISwarm
    {
        double Best_Fitness { get; }
        IParticle BestParticle { get; }
        IGearbox GearBox { get; }
        void Initialization();
        void Evaluate();
    }
}
