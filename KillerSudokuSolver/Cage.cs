using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Cage : House
        {
            readonly char type;

            public Cage(int id, Cell[] c, int n, char type, int maxValue)
            {
                Id = id;
                Cells = c;
                Goal = n;
                this.type = type;

                possibleValues = new HashSet<int>();

                for (int i = 1; i <= maxValue; i++)
                {
                    possibleValues.Add(i);
                }
            }
        }
    }
}