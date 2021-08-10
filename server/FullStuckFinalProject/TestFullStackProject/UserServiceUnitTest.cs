using NUnit.Framework;
using ProjectContracts;
using ProjectDTO;
using System;
using Microsoft.Extensions.Options;
using System.Text;
using UserServiceImpl;
using InfraDAL;
using Config;
using DALContract;

namespace TestFullStackProject
{
    public class UserServiceUnitTest
    {
        IUserService m_UserService;
        Guid m_generatedUUID = Guid.NewGuid();
        UserDTO m_ExistingUser;
        UserDTO m_NoneExistUser;
        UserDTO m_NewUser;

        [SetUp]
        public void Setup()
        {
            string sqlConn = "Server = LAPTOP-46S9CPN6\\SQLEXPRESS;";
            var settings = new ConfigOptions()
            {

                MyConn = sqlConn + "Database = FinalProjectDB; Trusted_Connection = True;",
            };
            IOptions<ConfigOptions> appSettingsOptions = Options.Create(settings);
            DAL dal = new DAL(appSettingsOptions);
            m_UserService = new UserService();
            m_UserService.DALServices = dal;
            m_ExistingUser = new UserDTO();
            m_NoneExistUser = new UserDTO();
            m_NewUser = new UserDTO();
            m_ExistingUser.UserID = "1";
            m_NoneExistUser.UserID = "22";
            m_NewUser.UserID = m_generatedUUID.ToString();
            m_ExistingUser.UserName = "Test@gmail.com";
            m_NoneExistUser.UserName = "Tests@gmail.com";
            m_NewUser.UserName = string.Format("{0}@gmail.com", m_generatedUUID.ToString());
        }
        [Test]
        public void Test_1_RegisterNoneExistingUser()
        { 
            Assert.AreEqual(true, m_UserService.SignUp(m_NewUser));
        }
        [Test]
        public void Test_2_RegisterExistingUser()
        {
            Assert.AreEqual(false, m_UserService.SignUp(m_ExistingUser));
        }
        [Test]
        public void Test_3_CheckIfUserExist()
        {
            Assert.AreEqual(true, m_UserService.IsUserExist(m_ExistingUser.UserID));
        }
        [Test]
        public void Test_4_CheckIfUserNotExist()
        {
            Assert.AreEqual(false, m_UserService.IsUserExist(m_NoneExistUser.UserID));
        }
        [Test]
        public void Test_5_UnsubscribeUser()
        {
            Assert.AreEqual(true, m_UserService.Unsubscribe(m_NewUser.UserID));
        }
    }
}