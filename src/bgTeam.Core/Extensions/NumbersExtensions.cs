namespace bgTeam.Extensions
{
    using System;

    public static class NumbersExtensions
    {
        public static decimal RoundUp(this decimal input, int places)
        {
            decimal multiplier = Convert.ToDecimal(Math.Pow(10, Convert.ToDouble(places)));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
    }
}
