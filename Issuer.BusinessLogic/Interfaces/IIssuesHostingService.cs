using Issuer.BusinessLogic.Models;

namespace Issuer.BusinessLogic.Interfaces;

public interface IIssuesHostingService
{
    Task<List<Issue>> GetIssuesAsync();
}
