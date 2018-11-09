using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.xBIM
{
    public class IFCWriter
    {
        public void Write()
        {
            var editor = new XbimEditorCredentials
            {
                ApplicationDevelopersName = "xBIM Team",
                ApplicationFullName = "xBIM Toolkit",
                ApplicationIdentifier = "xBIM",
                ApplicationVersion = "4.0",
                EditorsFamilyName = "Santini Aichel",
                EditorsGivenName = "Johann Blasius",
                EditorsOrganisationName = "Independent Architecture"
            };
        }
    }
}
