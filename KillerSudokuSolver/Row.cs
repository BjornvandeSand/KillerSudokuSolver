using System;
using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    class Row : House
    {
		readonly int dimension;

        public Row(int y, Cell[] cells, int maxValue, int dimension)
        {
            Id = y;
            Cells = cells;
			this.dimension = dimension;

            PossibleValues = new SortedSet<int>();

            for (int i = 1; i <= maxValue; i++)
            {
                PossibleValues.Add(i);
            }
        }

		public override string ToString()
		{
			string output = Id + 1 + string.Concat(Enumerable.Repeat(" ", Cells.Length.ToString().Length - Id.ToString().Length + 1)) + "| ";
			int maxLength = Cells.Length.ToString().Length;

			string cellOutput;

			for (int i = 0; i < Cells.Length; i++)
			{
				cellOutput = Cells[i].ToString();
				output += cellOutput + string.Concat(Enumerable.Repeat(" ", maxLength - cellOutput.Length + 1));

				if ((i + 1) % dimension == 0)
				{
					output += "| ";
				}
			}

            output += Id + 1 + Environment.NewLine;

            return output;
        }
    }
}