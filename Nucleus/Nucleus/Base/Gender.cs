using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// Enumerated value representing grammatical gender
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Gender-neutral terms - 'its', 'their' etc.
        /// </summary>
        Neutral = 0,

        /// <summary>
        /// Maculine terms - i.e. 'his'
        /// </summary>
        Masculine = 1,

        /// <summary>
        /// Feminine terms - i.e. 'her'
        /// </summary>
        Feminine = 2
    }

    /// <summary>
    /// Static helper functions to aid working with the Gender enum
    /// </summary>
    public static class GenderHelper
    {
        /// <summary>
        /// Get the gender of the specified object (if applicable)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Gender GenderOf(object obj)
        {
            if (obj is Element)
            {
                var dOwner = (Element)obj;
                var eG = dOwner.GetData<ElementGender>();
                if (eG != null) return eG.Gender;
            }
            return Gender.Neutral;
        }
    }

}
