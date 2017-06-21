using FreeBuild.Base;
using FreeBuild.Geometry;
using GoogleMaps.LocationServices;
using OsmSharp.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Maps
{
    /// <summary>
    /// Class to extract map geometry from OpenStreetMap
    /// </summary>
    public class MapExtractor
    {
        #region Constants

        /// <summary>
        /// The URI of the OpenStreetMap API
        /// </summary>
        public const string OSMAPI = "http://api.openstreetmap.org/api/";

        /// <summary>
        /// The targeted version of the OpenStreetMap API
        /// </summary>
        public const string OSMAPIVersion = "0.6";

        #endregion

        #region Properties

        /// <summary>
        /// The character sequence which should be used to separate layer name sublevels.
        /// If this is null, layer sublevels will not be created.
        /// Note that some characters will not be valid in layernames in some contexts.
        /// </summary>
        public string LayerSeparator { get; set; } = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new MapExtractor with the default settings
        /// </summary>
        public MapExtractor() { }

        #endregion

        #region Methods

        /// <summary>
        /// Get the longitude and latitude for an address string
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Vector LatitudeAndLongitudeFromAddress(string address)
        {
            var gls = new GoogleLocationService();
            try
            {
                var mapPt = gls.GetLatLongFromAddress(address);
                if (mapPt == null) return Vector.Unset;
                return new Vector(mapPt.Longitude, mapPt.Latitude);
            }
            catch
            {
                return Vector.Unset;
            }
        }

        /// <summary>
        /// Download a map showing a range around the specified latitude 
        /// and longitude and save it to the given file path.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="saveTo"></param>
        /// <param name="range"></param>
        public void DownloadMap(double latitude, double longitude, FilePath saveTo, double range = 0.005)
        {
            DownloadMap(longitude - range, latitude - range, longitude + range, latitude + range, saveTo);
        }

        /// <summary>
        /// Download a map for the given coordinate range and save it at the specified location
        /// </summary>
        /// <param name="left">The lower-bound longitude</param>
        /// <param name="bottom">The lower-bound latitude</param>
        /// <param name="right">The upper-bound longitude</param>
        /// <param name="top">The upper-bound latitude</param>
        /// <param name="saveTo">The filepath to save the map file to</param>
        public void DownloadMap(double left, double bottom, double right, double top, FilePath saveTo)
        {
            //Compile the OpenStreetMap API URL:
            string osmAPIGet = OSMAPI + OSMAPIVersion + "/map?bbox=" +
                left + "," + bottom + "," + right + "," + top;
            using (var client = new WebClient())
            {
                client.DownloadFile(osmAPIGet, saveTo);
            }
        }

        /// <summary>
        /// Read map geometry data, automatically retrieving the 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="layerNames"></param>
        /// <returns></returns>
        public GeometryLayerTable ReadMap(string address, IList<string> layerNames = null)
        {
            Vector ll = LatitudeAndLongitudeFromAddress(address);
            if (!ll.IsValid()) return null;
            double latitude = ll.Y;
            double longitude = ll.X;
            FilePath osmFile = FilePath.Temp + "TempMap.osm";
            DownloadMap(latitude, longitude, osmFile);
            return ReadMap(osmFile, latitude, longitude);
        }

        /// <summary>
        /// Read map geometry data from a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public GeometryLayerTable ReadMap(FilePath filePath, double originLatitude, double originLongitude, IList<string> layerNames = null)
        {
            GeometryLayerTable result = new GeometryLayerTable();

            if (layerNames == null)
            {
                layerNames = new List<string>();
                layerNames.Add("Building");
                layerNames.Add("Highway");
            }

            var nodes = new Dictionary<long, OsmSharp.Node>();
            var ways = new Dictionary<long, OsmSharp.Way>();

            using (var fileStream = filePath.Info.OpenRead())
            {
                var source = new XmlOsmStreamSource(fileStream);

                foreach (var element in source)
                {
                    if (element.Id != null)
                    {
                        if (element.Type == OsmSharp.OsmGeoType.Node)
                        {
                            nodes.Add((long)element.Id, (OsmSharp.Node)element);
                        }
                        else if (element.Type == OsmSharp.OsmGeoType.Way)
                        {
                            ways.Add((long)element.Id, (OsmSharp.Way)element);
                        }
                        //TODO: Relations?
                    }
                }
                foreach (var way in ways.Values)
                {
                    var pts = new List<Vector>(way.Nodes.Length);
                    for (int i = 0; i < way.Nodes.Length; i++)
                    {
                        long nodeID = way.Nodes[i];
                        if (nodes.ContainsKey(nodeID))
                        {
                            var node = nodes[nodeID];
                            pts.Add(node.Position(originLatitude, originLongitude));
                        }
                    }
                    VertexGeometry geometry = null;
                    if (pts.Count == 2)
                    {
                        geometry = new Line(pts[0], pts[1]);
                    }
                    else if (pts.Count > 2)
                    {
                        geometry = new PolyLine(pts);
                        //TODO: Areas
                    }
                    if (geometry != null)
                    {
                        string layerName = "Miscellaneous";
                        //Assign layer according to keys:
                        if (way.Tags != null)
                        {
                            foreach (string name in layerNames)
                            {
                                string lName = name.ToLower();
                                if (way.Tags.ContainsKey(lName))
                                {
                                    string value = way.Tags[lName];
                                    if (LayerSeparator != null && !string.IsNullOrWhiteSpace(value) && value != "yes")
                                    {
                                        layerName = name + LayerSeparator + value;
                                    }
                                    else
                                    {
                                        layerName = name;
                                    }
                                    break;
                                }
                            }
                        }
                        var layer = result.GetOrCreate(layerName);
                        layer.Add(geometry);
                    }
                }
            }
            return result;
        }

        #endregion
    }
}
