using CMS.DataEngine;
using CMS.DocumentEngine;
using RelationshipsExtended.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RelationshipsExtended.Interfaces
{
    interface IRelationshipExtendedHelper
    {
        #region "Related Pages"

        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="nodeGuid">The NodeGuid that the documents must be related to</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on this relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        DocumentQuery InRelationWithOrder(DocumentQuery baseQuery, Guid nodeGuid, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false);


        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="nodeID">The NodeID</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        DocumentQuery InRelationWithOrder(DocumentQuery baseQuery, int nodeID, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false);


        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="nodeGuid">The NodeGuid</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        DocumentQuery<TDocument> InRelationWithOrder<TDocument>(DocumentQuery<TDocument> baseQuery, Guid nodeGuid, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false) where TDocument : TreeNode, new();


        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="nodeID">The NodeID</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        DocumentQuery<TDocument> InRelationWithOrder<TDocument>(DocumentQuery<TDocument> baseQuery, int nodeID, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false) where TDocument : TreeNode, new();


        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="baseQuery">The Base Document Query</param>
        /// <param name="nodeGuid">The NodeGuid</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        MultiDocumentQuery InRelationWithOrder(MultiDocumentQuery baseQuery, Guid nodeGuid, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false);


        /// <summary>
        /// Filters documents by those in relationship to the given Node using Related Pages.
        /// </summary>
        /// <param name="nodeID">The NodeID</param>
        /// <param name="relationshipName">Name of the relationship. If not provided documents from all relationships will be retrieved.</param>
        /// <param name="Ordered">Order the documents based on relationship.</param>
        /// <param name="AscendingOrder">If the ordering should be ascending or not</param>
        /// <param name="ReverseRelationship">If true, then will filter documents that have the given node as a relationship, vs. filtering documents that are related to the given node.  Ordering is disabled if reversed</param>
        MultiDocumentQuery InRelationWithOrder(MultiDocumentQuery baseQuery, int nodeID, string relationshipName = null, bool Ordered = true, bool AscendingOrder = true, bool ReverseRelationship = false);

        #endregion

        #region "Custom Relationships"

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>        
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.BarNode</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Bar</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: BarID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the Document identity value. Ex: BarNodeNodeID (from Demo.BarNode)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: BarNodeBarID (from Demo.BarNode)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        /// <returns></returns>
        DocumentQuery InCustomRelationshipWithOrder(DocumentQuery baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID);

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>    
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.BarNode</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Bar</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: BarID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the Document identity value. Ex: BarNodeNodeID (from Demo.BarNode)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: BarNodeBarID (from Demo.BarNode)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        DocumentQuery<TDocument> InCustomRelationshipWithOrder<TDocument>(DocumentQuery<TDocument> baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID) where TDocument : TreeNode, new();

        /// <summary>
        /// Filters documents by those in relationship to the given object using a custom binding class.
        /// </summary>    
        /// <example>You want to grab Nodes that are related to a Bar object using the binding table Demo_BarNode</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectNode"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.BarNode</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Bar</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'BarA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: BarID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the Document identity value. Ex: BarNodeNodeID (from Demo.BarNode)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: BarNodeBarID (from Demo.BarNode)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        MultiDocumentQuery InCustomRelationshipWithOrder(MultiDocumentQuery baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID);

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.FooBar</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Foo</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the the primary class's identity value. Ex: FooBarFooID (from Demo.FooBar)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBarBarID (from Demo.FooBar)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order. Ex: FooBarOrder</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        ObjectQuery InCustomRelationshipWithOrder(ObjectQuery baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID);

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.FooBar</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Foo</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the the primary class's identity value. Ex: FooBarFooID (from Demo.FooBar)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBarBarID (from Demo.FooBar)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order. Ex: FooBarOrder</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        ObjectQuery<TObject> InCustomRelationshipWithOrder<TObject>(ObjectQuery<TObject> baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID) where TObject : BaseInfo, new();

        /// <summary>
        /// Filters objects by those in relationship to the given other object using a custom binding class.
        /// </summary>        
        /// <example>If you are Querying Bar's that are related to the given Foo object (Demo.FooBar)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObjectWithOrder"/>
        /// <param name="BindingClass">The Binding Class Code Name Ex: Demo.FooBar</param>
        /// <param name="PrimaryClass">The primary class you are binding to Ex: Demo.Foo</param>
        /// <param name="InRelationshipWithValue">The value of the primary class that is used for the relationship lookup. Ex: 'FooA'</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains the the primary class's identity value. Ex: FooBarFooID (from Demo.FooBar)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBarBarID (from Demo.FooBar)</param>
        /// <param name="OrderColumn">The Order column name, if empty then will not order. Ex: FooBarOrder</param>
        /// <param name="OrderAsc">If the ordering should be done Ascending or Descending</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the Node, default is ID</param>
        MultiObjectQuery InCustomRelationshipWithOrder(MultiObjectQuery baseQuery, string BindingClass, string PrimaryClass, object InRelationshipWithValue, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, string OrderColumn = null, bool OrderAsc = true, IdentityType Identity = IdentityType.ID);

        #endregion

        #region "Document Category Filter"

        /// <summary>
        /// Adds Document Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Document.</param>
        DocumentQuery DocumentCategoryCondition(DocumentQuery baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string DocumentIDTableName = "CMS_Document");
        /// <summary>
        /// Adds Document Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Document.</param>
        DocumentQuery<TDocument> DocumentCategoryCondition<TDocument>(DocumentQuery<TDocument> baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string DocumentIDTableName = "CMS_Document") where TDocument : TreeNode, new();

        /// <summary>
        /// Adds Document Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Document.</param>
        MultiDocumentQuery DocumentCategoryCondition(MultiDocumentQuery baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string DocumentIDTableName = "CMS_Document");

        #endregion

        #region "Node Category Filter"

        /// <summary>
        /// Adds Tree Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="NodeIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Tree.</param>
        DocumentQuery TreeCategoryCondition(DocumentQuery baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string NodeIDTableName = "CMS_Tree");

        /// <summary>
        /// Adds Tree Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="NodeIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Tree.</param>
        DocumentQuery<TDocument> TreeCategoryCondition<TDocument>(DocumentQuery<TDocument> baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string NodeIDTableName = "CMS_Tree") where TDocument : TreeNode, new();

        /// <summary>
        /// Adds Tree Category Condition to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="NodeIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Tree.</param>
        MultiDocumentQuery TreeCategoryCondition(MultiDocumentQuery baseQuery, IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string NodeIDTableName = "CMS_Tree");

        #endregion

        #region "Binding Category Filter"

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.NodeRegion)</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value.  Ex: NodeRegionNodeID (from Demo.NodeRegion)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: NodeRegionCategoryID (from Demo.NodeRegion) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeRegion</param>
        DocumentQuery BindingCategoryCondition(DocumentQuery baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null);

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.NodeRegion)</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value.  Ex: NodeRegionNodeID (from Demo.NodeRegion)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: NodeRegionCategoryID (from Demo.NodeRegion) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeRegion</param>
        DocumentQuery<TDocument> BindingCategoryCondition<TDocument>(DocumentQuery<TDocument> baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null) where TDocument : TreeNode, new();

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the document query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving documents that are related to 'region' categories stored in a custom binding table (Demo.NodeRegion)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeCategoryCustomTable"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.NodeRegion)</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value.  Ex: NodeRegionNodeID (from Demo.NodeRegion)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: NodeRegionCategoryID (from Demo.NodeRegion) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeRegion</param>
        MultiDocumentQuery BindingCategoryCondition(MultiDocumentQuery baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null);

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.FooCategory)</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value.  Ex: FooCategoryFooID (from Demo.FooCategory)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: FooCategoryCategoryID (from Demo.FooCategory) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooCategory</param>
        ObjectQuery BindingCategoryCondition(ObjectQuery baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null);

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.FooCategory)</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value.  Ex: FooCategoryFooID (from Demo.FooCategory)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: FooCategoryCategoryID (from Demo.FooCategory) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooCategory</param>
        ObjectQuery<TObject> BindingCategoryCondition<TObject>(ObjectQuery<TObject> baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null) where TObject : BaseInfo, new();

        /// <summary>
        /// Adds Category Condition (with custom Binding table) to the object query.  If no categories given or none found of the given Values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>If you are retrieving Foo objects that are related to categories stored in a custom binding table (Demo.FooCategory)</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectCategory"/>
        /// <param name="BindingClass">The Binding Class Code Name (ex: Demo.FooCategory)</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value.  Ex: FooCategoryFooID (from Demo.FooCategory)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identity value.  Ex: FooCategoryCategoryID (from Demo.FooCategory) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooCategory</param>
        MultiObjectQuery BindingCategoryCondition(MultiObjectQuery baseQuery, string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null);

        #endregion

        #region "Custom Binding Filter"

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.NodeBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value. Ex: NodeBazNodeID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: NodeBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeBaz</param>
        DocumentQuery BindingCondition(DocumentQuery baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<string> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null);

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.NodeBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value. Ex: NodeBazNodeID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: NodeBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeBaz</param>
        DocumentQuery<TDocument> BindingCondition<TDocument>(DocumentQuery<TDocument> baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<string> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null) where TDocument : TreeNode, new();

        /// <summary>
        /// Adds Binding Condition to the document query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Nodes that have some of the given Baz values in their relationship table Demo_NodeBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/NodeObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.NodeBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: NodeID, DocumentID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value. Ex: NodeBazNodeID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: NodeBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_NodeBaz</param>
        MultiDocumentQuery BindingCondition(MultiDocumentQuery baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<string> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null);

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.FooBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value. Ex: FooBazFooID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames) (ex 'BazA', 'BazB')</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooBaz</param>
        ObjectQuery BindingCondition(ObjectQuery baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<string> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null);

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.FooBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value. Ex: FooBazFooID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames) (ex 'BazA', 'BazB')</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooBaz</param>
        ObjectQuery<TObject> BindingCondition<TObject>(ObjectQuery<TObject> baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<string> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null) where TObject : BaseInfo, new();

        /// <summary>
        /// Adds Binding Condition to the object query.  If no values given or none found of the given values, will not apply a true condition (1=1).
        /// </summary>
        /// <example>You want to find Foos that have some of the given Baz values in their relationship table Demo_FooBaz</example>
        /// <see cref="https://github.com/KenticoDevTrev/RelationshipsExtended/tree/master/Demo/Demo/Classes/ObjectObject"/>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.FooBaz</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Baz</param>
        /// <param name="ObjectIDFieldName">The Field Name of object that matches the binding table's Left Field value. Ex: FooID</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains Object IDs value. Ex: FooBazFooID (from Demo.NodeBaz)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identity value.  Ex: FooBazBazID (from Demo.NodeBaz)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames) (ex 'BazA', 'BazB')</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooBaz</param>
        MultiObjectQuery BindingCondition(MultiObjectQuery baseQuery, string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<string> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null);

        #endregion


        #region "Where Condition Generators"

        /// <summary>
        /// Returns a full where condition (for Document Category Relationships) to be used in filtering (ex repeaters).  
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Document.</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        string GetDocumentCategoryWhere(IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string DocumentIDTableName = "CMS_Document");

        /// <summary>
        /// Returns a full where condition (for Node Category Relationships) to be used in filtering (ex repeaters).  
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="NodeIDTableName">The Table Name/Alias where the NodeID belongs. Only needed for the 'All' Condition, defaults to CMS_Tree.</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        string GetNodeCategoryWhere(IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string NodeIDTableName = "CMS_Tree");

        /// <summary>
        /// Returns a full where condition (for Binding tables that bind an object to Categories) to be used in filtering (ex repeaters).  For property examples we will use Demo.Foo, CMS.Category, and Demo.FooCategory  
        /// </summary>
        /// <param name="BindingClass">The Binding Class Code Name</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID (From Demo.Foo)</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value.  Ex: FooID (from Demo.FooCategory)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identy value.  Ex: CategoryID (from Demo.FooCategory) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_Foo</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        string GetBindingCategoryWhere(string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null);

        /// <summary>
        /// Returns a full where condition (for Binding Tables that bind on any object) to be used in filtering (ex repeaters).  For property exampples, we will assume Demo.Foo, Demo.Bar, and Demo.FooBar
        /// </summary>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.FooBar</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Bar</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID (from Demo.Foo)</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value. Ex: FooID (from Demo.FooBar)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identy value.  Ex: BarID (from Demo.FooBar)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooBar</param>
        /// <returns>The Where Condition, If no object values provided or none found, returns 1=1</returns>
        string GetBindingWhere(string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<string> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null);

        #endregion
    }
}
