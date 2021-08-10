using DALContract;
using Microsoft.AspNetCore.Mvc;
using ProjectContracts;
using ProjectDTO;

namespace FinalProject.Controllers
{
    enum RequestedAction { CreateShare, RemoverShare }
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class SharingController : ControllerBase
    {
        IUserService _userService;
        ISharingService _sharService;
        IDocumentService _documentService;
        public SharingController(IUserService userService, IDocumentService documentService, ISharingService sharService, IDAL iDAL)
        {
            _userService = userService;
            _sharService = sharService;
            _documentService = documentService;
            _userService.DALServices = iDAL;
            _sharService.DALServices = iDAL;
            _documentService.DALServices = iDAL;
        }
        [HttpPost]
        public GeneralResponseDTO CreateShare([FromBody] ShareDTO shareDTO)
        {
            var document = _documentService.GetDocumentByID(shareDTO.DocumentID);
            GeneralResponseDTO retval = responseGenerator(shareDTO.UserID, shareDTO.ToShareUserID, shareDTO.DocumentID, document);
            if (shareDTO.UserID == shareDTO.ToShareUserID)
            {
                retval.Message = "User cannot share documents with himself";
                retval.Succeed = false;
            }
            if (retval.Succeed)
            {
                if (document.UserID == shareDTO.UserID)
                {
                    retval.Succeed = _sharService.CreateShare(shareDTO.DocumentID, shareDTO.ToShareUserID);
                    if (!retval.Succeed)
                    {
                        retval.Message = "Cannot add this share (SQL failed)";
                    }
                }
                else
                {
                    retval.Message = shareDTO.UserID + " is not owner of document: " + shareDTO.DocumentID;
                    retval.Succeed = false;
                }
            }
            return retval;
        }

        [HttpPost]
        public GeneralResponseDTO RemoverShare([FromBody] ShareDTO shareDTO)
        {
            var document = _documentService.GetDocumentByID(shareDTO.DocumentID);
            GeneralResponseDTO retval = responseGenerator(shareDTO.UserID, shareDTO.ToShareUserID, shareDTO.DocumentID, document);
            if (retval.Succeed)
            {
                if (document.UserID == shareDTO.UserID || shareDTO.ToShareUserID == shareDTO.UserID)
                {
                    retval.Succeed = _sharService.RemoverShare(shareDTO.DocumentID, shareDTO.ToShareUserID);
                    if (!retval.Succeed)
                    {
                        retval.Message = "Cannot remove share " +shareDTO.ToShareUserID+","+ shareDTO.DocumentID;
                    }
                }
                else
                {
                    retval.Message = "you are not allowed to delete this share";
                    retval.Succeed = false;
                }
            }
            return retval;
        }

        private GeneralResponseDTO responseGenerator(string userID, string toShareUserID, string documentID, DocumentDTO document)
        {
            var retval = new GeneralResponseDTO();
            retval.Succeed = true;
            retval.Message = "All good";
           
            if (!_userService.IsUserExist(userID))
            {
                retval.Message = "User not exist";
                retval.Succeed = false;
            }
            if (!_userService.IsUserExist(toShareUserID))
            {
                retval.Message = "User: " + toShareUserID + " not exist";
                retval.Succeed = false;
            }
            if (document == null)
            {
                retval.Message = documentID + " not exsit";
                retval.Succeed = false;
            }

            return retval;
        }
    }
}

