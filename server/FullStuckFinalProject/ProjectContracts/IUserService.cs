using DALContract;
using ProjectDTO;
using System;

namespace ProjectContracts
{
    public interface IUserService
    {
        public bool SignUp(UserDTO i_UserDetails);
        public bool IsUserExist(string userId);
        public bool Unsubscribe(string userId);
        public IDAL DALServices { get; set; }
        public string[] GetUsers(string userId);
    }
}