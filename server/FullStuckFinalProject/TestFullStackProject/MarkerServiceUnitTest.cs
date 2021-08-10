using MarkerServiceImp;
using NUnit.Framework;
using InfraDAL;
using Microsoft.Extensions.Options;
using Config;
using System;
using ProjectContracts;
using DocumentServiceImpl;
using ProjectDTO;

namespace TestFullStackProject
{
    public class MarkerTests
    {
        Guid generatedID = Guid.NewGuid();
        string docID, userID;
        IMarkerService markerService;
        int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
        string actual0;
        string actual1;
        string actual2;

        [SetUp]
        public void Setup()
        {
            string sqlConn = "Server = LAPTOP-46S9CPN6\\SQLEXPRESS;";
            var settings = new ConfigOptions()
            {

                MyConn = sqlConn + "Database = FinalProjectDB; Trusted_Connection = True;",
                UploadPath = @"C:\Users\eiloz\Desktop\FS project\NissimIluz\fsfinalclient_4\FullStuckFinalProject\src\assets"
            };
            IOptions<ConfigOptions> appSettingsOptions = Options.Create(settings);

            markerService = new MarkerService();
            markerService.DALServices = new DAL(appSettingsOptions);
            IDocumentService docService = new DocumentService();
            docService.DALServices = new DAL(appSettingsOptions);
            DocumentDTO docDTO = new DocumentDTO();
            docID = "testMarker-" + generatedID;
            userID = "test1234";
            try
            {
                docDTO.ImageURL = "tset";
                docDTO.documentName = "test";
                docDTO.documentID = docID;
                docDTO.UserID = userID;
                docService.CreateDocument(docDTO);
            }
            catch { }

        }

        [Test]
        public void Test_1_CreateMarker()
        {
            actual0 = markerService.CreateMarker(docID, "rectangle", x1, y1, x2, y2, "blue", "red", userID);
            Assert.AreEqual(true, actual0 != null);
        }
        [Test]
        public void Test_2_CreateMarker()
        {
            actual1 = markerService.CreateMarker(docID, "circle", x1, y1, x2, y2, "blue", "red", userID);
            Assert.AreEqual(true, actual1 != null);
        }
        [Test]
        public void Test_3_CreateMarker()
        {
            x1 = 782; y1 = 54; x2 = 15; y2 = 32;
            actual2 = markerService.CreateMarker(docID, "circle", x1, y1, x2, y2, "blue", "red", userID);
            Assert.AreEqual(true, actual2 != null);
        }
        [Test]
        public void Test_4_ChangeColor()
        {
            var color1 = "yellow";
            var color2 = "pink";
            var actual = markerService.ChangeColor(actual2, color1, color2);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Test_5_GetMarkers()
        {
            var array = markerService.GetMarkers(docID);
            for (var i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(true, (array[i].X1 == x1 && array[i].Y1 == y1) && (array[i].X2 == x2 && array[i].Y2 == y2) ||
                    (array[i].X1 == 0 && array[i].Y1 == 0) && (array[i].X2 == 0 && array[i].Y2 == 0));
            }
        }

        [Test]
        public void Test_6_RemoveMarker()
        {
            Assert.AreEqual(true, markerService.RemoveMarker(actual0));
            Assert.AreEqual(true, markerService.RemoveMarker(actual1));
            Assert.AreEqual(true, markerService.RemoveMarker(actual2));
            var array = markerService.GetMarkers(docID);
            Assert.AreEqual(true, markerService.GetMarkers(docID).Length == 0);
        }
    }
}