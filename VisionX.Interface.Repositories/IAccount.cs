using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionX.Interface.Repositories
{
    public interface IAccount
    {
        DataSet LoginAuthentication(string UserID, string Password);
    }
}
