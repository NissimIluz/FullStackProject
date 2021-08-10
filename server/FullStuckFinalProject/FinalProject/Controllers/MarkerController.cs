using System;
using DALContract;
using Microsoft.AspNetCore.Mvc;
using ProjectContracts;
using ProjectDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class MarkerController : ControllerBase
    {
        IUserService _userService;
        IMarkerService _markerService;
        ISharingService _sharingService;
        IDocumentService _documentService;
        public MarkerController(IUserService userService, IMarkerService markerService, IDocumentService doc, ISharingService share, IDAL iDAL)
        {
            _userService = userService;
            _markerService = markerService;
            _documentService = doc;
            _sharingService = share;
            _userService.DALServices = iDAL;
            _markerService.DALServices = iDAL;
            _documentService.DALServices = iDAL;
            _sharingService.DALServices = iDAL;
        }
        // POST api/<MarkerController>
        [HttpPost]
        public GeneralResponseDTO AddNewMarker([FromBody] Marker marker)
        {
            //check if user exist and then add the marker.
            GeneralResponseDTO respnse = new GeneralResponseDTO();
            respnse.Succeed = false;
            Document[] allSharedDocument = _sharingService.GetSharedDocuments(marker.UserID);
            bool isSharedDocument = false;
            foreach (Document doc in allSharedDocument)
            {
                if (doc.DocumentID == marker.DocumentID)
                    isSharedDocument = true;
            }
            if (_userService.IsUserExist(marker.UserID))
            {
                DocumentDTO documentDTO = _documentService.GetDocumentByID(marker.DocumentID);
                if (documentDTO != null)
                {
                    if (documentDTO.UserID == marker.UserID || isSharedDocument)
                    {
                        try
                        {
                            string retval = _markerService.CreateMarker(marker.DocumentID, marker.MarkerType, marker.X1, marker.Y1, marker.X2, marker.Y2, marker.MarkerStrokeColor, marker.MarkerFillColor, marker.UserID);
                            if (retval == null)
                            {
                                respnse.Message = "Can't save this marker";
                            }
                            else
                            {
                                respnse.Message = retval;
                                respnse.Succeed = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            respnse.Message = ex.Message;
                        }
                    }
                    else
                    {
                        respnse.Message = "Document doesn't connect to this user!";
                    }
                }
                else
                {
                    respnse.Message = "Document doesn't exist!";

                }
            }
            else
            {
                respnse.Message = "User not exist!";

            }
            return respnse;
        }

        [HttpPost]
        public GeneralResponseDTO UpdateMarker([FromBody] Marker marker)
        {
            //as before and change marker color.
            GeneralResponseDTO respnse = new GeneralResponseDTO();
            respnse.Succeed = false;
            Document[] allSharedDocument = _sharingService.GetSharedDocuments(marker.UserID);
            bool isSharedDocument = false;
            foreach (Document doc in allSharedDocument)
            {
                if (doc.DocumentID == marker.DocumentID)
                    isSharedDocument = true;
            }
            Marker[] allMarkers = _markerService.GetMarkers(marker.DocumentID);
            if (allMarkers.Length == 0)
            {
                respnse.Message = "Document doesn't exist!";
            }
            else
            {
                bool isFound = false;
                foreach (Marker m in allMarkers)
                {
                    //check if marker exist:
                    if (m.MarkerID == marker.MarkerID)
                    {
                        isFound = true;
                        if (_userService.IsUserExist(marker.UserID))
                        {
                            DocumentDTO documentDTO = _documentService.GetDocumentByID(marker.DocumentID);
                            if (documentDTO != null)
                            {
                                if (documentDTO.UserID == marker.UserID || isSharedDocument)
                                {
                                    try
                                    {
                                        respnse.Succeed = _markerService.ChangeColor(marker.MarkerID, marker.MarkerStrokeColor, marker.MarkerFillColor);
                                        if (respnse.Succeed)
                                            respnse.Message = "Successfully saved";
                                    }
                                    catch (Exception ex)
                                    {
                                        respnse.Message = ex.Message;
                                    }
                                }
                                else
                                {
                                    respnse.Message = "Document doesn't connect to this user!";
                                }
                            }
                            else
                            {
                                respnse.Message = "Document doesn't exist!";
                            }
                        }
                        else
                            respnse.Message = "User not exist!";
                    }
                }
                if (!isFound)
                {
                    respnse.Message = "Marker doen't exist!";
                }


            }
            return respnse;

        }

        [HttpPost]
        public GeneralResponseDTO RemoveMarker([FromBody] Marker marker)
        {
            //check if user and marker exist and then remove.
            GeneralResponseDTO respnse = new GeneralResponseDTO();
            if (_userService.IsUserExist(marker.UserID))
            {
                DocumentDTO documentDTO = _documentService.GetDocumentByID(marker.DocumentID);
                if (documentDTO != null)
                {
                    try
                    {
                        respnse.Succeed = _markerService.RemoveMarker(marker.MarkerID);
                        if (!respnse.Succeed)
                        {
                            respnse.Message = "Can't remove this marker";
                        }
                        else
                        {
                            respnse.Message = "Removed successfully";
                        }

                    }
                    catch (Exception ex)
                    {
                        respnse.Message = ex.Message;

                    }
                }
                else
                {
                    respnse.Message = "Document doesnt connect to this user!";
                }

            }
            else
                respnse.Message = "User not exist!";
            return respnse;
        }
        [HttpPost]
        public MarkerDTO GetMarkers([FromBody] UserDocumentDTO userDocumentDTO)
        {

            MarkerDTO retval = new MarkerDTO();
            retval.Succesed = false;

            if (_userService.IsUserExist(userDocumentDTO.UserID))
            {
                bool access = false;
                var document = _documentService.GetDocumentByID(userDocumentDTO.DocumentID);
                if (document != null)
                {
                    if (document.UserID == userDocumentDTO.UserID)
                        access = true;
                    else
                    {
                        var allSharedDocument = _sharingService.GetSharedDocuments(userDocumentDTO.UserID);
                        foreach (Document doc in allSharedDocument)
                        {
                            if (doc.DocumentID == userDocumentDTO.DocumentID)
                                access = true;
                        }
                    }
                    if (access)
                    {
                        retval.MarkersArray = _markerService.GetMarkers(userDocumentDTO.DocumentID);
                        retval.Succesed = true;

                    }
                    else
                    {
                        retval.Message = "Access Denied";
                    }
                }
                else
                {
                    retval.Message = "Document not exist";
                }

            }
            else
            {
                retval.Message = "User not exists";
            }
            return retval;
        }
    }
}
