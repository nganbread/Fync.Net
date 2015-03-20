namespace Fync.Common.Common
{
    public static class Logger
    {
        private static ILogger _instance;

        public static ILogger Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                if (_instance == null) _instance = value;
            }
        }
    }
}