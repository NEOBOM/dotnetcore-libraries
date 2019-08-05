using Library.Framework.JsonUtil;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.HttpServices.Builders.Common
{
    public abstract class HttpClientBase
    {
        protected readonly HttpClient _httpClient;

        protected WebProxy _webProxy = null;
        protected CookieCollection _cookies = null;
        protected HttpContent _content = null;

        protected string _mediaTypeAccept = null;
        protected string _contentType = null;
        protected int _maxConnectionsPerServer = 1500;
        protected int _maxAutomaticRedirections = 3;
        protected string _bearerToken = null;
        protected string _browserUserAgent = null;
        protected TimeSpan _timeOut = TimeSpan.FromMilliseconds(3000);

        public HttpClientBase(Uri baseAddress) : this(baseAddress, "text/html")
        {
        }

        public HttpClientBase(Uri baseAddress, string mediaTypeAccept)
        {
            _mediaTypeAccept = mediaTypeAccept;
            _contentType = mediaTypeAccept;

            _httpClient = CreateHttpClienWithHandler(baseAddress);
        }

        public virtual HttpResponseMessage GetSync(string uri)
        {
            return GetAsync(uri).GetAwaiter().GetResult();
        }

        public virtual Task<HttpResponseMessage> GetAsync(string uri)
        {
            return _httpClient.GetAsync(uri);
        }

        public virtual string GetStringSync(string uri)
        {
            return GetStringAsync(uri).GetAwaiter().GetResult();
        }

        public virtual async Task<string> GetStringAsync(string uri)
        {
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public virtual HttpResponseMessage PostSync(string uri, string content)
        {
            return PostAsync(uri, content).GetAwaiter().GetResult();
        }

        public virtual Task<HttpResponseMessage> PostAsync(string uri, string content)
        {
            return _httpClient.PostAsync(uri, GetStringContent(content));
        }

        public virtual HttpResponseMessage PostSync(string uri, object content)
        {
            return PostAsync(uri, content).GetAwaiter().GetResult();
        }

        public virtual Task<HttpResponseMessage> PostAsync(string uri, object content)
        {
            return _httpClient.PostAsync(uri, GetStringContent(content));
        }

        public virtual HttpResponseMessage PutSync(string uri, object content)
        {
            return PutAsync(uri, content).GetAwaiter().GetResult();
        }

        public virtual Task<HttpResponseMessage> PutAsync(string uri, object content)
        {
            return _httpClient.PutAsync(uri, GetStringContent(content));
        }

        public virtual HttpResponseMessage DeleteSync(string uri, object content)
        {
            return DeleteAsync(uri).GetAwaiter().GetResult();
        }

        public virtual Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            return _httpClient.DeleteAsync(uri);
        }

        protected virtual Task<HttpResponseMessage> Send(string uri, object content, HttpMethod httpMethod)
        {
            return _httpClient.SendAsync(CreateRequestMessage(uri, content, httpMethod));
        }

        protected virtual Task<HttpResponseMessage> Send(string uri, object content, HttpMethod httpMethod, CancellationToken cancellationToken)
        {
            return _httpClient.SendAsync(CreateRequestMessage(uri, content, httpMethod), cancellationToken);
        }

        #region Private

        private HttpClient CreateHttpClienWithHandler(Uri baseAddress)
        {
            var http = new HttpClient(CreateHttpClientHandler(baseAddress))
            {
                BaseAddress = baseAddress,
                Timeout = _timeOut
            };

            http.DefaultRequestHeaders.TryAddWithoutValidation("Accept", _mediaTypeAccept);
            http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", _contentType);

            if (!string.IsNullOrEmpty(_browserUserAgent))
                http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", _browserUserAgent);

            if (!string.IsNullOrEmpty(_bearerToken))
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            return http;
        }

        private HttpClientHandler CreateHttpClientHandler(Uri baseAddress)
        {
            ServicePointManager.DefaultConnectionLimit = 15000;
            var handler = new HttpClientHandler();

            if (_webProxy != null)
            {
                handler.Proxy = _webProxy;
                handler.UseProxy = true;
                handler.PreAuthenticate = true;
                handler.UseDefaultCredentials = false;
            }

            if (_cookies != null)
            {
                handler.CookieContainer.Add(_cookies);
                handler.UseCookies = true;
            }

            if (baseAddress.AbsoluteUri.Contains("https"))
            {
                handler.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
                handler.ServerCertificateCustomValidationCallback = ValidateServerCertificate;
            }

            handler.AllowAutoRedirect = _maxAutomaticRedirections > 0;
            handler.MaxAutomaticRedirections = _maxAutomaticRedirections;
            handler.MaxConnectionsPerServer = _maxConnectionsPerServer;
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return handler;
        }

        private HttpRequestMessage CreateRequestMessage(string requestUri, object content, HttpMethod method)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(requestUri),
                Content = GetStringContent(content)
            };

            return request;
        }

        private HttpContent GetStringContent(object content)
        {
            return (content != null) ? new StringContent(JsonHelper.SerializeObject(content), Encoding.UTF8) : null;
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }

        #endregion
    }
}