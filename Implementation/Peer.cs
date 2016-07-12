using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Integration;

namespace Implementation
{
    /// <summary>
    /// defines a Peer, to which Messages can be send
    /// </summary>
    public class Peer : IPeer
    {
        /// <summary>
        /// for example: "http" or "https"
        /// </summary>
        public string ProtocolScheme { get; set; }

        /// <summary>
        /// the uri, including the following informations: <see cref="ProtocolScheme"/>, <see cref="IpEndPoint"/> (host adress or name and port), Path
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// the host adress or name and port. <see cref="IpEndPoint"/>
        /// </summary>
        public IPEndPoint IpEndPoint { get; set; }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <returns>if the test succeded</returns>
        public async Task<bool> TestConnectionAsync()
        {
            // TODO Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests the connection and return's the http response.
        /// </summary>
        /// <returns>the http response. See: <see cref="HttpResponseMessage"/></returns>
        public async Task<HttpResponseMessage> TestConnectionWithResponseAsync()
        {
            HttpResponseMessage httpResult;

            var requestMessage = new HttpRequestMessage { Version = new Version("1.1"), Method = HttpMethod.Get, RequestUri = Uri };

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpResult = await httpClient.SendAsync(requestMessage);
                }
            }
            catch (Exception)
            {
                // scheme += "s";
                // Uri.TryCreate(string.Format("{1}://{0}", new object[] { peer, scheme }), UriKind.Absolute, out url);
                // endPoint = new IPEndPoint(ip, url.Port);
                // requestMessage = new HttpRequestMessage { Version = new Version("1.1"), Method = HttpMethod.Get, RequestUri = url };

                try
                {
                    // using (var httpClient = new HttpClient())
                    // {
                    //     httpResult = httpClient.SendAsync(requestMessage);
                    // }
                }
                catch (Exception)
                {
                    // TODO Add logging
                    throw;
                }
            }

            // TODO Implement
            return null;
        }
    }
}
