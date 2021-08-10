using System;
using System.Collections.Generic;
using System.Text;
using DALContract;
using Microsoft.AspNetCore.Http;
using ProjectDTO;

namespace ProjectContracts
{
    public interface IDocumentService
    {
        public IDAL DALServices { get; set; }
        public GeneralResponseDTO CreateDocument(DocumentDTO doc);
        public void UploadDocument(IFormFile formFile, string filePath);
        public DocumentDTO GetDocumentByID(string docId);
        public bool RemoveDocument(string docId);
        public GetDocumentsDTO GetAllDocuments(string userId);
    }
}
