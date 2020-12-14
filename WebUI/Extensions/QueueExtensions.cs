using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Extensions
{
    public static class QueueExtensions
    {
        public static IEnumerable<T> DequeueChunk<T>(this ConcurrentQueue<T> queue, int chunkSize)
        {
            for (int i = 0; i < chunkSize && queue.Count > 0; i++)
            {
                T result = default;
                var isReturned = queue.TryDequeue(out result);
                if (!isReturned)
                {
                    continue;
                }
                yield return result;
            }
        }

        public static void Remove<T>(this ConcurrentQueue<T> queue, T itemToRemove) where T : class
        {
            var list = queue.ToList(); //Needs to be copy, so we can clear the queue
            queue.Clear();
            foreach (var item in list)
            {
                if (item == itemToRemove)
                    continue;

                queue.Enqueue(item);
            }
        }

        public static void RemoveAt<T>(this ConcurrentQueue<T> queue, int itemIndex)
        {
            var list = queue.ToList(); //Needs to be copy, so we can clear the queue
            queue.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                if (i == itemIndex)
                    continue;

                queue.Enqueue(list[i]);
            }
        }
    }
}
