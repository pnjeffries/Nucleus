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
    /// conditions.  May be attached to a node.
    /// </summary>
    [Serializable]
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
            Reset();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reset this particle to its initial position
        /// </summary>
        public void Reset()
        {
            if (Node != null)
                Position = Node.Position;
        }

        public void Merge(INodeDataComponent other)
        {
            //TODO?
        }

        /// <summary>
        /// Move this particle according to its current velocity for the
        /// specified time-step
        /// </summary>
        /// <param name="dt"></param>
        public void Move(double dt)
        {
            Position += Velocity * dt;
        }

        /// <summary>
        /// Reduce the velocity of this particle
        /// </summary>
        /// <param name="vFactor">The proportional velocity factor.
        /// Between 0-1.  The current velocity will be multiplied by
        /// this factor.</param>
        public void Damp(double vFactor)
        {
            Velocity *= vFactor;
        }

        /// <summary>
        /// Apply a force to this particle.
        /// This will apply an accelleration.
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForce(Vector force)
        {
            if (Mass > 0)
                Velocity += force / Mass;
            else
                Velocity += force; //TODO: Review?
        }

        #endregion
    }
}
