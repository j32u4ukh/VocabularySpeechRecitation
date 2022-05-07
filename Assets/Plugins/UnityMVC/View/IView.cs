namespace UnityMVC
{
    public interface IView
    {
        void register(IMediator mediator);

        bool isExists(string name);

        IMediator get(string name);

        IMediator release(string name);
    }
}
