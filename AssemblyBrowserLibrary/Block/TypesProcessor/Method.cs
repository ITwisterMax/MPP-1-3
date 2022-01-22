using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AssemblyBrowserLibrary.Helper;
using AssemblyBrowserLibrary.Model;

namespace AssemblyBrowserLibrary.Block.TypesProcessor
{
    /// <summary>
    ///     Get methods info
    /// </summary>
    public class Method : GeneralType
    {
        /// <summary>
        ///     Methods info
        /// </summary>
        public MethodBase MethodInfo { get; private set; }

        /// <summary>
        ///     Get methods permission
        /// </summary>
        /// 
        /// <returns>String</returns>
        protected override string GetAccessPermissions()
        {
            if (MethodInfo.IsPrivate)
            {
                return "private";
            }
            if (MethodInfo.IsPublic) 
            {
                return "public";
            }
            if (MethodInfo.IsAssembly) 
            {
                return "internal";
            }
            if (MethodInfo.IsFamilyAndAssembly) 
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

            if (MethodInfo.IsAbstract) 
            {
                accessPermissions |= Permissions.Abstract;
            }
            if (MethodInfo.IsVirtual)
            {
                accessPermissions |= Permissions.Virtual;
            }
            if (MethodInfo.IsStatic)
            {
                accessPermissions |= Permissions.Static;
            }
           
            return accessPermissions;
        }

        /// <summary>
        ///     Get methods parameters
        /// </summary>
        /// 
        /// <returns>Dictionary<string, string></returns>
        private Dictionary<string, string> GetParameters()
        {
            var parameters = new Dictionary<string, string>();

            try
            {
                foreach (ParameterInfo parameterInfo in MethodInfo.GetParameters())
                {
                    parameters.Add(parameterInfo.Name, ConvertTypeName(parameterInfo.ParameterType));
                }

                return parameters;
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        ///     Get methods info
        /// </summary>
        /// 
        /// <param name="data">Method info</param>
        /// 
        /// <returns>DataWrapper</returns>
        public override DataWrapper GetData(MemberInfo data)
        {
            MethodInfo = (MethodBase)data;
            
            var isExtension = false;
            string returnType = string.Empty;
            
            if (data is MethodInfo)
            {
                var method = ((MethodInfo)MethodInfo);
                returnType = ConvertTypeName(method.ReturnType);
                isExtension = (method.GetBaseDefinition().DeclaringType == method.DeclaringType) &&
                    MethodInfo.IsDefined(typeof(ExtensionAttribute));
            }

            return new MethodData(MethodInfo.Name, GetAccessPermissions(), returnType, 
                GetParameters(), GetAccessPermissionsList(), isExtension);
        }
    }

    /// <summary>
    ///     Method data format
    /// </summary>
    public class MethodData : DataWrapper
    {
        /// <summary>
        ///     Return type
        /// </summary>
        public string ReturnType { get; private set; }

        /// <summary>
        ///     Check if method is extention
        /// </summary>
        public bool IsExtension { get; private set; }

        /// <summary>
        ///     Method parameters
        /// </summary>
        public Dictionary<string, string> Parameters { get; private set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="name">Method name</param>
        /// <param name="accessPermissions">Method permission</param>
        /// <param name="returnType">Method return type</param>
        /// <param name="parameters">Method parameters</param>
        /// <param name="accessPermissionsList">Permissions list</param>
        /// <param name="isExtension">Check if method is extention</param>
        public MethodData(string name, string accessPermissions, string returnType, Dictionary<string, string> parameters, Permissions accessPermissionsList, bool isExtension) : 
            base(name, accessPermissions, accessPermissionsList)
        {
            IsExtension = isExtension;
            ReturnType = returnType;
            Parameters = parameters;

            WrapperDeclaration = ToString();
        }

        /// <summary>
        ///     Convert permissions to string
        /// </summary>
        /// 
        /// <returns>String</returns>
        protected override string ConvertPermissions()
        {
            string modifiers = string.Empty;

            if ((AccessPermissionsList & Permissions.Sealed) != 0) 
            {
                modifiers = modifiers + "sealed";
            }
            if ((AccessPermissionsList & Permissions.Abstract) != 0) 
            {
                modifiers = modifiers + "abstract";
            }
            if ((AccessPermissionsList & Permissions.Virtual) != 0) 
            {
                modifiers = modifiers + "virtual";
            }
            if ((AccessPermissionsList & Permissions.Static) != 0) 
            {
                modifiers = modifiers + "static";
            }

            return modifiers;
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
            result += " " + ReturnType;
            result += " " + Name;
            result += "(";

            foreach (var pair in Parameters)
            {
                result += pair.Value + " " + pair.Key + ", ";
            }

            if (Parameters.Count > 0)
            {
                result = result.Remove(result.Length - 2, 2);
            }
            result += ")";

            if (IsExtension)
            {
                result += "(extension method)";
            }
            
            return result;
        }
    }
}
