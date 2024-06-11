namespace tmath.algorithms.pso
{
    partial struct Velocity
    {
        int size; // Dimension
        double[] v;
    }

    partial struct Position
    {
        int size;   // Dimension
        double[] x;
        //double f;
    }

    public class BaseParticle : IParticle
    {
        Velocity velocity;
        Position position;

        private double Fitness; // the fitness value at this position
        private double Previous_Fitness; // the fitness value of this previous best

        /// <summary>
        /// The velocity update equations
        /// </summary>

        public void UpdateSpeed()
        {

        }

        /// <summary>
        /// Move by applying the new velocity to the current position
        /// </summary>
        public void Move()
        {

        }

        public void Inform()
        {

        }

    }


    public class LinearSwarm
    {

    }

    public class RingSwarm
    {

    }

    public class InformStrategy
    {

    }
}
