using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RelationshipsExtended.Enums
{
    public enum ConditionType
    {
        Any, All, None
    }

    public enum IdentityType
    {
        ID, Guid, CodeName
    }


    /// <summary>
    /// Macro Evaluator, used to allow access to the enums through the AccentsEnums.[Enum] macro command.
    /// </summary>
    public static class EnumMacroEvaluator
    {
        // Callback method that provides the value of the 'CallbackField' macro field
        public static object EnumMacroObjects()
        {
            // First create a Dictionary of the EnumName (string) and the DataRow that contains itself
            Dictionary<string, DataRow> EnumTableRows = new Dictionary<string, DataRow>();
            Dictionary<string, int> EnumNameToValues = null;

            EnumNameToValues = new Dictionary<string, int>();
            foreach (var value in EnumUtil.GetValues<ConditionType>())
            {
                EnumNameToValues.Add(value.ToStringRepresentation(), (int)value);
            }
            EnumTableRows.Add("ConditionType", EnumNameValueToDataRow(EnumNameToValues));

            EnumNameToValues = new Dictionary<string, int>();
            foreach (var value in EnumUtil.GetValues<IdentityType>())
            {
                EnumNameToValues.Add(value.ToStringRepresentation(), (int)value);
            }
            EnumTableRows.Add("IdentityType", EnumNameValueToDataRow(EnumNameToValues));

            // Convert to a Table now
            DataTable AllEnumsDT = new DataTable();
            foreach (string key in EnumTableRows.Keys)
            {
                AllEnumsDT.Columns.Add(key, EnumTableRows[key].GetType());
            }
            DataRow AllEnumDR = AllEnumsDT.NewRow();
            foreach (string key in EnumTableRows.Keys)
            {
                AllEnumDR[key] = EnumTableRows[key];
            }

            return AllEnumDR;
        }

        private static DataRow EnumNameValueToDataRow(Dictionary<string, int> EnumNameToValues)
        {
            DataTable EnumDT = new DataTable();
            foreach (string key in EnumNameToValues.Keys)
            {
                EnumDT.Columns.Add(key, 0.GetType());
            }
            DataRow EnumDR = EnumDT.NewRow();
            foreach (string key in EnumNameToValues.Keys)
            {
                EnumDR[key] = EnumNameToValues[key];
            }
            return EnumDR;
        }
    }
    /// <summary>
    /// Used to set Enum Macro values
    /// </summary>
    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
