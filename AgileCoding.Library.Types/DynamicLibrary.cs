using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AgileCoding.Library.Types
{
    public static class DynamicLibrary
    {
        public static List<Type> GetInterfaceTypes<TInterfaceType>(List<string> namespacesToFiler, bool includeOrExcludeFilter)
        {
            List<Type> listOfInterfaceTypes = GetInterfaceTypes<TInterfaceType>();

            if (includeOrExcludeFilter)
            {
                return listOfInterfaceTypes.Where(x => namespacesToFiler.Any(y => x.Namespace.StartsWith(y))).ToList();
            }
            else
            {
                return listOfInterfaceTypes.Where(x => namespacesToFiler.Any(y => !x.Namespace.StartsWith(y))).ToList();
            }
        }

        public static List<Type> GetInterfaceTypes<TInterfaceType>()
        {
            List<Type> listOfInterfaceTypes = new List<Type>();
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(delegate (Assembly assembly)
            {
                listOfInterfaceTypes
                .AddRange(assembly
                            .DefinedTypes
                            .Where((TypeInfo type) => type.ImplementedInterfaces.Any((Type inter) => inter == typeof(TInterfaceType)) &&
                                            !type.IsInterface &&
                                            !type.Namespace.ToLower().EndsWith(".dummy") &&
                                            !type.Name.ToLower().StartsWith("dummy"))
                            .ToList());
            });
            return listOfInterfaceTypes;
        }

        public static List<Type> GetInterfaceTypes<TInterfaceType>(List<Type> listOfRequiredImplementedInterfaceTypes)
        {
            if (listOfRequiredImplementedInterfaceTypes == null)
            {
                listOfRequiredImplementedInterfaceTypes = new List<Type>();
            }

            List<Type> listOfInterfaceTypes = new List<Type>();
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(delegate (Assembly assembly)
                {
                    listOfInterfaceTypes
                    .AddRange(assembly
                                .DefinedTypes
                                .Where(type => type.ImplementedInterfaces.Any((Type inter) => inter == typeof(TInterfaceType)) &&
                                    listOfRequiredImplementedInterfaceTypes.Intersect(type.ImplementedInterfaces).Count() == listOfRequiredImplementedInterfaceTypes.Count &&
                                    !type.IsInterface &&
                                    !type.Namespace.ToLower().EndsWith(".dummy") &&
                                    !type.Name.ToLower().StartsWith("dummy")).ToList());
                });

            return listOfInterfaceTypes;
        }

        public static List<Type> GetInterfaceTypesWithAttributes<TAttribute>() where TAttribute : Attribute
        {
            List<Type> listOfEnumTypes = new List<Type>();
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(delegate (Assembly assembly)
            {
                listOfEnumTypes.AddRange(assembly.DefinedTypes.Where(delegate (TypeInfo type)
                {
                    if (type.CustomAttributes.Any((CustomAttributeData inter) => inter.AttributeType == typeof(TAttribute)) && !type.IsInterface && !type.Name.ToLower().StartsWith("dummy") && !type.Name.ToLower().EndsWith("response"))
                    {
                        return !type.Name.ToLower().StartsWith("request");
                    }
                    return false;
                }).ToList());
            });
            return listOfEnumTypes;
        }
    }
}
