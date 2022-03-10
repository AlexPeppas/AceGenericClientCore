using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("UserId", headers.UserId);
            if (string.IsNullOrEmpty(headers.RequestUUID))
                headers.RequestUUID = Guid.NewGuid().ToString();
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("RequestUUID", headers.RequestUUID);
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("GlobalUUID", headers.GlobalUUID ?? headers.RequestUUID);
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("Lang", headers.Lang.ToString() ?? Enums.AceClientLang.GRE.ToString());
            AceClient.DefaultRequestHeaders.TryAddWithoutValidation("SecurityToken", headers.SecurityToken.ToString());
        }

        public AceClientResponse<R> ExecuteGetGeneric<T, R>(AceClientRequest<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
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
                            AceError = default(CbsErrorData)
                        };
                    }
                    else
                    {
                        var errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse
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

        public AceClientResponse<R> ExecutePostGeneric<T, R>(AceClientRequest<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
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
                            AceError = default(CbsErrorData)
                        };
                    }
                    else
                    {
                        var errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse
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
                            AceError = default(CbsErrorData)
                        };
                    }
                    else
                    {
                        var errorDataResponse = aceResponse.Content.ReadAsAsync<CbsErrorData>().Result;

                        return new AceClientResponse<R>
                        {
                            AceResponse = default(R),
                            AceError = errorDataResponse
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AceClientResponseWithControl<R> ExecutePostWithControl<R, T>(AceClientRequestWithControl<T> myNbgRequest)
        {
            string token = string.Empty;
            try
            {
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
                                AceResponse = result.Payload
                            };
                        }

                        var outputInfoMessages = ModelConverter.ValidationMessageToOutputInfoMessage(result.ValidationControlsRespo?.ValidationsRequired);

                        return new AceClientResponseWithControl<R>
                        {
                            AceResponse = result.Payload,
                            AceExceptionMessages = outputInfoMessages.Item1,
                            AceInformationMessages = outputInfoMessages.Item2
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
