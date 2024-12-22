using System;

namespace IngameScript
{
    class EchoLogger : ILoggable
    {
        private Action<string> _echo;
        
        public EchoLogger(Action<string> echo)
        {
            _echo = echo;
        }
        public void Log(string message)
        {
            _echo(message);
        }

        public void LogLine(string message)
        {
            _echo(message + "\n");
        }
    }
}