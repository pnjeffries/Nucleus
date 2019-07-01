using Microsoft.VisualStudio.DebuggerVisualizers;
using Nucleus.Geometry;
using Nucleus.WPF;
using System;
using System.Collections.Generic;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(Nucleus.Geometry.VertexGeometry),
Description = ".NUCLEUS Geometry Visualiser")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(IList<VertexGeometry>),
Description = ".NUCLEUS Geometry Visualiser")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(VertexGeometryCollection),
Description = ".NUCLEUS Geometry Visualiser")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(CurveCollection),
Description = ".NUCLEUS Geometry Visualiser")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(MeshFaceCollection),
Description = ".NUCLEUS Geometry Visualiser")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(MeshFace),
Description = ".NUCLEUS Geometry Visualiser")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(VertexCollection),
Description = ".NUCLEUS Geometry Visualiser")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(IList<Vector>),
Description = ".NUCLEUS Geometry Visualiser")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(Vector[]),
Description = ".NUCLEUS Geometry Visualiser")]

namespace Nucleus.Visualiser
{
   
    public class DebuggerSide : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            try
            { 
                GeometryVisualiserDialog.ShowDialog(objectProvider.GetObject());
            }
            catch
            {

            }
        }
    }
}
