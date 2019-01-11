using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A collection of room templates
    /// </summary>
    public class RoomTemplateCollection : UniquesCollection<RoomTemplate>
    {
        #region Methods

        /// <summary>
        /// Get all templates in this collection of the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RoomTemplateCollection GetAllOfType(RoomType type)
        {
            var result = new RoomTemplateCollection();
            foreach (var room in this)
            {
                if (room.RoomType == type) result.Add(room);
            }
            return result;
        }

        #endregion
    }
}
