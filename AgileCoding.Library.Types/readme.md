AgileCoding.Library.Types
=========================

Introduction
------------

The AgileCoding.Library.Types is a .NET 6.0 package that provides utilities to handle types and reflections in a more comprehensive manner. This library is authored by Ernie Gunning. It is available as a NuGet package version 2.0.5.

This library provides classes such as `DictionaryOfInstances`, `DictionaryOfTypes`, and `DynamicLibrary`. These classes can generate dictionaries containing instances or types based on certain conditions and dynamically retrieve interface types. These utilities can be beneficial for complex applications dealing with dynamic types and requiring flexible handling of types and instances.

Getting Started
---------------

1.  Install the NuGet package into your project.

bashCopy code

`Install-Package AgileCoding.Library.Types -Version 2.0.5`

1.  Add the following using directive in your class file:

csharpCopy code

`using AgileCoding.Library.Types;`

Key Features
------------

-   Create a dictionary of instances that implements interfaces with certain properties. This can be useful for dynamic dependency injection or instance retrieval based on certain conditions.

-   Generate a dictionary containing types based on enum properties on the interface. This can be beneficial in scenarios where types are associated with specific enums.

-   Retrieve interface types dynamically from the currently loaded assemblies.

Sample Usage
------------

### Creating Dictionary of Instances

```csharp
Dictionary<MyEnum, IMyInterface> myDictionary = DictionaryOfInstances.CreateDictionaryOfInstancesThatImplementsEnumInterfaces<MyEnum, IMyInterface>(...parameters);
```

### Getting Interface Types Dynamically

```csharp
List<Type> myTypes = DynamicLibrary.GetInterfaceTypes<IMyInterface>();
```

Documentation
-------------

For detailed information about the usage and API references, please visit the [Wiki](https://github.com/ToolMaker/AgileCoding.Library.Types/wiki).

Source Code
-----------

You can find the source code of the library on [GitHub](https://github.com/ToolMaker/AgileCoding.Library.Types).

License
-------

Please see the `LICENSE` file distributed with this package. By installing and using this package, you agree to the terms and conditions of this license.

Contributions and Issues
------------------------

Contributions are welcome. Please submit a pull request on GitHub. If you encounter any issues or have questions, please post them on the GitHub repository.