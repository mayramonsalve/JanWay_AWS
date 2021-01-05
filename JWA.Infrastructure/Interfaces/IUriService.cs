using System;

namespace JWA.Infrastructure.Interfaces
{
    public interface IUriService
    {
        Uri GetPaginationUri(string actionUrl);
    }
}