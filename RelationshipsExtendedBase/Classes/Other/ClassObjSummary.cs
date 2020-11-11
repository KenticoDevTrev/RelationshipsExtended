namespace RelationshipsExtended
{
    /// <summary>
    /// Internal use only, creates a summary of a Class for processing
    /// </summary>
    public class ClassObjSummary
    {
        public string ClassName;
        public string TableName;
        public string IDColumn;
        public string GUIDColumn;
        public string CodeNameColumn;
        public bool ClassIsDocumentType;
        public bool ClassIsCustomTable;
        public bool ClassIsForm;

        public ClassObjSummary(string ClassName)
        {
            this.ClassName = ClassName;
        }
    }
}
