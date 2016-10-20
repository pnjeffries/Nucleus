using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using FreeBuild.Geometry;
using FreeBuild.Model;

namespace FreeBuild.Robot
{
    /// <summary>
    /// Helper class to convert from Robot to FreeBuild types
    /// </summary>
    public static class ROBtoFB
    {
        /// <summary>
        /// Convert a Robot 3D point to a FreeBuild Vector
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static Vector Convert(IRobotGeoPoint3D pt)
        {
            return new Vector(pt.X, pt.Y, pt.Z);
        }

        /// <summary>
        /// Convert a Robot bar end offset into a FreeBuild Vector 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector Convert(IRobotBarEndOffsetData offset)
        {
            return new Vector(offset.UX, offset.UY, offset.UZ);
        }

        /// <summary>
        /// Convert a Robot node to a FreeBuild one
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Node Convert(IRobotNode node)
        {
            Node result = new Node(node.X, node.Y, node.Z);
            return result;
        }

        /// <summary>
        /// Extract the position of a Robot Node as a FreeBuild Vector
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Vector PositionOf(IRobotNode node)
        {
            return new Vector(node.X, node.Y, node.Z);
        }

        /// <summary>
        /// Extract the position of a Robot bar end as a FreeBuild Vector,
        /// assembled from the position of the end's node and offset vector.
        /// </summary>
        /// <param name="barEnd">The bar end to extract the position of</param>
        /// <param name="structureNodes">The full collection of nodes within the structure that contains the bar</param>
        /// <returns></returns>
        public static Vector PositionOf(IRobotBarEnd barEnd, IRobotCollection structureNodes)
        {
            IRobotNode node = structureNodes.Get(barEnd.Node);
            return PositionOf(node) + Convert(barEnd.GetOffsetValue());
        }

        /// <summary>
        /// Extract the straight-line geometry of a Robot bar as a FreeBuild line
        /// </summary>
        /// <param name="bar">The bar to extract geometry for</param>
        /// <param name="structureNodes">The full collection of nodes within the structure that contains the bar</param>
        /// <returns></returns>
        public static Line GeometryOf(IRobotBar bar, IRobotCollection structureNodes)
        {
            return new Line(PositionOf(bar.Start, structureNodes), PositionOf(bar.End, structureNodes));
        }

        public static Profile Convert(IRobotBarSectionData section)
        {
            double D = section.GetValue(IRobotBarSectionDataValue.I_BSDV_D); //Section depth
            //section.GetValue(IRobotBarSectionDataValue.
            if (section.ShapeType == IRobotBarSectionShapeType.I_BSST_RECT_FILLED) //Filled rectangle
            {
                //section.
                //RectangularProfile profile = new RectangularProfile(section)
            }
            return null;
        }

        /// <summary>
        /// Convert a Robot structure into a FreeBuild model.
        /// This is a test function and is not the recommended way to import models from Robot.
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public static Model.Model Convert(IRobotStructure structure)
        {
            //TEMP!
            Model.Model result = new Model.Model();

            //Sections
            

            //Nodes
            IRobotCollection nodes = structure.Nodes.GetAll();

            //Bars
            IRobotCollection bars = structure.Bars.GetAll();
            for (int i = 0; i < bars.Count; i++)
            {
                IRobotBar bar = bars.Get(i);
                //Create element:
                LinearElement element = new LinearElement(GeometryOf(bar, nodes));
                result.Add(element);
            }

            return result;
        }
    }
}
