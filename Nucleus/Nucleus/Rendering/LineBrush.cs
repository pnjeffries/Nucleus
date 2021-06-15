using Nucleus.Rendering;
using System;

/// <summary>
/// A brush for styling Shapes Line geometry
/// </summary>
[Serializable]
public class LineBrush : DisplayBrush
{
    #region Properties

    /// <summary>
    /// Private backing field for Colour
    /// </summary>
    private Colour _Colour = Colour.RambollMagenta;

    /// <summary>
    /// The colour of the brush
    /// </summary>
    public Colour Colour
    {
        get { return _Colour; }
        set { _Colour = value; }
    }

    /// <summary>
    /// Private backing field for Thickness
    /// </summary>
    private float _Thickness = 1f;

    /// <summary>
    /// The thickness of the brush in metres
    /// </summary>
    public float Thickness
    {
        get { return _Thickness; }
        set { _Thickness = value; }
    }

    /// <summary>
    /// Private backing field for DashStyle
    /// </summary>
    private DashStyle _DashStyle = null;

    /// <summary>
    /// The line dash style of the brush
    /// </summary>
    public DashStyle DashStyle
    {
        get { return _DashStyle; }
        set { _DashStyle = value; }
    }

    /// <summary>
    /// Private backing field for ShowVertices
    /// </summary>
    private bool _ShowVertices = true;

    /// <summary>
    /// Display a set of meshes to represent the vertices of this line?
    /// </summary>
    public bool ShowVertices
    {
        get { return _ShowVertices; }
        set { _ShowVertices = value; }
    }

    /// <summary>
    /// Get the base colour for this display brush - a single
    /// colour that can be used to represent this brush in cases where
    /// more complex shading is not available.
    /// For LineBrushes this will return the Colour property.
    /// </summary>
    public override Colour BaseColour { get { return Colour; } }

    #endregion

    #region Constructors

    public LineBrush(Colour colour, bool showVertices = true)
    {
        Colour = colour;
        ShowVertices = showVertices;
    }

    public LineBrush(Colour colour, DashStyle dashStyle, bool showVertices = true)
    {
        Colour = colour;
        DashStyle = dashStyle;
        ShowVertices = showVertices;
    }

    public LineBrush(Colour colour, float thickness, bool showVertices = true)
    {
        Colour = colour;
        Thickness = thickness;
        ShowVertices = showVertices;
    }

    public LineBrush(Colour colour, float thickness, DashStyle dashStyle, bool showVertices = true)
    {
        Colour = colour;
        Thickness = thickness;
        DashStyle = dashStyle;
        ShowVertices = showVertices;
    }

    #endregion
}
