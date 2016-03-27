namespace CurrentTasksTrayIconNotifier
{
    internal interface IRunable
    {
        void Run();
        void SetParam(string name, object value);
    }
}