using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// Enumerated value representing grammatical person
    /// </summary>
    public enum GrammaticalPerson
    {
        /// <summary>
        /// First persion - the speaker - 'I', 'my' etc.
        /// </summary>
        First,

        /// <summary>
        /// Second person - the addressee - 'You', 'your' etc.
        /// </summary>
        Second,

        /// <summary>
        /// Third person - other parties - 'They', 'their' etc.
        /// </summary>
        Third
    }
}
