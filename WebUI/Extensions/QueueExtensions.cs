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
    }
}
