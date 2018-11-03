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

		public bool AllPossibleValuesKnown()
		{
			return PossibleValues().Count == Cells.Length;
		}

		public abstract House GenerateSuccessor();
	}
}