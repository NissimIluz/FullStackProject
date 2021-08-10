using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectContracts
{
    public class Marker
    {
        public string MarkerID { get; set; }
        public string MarkerType { get; set; } 
        public string MarkerStrokeColor { get; set; }
        public string MarkerFillColor { get; set; }
        public string UserID { get; set; }
        public string DocumentID { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
    }
}