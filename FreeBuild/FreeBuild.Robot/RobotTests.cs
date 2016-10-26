using FreeBuild.Base;
using FreeBuild.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Robot
{
    /// <summary>
    /// Static class for Robot reading/writing tests
    /// </summary>
    public static class RobotTests
    {
        /// <summary>
        /// Read a robot file and write a copy to a new location
        /// </summary>
        /// <param name="readPath"></param>
        /// <param name="writePath"></param>
        public static void ReadWriteTest(FilePath readPath, FilePath writePath)
        {
            var robot = new RobotController();
            RobotIDMappingTable idMap = new RobotIDMappingTable();
            Model.Model model = robot.LoadModelFromRobot(readPath, ref idMap);
            robot.WriteModelToRobot(writePath, model, ref idMap);
        }
    }
}
