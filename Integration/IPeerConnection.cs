using System.Threading.Tasks;
using Integration.Contracts;

namespace Integration
{
    public interface IPeerConnection
    {
        IPeer Peer { get; set; }
        Task<bool> TestConnectionAsync();
        Task<object> TestConnectionWithResponseAsync();
        Task<object> SendMessage(IMessage message);
    }
}
