namespace tmath.algorithms.pso
{

    public interface IParticle
    {
        double[] BestPosition { get; set; }
        double[] Position { get; set; }
        double[] Velocity { get; set; }

        int Dimension { get; set; }
        double Fitness { get; set; } // the fitness value at this position
        double Previous_Fitness { get; set; } // the fitness value of this previous best
        IParticle CreateNew();
    }

}
