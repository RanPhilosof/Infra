namespace RP.Infra.Logger
{
    public interface ILogger
    {
        void Error(Exception ex);
        void Error(string message);
        void Warning(string message);
        void Info(string message);
    }
}
