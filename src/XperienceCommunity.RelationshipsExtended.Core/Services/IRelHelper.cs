using RelationshipsExtended.Enums;
using XperienceCommunity.RelationshipsExtended.Classes.Helpers;

namespace XperienceCommunity.RelationshipsExtended.Services
{
    public interface IRelHelper
    {
        Task<IEnumerable<string>> ObjectIdentitiesToCodeNames(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications);
        Task<IEnumerable<Guid>> ObjectIdentitiesToGUIDs(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications);
        Task<IEnumerable<int>> ObjectIdentitiesToIDs(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications);
        Task<string> GetBindingTagsWhere(string bindingClass, string objectIDFieldName, string leftFieldName, string rightFieldName, IEnumerable<object> values, IdentityType identity = IdentityType.ID, ConditionType condition = ConditionType.Any, string? objectIDTableName = null);
        Task<string> GetBindingWhere(string bindingClass, string objectClass, string objectIDFieldName, string leftFieldName, string rightFieldName, IEnumerable<string> values, IdentityType identity = IdentityType.ID, ConditionType condition = ConditionType.Any, string? objectIDTableName = null);
        Task<ClassObjSummary> GetClassObjSummary(string ClassName);
        Task<string> GetContentTagsWhere(IEnumerable<object> values, ConditionType condition = ConditionType.Any, string contentIDTableName = "CMS_ContentItem");
        Task<IEnumerable<string>> TagIdentitiesToCodeNames(IEnumerable<object> tagIdentifications);
        Task<IEnumerable<Guid>> TagIdentitiesToGUIDs(IEnumerable<object> tagIdentifications);
        Task<IEnumerable<int>> TagIdentitiesToIDs(IEnumerable<object> tagIdentifications);
        string GetBracketedColumnName(string Field);
    }
}