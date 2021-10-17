using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A style which holds data about a stage to be generated
    /// </summary>
    [Serializable]
    public class StageStyle : Named
    {
        /// <summary>
        /// The collection of templates to be used to generate rooms
        /// </summary>
        public RoomTemplateCollection Templates { get; } = new RoomTemplateCollection();

        /// <summary>
        /// The chance to generate a door on designated doorway tiles
        /// </summary>
        public double DoorChance { get; set; } = 0.5;

        /// <summary>
        /// The minimum size of loop before two rooms will connect into one another
        /// </summary>
        public int MinLoopSize { get; set; } = 4;

        /// <summary>
        /// Default constructor
        /// </summary>
        public StageStyle() { }

        /// <summary>
        /// Templates constructor
        /// </summary>
        /// <param name="templates"></param>
        public StageStyle(params RoomTemplate[] templates)
        {
            foreach (var template in templates) Templates.Add(template);
        }

        /// <summary>
        /// Templates constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="templates"></param>
        public StageStyle(string name, params RoomTemplate[] templates) : base(name)
        {
            foreach (var template in templates) Templates.Add(template);
        }
    }
}
