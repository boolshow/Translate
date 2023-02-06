using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Translate
{
    internal class GoogleTranslation
    {
        GetAppsettingJson getAppsettingJson = new();
        public GoogleTranslation()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
        }
        /// <summary>
        /// 谷歌翻译
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="to">目标语言</param>
        /// <returns></returns>
        public async Task<string> GetGoogleTranslationAsync(string text, string to = "zh-CN")
        {
            string tk = await GetTkAsync(text);
            tk = JsonConvert.DeserializeObject<string>(tk);
            string url = $"{getAppsettingJson.GetKey("GugeTrancn")}&tl={to}&tk={tk}";
            var FormData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("q", text),
                };
            using HttpClient httpClient = new();
            HttpContent content = new FormUrlEncodedContent(FormData);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            httpClient.DefaultRequestHeaders.Add("user-agent", getAppsettingJson.GetuseAge());
            HttpResponseMessage res = await httpClient.PostAsync(url, content);
            res.EnsureSuccessStatusCode();
            string responseBody = await res.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<dynamic>(responseBody);
            string msg = jsonObj[0]?[0]?.ToString();
            return msg;
        }
        /// <summary>
        /// 获取谷歌翻译tkk
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        private async Task<string> GetTkAsync(string parms)
        {
            try
            {
                string url = getAppsettingJson.GetKey("signapi") + "gettk";
                using HttpClient httpClient = new();
                Paramete paramete = new(parms);
                string strJson = JsonConvert.SerializeObject(paramete);
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var HttpResponseMessage = await httpClient.PostAsync(url, content);
                HttpResponseMessage.EnsureSuccessStatusCode();
                string responseBody = await HttpResponseMessage.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
