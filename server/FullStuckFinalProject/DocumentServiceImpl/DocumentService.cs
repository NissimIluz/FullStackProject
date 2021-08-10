using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DALContract;
using Microsoft.AspNetCore.Http;
using ProjectContracts;
using ProjectDTO;
using InfraAttributes;

namespace DocumentServiceImpl
{
    [Register(typeof(IDocumentService))]
    [Policy(Policy.Transient)]
    public class DocumentService : IDocumentService
    {
        public IDAL DALServices { get; set; }

        public GeneralResponseDTO CreateDocument(DocumentDTO doc)
        {
            GeneralResponseDTO retval = new GeneralResponseDTO();
            retval.Succeed = false;
            var parameterDocID = DALServices.CreateParameter("DocumentID", doc.documentID);
            var parameterUserID = DALServices.CreateParameter("UserID", doc.UserID);
            var parameterImageURL = DALServices.CreateParameter("ImageURL", doc.ImageURL);
            var parameterDocName = DALServices.CreateParameter("DocumentName", doc.documentName);
            try
            {
                DALServices.ExecuteQuery("CreateDocument", parameterImageURL, parameterDocID, parameterDocName, parameterUserID);
                retval.Succeed = true;
                retval.Message = $"DocumentId:{doc.documentID},DocumentName:{doc.documentName}";
            }
            catch
            {
                retval.Succeed = false;

            }
                       
            return retval;
        }

        public DocumentDTO GetDocumentByID(string docId)
        {
            var parameterDocID = DALServices.CreateParameter("DocumentID", docId);
            var dataset = DALServices.ExecuteQuery("GetDocumentByID", parameterDocID);
            DocumentDTO retval = null;
            if(dataset.Tables[0].Rows.Count > 0)
            {
                retval = new DocumentDTO();
                retval.UserID = (string)dataset.Tables[0].Rows[0]["UserID"];
                retval.documentID = (string)dataset.Tables[0].Rows[0]["DocumentID"];
                retval.documentName = (string)dataset.Tables[0].Rows[0]["DocumentName"];
                retval.ImageURL = (string)dataset.Tables[0].Rows[0]["ImageURL"];
            }
            
            return retval;
        }
        
        public GetDocumentsDTO GetAllDocuments(string i_UserID)
        {
            GetDocumentsDTO retval = new GetDocumentsDTO();
            getOwnDocuments(ref retval, i_UserID);
            getSharedByUser(ref retval, i_UserID);
            return retval;
        }

        private void getOwnDocuments(ref GetDocumentsDTO retval, string i_UserID)
        {
            var parameterUserID = DALServices.CreateParameter("UserID", i_UserID);
            var dataset = DALServices.ExecuteQuery("GetNotSharedWithUsers", parameterUserID);
            Document[] documents = new Document[dataset.Tables[0].Rows.Count];
            if (dataset.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                {
                    documents[i] = new Document();
                    documents[i].DocumentID = (string)dataset.Tables[0].Rows[i]["DocumentID"];
                    documents[i].DocumentName = (string)dataset.Tables[0].Rows[i]["DocumentName"];
                }
            }
            retval.ownDocuments = documents;
        }

        private void getSharedByUser(ref GetDocumentsDTO retval, string i_UserID)
        {
            var parameterUserID = DALServices.CreateParameter("UserID", i_UserID);
            var dataset = DALServices.ExecuteQuery("GetSharedByUser", parameterUserID);
            List<Document> documents = new List<Document>();
            if (dataset.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dataset.Tables[0].Rows.Count;)
                {
                    Document document = new Document();
                    document.SharedWithUsers = new List<string>();
                    document.DocumentID = (string)dataset.Tables[0].Rows[i]["DocumentID"];
                    document.DocumentName = (string)dataset.Tables[0].Rows[i]["DocumentName"];
                    while (i < dataset.Tables[0].Rows.Count && document.DocumentID == (string)dataset.Tables[0].Rows[i]["DocumentID"])
                    {
                        document.SharedWithUsers.Add((string)dataset.Tables[0].Rows[i++]["UserID"]);
                    }
                    documents.Add(document);
                }
            }
            documents.AddRange(retval.ownDocuments.ToList());
            retval.ownDocuments = documents.ToArray();
        }

        public bool RemoveDocument(string docId)
        {
            bool retval;
            var parametermDocumentID = DALServices.CreateParameter("DocumentID", docId);
            try
            {
                var dataset = DALServices.ExecuteQuery("RemoveDocument", parametermDocumentID);
                retval = true;
            }
            catch
            {
                retval = false;
            }

            return retval;
        }

        public void UploadDocument(IFormFile formFile, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }
        }
    }
}
