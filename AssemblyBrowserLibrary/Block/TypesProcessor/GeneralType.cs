using System;
using System.Reflection;
using AssemblyBrowserLibrary.Helper;
using AssemblyBrowserLibrary.Model;

namespace AssemblyBrowserLibrary.Block.TypesProcessor
{
    /// <summary>
    ///     Get element info
    /// </summary>
    public abstract class GeneralType
    {
        /// <summary>
        ///     Get element permission
        /// </summary>
        /// 
        /// <returns>String</returns>
        protected abstract string GetAccessPermissions();

        /// <summary>
        ///     Get permissions list
        /// </summary>
        /// 
        /// <returns>Permissions</returns>
        protected abstract Permissions GetAccessPermissionsList();

        /// <summary>
        ///     Get element info
        /// </summary>
        /// 
        /// <param name="data">Element info</param>
        /// 
        /// <returns>DataWrapper</returns>
        public abstract DataWrapper GetData(MemberInfo data);

        /// <summary>
        ///     Convert types to string format
        /// </summary>
        /// 
        /// <param name="type">Element type</param>
        /// 
        /// <returns>String</returns>
        protected string ConvertTypeName(Type type)
        {
            // For generic types
            if (type.IsGenericType)
            {
                var representation = string.Empty;
                var nestedTypes = type.GetGenericArguments();
                var result = type.Name.Remove(type.Name.Length - 2, 2) + "<";

                // Get nested types in string format
                foreach (var nestedType in nestedTypes)
                {
                    representation += ConvertTypeName(nestedType) + ", ";
                }

                // Create result string
                representation = representation.Length > 0 ? representation.Remove(representation.Length - 2, 2) : representation;

                return result + representation + ">";
            }
            // Other types
            else
            {
                return type.Name;
            }
        }
    }
}
