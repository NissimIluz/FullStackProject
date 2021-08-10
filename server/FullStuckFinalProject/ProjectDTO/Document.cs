using System.Collections.Generic;

namespace ProjectDTO
{
    public class Document
    {
        private List<string> m_SharedWithUsers = null;
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public List<string> SharedWithUsers
        {
            get { return m_SharedWithUsers; }
            set { m_SharedWithUsers = value; }
        }
    }
}
