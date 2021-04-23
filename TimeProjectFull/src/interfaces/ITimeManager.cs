using System.Threading.Tasks;
namespace Timeproject.Interface
{
    interface ITimeManager
    {
        Task<bool> CompareTime();
    }
}