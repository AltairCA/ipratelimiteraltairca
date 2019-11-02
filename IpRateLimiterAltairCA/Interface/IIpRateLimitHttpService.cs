using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IpRateLimiter.AspNetCore.AltairCA.Interface
{
    public interface IIpRateLimitHttpService
    {
        Task ClearLimit();
        Task ClearLimit([Required(AllowEmptyStrings = false)] string groupKey);
    }
}