using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Analysis
{
    /// <summary>
    /// Calculation of wind loads on a simple box to Eurocode 1 EN 1991-1-4
    /// </summary>
    public class WindLoadsBox
    {
        /// <summary>
        /// Calculate the basic wind velocity (v_b) using Expression 4.1
        /// </summary>
        /// <param name="v_b0">The fundamental value of the basic wind velocity</param>
        /// <param name="c_dir">The directional factor.  May be found in the National Annex.</param>
        /// <param name="c_season">The seasonal factor.  May be found in the National Annex.</param>
        /// <returns></returns>
        public double BasicWindVelocity(double v_b0, double c_dir = 1.0, double c_season = 1.0)
        {
            return c_dir * c_season * v_b0; // 4.1
        }

        /// <summary>
        /// Calculate the mean wind velocity (v_m) using Expression 4.3
        /// </summary>
        /// <param name="v_b">The basic wind velocity</param>
        /// <param name="c_r">The terrain roughness factor, given in 4.3.2 </param>
        /// <param name="c_o">The orography factor, taken as 1.0 unless otherwise specified in 4.3.3</param>
        /// <returns></returns>
        public double MeanWindVelocity(double v_b, double c_r, double c_o = 1.0)
        {
            return c_r * c_o * v_b; // 4.3
        }

        /// <summary>
        /// Calculate the Altitude Factor (c_alt)
        /// </summary>
        /// <param name="A">The site altitude (ground level in front of the building in m above sea level)</param>
        /// <param name="z_s">The reference height.  For global effects, this is 0.6 * the building height.  For individual element
        /// calculations this should be taken from EN1991-1-4 Fig. 6.1</param>
        /// <returns></returns>
        public double AltitudeFactor(double A, double z_s)
        {
            if (z_s < 10)
            {
                return 1 + 0.001 * A;
            }
            else
                return 1 + 0.001 * A * Math.Pow((10 / z_s), 0.2);
        }

        /// <summary>
        /// Calculate the Peak Velocity Pressure (q_p), in Pa
        /// </summary>
        /// <param name="v_map">The map velocity, in m/s</param>
        /// <param name="c_alt">The altitude factor</param>
        /// <param name="c_dir">The direction factor</param>
        /// <param name="c_e">The exposure factor</param>
        /// <param name="c_eT">The town terrain correction factor</param>
        /// <returns></returns>
        public double PeakVelocityPressure(double v_map, double c_alt, double c_dir, double c_e, double c_eT)
        {
            return 0.613 * Math.Pow((v_map * c_alt * c_dir), 2) * c_e * c_eT;
        }

        /// <summary>
        /// Calculate the wind force (F_w)
        /// </summary>
        /// <param name="q_p">The peak velocity pressure, in Pa</param>
        /// <param name="c_s">The size factor</param>
        /// <param name="c_d">The dynamic factor</param>
        /// <param name="c_f">The force coefficient</param>
        /// <param name="a_sh">The shadow area (usually b*h)</param>
        /// <returns></returns>
        public double WindForce(double q_p, double c_s, double c_d, double c_f, double a_sh)
        {
            return q_p * c_s * c_d * c_f * a_sh;
        }

        /// <summary>
        /// Calculate the wind force coefficient based on the height and depth of the building
        /// </summary>
        /// <param name="h">The height of the building</param>
        /// <param name="d">The depth of the building</param>
        /// <returns></returns>
        public double ForceCoefficient(double h, double d)
        {
            if (h / d <= 0.25)
                return 0.68;
            else if (h / d <= 1.0)
                return 0.935 + 0.1839 * Math.Log(h / d);
            else if (h / d <= 5.0)
                return (0.8125 + 0.0375 * h / d) * (1.1 + 0.1243 * Math.Log(h / d));
            else
                throw new NotImplementedException("Building h/d values of > 5 are not currently supported");
                // TODO: Finish!
                // For greater ratios, Figures 7.23 and 7.36 from EN 1991-1-4 should be used.
        }

        /// <summary>
        /// Calculate the wind frictional force (F_fr)
        /// </summary>
        /// <param name="c_fr">The friction coefficient (see EN 1991-1-4 Table 7.10)</param>
        /// <param name="q_p">The peak velocity pressure.  May substitute for q_p(o) in an orographic situation.</param>
        /// <param name="A_fr">The frictional area.</param>
        /// <returns></returns>
        public double FrictionalForce(double c_fr, double q_p, double A_fr)
        {
            return c_fr * q_p * A_fr;
        }

        /// <summary>
        /// Test-run of wind calculation
        /// </summary>
        /// <returns></returns>
        public void Test(StringBuilder log)
        {
            log.AppendLine("Simple Wind Calc Example");
            log.AppendLine("========================");
            double v_map = 21.5; // Velocity from map
            double A = 0; // Altitude
            double h = 20; // Building height
            double b = 104; // Bredth in m
            double d = 144; // Depth in m

            
            double c_alt = AltitudeFactor(A, 0.6 * h);
            log.AppendLine("c_alt = " + c_alt);

            double c_dir = 1.0; //Directional coefficient - non-directional approach taken!
                                // TODO: Implement directional wind!

            double h_dis = 0.0; // Displacement height - conservatively taken as 0
                                // TODO: Implement displacement height!

            double d_shore = 0.0; // Distance to shore

            double c_e = 3.2; // Exposure factor
            //NON CONSERVATIVE VALUE - MAY BE AS HIGH AS 4.2
            //TODO: Calculate exposure factor properly!

            double c_eT = 1.0; // Town terrain correction factor
            //TODO: This as well!

            double q_p = PeakVelocityPressure(v_map, c_alt, c_dir, c_e, c_eT);
            log.AppendLine("q_p = " + q_p + " Pa");

            double c_o = 1.0; // Orography factor
            // NON-CONSERVATIVE VALUE - not valid for buildings on or near peaks
            //TODO: Calculate Orographic peak velocity pressure

            double delta_s = 0.05; //Logarithmic decrement of structural damping (typical for steel)
            double c_d = 1.011; // Dynamic factor (Non-conservative value!)
            double c_f = ForceCoefficient(h,d); // Force coefficient (non-conservative value!)
            log.AppendLine("c_f = " + c_f);
            double c_s = 1.0; // Size factor (taken conservatively as 1)

            double a_sh = b * h;

            double F_w = WindForce(q_p, c_s, c_d, c_f, a_sh);
            log.AppendLine("F_w = " + F_w + " N");

            if (d*(b + 2*h) < b*h)
            {
                // Total area of surfaces parallel with the wind < 4 * the total area of surfaces perpendicular to it
                // (For a cuboid)
                // Friction may be ignored!
                log.AppendLine("Friction may be ignored.");
            }
            else
            {
                // Friction may not be ignored!
                log.AppendLine("Friction must be considered.");
                double c_fr = 0.04; //Friction coefficient (conservative value for ribbed surfaces)
                double detatchment = Math.Min(2 * b, 4 * h);
                double A_fr = Math.Max((d - detatchment), 0) * (2 * h + b); //Friction area
                double F_fr = FrictionalForce(c_fr, q_p, A_fr);
                log.AppendLine("F_fr = " + F_fr + " N");
                F_w += F_fr;
                log.AppendLine("F_w' = F_w + F_fr = " + F_w + " N");
                // TODO!
            }

            log.AppendLine("Smeared Wind Pressure: " + (F_w / a_sh) + " Pa");

            //throw new NotImplementedException();
        }

    }
}
