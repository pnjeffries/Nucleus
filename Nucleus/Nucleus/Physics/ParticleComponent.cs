﻿using Nucleus.Base;
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
    /// conditions
    /// </summary>
    public class ParticleComponent : INodeDataComponent
    {
        #region Properties

        /// <summary>
        /// The current, displaced, position of the node
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// The current velocity of the node
        /// </summary>
        public Vector Velocity { get; set; }
        //TODO

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new ParticleComponent at the position of
        /// the specified node
        /// </summary>
        /// <param name="node"></param>
        public ParticleComponent(Node node)
        {
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
