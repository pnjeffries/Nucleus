# .NUCLEUS
.NET Unifying Class Library for Engineering Utility Software

.Nucleus is an open-source .NET library intended as a generalisable base framework for the development of custom BIM and structural engineering workflow tools.  It forms the core underlying class system of [Salamander 3](https://www.food4rhino.com/app/salamander-3) and several internal Ramboll software projects.

The library provides the following functionality:

- A set of extensions and general-purpose utility classes useful for any .NET project.
- A basic geometry library based around a common vertex-based structure (*Geometry* namespace).
- An extensible, lightweight framework for the representation of BIM data (*Model* namespace).
- A libray of custom WPF-based UI components (*WPF* namespace).
- Various sub-projects to allow the above to interact and be exchanged with different external packages.

Note that the code is in an early stage of development and the architecture may still change considerably.

The core library is designed to avoid any non-standard external dependencies, however the following libraries are utilised within some of the translation sub-projects:

netDXF: https://github.com/haplokuon/netDxf | [LGPL 2.1](https://github.com/haplokuon/netDxf/blob/master/LICENSE)

OsmSharp: https://github.com/OsmSharp/core | [MIT](https://github.com/OsmSharp/core/blob/master/LICENSE.md)

Nominatim.API: https://github.com/f1ana/Nominatim.API | [MIT](https://github.com/f1ana/Nominatim.API/blob/master/LICENSE)

OpenTK: https://github.com/opentk/opentk | [The Open Toolkit Library License](https://github.com/opentk/opentk/blob/master/License.txt)

DotSpatial: https://github.com/DotSpatial/DotSpatial | [MIT](https://github.com/DotSpatial/DotSpatial/blob/master/LICENSE)
