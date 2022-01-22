using AssemblyBrowserLibrary.Helper;

namespace AssemblyBrowserLibrary.Model
{
    /// <summary>
    ///     Data wrapper
    /// </summary>
    public abstract class DataWrapper
    {
        /// <summary>
        ///     Element name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Element permission
        /// </summary>
        public string AccessPermissions { get; private set; }

        /// <summary>
        ///     Permissions list
        /// </summary>
        public Permissions AccessPermissionsList { get; private set; }

        /// <summary>
        ///     Data declaration
        /// </summary>
        public string WrapperDeclaration { get; protected set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// 
        /// <param name="name">Element name</param>
        /// <param name="accessPermissions">Element permissions</param>
        /// <param name="accessPermissionsList">Permissions list</param>
        protected DataWrapper(string name, string accessPermissions, Permissions accessPermissionsList)
        {
            Name = name;
            AccessPermissions = accessPermissions;
            AccessPermissionsList = accessPermissionsList;
        }

        /// <summary>
        ///     Convert permissions to string format
        /// </summary>
        /// 
        /// <returns>String</returns>
        protected abstract string ConvertPermissions();

        /// <summary>
        ///     Return info in string format
        /// </summary>
        /// 
        /// <returns>String</returns>
        public abstract override string ToString();
    }
}
