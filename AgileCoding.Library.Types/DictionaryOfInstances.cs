namespace AgileCoding.Library.Types
{
    using System;
    using System.Collections.Generic;
    using AgileCoding.Library.Interfaces.Logging;
    using AgileCoding.Library.Types.Exceptions;

    public static class DictionaryOfInstances
    {
        /// <summary>
        /// Creates a dictionary of instances that implements both TInterfaceType AND interfaceImplementingEnumPropertyType intefraces. The TEnum should be defined in
        /// enumPropertyNameOnInterface with name set to enumPropertyNameOnInterface for this function to generate a dictionary.
        /// </summary>
        /// <typeparam name="TEnumKey">Enum to be used as key</typeparam>
        /// <typeparam name="TInterfaceType">Interface type to be used as Value</typeparam>
        /// <param name="enumPropertyNameOnInterface">Property name of Enum. Tip: Use nameof instead of ToString</param>
        /// <param name="interfaceImplementingEnumPropertyType">Interface type that implements the enum un property name enumPropertyNameOnInterface</param>
        /// <param name="interfaceTypestoUse">List of interfaces to use. If list is null or empty then we scan acress all included assemblies</param>
        /// <param name="defaultConstructuorArgs">your constructor arguments</param>
        /// <returns>dictionary of instances with the enum as key</returns>
        public static Dictionary<TEnumKey, TInterfaceType> CreateDictionaryOfInstancesThatImplmentsENUMInterfaces<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            Type interfaceImplementingEnumPropertyType,
            List<Type> interfaceTypestoUse,
            params object[] defaultConstructuorArgs)
        where TEnumKey : struct
        {
            return CreateDictionaryOfInstancesThatImplmentsENUMInterfacesBase<TEnumKey, TInterfaceType>(logger, enumPropertyNameOnInterface, interfaceImplementingEnumPropertyType, interfaceTypestoUse, null, defaultConstructuorArgs);
        }

        public static Dictionary<TEnumKey, TInterfaceType> CreateDictionaryOfInstancesThatImplmentsENUMInterfaces<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            Type interfaceImplementingEnumPropertyType,
            List<Type> interfaceTypestoUse,
            Func<string, object[]> defaultConstFuncGeneratorFunc)
        where TEnumKey : struct
        {
            return CreateDictionaryOfInstancesThatImplmentsENUMInterfacesBase<TEnumKey, TInterfaceType>(logger, enumPropertyNameOnInterface, interfaceImplementingEnumPropertyType, interfaceTypestoUse, defaultConstFuncGeneratorFunc);
        }

        /// <summary>
        /// Creates a dictionary of instances that implements both TInterfaceType AND interfaceImplementingEnumPropertyType intefraces. The TEnum should be defined in
        /// enumPropertyNameOnInterface with name set to enumPropertyNameOnInterface for this function to generate a dictionary.
        /// </summary>
        /// <typeparam name="TEnumKey">Enum to be used as key</typeparam>
        /// <typeparam name="TInterfaceType">Interface type to be used as Value</typeparam>
        /// <param name="enumPropertyNameOnInterface">Property name of Enum. Tip: Use nameof instead of ToString</param>
        /// <param name="interfaceImplementingEnumPropertyType">Interface type that implements the enum un property name enumPropertyNameOnInterface</param>
        /// <param name="interfaceTypestoUse">List of interfaces to use. If list is null or empty then we scan acress all included assemblies</param>
        /// <param name="defaultConstFuncGeneratorFunc">Fucntion to generat your constructor arguments per type</param>
        /// <returns>dictionary of instances with the enum as key</returns>
        private static Dictionary<TEnumKey, TInterfaceType> CreateDictionaryOfInstancesThatImplmentsENUMInterfacesBase<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            Type interfaceImplementingEnumPropertyType,
            List<Type> interfaceTypestoUse,
            Func<string, object[]> defaultConstFuncGeneratorFunc = null,
            params object[] defaultConstructuorsArgs)
        where TEnumKey : struct
        {
            Dictionary<TEnumKey, TInterfaceType> dictionaryContiantingEnumTypes = null;

            try
            {
                DictionaryOfTypeBase.InputValidation<TEnumKey>(enumPropertyNameOnInterface, interfaceImplementingEnumPropertyType);
                List<Type> allImplementingInterfaceTypes = new List<Type>();
                allImplementingInterfaceTypes.Add(typeof(TInterfaceType));
                allImplementingInterfaceTypes.Add(interfaceImplementingEnumPropertyType);

                interfaceTypestoUse = DictionaryOfTypeBase.PopulateInterfacesToUse<TInterfaceType>(interfaceTypestoUse, allImplementingInterfaceTypes);
                Dictionary<Type, Object[]> paramsList = DictionaryOfTypeBase.PopulateDefaultConstructorParamDictionary(interfaceTypestoUse, defaultConstFuncGeneratorFunc, defaultConstructuorsArgs);

                dictionaryContiantingEnumTypes = new Dictionary<TEnumKey, TInterfaceType>();

                if (interfaceTypestoUse.Count == 0)
                {
                    throw new Exception($"Unable to create a dictionary of type {typeof(TInterfaceType)}. No assemblies referenced contains this type. If you are sure you are referencing the type please make sure the type is used somewhere before the compiler will include it in compile time.");
                }

                DictionaryOfTypeBase.GenerateDictionaryOfInstances(logger, enumPropertyNameOnInterface, interfaceTypestoUse, defaultConstFuncGeneratorFunc, paramsList, dictionaryContiantingEnumTypes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dictionaryContiantingEnumTypes;
        }

        /// <summary>
        /// Creates a dictionary of instances that implements TInterfaceType with a TEnum property named enumPropertyNameOnInterface
        /// </summary>
        /// <typeparam name="TEnumKey">Enum to be used as key</typeparam>
        /// <typeparam name="TInterfaceType">Interface that implements the TEnum</typeparam>
        /// <param name="enumPropertyNameOnInterface">Name of property on interface thet implements TEnum</param>
        /// <param name="interfaceTypestoUse">List of interfaces to use. If list is null or empty then we scan acress all included assemblies</param>
        /// <param name="defaultConstructuorArgs">your constructor arguments</param>
        /// <returns></returns>
        private static Dictionary<TEnumKey, TInterfaceType> CreateDictionaryOfInstancesFromEnumPropertyBase<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            List<Type> interfaceTypestoUse,
            Func<string, object[]> defaultConstFuncGenerator = null,
            params object[] defaultConstructuorsArgs)
        where TEnumKey : struct
        {
            Dictionary<TEnumKey, TInterfaceType> dictionaryContiantingEnumTypes = null;

            try
            {
                DictionaryOfTypeBase.InputValidation<TEnumKey, TInterfaceType>(enumPropertyNameOnInterface);

                interfaceTypestoUse = DictionaryOfTypeBase.PopulateInterfacesToUse<TInterfaceType>(interfaceTypestoUse);
                Dictionary<Type, Object[]> paramsList = DictionaryOfTypeBase.PopulateDefaultConstructorParamDictionary(interfaceTypestoUse, defaultConstFuncGenerator, defaultConstructuorsArgs);

                dictionaryContiantingEnumTypes = new Dictionary<TEnumKey, TInterfaceType>();

                if (interfaceTypestoUse.Count == 0)
                {
                    throw new TypeNotFoundException($"Unable to create a dictionary of type {typeof(TInterfaceType)}. No assemblies referenced contains this type. If you are sure you are referencing the type please make sure the type is used somewhere before the compiler will include it in compile time.");
                }

                DictionaryOfTypeBase.GenerateDictionaryOfInstances(logger, enumPropertyNameOnInterface, interfaceTypestoUse, defaultConstFuncGenerator, paramsList, dictionaryContiantingEnumTypes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dictionaryContiantingEnumTypes;
        }

        /// <summary>
        /// Creates a dictionary of instances that implements TInterfaceType with a TEnum property named enumPropertyNameOnInterface
        /// </summary>
        /// <typeparam name="TEnumKey">Enum to be used as key</typeparam>
        /// <typeparam name="TInterfaceType">Interface that implements the TEnum</typeparam>
        /// <param name="enumPropertyNameOnInterface">Name of property on interface thet implements TEnum</param>
        /// <param name="interfaceTypestoUse">List of interfaces to use. If list is null or empty then we scan acress all included assemblies</param>
        /// <param name="defaultConstFuncGeneratorFunc">your function to generate constructor arguments per type</param>
        /// <returns></returns>
        public static Dictionary<TEnumKey, TInterfaceType> CreateDictionaryOfInstancesFromEnumProperty<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            List<Type> interfaceTypestoUse,
            Func<string, object[]> defaultConstFuncGeneratorFunc)
        where TEnumKey : struct
        {
            return CreateDictionaryOfInstancesFromEnumPropertyBase<TEnumKey, TInterfaceType>(logger, enumPropertyNameOnInterface, interfaceTypestoUse, defaultConstFuncGeneratorFunc);
        }

        public static Dictionary<TEnumKey, TInterfaceType> CreateDictionaryOfInstancesFromEnumProperty<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            List<Type> interfaceTypestoUse,
            params object[] generateDefaultConstArgs)
        where TEnumKey : struct
        {
            return CreateDictionaryOfInstancesFromEnumPropertyBase<TEnumKey, TInterfaceType>(logger, enumPropertyNameOnInterface, interfaceTypestoUse, null, generateDefaultConstArgs);
        }
    }
}
