namespace XperienceCommunity.RelationshipsExtended.Classes.Helpers
{
    /// <summary>
    /// Internal use only, creates a summary of a Class for processing
    /// </summary>
    public class ClassObjSummary
    {
        public string ClassName;
        public string TableName = string.Empty;
        public string IDColumn = string.Empty;
        public string GUIDColumn = string.Empty;
        public string CodeNameColumn = string.Empty;
        public bool ClassIsContentType;

        public ClassObjSummary(string className)
        {
            this.ClassName = className;
        }
    }
}
