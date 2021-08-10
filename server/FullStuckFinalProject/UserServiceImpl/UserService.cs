using System;
using DALContract;
using ProjectContracts;
using ProjectDTO;
using InfraAttributes;

namespace UserServiceImpl
{
    [Register(typeof(IUserService))]
    [Policy(Policy.Transient)]
    public class UserService : IUserService
    {
        public IDAL DALServices { get; set; }
        public bool IsUserExist(string userId)
        {
            bool isUserLoggedInSuccessfully = false;
            var paramUserID = DALServices.CreateParameter("UserID", userId);
            var dataset = DALServices.ExecuteQuery("GetUser", paramUserID);
            if (dataset.Tables[0].Rows.Count != 0)
            {
                isUserLoggedInSuccessfully = true;
            }
            return isUserLoggedInSuccessfully;
        }

        public bool SignUp(UserDTO i_UserDetails)
        {
            bool isUserSignUpSuccessfully;
            var paramUserUserID = DALServices.CreateParameter("UserID", i_UserDetails.UserID);
            var paramUserName = DALServices.CreateParameter("UserName", i_UserDetails.UserName);
            try
            {
                DALServices.ExecuteNonQuery("CreateUser", paramUserUserID, paramUserName);
                isUserSignUpSuccessfully = true;
            }
            catch
            {
                isUserSignUpSuccessfully = false;
            }
            return isUserSignUpSuccessfully;
        }

        public bool Unsubscribe(string userId)
        {
            bool isUserUnsubscribedSuccessfully;
            var paramUserUserID = DALServices.CreateParameter("UserID", userId);
            try
            {
                DALServices.ExecuteNonQuery("MarkUserAsRemoved", paramUserUserID);
                isUserUnsubscribedSuccessfully = true;
            }
            catch
            {
                isUserUnsubscribedSuccessfully = false;
            }
            return isUserUnsubscribedSuccessfully;
        }
        public string[] GetUsers(string userId)
        {
            var parameterUserID = DALServices.CreateParameter("UserID", userId);
            var dataset = DALServices.ExecuteQuery("GetAllOtherUsers", parameterUserID);
            string[] retval = new string[dataset.Tables[0].Rows.Count];
            for (var i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                retval[i] = (string)dataset.Tables[0].Rows[i]["UserID"];
            }
            return retval;
        }
    }
}