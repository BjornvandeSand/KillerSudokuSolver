using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Diagonal : House
        {
            public Diagonal(int x, Cell[] c, int n, int maxValue)
            {
                Id = x;
                Goal = n;
                Cells = c;

                possibleValues = new HashSet<int>();

                for (int i = 1; i <= maxValue; i++)
                {
                    possibleValues.Add(i);
                }
            }
        }
    }
}