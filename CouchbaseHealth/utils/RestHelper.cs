using RestSharp;
using RestSharp.Authenticators;
using System;

namespace CouchbaseHealth.utils
{
    public class RestHelper
    {

        private readonly IAuthenticator _authenticator;
        private readonly Uri _baseUrl;
        public RestHelper(IAuthenticator authenticator,Uri baseUrl)
        {
            _authenticator = authenticator;
            _baseUrl = baseUrl;
        }

        public T Execute<T>(IRestRequest request) where T : new()
        {
            var client = new RestClient
            {
                BaseUrl = _baseUrl,
                Authenticator = _authenticator
            };

            var response = client.Execute<T>(request);

            if (response.ErrorException == null) return response.Data;

            const string message = "Error retrieving response.  Check inner details for more info.";
            throw new ApplicationException(message, response.ErrorException);
        }

    }
}
