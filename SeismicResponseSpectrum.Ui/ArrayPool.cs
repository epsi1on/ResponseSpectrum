using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SeismicResponseSpectrum.Ui
{
    public class ObjectPool<T>
    {
        private ConcurrentBag<T> _objects;
        private Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator;
        }

        public T GetObject()
        {
            T item;
            if (_objects.TryTake(out item)) return item;
            return _objectGenerator();
        }

        public void PutObject(T item)
        {
            _objects.Add(item);
        }
    }

    public class DoubleArrayPool
    {
        private static ConcurrentBag<double[]> Pool = new ConcurrentBag<double[]>();

        public static int tmp = 00;

        public static double[] GetArray(int length)
        {
            tmp++;
            double[] buf;

            if (Pool.TryTake(out buf))
            {
                if (buf.Length != length)
                    Array.Resize(ref buf, length);

                for (int i = length - 1; i >= 0; i--)
                {
                    buf[i] = 0;
                }

                return buf;
            }

            return new double[length];

        }

        public static void PutArray(double[] arr)
        {
            Pool.Add(arr);
        }
    }
}
