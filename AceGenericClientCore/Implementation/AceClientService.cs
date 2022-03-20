using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AceGenericClientFramework.JWTMechanism;
using AceGenericClientFramework.Model;
using AceGenericClientFramework.Types;

namespace AceGenericClientFramework
{
    public class AceClientService : IAceClientService
    {
        private static HttpClient AceClient { get; set; }

        private static string _baseUrl;
        private static string _bankId = "011";
        private static string _channelCode = "MYNBG";

        public AceClientService(string BaseUrl, string BankId, string ChannelCode)
        {
            _baseUrl = BaseUrl;
            _bankId = BankId ?? _bankId;
            _channelCode = ChannelCode ?? _channelCode;
            InitializeClient();
        }

        private void InitializeClient()
        {
            AceClient = new HttpClient();
            AceClient.DefaultRequestHeaders.Accept.Clear();
            AceClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("ChannelCode", _channelCode);
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("BankId", _bankId);
        }

        private void AddClientHeaders(AceClientRequestHeaders headers)
        {
            if (headers?.MockMode != null)
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("mockMode", headers.MockMode);
            if (headers?.SandboxId != null)
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("Sandbox_id", headers.SandboxId);
            if (headers?.UserName != null)
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("UserName", headers.UserName);
            if (headers?.UserId != null)
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("UserId", headers.UserId);
            if (string.IsNullOrEmpty(headers.RequestUUID))
                headers.RequestUUID = Guid.NewGuid().ToString();
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("RequestUUID", headers?.RequestUUID);
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("GlobalUUID", headers?.GlobalUUID ?? headers.RequestUUID);
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("Lang", headers?.Lang.ToString() ?? Enums.AceClientLang.GRE.ToString());
            if (!string.IsNullOrEmpty(headers?.SecurityToken))
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("SecurityToken", headers.SecurityToken.ToString());
            if (!string.IsNullOrEmpty(headers?.WorkstationId))
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("WorkstationId", headers.WorkstationId);
            if (!string.IsNullOrEmpty(headers?.BranchId))
                AceClient.DefaultRequestHeaders.TryAddWithoutValidation("BranchId", headers.BranchId);
        }

        public async Task<AceClientResponse<R>> ExecuteGetGenericAsync<T, R>(AceClientRequest<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.UserId != null)
                    token = JWT.RetrieveJWT(myNbgRequest.UserId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            myNbgRequest.SecurityToken = token;

            AddClientHeaders(myNbgRequest);

            BuildQueries(myNbgRequest);

            try
            {
                using (HttpResponseMessage aceResponse = await AceClient.GetAsync(_baseUrl))
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = await aceResponse.Content.ReadAsAsync<R>();
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                    else
                    {
                        var errorDataResponse = await aceResponse.Content.ReadAsAsync<CbsErrorData>();

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AceClientResponse<R> ExecuteGetGeneric<T, R>(AceClientRequest<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.UserId != null)
                    token = JWT.RetrieveJWT(myNbgRequest.UserId);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            myNbgRequest.SecurityToken = token;

            AddClientHeaders(myNbgRequest);

            BuildQueries(myNbgRequest);

            try
            {
                using (HttpResponseMessage aceResponse = AceClient.GetAsync(_baseUrl).Result)
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = aceResponse.Content.ReadAsAsync<R>().Result;
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                    else
                    {
                        var errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void BuildQueries<T> (AceClientRequest<T> myNbgRequest)
        {
            var properties = myNbgRequest.AceRequest.GetType().GetProperties();
            if (properties.Count() > 0)
            {
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                foreach (var prop in properties)
                {
                    var objValue = prop.GetValue(myNbgRequest.AceRequest, null);
                    if (objValue != null)
                    {
                        string value = objValue.ToString();
                        if (!string.IsNullOrEmpty(value))
                            query.Add(prop.Name.ToString(), value);
                    }
                }

                string queryString = query.ToString();
                _baseUrl += $"?{queryString}";
            }
        }

        public async Task<AceClientResponse<R>> ExecutePostGenericAsync<T, R>(AceClientRequest<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.UserId != null)
                    token = JWT.RetrieveJWT(myNbgRequest.UserId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            myNbgRequest.SecurityToken = token;

            AddClientHeaders(myNbgRequest);

            try
            {
                using (HttpResponseMessage aceResponse = await AceClient.PostAsJsonAsync<object>(_baseUrl, myNbgRequest.AceRequest))
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = await aceResponse.Content.ReadAsAsync<R>();
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                    else
                    {
                        var errorDataResponse = await aceResponse.Content.ReadAsAsync<CbsErrorData>();

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AceClientResponse<R> ExecutePostGeneric<T, R>(AceClientRequest<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.UserId != null)
                    token = JWT.RetrieveJWT(myNbgRequest.UserId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            myNbgRequest.SecurityToken = token;

            AddClientHeaders(myNbgRequest);

            try
            {
                using (HttpResponseMessage aceResponse = AceClient.PostAsJsonAsync<object>(_baseUrl, myNbgRequest.AceRequest).Result)
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = aceResponse.Content.ReadAsAsync<R>().Result;
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                    else
                    {
                        var errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AceClientResponse<R>> ExecutePutGenericAsync<T, R>(AceClientRequest<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.UserId != null)
                    token = JWT.RetrieveJWT(myNbgRequest.UserId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            myNbgRequest.SecurityToken = token;

            AddClientHeaders(myNbgRequest);

            try
            {
                using (HttpResponseMessage aceResponse = await AceClient.PutAsJsonAsync<object>(_baseUrl, myNbgRequest.AceRequest))
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = await aceResponse.Content.ReadAsAsync<R>();
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                    else
                    {
                        var errorDataResponse = await aceResponse.Content.ReadAsAsync<CbsErrorData>();

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AceClientResponse<R> ExecutePutGeneric<T, R>(AceClientRequest<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.UserId != null)
                    token = JWT.RetrieveJWT(myNbgRequest.UserId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            myNbgRequest.SecurityToken = token;

            AddClientHeaders(myNbgRequest);

            try
            {
                using (HttpResponseMessage aceResponse = AceClient.PutAsJsonAsync<object>(_baseUrl, myNbgRequest.AceRequest).Result)
                {
                    if (aceResponse.IsSuccessStatusCode)
                    {
                        R result = aceResponse.Content.ReadAsAsync<R>().Result;
                        return new AceClientResponse<R>
                        {
                            AceResponse = result,
                            AceError = default(CbsErrorData),
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                    else
                    {
                        var errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse,
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AceClientResponseWithControl<R>> ExecutePostWithControlAsync<T,R>(AceClientRequestWithControl<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.UserId != null)
                    token = JWT.RetrieveJWT(myNbgRequest.UserId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            myNbgRequest.SecurityToken = token;

            AddClientHeaders(myNbgRequest);

            var response = await ExecutePostWithControlHelperStringManipulationAsync<R, T>(myNbgRequest);

            return response;
        }

        public AceClientResponseWithControl<R> ExecutePostWithControl<T,R>(AceClientRequestWithControl<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
                if (myNbgRequest.UserId != null)
                    token = JWT.RetrieveJWT(myNbgRequest.UserId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            myNbgRequest.SecurityToken = token;

            AddClientHeaders(myNbgRequest);

            var response = ExecutePostWithControlHelperStringManipulation<R, T>(myNbgRequest);

            return response;
        }

        /// <summary>
        /// Perform string manipulation to recompose the response asynchronously
        /// </summary>
        private async Task<AceClientResponseWithControl<R>> ExecutePostWithControlHelperStringManipulationAsync<R, T>(AceClientRequestWithControl<T> myNbgRequest)
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
                using (HttpResponseMessage aceResponse = await AceClient.PostAsJsonAsync<object>(_baseUrl, wrapperAceRequest))
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
                                AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                            };
                        }

                        var outputInfoMessages = ModelConverter.ValidationMessageToOutputInfoMessage(result.ValidationControlsRespo?.ValidationsRequired);

                        return new AceClientResponseWithControl<R>
                        {
                            AceResponse = result.Payload,
                            AceExceptionMessages = outputInfoMessages.Item1,
                            AceInformationMessages = outputInfoMessages.Item2,
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                    else
                    {
                        var errorResponse = await aceResponse.Content.ReadAsAsync<CbsErrorData>();

                        return new AceClientResponseWithControl<R>
                        {
                            AceError = errorResponse
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
        private AceClientResponseWithControl<R> ExecutePostWithControlHelperStringManipulation<R, T>(AceClientRequestWithControl<T> myNbgRequest)
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
                using (HttpResponseMessage aceResponse = AceClient.PostAsJsonAsync<object>(_baseUrl, wrapperAceRequest).Result)
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
                                AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                            };
                        }

                        var outputInfoMessages = ModelConverter.ValidationMessageToOutputInfoMessage(result.ValidationControlsRespo?.ValidationsRequired);

                        return new AceClientResponseWithControl<R>
                        {
                            AceResponse = result.Payload,
                            AceExceptionMessages = outputInfoMessages.Item1,
                            AceInformationMessages = outputInfoMessages.Item2,
                            AceHttpStatusCode = Convert.ToString(aceResponse.StatusCode)
                        };
                    }
                    else
                    {
                        var errorResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;

                        return new AceClientResponseWithControl<R>
                        {
                            AceError = errorResponse
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
