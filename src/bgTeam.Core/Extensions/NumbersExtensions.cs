namespace bgTeam.Extensions
{
    using System;

    /// <summary>
    /// Расширения для работы с числами
    /// </summary>
    public static class NumbersExtensions
    {
        /// <summary>
        /// Округляет в большую сторону число input до places знака
        /// </summary>
        /// <param name="input">Округляемое число</param>
        /// <param name="places">Округление до какого знака</param>
        /// <returns>Округлённое число</returns>
        public static decimal RoundUp(this decimal input, int places)
        {
            decimal multiplier = Convert.ToDecimal(Math.Pow(10, Convert.ToDouble(places)));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
    }
}
