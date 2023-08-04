namespace AgileCoding.Library.Types
{
    using System;
    using System.Collections.Generic;
    using AgileCoding.Extentions.Loggers;
    using AgileCoding.Library.Interfaces.Logging;
    using AgileCoding.Library.Types.Exceptions;

    public static class DictionaryOfTypes
    {
        private static Dictionary<TEnumKey, Type> CreateDictionaryOfTypesFromEnumPropertyBase<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            List<Type> interfaceTypes,
            Func<string, object[]>? defaultConstFuncGeneratorFunc = null,
            object[]? defaultConstructuorsArgs = null) where TEnumKey : struct
        {
            Dictionary<TEnumKey, Type>? dictionaryContiantingEnumTypes = null;

            try
            {
                DictionaryOfTypeBase.InputValidation<TEnumKey, TInterfaceType>(enumPropertyNameOnInterface);

                interfaceTypes = DictionaryOfTypeBase.PopulateInterfacesToUse<TInterfaceType>(interfaceTypes);
                logger.WriteVerbose($"Picked up a total of {interfaceTypes.Count} interfaces.");
                Dictionary<Type, Object[]> paramsList = DictionaryOfTypeBase.PopulateDefaultConstructorParamDictionary(interfaceTypes, defaultConstFuncGeneratorFunc, defaultConstructuorsArgs);
                logger.WriteVerbose($"Defulat Constructor Paramertlist Created");
                dictionaryContiantingEnumTypes = new Dictionary<TEnumKey, Type>();

                if (interfaceTypes.Count == 0)
                {
                    throw new TypeNotFoundException($"Unable to create a dictionary of type {typeof(TInterfaceType)}. No assemblies referenced contains this type. If you are sure you are referencing the type please make sure the type is used somewhere before the compiler will include it in compile time.");
                }
                logger.WriteVerbose($"Creating Dictionary of Types");
                DictionaryOfTypeBase.GenerateDictionarOfTypes<TEnumKey, TInterfaceType>(logger, enumPropertyNameOnInterface, interfaceTypes, defaultConstructuorsArgs, paramsList, dictionaryContiantingEnumTypes);
            }
            catch (Exception)
            {
                throw;
            }
            return dictionaryContiantingEnumTypes;
        }

        public static Dictionary<TEnumKey, Type> CreateDictionaryOfTypesFromEnumProperty<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            List<Type> interfaceTypes,
            params object[] defaultConstructuorArgs) where TEnumKey : struct
        {
            return CreateDictionaryOfTypesFromEnumPropertyBase<TEnumKey, TInterfaceType>(logger, enumPropertyNameOnInterface, interfaceTypes, null, defaultConstructuorArgs);
        }

        public static Dictionary<TEnumKey, Type> CreateDictionaryOfTypesFromEnumProperty<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            List<Type> interfaceTypes,
            Func<string, object[]> defaultConstFuncGeneratorFunc) where TEnumKey : struct
        {
            return CreateDictionaryOfTypesFromEnumPropertyBase<TEnumKey, TInterfaceType>(logger, enumPropertyNameOnInterface, interfaceTypes, defaultConstFuncGeneratorFunc);
        }
    }
}
