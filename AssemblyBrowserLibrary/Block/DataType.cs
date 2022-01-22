using System.Collections.Generic;
using AssemblyBrowserLibrary.Helper;
using AssemblyBrowserLibrary.Model;

namespace AssemblyBrowserLibrary.Block
{
    /// <summary>
    ///     Data type
    /// </summary>
    public class DataType : DataWrapper
    {
        /// <summary>
        ///     Element type name
        /// </summary>
        public string TypeName { get; private set; }

        /// <summary>
        ///     Members
        /// </summary>
        public List<DataWrapper> Members { get; private set; }

        /// <summary>
        ///     Check if element is extention
        /// </summary>
        public bool IsExtension { get; private set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// 
        /// <param name="typeName">Element type name</param>
        /// <param name="members">Members</param>
        /// <param name="isExtension">Check if element is extention</param>
        /// <param name="name">Element name</param>
        /// <param name="accessPermissions">Element permissions</param>
        /// <param name="accessPermissionsList">Permissions list</param>
        public DataType(string typeName, List<DataWrapper> members, bool isExtension, string name, string accessPermissions, Permissions accessPermissionsList) : 
            base(name, accessPermissions, accessPermissionsList)
        {
            TypeName = typeName;
            Members = members;
            IsExtension = isExtension;

            WrapperDeclaration = ToString();
        }

        /// <summary>
        ///     Convert permissions to string format
        /// </summary>
        /// 
        /// <returns>String</returns>
        protected override string ConvertPermissions()
        {
            if ((AccessPermissionsList & Permissions.Abstract) != 0)
            {
                return "abstract";
            }

            if ((AccessPermissionsList & Permissions.Static) != 0)
            {
                return "static";
            }

            return "";
        }

        /// <summary>
        ///     Return info in string format
        /// </summary>
        /// 
        /// <returns>String</returns>
        public sealed override string ToString()
        {
            string result = string.Empty;

            result += AccessPermissions;
            result += " " + ConvertPermissions();
            result += " " + TypeName;
            result += " " + Name;
            
            return result;
        }
    }
}
