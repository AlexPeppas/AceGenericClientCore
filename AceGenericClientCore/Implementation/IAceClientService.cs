using AceGenericClientFramework.Types;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public interface IAceClientService
    {
        AceClientResponse<R> ExecuteGetGeneric<T, R>(AceClientRequest<T> myNbgRequest,string url, bool ignoreBody);

        //AceClientResponse<R> ExecuteGetGeneric<R>(AceClientRequestHeaders headers);

        AceClientResponse<R> ExecutePostGeneric<T, R>(AceClientRequest<T> myNbgRequest,string url);

        AceClientResponseWithControl<R> ExecutePostWithControl<T,R>(AceClientRequestWithControl<T> myNbgRequest,string url);

        AceClientResponse<R> ExecutePutGeneric<T, R>(AceClientRequest<T> myNbgRequest, string url);

        Task<AceClientResponse<R>> ExecuteGetGenericAsync<T, R>(AceClientRequest<T> myNbgRequest, string url, bool ignoreBody);

        //Task<AceClientResponse<R>> ExecuteGetGenericAsync<R>(AceClientRequestHeaders headers);

        Task<AceClientResponse<R>> ExecutePostGenericAsync<T, R>(AceClientRequest<T> myNbgRequest, string url);

        Task<AceClientResponseWithControl<R>> ExecutePostWithControlAsync<T,R>(AceClientRequestWithControl<T> myNbgRequest, string url);

        Task<AceClientResponse<R>> ExecutePutGenericAsync<T, R>(AceClientRequest<T> myNbgRequest, string url);
    }
}
