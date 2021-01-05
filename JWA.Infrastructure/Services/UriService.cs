using JWA.Core.QueryFilters;
using JWA.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWA.Infrastructure.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPaginationUri(string actionUrl) //filters removed from here
        {
            string baseUrl = $"{_baseUri}{actionUrl}";
            return new Uri(baseUrl);
        }
    }
}
