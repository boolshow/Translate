using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Translate
{
    /// <summary>
    /// 此处提供两种方式 一种是内置浏览器 一种是网址
    /// </summary>
    internal class MicrosoftTranslate
    {
        GetAppsettingJson getAppsettingJson = new();

        public MicrosoftTranslate()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
        }

        #region bing
        /// <summary>
        /// bing翻译
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fromLang"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public async Task<string> GetBingTranslateAsync(string text, string fromLang = "auto-detect", string to = "zh-Hans")
        {
            try
            {
                var heade = await GetHeadeAsync();
                string url = $"{getAppsettingJson.GetKey("BingTrancn")}ttranslatev3?isVertical=1&&IG={heade.IG}&IID=translator.5024.1";
                var FormData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("fromLang", fromLang),
                    new KeyValuePair<string, string>("text", text),
                    new KeyValuePair<string, string>("to", to),
                    new KeyValuePair<string, string>("token", heade.Token),
                    new KeyValuePair<string, string>("key", heade.Key)
                };
                using HttpClient httpClient = new();
                HttpContent content = new FormUrlEncodedContent(FormData);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                httpClient.DefaultRequestHeaders.Add("user-agent", getAppsettingJson.GetuseAge());
                HttpResponseMessage res = await httpClient.PostAsync(url, content);
                res.EnsureSuccessStatusCode();
                string responseBody = await res.Content.ReadAsStringAsync();
                var jsonObj = JsonConvert.DeserializeObject<dynamic>(responseBody);
                string msg = jsonObj?[0]?["translations"]?[0]?["text"]?.ToString();
                return msg;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 获取令牌token啥的
        /// </summary>
        /// <returns></returns>
        private async Task<Heade> GetHeadeAsync()
        {
            try
            {
                Heade heade = new();
                string url = getAppsettingJson.GetKey("BingTrancn") + "translator";
                string responseBody = string.Empty;
                using (HttpClient httpClient = new())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30000);
                    httpClient.DefaultRequestHeaders.Add("user-agent", getAppsettingJson.GetuseAge());
                    var HttpResponseMessage = await httpClient.GetAsync(url);
                    HttpResponseMessage.EnsureSuccessStatusCode();
                    responseBody = await HttpResponseMessage.Content.ReadAsStringAsync();
                }
                Match ig = Regex.Match(responseBody, "(?<=(IG:\")).*?(?=(\",))");
                Match ml = Regex.Match(responseBody, "(?<=(var params_AbusePreventionHelper =)) .*?(?=(;))");
                if (ml != null)
                {
                    string[] token = JsonConvert.DeserializeObject<string[]>(ml.ToString());
                    heade.Key = token[0];
                    heade.Token = token[1];
                    heade.IG = ig.ToString();
                    return heade;
                }
                else
                    throw new Exception("Token acquisition failed!");
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Microsoft
        public async Task<string> GetMicrosoftTranslateAsync(string text, string fromLang = "", string to = "zh-CHS")
        {
            try
            {
                string token = "Bearer " + await GetTokenAsync();
                string url = $"{getAppsettingJson.GetKey("apicognitive")}&from={fromLang}&to={to}";
                List<Edgbody> body = new()
                {
                    new Edgbody { Text = text }
                };
                string strJson = JsonConvert.SerializeObject(body);
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                using HttpClient httpClient = new();
                httpClient.DefaultRequestHeaders.Add("authorization", token);
                httpClient.DefaultRequestHeaders.Add("user-agent", getAppsettingJson.GetuseAge());
                HttpResponseMessage res = await httpClient.PostAsync(url, content);
                res.EnsureSuccessStatusCode();
                string responseBody = await res.Content.ReadAsStringAsync();
                var jsonObj = JsonConvert.DeserializeObject<dynamic>(responseBody);
                string msg = jsonObj?[0]?["translations"]?[0]?["text"]?.ToString();
                return msg;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取认证令牌
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetTokenAsync()
        {
            try
            {
                string url = getAppsettingJson.GetKey("edgeapi");
                string responseBody = string.Empty;
                using HttpClient httpClient = new();
                httpClient.Timeout = TimeSpan.FromSeconds(30000);
                httpClient.DefaultRequestHeaders.Add("user-agent", getAppsettingJson.GetuseAge());
                var HttpResponseMessage = await httpClient.GetAsync(url);
                HttpResponseMessage.EnsureSuccessStatusCode();
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
