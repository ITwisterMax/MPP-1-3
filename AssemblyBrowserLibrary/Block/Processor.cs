using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AssemblyBrowserLibrary.Block.TypesProcessor;
using AssemblyBrowserLibrary.Helper;
using AssemblyBrowserLibrary.Model;

namespace AssemblyBrowserLibrary.Block
{
    /// <summary>
    ///     Processor for types
    /// </summary>
    public class Processor
    {
        /// <summary>
        ///     Binding flags
        /// </summary>
        private BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | 
            BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        /// <summary>
        ///     Field processor
        /// </summary>
        public GeneralType FieldProcessor { get; private set; }

        /// <summary>
        ///     Method processor
        /// </summary>
        public GeneralType MethodProcessor { get; private set; }

        /// <summary>
        ///     Property processor
        /// </summary>
        public GeneralType PropertyProcessor { get; private set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        public Processor()
        {
            FieldProcessor = new Field();
            MethodProcessor = new Method();
            PropertyProcessor = new Property();
        }

        public Type DataType { get; private set; }

        /// <summary>
        ///     Get type name
        /// </summary>
        /// 
        /// <returns>String</returns>
        private string GetTypeName()
        {
            if (DataType.IsClass && DataType.BaseType.Name == "MulticastDelegate") 
            {
                return "delegate";
            }
            if (DataType.IsClass) 
            {
                return "class";
            }
            if (DataType.IsInterface) 
            {
                return "interface";
            }
            if (DataType.IsEnum) 
            {
                return "enum";
            }
            if (DataType.IsValueType && !DataType.IsPrimitive) 
            {
                return "struct";
            }

            return null;
        }

        /// <summary>
        ///     Get element permission
        /// </summary>
        /// 
        /// <returns>String</returns>
        private string GetAccessPermissions()
        {
            if (DataType.IsNotPublic)
            {
                return "internal";
            }

            return "public";
        }

        /// <summary>
        ///     Get permissions list
        /// </summary>
        /// 
        /// <returns>Permissions</returns>
        private Permissions GetAccessPermissionsList()
        {
            Permissions accessPermissions = 0;

            if (DataType.IsAbstract && DataType.IsSealed)
            {
                return accessPermissions |= Permissions.Static;
            }
            if (DataType.IsAbstract)
            {
                accessPermissions |= Permissions.Abstract;
            }
            
            return accessPermissions;
        }

        /// <summary>
        ///     Get element info
        /// </summary>
        /// 
        /// <param name="type">Element type</param>
        /// 
        /// <returns>DataType</returns>
        public DataType GetData(Type type)
        {
            DataType = type;
            var members = new List<DataWrapper>();
            
            // Get methods
            foreach (var method in DataType.GetMethods(bindingFlags))
            {
                if (!method.IsSpecialName)
                {
                    members.Add(MethodProcessor.GetData(method));
                }
            }

            // Get constructors
            foreach (var constructor in DataType.GetConstructors(bindingFlags))
            {
                members.Add(MethodProcessor.GetData(constructor));
            } 

            // Get fields
            foreach (var field in DataType.GetFields(bindingFlags)) 
            {
                members.Add(FieldProcessor.GetData(field));
            }

            // Get properties
            foreach (var property in DataType.GetProperties(bindingFlags))
            {
                members.Add(PropertyProcessor.GetData(property));
            }
            
            return new DataType(GetTypeName(), members, DataType.IsDefined(typeof(ExtensionAttribute)), 
                DataType.Name, GetAccessPermissions(), GetAccessPermissionsList());
        }
    }
}
