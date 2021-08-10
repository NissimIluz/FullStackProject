using NUnit.Framework;
using ProjectContracts;
using ProjectDTO;
using Microsoft.Extensions.Options;
using UserServiceImpl;
using DocumentServiceImpl;
using InfraDAL;
using Config;
using FinalProject.Controllers;
using Microsoft.AspNetCore.Http;
using System.IO;
using SharingServiceImp;

namespace TestFullStackProject
{
    public class DocumentServiceUnitTest
    {
        IUserService m_UserService;
        UserDTO m_ExistingUser;
        UserDTO m_NoneExistUser;
        IDocumentService _doc;
        UserDocumentDTO userdoc;
        string _path;
        private IOptions<ConfigOptions> appSettingsOptions;
        DAL dal;
        ISharingService m_shareService;
        [SetUp]
        public void Setup()
        {
            string sqlConn = "Server = LAPTOP-46S9CPN6\\SQLEXPRESS;";
            var settings = new ConfigOptions()
            {
                MyConn = sqlConn + "Database = FinalProjectDB; Trusted_Connection = True;",
                UploadPath = "..\\..\\..\\..\\..\\..\\fsfinalclient_4\\FullStuckFinalProject\\src\\assets"
            };
            appSettingsOptions = Options.Create(settings);
            dal = new DAL(appSettingsOptions);
            userdoc = new UserDocumentDTO();
            userdoc.UserID = "nisim@fss.ssr22";
            userdoc.DocumentID = "99cdd142-f7b5-4cca-b3a2-6b2e78b21475";
            m_UserService = new UserService();
            _doc = new DocumentService();
            m_UserService.DALServices = dal;
            _doc.DALServices = dal;
            _path = appSettingsOptions.Value.UploadPath;
            m_ExistingUser = new UserDTO();
            m_NoneExistUser = new UserDTO();
            m_ExistingUser.UserID = "1";
            m_NoneExistUser.UserID = "22";
            m_ExistingUser.UserName = "Test@gmail.com";
            m_NoneExistUser.UserName = "Tests@gmail.com";
            m_shareService = new SharingService();
            m_shareService.DALServices = dal;
        }

        [Test]
        public void Test_1_UploadDocument()
        {
            var fullPath = Path.GetTempFileName();
            IFormFile file;
            var stream = File.OpenRead(fullPath);
            file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
            _doc.UploadDocument(file, $"{_path}\\21_06_29_10_14_26iis.png");
            stream.Close();
            Assert.IsTrue(File.Exists(fullPath));
        }
        [Test]
        public void Test_2_RemoveDocument()
        {
            DocumentController document = new DocumentController(_doc, appSettingsOptions, dal, m_UserService, m_shareService);
            string filePath = $"{_path} + \\21_06_29_10_14_26iis.png";
            Assert.IsTrue(File.Exists(filePath));
            GeneralResponseDTO result = document.RemoveDocument(userdoc);
            Assert.AreEqual(true, result.Succeed);
            Assert.IsTrue(!File.Exists(filePath));
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
    }
}