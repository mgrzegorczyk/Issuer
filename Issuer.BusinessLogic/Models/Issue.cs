namespace Issuer.BusinessLogic.Models;

public record Issue(
    string Title,
    string Description,
    bool IsClosed
);