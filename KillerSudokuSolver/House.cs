using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        abstract class House
        {
            public int Id { get; set; }
            public int Goal { get; set; }
            public HashSet<int> possibleValues;

            public Cell[] Cells { get; set; }

            //Removes this option from all Cells in the House
            public void removeOption(int i)
            {
                foreach (Cell cell in Cells)
                {
                    if (possibleValues.Contains(i))
                    {
                        cell.removeOption(i);
                    }
                }
            }
        }
    }
}