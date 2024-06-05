namespace tmath.algorithms.pso
{

    public class Particle : IParticle
    {
        private TPoint2D Position; // a position inside at this position;
        private TVector2D Velocity; // a velocity which will be used to compute the next position;

        private object Data;
        private object Memory; // contains the best position (called the previous best) found so far by the particle;

        private float Fitness; // the fitness value at this position
        private float Previous_Fitness; // the fitness value of this previous best

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
