using FreeBuild.Geometry;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Selection
{
    /// <summary>
    /// A selection of Linear Elements
    /// </summary>
    public class LinearElementSelection : SelectionViewModel<LinearElementCollection, LinearElement>
    {
        /// <summary>
        /// Get or set the combined name value of the objects in this collection.
        /// If all the objects in this collection have the same name, that name will be returned.
        /// Otherwise the string "[Multi]" will be returned.
        /// Set this property to set the name property of all objects in this collection
        /// </summary>
        public virtual string Name
        {
            get { return (string)CombinedValue(i => i.Name, "[Multi]"); }
            set { foreach (LinearElement item in Selection) item.Name = value; }
        }

        public virtual Curve Geometry
        {
            get
            {
                if (Selection.Count == 1) return Selection[0].Geometry;
                else return null;
            }
        }

        /// <summary>
        /// Set or set the combined value of the section properties of the elsments
        /// within this collection.
        /// </summary>
        public SectionProperty Property
        {
            get { return (SectionProperty)CombinedValue(i => i.Property, null); }
            set
            {
                if (value != null && value is SectionPropertyDummy)
                {
                    SectionPropertyDummy dummy = (SectionPropertyDummy)value;
                    if (dummy.Name.Equals("New..."))
                    {
                        SectionProperty newSection = Selection[0].Model?.Create.SectionProperty(null);
                        value = newSection;
                        NotifyPropertyChanged("AvailableSections");
                    }
                }
                foreach (LinearElement lEl in Selection) lEl.Property = value;
               
                NotifyPropertyChanged("Property");
            }
        }

        /// <summary>
        /// The set of sections which are available to be assigned to the elements
        /// in this collection.
        /// </summary>
        public SectionPropertyCollection AvailableSections
        {
            get
            {
                if (Selection.Count > 0)
                {
                    SectionPropertyCollection result = new SectionPropertyCollection(Selection[0].Model?.Properties.Sections);
                    result.Add(new SectionPropertyDummy("New..."));
                    return result;
                }
                else return null;
            }
        }
    }
}
