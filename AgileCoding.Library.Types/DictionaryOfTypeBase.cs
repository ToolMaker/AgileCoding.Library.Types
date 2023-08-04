namespace AgileCoding.Library.Types
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using AgileCoding.Extentions.Activators;
    using AgileCoding.Extentions.Loggers;
    using AgileCoding.Library.Interfaces.Logging;

    internal class DictionaryOfTypeBase
    {
        internal static void GenerateDictionaryOfInstances<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            List<Type> interfaceTypestoUse,
            Func<string, object[]> defaultConstructuorArgs,
            Dictionary<Type, object[]> paramsList,
            Dictionary<TEnumKey, TInterfaceType> dictionaryContiantingEnumTypes)
            where TEnumKey : struct
        {
            interfaceTypestoUse
                .ForEach(delegate (Type typeDerivedFromInterface)
                {
                    TInterfaceType? val;
                    try
                    {
                        val = typeDerivedFromInterface.CreateInstanceWithLogging<TInterfaceType>(logger, paramsList[typeDerivedFromInterface]);
                    }
                    catch (Exception innerException)
                    {
                        throw new ArgumentException(string.Format("Tried to infoke type {0} and got a exception. This can cause because the call didnt contain all paramaters to satisfy the type {0}'s constructur defined. See innerexception for more details", typeDerivedFromInterface.GetType().Name, defaultConstructuorArgs), innerException);
                    }
                    if (val != null)
                    {
                        var propertyInfo = val.GetType().GetProperty(enumPropertyNameOnInterface);
                        if (propertyInfo != null)
                        {
                            var objectValue = propertyInfo.GetValue(val, null);
                            if (objectValue != null)
                            {
                                TEnumKey key = (TEnumKey)objectValue;
                                if (!dictionaryContiantingEnumTypes.ContainsKey(key))
                                {
                                    dictionaryContiantingEnumTypes.Add(key, val);
                                }
                            }
                        }
                    }
                });
        }

        internal static List<Type> PopulateInterfacesToUse<TInterfaceType>(List<Type> interfaceTypestoUse, List<Type>? allImplementingInterfaceTypes = null)
        {
            if (interfaceTypestoUse == null || interfaceTypestoUse.Count == 0)
            {
                interfaceTypestoUse = allImplementingInterfaceTypes == null
                    ? DynamicLibrary.GetInterfaceTypes<TInterfaceType>()
                    : DynamicLibrary.GetInterfaceTypes<TInterfaceType>(allImplementingInterfaceTypes);
            }

            return interfaceTypestoUse;
        }

        internal static void InputValidation<TEnumKey, TInterfaceType>(string enumPropertyNameOnInterface) where TEnumKey : struct
        {
            if (!typeof(TEnumKey).IsEnum)
            {
                throw new ArgumentException("GetCachedDictionaryType requires a enum in the TEnumKey parameter");
            }

            if (!typeof(TInterfaceType).IsInterface)
            {
                throw new ArgumentException("GetCachedDictionaryType requires a interface in the TInterfaceType parameter");
            }
        }

        internal static void InputValidation<TEnumKey>(string enumPropertyNameOnInterface, Type interfaceImplementingEnumPropertyType)
            where TEnumKey : struct
        {
            if (!typeof(TEnumKey).IsEnum)
            {
                throw new ArgumentException("GetCachedDictionaryType requires a enum in the TEnumKey parameter");
            }

            if (!interfaceImplementingEnumPropertyType.IsInterface)
            {
                throw new ArgumentException("GetCachedDictionaryType requires a interface in the TInterfaceType parameter");
            }

            if (interfaceImplementingEnumPropertyType.GetProperty(enumPropertyNameOnInterface) == null)
            {
                throw new ArgumentNullException($"Interface type '{interfaceImplementingEnumPropertyType.Name}' does not implement property with name '{enumPropertyNameOnInterface}'");
            }
        }

        internal static Dictionary<Type, Object[]> PopulateDefaultConstructorParamDictionary(List<Type>? interfaceTypestoUse, Func<string, object[]>? defaultConstGenerationFunc = null, object[]? defaultGlobalConstParams = null)
        {
            Dictionary<Type, Object[]> paramsList = new Dictionary<Type, object[]>();
            if(interfaceTypestoUse == null)
            {
                return paramsList;
            }

            interfaceTypestoUse.ForEach((item) =>
            {
                object[]? paramsToUse = defaultConstGenerationFunc == null
                    ? defaultGlobalConstParams
                    : defaultConstGenerationFunc(item.Name);

                if (paramsToUse != null)
                {
                    paramsList.Add(item, paramsToUse);
                }
            });

            return paramsList;
        }

        internal static void GenerateDictionarOfTypes<TEnumKey, TInterfaceType>(
            ILogger logger,
            string enumPropertyNameOnInterface,
            List<Type> interfaceTypes,
            object[]? defaultConstructuorsArgs,
            Dictionary<Type, object[]> paramsList,
            Dictionary<TEnumKey, Type> dictionaryContiantingEnumTypes)
        where TEnumKey : struct
        {
            logger.WriteVerbose($"{nameof(GenerateDictionarOfTypes)} - Interface types count {interfaceTypes.Count}");

            interfaceTypes
            .ForEach(delegate (Type typeDerivedFromInterface)
            {
                logger.WriteVerbose($"{nameof(GenerateDictionarOfTypes)} - Ready to create instance of type '{typeDerivedFromInterface.Name}'");
                TInterfaceType? val;
                try
                {
                    val = typeDerivedFromInterface.CreateInstanceWithLogging<TInterfaceType>(logger, paramsList[typeDerivedFromInterface]);
                    logger.WriteVerbose($"{nameof(GenerateDictionarOfTypes)} - Done creting instance of type '{typeDerivedFromInterface.Name}'");
                }
                catch (Exception innerException)
                {
                    throw new ArgumentException($"Tried to infoke type '{typeDerivedFromInterface.GetType().Name}' and got a exception. This can cause because the call didnt contain all paramaters to satisfy the type '{typeDerivedFromInterface.GetType().Name}'s constructur defined. See innerexception for more details", innerException);
                }

                logger.WriteVerbose($"{nameof(GenerateDictionarOfTypes)} - Getting Enum Value from property '{enumPropertyNameOnInterface}' on tpye '{typeDerivedFromInterface.Name}'");
                if (val != null)
                {
                    var propertyInfo = val.GetType().GetProperty(enumPropertyNameOnInterface);
                    if (propertyInfo != null)
                    {
                        var objectValue = propertyInfo.GetValue(val, null);
                        if (objectValue != null)
                        {
                            TEnumKey key = (TEnumKey)objectValue;
                            if (!dictionaryContiantingEnumTypes.ContainsKey(key))
                            {
                                dictionaryContiantingEnumTypes.Add(key, typeDerivedFromInterface);
                            }
                            else
                            {
                                logger.WriteVerbose($"{nameof(GenerateDictionarOfTypes)} - Enum Value ALEARDY in indictionary, doing nothing");
                            }
                        }
                    }
                }
            });
        }
    }
}
