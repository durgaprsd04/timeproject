using System.Threading.Tasks;
namespace Timeproject.Interface
{
    public interface ITimeAPIHelper
    {
        Task<string> GetUTCTimeFromAPI();
    }
}