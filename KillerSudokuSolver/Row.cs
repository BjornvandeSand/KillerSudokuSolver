using System;
using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Row : House
        {
            public Row(int y, Cell[] c, int n, int maxValue)
            {
                Id = y;
                Goal = n;
                Cells = c;

                possibleValues = new HashSet<int>();

                for (int i = 1; i <= maxValue; i++)
                {
                    possibleValues.Add(i);
                }
            }

            public override string ToString()
            {
                string output = Id + 1 + String.Concat(Enumerable.Repeat(" ", Cells.Length.ToString().Length - Id.ToString().Length + 1)) + "| ";
                int maxLength = Cells.Length.ToString().Length;

                string cellOutput;

                foreach (Cell cell in Cells)
                {
                    cellOutput = cell.ToString();
                    output += cellOutput + String.Concat(Enumerable.Repeat(" ", maxLength - cellOutput.Length + 1));
                }

                output += "| " + (Id + 1) + Environment.NewLine;

                return output;
            }
        }
    }
}