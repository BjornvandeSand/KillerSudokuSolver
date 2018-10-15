using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Diagonal : House
        {
            public Diagonal(int id, Cell[] cells, int maxValue)
            {
                Id = id;
                Cells = cells;

                PossibleValues = new SortedSet<int>();

                for (int i = 1; i <= maxValue; i++)
                {
                    PossibleValues.Add(i);
                }
            }
        }
    }
}