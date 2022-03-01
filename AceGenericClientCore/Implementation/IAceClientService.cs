using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework
{
    public interface IAceClientService
    {
        IAceClientResponse<R> ExecuteGetGeneric<T, R>(IAceClientRequest<T> myNbgRequest);

        IAceClientResponse<R> ExecutePostGeneric<T, R>(IAceClientRequest<T> myNbgRequest);

        IAceClientResponseWithControl<R> ExecutePostWithControl<R, T>(IAceClientRequestWithControl<T> myNbgRequest);

        IAceClientResponse<R> ExecutePutGeneric<T, R>(IAceClientRequest<T> myNbgRequest);
    }
}
