using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUnit.Utils
{
    internal class SingletonProvider<T> where T :new()
    {
        private SingletonProvider() {}
        private static T instance;
        private static readonly object lockObject = new object();
        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        static SingletonProvider()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            
        }
    }
}
