using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;
using Nucleus.Geometry;
using Nucleus.Rhino;
using RC = Rhino.Geometry;
using FB = Nucleus.Geometry;
using Rhino.DocObjects;
using Nucleus.Rendering;

namespace Nucleus.Rhino
{
    /// <summary>
    /// Static helper class to deal with Rhino object creation and manipulation
    /// </summary>
    public static class RhinoOutput
    {
        /// <summary>
        /// Is tool currently writing to the Rhino document?
        /// </summary>
        public static bool Writing { get; set; } = false;

        /// <summary>
        /// Private backing field for the active document
        /// </summary>
        private static RhinoDoc _ActiveRhinoDoc = null;

        /// <summary>
        /// Get current active document. If null will return Rhino.ActiveDoc
        /// Set is undertaken by UpdateActiveRhinoDoc method
        /// </summary>
        public static RhinoDoc ActiveRhinoDoc {
            get
            {
                if(_ActiveRhinoDoc == null)
                {
                    _ActiveRhinoDoc = RhinoDoc.ActiveDoc;
                }
                return _ActiveRhinoDoc;
            }
        }

        /// <summary>
        /// Manually override current active document.
        /// Typically default should be used. Only in special circumstances where it does not work.
        /// </summary>
        /// <param name="rhinoDocument"></param>
        public static void UpdateActiveRhinoDoc(RhinoDoc rhinoDocument)
        {
            _ActiveRhinoDoc = rhinoDocument;
        }

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
            return ActiveRhinoDoc.Objects.AddPoint(point);
        }

        /// <summary>
        /// Add a point to the current Rhino document
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Guid BakePoint(Vector point)
        {
            return BakePoint(ToRC.Convert(point));
        }

        /// <summary>
        /// Replace an object in the current Rhino document with a point object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool ReplacePoint(Guid obj, Point3d point)
        {
            return ActiveRhinoDoc.Objects.Replace(obj, point);
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
            result = ReplacePoint(obj, ToRC.Convert(point));
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
            return ActiveRhinoDoc.Objects.AddCurve(curve);
        }

        /// <summary>
        /// Add a curve to the Rhino document
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static Guid BakeCurve(FB.Curve curve)
        {
            return BakeCurve(ToRC.Convert(curve));
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
            result = ActiveRhinoDoc.Objects.Replace(obj, curve);
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
            return ReplaceCurve(obj, ToRC.Convert(curve));
        }

        /// <summary>
        /// Add a line between two points to the current Rhino document
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static Guid BakeLine(Point3d startPoint, Point3d endPoint)
        {
            return ActiveRhinoDoc.Objects.AddLine(startPoint, endPoint);
        }

        /// <summary>
        /// Add a line between two points to the current Rhino document
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static Guid BakeLine(Vector startPoint, Vector endPoint)
        {
            return BakeLine(ToRC.Convert(startPoint), ToRC.Convert(endPoint));
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
            result = ActiveRhinoDoc.Objects.Replace(obj, line);
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
            return ReplaceLine(obj, ToRC.Convert(startPoint), ToRC.Convert(endPoint));
        }

        /// <summary>
        /// Add a new extrusion object to the current Rhino document
        /// </summary>
        /// <param name="extrusion"></param>
        /// <returns></returns>
        public static Guid BakeExtrusion(RC.Extrusion extrusion)
        {
            return ActiveRhinoDoc.Objects.AddExtrusion(extrusion);
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
            result = ActiveRhinoDoc.Objects.Replace(objID, extrusion);
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
            return ActiveRhinoDoc.Objects.AddMesh(mesh);
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
            result = ActiveRhinoDoc.Objects.Replace(objID, mesh);
            Writing = false;
            return result;
        }

        /// <summary>
        /// Bake a piece of Rhino geometry in the active document
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static Guid Bake(RC.GeometryBase geometry)
        {
            if (geometry != null)
                return ActiveRhinoDoc.Objects.Add(geometry);
            return Guid.Empty;
        }

        /// <summary>
        /// Bake a piece of Nucleus geometry in the active Rhino document
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static Guid Bake(VertexGeometry geometry)
        {
            Guid result = Guid.Empty;
            if (geometry != null)
            {
                GeometryBase gB = ToRC.Convert(geometry);
                if (gB != null)
                {
                    Writing = true;
                    result = Bake(gB);
                    Writing = false;
                }
                //else throw new NotImplementedException();
            }

            return result;
        }

        /// <summary>
        /// Bake a layered table of Nucleus geometry to Rhino equivalent
        /// geometries and layers
        /// </summary>
        /// <param name="geometryTable"></param>
        /// <returns></returns>
        public static void BakeAll(GeometryLayerTable geometryTable)
        {
            foreach (GeometryLayer layer in geometryTable)
            {
                BakeAll(layer);
            }
        }

        /// <summary>
        /// Bake all geometry in the specified layer into Rhino on an
        /// equivalent layer
        /// </summary>
        /// <param name="layer"></param>
        public static void BakeAll(GeometryLayer layer)
        {
            int layerID = GetLayerID(layer.Name);
            if (layer.Brush != null) SetLayerColour(layerID, layer.Brush.BaseColour);

            if (layer.Count > 0)
            {
                foreach (VertexGeometry vG in layer)
                {
                    Guid gID = Bake(vG);
                    SetObjectLayer(gID, layerID);
                }
            }
        }

        /// <summary>
        /// Replace a geometry object in the active Rhino document
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static bool Replace(Guid objID, GeometryBase geometry)
        {
            bool result = false;
            Writing = true;
            if (geometry == null)
                result = false;
            else if (geometry is RC.Curve)
                result = ActiveRhinoDoc.Objects.Replace(objID, (RC.Curve)geometry);
            else if (geometry is RC.Mesh)
                result = ActiveRhinoDoc.Objects.Replace(objID, (RC.Mesh)geometry);
            else if (geometry is RC.Brep)
                result = ActiveRhinoDoc.Objects.Replace(objID, (RC.Brep)geometry);
            else if (geometry is RC.Surface)
                result = ActiveRhinoDoc.Objects.Replace(objID, (RC.Curve)geometry);
            Writing = false;
            return result;
        }

        /// <summary>
        /// Replace an object in the current Rhino document
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static bool Replace(Guid objID, VertexGeometry geometry)
        {
            return Replace(objID, ToRC.Convert(geometry));
        }

        /// <summary>
        /// Replace an object if it exists or bake a new one if it does not
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static Guid BakeOrReplace(Guid objID, VertexGeometry geometry)
        {
            if (objID != Guid.Empty && Replace(objID, geometry))
                return objID;
            else
                return Bake(geometry);
        }

        /// <summary>
        /// Replace a point object if it exists or bake a new one if it does not
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Guid BakeOrReplacePoint(Guid objID, Vector point)
        {
            if (objID != Guid.Empty && ReplacePoint(objID, point))
                return objID;
            else
                return BakePoint(point);
        }

        /// <summary>
        /// Helper function to get an object by it's GUID
        /// </summary>
        /// <param name="objID"></param>
        /// <returns></returns>
        private static RhinoObject GetObject(Guid objID)
        {
            return ActiveRhinoDoc.Objects.Find(objID);
        }

        /// <summary>
        /// Select the object with the specified GUID in Rhino
        /// </summary>
        /// <param name="objID"></param>
        public static void SelectObject(Guid objID)
        {
            GetObject(objID)?.Select(true);
        }

        /// <summary>
        /// Unselect all objects in Rhino
        /// </summary>
        public static void UnselectAllObjects()
        {
            ActiveRhinoDoc.Objects.UnselectAll();
        }

        /// <summary>
        /// Does an object with the specified ID exist within the current document?
        /// </summary>
        /// <param name="objID"></param>
        /// <returns></returns>
        public static bool ObjectExists(Guid objID)
        {
            return GetObject(objID) != null;
        }

        /// <summary>
        /// Is the specified object currently visible?
        /// (i.e. not hidden, and on a layer which is visible)
        /// </summary>
        /// <param name="objID"></param>
        /// <returns></returns>
        public static bool ObjectVisible(Guid objID)
        {
            RhinoObject obj = GetObject(objID);
            if (obj != null && !obj.IsHidden)
            {
                return LayerVisible(obj.Attributes.LayerIndex);
            }
            else return false;
        }

        /// <summary>
        /// Is the specified layer currently visible?
        /// </summary>
        /// <param name="layerID"></param>
        /// <returns></returns>
        public static bool LayerVisible(int layerID)
        {
            var layer = ActiveRhinoDoc.Layers[layerID];
            return layer.IsVisible;
        }

        /// <summary>
        /// Set the visibility of the specified layer
        /// </summary>
        /// <param name="layerID"></param>
        /// <param name="visibility"></param>
        public static void SetLayerVisible(int layerID, bool visibility)
        {
            var layer = ActiveRhinoDoc.Layers[layerID];
            layer.IsVisible = visibility;
            layer.CommitChanges();
        }

        /// <summary>
        /// Is the specified object currently selected?
        /// </summary>
        /// <param name="objID"></param>
        /// <returns></returns>
        public static bool ObjectSelected(Guid objID)
        {
            RhinoObject obj = GetObject(objID);
            if (obj != null && obj.IsSelected(false) > 0) return true; //TODO: Check docs
            else return false;
        }

        /// <summary>
        /// Delete an object from the Rhino document
        /// </summary>
        /// <param name="objID"></param>
        /// <returns></returns>
        public static bool DeleteObject(Guid objID)
        {
            return ActiveRhinoDoc.Objects.Delete(objID, true);
        }

        /// <summary>
        /// Undelete an 
        /// </summary>
        /// <param name="objID"></param>
        /// <returns></returns>
        public static bool UndeleteObject(Guid objID)
        {
            RhinoObject rObj = GetObject(objID);
            if (rObj == null) return false;
            return ActiveRhinoDoc.Objects.Undelete(rObj);
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
                return rObj.CommitChanges();
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
        public static int GetLayerID(string path, bool isReference = false, bool create = true)
        {
            int result = ActiveRhinoDoc.Layers.FindByFullPath(path, true);
            if (result < 0)
            {
                string[] tokens = path.Split(LAYER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                string compositePath = "";
                Guid parentID = Guid.Empty;
                foreach (string token in tokens)
                {
                    if (!string.IsNullOrEmpty(compositePath)) compositePath += "::";
                    compositePath += token;
                    result = ActiveRhinoDoc.Layers.FindByFullPath(compositePath, true);
                    if (result < 0 && create) //Layer not found
                    {
                        Layer newLayer = new Layer();
                        newLayer.Name = token;
                        if (parentID != Guid.Empty)
                        {
                            newLayer.ParentLayerId = parentID;
                        }
                        if (isReference)
                            result = ActiveRhinoDoc.Layers.AddReferenceLayer(newLayer);
                        else
                            result = ActiveRhinoDoc.Layers.Add(newLayer);
                    }
                    if (result >= 0)
                    {
                        Layer thisLayer = ActiveRhinoDoc.Layers[result];
                        parentID = thisLayer.Id;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Set the default display colour of the layer with the specified path.
        /// If no layer already exists with that path a new one will be created.
        /// </summary>
        /// <param name="layerPath"></param>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static bool SetLayerColour(string layerPath, Colour colour)
        {
            int layerID = GetLayerID(layerPath);
            return SetLayerColour(layerID, colour);
        }

        /// <summary>
        /// Set the default display colour of the layer with the specified ID.
        /// </summary>
        /// <param name="layerPath"></param>
        /// <param name="colour"></param>
        /// <returns></returns>
        public static bool SetLayerColour(int layerID, Colour colour)
        {
            var layer = ActiveRhinoDoc.Layers[layerID];
            layer.Color = ToRC.Convert(colour);
            return layer.CommitChanges();
        }

        /// <summary>
        /// Set the name of the layer with the specified ID.
        /// </summary>
        /// <param name="layerID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ChangeLayerName(int layerID, string name)
        {
            var layer = ActiveRhinoDoc.Layers[layerID];
            layer.Name = name;
            return layer.CommitChanges();
        }

        /// <summary>
        /// Change the name of the layer at the specified path
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public static bool ChangeLayerName(string oldPath, string newName)
        {
            int layerID = GetLayerID(oldPath, false, false);
            if (layerID >= 0) return ChangeLayerName(layerID, newName);
            return false;
        }

        /// <summary>
        /// Set the layer of the specified object
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="layerID"></param>
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
        /// Set the layer of the specified object
        /// </summary>
        /// <param name="objID"></param>
        /// <param name="layerPath"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public static bool SetObjectLayer(Guid objID, string layerPath, bool commit = true)
        {
            int layerID = GetLayerID(layerPath);
            if (layerID >= 0) return SetObjectLayer(objID, layerID, commit);
            else return false;
        }

        /// <summary>
        /// Clear a layer by deleting all geometry that is currently on it
        /// </summary>
        /// <param name="layerID"></param>
        /// <returns></returns>
        public static bool ClearLayer(int layerID)
        {
            if (layerID < 0) return false;
            Layer layer = ActiveRhinoDoc.Layers[layerID];
            var objs = ActiveRhinoDoc.Objects.FindByLayer(layer);
            if (objs != null)
                foreach (var obj in objs) DeleteObject(obj.Id);
            return true;
        }

        /// <summary>
        /// Delete the layer specified by the given ID
        /// </summary>
        /// <param name="layerID">The index of the layer to delete</param>
        /// <param name="onlyIfEmpty">If true, the layer will only be deleted if there are no objects on that layer</param>
        /// <returns></returns>
        public static bool DeleteLayer(int layerID, bool onlyIfEmpty = false)
        {
            if (layerID >= 0 && layerID < ActiveRhinoDoc.Layers.Count)
            {
                if (onlyIfEmpty)
                {
                    Layer layer = ActiveRhinoDoc.Layers[layerID];
                    if (layer != null)
                    {
                        RhinoObject[] objects = ActiveRhinoDoc.Objects.FindByLayer(layer);
                        if (objects == null || objects.Count() == 0)
                            return ActiveRhinoDoc.Layers.Delete(layerID, true);
                    }
                }
                else return ActiveRhinoDoc.Layers.Delete(layerID, true);
            }
            return false;
        }

        /// <summary>
        /// Delete the specified layer and all children provided the child tree contains no objects
        /// </summary>
        /// <param name="layer">The layer index to delete</param>
        /// <returns></returns>
        public static bool DeleteEmptySubLayers(string path)
        {
            return DeleteEmptySubLayers(GetLayerID(path));
        }

        /// <summary>
        /// Delete the specified layer and all children provided the child tree contains no objects
        /// </summary>
        /// <param name="layer">The layer index to delete</param>
        /// <returns></returns>
        public static bool DeleteEmptySubLayers(int layerID)
        {
            if (layerID >= 0 && layerID < ActiveRhinoDoc.Layers.Count)
            {
                Layer layer = ActiveRhinoDoc.Layers[layerID];
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
                    RhinoObject[] objects = ActiveRhinoDoc.Objects.FindByLayer(layer);
                    if (objects == null || objects.Count() == 0)
                    {
                        ActiveRhinoDoc.Layers.Delete(layer.LayerIndex, true);
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
