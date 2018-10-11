using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Cage : House
        {
            readonly char type;

            public Cage(int id, Cell[] cells, int goal, char type)
            {
                Id = id;
                Cells = cells;
                Goal = goal;
                this.type = type;

                PossibleValues = new SortedSet<int>();

                foreach (int value in Cells[0].PossibleValues)
                {
                    PossibleValues.Add(value);
                }
            }
        }
    }
}