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
        private ObjectPool<DataMessage_Collision> collision = null;
        private ObjectPool<DataMessage_Fire> fire = null;
        private ObjectPool<DataMessage_Move> move = null;
        private ObjectPool<DataMessage_ObjectTransform> objTransform = null;
        private ObjectPool<DataMessage_SpawnMissile> spawnMissile = null;
        private ObjectPool<DataMessage_ClockQuery> clockQuery = null;
        private ObjectPool<DataMessage_ClockResponse> clockResponse = null;

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
            collision = new ObjectPool<DataMessage_Collision>(() => new DataMessage_Collision());
            fire = new ObjectPool<DataMessage_Fire>(() => new DataMessage_Fire());
            move = new ObjectPool<DataMessage_Move>(() => new DataMessage_Move());
            objTransform = new ObjectPool<DataMessage_ObjectTransform>(() => new DataMessage_ObjectTransform());
            spawnMissile = new ObjectPool<DataMessage_SpawnMissile>(() => new DataMessage_SpawnMissile());
            clockQuery = new ObjectPool<DataMessage_ClockQuery>(() => new DataMessage_ClockQuery());
            clockResponse = new ObjectPool<DataMessage_ClockResponse>(() => new DataMessage_ClockResponse(0));
        }

        public static DataMessage_Collision Get_Collision()
        {
            return instance.collision.Get();
        }

        public static DataMessage_Fire Get_Fire()
        {
            return instance.fire.Get();
        }

        public static DataMessage_Move Get_Move()
        {
            return instance.move.Get();
        }

        public static DataMessage_ObjectTransform Get_ObjectTransform()
        {
            return instance.objTransform.Get();
        }

        public static DataMessage_SpawnMissile Get_SpawnMissile()
        {
            return instance.spawnMissile.Get();
        }

        public static DataMessage_ClockQuery Get_ClockQuery()
        {
            return instance.clockQuery.Get();
        }

        public static DataMessage_ClockResponse Get_ClockResponse()
        {
            return instance.clockResponse.Get();
        }

        public static void Return(DataMessage_Collision msg)
        {
            instance.collision.Return(msg);
        }

        public static void Return(DataMessage_Move msg)
        {
            instance.move.Return(msg);
        }

        public static void Return(DataMessage_Fire msg)
        {
            instance.fire.Return(msg);
        }

        public static void Return(DataMessage_ObjectTransform msg)
        {
            instance.objTransform.Return(msg);
        }

        public static void Return(DataMessage_SpawnMissile msg)
        {
            instance.spawnMissile.Return(msg);
        }

        public static void Return(DataMessage_ClockQuery msg)
        {
            instance.clockQuery.Return(msg);
        }

        public static void Return(DataMessage_ClockResponse msg)
        {
            instance.clockResponse.Return(msg);
        }
    }
}
