namespace tmath.algorithms.pso
{
    public class SPSO2006Gearbox : IGearbox
    {
        double w;
        double c;

        public IRandomization randomization;

        public SPSO2006Gearbox()
        {
            randomization = new KISSRandomization();
        }

        public void Drive(IParticle particle)
        {
            int D = particle.Dimension;

            var v = particle.Velocity;
            var p = particle.Position;
            var bp = particle.BestPosition;

            for(int d = 0; d < D; d++)
            {
                particle.Velocity[d] =  v[d] + randomization.GetDouble(0, c) * (bp[d] - p[d]);
            }
        }
    }
}
