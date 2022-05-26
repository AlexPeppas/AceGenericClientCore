using Nbg.NetCore.Services.Ace.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Nbg.NetCore.Services.Ace.Http.Extensions
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
            string baseAddress = null,
            int? timeOutSeconds = null,
            string bankId = null
            )
        {
            
            services.AddHttpClient<IAceClientService, AceClientService>(client =>
            {
                return new AceClientService(baseAddress??string.Empty, bankId);
            })
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(timeOutSeconds ?? Settings.ClientSettings.timeOutSeconds));

            return services;
        }
    }
}
