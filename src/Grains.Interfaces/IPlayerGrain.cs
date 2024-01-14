using System.Threading.Tasks;
using Orleans;

namespace Grains.Interfaces
{
    public interface IPlayerGrain : IGrainWithGuidKey
    {
        Task<IGameGrain> GetCurrentGameAsync();
        Task JoinGameAsync(IGameGrain game);
        Task LeaveGameAsync(IGameGrain game); 
    }
}