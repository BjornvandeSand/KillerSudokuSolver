using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Column : House
        {
            public Column(int x, Cell[] cells, int maxValue)
            {
                Id = x;
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