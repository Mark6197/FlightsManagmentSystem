using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BL
{
    class FlightCenterSystem
    {

        private static FlightCenterSystem _instance;

        private static readonly object key = new object();

        private FlightCenterSystem()
        {
        }

        public static FlightCenterSystem GetInstance()
        {

            if (_instance == null)
            {
                lock (key)
                {
                    if (_instance == null)
                    {
                        _instance = new FlightCenterSystem();
                    }
                }
            }

            return _instance;
        }

        public T GetFacade<T>() where T : FacadeBase, new()
        {
            return new T();
        }


    }
}
