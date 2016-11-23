using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Selection
{
    public class SectionPropertySelection : SelectionViewModel<SectionPropertyCollection, SectionProperty>
    {
        /// <summary>
        /// The primary selected section for individual property display - will be the last selected section
        /// </summary>
        public SectionProperty Section
        {
            get
            {
                if (Selection.Count > 0)
                {
                    SectionProperty result = Selection.Last();
                    //if (result.StructuralModifiers == null) result.StructuralModifiers = new ProfileStructuralModifiers();
                    return result;
                }
                else return null;
            }
            set
            {
                if (value != null)
                {
                    if (Selection.Contains(value.GUID)) Selection.Remove(value);
                    Selection.Add(value);
                }
            }
        }

        /*public Type ProfileType
        {
            get
            {
                if (Section != null) return Section.Profile.GetType();
                return null;
            }
            set
            {
                //Change the section type:
                if (Section != null && value != null && Section.GetType() != value)
                {
                    SectionProfile original = Section;
                    Design design = Document.Design;
                    Selection.Remove(original);
                    Section = ReflectionHelper.ChangeType<SectionProfile>(original, value, design);
                }
            }
        }*/

        /*public IList<Type> AvailableSectionTypes
        {
            get
            {
                return ReflectionHelper.GetSubTypes(typeof(SectionProfile));
            }
        }

        public SectionProfileSelection()
        {

        }

        public SectionProfileSelection(SectionProfile section)
        {
            Section = section;
        }

        public void MonitorElementSelectionSection(StructAnalysis1DElementSelection elementSelection)
        {
            elementSelection.PropertyChanged += HandlesElementPropertyChanged;
        }

        /// <summary>
        /// Handle a change of section profile in the element selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlesElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is StructAnalysis1DElementSelection && (e.PropertyName == null ||
                e.PropertyName == PropertyNames.STRUCTANALYSIS1DELEMENT_SECTIONPROFILE))
            {
                StructAnalysis1DElementSelection elementSelection = (StructAnalysis1DElementSelection)sender;
                object selected = elementSelection.SectionProfile;
                if (selected != null && selected is SectionProfile)
                {
                    Set((SectionProfile)selected);
                }
                else Clear();
            }
        }*/
    }
}
