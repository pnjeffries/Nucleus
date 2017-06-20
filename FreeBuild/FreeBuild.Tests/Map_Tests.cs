using FreeBuild.Base;
using FreeBuild.DXF;
using FreeBuild.Geometry;
using FreeBuild.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Tests
{
    public static class Map_Tests
    {
        public static void DownloadMapTest(double latitude = 51.57168, double longitude = -0.35309)
        {
            Core.Print("Downloading Map...");
            var extractor = new MapExtractor();
            FilePath saveTo = FilePath.Temp + "Test2.osm";
            extractor.DownloadMap(latitude, longitude, saveTo, 0.005);
            Core.Print("Downloaded.");
            Core.Print("Reading...");
            var geoTable = extractor.ReadMap(saveTo, latitude, longitude);

            var dxfWriter = new DXFWriter();
            dxfWriter.WriteDXF(FilePath.Temp + "MapTest.dxf", geoTable);

            Core.Print("Read.");
        }

        public static string AddressToLat(string address)
        {
            var extractor = new MapExtractor();
            Vector pt = extractor.LatitudeAndLongitudeFromAddress(address);
            return "Latitude and Longitude: " + pt.Y + "°, " + pt.X + "°";
        }

        public static void DownloadAddressTest(string address)
        {
            var extractor = new MapExtractor();
            Vector pt = extractor.LatitudeAndLongitudeFromAddress(address);
            DownloadMapTest(pt.Y, pt.X);
        }
    }
}
