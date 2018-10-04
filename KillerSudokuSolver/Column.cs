using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Column : House
        {
            public Column(int x, Cell[] c, int n, int maxValue)
            {
                Id = x;
                Cells = c;
                Goal = n;

                possibleValues = new HashSet<int>();

                for (int i = 1; i <= maxValue; i++)
                {
                    possibleValues.Add(i);
                }
            }
        }
    }
}