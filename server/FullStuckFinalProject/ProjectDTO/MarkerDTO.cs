using ProjectContracts;
using System;
using System.Collections.Generic;
using System.Text;
namespace ProjectDTO
{
    public class MarkerDTO
    {
        public Marker[] MarkersArray { get; set; }
        public Boolean Succesed { get; set; }
        public string  Message  { get; set; }

    }
}
