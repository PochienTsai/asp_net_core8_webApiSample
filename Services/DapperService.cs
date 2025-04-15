using System.Data;
using Dapper;

namespace asp_net_core8_webApiSample.Services
{
  public class DapperService
  {
    private readonly IDbConnection _dbConnection;

    public DapperService(IDbConnection dbConnection)
    {
      _dbConnection = dbConnection;
    }

    // 建立
    public int Create<T>(string sql, T param)
    {
      return _dbConnection.Execute(sql, param);
    }

    // 讀取
    public IEnumerable<T> Read<T>(string sql, object? param = null)
    {
      return _dbConnection.Query<T>(sql, param);
    }

    // 更新
    public int Update<T>(string sql, T param)
    {
      return _dbConnection.Execute(sql, param);
    }

    // 刪除
    public int Delete(string sql, object? param = null)
    {
      return _dbConnection.Execute(sql, param);
    }
  }
}
