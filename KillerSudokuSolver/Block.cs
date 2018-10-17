using System.Collections.Generic;

namespace KillerSudokuSolver
{
	class Block : House
    {
        public Block(Cell[] c, int maxValue)
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