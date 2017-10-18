namespace Trcont.Cud.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam.Extensions;

    public static class ETSNGHelper
    {
        public static string AddControlDigit(string estngCode)
        {
            estngCode.CheckNull(nameof(estngCode));

            if (estngCode.Length != 5)
            {
                throw new ArgumentException("ЕСТНГ код должен состоять из 5 цифр");
            }

            int result;
            if (!int.TryParse(estngCode, out result))
            {
                throw new ArgumentException("ЕСТНГ код должен состоять из 5 цифр");
            }

            return AddControlDigitInternal(result);
        }

        private static string AddControlDigitInternal(int estngCode)
        {
            if (estngCode == 0)
            {
                return "0";
            }

            int scale;
            int pos = 5;
            int sum = 0;
            int i = 0;

            while(i < 2)
            {
                int val = estngCode;
                scale = pos;
                while(val != 0)
                {
                    sum += val % 10 * (scale + i);
                    scale--;
                    val = val / 10;
                }

                sum = sum % 11;
                if (sum != 10)
                    break;
                i += 2;
            }

            if (sum == 0)
                sum = 0;

            string result = (estngCode * 10 + sum).ToString();
            return result.Substring(result.Length - 6, 6);
        }
    }
}
