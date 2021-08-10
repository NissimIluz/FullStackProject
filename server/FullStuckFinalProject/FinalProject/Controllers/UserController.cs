using DALContract;
using Microsoft.AspNetCore.Mvc;
using ProjectContracts;
using ProjectDTO;

namespace FinalProject.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService m_UserService;
        public UserController(IUserService i_UserService, IDAL i_Dal)
        {
            m_UserService = i_UserService;
            m_UserService.DALServices = i_Dal;
        }
        [HttpPost]
        public GeneralResponseDTO SignUp([FromBody] UserDTO i_UserDetails)
        {
            GeneralResponseDTO signupResult = new GeneralResponseDTO();
            if (!m_UserService.IsUserExist(i_UserDetails.UserID))
            {

                signupResult.Succeed = m_UserService.SignUp(i_UserDetails);
                if(!signupResult.Succeed)
                {
                    signupResult.Message = "Error, User already exists";
                }
                else
                {
                    signupResult.Message = "User signed up successfully";
                }
            }
            else
            {
                signupResult.Succeed = false;
                signupResult.Message = "Error, User already exist";
            }
            return signupResult;
        }
        [HttpPost]
        public GeneralResponseDTO Login([FromBody] IDDTO i_UserID)
        {
            GeneralResponseDTO isUserExistResult = new GeneralResponseDTO();
            isUserExistResult.Succeed = m_UserService.IsUserExist(i_UserID.ID);
            if(!isUserExistResult.Succeed)
            {
                isUserExistResult.Message = "Error, User is not exist";
            }
            else
            {
                isUserExistResult.Message = "User logged in successfully";
            }
                return isUserExistResult;
        }
        [HttpPost]
        public GeneralResponseDTO MarkUserAsRemoved([FromBody] IDDTO i_UserID)
        {
            GeneralResponseDTO markUserAsRemovedResult = new GeneralResponseDTO();
            if (m_UserService.IsUserExist(i_UserID.ID))
            {
                markUserAsRemovedResult.Succeed = m_UserService.Unsubscribe(i_UserID.ID);
                if (markUserAsRemovedResult.Succeed)
                {
                    markUserAsRemovedResult.Message = "User is no longer subscribed";
                }
                else
                { 
                    markUserAsRemovedResult.Message = "General error, Please check your internet connection"; 
                }
            }
            else
            {
                markUserAsRemovedResult.Succeed = false;
                markUserAsRemovedResult.Message = "Error, User is not exist";
            }
        return markUserAsRemovedResult;
        }

        [HttpPost]
        public string[] GetUsers([FromBody] IDDTO i_UserID)
        {
            string[] retval = null;
            if (m_UserService.IsUserExist(i_UserID.ID))
            {
                retval = m_UserService.GetUsers(i_UserID.ID);
            }

            return retval;
        }
    }
}
