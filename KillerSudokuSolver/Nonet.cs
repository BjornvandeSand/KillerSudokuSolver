using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
		class Nonet : House
        {

            public Nonet(Cell[] c, int n, int maxValue)
            {
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