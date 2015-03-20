using Fync.Common.Common;
using PropertyChanged;

namespace Fync.Client.Windows.ViewModels.Windows
{
    [ImplementPropertyChanged]
    internal class LoggingWindowModel
    {
        public LoggingWindowModel(ILogger logger)
        {
            logger.Listen(LoggerUpdated);
        }

        private void LoggerUpdated(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}
