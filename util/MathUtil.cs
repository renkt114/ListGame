using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSLG.util
{
    public static  class MathUtil
    {

        public static int RoundUpToDigit(int number, int digit)
        {
            // 指定された桁の数を取得
            int divisor = (int)Math.Pow(10, digit);

            // 指定された桁で割り、切り上げ
            int roundedNumber = (int)Math.Ceiling((double)number / divisor);

            // 元の桁に戻す
            int result = roundedNumber * divisor;

            return result;
        }


    }
}
