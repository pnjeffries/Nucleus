using FreeBuild.Actions;
using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Manager class to store, restore and merge model objects
    /// </summary>
    public class ModelSourceHistory
    {
        #region Properties

        /// <summary>
        /// The map of objects to sources, stored as a list of UniqueObjects indexed by iteration and keyed by source reference
        /// </summary>
        public IDictionary<string, IList<IList<ModelObject>>> SourceMap { get; }
           = new Dictionary<string, IList<IList<ModelObject>>>();

        #endregion

        #region Methods

        /// <summary>
        /// Get a previously stored object
        /// </summary>
        /// <param name="exInfo"></param>
        /// <returns>The object previously stored with equivalent execution information, 
        /// or null if no previously stored object exists</returns>
        public ModelObject Get(ExecutionInfo exInfo)
        {
            if (SourceMap.ContainsKey(exInfo.SourceReference))
            {
                IList<IList<ModelObject>> iterations = SourceMap[exInfo.SourceReference];
                if (iterations.Count > exInfo.Iteration)
                {
                    IList<ModelObject> iteration = iterations[exInfo.Iteration];
                    if (iteration.Count > exInfo.HistoryItemCount) return iteration[exInfo.HistoryItemCount];
                }
            }
            return null;
        }

        /// <summary>
        /// Store a generated object keyed by the given execution info of its generating action
        /// </summary>
        /// <param name="exInfo"></param>
        /// <param name="unique"></param>
        public void Set(ExecutionInfo exInfo, ModelObject unique)
        {
            if (exInfo != null && exInfo.SourceReference != null)
            {
                IList<IList<ModelObject>> iterations;
                if (SourceMap.ContainsKey(exInfo.SourceReference)) iterations = SourceMap[exInfo.SourceReference];
                else
                {
                    iterations = new List<IList<ModelObject>>();
                    SourceMap[exInfo.SourceReference] = iterations;
                }
                while (exInfo.Iteration >= iterations.Count) iterations.Add(null);
                IList<ModelObject> iteration = iterations[exInfo.Iteration];
                if (iteration == null)
                {
                    iteration = new List<ModelObject>();
                    iterations[exInfo.Iteration] = iteration;
                }
                if (exInfo.HistoryItemCount < iteration.Count) iteration[exInfo.HistoryItemCount] = unique;
                else iteration.Add(unique);
                exInfo.HistoryItemCount++;
            }
        }

        /// <summary>
        /// Replace the previous data stored with this execution information with the specified new value.
        /// If a stored object exists and is of the same type as the new one, it will be updated to match the new values
        /// otherwise the record will be replaced with the new object.
        /// </summary>
        /// <param name="exInfo"></param>
        /// <param name="unique"></param>
        /// <returns>The current object - either the orginal stored value if it was updated, or the new object
        /// if it was replaced</returns>
        public ModelObject Update(ExecutionInfo exInfo, ModelObject unique)
        {
            if (exInfo != null)
            {
                ModelObject original = Get(exInfo);
                if (original != null && unique != null && original.GetType() == unique.GetType())
                {
                    original.CopyFieldsFrom(unique);
                    original.Undelete();
                    exInfo.HistoryItemCount++;
                    return original;
                }
                else
                {
                    if (unique != null)
                        Set(exInfo, unique);
                    else exInfo.HistoryItemCount++;
                    if (original != null) original.Delete();
                    return unique;
                }
            }
            else return unique;
        }

        /// <summary>
        /// Mark all items after the specified iteration number as deleted
        /// </summary>
        /// <param name="sourceRef"></param>
        /// <param name="iteration"></param>
        public void CleanSubsequentIterations(string sourceRef, int iteration)
        {
            if (SourceMap.ContainsKey(sourceRef))
            {
                IList<IList<ModelObject>> iterations = SourceMap[sourceRef];
                for (int i = iteration + 1; i < iterations.Count; i++)
                {
                    IList<ModelObject> uniques = iterations[i];
                    foreach (ModelObject unique in uniques)
                    {
                        if (unique != null) unique.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// Mark all items after the iteration number of the execution info as deleted
        /// </summary>
        /// <param name="exInfo"></param>
        public void CleanSubsequentIterations(ExecutionInfo exInfo)
        {
            CleanSubsequentIterations(exInfo.SourceReference, exInfo.Iteration);
        }

        /// <summary>
        /// Mark all items in the specified iteration after the specified historyItemCount as deleted
        /// </summary>
        /// <param name="sourceRef"></param>
        /// <param name="iteration"></param>
        /// <param name="historyItemCount"></param>
        public void CleanIteration(string sourceRef, int iteration, int historyItemCount)
        {
            if (SourceMap.ContainsKey(sourceRef))
            {
                IList<IList<ModelObject>> iterations = SourceMap[sourceRef];
                for (int i = iteration + 1; i < iterations.Count; i++)
                {
                    IList<ModelObject> uniques = iterations[i];
                    for (int j = historyItemCount; j < uniques.Count; j++)
                    {
                        ModelObject unique = uniques[j];
                        if (unique != null) unique.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// Mark all items after the current iteration after the current historyItemCount as deleted
        /// </summary>
        /// <param name="exInfo"></param>
        public void CleanIteration(ExecutionInfo exInfo)
        {
            CleanIteration(exInfo.SourceReference, exInfo.Iteration, exInfo.HistoryItemCount);
        }

        #endregion
    }
}
