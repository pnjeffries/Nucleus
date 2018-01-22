﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A generic interface for maps of cells
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICellMap<T>
    {
        /// <summary>
        /// Get or set the contents of the cell at the specified cell index
        /// </summary>
        /// <param name="cellIndex">The 1-dimensional cell index</param>
        /// <returns></returns>
        T this[int cellIndex] { get; set; }

        /// <summary>
        /// Does a cell exist at the specified index?
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        bool Exists(int cellIndex);

        /// <summary>
        /// Get the index of the cell at the specified location
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        int IndexAt(Vector location);

        /// <summary>
        /// Get the cell index of the specified adjacent cell to the specified cell
        /// </summary>
        /// <param name="cellIndex">The index of the starting cell</param>
        /// <param name="adjacencyIndex">The adjacency index of the cell to retrieve</param>
        /// <returns></returns>
        int AdjacentCell(int cellIndex, int adjacencyIndex);

        /// <summary>
        /// Get the cell index of the cell adjacent to the cell with the specified
        /// index in the specified direction.
        /// </summary>
        /// <param name="cellIndex">The index of the starting cell</param>
        /// <param name="direction">The direction of the cell to retrieve</param>
        /// <returns></returns>
        int AdjacentCell(int cellIndex, Vector direction);
    }
}