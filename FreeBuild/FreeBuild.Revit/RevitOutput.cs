using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AD = Autodesk.Revit.DB;
using FB = FreeBuild.Geometry;

namespace FreeBuild.Revit
{
    /// <summary>
    /// Static output Revit helper functions
    /// </summary>
    public static class RevitOutput
    {
        public static Document Document
        {
            get
            {
                throw new NotFiniteNumberException();
                //TODO: return Host.Instance.RevitDocument;
            }
        }

        /// <summary>
        /// Create a new structural member in the specified Revit document
        /// </summary>
        /// <param name="curve">The set-out curve of the member</param>
        /// <param name="symbol">The family symbol of the member to create</param>
        /// <param name="document">The document to add the member to</param>
        /// <param name="distanceUnit">The distance unit the passed-in curve is expressed in</param>
        /// <returns></returns>
        public static FamilyInstance CreateStructuralFraming(FB.Curve curve, FamilySymbol symbol, StructuralType structuralType)
        {
            FamilyInstance instance = CreateFamilyInstance(symbol, structuralType);

            AD.Curve rCrv = FBtoRevit.Convert(curve);
            if (rCrv != null)
            {
                LocationCurve location = instance.Location as LocationCurve;
                location.Curve = rCrv;
            }
            return instance;
        }

        /// <summary>
        /// Create a new column member in the specified Revit document
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="symbol"></param>
        /// <param name="document"></param>
        /// <param name="distanceUnit"></param>
        /// <returns></returns>
        public static FamilyInstance CreateColumn(FB.Curve curve, FamilySymbol symbol)
        {
            FamilyInstance instance = CreateStructuralFraming(curve, symbol, StructuralType.Column);
            instance.SetParameter(BuiltInParameter.SLANTED_COLUMN_TYPE_PARAM, (int)SlantedOrVerticalColumnType.CT_EndPoint);
            return instance;
        }

        /// <summary>
        /// Create an instance of the specified family, located at the origin
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="structuralType"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public static FamilyInstance CreateFamilyInstance(FamilySymbol symbol, StructuralType structuralType)
        {
            if (!symbol.IsActive) symbol.Activate();
            return Document.Create.NewFamilyInstance(new XYZ(0, 0, 0), symbol, structuralType);
        }

        /// <summary>
        /// Get a Revit FamilyInstance by it's element ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static FamilyInstance GetInstance(ElementId id)
        {
            FamilyInstance result = Document.GetElement(id) as FamilyInstance;
            return result;
        }
    }
}
