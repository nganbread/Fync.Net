using System;

namespace Fync.Common.Common
{
    public interface ILogger
    {
        void Log(string text);
        void Log(string text, params object[] objects);
        void Log(object o);
        Guid Listen(Action<string> updateAction);
    }
}