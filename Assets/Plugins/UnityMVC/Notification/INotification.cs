using System;

namespace UnityMVC
{
    public interface INotification
    {
        void setName(string name);

        public string getName();

        public void setData(object data);

        public object getData();

        public T1 getData<T1>(T1 default_t1 = default);

        public (T1, T2) getData<T1, T2>(T1 default_t1 = default, T2 default_t2 = default);

        public (T1, T2, T3) getData<T1, T2, T3>(T1 default_t1 = default, T2 default_t2 = default, T3 default_t3 = default);

        public (T1, T2, T3, T4) getData<T1, T2, T3, T4>(T1 default_t1 = default, T2 default_t2 = default, T3 default_t3 = default, T4 default_t4 = default);

        public (T1, T2, T3, T4, T5) getData<T1, T2, T3, T4, T5>(T1 default_t1 = default, T2 default_t2 = default, T3 default_t3 = default, 
                                                                T4 default_t4 = default, T5 default_t5 = default);

        public (T1, T2, T3, T4, T5, T6) getData<T1, T2, T3, T4, T5, T6>(T1 default_t1 = default, T2 default_t2 = default,  T3 default_t3 = default,
                                                                        T4 default_t4 = default, T5 default_t5 = default, T6 default_t6 = default);

        public (T1, T2, T3, T4, T5, T6, T7) getData<T1, T2, T3, T4, T5, T6, T7>(T1 default_t1 = default, T2 default_t2 = default, T3 default_t3 = default, 
                                                                                T4 default_t4 = default, T5 default_t5 = default, T6 default_t6 = default, 
                                                                                T7 default_t7 = default);

        public (T1, T2, T3, T4, T5, T6, T7, Tuple<T8>) getData<T1, T2, T3, T4, T5, T6, T7, T8>(T1 default_t1 = default, T2 default_t2 = default, 
                                                                                               T3 default_t3 = default, T4 default_t4 = default, 
                                                                                               T5 default_t5 = default, T6 default_t6 = default, 
                                                                                               T7 default_t7 = default, T8 default_t8 = default);
    }
}
