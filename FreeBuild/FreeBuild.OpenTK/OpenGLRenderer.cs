using FreeBuild.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using FreeBuild.Geometry;

namespace FreeBuild.OpenTK
{
    public class OpenGLRenderer : Renderer
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenGLRenderer()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the colour of the 
        /// </summary>
        /// <param name="colour"></param>
        public void SetBackground(Colour colour)
        {
            GL.ClearColor(0.25f, 0.25f, 0.25f, 1.0f);
        }

        /// <summary>
        /// Clear the screen in preparation for rendering a new frame
        /// </summary>
        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        /// <summary>
        /// Set the current projection matrix to an orthographic one with the specified dimensions
        /// </summary>
        /// <param name="offsetX">The x-offset of the projection zone</param>
        /// <param name="offsetY">The y-offset of the projection zone</param>
        /// <param name="width">The width of the projection zone</param>
        /// <param name="height">The height of the projection zone</param>
        /// <param name="near">The near cutting plane z-level</param>
        /// <param name="far">The far cutting plane z-level</param>
        public void OrthographicProjection(double width, double height, double offsetX = 0, double offsetY = 0, double near = -1, double far = 1)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(offsetX, offsetX + width, offsetY, offsetY + height, near, far); // Bottom-left corner pixel has coordinate (offsetX, offsetY)
        }

        /// <summary>
        /// Set the current projection matrix to a perspective one with the specified property
        /// </summary>
        /// <param name="fov">The field of view in the y-direction, in radians</param>
        /// <param name="aspect">The aspect ratio of the field of view, width/height</param>
        /// <param name="near">The distance to the near clipping plane</param>
        /// <param name="far">The distance to the far clipping plane</param>
        public void PerspectiveProjection(double fov, double aspect, double near = 0.01, double far = 100)
        {
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)fov, (float)aspect, (float)near, (float)far);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }

        /// <summary>
        /// Set the current view of the model based on the specified parameters
        /// </summary>
        /// <param name="eyeX">The x coordinate of the eye point</param>
        /// <param name="eyeY">The y coordinate of the eye point</param>
        /// <param name="eyeZ">The z coordinate of the eye point</param>
        /// <param name="targetX">The x coordinate of the target point</param>
        /// <param name="targetY">The y coordinate of the target point</param>
        /// <param name="targetZ">The z coordinate of the target point</param>
        /// <param name="upX">The x component of the up vector</param>
        /// <param name="upY">The y component of the up vector</param>
        /// <param name="upZ">The z component of the up vector</param>
        public void SetModelView(Vector eye, Vector target, Vector up)
        {
            Matrix4 lookAt = Matrix4.LookAt((float)eye.X, (float)eye.Y, (float)eye.Z,
                                           (float)target.X, (float)target.Y, (float)target.Z,
                                           (float)up.X, (float)up.Y, (float)up.Z);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookAt);
        }

        /// <summary>
        /// Draw a straight line between two points
        /// </summary>
        /// <param name="pt0">The line start point</param>
        /// <param name="pt1">The line end point</param>
        /// <param name="colour">The colour to draw the line</param>
        /// <param name="thickness">The line thickness</param>
        public void DrawLine(Vector pt0, Vector pt1, Colour colour, float thickness = 1f)
        {
            GL.LineWidth(thickness);
            GL.Color4(FBtoOTK.Convert(colour));
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(FBtoOTK.Convert(pt0));
            GL.Vertex3(FBtoOTK.Convert(pt1));
            GL.End();
        }

        /// <summary>
        /// Draw a point
        /// </summary>
        /// <param name="pt">The point to draw</param>
        /// <param name="size">The point size</param>
        /// <param name="colour">The colour to draw the point</param>
        public void DrawPoint(Vector pt, float size, Colour colour)
        {
            GL.PointSize(size);
            GL.Color4(FBtoOTK.Convert(colour));
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(FBtoOTK.Convert(pt));
            GL.End();
        }

        #endregion
    }
}
