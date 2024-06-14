namespace tmath.algorithms.pso
{

    public interface IGearbox
    {
        /// <summary>
        /// 加速度的生成, 受目标的影响
        /// </summary>
        /// <param name="swarm"></param>
        void Drive(ref IParticle particle);
    }
}
