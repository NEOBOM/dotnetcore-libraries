using System.Net.Http;
using System.Threading.Tasks;

namespace Library.HttpServices.Builders.Interfaces
{
    public interface IHttpBuilder
    {
        HttpResponseMessage GetSync(string uri);

        Task<HttpResponseMessage> GetAsync(string uri);

        string GetStringSync(string uri);

        Task<string> GetStringAsync(string uri);

        Task<HttpResponseMessage> PostAsync(string uri, string content);

        HttpResponseMessage PostSync(string uri, string content);

        Task<HttpResponseMessage> PostAsync(string uri, object content);

        HttpResponseMessage PostSync(string uri, object content);
    }
}
