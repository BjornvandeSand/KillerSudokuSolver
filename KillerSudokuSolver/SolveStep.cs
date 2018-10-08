using System;
using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class SolveStep : IComparable
        {
            readonly House target;
            //readonly function step
            readonly float priority;

            public SolveStep(House target, float priority)
            {
                this.target = target;
                //this.step = function;
                this.priority = priority;
            }

            //Executes the Step this object contains on the House it contains and returns a list of affected Cells
            public List<Cell> Execute()
            {
                List<Cell> output = new List<Cell>();
                //RemoveHighLow(target);

                return output;
            }

            //Allows the sorting of Steps based on their priority
            public int CompareTo(object obj)
            {
                SolveStep step = obj as SolveStep;

                if (priority == step.priority)
                {
                    return 0;
                }

                else if (priority < step.priority)
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