namespace FoodChain
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

        public static float MustBePercentage(float value)
        {
            if (value > 1f) return 1f;
            if (value < 0f) return 0f;
            return value;
        }
    }
}
