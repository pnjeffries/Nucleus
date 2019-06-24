using Microsoft.VisualStudio.DebuggerVisualizers;
using Nucleus.WPF;
using System;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(Nucleus.Geometry.VertexGeometry),
Description = ".NUCLEUS Geometry Visualiser")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(Nucleus.Geometry.VertexGeometryCollection),
Description = ".NUCLEUS Geometry Visualiser")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(Nucleus.Visualiser.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(Nucleus.Geometry.Line),
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
