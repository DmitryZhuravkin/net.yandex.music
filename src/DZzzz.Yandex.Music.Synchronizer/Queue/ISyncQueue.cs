namespace DZzzz.Yandex.Music.Synchronizer.Queue
{
    public interface ISyncQueue<T>
    {
        void Enqueue(T context);
        T TryDequeue();
    }
}