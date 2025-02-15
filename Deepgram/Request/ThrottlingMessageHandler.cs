﻿// originally from https://github.com/saguiitay/OneDriveRestAPI

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Deepgram.Request
{
    internal class ThrottlingMessageHandler : DelegatingHandler
    {
        private readonly TimeSpanSemaphore _execTimeSpanSemaphore;

        public ThrottlingMessageHandler(TimeSpanSemaphore execTimeSpanSemaphore)
            : this(execTimeSpanSemaphore, new HttpClientHandler { AllowAutoRedirect = true })
        { }

        public ThrottlingMessageHandler(TimeSpanSemaphore execTimeSpanSemaphore, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            _execTimeSpanSemaphore = execTimeSpanSemaphore;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _execTimeSpanSemaphore?.RunAsync(base.SendAsync, request, cancellationToken) ?? base.SendAsync(request, cancellationToken);
        }
    }
}