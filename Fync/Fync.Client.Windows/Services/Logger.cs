using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Fync.Common;
using Fync.Common.Common;
using Newtonsoft.Json;

namespace Fync.Client.Windows.Services
{
    internal class Logger : ILogger
    {
        private readonly ConcurrentQueue<string> _lines;
        private readonly IDictionary<Guid, Action<string>> _listeners;

        public Logger()
        {
            _lines = new ConcurrentQueue<string>();
            _listeners = new Dictionary<Guid, Action<string>>();
        }

        public void Log(string text, params object[] objects)
        {
            Log(text.FormatWith(objects));
        }

        public void Log(object o)
        {
            Log(JsonConvert.SerializeObject(o));
        }

        public void Log(string text)
        {
            _lines.Enqueue(text);

            Trigger();
        }

        private void Trigger()
        {
            var lines = string.Join(Environment.NewLine, _lines.Reverse());//.Take(20));
            foreach (var listener in _listeners.Values)
            {
                listener(lines);
            }
        }

        public Guid Listen(Action<string> updateAction)
        {
            var cancelToken = Guid.NewGuid();
            _listeners.Add(cancelToken, updateAction);
            Trigger();
            return cancelToken;
        }

        public void StopListening(Guid cancelToken)
        {
            _listeners.Remove(cancelToken);
        }
    }
}