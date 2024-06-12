using tmath.algorithms.GAPSO.Randomization;

namespace tmath.algorithms.pso
{
    public class KISSRandomization : BaseRandomization
    {
        static ulong RAND_MAX_KISS = 4294967295;
        static ulong kiss_x;
        static ulong kiss_y;
        static ulong kiss_z;
        static ulong kiss_w;
        static ulong kiss_carry = 0;

        static ulong kiss_k;
        static ulong kiss_m;

        public KISSRandomization(ulong seed)
        {
            kiss_x = seed | 1;
            kiss_y = seed | 2;
            kiss_z = seed | 4;
            kiss_w = seed | 8;
            kiss_carry = 0;
        }
        ulong KISS()
        {
            kiss_x = kiss_x * 69069 + 1;
            kiss_y ^= kiss_y << 13;
            kiss_y ^= kiss_y >> 17;
            kiss_y ^= kiss_y << 5;
            kiss_k = (kiss_z >> 2) + (kiss_w >> 3) + (kiss_carry >> 2);
            kiss_m = kiss_w + kiss_w + kiss_z + kiss_carry;
            kiss_z = kiss_w;
            kiss_w = kiss_m;
            kiss_carry = kiss_k >> 30;
            return kiss_x + kiss_y + kiss_w;
        }

        public override double GetDouble() => KISS() / RAND_MAX_KISS;

    }
}