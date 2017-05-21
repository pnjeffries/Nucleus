using FreeBuild.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Undo
{
    /// <summary>
    /// An undo state which will reverse a transformation performed on
    /// a geometric object
    /// </summary>
    public class TransformUndoState : UndoState
    {
        #region Properties

        public override bool IsValid
        {
            get
            {
                return Geometry != null && Transform != null;
            }
        }

        public VertexGeometry Geometry { get; set; }

        public Transform Transform { get; set; }

        private Transform _Inverse = null;

        public Transform Inverse
        {
            get
            {
                if (_Inverse == null) _Inverse = new Transform(Transform.Inverse());
                return _Inverse;
            }
        }

        #endregion

        #region Constructor

        public TransformUndoState(VertexGeometry geometry, Transform transform)
        {
            Geometry = geometry;
            Transform = transform;
        }

        public TransformUndoState(VertexGeometry geometry, Transform transform, Transform inverse) : this(geometry, transform)
        {
            _Inverse = inverse;
        }

        #endregion

        #region Methods

        public override UndoState GenerateRedo()
        {
            return new TransformUndoState(Geometry, Inverse, Transform);
        }

        public override void Restore()
        {
            Geometry.Transform(Inverse);
        }

        #endregion
    }
}
