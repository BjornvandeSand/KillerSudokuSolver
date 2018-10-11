using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
		class Nonet : House
        {

            public Nonet(Cell[] c, int maxValue)
            {
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