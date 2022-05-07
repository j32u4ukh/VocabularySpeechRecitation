namespace UnityMVC
{
    public interface IController
    {
        void register(ICommander commnad);

        bool isExists(string name);

        ICommander release(string name);
    }
}
