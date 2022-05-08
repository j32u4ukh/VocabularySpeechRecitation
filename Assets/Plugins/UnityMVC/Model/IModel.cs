namespace UnityMVC
{
    public interface IModel
    {
        void register(IProxy proxy);

        bool isExists(string name);

        bool tryGet(string name, out IProxy proxy);

        IProxy release(string name);
    }
}