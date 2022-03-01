﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using AceGenericClientCore.Properties.Settings;
using System.Threading.Tasks;
using AceGenericClientFramework.CacheMechanism;


namespace AceGenericClientFramework.JWTMechanism
{
    public static class JWT
    {
        public static string RetrieveJWT(string user)
        {
            string JWT = string.Empty;
            try
            {
                JWT = CacheHelper(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return JWT;
        }

        private static string CacheHelper(string user)
        {
            string cacheKey = Settings.ClientSettings.cacheKeyPrefix+user.ToString();

            var cacheValue = Cache.GetCached<CachedJWTValue>(cacheKey);

            if (cacheValue == null || string.IsNullOrEmpty(cacheValue?.access_token))
            {
                try
                {
                    cacheValue = JWTHelper(user).Result;
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to retrieve JWT,{Environment.NewLine}Details :: {ex.Message}");
                }
                Cache.UpdateCache<CachedJWTValue>(cacheKey, Convert.ToInt32(cacheValue.expires_in), cacheValue);
            }

            return cacheValue.access_token;
        }

        private static async Task<CachedJWTValue> JWTHelper(string userId)
        {
            var parameters = new Dictionary<string, string>
            {
                //{ "custom_credential_employee_code",Properties.Settings.ClientSettings.custom_credential_employee_code + $"\\{userId}"},
                { "custom_credential_employee_code",userId},
                { "client_id", Settings.ClientSettings.client_id},
                { "client_secret",Settings.ClientSettings.client_secret},
                { "grant_type", Settings.ClientSettings.grant_type},
                { "scope", Settings.ClientSettings.scope },
                { "user_registry_id", Settings.ClientSettings.user_registry_id}
            };

            string uri = Settings.ClientSettings.tokenUrl;
            string responseToken = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient { BaseAddress = new Uri(uri) })
                {
                    responseToken = await client.PostAsync(uri, new FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
                }
                
                if (responseToken.Contains("error")) throw new Exception(responseToken);
                
                return new CachedJWTValue
                {
                    access_token = Newtonsoft.Json.Linq.JObject.Parse(responseToken)["access_token"].ToString(),
                    expires_in = Newtonsoft.Json.Linq.JObject.Parse(responseToken)["expires_in"].ToString()
                };

            }
            catch (Exception ex) 
            { 
                throw new Exception(ex.Message); 
            }
        }
    }
}
