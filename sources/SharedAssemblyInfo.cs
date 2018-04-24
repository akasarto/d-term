using System.Reflection;
using System.Resources;

// General Info
[assembly: AssemblyProduct("dTerm")]
[assembly: AssemblyCompany("Sarto Research")]
[assembly: AssemblyCopyright("Copyright © 2018 Thiago Alberto Schneider")]
[assembly: NeutralResourcesLanguage("en-US")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

// Version
[assembly: AssemblyVersion("3.1.0.0")]
[assembly: AssemblyFileVersion("3.1.0.0")]