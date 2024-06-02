using Microsoft.AspNetCore.Http;

namespace WebASPMvcCore.Insfrastructure.Abstracts
{
    public interface IHelperService
    {
        string ConvertToSlug(string str);
    }
}
