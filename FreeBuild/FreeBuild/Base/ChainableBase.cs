using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Base
{
    /// <summary>
    /// Abstract, generic base class for chainable objects
    /// </summary>
    /// <typeparam name="TPrevious"></typeparam>
    /// <typeparam name="TNext"></typeparam>
    [Serializable]
    public abstract class ChainableBase<TPrevious, TNext> : Unique, IChainable
        where TPrevious : class, IChainable
        where TNext : class, IChainable
    {
        #region Properties

        /// <summary>
        /// The previous item in the chain
        /// </summary>
        public TPrevious Previous { get; private set; }

        IChainable IChainable.Previous { get { return Previous; } }

        /// <summary>
        /// Private backing field for the Next property
        /// </summary>
        private TNext _Next;

        /// <summary>
        /// The next item in the chain
        /// </summary>
        public TNext Next
        {
            get { return _Next; }
            set { _Next = value; NotifyPropertyChanged("Next"); }
        }

        IChainable IChainable.Next
        {
            get { return Next; }
            set { Next = value as TNext; }
        }

        #endregion


    }
}
