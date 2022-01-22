using System.Collections.Generic;
using AssemblyBrowserLibrary.Block;

namespace AssemblyBrowserLibrary.Model
{
    /// <summary>
    ///     Namespace wrapper
    /// </summary>
    public class NamespaceWrapper
    {
        /// <summary>
        ///     Namespace name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Elements types
        /// </summary>
        public List<DataType> Types { get; private set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="name">Namespace name</param>
        /// <param name="types">Elements types</param>
        public NamespaceWrapper(string name, List<DataType> types)
        {
            Name = name;
            Types = types;
        }
    }
}
