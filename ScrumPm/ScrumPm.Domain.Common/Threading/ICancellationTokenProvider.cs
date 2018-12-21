using System.Threading;

namespace ScrumPm.Domain.Common.Threading
{
    public class ICancellationTokenProvider
    {
        public CancellationToken Token { get; }
    }
}
