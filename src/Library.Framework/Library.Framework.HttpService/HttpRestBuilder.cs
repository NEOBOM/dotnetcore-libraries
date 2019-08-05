using Library.HttpServices.Builders.Common;
using Library.HttpServices.Builders.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Library.HttpServices.Builders
{
    public class HttpRestBuilder : HttpClientBase, IHttpBuilder
    {
        public HttpRestBuilder(Uri baseAddress) : base(baseAddress, "application/json")
        {

        }

        public HttpRestBuilder AddBearerToken(string bearerToken)
        {
            _bearerToken = bearerToken;
            return this;
        }

        public HttpRestBuilder AddTimeout(TimeSpan timeout)
        {
            _timeOut = timeout;
            return this;
        }

        public HttpRestBuilder AddMaxConnectionsPerServer(int maxConnectionsPerServer)
        {
            _maxConnectionsPerServer = maxConnectionsPerServer;
            return this;
        }

        public HttpRestBuilder AddProxy(string uri, string userName = "", string password = "")
        {
            _webProxy = new WebProxy(uri, false)
            {
                UseDefaultCredentials = false,
                Credentials = string.IsNullOrEmpty(userName) ? null : new NetworkCredential(userName, password)
            };
            return this;
        }

        public HttpRestBuilder AddBrowserUserAgent(string browserUserAgent)
        {
            _browserUserAgent = browserUserAgent;
            return this;
        }

        public HttpRestBuilder AddContentType(string contentType)
        {
            _contentType = contentType;
            return this;
        }

        public HttpRestBuilder AddCookie(string name, string value, string path = null, string domain = null)
        {
            Cookie cookie = null;

            if(path == null && domain == null)
                cookie = new Cookie(name, value);
            else
                cookie = new Cookie(name, value, path, domain);

            if (_cookies == null)
                _cookies = new CookieCollection();

            _cookies.Add(cookie);

            return this;
        }
    }       
}