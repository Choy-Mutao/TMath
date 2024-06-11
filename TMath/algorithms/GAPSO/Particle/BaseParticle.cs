namespace tmath.algorithms.pso
{
    partial struct Position
    {
        int size;   // Dimension
        double[] x;
        //double f;
    }

    public class BaseParticle : IParticle
    {
        Position position;

        private double Fitness; // the fitness value at this position
        private double Previous_Fitness; // the fitness value of this previous best

        public void Initialization()
        {
            // Only Initial position;
            throw new System.NotImplementedException();
        }
    }

}
