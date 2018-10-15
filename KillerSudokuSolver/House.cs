using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        abstract class House
        {
            public Cell[] Cells { get; set; }
            public int Id { get;  set; }
            public SortedSet<int> PossibleValues { get; set; }

			public List<Cell> RemovePossibleValue(int i)
			{
				List<Cell> output = new List<Cell>();
				foreach(Cell cell in Cells)
				{
					if(cell.RemovePossibleValueIfPresent(i))
					{
						output.Add(cell);
					}
					
				}

				return output;
			}
		}
    }
}