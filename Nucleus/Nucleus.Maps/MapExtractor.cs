using Nucleus.Base;
using Nucleus.Geometry;
using OsmSharp.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Nucleus.Alerts;
using System.Globalization;
//using Nominatim.API.Geocoders;
//using Nominatim.API.Models;

namespace Nucleus.Maps
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

        /// <summary>
        /// The URI of the Nominatim API search function
        /// </summary>
        public const string NominatimAPI = "https://nominatim.openstreetmap.org/search?";

        #endregion

        #region Properties

        /// <summary>
        /// The character sequence which should be used to separate layer name sublevels.
        /// If this is null, layer sublevels will not be created.
        /// Note that some characters will not be valid in layernames in some contexts.
        /// </summary>
        public string LayerSeparator { get; set; } = null;

        /// <summary>
        /// The default height given to imported buildings which do not have 
        /// height-defining tags provided.  If 0, only buildings which have
        /// a default height specified will be extruded.
        /// </summary>
        public double DefaultBuildingHeight { get; set; } = 0;

        /// <summary>
        /// The storey height used to calculate the height of buildings which do not
        /// have explict height tags supplied by do state the number of storeys
        /// </summary>
        public double StoreyHeight { get; set; } = 3.0;

        /// <summary>
        /// The additional height added to assumed building heights based on storey count,
        /// to account for higher ground floors, roof furniture etc.
        /// </summary>
        public double ByStoreysExtraHeight { get; set; } = 2.0;

        /// <summary>
        /// Get or set whether building outlines should be extruded.
        /// </summary>
        public bool ExtrudeBuildings { get; set; } = true;

        /// <summary>
        /// Get or set whether roads should be imported as surfaces
        /// </summary>
        public bool RoadsAsSurfaces { get; set; } = true;

        /// <summary>
        /// The default width to be used for roads which do not define
        /// a width or lane count and are not one of the standard road types
        /// </summary>
        public double DefaultRoadWidth { get; set; } = 5.5;

        /// <summary>
        /// The default width to be assumed for each lane in a road.
        /// Used when number of lanes is specified but the overall road
        /// width is not.
        /// </summary>
        public double DefaultRoadLaneWidth { get; set; } = 3.65;

        private IDictionary<string, double> _RoadWidths = null;

        /// <summary>
        /// The preset assumed widths for different types of roads,
        /// stored as a dictionary of width values in m, keyed by
        /// the value of the 'highway' tag in lower case.
        /// </summary>
        public IDictionary<string, double> RoadWidths
        {
            get
            {
                if (_RoadWidths == null)
                {
                    //Defaults:
                    _RoadWidths = new Dictionary<string, double>();
                    _RoadWidths.Add("motorway", 22);
                    _RoadWidths.Add("trunk", 16);
                    _RoadWidths.Add("primary", 7.3);
                    _RoadWidths.Add("secondary", 7.3);
                    _RoadWidths.Add("tertiary", 7.3);
                    _RoadWidths.Add("unclassified", 5.5);
                    _RoadWidths.Add("residential", 5.5);
                }
                return _RoadWidths;
            }
            set
            {
                _RoadWidths = value;
            }
        }

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
        public AnglePair LatitudeAndLongitudeFromAddress(string address, AlertLog log = null)
        {
            // Nominatim requires a contact email address
            string queryString = NominatimAPI + "q=" + address + "&format=xml&email=paul@vitruality.com";

            var web = new WebClient();
            string xml = web.DownloadString(queryString);
            XElement xmlTree = XElement.Parse(xml);
            var place = xmlTree.Element("place");
            string lat = place.Attribute("lat").Value;
            string lon = place.Attribute("lon").Value;

            double latitude;
            double.TryParse(lat, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out latitude);

            double longitude;
            double.TryParse(lon, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out longitude);

            return AnglePair.FromDegrees(latitude, longitude);


            /*var geocoder = new ForwardGeocoder();
            var task = geocoder.Geocode(new ForwardGeocodeRequest
            {
                queryString = address,
                BreakdownAddressElements = true,
                ShowExtraTags = true,
                ShowAlternativeNames = true,
            });

            if (task.Result.Length > 0)
            {
                var response = task.Result[0];
                return AnglePair.FromDegrees(response.Latitude, response.Longitude);
            }
            else
                throw new Exception("Address '" + address + "' could not be found.");
            */
            
            /*var gls = new GoogleLocationService();
            try
            {
                var mapPt = gls.GetLatLongFromAddress(address);
                if (mapPt == null) return Vector.Unset;
                return new Vector(mapPt.Longitude, mapPt.Latitude);
            }
            catch
            {
                return Vector.Unset;
            }*/
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
        public bool DownloadMap(double left, double bottom, double right, double top, FilePath saveTo, AlertLog log = null)
        {
            //Compile the OpenStreetMap API URL:
            string osmAPIGet = BuildGetURL(left, bottom, right, top);
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(osmAPIGet, saveTo);
                }
                catch(Exception ex)
                {
                    log?.RaiseAlert("Error downloading map " + ex.Message, AlertLevel.Warning);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Download map data for a specified bounding box as a byte array
        /// </summary>
        /// <param name="left"></param>
        /// <param name="bottom"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public byte[] DownloadMapData(double left, double bottom, double right, double top, AlertLog log = null)
        {
            string osmAPIGet = BuildGetURL(left, bottom, right, top);
            using (var client = new WebClient())
            {
                try
                {
                    return client.DownloadData(osmAPIGet);
                }
                catch (Exception ex)
                {
                    log?.RaiseAlert("Error downloading map data " + ex.Message, AlertLevel.Warning);
                    return new byte[] { };
                }
            }
        }

        /// <summary>
        /// Build a OpenStreetMap API Get URL
        /// </summary>
        /// <param name="left"></param>
        /// <param name="bottom"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public string BuildGetURL(double left, double bottom, double right, double top)
        {
            //Compile the OpenStreetMap API URL:
            string osmAPIGet = OSMAPI + OSMAPIVersion + "/map?bbox=" +
                SanitiseDouble(left) + "," +
                SanitiseDouble(bottom) + "," +
                SanitiseDouble(right) + "," +
                SanitiseDouble(top);
            return osmAPIGet;
        }

        /// <summary>
        /// Convert a double to a string such that it is compatible
        /// with the OpenStreetMap API (i.e. no comma-separated decimals)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>The double to sanitise</returns>
        private string SanitiseDouble(double input)
        {
            return input.ToString().Replace(',', '.');
        }

        /// <summary>
        /// Read map geometry data, automatically retrieving the 
        /// </summary>
        /// <param name="address">The address to search for</param>
        /// <param name="range">The distance around the specified location to download</param>
        /// <param name="layerNames"></param>
        /// <returns></returns>
        public GeometryLayerTable ReadMap(string address, double range = 0.005, IList<string> layerNames = null,
            AlertLog log = null)
        {
            /*Vector ll = LatitudeAndLongitudeFromAddress(address);
            if (!ll.IsValid()) return null;*/
            var latLong = LatitudeAndLongitudeFromAddress(address);
            return ReadMap(latLong, range, layerNames, log);
        }

        /// <summary>
        /// Read map geometry data, automatically retrieving map data from the OpenStreetMap servers 
        /// </summary>
        /// <param name="latLong">The latitude and longitude of the map origin</param>
        /// <param name="range">The range around the specified latitude and longitude to be collected, in degrees</param>
        /// <param name="layerNames"></param>
        /// <returns></returns>
        public GeometryLayerTable ReadMap(AnglePair latLong, double range = 0.005, IList<string> layerNames = null,
            AlertLog log = null)
        {
            return ReadMap(latLong.Elevation.Degrees, latLong.Azimuth.Degrees, range, layerNames, log);
        }

        /// <summary>
        /// Read map geometry data, automatically retrieving map data from the OpenStreetMap servers 
        /// </summary>
        /// <param name="latitude">The latitude, in degrees</param>
        /// <param name="longitude">The longitude, in degrees</param>
        /// <param name="range">The range around the specified latitude and longitude to be collected, in degrees</param>
        /// <param name="layerNames"></param>
        /// <returns></returns>
        public GeometryLayerTable ReadMap(double latitude, double longitude, double range = 0.005, IList<string> layerNames = null,
            AlertLog log = null)
        {
            //FilePath osmFile = FilePath.Temp + "TempMap.osm";
            //DownloadMap(latitude, longitude, osmFile, range);
            //return ReadMap(osmFile, latitude, longitude);
            var data = DownloadMapData(longitude - range, latitude - range, longitude + range, latitude + range);
            try
            {
                using (var stream = new MemoryStream(data))
                {
                    return ReadMap(stream, AnglePair.FromDegrees(latitude, longitude), layerNames);
                }
            }
            catch (Exception ex)
            {
                log?.RaiseAlert("Error reading map " + ex.Message, AlertLevel.Information);
                return null;
            }
        }
        
        /// <summary>
        /// Read a map from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="originLatLong"></param>
        /// <param name="layerNames"></param>
        /// <returns></returns>
        public GeometryLayerTable ReadMap(Stream stream, AnglePair originLatLong, IList<string> layerNames = null)
        {
            var result = new GeometryLayerTable();
            var source = new XmlOsmStreamSource(stream);

            var nodes = new Dictionary<long, OsmSharp.Node>();
            var ways = new Dictionary<long, OsmSharp.Way>();
            var roadNetwork = new List<PathSurface>();

            if (layerNames == null)
            {
                layerNames = new List<string>();
                layerNames.Add("Building");
                layerNames.Add("Highway");
            }

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
                        OsmSharp.Node node = nodes[nodeID];
                        pts.Add(node.Position(originLatLong));
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
                        if (ExtrudeBuildings)
                        {
                            if (geometry is Curve && way.Tags.ContainsKey("height"))
                            {
                                string heightTag = way.Tags["height"];
                                if (ParseDistance(heightTag, out double height))
                                {
                                    // TODO: Deal with tags with different units on the end!
                                    geometry = new Extrusion((Curve)geometry, new Vector(0, 0, height));
                                }
                            }
                            if (geometry is Curve && way.Tags.ContainsKey("building:levels"))
                            {
                                // Height not supplied - fall back to storeys:
                                string levelsTag = way.Tags["building:levels"];
                                if (double.TryParse(levelsTag, out double levels))
                                {
                                    geometry = new Extrusion((Curve)geometry, new Vector(0, 0, levels * StoreyHeight + ByStoreysExtraHeight));
                                }
                            }
                            if (geometry is Curve && DefaultBuildingHeight > 0 && way.Tags.ContainsKey("building"))
                            {
                                // No indication of height supplied - fall back to default:
                                geometry = new Extrusion((Curve)geometry, new Vector(0, 0, DefaultBuildingHeight));
                            }
                        }
                        if (RoadsAsSurfaces)
                        {
                            if (geometry is Curve && way.Tags.ContainsKey("highway"))
                            {
                                if (way.Tags.ContainsKey("width"))
                                {
                                    var widthTag = way.Tags["width"];
                                    if (ParseDistance(widthTag, out double width))
                                    {
                                        geometry = new PathSurface((Curve)geometry, width);
                                    }
                                }
                                if (geometry is Curve && way.Tags.ContainsKey("lanes"))
                                {
                                    var lanesTag = way.Tags["lanes"];
                                    if (double.TryParse(lanesTag, out double lanes))
                                    {
                                        geometry = new PathSurface((Curve)geometry, DefaultRoadLaneWidth * lanes);
                                    }
                                }
                                if (geometry is Curve)
                                {
                                    var typeTag = way.Tags["highway"];
                                    if (RoadWidths.ContainsKey(typeTag))
                                    {
                                        double width = RoadWidths[typeTag];
                                        geometry = new PathSurface((Curve)geometry, width);
                                    }
                                }
                                if (geometry is Curve)
                                {
                                    geometry = new PathSurface((Curve)geometry, DefaultRoadWidth);
                                }

                                // Build network:
                                if (geometry is PathSurface path) roadNetwork.Add(path);
                            }
                        }
                    }
                    var layer = result.GetOrCreate(layerName);
                    layer.Add(geometry);

                    if (roadNetwork.Count > 0)
                    {
                        roadNetwork.GenerateNetworkPathNodes(new Model.NodeGenerationParameters(1.0));
                        roadNetwork.GenerateNetworkPathEdges();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parse width and height values from OpenStreetMap tags and return
        /// them as a double value expressed in m
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool ParseDistance(string value, out double result)
        {
            // TODO: Detect and deal with different unit types
            value = value.TrimEnd('m').Trim();
            return double.TryParse(value, out result);
        }

        /// <summary>
        /// Read map geometry data from a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public GeometryLayerTable ReadMap(FilePath filePath, double originLatitude, double originLongitude, IList<string> layerNames = null)
        {

            if (filePath.Exists)
            {
                using (var fileStream = filePath.Info.OpenRead())
                {
                    return ReadMap(fileStream, AnglePair.FromDegrees(originLatitude, originLongitude), layerNames);
                }
            }
            else return null;
        }

        #endregion
    }
}
