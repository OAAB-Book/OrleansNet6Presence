using System.Threading.Tasks;
using Grains.Interfaces.Models;
using Orleans;

namespace Grains.Interfaces
{
    public interface IGameGrain : IGrainWithGuidKey
    {
        Task UpdateGameStatusAsync(GameStatus status);
        Task ObserveGameUpdatesAsync(IGameObserver observer);
        Task UnobserveGameUpdatesAsync(IGameObserver observer);
    }
}