using System;
using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class SolveStep : IComparable
        {
            readonly House target;
            readonly float priority;

            public SolveStep(House target, float priority)
            {
                this.target = target;
                this.priority = priority;
            }

            public List<Cell> Execute()
            {
                List<Cell> output = new List<Cell>();
                //RemoveHighLow(target);

                return output;
            }

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