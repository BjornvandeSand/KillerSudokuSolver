using System;
using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        abstract class Rule : IComparable
        {
            public House Target { get; set; }
            public float Priority { get; set; }

            //Executes the Step this object contains on the House it contains and returns a list of affected Cells
            public abstract List<Cell> Execute();

            //Allows the sorting of Steps based on their priority
            public int CompareTo(object obj)
            {
                Rule step = obj as Rule;

                if (Priority == step.Priority)
                {
                    return 0;
                }

                else if (Priority < step.Priority)
                {
                    return -1;
                }

                else
                {
                    return 1;
                }
            }
        }
    }
}