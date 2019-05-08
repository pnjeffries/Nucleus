using Nucleus.Extensions;
using Nucleus.Maths;
using Nucleus.Optimisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Tests
{
    public static class Optimisation_Tests
    {
        public static void GeneticAlgorithmTest()
        {
            // In this test case, the GA is tasked with finding two numbers A and B
            // where A to the power of B = 100 and the difference between A and B is
            // as great as possible.

            var settings = new GeneticAlgorithmSettings()
            {
                Objective = OptimisationObjective.Minimise,
                MaxGenerations = 1000,
                 WildcardRate = 0.1,
                 InitialBoost = 5,
            };
            var solver = new TestGASolver(settings);
            var result = solver.Run();
            var value = solver.CalculateValue(result);
            Core.Print("Result: " + result[0] + " ^ " + result[1] + " = " + value);
        }

        public static void SimulatedAnnealingTest()
        {
            var settings = new SimulatedAnnealingSettings()
            {
                Objective = OptimisationObjective.Minimise,
                MaxIterations = 50000
            };
            var solver = new TestAnnealingSolver();
            solver.Settings = settings;
            var result = solver.Run();
            var value = solver.CalculateValue(result);
            Core.Print("Result: " + result[0] + " ^ " + result[1] + " = " + value);
        }

        public class TestGASolver : GeneticSolverBase<double[]>
        {
            public double Range { get; set; } = 10;

            public double Target { get; set; } = 100;

            public TestGASolver(GeneticAlgorithmSettings settings) : base(settings) { }

            public virtual double[] Run()
            {
                // Create the initial population
                IList<double[]> population = CreateInitialPopulation();

                double bestFitness = double.MaxValue;

                // Iterate
                for (int i = 0; i < Settings.MaxGenerations; i++)
                {
                    if (TerminationConditionsMet(population)) break;
                    else population = SpawnNextGeneration(population);

                    var result = Fittest(population);
                    var fitness = OverallFitness(result);
                    if (fitness < bestFitness)
                    {
                        var value = CalculateValue(result);
                        bestFitness = fitness;
                        Core.Print("Iteration " + i + ": " + result[0] + " ^ " + result[1] + " = " + value);
                    }
                }

                // Return the best
                return Fittest(population);
            }


            public override double[] Breed(double[] mother, double[] father)
            {
                var result = new double[2];
                for (int i = 0; i < result.Length; i++)
                {
                    int rando = Settings.RNG.Next(3);
                    if (rando == 1)
                    {
                        result[i] = mother[i];
                    }
                    else if (rando == 2)
                    {
                        result[i] = father[i];
                    }
                    else result[i] = mother[i].Interpolate(father[i], Settings.RNG.NextDouble());
                    if (Settings.RNG.NextDouble() > 0.25)
                        result[i] += (Settings.RNG.NextDouble() - 0.5) * Range/100; //Mutation
                    result[i] = result[i].Limit(0, Range);
                }
                return result;
            }

            public override double[] GeneratePhenotype()
            {
                return new double[] 
                { Settings.RNG.NextDouble() * Range,
                    Settings.RNG.NextDouble() * Range};
            }

            public override double OverallFitness(double[] option)
            {
                double value = CalculateValue(option);
                return (Target - value).Abs() - (option[0] - option[1]).Abs();
            }

            public double CalculateValue(double[] option)
            {
                return option[0].Power(option[1]);
            }
        }

        public class TestAnnealingSolver :
            AnnealingSolverBase<double[]>
        {
            public double Range { get; set; } = 10;

            public double Target { get; set; } = 100;

            public double CalculateValue(double[] option)
            {
                return option[0].Power(option[1]);
            }

            public override double OverallFitness(double[] option)
            {
                double value = CalculateValue(option);
                return (Target - value).Abs() - (option[0] - option[1]).Abs();
            }

            public override double[] GeneratePhenotype()
            {
                return new double[]
                { Settings.RNG.NextDouble() * Range,
                    Settings.RNG.NextDouble() * Range};
            }

            public override double[] GenerateNext(double[] current, double temperature)
            {
                var result = new double[current.Length];
                int iMod = Settings.RNG.Next(current.Length);
                for (int i = 0; i < current.Length; i++)
                {
                    if (i == iMod)
                    {
                        double newValue = Settings.RNG.NextDoubleNear(current[iMod], Range * temperature.Squared());
                        newValue = newValue.Limit(0, Range);
                        result[i] = newValue;
                    }
                    else
                        result[i] = current[i];
                }
                return result;
            }

            public override void StoreNewBest(double[] newBest)
            {
                base.StoreNewBest(newBest);
                double value = CalculateValue(Best);
                Core.Print("Iteration " + Iteration + ": " + Best[0] + " ^ " + Best[1] + " = " + value);
            }
        }
    }
}
