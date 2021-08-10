using DALContract;
using InfraDAL;
using System;

namespace ProjectContracts
{
    public enum MarkerType { circle, rectangle }
    public interface IMarkerService
    {
        /**
         * @param docID 
         * @param markerType
         * @param markerLocationX
         * @param markerLocationY
         * @return MarkerID when process succeeded and null when not
         */
        public string CreateMarker(
            string docID,
            string markerType,
            int markerLocationX1,
            int markerLocationY1,
            int markerLocationX2,
            int markerLocationY2,
            string markerStrokeColor,
            string markerFillColor,
            string userID);
        /**
        * @param markerID 
        * @return true when the process succeeded
        */
        public bool RemoveMarker(string markerID);
        /**docID
       * @param docID 
       * @return array of Markers
       */
        public Marker[] GetMarkers(string docID);
        public bool ChangeColor(string markerID, string markerStrokeColor, string markerFillColor);

        public IDAL DALServices { get; set; }
    }
}