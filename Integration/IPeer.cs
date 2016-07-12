using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integration
{
    /// <summary>
    /// defines a Peer, to which Messages can be send
    /// </summary>
    public interface IPeer
    {
        /// <summary>
        /// for example: "http" or "https"
        /// </summary>
        string ProtocolScheme { get; set; }

        /// <summary>
        /// the uri, including the following informations: <see cref="ProtocolScheme"/>, <see cref="IpEndPoint"/> (host adress or name and port), Path
        /// </summary>
        Uri Uri { get; set; }

        /// <summary>
        /// the host adress or name and port. <see cref="IpEndPoint"/>
        /// </summary>
        IPEndPoint IpEndPoint { get; set; }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <returns>if the test succeded</returns>
        Task<bool> TestConnectionAsync();

        /// <summary>
        /// Tests the connection and return's the http response.
        /// </summary>
        /// <returns>the http response. See: <see cref="HttpResponseMessage"/></returns>
        Task<HttpResponseMessage> TestConnectionWithResponseAsync();
    }
}
