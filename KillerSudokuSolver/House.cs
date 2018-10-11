using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        abstract class House
        {
            public Cell[] Cells { get; set; }
            public int Id { get; set; }
            public SortedSet<int> PossibleValues { get; set; }
			public int Goal { get; set; }

			//Removes this option from all Cells in the House
			public void RemoveOption(int i)
            {
                foreach (Cell cell in Cells)
                {
                    cell.PossibleValues.Remove(i);
                }
            }
        }
    }
}