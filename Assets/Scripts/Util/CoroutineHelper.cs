using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class CoroutineHelper
    {
        public static Coroutine StartThrowingCoroutine(this MonoBehaviour monoBehaviour, IEnumerator enumerator, Action<Exception> done) 
            => monoBehaviour.StartCoroutine(RunThrowingIterator(enumerator, done));

        public static IEnumerator RunThrowingIterator(IEnumerator enumerator, Action<Exception> done)
        {
            while (true)
            {
                object current;

                try
                {
                    if (enumerator.MoveNext() == false)
                    {
                        break;
                    }

                    current = enumerator.Current;
                }
                catch (Exception ex)
                {
                    done?.Invoke(ex);
                    yield break;
                }

                yield return current;
            }

            done?.Invoke(null);
        }

        public static IEnumerator ToCoroutine<T>(this Task<T> task, Action<Task<T>> completed)
        {
            yield return new WaitUntil(() => task.IsCompleted);
            completed?.Invoke(task);
        }
	}
}
