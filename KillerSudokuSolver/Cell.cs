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

            public House[] Houses { get; set; }

            public Cell(int maxValue, int housesAmount)
            {
                Value = 0;
                PossibleValues = new HashSet<int>();
                Houses = new House[housesAmount];

                for (int i = 1; i <= maxValue; i++)
                {
                    PossibleValues.Add(i);
                }
            }

            public void RemoveOption(int i)
            {
                if (PossibleValues.Contains(i))
                {
                    PossibleValues.Remove(i);
                    Evaluate();
                }
            }

            void Evaluate()
            {
                if (PossibleValues.Count == 1)
                {
                    Value = PossibleValues.ElementAt(0);
                    Cage.RemoveOption(Value); //Tell the Cage to remove the Value from all other Cells
                    Row.RemoveOption(Value); //Tell the Row to remove the Value from all other Cells
                    Column.RemoveOption(Value); //Tell the Column to remove the Value from all other Cells
                    Nonet.RemoveOption(Value); //Tell the Nonet to remove the Value from all other Cells
                }
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}