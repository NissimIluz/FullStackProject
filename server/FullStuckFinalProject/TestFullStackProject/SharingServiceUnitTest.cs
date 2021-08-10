using ProjectContracts;
using MarkerServiceImp;
using NUnit.Framework;
using InfraDAL;
using Microsoft.Extensions.Options;
using Config;
using System;
using DALContract;
using SharingServiceImp;
using UserServiceImpl;
using DocumentServiceImpl;
using ProjectDTO;

namespace TestFullStackProject
{

    public class SharingServiceUnitTest
    {
        ISharingService sharingService;
        DAL dal;
        string docID1;
        string docID2;
        string docID3;
        string docID4;
        string userID1;
        string userID2;
        IUserService userService;
        IDocumentService documentService;

        [SetUp]
        public void Setup()
        {
            sharingService = new SharingService();
            string sqlConn = "Server = DESKTOP-PBFVGRF\\SQLEXPRESS;";
            var settings = new ConfigOptions()
            {
                MyConn = sqlConn + "Database = FinalProjectDB; Trusted_Connection = True;",
            };
            IOptions<ConfigOptions> appSettingsOptions = Options.Create(settings);
            dal = new DAL(appSettingsOptions);

            sharingService.DALServices = new DAL(appSettingsOptions);
            docID1 = "SharingServiceTest_1";
            docID2 = "SharingServiceTest_2";
            docID3 = "SharingServiceTest_3";
            docID4 = "SharingServiceTest_4";
            userID1 = "SharingServiceTest_1";
            userID2 = "SharingServiceTest_2";
            userService = new UserService();
            documentService = new DocumentService();
            userService.DALServices = dal;
            documentService.DALServices = dal;
        }
        [Test]
        public void Test_1_Init()
        {
            UserDTO userDTO = new UserDTO();
            userDTO.UserID = userID1;
            userDTO.UserName = "test";
            try { userService.SignUp(userDTO); }
            catch { }
            userDTO.UserID = userID2;
            try { userService.SignUp(userDTO); }
            catch { }

            DocumentDTO docDTO = new DocumentDTO();
            docDTO.documentID = docID1;
            docDTO.documentName = "test";
            docDTO.ImageURL = "test";
            docDTO.UserID = userID1;
            Assert.AreEqual(true, documentService.CreateDocument(docDTO).Succeed);
            docDTO.documentID = docID2;
            Assert.AreEqual(true, documentService.CreateDocument(docDTO).Succeed);
            docDTO.documentID = docID3;
            Assert.AreEqual(true, documentService.CreateDocument(docDTO).Succeed);
            docDTO.UserID = userID2;
            docDTO.documentID = docID4;
            Assert.AreEqual(true, documentService.CreateDocument(docDTO).Succeed);

        }
        [Test]
        public void Test_2_CreateShare1()
        {
            var actual = sharingService.CreateShare(docID1, userID2);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Test_3_CreateShare2()
        {
            var actual = sharingService.CreateShare(docID2, userID1);
            Assert.AreEqual(true, actual);
        }
        [Test]
        public void Test_4_CreateShare3()
        {
            var actual = sharingService.CreateShare(docID3, userID2);
            Assert.AreEqual(true, actual);
        }
        [Test]
        public void Test_5_CreateShare4()
        {
            var actual = sharingService.CreateShare(docID4, userID1);
            Assert.AreEqual(true, actual);
        }
        [Test]
        public void Test_6_RemoverTest()
        {
            var actual = sharingService.RemoverShare(docID4, userID2);
            Assert.AreEqual(true, actual);
        }
        [Test]
        public void Test_7_GetSharedAndRemover()
        {
            bool actual;
            var array = sharingService.GetSharedDocuments(userID2);
            for (var i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(true, array[i].DocumentID == docID1 || array[i].DocumentID == docID2 || array[i].DocumentID == docID3);
                actual = sharingService.RemoverShare(array[i].DocumentID, userID2);
                Assert.AreEqual(true, actual);
            }
            array = sharingService.GetSharedDocuments(userID2);
            Assert.AreEqual(true, array.Length == 0);
        }
        [Test]
        public void Test_8_Clear()
        {
            Assert.AreEqual(true, documentService.RemoveDocument(docID1));
            Assert.AreEqual(true, documentService.RemoveDocument(docID2));
            Assert.AreEqual(true, documentService.RemoveDocument(docID3));
            Assert.AreEqual(true, documentService.RemoveDocument(docID4));
        }
    }
}
