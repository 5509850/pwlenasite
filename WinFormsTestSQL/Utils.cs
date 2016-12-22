using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTestSQL
{
    public static class Utils
    {
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static int getRandom()
        {
            int random = 0;
            int.TryParse(string.Format("{0}{1}{2}{3}{4}{5}",
                GetRandomNumberNotZero(),
                GetRandomNumber(),
                GetRandomNumber(),
                GetRandomNumber(),
                GetRandomNumber(),
                GetRandomNumber()
                ), out random);
            return random;
        }



        private static int GetRandomNumber()
        {
            lock (syncLock)
            { // synchronize
                //int min, int max
                return getrandom.Next(10);
            }
        }

        private static int GetRandomNumberNotZero()
        {
            lock (syncLock)
            { // synchronize
                //int min, int max
                return getrandom.Next(1, 10);
            }
        }

    }
}
