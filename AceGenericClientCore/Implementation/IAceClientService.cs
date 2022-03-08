using AceGenericClientFramework.Types;

namespace AceGenericClientFramework
{
    public interface IAceClientService
    {
        AceClientResponse<R> ExecuteGetGeneric<T, R>(AceClientRequest<T> myNbgRequest);

        AceClientResponse<R> ExecutePostGeneric<T, R>(AceClientRequest<T> myNbgRequest);

        AceClientResponseWithControl<R> ExecutePostWithControl<R, T>(AceClientRequestWithControl<T> myNbgRequest);

        AceClientResponse<R> ExecutePutGeneric<T, R>(AceClientRequest<T> myNbgRequest);
    }
}
