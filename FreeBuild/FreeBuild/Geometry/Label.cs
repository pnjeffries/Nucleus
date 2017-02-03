using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Geometry
{
    /// <summary>
    /// A geometry object representing a text label
    /// </summary>
    [Serializable]
    public class Label : Point
    {
        #region Properties

        /// <summary>
        /// Private backing field for Text property
        /// </summary>
        private string _Text;

        /// <summary>
        /// The text to be displayed by this label
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { _Text = value;  NotifyPropertyChanged("Text"); }
        }

        /// <summary>
        /// Private backing field for TextHeight property
        /// </summary>
        private double _TextSize = 1.0;

        /// <summary>
        /// The display height of characters in the label 
        /// </summary>
        public double TextSize
        {
            get { return _TextSize; }
            set { _TextSize = value;  NotifyPropertyChanged("TextSize"); }
        }

        /// <summary>
        /// Private backing field for HorizontalSetOut property
        /// </summary>
        private HorizontalSetOut _HorizontalSetOut = HorizontalSetOut.Left;

        /// <summary>
        /// The horizontal set-out of the text relative to the specified position
        /// </summary>
        public HorizontalSetOut HorizontalSetOut
        {
            get { return _HorizontalSetOut; }
            set { _HorizontalSetOut = value;  NotifyPropertyChanged("HorizontalSetOut"); }
        }

        /// <summary>
        /// Privat backing field for VerticalSetOut property
        /// </summary>
        private VerticalSetOut _VerticalSetOut = VerticalSetOut.Top;

        /// <summary>
        /// The vertical set-out of the text relative to the specified position
        /// </summary>
        public VerticalSetOut VerticalSetOut
        {
            get { return _VerticalSetOut; }
            set { _VerticalSetOut = value;  NotifyPropertyChanged("VerticalSetOut"); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Initialises a new Label with no data
        /// </summary>
        public Label() : base()
        { }

        /// <summary>
        /// Initialises a new label with the specified position and text
        /// </summary>
        /// <param name="position">The position the text will start</param>
        /// <param name="text">The text</param>
        /// <param name="textSize">The height of the text</param>
        public Label(Vector position, string text, double textSize = 1.0, 
            VerticalSetOut verticalSetOut = VerticalSetOut.Top, HorizontalSetOut horizontalSetOut = HorizontalSetOut.Left, GeometryAttributes attributes = null) : base(position)
        {
            Text = text;
            TextSize = textSize;
            VerticalSetOut = verticalSetOut;
            HorizontalSetOut = horizontalSetOut;
            Attributes = attributes;
        }

        #endregion
    }
}
