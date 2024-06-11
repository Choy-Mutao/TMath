namespace tmath.algorithms.pso
{
    /// <summary>
    /// A particle is made of 
    /// 1. a position inside at this position
    /// 2. the fitness value at this position
    /// 3. a velocity(in fact a displacement), which will be used to compute the next position
    /// 4. a memory, that contains the best position(called the previous best) found so far by the particle
    /// 5. the fitness value of this previous best
    /// 
    /// NOTE: in this project, all geometry params use 2D message;
    /// </summary>
    public interface IParticle
    {
        void Initialization();
    }

}
