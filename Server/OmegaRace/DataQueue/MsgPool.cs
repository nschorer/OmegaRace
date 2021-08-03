using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace OmegaRace
{
    // As outlined here: https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool
    public class ObjectPool<T>
    {
        private readonly ConcurrentBag<T> _objects;
        private readonly Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
            _objects = new ConcurrentBag<T>();
        }

        public T Get() => _objects.TryTake(out T item) ? item : _objectGenerator();

        public void Return(T item) => _objects.Add(item);
    }

    public class DataMessagePool
    {
        private ObjectPool<DataMessage> pool = null;
        private static DataMessagePool instance = null;

        public static DataMessagePool Instance()
        {
            if (instance == null)
            {
                instance = new DataMessagePool();
            }
            return instance;
        }

        private DataMessagePool()
        {
            pool = new ObjectPool<DataMessage>(() => new DataMessage());
        }

        public static DataMessage Get()
        {
            return instance.pool.Get();
        }

        public static void Return(DataMessage msg)
        {
            instance.pool.Return(msg);
        }
    }
}
