﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("VegasProData")]
#if VP13
[assembly: AssemblyDescription("for SONY versions (13 and earlier)")]
#else 
[assembly: AssemblyDescription("for MAGIX versions (14 and later)")]
#endif
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("RatinFX")]
[assembly: AssemblyProduct("VegasProData")]
[assembly: AssemblyCopyright("Copyright © RatinFX 2022-2024")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("24124171-3918-4bf5-a07c-3e00807b7134")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.6.1.0")]
[assembly: AssemblyFileVersion("1.6.1.0")]
