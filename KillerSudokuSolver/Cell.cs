using System;
using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Cell
        {
            public int Value { get; set; } //The final Value for this Cell

            public Cage Cage { get; set; }

            public HashSet<int> PossibleValues { get; set; } //The Values that are still possible for this Cell

            public Nonet Nonet { get; set; }

            public Column Column { get; set; }

            public Row Row { get; set; }

            public Diagonal Diagional { get; set; }

            public Cell(int maxValue)
            {
                Value = 0;
                PossibleValues = new HashSet<int>();

                for (int i = 1; i <= maxValue; i++)
                {
                    PossibleValues.Add(i);
                }
            }

            public void removeOption(int i)
            {
                if (PossibleValues.Contains(i))
                {
                    PossibleValues.Remove(i);
                    evaluate();
                }
            }

            void evaluate()
            {
                if (PossibleValues.Count == 1)
                {
                    Value = PossibleValues.ElementAt(0);
                    Cage.removeOption(Value); //Tell the Cage to remove the Value from all other Cells
                    Row.removeOption(Value); //Tell the Row to remove the Value from all other Cells
                    Column.removeOption(Value); //Tell the Column to remove the Value from all other Cells
                    Nonet.removeOption(Value); //Tell the Nonet to remove the Value from all other Cells
                }
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}