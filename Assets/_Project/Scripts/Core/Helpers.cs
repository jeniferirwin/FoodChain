namespace FoodChain.Core
{
    // ABSTRACTION
    public static class Helpers
    {
        public static float MustBePositive(float value)
        {
            if (value > 0f)
                return value;
            else
                return 0f;
        }
        
        public static int MustBePositive(int value)
        {
            if (value > 0)
                return value;
            else
                return 0;
        }

        public static float MustBePercentage(float value)
        {
            if (value > 1f) return 1f;
            if (value < 0f) return 0f;
            return value;
        }
        
        public static float MustBePositivePercentage(float value)
        {
            return MustBePositive(MustBePercentage(value));
        }
    }
}
