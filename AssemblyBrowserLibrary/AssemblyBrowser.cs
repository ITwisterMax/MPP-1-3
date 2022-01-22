using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyBrowserLibrary.Block;
using AssemblyBrowserLibrary.Block.TypesProcessor;
using AssemblyBrowserLibrary.Model;

namespace AssemblyBrowserLibrary
{
    /// <summary>
    ///     Get format assembly result
    /// </summary>
    public class AssemblyBrowser
    {
        /// <summary>
        ///     Processor type
        /// </summary>
        public Processor TypeProcessor { get; private set; }

        /// <summary>
        ///     Result namespaces dictionary
        /// </summary>
        public Dictionary<string, List<DataType>> NamespaceTypesDictionary { get; private set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        public AssemblyBrowser()
        {
            TypeProcessor = new Processor();
            NamespaceTypesDictionary = new Dictionary<string, List<DataType>>();
        }

        /// <summary>
        ///     Try get extension methods
        /// </summary>
        /// 
        /// <param name="type">Type</param>
        /// <param name="methods">Methods list</param>
        /// <param name="indexes">List of indexes</param>
        /// 
        /// <returns>Bool</returns>
        public bool TryGetExtensionMethods(DataType type, out List<MethodData> methods, out List<int> indexes)
        {
            methods = new List<MethodData>();
            indexes = new List<int>();

            for (int j = 0; j < type.Members.Count; j++)
            {
                var member = type.Members[j];

                // Skip not methods
                if (!(member is MethodData) || !((MethodData)member).IsExtension)
                {
                    continue;
                } 

                // Add methods
                methods.Add((MethodData)member);
                indexes.Add(j);
            }

            if (methods.Count > 0) {
                return true;
            }
            
            return false;
        }

        /// <summary>
        ///     Find extensible type by name
        /// </summary>
        /// 
        /// <param name="extensibleType">Name</param>
        /// 
        /// <returns>DataType</returns>
        public DataType FindExtensibleType(string extensibleType)
        {
            foreach (var keyValue in NamespaceTypesDictionary)
            {
                var types = keyValue.Value;

                foreach (var type in keyValue.Value)
                {
                    if (type.Name == extensibleType)
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///     Process extension methods
        /// </summary>
        public void ProcessExtensionMethods()
        {
            foreach (var keyValue in NamespaceTypesDictionary)
            {
                var types = keyValue.Value;
                for (int i = 0; i < types.Count; i++)
                {
                    var type = types[i];

                    // Skip not extension methods
                    if (!type.IsExtension)
                    {
                        continue;
                    }
                    
                    // Replace extension method in neccessary class
                    if (TryGetExtensionMethods(type, out List<MethodData> methods, out List<int> indexes))
                    {
                        type.Members.RemoveAll(elem => methods.Any(newElem => elem == newElem));
                        foreach (var method in methods)
                        {
                            var extensibleType = FindExtensibleType(method.Parameters.Values.First());
                            extensibleType?.Members.Add(method);
                        }

                    }
                }
            }
        }

        /// <summary>
        ///     Get info about assembly
        /// </summary>
        /// 
        /// <param name="path">Path to assembly</param>
        /// 
        /// <returns>List<NamespaceWrapper></returns>
        public List<NamespaceWrapper> GetAssemblyData(string path)
        {
            NamespaceTypesDictionary.Clear();

            // Get all types in assembly
            var assembly = Assembly.LoadFrom(path);
            var assemblyTypes = assembly.GetTypes();

            // Formatting types info
            foreach (var assemblyType in assemblyTypes)
            {
                var typeData = TypeProcessor.GetData(assemblyType);
                if (NamespaceTypesDictionary.TryGetValue(assemblyType?.Namespace ?? "Without namespace", out List<DataType> namespaceTypes))
                {
                    namespaceTypes.Add(typeData);
                }
                else
                {
                    NamespaceTypesDictionary.Add(assemblyType?.Namespace ?? "Without namespace", new List<DataType>() { typeData });
                }
            }

            // Check if method is extensions
            ProcessExtensionMethods();

            // Create result
            var namespaces = new List<NamespaceWrapper>();
            foreach (var pair in NamespaceTypesDictionary)
            {
                namespaces.Add(new NamespaceWrapper(pair.Key, pair.Value));
            }

            return namespaces;
        }
    }
}
