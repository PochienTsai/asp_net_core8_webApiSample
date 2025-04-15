using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace asp_net_core8_webApiSample.Services
{
  public class LdapService
  {
    private readonly string _ldapPath;
    private readonly string _username;
    private readonly string _password;

    public LdapService(string ldapPath, string username, string password)
    {
      _ldapPath = ldapPath;
      _username = username;
      _password = password;
    }

    // 驗證使用者帳號密碼
    public bool ValidateUser(string userName, string password)
    {
      using (var context = new PrincipalContext(ContextType.Domain, null, _ldapPath, _username, _password))
      {
        return context.ValidateCredentials(userName, password);
      }
    }

    // 取得使用者資訊
    public UserPrincipal? GetUserInfo(string userName)
    {
      using (var context = new PrincipalContext(ContextType.Domain, null, _ldapPath, _username, _password))
      using (var searcher = new PrincipalSearcher(new UserPrincipal(context) { SamAccountName = userName }))
      {
        return searcher.FindOne() as UserPrincipal;
      }
    }
  }
}
