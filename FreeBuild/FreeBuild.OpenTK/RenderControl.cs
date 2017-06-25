using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;

namespace Nucleus.OpenTK
{
    public partial class RenderControl : GLControl
    {
        public RenderControl() : base() // Could set up specialised GraphicsMode here
        {
            InitializeComponent();
        }
    }
}
