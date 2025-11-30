using System.Configuration;

namespace QuizApp.Data
{
    public sealed class DbConfig
    {
        private static DbConfig _instance;
        private static readonly object _lock = new object();

        public string ConnectionString { get; }

        private DbConfig()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["QuizDb"].ConnectionString;
        }

        public static DbConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new DbConfig();
                    }
                }
                return _instance;
            }
        }
    }
}
