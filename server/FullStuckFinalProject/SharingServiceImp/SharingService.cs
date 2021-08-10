using System;
using DALContract;
using ProjectContracts;
using ProjectDTO;
using InfraAttributes;

namespace SharingServiceImp
{
    [Register(typeof(ISharingService))]
    [Policy(Policy.Transient)]
    public class SharingService : ISharingService
    {
        public bool CreateShare(string documentID, string userID)
        {
            return ExexutegGenericSP("CreateShare", documentID, userID);
        }
        public bool RemoverShare(string documentID, string userID)
        {
            return ExexutegGenericSP("RemoveShare", documentID, userID);
        }
        public Document[] GetSharedDocuments(string userID)
        {
            var parameterUserID = DALServices.CreateParameter("UserID", userID);
            var dateset = DALServices.ExecuteQuery("GetSharedDocuments", parameterUserID);
            Document[] retvalArray = new Document[dateset.Tables[0].Rows.Count];
            for (var i = 0; i < (int)dateset.Tables[0].Rows.Count; i++)
            {
                retvalArray[i] = new Document();
                retvalArray[i].DocumentID = (string)dateset.Tables[0].Rows[i]["DocumentID"];
                retvalArray[i].DocumentName = (string)dateset.Tables[0].Rows[i]["DocumentName"];
            }
            return retvalArray;
        }

        private bool ExexutegGenericSP(string SPName, string documentID, string userID)
        {
            bool retval;
            var parameterDocID = DALServices.CreateParameter("DocumentID", documentID);
            var parameterUserID = DALServices.CreateParameter("UserID", userID);
            try
            {
                DALServices.ExecuteNonQuery(SPName, parameterDocID, parameterUserID);
                retval = true;
            }
            catch
            {
                retval = false;
            }
            return retval;
        }
        public IDAL DALServices { get; set; }
    }
}
