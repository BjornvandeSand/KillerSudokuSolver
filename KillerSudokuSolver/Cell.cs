using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Cell
        {
            public SortedSet<int> PossibleValues { get; set; }

            public int Value { get; set; } //The final Value for this Cell

            public Cage Cage { get; set; }

            public Nonet Nonet { get; set; }

            public Column Column { get; set; }

            public Row Row { get; set; }

            public Diagonal Diagional { get; set; }

            public House[] Houses { get; set; }

            public Cell(int maxValue, int housesAmount)
            {
                Value = 0;
                PossibleValues = new SortedSet<int>();
                Houses = new House[housesAmount];

                for (int i = 1; i <= maxValue; i++)
                {
                    PossibleValues.Add(i);
                }
            }

            void Evaluate()
            {
                if (PossibleValues.Count == 1)
                {
                    Value = PossibleValues.ElementAt(0); //Set the final Value

                    //Remove this possible Value from all other Houses associated with this Cell
                    foreach (House house in Houses)
                    {
                        house.RemoveOption(Value);
                    }
                }
            }

            //Provides a String representation of this Cell for easy printing
            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}