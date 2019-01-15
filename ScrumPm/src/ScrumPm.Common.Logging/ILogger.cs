using System;

namespace ScrumPm.Common.Logging
{
    public interface ILogger
    {
        void Error(string message);

        void Error(Exception exception);

        void Information(string message);

        void Verbose(string message);

        void Debug(string message);

        void Warning(string message);
    }
}
