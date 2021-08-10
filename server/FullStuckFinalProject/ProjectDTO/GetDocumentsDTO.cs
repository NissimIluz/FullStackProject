using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDTO
{
    public class GetDocumentsDTO
    {
        public Document[] SharedWithUser { get; set; }
        public Document[] ownDocuments { get; set; }
        public Document[] SharedByUser { get; set; }
    }
}
