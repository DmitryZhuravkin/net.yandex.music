using System.Collections.Concurrent;

namespace DZzzz.Yandex.Music.Synchronizer.Queue
{
    public class MemorySyncQueue<T> : ISyncQueue<T>
    {
        private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

        public void Enqueue(T context)
        {
            queue.Enqueue(context);
        }

        public T TryDequeue()
        {
            if (queue.TryDequeue(out T result))
            {
                return result;
            }

            return default(T);
        }
    }
}