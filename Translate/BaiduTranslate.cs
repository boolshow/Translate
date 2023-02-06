using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Translate
{
    internal class BaiduTranslate
    {
        GetAppsettingJson getAppsettingJson = new();
        public BaiduTranslate()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
        }
        /// <summary>
        /// 检测当前语种
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<string> GetlangdetectAsync(string query)
        {
            try
            {
                CookieContainer cookieContainer = new();
                string url = $"{getAppsettingJson.GetKey("BaiduTrancn")}langdetect";
                var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = true, UseCookies = true };
                using HttpClient httpClient = new(handler);
                httpClient.DefaultRequestHeaders.Add("user-agent", getAppsettingJson.GetuseAge());
                var FormData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("query", query),
                    };
                HttpContent postContent = new FormUrlEncodedContent(FormData);
                postContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var HttpResponseMessage = await httpClient.PostAsync(url, postContent);
                string responseBody = await HttpResponseMessage.Content.ReadAsStringAsync();
                var jsonObj = JsonConvert.DeserializeObject<dynamic>(responseBody);
                string lang = jsonObj["lan"];
                return lang;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 百度翻译
        /// </summary>
        /// <param name="information">文本</param>
        /// <param name="from">当前语言</param>
        /// <param name="to">目标语言</param>
        /// <returns></returns>
        public async Task<string> GetBaiduTranslateAsync(string information, string from = "", string to = "zh")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(from))
                    from = await GetlangdetectAsync(from);
                string asd = getAppsettingJson.GetUTCStartMilliseconds().ToString();
                string ua = getAppsettingJson.GetuseAge();
                BaiduToken header = await GetTokenAsync();
                Browserheader browserheader = new()
                {
                    url = $"{getAppsettingJson.GetKey("BaiduTrancn")}#" + from + "/" + to + "/" + HttpUtility.UrlEncode(information),
                    ua = ua,
                    clientTs = getAppsettingJson.GetUTCStartMilliseconds().ToString()
                };
                string edc = JsonConvert.SerializeObject(browserheader);
                string asetoken = getAppsettingJson.GetUTCStartMilliseconds().ToString() + "_" + getAppsettingJson.GetUTCStartMilliseconds().ToString() + "_" + EncryptAES(edc, "wgqaaeaaugaoomms", "1234567887654321");
                string sign = await GetsignAsync(information);
                sign = JsonConvert.DeserializeObject<string>(sign);
                string res = await GetResultAsync(asetoken, from, to, information, sign, header.Token, header.cookieContainer);
                var jsonObj = JsonConvert.DeserializeObject<dynamic>(res);
                string msg = jsonObj["trans_result"]?["data"]?[0]?["dst"].ToString();
                return msg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 进行翻译(百度)
        /// </summary>
        /// <param name="asetoken"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="information"></param>
        /// <param name="sign"></param>
        /// <param name="token"></param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        private async Task<string> GetResultAsync(string asetoken, string from, string to, string information, string sign, string token, CookieContainer cookieContainer)
        {
            try
            {
                HttpClientHandler handler = new()
                {
                    CookieContainer = cookieContainer,
                    AllowAutoRedirect = true,
                    UseCookies = true
                };
                using HttpClient httpClient = new(handler);
                string url = $"{getAppsettingJson.GetKey("BaiduTrancn")}v2transapi?from={from}&to={to}";
                httpClient.DefaultRequestHeaders.Add("user-agent", getAppsettingJson.GetuseAge());
                httpClient.DefaultRequestHeaders.Add("acs-token", asetoken);
                var FormData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("from", from),
                        new KeyValuePair<string, string>("query", information),
                        new KeyValuePair<string, string>("simple_means_flag", "3"),
                        new KeyValuePair<string, string>("sign", sign),
                        new KeyValuePair<string, string>("token", token),
                        new KeyValuePair<string, string>("domain","common")
                    };
                HttpContent postContent = new FormUrlEncodedContent(FormData);
                postContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var HttpResponseMessage = await httpClient.PostAsync(url, postContent);
                HttpResponseMessage.EnsureSuccessStatusCode();
                string responseBody = await HttpResponseMessage.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="passPhrase"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private string EncryptAES(string plainText, string passPhrase, string iv)
        {

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var keyBytes = Encoding.UTF8.GetBytes(passPhrase);
            var ivBytes = Encoding.UTF8.GetBytes(iv);
            using var symmetricKey = Aes.Create();
            symmetricKey.Mode = CipherMode.CBC;
            symmetricKey.Padding = PaddingMode.PKCS7;
            using var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivBytes);
            // 加密后的输出流
            using var memoryStream = new MemoryStream();
            // 将加密后的目标流（encryptStream）与加密转换（encryptTransform）相连接
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            // 将一个字节序列写入当前 CryptoStream （完成加密的过程）
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = memoryStream.ToArray();
            return Convert.ToBase64String(cipherTextBytes);
        }
        /// <summary>
        /// 获取cookie和token
        /// </summary>
        /// <returns></returns>
        private async Task<BaiduToken> GetTokenAsync()
        {
            try
            {
                BaiduToken header = new();
                string url = $"{getAppsettingJson.GetKey("BaiduTrancn")}translate";
                CookieContainer cookieContainer = new();
                var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = true, UseCookies = true };
                using HttpClient httpClient = new(handler);
                var HttpResponseMessage = await httpClient.GetAsync(url);
                HttpResponseMessage = await httpClient.GetAsync(url);
                string responseBody = await HttpResponseMessage.Content.ReadAsStringAsync();
                Match ml = Regex.Match(responseBody, "(?<=(token: ')).*?(?=(',))");
                string token = ml?.ToString();
                header.Token = token;
                header.cookieContainer = cookieContainer;
                return header;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取sign
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        private async Task<string> GetsignAsync(string parms)
        {
            try
            {
                string url = getAppsettingJson.GetKey("signapi") + "regSign";
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
