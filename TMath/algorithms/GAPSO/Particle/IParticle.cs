namespace tmath.algorithms.pso
{

    public interface IParticle
    {
        double[] GlobalBest { get; set; }
        double[] BestPosition { get; set; }
        double[] Position { get; set; }
        double[] Velocity { get; set; }

        int Dimension { get; set; }
        double Fitness { get; set; } // the fitness value at this position
        double BestFitness { get; set; } // the fitness value of this previous best

        int MoveCount { get; }
        IParticle CreateNew();
        IParticle Clone();
        void Move();
        void Confine();
    }

}
