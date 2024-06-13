namespace tmath.algorithms.pso
{
    public interface IInformTopology
    {
        IParticle[] Particles { get; }
        int InformCount { get; }

        void Build();
        void UpdateInformation();

        IParticle[] GetInformedParticles();
    }
}
