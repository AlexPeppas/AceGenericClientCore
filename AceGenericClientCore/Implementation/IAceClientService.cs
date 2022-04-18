using Nbg.NetCore.Services.Ace.Http.Types;
using System.Threading.Tasks;

namespace Nbg.NetCore.Services.Ace.Http
{
    public interface IAceClientService
    {
        
        AceClientResponse<R> ExecuteGetGeneric<T, R>(AceClientRequest<T> myNbgRequest,string url);

        Task<AceClientResponse<R>> ExecuteGetGenericAsync<T, R>(AceClientRequest<T> myNbgRequest, string url);

        AceClientResponse<R> ExecuteGetGeneric<R>(AceClientRequestHeaders headers, string url);

        Task<AceClientResponse<R>> ExecuteGetGenericAsync<R>(AceClientRequestHeaders headers, string url);

        AceClientResponse<R> ExecutePostGeneric<T, R>(AceClientRequest<T> myNbgRequest,string url);

        AceClientResponse<R> ExecutePostGenericAsString<T, R>(AceClientRequest<T> myNbgRequest, string url);


        Task<AceClientResponse<R>> ExecutePostGenericAsync<T, R>(AceClientRequest<T> myNbgRequest, string url);

        Task<AceClientResponse<R>> ExecutePostGenericAsStringAsync<T, R>(AceClientRequest<T> myNbgRequest, string url);

        AceClientResponse<R> ExecutePutGeneric<T, R>(AceClientRequest<T> myNbgRequest, string url);

        AceClientResponse<R> ExecutePutGenericAsString<T, R>(AceClientRequest<T> myNbgRequest, string url);

        Task<AceClientResponse<R>> ExecutePutGenericAsync<T, R>(AceClientRequest<T> myNbgRequest, string url);

        Task<AceClientResponse<R>> ExecutePutGenericAsStringAsync<T, R>(AceClientRequest<T> myNbgRequest, string url);

        AceClientResponseWithControl<R> ExecutePostWithControl<T, R>(AceClientRequestWithControl<T> myNbgRequest, string url);

        Task<AceClientResponseWithControl<R>> ExecutePostWithControlAsync<T,R>(AceClientRequestWithControl<T> myNbgRequest, string url);
        
    }
}
