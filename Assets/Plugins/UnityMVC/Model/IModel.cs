namespace UnityMVC
{
    public interface IModel
    {
        void register(IProxy proxy);

        bool isExists(string name);

        IProxy get(string name);

        IProxy release(string name);
    }
}