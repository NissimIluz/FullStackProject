using DALContract;
using ProjectDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectContracts
{
    public interface ISharingService
    {
        bool CreateShare(string documentID, string userID);
        Document[] GetSharedDocuments(string userID);
        bool RemoverShare(string documentID, string userID);
        IDAL DALServices { get; set; }

    }
}
