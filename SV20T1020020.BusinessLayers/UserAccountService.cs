using SV20T1020020.DataLayers;
using SV20T1020020.DataLayers.SQLServer;
using SV20T1020020.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020020.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAccountDB;
        static UserAccountService()
        {
            string connectionString = Configuration.ConnectiongString;
            employeeAccountDB = new EmployeeAccountDAL(connectionString);
        }
        public static UserAccount? Authorize(string userName, string password)
        {
            return employeeAccountDB.Authorize(userName, password);
        }
        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {

           return employeeAccountDB.ChangePassword(userName, oldPassword, newPassword);

        }

        public static bool CheckPassword(string userName, string oldPassword)
        {
            return employeeAccountDB.CheckPassword(userName, oldPassword);
        }
    }
}
