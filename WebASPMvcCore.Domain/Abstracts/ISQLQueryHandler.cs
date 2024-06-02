
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WebASPMvcCore.Domain.Abstracts
{
    public interface ISQLQueryHandler
    {
        Task ExecuteNonReturnAsync(string query, DynamicParameters parammeters = null, IDbTransaction dbTransaction = null);
        Task<T> ExecuteReturnSingleRowAsync<T>(string query, DynamicParameters parammeters = null, IDbTransaction dbTransaction = null);
        Task<T?> ExecuteReturnSingleValueScalarAsync<T>(string query, DynamicParameters parammeters = null, IDbTransaction dbTransaction = null);
        Task<IEnumerable<T>> ExecuteStoreProdecureReturnListAsync<T>(string storeName, DynamicParameters parammeters = null, IDbTransaction dbTransaction = null);
        Task<IEnumerable<T>> ExecuteReturnListRowAsync<T>(string query, DynamicParameters parammeters = null, IDbTransaction dbTransaction = null);
    }
}
