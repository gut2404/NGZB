using System;

namespace NGZB.Models.Class
{
    public class StringHelp
    {
        /// <summary>
        /// 生成本站MD5小写字符串（32位)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string getMd5(string str)
        {
            JsMd5 jsmd5 = new JsMd5();
            return jsmd5.md5(jsmd5.md5(str).ToString().Replace('a', 'c').Replace('f', 'h')).ToString();
        }

        /// <summary>
        /// 普通随机数：只用于生成单个随机数,不能用于生成循环随机数
        /// </summary>
        /// <param name="randnumlength"></param>
        /// <returns></returns>
        public static string GetRandnum(int randnumlength)
        {
            string sb;
            Random randnum = new Random(unchecked((int)DateTime.Now.Ticks));
            sb = randnum.NextDouble().ToString().Replace(".", "").Substring(0, randnumlength);
            return sb.Trim();
        }

        /// <summary>
        /// 高级随机数：生成循环随机数
        /// </summary>
        /// <param name="randnumlength">长度不大于10</param>
        /// <param name="xh">生成随机数的种子，取循环的序号</param>
        /// <returns></returns>
        public static string GetRandnumMore(int randnumlength, int xh)
        {
            Random randnum = new Random(xh * unchecked((int)DateTime.Now.Ticks));
            string sb = randnum.NextDouble().ToString().Replace(".", "").Substring(0, randnumlength);
            return sb.Trim();
        }

        /// <summary>
        /// 生成介于min与max之间的随机数,如果是小数，小数位数取给定范围位数最多的那个
        /// </summary>
        /// <param name="xh">循环标识，生成不同种子数，如果不是循环生产，此值可为大于0的任何整数</param>
        /// <param name="max">随机数最大值，可以取此值</param>
        /// <param name="min">随机数最小值，可以取此值</param>
        /// <returns></returns>
        public static decimal GetRandnumBetween(int xh, decimal min, decimal max)
        {
            xh = xh + 1;
            decimal sb;
            string[] max_s = max.ToString().Split('.');
            string[] min_s = min.ToString().Split('.');
            int max_l = 0;
            int min_l = 0;
            int main_l = 0;
            if (max_s.Length > 1)
            {
                max_l = max_s[1].Length;
            }
            if (min_s.Length > 1)
            {
                min_l = min_s[1].Length;
            }
            if (max_l == min_l)
            {
                main_l = max_l;
            }
            else
            {
                if (max_l > min_l)
                {
                    main_l = max_l;
                }
                else
                {
                    main_l = min_l;
                }
            }
            int bs = 1;
            for (int i = 0; i < main_l; i++)
            {
                bs = bs * 10;
            }
            decimal _max_ = max * bs;
            decimal _min_ = min * bs;
            int _max = (int)_max_;
            int _min = (int)_min_;
            Random randnum = new Random(xh * unchecked((int)DateTime.Now.Ticks));
            sb = Math.Round((decimal)randnum.Next(_min, _max) / bs, main_l);
            return sb;
        }

        /// <summary>
        /// 生成介于min与max之间的随机数
        /// </summary>
        /// <param name="xh">循环标识，生成不同种子数，如果不是循环生产，此值可为大于0的任何整数</param>
        /// <param name="max">随机数最大值，可以取此值</param>
        /// <param name="min">随机数最小值，可以取此值</param>
        /// <returns></returns>

        public static int GetRandnumBetween_int(int xh, int min, int max)
        {
            xh = xh + 1;
            int sb;
            Random randnum = new Random(xh * unchecked((int)DateTime.Now.Ticks));
            sb = randnum.Next(min, max);
            return sb;
        }
    }
}