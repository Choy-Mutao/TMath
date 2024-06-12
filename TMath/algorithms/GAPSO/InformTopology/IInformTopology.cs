namespace tmath.algorithms.pso
{
    public interface IInformTopology
    {
        IParticle[] Particles { get; }

        void Build();

        void Inform(IParticle particle);

        void InformAll();
    }
}
