using System.Threading;

namespace ScrumPm.Domain.Common.Threading
{
    public class NullCancellationTokenProvider : ICancellationTokenProvider
    {
        public static NullCancellationTokenProvider Instance { get; } = new NullCancellationTokenProvider();

        public CancellationToken Token { get; } = default;

        private NullCancellationTokenProvider()
        {
            
        }
    }
}
