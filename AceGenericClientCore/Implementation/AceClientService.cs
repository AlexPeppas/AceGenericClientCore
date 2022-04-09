using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Nbg.NetCore.Services.Ace.Http.JWTMechanism;
using Nbg.NetCore.Services.Ace.Http.Model;
using Nbg.NetCore.Services.Ace.Http.Types;

namespace Nbg.NetCore.Services.Ace.Http
{
    public class AceClientService : IAceClientService
    {
        private static HttpClient AceClient { get; set; }

        private static string _baseUrl;
        private static string _bankId = "011";
        private static string _channelCode = "MYNBG";

        public AceClientService(string BaseUrl, string BankId, string ChannelCode)
        {
            AceClient = new HttpClient();
            _baseUrl = BaseUrl;
            _bankId = BankId ?? _bankId;
            _channelCode = ChannelCode ?? _channelCode;
        }

        private void AddClientHeaders(Dictionary<string,string> clientHeaders,string JWT)
        {
            AceClient.DefaultRequestHeaders.Clear();
            AceClient.DefaultRequestHeaders.Accept.Clear();
            AceClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("ChannelCode", _channelCode);
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("BankId", _bankId);

            foreach (var header in clientHeaders)
            {
                if (header.Value != null)
                    AceClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (!clientHeaders.ContainsKey("RequestUUID"))
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("RequestUUID", Guid.NewGuid().ToString());
            if (!clientHeaders.ContainsKey("GlobalUUID"))
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("GlobalUUID", clientHeaders["RequestUUID"]);
            if (!clientHeaders.ContainsKey("Lang"))
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("Lang", Enums.AceClientLang.GRE.ToString());
            if (!string.IsNullOrEmpty(JWT) || JWT!=string.Empty)
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("SecurityToken", JWT);
        }

        public async Task<AceClientResponse<R>> ExecuteGetGenericAsync<T, R>(AceClientRequest<T> myNbgRequest,string url)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.Headers.ContainsKey("UserId"))
                    token = JWT.RetrieveJWT(myNbgRequest.Headers["UserId"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            AddClientHeaders(myNbgRequest.Headers,token);
            
            try
            {
                url = "?"+BuildQueries(myNbgRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to build query params from request object {Environment.NewLine} {ex.ToString()}");
            }

            try
            {
                using (HttpResponseMessage aceResponse = await AceClient.GetAsync(_baseUrl+url))
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = await aceResponse.Content.ReadAsAsync<R>();
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                    else
                    {
                        CbsErrorData errorDataResponse;

                        if (aceResponse.Content.Headers.ContentType?.MediaType == "application/json")
                            errorDataResponse = await aceResponse.Content.ReadAsAsync<CbsErrorData>();
                        else
                            errorDataResponse = default(CbsErrorData);

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
      
        public async Task<AceClientResponse<R>> ExecuteGetGenericAsync<R>(AceClientRequestHeaders headers,string url)
        {
            string token = string.Empty;
            try
            {
                if (headers.Headers.ContainsKey("UserId"))
                    token = JWT.RetrieveJWT(headers.Headers["UserId"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            AddClientHeaders(headers.Headers,token);

            try
            {
                using (HttpResponseMessage aceResponse = await AceClient.GetAsync(_baseUrl+url))
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = await aceResponse.Content.ReadAsAsync<R>();
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                    else
                    {
                        CbsErrorData errorDataResponse;

                        if (aceResponse.Content.Headers.ContentType?.MediaType == "application/json")
                            errorDataResponse = await aceResponse.Content.ReadAsAsync<CbsErrorData>();
                        else
                            errorDataResponse = default(CbsErrorData);

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public AceClientResponse<R> ExecuteGetGeneric<R>(AceClientRequestHeaders headers,string url)
        {
            string token = string.Empty;
            try
            {
                if (headers.Headers.ContainsKey("UserId"))
                    token = JWT.RetrieveJWT(headers.Headers["UserId"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            AddClientHeaders(headers.Headers, token);

            try
            {
                using (HttpResponseMessage aceResponse = AceClient.GetAsync(_baseUrl+url).Result)
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = aceResponse.Content.ReadAsAsync<R>().Result;
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                    else
                    {
                        CbsErrorData errorDataResponse;

                        if (aceResponse.Content.Headers.ContentType?.MediaType == "application/json")
                            errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;
                        else
                            errorDataResponse = default(CbsErrorData);

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public AceClientResponse<R> ExecuteGetGeneric<T, R>(AceClientRequest<T> myNbgRequest, string url)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.Headers.ContainsKey("UserId"))
                    token = JWT.RetrieveJWT(myNbgRequest.Headers["UserId"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            AddClientHeaders(myNbgRequest.Headers, token);

            try
            {
                url = "?" + BuildQueries(myNbgRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to build query params from request object {Environment.NewLine} {ex.ToString()}");
            }
            
            try
            {
                using (HttpResponseMessage aceResponse = AceClient.GetAsync(_baseUrl+url).Result)
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = aceResponse.Content.ReadAsAsync<R>().Result;
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                    else
                    {
                        CbsErrorData errorDataResponse;

                        if (aceResponse.Content.Headers.ContentType?.MediaType == "application/json")
                            errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;
                        else
                            errorDataResponse = default(CbsErrorData);

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string BuildQueries<T> (AceClientRequest<T> myNbgRequest)
        {
            var properties = myNbgRequest?.AceRequest?.GetType()?.GetProperties();
            if (properties?.Count() > 0)
            {
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                foreach (var prop in properties)
                {
                    var dataMemberName = ((DataMemberAttribute)prop.GetCustomAttributes(typeof(DataMemberAttribute), true)?.FirstOrDefault())?.Name;
                    var propName = dataMemberName ?? prop.Name; //if dataMember is null
                    var objValue = prop.GetValue(myNbgRequest.AceRequest, null);
                    if (objValue != null)
                    {
                        var objType = objValue?.GetType();
                        string value = string.Empty;
                        if (objType == typeof(decimal)) //decimal replace , with .
                        {
                            value = ((decimal)objValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (objType == typeof(double)) //double replace , with .
                        {
                            value = ((double)objValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (objType == typeof(DateTime)) // ISO DateTime
                        {
                            value = ((DateTime)objValue).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fff");
                        }
                        else
                            value = objValue.ToString();

                        if (!string.IsNullOrEmpty(value))
                            query.Add(propName, value);
                    }
                }

                string queryString = query?.ToString();
                return queryString;
            }
            return string.Empty;
        }

        public async Task<AceClientResponse<R>> ExecutePostGenericAsync<T, R>(AceClientRequest<T> myNbgRequest,string url)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.Headers.ContainsKey("UserId"))
                    token = JWT.RetrieveJWT(myNbgRequest.Headers["UserId"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            AddClientHeaders(myNbgRequest.Headers, token);

            try
            {
                using (HttpResponseMessage aceResponse = await AceClient.PostAsJsonAsync<object>(_baseUrl+url, myNbgRequest.AceRequest))
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = await aceResponse.Content.ReadAsAsync<R>();
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                    else
                    {
                        CbsErrorData errorDataResponse;

                        if (aceResponse.Content.Headers.ContentType?.MediaType == "application/json")
                            errorDataResponse = await aceResponse.Content.ReadAsAsync<CbsErrorData>();
                        else
                            errorDataResponse = default(CbsErrorData);

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AceClientResponse<R> ExecutePostGeneric<T, R>(AceClientRequest<T> myNbgRequest,string url)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.Headers.ContainsKey("UserId"))
                    token = JWT.RetrieveJWT(myNbgRequest.Headers["UserId"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            AddClientHeaders(myNbgRequest.Headers, token);

            try
            {
                using (HttpResponseMessage aceResponse = AceClient.PostAsJsonAsync<object>(_baseUrl+url, myNbgRequest.AceRequest).Result)
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = aceResponse.Content.ReadAsAsync<R>().Result;
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                    else
                    {
                        CbsErrorData errorDataResponse;

                        if (aceResponse.Content.Headers.ContentType?.MediaType == "application/json")
                            errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;
                        else
                            errorDataResponse = default(CbsErrorData);

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AceClientResponse<R>> ExecutePutGenericAsync<T, R>(AceClientRequest<T> myNbgRequest,string url)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.Headers.ContainsKey("UserId"))
                    token = JWT.RetrieveJWT(myNbgRequest.Headers["UserId"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            AddClientHeaders(myNbgRequest.Headers, token);

            try
            {
                using (HttpResponseMessage aceResponse = await AceClient.PutAsJsonAsync<object>(_baseUrl+url, myNbgRequest.AceRequest))
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = await aceResponse.Content.ReadAsAsync<R>();
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                    else
                    {
                        CbsErrorData errorDataResponse;

                        if (aceResponse.Content.Headers.ContentType?.MediaType == "application/json")
                            errorDataResponse = await aceResponse.Content.ReadAsAsync<CbsErrorData>();
                        else
                            errorDataResponse = default(CbsErrorData);

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AceClientResponse<R> ExecutePutGeneric<T, R>(AceClientRequest<T> myNbgRequest,string url)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.Headers.ContainsKey("UserId"))
                    token = JWT.RetrieveJWT(myNbgRequest.Headers["UserId"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            AddClientHeaders(myNbgRequest.Headers, token);

            try
            {
                using (HttpResponseMessage aceResponse = AceClient.PutAsJsonAsync<object>(_baseUrl+url, myNbgRequest.AceRequest).Result)
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = aceResponse.Content.ReadAsAsync<R>().Result;
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                    else
                    {
                        CbsErrorData errorDataResponse;

                        if (aceResponse.Content.Headers.ContentType?.MediaType == "application/json")
                            errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;
                        else
                            errorDataResponse = default(CbsErrorData);

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AceClientResponseWithControl<R>> ExecutePostWithControlAsync<T,R>(AceClientRequestWithControl<T> myNbgRequest, string url)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.Headers.ContainsKey("UserId"))
                    token = JWT.RetrieveJWT(myNbgRequest.Headers["UserId"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            AddClientHeaders(myNbgRequest.Headers, token);

            var response = await ExecutePostWithControlHelperStringManipulationAsync<R, T>(myNbgRequest,url);

            return response;
        }

        public AceClientResponseWithControl<R> ExecutePostWithControl<T,R>(AceClientRequestWithControl<T> myNbgRequest,string url)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.Headers.ContainsKey("UserId"))
                    token = JWT.RetrieveJWT(myNbgRequest.Headers["UserId"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            AddClientHeaders(myNbgRequest.Headers, token);

            var response = ExecutePostWithControlHelperStringManipulation<R, T>(myNbgRequest,url);

            return response;
        }

        /// <summary>
        /// Perform string manipulation to recompose the response asynchronously
        /// </summary>
        private async Task<AceClientResponseWithControl<R>> ExecutePostWithControlHelperStringManipulationAsync<R, T>(AceClientRequestWithControl<T> myNbgRequest,string url)
        {
            var requestValidationMessages = ModelConverter.InputMessageToRequestValidationMessage(myNbgRequest.AceMessages);

            var validationControls = new RequestValidationControls
            {
                ExecuteTranIfValid = myNbgRequest.CanBeExecuted,
                ValidationsFullfiled = requestValidationMessages?.ToList()
            };

            //this is the wrapperRequest payload decompose statement
            var wrapperAceRequest = ModelConverter.BuildWrapperRequest<T>(myNbgRequest.AceRequest, validationControls);

            try
            {
                using (HttpResponseMessage aceResponse = await AceClient.PostAsJsonAsync<object>(_baseUrl+url, wrapperAceRequest))
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        var resultAce = await aceResponse.Content.ReadAsStringAsync();
                        var result = ModelConverter.BuildResponseStringManipulation<R>(resultAce);

                        if (result.ValidationControlsRespo.ExecuteTranIfValid && result.ValidationControlsRespo.AllValidationAreFullfiled)
                        {
                            return new AceClientResponseWithControl<R>
                            {
                                AceResponse = result.Payload,
                                AceHttpStatusCode = aceResponse.StatusCode
                            };
                        }

                        var outputInfoMessages = ModelConverter.ValidationMessageToOutputInfoMessage(result.ValidationControlsRespo?.ValidationsRequired);

                        return new AceClientResponseWithControl<R>
                        {
                            AceResponse = result.Payload,
                            AceExceptionMessages = outputInfoMessages.Item1,
                            AceInformationMessages = outputInfoMessages.Item2,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                    else
                    {
                        CbsErrorData errorDataResponse;

                        if (aceResponse.Content.Headers.ContentType?.MediaType == "application/json")
                            errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;
                        else
                            errorDataResponse = default(CbsErrorData);

                        return new AceClientResponseWithControl<R>
                        {
                            AceError = errorDataResponse,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Perform string manipulation to recompose the response 
        /// </summary>
        private AceClientResponseWithControl<R> ExecutePostWithControlHelperStringManipulation<R, T>(AceClientRequestWithControl<T> myNbgRequest,string url)
        {
            var requestValidationMessages = ModelConverter.InputMessageToRequestValidationMessage(myNbgRequest.AceMessages);
            
            var validationControls = new RequestValidationControls
            {
                ExecuteTranIfValid = myNbgRequest.CanBeExecuted,
                ValidationsFullfiled = requestValidationMessages?.ToList()
            };

            //this is the wrapperRequest payload decompose statement
            var wrapperAceRequest = ModelConverter.BuildWrapperRequest<T>(myNbgRequest.AceRequest, validationControls);

            try
            {
                using (HttpResponseMessage aceResponse = AceClient.PostAsJsonAsync<object>(_baseUrl+url, wrapperAceRequest).Result)
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        var resultAce = aceResponse.Content.ReadAsStringAsync().Result;
                        var result = ModelConverter.BuildResponseStringManipulation<R>(resultAce);

                        if (result.ValidationControlsRespo.ExecuteTranIfValid && result.ValidationControlsRespo.AllValidationAreFullfiled)
                        {
                            return new AceClientResponseWithControl<R>
                            {
                                AceResponse = result.Payload,
                                AceHttpStatusCode = aceResponse.StatusCode
                            };
                        }

                        var outputInfoMessages = ModelConverter.ValidationMessageToOutputInfoMessage(result.ValidationControlsRespo?.ValidationsRequired);

                        return new AceClientResponseWithControl<R>
                        {
                            AceResponse = result.Payload,
                            AceExceptionMessages = outputInfoMessages.Item1,
                            AceInformationMessages = outputInfoMessages.Item2,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                    else
                    {
                        CbsErrorData errorDataResponse;

                        if (aceResponse.Content.Headers.ContentType?.MediaType == "application/json")
                            errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;
                        else
                            errorDataResponse = default(CbsErrorData);

                        return new AceClientResponseWithControl<R>
                        {
                            AceError = errorDataResponse,
                            AceHttpStatusCode = aceResponse.StatusCode
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}
