namespace Auth.SharedKernel.Domain.Notification
{
    public interface INotification
    {
        void Add(string message);
        string GetErrors();
        T AddWithReturn<T>(string message);
        bool Any();
    }
}