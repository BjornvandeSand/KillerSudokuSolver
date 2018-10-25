using System.Collections.Generic;

namespace KillerSudokuSolver
{
    abstract class House
    {
        public Cell[] Cells { get; set; }

        public int Id { get;  set; }

		public char Operation;

		public SortedSet<int> PossibleValues()
		{
			SortedSet<int> output = new SortedSet<int>();

			foreach (Cell cell in Cells)
			{
				output.UnionWith(cell.PossibleValues);
			}

			return output;
		}

		public List<Cell> RemovePossibleValue(int i)
		{
			List<Cell> output = new List<Cell>(Cells.Length);

			foreach (Cell cell in Cells)
			{
				if (cell.RemovePossibleValueIfPresent(i))
				{
					output.Add(cell);
				}
			}

			return output;
		}

		public abstract House GenerateSuccessor();
	}
}