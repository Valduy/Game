using System;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
    public static class TaskHelper
    {
        public static async Task<T> GetResultOrThrowOnTimeOutAsync<T>(this Task<T> task, int timer)
        {
            if (await Task.WhenAny(task, Task.Delay(timer)) == task)
            {
                return await task;
            }

            throw new TimeoutException();
        }
    }
}
