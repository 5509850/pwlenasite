﻿using System.Threading.Tasks;

namespace pw.lena.CrossCuttingConcerns.Helpers
{
    public  class TaskHelper
    {
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
    }
}
