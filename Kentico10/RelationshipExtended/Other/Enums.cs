using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationshipsExtended.Enums
{
    enum SelectorFieldSaveType { ID, GUID, String, CategoryName };
    enum DisplayType { Tree, List };
    enum SaveType { ToField, ToCategory, Both, BothNode, ToJoinTable };
    enum CategoryFieldSaveType { ID, GUID, CategoryName };

}
