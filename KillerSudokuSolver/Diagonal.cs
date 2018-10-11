using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Diagonal : House
        {
            public Diagonal(int x, Cell[] c, int maxValue)
            {
                Id = x;
                Cells = c;

                PossibleValues = new SortedSet<int>();

                for (int i = 1; i <= maxValue; i++)
                {
                    PossibleValues.Add(i);
                }
            }
        }
    }
}