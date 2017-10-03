using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Physics
{
    /// <summary>
    /// A data component that stores information necessary to simulate a particle subject to physical
    /// conditions.  May be attached to a node.
    /// </summary>
    public class Particle : INodeDataComponent
    {
        #region Properties

        /// <summary>
        /// The node to which this particle is attached
        /// </summary>
        public Node Node { get; set; }

        /// <summary>
        /// The current, displaced, position of the node
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// The current velocity of the node
        /// </summary>
        public Vector Velocity { get; set; }

        /// <summary>
        /// The mass of the node
        /// </summary>
        public double Mass { get; set; } = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new ParticleComponent at the position of
        /// the specified node
        /// </summary>
        /// <param name="node"></param>
        public Particle(Node node)
        {
            Node = node;
            Position = node.Position;
        }

        #endregion

        #region Methods

        public void Merge(INodeDataComponent other)
        {
            //TODO?
        }

        #endregion
    }
}
