using System.Net.Http;

namespace HL.Loyalty.Common
{
    public interface IRestClient
    {
        T GetProxyData<T>(string baseAddressConfig, string uri);
        HttpResponseMessage PostProxyData<T>(T postData, string baseAddressConfig, string uri);
    }
}