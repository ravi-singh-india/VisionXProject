using Microsoft.Data.SqlClient;
using System.Data;
using VisionX.BusinessAccessLayer.Repositories;
using VisionX.BusinessEntities.Repositories;
using VisionX.DataAccessLayer.Repositories;
namespace VisionX.Interface.Repositories
{
    public class Account : IAccount
    {
        #region "Variable"

        SqlParameter[] sqlpara;
        private Dal oprDAL;
        private Bal oprBAL;
        #endregion

        public Account(Dal _oDAL, Bal _oBAL)
        {
            oprDAL = _oDAL;
            oprBAL = _oBAL;
        }
        public DataSet LoginAuthentication(string UserID, string Password)
        {
            string ClientHost = System.Net.Dns.GetHostName();
            string ClientIp = "";
            string ProductName = "";

            try
            {
                sqlpara = new SqlParameter[5];
                sqlpara[0] = new SqlParameter("@UserID", UserID);
                sqlpara[1] = new SqlParameter("@Password", oprBAL.Pass_Encrypt(Password));
                sqlpara[2] = new SqlParameter("@ClientHost", ClientHost);
                sqlpara[3] = new SqlParameter("@ClientIp", ClientIp);
                sqlpara[4] = new SqlParameter("@ProductName", ProductName);
                return oprDAL.GetDataSet("SPLogin", sqlpara);
            }
            catch
            {
                throw;
            }
        }
    }
}
