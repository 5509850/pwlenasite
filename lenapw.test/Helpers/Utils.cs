using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace lenapw.test.Helpers
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

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static Stream ObjectToStream(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms;
            }
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

        public static Task Complete()
        {
            var result = new TaskCompletionSource<bool>();
            result.SetResult(true);
            return result.Task;
        }

        public static Task<T> Complete<T>(T result)
        {
            var r = new TaskCompletionSource<T>();
            r.SetResult(result);
            return r.Task;
        }

        public static  string InitSqlPath(string connectionString)
        {
#if DEBUG
            return ConfigurationManager.ConnectionStrings["Local_alexandr_gorbunov_ConnectionString"].ConnectionString;
            //return ConfigurationManager.ConnectionStrings["kitchen_ConnectionString"].ConnectionString;
#else
            return connectionString;
#endif

        }

    }

    


}
