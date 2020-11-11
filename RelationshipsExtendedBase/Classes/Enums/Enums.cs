using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationshipsExtended.Enums
{
    public enum SelectorFieldSaveType { ID, GUID, String, CategoryName };
    public enum DisplayType { Tree, List };
    public enum SaveType { ToField, ToCategory, Both, BothNode, ToJoinTable };
    public enum CategoryFieldSaveType { ID, GUID, CategoryName };

}
