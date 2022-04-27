using Nbg.NetCore.Services.Ace.Http.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Nbg.NetCore.Services.Ace.Http.Model
{
    internal static class ModelConverter
    {
        public static Tuple<OutputMessage[], CbsInformationMessage[]> ValidationMessageToOutputInfoMessage(List<ResponseValidationMessage> request)
        {
            var outputMessage = new List<OutputMessage>();
            var cbsInfoMessage = new List<CbsInformationMessage>();

            foreach (var item in request)
            {
                if (item.ValidationType == "Exception")
                    outputMessage.Add(new OutputMessage
                    {
                        Code = item?.ValidationCode,
                        ActionGroups = item?.MinAuthLevel?.Split(',') ?? new string[0],
                        Description = item?.ValidationDescription,
                        RelatedEntityId = item?.RelatedEntityId,
                        RelatedEntityType = item?.RelatedEntityType,
                        SourceSystemCode = Enums.SourceSystemCode.ACE.ToString()
                    });
                else if (item.ValidationType == "Information")
                    cbsInfoMessage.Add(new CbsInformationMessage
                    {
                        Code = item?.ValidationCode,
                        Description = item?.ValidationDescription
                    });
            }
            return new Tuple<OutputMessage[], CbsInformationMessage[]>
                       (outputMessage.ToArray(), cbsInfoMessage.ToArray());
        }

        public static RequestValidationMessage[] InputMessageToRequestValidationMessage(InputMessage[] request)
        {
            var output = new RequestValidationMessage[request.GetLength(0)];
            for (int i = 0; i < request.GetLength(0); i++)
            {
                output[i] = new RequestValidationMessage();
                output[i].ValidationCode = request[i]?.Code;
                if (request[i]?.AuthUserActionGroups.Count > 0)
                    output[i].AuthLevel = string.Join(',', request[i]?.AuthUserActionGroups);
                else
                    output[i].AuthLevel = "";
                output[i].AuthUser = request[i]?.AuthUser;
                output[i].RelatedEntityId = request[i]?.RelatedEntityId;
                output[i].RelatedEntityType = request[i]?.RelatedEntityType;
                output[i].ValidationType = "Exception";
            }
            return output;
        }

        /// <summary>
        /// Decompose the request object and returns a new unified object
        /// </summary>
        public static object BuildWrapperRequest<T>(T request, RequestValidationControls controls)
        {
            dynamic runtimeObject = new System.Dynamic.ExpandoObject();
            var runtimeDict = runtimeObject as IDictionary<string, object>;

            foreach (var prop in request.GetType().GetProperties())
            {
                var dataMemberName = ((DataMemberAttribute)prop.GetCustomAttributes(typeof(DataMemberAttribute), true)?.FirstOrDefault())?.Name;
                var propName = dataMemberName ?? prop.Name; //if dataMember is null
                var objValue = prop.GetValue(request, null);
                if (objValue != null)
                {
                    runtimeDict.Add(propName, objValue);
                }
            }

            runtimeDict.Add("ValidationControlsRequest", controls);

            var serializedObject = JsonConvert.SerializeObject(runtimeDict);
            var runtimeJObject = Newtonsoft.Json.Linq.JObject.Parse(serializedObject);
            
            return runtimeJObject;
            
            
        }

        /// <summary>
        /// Recompose the flattened response into the {R payload, controls} format
        /// </summary>
        public static Tuple<object, object> BuildWrapperResponse(object aceResponse)
        {
            dynamic runtimeObject = new System.Dynamic.ExpandoObject();
            var runtimeDict = runtimeObject as IDictionary<string, object>;
            object validationControls = null;
            foreach (var prop in aceResponse.GetType().GetProperties())
            {
                if (prop.Name != "ValidationControlsRespo")
                    runtimeDict.Add(prop.Name, prop.GetValue(aceResponse));
                else
                    validationControls = prop.GetValue(aceResponse);
            }
            return new Tuple<object, object>(validationControls, runtimeObject);
        }

        public static WrapperAceResponse<R> BuildResponseStringManipulation<R>(string resultAce)
        {
            StringBuilder controlsRespoString = new StringBuilder();
            StringBuilder payloadString = new StringBuilder();

            R payloadObject = default(R);
            ResponseValidationControls controlsObject = default(ResponseValidationControls);
            
            if (resultAce.Contains("validationControlsResponse"))
            {
                payloadString.Append(resultAce.Substring(0, resultAce.IndexOf(",\"validationControlsResponse\"")));
                payloadString.Append("}");
                payloadObject = JsonConvert.DeserializeObject<R>(payloadString.ToString());
            }
            if (resultAce.Contains("{\"allValidationsAreFulfilled\""))
            {
                controlsRespoString.Append(resultAce.Substring(resultAce.IndexOf("{\"allValidationsAreFulfilled\"")));
                controlsRespoString.Remove(controlsRespoString.Length - 1, 1);
                controlsObject = JsonConvert.DeserializeObject<ResponseValidationControls>(controlsRespoString.ToString());
            }

            var result = new WrapperAceResponse<R>();
            result.Payload = payloadObject;
            result.ValidationControlsRespo = controlsObject;

            return result;
        }
    }
}
