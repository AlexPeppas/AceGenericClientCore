using AceGenericClientFramework.Types;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public interface IAceClientService
    {
        AceClientResponse<R> ExecuteGetGeneric<T, R>(AceClientRequest<T> myNbgRequest);

        AceClientResponse<R> ExecutePostGeneric<T, R>(AceClientRequest<T> myNbgRequest);

        AceClientResponseWithControl<R> ExecutePostWithControl<T,R>(AceClientRequestWithControl<T> myNbgRequest);

        AceClientResponse<R> ExecutePutGeneric<T, R>(AceClientRequest<T> myNbgRequest);

        Task<AceClientResponse<R>> ExecuteGetGenericAsync<T, R>(AceClientRequest<T> myNbgRequest);

        Task<AceClientResponse<R>> ExecutePostGenericAsync<T, R>(AceClientRequest<T> myNbgRequest);

        Task<AceClientResponseWithControl<R>> ExecutePostWithControlAsync<T,R>(AceClientRequestWithControl<T> myNbgRequest);

        Task<AceClientResponse<R>> ExecutePutGenericAsync<T, R>(AceClientRequest<T> myNbgRequest);
    }
}
