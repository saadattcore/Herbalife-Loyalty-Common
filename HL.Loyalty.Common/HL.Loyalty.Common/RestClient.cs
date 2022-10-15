using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HL.Loyalty.Common
{
    public class RestClient : IRestClient
    {
        #region Private methods
        /// <summary>
        /// Generic Method Which Get Records From Query Api
        /// </summary>
        /// <typeparam name="T">type into which result from query api will be transled to</typeparam>
        /// <param name="uri">uri with base address plus any additional query string</param>
        /// <returns></returns>
        public T GetProxyData<T>(string baseAddress, string uri)
        {
            T ret = default(T);
            using (var client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(uri).Result;
                ret = response.Content.ReadAsAsync<T>().Result;
            }

            return ret;
        }

        /// <summary>
        /// Post data to specified end point 
        /// </summary>
        /// <typeparam name="T">Genertic type</typeparam>
        /// <param name="postData">Data to post</param>
        /// <param name="baseAddressConfig">Base address for the end point</param>
        /// <param name="uri">Url specifying api controller and action</param>
        /// <returns></returns>
        public HttpResponseMessage PostProxyData<T>(T postData, string baseAddressConfig, string uri)
        {
            using (var client = new HttpClient())
            {
                var baseAddress = baseAddressConfig;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync(uri, postData).Result;

                return response;
            }
        }

        #endregion
    } 
}
