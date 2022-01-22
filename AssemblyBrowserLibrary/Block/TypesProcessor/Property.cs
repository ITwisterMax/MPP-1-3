using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyBrowserLibrary.Helper;
using AssemblyBrowserLibrary.Model;

namespace AssemblyBrowserLibrary.Block.TypesProcessor
{
    /// <summary>
    ///     Get properties info
    /// </summary>
    public class Property : GeneralType
    {
        /// <summary>
        ///     Property info
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        ///     Get property permission
        /// </summary>
        /// 
        /// <returns>String</returns>
        protected override string GetAccessPermissions()
        {
            var accessor = PropertyInfo.GetAccessors(true)[0];

            if (accessor.IsPrivate) 
            {
                return "private";
            }
            if (accessor.IsPublic) 
            {
                return "public";
            }
            if (accessor.IsAssembly) 
            {
                return "internal";
            }
            if (accessor.IsFamilyAndAssembly)
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
            var accessor = PropertyInfo.GetAccessors(true)[0];

            if (accessor.IsAbstract) 
            {
                accessPermissions |= Permissions.Abstract;
            }
            if (accessor.IsVirtual) 
            {
                accessPermissions |= Permissions.Virtual;
            }
            if (accessor.IsStatic) 
            {
                accessPermissions |= Permissions.Static;
            }

            return accessPermissions;
        }

        /// <summary>
        ///     Get properties info
        /// </summary>
        /// 
        /// <param name="data">Property info</param>
        /// 
        /// <returns>DataWrapper</returns>
        public override DataWrapper GetData(MemberInfo data)
        {
            PropertyInfo = (PropertyInfo)data;

            return new PropertyData(PropertyInfo.Name, GetAccessPermissions(),
                ConvertTypeName(PropertyInfo.PropertyType), GetAccessPermissionsList(), PropertyInfo.GetAccessors(true));
        }
    }

    /// <summary>
    ///     Property data format
    /// </summary>
    public class PropertyData : DataWrapper
    {
        /// <summary>
        ///     Property type
        /// </summary>
        public string PropertyType { get; private set; }

        /// <summary>
        ///     Access permissions
        /// </summary>
        public MethodInfo[] Accessors { get; private set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// 
        /// <param name="name">Property name</param>
        /// <param name="accessPermissions">Property permissions</param>
        /// <param name="propertyType">Property type</param>
        /// <param name="accessPermissionsList">Permissions list</param>
        /// <param name="accessors">Access permissions</param>
        public PropertyData(string name, string accessPermissions, string propertyType, Permissions accessPermissionsList, MethodInfo[] accessors) : 
            base(name, accessPermissions, accessPermissionsList)
        {
            PropertyType = propertyType;
            Accessors = accessors;

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

            if ((AccessPermissionsList & Permissions.Sealed) != 0) 
            {
                accessPermissions = accessPermissions + "sealed";
            }
            if ((AccessPermissionsList & Permissions.Abstract) != 0) 
            {
                accessPermissions = accessPermissions + "abstract";
            }
            if ((AccessPermissionsList & Permissions.Virtual) != 0) 
            {
                accessPermissions = accessPermissions + "virtual";
            }
            if ((AccessPermissionsList & Permissions.Static) != 0) 
            {
                accessPermissions = accessPermissions + "static";
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
            StringBuilder result = new StringBuilder();

            result.Append(AccessPermissions + " ");
            result.Append(ConvertPermissions());
            result.Append(PropertyType + " ");
            result.Append(Name);
            result.Append(" { ");

            string acc = string.Empty;
            foreach (var accessor in Accessors)
            {
                if (accessor.IsSpecialName)
                {
                    if (accessor.IsPrivate)
                    {
                        acc += "private ";
                    }
                    acc += accessor.Name;
                    acc += ", ";
                }
            }

            if (acc.Length > 0)
            {
                acc = acc.Remove(acc.Length - 2, 2);
            }
            result.Append(acc);
            result.Append(" } ");

            return result.ToString();
        }

    }
}
