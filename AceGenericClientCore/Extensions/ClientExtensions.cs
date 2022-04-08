using AceGenericClientFramework;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AceGenericClientCore.Extensions
{
    public static class ClientExtensions
    {
        /// <summary>
        /// Using IHttpClientFactory and binds IAceClientService with AceClientService for dependency injection.
        /// 
        /// </summary>
        /// <param name="timeOut">5 (in Seconds)</param>
        /// <param name="baseAddress">If baseAddress changes along the lifetime of client, leave it empty and provide
        /// url through its provided methods, ExecuteGetGeneric, ExecutePostGeneric etc.</param>
        /// <returns></returns>
        public static IServiceCollection AddAceClient(this IServiceCollection services,
            int? timeOutSeconds = null,
            string bankId = null,
            string channelCode = null,
            string baseAddress = null)
        {
            services.AddHttpClient<IAceClientService, AceClientService>(client =>
            {
                return new AceClientService(baseAddress??string.Empty, bankId, channelCode);
            })
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(timeOutSeconds ?? 60));

            return services;
        }
    }
}
