using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace UnityMVC
{
    public class Notification : INotification
    {
        string name = string.Empty;
        object data = null;

        public Notification(string name, object data = null)
        {
            setName(name: name);
            setData(data: data);
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return name;
        }

        public void setData(object data)
        {
            this.data = data;
        }

        public override string ToString()
        {
            string description = $"Notification({name})";

            if(data != null)
            {
                description += $" -> data: {data}";
            }

            return description;
        }

        public object getData()
        {
            return data;
        }

        public T1 getData<T1>(T1 default_t1 = default)
        {
            if(data == null)
            {
                return default_t1;
            }
            else
            {
                return (T1)data;
            }
        }

        public (T1, T2) getData<T1, T2>(T1 default_t1 = default, T2 default_t2 = default)
        {
            if (data == null)
            {
                return (default_t1, default_t2);
            }
            else
            {
                return (data as (T1, T2)?).Value;
            }
        }

        public (T1, T2, T3) getData<T1, T2, T3>(T1 default_t1 = default, T2 default_t2 = default, T3 default_t3 = default)
        {
            if(data == null)
            {
                return (default_t1, default_t2, default_t3);
            }
            else
            {
                return (data as (T1, T2, T3)?).Value;
            }
        }

        public (T1, T2, T3, T4) getData<T1, T2, T3, T4>(T1 default_t1 = default, T2 default_t2 = default, T3 default_t3 = default, T4 default_t4 = default)
        {
            if(data == null)
            {
                return (default_t1, default_t2, default_t3, default_t4);
            }
            else
            {
                return (data as (T1, T2, T3, T4)?).Value;
            }
        }

        public (T1, T2, T3, T4, T5) getData<T1, T2, T3, T4, T5>(T1 default_t1 = default, T2 default_t2 = default, T3 default_t3 = default, T4 default_t4 = default, T5 default_t5 = default)
        {
            if(data == null)
            {
                return (default_t1, default_t2, default_t3, default_t4, default_t5);
            }
            else
            {
                return (data as (T1, T2, T3, T4, T5)?).Value;
            }
        }

        public (T1, T2, T3, T4, T5, T6) getData<T1, T2, T3, T4, T5, T6>(T1 default_t1 = default, T2 default_t2 = default, T3 default_t3 = default, T4 default_t4 = default, T5 default_t5 = default, T6 default_t6 = default)
        {
            if(data == null)
            {
                return (default_t1, default_t2, default_t3, default_t4, default_t5, default_t6);
            }
            else
            {
                return (data as (T1, T2, T3, T4, T5, T6)?).Value;
            }
        }

        public (T1, T2, T3, T4, T5, T6, T7) getData<T1, T2, T3, T4, T5, T6, T7>(T1 default_t1 = default, T2 default_t2 = default, T3 default_t3 = default, T4 default_t4 = default, T5 default_t5 = default, T6 default_t6 = default, T7 default_t7 = default)
        {
            if(data == null)
            {
                return (default_t1, default_t2, default_t3, default_t4, default_t5, default_t6, default_t7);
            }
            else
            {
                return (data as (T1, T2, T3, T4, T5, T6, T7)?).Value;
            }
        }

        public (T1, T2, T3, T4, T5, T6, T7, Tuple<T8>) getData<T1, T2, T3, T4, T5, T6, T7, T8>(T1 default_t1 = default, T2 default_t2 = default, T3 default_t3 = default, T4 default_t4 = default, T5 default_t5 = default, T6 default_t6 = default, T7 default_t7 = default, T8 default_t8 = default)
        {
            if(data == null)
            {
                return (default_t1, default_t2, default_t3, default_t4, default_t5, default_t6, default_t7, new Tuple<T8>(default_t8));
            }
            else
            {
                return (data as (T1, T2, T3, T4, T5, T6, T7, Tuple<T8>)?).Value;
            }
        }
    }
}