using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;
using FreeBuild.Geometry;
using FreeBuild.Rhino;
using RC = Rhino.Geometry;
using FB = FreeBuild.Geometry;
using Rhino.DocObjects;

namespace Newt.Rhino
{
    public static class RhinoOutput
    {
        /// <summary>
        /// Is Salamander currently writing to the Rhino document?
        /// </summary>
        public static bool Writing { get; set; } = false;

        /// <summary>
        /// Create the standard layers
        /// </summary>
        public static void InitialiseLayers()
        {

        }

        /// <summary>
        /// Add a point to the current Rhino document
        /// </summary>
        /// <param name="point">The point to bake</param>
        /// <returns></returns>
        public static Guid BakePoint(Point3d point)
        {
            return RhinoDoc.ActiveDoc.Objects.AddPoint(point);
        }

        /// <summary>
        /// Add a point to the current Rhino document
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Guid BakePoint(Vector point)
        {
            return BakePoint(FBtoRC.Convert(point));
        }

        /// <summary>
        /// Replace an object in the current Rhino document with a point object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool ReplacePoint(Guid obj, Point3d point)
        {
            return RhinoDoc.ActiveDoc.Objects.Replace(obj, point);
        }

        /// <summary>
        /// Replace an object in the current Rhino document with a point object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool ReplacePoint(Guid obj, Vector point)
        {
            bool result = false;
            Writing = true;
            result = ReplacePoint(obj, FBtoRC.Convert(point));
            Writing = false;
            return result;
        }

        /// <summary>
        /// Add a curve to the Rhino document
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static Guid BakeCurve(RC.Curve curve)
        {
            return RhinoDoc.ActiveDoc.Objects.AddCurve(curve);
        }

        /// <summary>
        /// Add a curve to the Rhino document
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static Guid BakeCurve(FB.Curve curve)
        {
            return BakeCurve(FBtoRC.Convert(curve));
        }

        /// <summary>
        /// Replace an existing curve in the Rhino document
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static bool ReplaceCurve(Guid obj, RC.Curve curve)
        {
            bool result = false;
            Writing = true;
            result = RhinoDoc.ActiveDoc.Objects.Replace(obj, curve);
            Writing = false;
            return result;
        }

        /// <summary>
        /// Replace an existing curve in the Rhino document
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static bool ReplaceCurve(Guid obj, FB.Curve curve)
        {
            return ReplaceCurve(obj, FBtoRC.Convert(curve));
        }

        /// <summary>
        /// Add a line between two points to the current Rhino document
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static Guid BakeLine(Point3d startPoint, Point3d endPoint)
        {
            return RhinoDoc.ActiveDoc.Objects.AddLine(startPoint, endPoint);
        }

        /// <summary>
        /// Add a line between two points to the current Rhino document
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static Guid BakeLine(Vector startPoint, Vector endPoint)
        {
            return BakeLine(FBtoRC.Convert(startPoint), FBtoRC.Convert(endPoint));
        }

        /// <summary>
        /// Replace an existing object in the Rhino document with a line object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool ReplaceLine(Guid obj, RC.Line line)
        {
            bool result = false;
            Writing = true;
            result = RhinoDoc.ActiveDoc.Objects.Replace(obj, line);
            Writing = false;
            return result;
        }

        /// <summary>
        /// Replace an existing object in the Rhino document with a line object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static bool ReplaceLine(Guid obj, Point3d startPoint, Point3d endPoint)
        { 
            return ReplaceLine(obj, new RC.Line(startPoint, endPoint));
        }

        /// <summary>
        /// Replace an existing object in the Rhino document with a line object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static bool ReplaceLine(Guid obj, Vector startPoint, Vector endPoint)
        {
            return ReplaceLine(obj, FBtoRC.Convert(startPoint), FBtoRC.Convert(endPoint));
        }

        /// <summary>
        /// Add a new extrusion object to the current Rhino document
        /// </summary>
        /// <param name="extrusion"></param>
        /// <returns></returns>
        public static Guid BakeExtrusion(RC.Extrusion extrusion)
        {
            return RhinoDoc.ActiveDoc.Objects.AddExtrusion(extrusion);
        }

        /// <summary>
        /// Replace an extrusion object in the current Rhino document
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="extrusion"></param>
        /// <returns></returns>
        public static bool ReplaceExtrusion(Guid objID, RC.Extrusion extrusion)
        {
            bool result = false;
            Writing = true;
            result = RhinoDoc.ActiveDoc.Objects.Replace(objID, extrusion);
            Writing = false;
            return result;
        }

        /// <summary>
        /// Add a new mesh object to the current Rhino document
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static Guid BakeMesh(RC.Mesh mesh)
        {
            return RhinoDoc.ActiveDoc.Objects.AddMesh(mesh);
        }

        /// <summary>
        /// Replace a mesh object in the current Rhino document
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static bool ReplaceMesh(Guid objID, RC.Mesh mesh)
        {
            bool result = false;
            Writing = true;
            result = RhinoDoc.ActiveDoc.Objects.Replace(objID, mesh);
            Writing = false;
            return result;
        }

        /// <summary>
        /// Helper function to get an object by it's GUID
        /// </summary>
        /// <param name="objID"></param>
        /// <returns></returns>
        private static RhinoObject GetObject(Guid objID)
        {
            return RhinoDoc.ActiveDoc.Objects.Find(objID);
        }

        /// <summary>
        /// Delete an object from the Rhino document
        /// </summary>
        /// <param name="objID"></param>
        /// <returns></returns>
        public static bool DeleteObject(Guid objID)
        {
            return RhinoDoc.ActiveDoc.Objects.Delete(objID, true);
        }

        /// <summary>
        /// Set the name of an object
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SetObjectName(Guid objID, string name)
        {
            RhinoObject rObj = GetObject(objID);
            if (rObj != null)
            {
                rObj.Attributes.Name = name;
                rObj.CommitChanges();
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Lock the given object so that it cannot be selected
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="value">If true, lock, if false, unlock</param>
        /// <param name="commit">If true, change will be automatically committed to the document</param>
        public static void LockObject(Guid objID, bool value = true, bool commit = true)
        {
            RhinoObject rObj = GetObject(objID);
            if (rObj != null)
            {
                if (value) rObj.Attributes.Mode = ObjectMode.Locked;
                else rObj.Attributes.Mode = ObjectMode.Normal;
                if (commit) rObj.CommitChanges();
            }
        }

        /// <summary>
        /// Hide the given object so that it cannot be seen
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="value">If true, hide, if false, unhide</param>
        public static void HideObject(Guid objID, bool value = true, bool commit = true)
        {
            RhinoObject rObj = GetObject(objID);
            if (rObj != null)
            {
                if (value) rObj.Attributes.Mode = ObjectMode.Hidden;
                else rObj.Attributes.Mode = ObjectMode.Normal;
                if (commit) rObj.CommitChanges();
            }
        }

        /// <summary>
        /// Set a user string on an object
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetObjectUserString(Guid objID, string key, string value)
        {
            RhinoObject rObj = GetObject(objID);
            if (rObj != null)
            {
                return rObj.Attributes.SetUserString(key, value);
            }
            return false;
        }

        /// <summary>
        /// The array of separator strings used to split layer paths into separate layers
        /// </summary>
        public static string[] LAYER_SEPARATOR = new string[] { "::" };

        /// <summary>
        /// Get the ID of the layer with the specified path, creating a new one if necessary
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isReference"></param>
        /// <returns></returns>
        public static int GetLayerID(string path, bool isReference = false)
        {
            int result = RhinoDoc.ActiveDoc.Layers.FindByFullPath(path, true);
            if (result < 0)
            {
                string[] tokens = path.Split(LAYER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                string compositePath = "";
                Guid parentID = Guid.Empty;
                foreach (String token in tokens)
                {
                    if (!String.IsNullOrEmpty(compositePath)) compositePath += "::";
                    compositePath += token;
                    result = RhinoDoc.ActiveDoc.Layers.FindByFullPath(compositePath, true);
                    if (result < 0) //Layer not found
                    {
                        Layer newLayer = new Layer();
                        newLayer.Name = token;
                        if (parentID != Guid.Empty)
                        {
                            newLayer.ParentLayerId = parentID;
                        }
                        if (isReference)
                            result = RhinoDoc.ActiveDoc.Layers.AddReferenceLayer(newLayer);
                        else
                            result = RhinoDoc.ActiveDoc.Layers.Add(newLayer);
                    }
                    if (result >= 0)
                    {
                        Layer thisLayer = RhinoDoc.ActiveDoc.Layers[result];
                        parentID = thisLayer.Id;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Set the layer of the specified object
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="layerName"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public static bool SetObjectLayer(Guid objID, int layerID, bool commit = true)
        {
            RhinoObject rObj = GetObject(objID);
            if (rObj != null)
            {
                rObj.Attributes.LayerIndex = layerID;
                if (commit) rObj.CommitChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delete the layer specified by the given ID
        /// </summary>
        /// <param name="layerID">The index of the layer to delete</param>
        /// <param name="onlyIfEmpty">If true, the layer will only be deleted if there are no objects on that layer</param>
        /// <returns></returns>
        public static bool DeleteLayer(int layerID, bool onlyIfEmpty = false)
        {
            if (layerID >= 0 && layerID < RhinoDoc.ActiveDoc.Layers.Count)
            {
                if (onlyIfEmpty)
                {
                    Layer layer = RhinoDoc.ActiveDoc.Layers[layerID];
                    if (layer != null)
                    {
                        RhinoObject[] objects = RhinoDoc.ActiveDoc.Objects.FindByLayer(layer);
                        if (objects == null || objects.Count() == 0)
                            return RhinoDoc.ActiveDoc.Layers.Delete(layerID, true);
                    }
                }
                else return RhinoDoc.ActiveDoc.Layers.Delete(layerID, true);
            }
            return false;
        }

        /// <summary>
        /// Delete the specified layer and all children provided the child tree contains no objects
        /// </summary>
        /// <param name="layer">The layer index to delete</param>
        /// <returns></returns>
        public static bool DeleteEmptySubLayers(int layerID)
        {
            if (layerID >= 0 && layerID < RhinoDoc.ActiveDoc.Layers.Count)
            {
                Layer layer = RhinoDoc.ActiveDoc.Layers[layerID];
                return DeleteEmptySubLayers(layer);
            }
            return true;
        }

        /// <summary>
        /// Delete the specified layer and all children provided the child tree contains no objects
        /// </summary>
        /// <param name="layer">The layer to delete</param>
        /// <returns></returns>
        public static bool DeleteEmptySubLayers(Layer layer)
        {

            if (layer != null && !layer.IsDeleted)
            {
                bool empty = true;
                Layer[] subLayers = layer.GetChildren();
                if (subLayers != null)
                {
                    foreach (Layer subLayer in subLayers)
                    {
                        if (!DeleteEmptySubLayers(subLayer)) empty = false;
                    }
                }

                if (empty)
                {
                    RhinoObject[] objects = RhinoDoc.ActiveDoc.Objects.FindByLayer(layer);
                    if (objects == null || objects.Count() == 0)
                    {
                        RhinoDoc.ActiveDoc.Layers.Delete(layer.LayerIndex, true);
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            return true;
        }

        /// <summary>
        /// Set the User String "SAL_ORIGINAL" to the value of an object's own ID.
        /// Used to determine when objects have been copied.
        /// </summary>
        /// <param name="objID"></param>
        /// <returns></returns>
        public static bool SetOriginalIDUserString(Guid objID)
        {
            return SetObjectUserString(objID, "SAL_ORIGINAL", objID.ToString());
        }
    }
}
