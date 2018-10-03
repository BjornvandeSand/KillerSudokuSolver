using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class Program
    {
        class Cell
        {
            public int Value { get; set; }

            public HashSet<int> PossibleValues { get; set; } //The Values that are still possible for this Cell

            Column Column { get; set; }

            Row Row { get; set; }

            public Cage Cage { get; set; }

            public Cell(int n)
            {
                Value = 0;
                PossibleValues = new HashSet<int>();

                for (int i = 1; i <= n; i++)
                {
                    PossibleValues.Add(i);
                }
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}