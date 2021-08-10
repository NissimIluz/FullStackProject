using Microsoft.AspNetCore.Mvc;
using System;
using ProjectContracts;
using ProjectDTO;
using System.IO;
using Microsoft.Extensions.Options;
using Config;
using DALContract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private IDocumentService _doc;
        private ISharingService _share;
        private string _path;
        private string _relativePath;
        private IUserService _user;

        public DocumentController(IDocumentService doc, IOptions<ConfigOptions> settings, IDAL dal, IUserService user, ISharingService share)
        {
            _doc = doc;
            _path = settings.Value.UploadPath;
            _relativePath = settings.Value.RelativePath;
            _doc.DALServices = dal;
            _user = user;
            _user.DALServices = dal;
            _share = share;
            _share.DALServices = dal;
        }

        [HttpPost]
        public GeneralResponseDTO Upload([FromForm] UploadDTO upload)
        {
            DocumentDTO docDetail = new DocumentDTO();
            GeneralResponseDTO docRes = new GeneralResponseDTO();
            docRes.Succeed = false;
            if (_user.IsUserExist(upload.UserID))
            {
                var fileType = Path.GetExtension(upload.file.FileName);
                if (fileType.ToLower() == ".gif" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
                {
                    var docName = Path.GetFileName(upload.file.FileName);
                    docName = docName.Replace(" ", "");
                    if (upload != null && upload.file.Length > 0)
                    {
                        string docId = Guid.NewGuid().ToString();
                        DateTime dateTime = DateTime.Now;
                        string fileName = dateTime.ToString("yy_MM_dd_hh_mm_ss") + docName;
                        var filePath = Path.Combine(_path, fileName);
                        var relPath = Path.Combine(_relativePath, fileName);
                        docDetail.UserID = upload.UserID;
                        docDetail.documentName = fileName;
                        docDetail.documentID = docId;
                        docDetail.ImageURL = relPath;
                        try
                        {
                            _doc.UploadDocument(upload.file, filePath);
                            docRes = _doc.CreateDocument(docDetail);
                        }
                        catch (Exception ex)
                        {
                            docRes.Message = ex.Message;
                        }
                    }
                    else
                    {
                        docRes = null;
                    }
                }
            }
            else
            {
                docRes.Message = "Invalid UserID";
            }
            return docRes;
        }

        [HttpPost]
        public GeneralResponseDTO RemoveDocument([FromBody] UserDocumentDTO docInfo)
        {
            DocumentDTO doc = _doc.GetDocumentByID(docInfo.DocumentID);
            GeneralResponseDTO retval = new GeneralResponseDTO();
            retval.Succeed = false;
            if (!_user.IsUserExist(docInfo.UserID))
            {
                retval.Message = "Invalid UserID";
            }
            else
            {
                if (doc == null)
                {
                    retval.Message = $"Invalid DocumentID";
                }
                else if (doc.UserID != docInfo.UserID)
                {
                    retval.Message = $"UserId '{docInfo.UserID}' is not the owner of Document '{docInfo.DocumentID}'";
                }
                else
                {
                    string filePath = Path.GetFullPath(Path.Combine(_path, doc.documentName));
                    FileInfo file = new FileInfo(filePath);
                    try
                    {
                        file.Delete();
                        retval.Succeed = _doc.RemoveDocument(docInfo.DocumentID);
                    }
                    catch (Exception ex)
                    {
                        retval.Message = ex.Message.ToString();
                    }
                }
            }

            return retval;
        }

        [HttpPost]
        public GetDocumentsDTO GetUserDocuments([FromBody] IDDTO i_UserID)
        {
            GetDocumentsDTO retval = null;
            if (_user.IsUserExist(i_UserID.ID))
            {
                retval = _doc.GetAllDocuments(i_UserID.ID);
                retval.SharedWithUser = _share.GetSharedDocuments(i_UserID.ID);
            }
            return retval;
        }

        [HttpPost]
        public GeneralResponseDTO Download([FromBody] IDDTO docId)
        {
            GeneralResponseDTO retval = new GeneralResponseDTO();
            DocumentDTO fileDetail = _doc.GetDocumentByID(docId.ID);
            retval.Succeed = false;
            if (fileDetail != null)
            {
                //get physical path
                var fileReadPath = Path.Combine(_path, fileDetail.documentName);
                if (!System.IO.File.Exists(fileReadPath))
                {
                    retval.Message = "File not found";
                }
                else
                {
                    retval.Succeed = true;
                    retval.Message = fileDetail.ImageURL;
                }
            }
            else
            {
                retval.Message = $"Invalid id:'{docId}'";
            }
            return retval;
        }
    }
}

