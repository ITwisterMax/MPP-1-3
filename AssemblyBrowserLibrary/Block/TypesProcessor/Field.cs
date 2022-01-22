using System.Reflection;
using System.Text;
using AssemblyBrowserLibrary.Helper;
using AssemblyBrowserLibrary.Model;

namespace AssemblyBrowserLibrary.Block.TypesProcessor
{
    /// <summary>
    ///     Get fields info
    /// </summary>
    public class Field : GeneralType
    {
        /// <summary>
        ///     Fields info
        /// </summary>
        public FieldInfo FieldInfo { get; private set; }

        /// <summary>
        ///     Get fields permission
        /// </summary>
        /// 
        /// <returns>String</returns>
        protected override string GetAccessPermissions()
        {
            if (FieldInfo.IsPrivate)
            {
                return "private";
            }
            if (FieldInfo.IsPublic)
            {
                return "public";
            }
            if (FieldInfo.IsAssembly)
            {
                return "internal";
            }
            if (FieldInfo.IsFamilyAndAssembly) 
            {
                return "private protected";
            }

            return "protected internal";
        }

        /// <summary>
        ///     Get permissions list
        /// </summary>
        /// 
        /// <returns>Permissions</returns>
        protected override Permissions GetAccessPermissionsList()
        {
            Permissions accessPermissions = 0;

            if (FieldInfo.IsStatic)
            {
                accessPermissions |= Permissions.Static;
            }
            if (FieldInfo.IsInitOnly) 
            {
                accessPermissions |= Permissions.Readonly;
            }
            
            return accessPermissions;
        }

        /// <summary>
        ///     Get fields info
        /// </summary>
        /// 
        /// <param name="data">Field info</param>
        /// 
        /// <returns>DataWrapper</returns>
        public override DataWrapper GetData(MemberInfo data)
        {
            FieldInfo = (FieldInfo)data;

            return new FieldData(
                FieldInfo.Name, 
                GetAccessPermissions(), 
                ConvertTypeName(FieldInfo.FieldType), 
                GetAccessPermissionsList()
            );
        }
    }

    /// <summary>
    ///     Field data format
    /// </summary>
    public class FieldData : DataWrapper
    {
        /// <summary>
        ///     Field type
        /// </summary>
        public string FieldType { get; private set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// 
        /// <param name="name">Field name</param>
        /// <param name="accessPermissions">Field permission</param>
        /// <param name="fieldType">Field type</param>
        /// <param name="accessPermissionsList">Permissions list</param>
        public FieldData(string name, string accessPermissions, string fieldType, Permissions accessPermissionsList) : 
            base(name, accessPermissions, accessPermissionsList)
        {
            FieldType = fieldType;
            WrapperDeclaration = ToString();
        }

        /// <summary>
        ///     Convert permissions to string
        /// </summary>
        /// 
        /// <returns>String</returns>
        protected override string ConvertPermissions()
        {
            string accessPermissions = string.Empty;

            if ((AccessPermissionsList & Permissions.Static) != 0)
            {
                accessPermissions = accessPermissions + "static";
            }
            if ((AccessPermissionsList & Permissions.Readonly) != 0) 
            {
                accessPermissions = accessPermissions + "readonly";
            } 
            
            return accessPermissions;
        }

        /// <summary>
        ///     Return info in string format
        /// </summary>
        /// 
        /// <returns>String</returns>
        public sealed override string ToString()
        {
            var result = new StringBuilder();

            result.Append(AccessPermissions + " ");
            result.Append(ConvertPermissions() + " ");
            result.Append(FieldType + " ");
            result.Append(Name);

            return result.ToString();
        }
    }
}
