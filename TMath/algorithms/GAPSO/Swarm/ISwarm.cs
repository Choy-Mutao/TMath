namespace tmath.algorithms.pso
{
    /// <summary>
    /// The set of m_particles
    /// </summary>
    public interface ISwarm
    {
        double Best_Fitness { get; }
        IGearbox GearBox { get; }
        IInformTopology Topology { get; }
        void Initialization();
        void Move();
        IParticle[] CurrentParticles();
    }
}
