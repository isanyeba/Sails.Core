using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Utils
{
    public class HttpMockerBase : IDisposable
    {
        HttpClientHandler handler;
        CookieContainer cookies;
        HttpClient httpClient;
        public HttpMockerBase(int timeout = 30)
        {
            cookies = new CookieContainer();
            handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
            handler.AllowAutoRedirect = true;
            handler.UseCookies = true;
            handler.CookieContainer = cookies;
            httpClient = new HttpClient(handler);

            httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            httpClient.Timeout = new TimeSpan(0, 0, timeout);
        }

        public void HeadersAdd(string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(name)|| !string.IsNullOrWhiteSpace(value))
            {
                httpClient.DefaultRequestHeaders.Add(name, value);
            }
        }

        public void HeadersRemove(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                httpClient.DefaultRequestHeaders.Remove(name);
            }
        }

        #region 基础

        public string Get(string url)
        {
            return GetAsync(url).Result;
        }

        public byte[] GetBytes(string url)
        {
            return GetBytesAsync(url).Result;
        }


        public async Task<string> GetAsync(string url)
        {

            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await httpClient.GetStringAsync(url).ConfigureAwait(false);
        }

        public async Task<byte[]> GetBytesAsync(string url)
        {
            return await httpClient.GetByteArrayAsync(url).ConfigureAwait(false);
        }

        public SailsResponse Post(string url, Dictionary<string, string> forms = null, string referer = null, bool loging = false)
        {
            return PostAsync(url, forms, referer, loging).Result;
        }

        public SailsResponse PostJ(string url, string jsonParams = null, string referer = null, bool loging = false)
        {
            return PostJAsync(url, jsonParams, referer, loging).Result;
        }
        public async Task<SailsResponse> PostAsync(string url, Dictionary<string, string> forms = null, string referer = null, bool loging = false)
        {
            SailsResponse httpResp = new SailsResponse();
            httpResp.ResponseUri = url;
            FormUrlEncodedContent content = null;
            if (forms != null && forms.Count > 0)
            {
                content = new FormUrlEncodedContent(forms);
            }
            if (referer != null)
            {
                httpClient.DefaultRequestHeaders.Referrer = new Uri(referer);
            }
            var httpRsp = await httpClient.PostAsync(url, content).ConfigureAwait(false);

            if (httpRsp != null)
            {
                if (loging)
                {
                    cookies.Add(handler.CookieContainer.GetCookies(new Uri(url)));
                }
                httpResp.ResponseUri = httpRsp.RequestMessage.RequestUri.ToString();
                httpResp.Content = httpRsp.Content.ReadAsStringAsync().Result;
                httpResp.StatusCode = httpRsp.StatusCode;
            }
            return httpResp;
        }

        public async Task<SailsResponse> PostJAsync(string url, string jsonParams = null, string referer = null, bool loging = false)
        {
            SailsResponse httpResp = new SailsResponse();
            httpResp.ResponseUri = url;
            HttpContent content = null;

            if (!string.IsNullOrWhiteSpace(jsonParams))
            {
                content = new StringContent(jsonParams);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            if (referer != null)
            {
                httpClient.DefaultRequestHeaders.Referrer = new Uri(referer);
            }
            var httpRsp = await httpClient.PostAsync(url, content).ConfigureAwait(false);

            if (httpRsp != null)
            {
                if (loging)
                {
                    cookies.Add(handler.CookieContainer.GetCookies(new Uri(url)));
                }
                httpResp.ResponseUri = httpRsp.RequestMessage.RequestUri.ToString();
                httpResp.Content = httpRsp.Content.ReadAsStringAsync().Result;
                httpResp.StatusCode = httpRsp.StatusCode;
            }
            return httpResp;
        }


        public SailsResponse PostBytes(string url, Dictionary<string, string> forms = null, string referer = null, bool loging = false)
        {
            return PostBytesAsync(url, forms, referer, loging).Result;
        }

        public async Task<SailsResponse> PostBytesAsync(string url, Dictionary<string, string> forms = null, string referer = null, bool loging = false)
        {
            SailsResponse httpResp = new SailsResponse();
            httpResp.ResponseUri = url;
            FormUrlEncodedContent content = null;
            if (forms != null && forms.Count > 0)
            {
                content = new FormUrlEncodedContent(forms);
            }
            if (referer != null)
            {
                httpClient.DefaultRequestHeaders.Referrer = new Uri(referer);
            }
            var httpRsp = await httpClient.PostAsync(url, content).ConfigureAwait(false);

            if (httpRsp != null)
            {
                if (loging)
                {
                    cookies.Add(handler.CookieContainer.GetCookies(new Uri(url)));
                }
                httpResp.ResponseUri = httpRsp.RequestMessage.RequestUri.ToString();
                httpResp.ContentBytes = httpRsp.Content.ReadAsByteArrayAsync().Result;
                httpResp.StatusCode = httpRsp.StatusCode;
            }
            return httpResp;
        }

        #endregion

        #region 析构

        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (httpClient != null)
                    {
                        httpClient.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
