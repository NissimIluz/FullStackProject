using ProjectContracts;
using InfraDAL;
using System;
using DALContract;
using InfraAttributes;

namespace MarkerServiceImp
{
    [Register(typeof(IMarkerService))]
    [Policy(Policy.Transient)]
    public class MarkerService : IMarkerService
    {

        public IDAL DALServices { get; set; }
        public string CreateMarker(string docID, string markerType, int markerLocationX1, int markerLocationY1, int markerLocationX2, int markerLocationY2, string markerStrokeColor, string markerFillColor, string userID)
        {
            string markerID = Guid.NewGuid().ToString();
            var parameterDocID = DALServices.CreateParameter("DocumentID", docID);
            var parameterMarkerID = DALServices.CreateParameter("MarkerID", markerID);
            var parameterMarkerType = DALServices.CreateParameter("MarkerType", markerType);
            string markerLocation = "((" + markerLocationX1 + "," + markerLocationY1 + "),(" + markerLocationX2 + "," + markerLocationY2 + "))";
            var parameterMarkerLocation = DALServices.CreateParameter("MarkerLocation", markerLocation);
            var parametermarkerStrokeColor = DALServices.CreateParameter("StrokeColor", markerStrokeColor);
            var parametermarkerFillColor = DALServices.CreateParameter("FillColor", markerFillColor);
            var parameterUserID = DALServices.CreateParameter("UserID", userID);
            try
            {
                DALServices.ExecuteNonQuery("CreateMarker", parameterDocID, parameterMarkerID, parameterMarkerType, parameterMarkerLocation, parametermarkerStrokeColor, parametermarkerFillColor, parameterUserID);
            }
            catch
            {
                markerID = null;
            }
            return markerID;
        }

        public Marker [] GetMarkers(string docID)
        {
            var parameterDocID = DALServices.CreateParameter("DocumentID", docID);
            var dataset = DALServices.ExecuteQuery("GetMarkers", parameterDocID);
            Marker [] retval = new Marker[dataset.Tables[0].Rows.Count];
            for (var i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                retval[i] = new Marker();
                retval[i].MarkerID = (string)dataset.Tables[0].Rows[i]["MarkerID"] ; 
                retval[i].MarkerType = (string)dataset.Tables[0].Rows[i]["MarkerType"]; 
                retval[i].MarkerStrokeColor = (string)dataset.Tables[0].Rows[i]["StrokeColor"]; 
                retval[i].MarkerFillColor = (string)dataset.Tables[0].Rows[i]["FillColor"]; 
                retval[i].UserID = (string)dataset.Tables[0].Rows[i]["UserID"];
                string temp = (string)dataset.Tables[0].Rows[i]["MarkerLocation"];
                temp = temp.Replace("(", "").Replace(")", "").Replace(" ", "");
                var location = temp.Split(',');
                if (int.TryParse(location[0], out int tempNum0) && int.TryParse(location[1], out int tempNum1) && int.TryParse(location[2], out int tempNum2) && int.TryParse(location[3], out int tempNum3))
                {
                    retval[i].X1 = tempNum0;
                    retval[i].Y1 = tempNum1;
                    retval[i].X2 = tempNum2;
                    retval[i].Y2 = tempNum3;
                }
                else
                {
                    retval[i] = null;
                }
            }
            return retval;
        }

        public bool RemoveMarker(string markerID)
        {
            var parametermMrkerID = DALServices.CreateParameter("MarkerID", markerID);
            bool retval;
            try
            {
                DALServices.ExecuteNonQuery("RemoveMarker", parametermMrkerID);
                retval = true;
            }
            catch
            {
                retval = false;
            }
            return retval;
        }
        public bool ChangeColor(string markerID, string markerStrokeColor, string markerFillColor)
        {
            var parameterMarkerID = DALServices.CreateParameter("MarkerID", markerID);
            var parametermarkerStrokeColor = DALServices.CreateParameter("StrokeColor", markerStrokeColor);
            var parametermarkerFillColor = DALServices.CreateParameter("FillColor", markerFillColor);
            var dateset = DALServices.ExecuteQuery("ChangeMarkerColor", parameterMarkerID, parametermarkerStrokeColor, parametermarkerFillColor);
            return dateset.Tables[0].Rows.Count == 1;

        }

       
    }
}