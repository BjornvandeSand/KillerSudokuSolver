using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    class Cell : House
    {
        public int Value { get; set; } //The final Value for this Cell

        public Cage Cage { get; set; }

        public Block Block { get; set; }

        public Column Column { get; set; }

        public Row Row { get; set; }

        public Diagonal Diagonal { get; set; }

        public List<House> Houses { get; set; }

		public new SortedSet<int> PossibleValues { get; set; }

		public Cell(int maxValue, int housesAmount)
        {
            Value = 0;
            PossibleValues = new SortedSet<int>();
            Houses = new List<House>(housesAmount);

            for (int i = 1; i <= maxValue; i++)
            {
                PossibleValues.Add(i);
            }
        }

        void EvaluatePossibleValues()
        {
            if (PossibleValues.Count == 1)
            {
				Value = PossibleValues.ElementAt(0); //Set the final Value
				PossibleValues.Clear();
			}
        }

		public bool RemovePossibleValueIfPresent(int i)
		{
			bool change = false;

			if (PossibleValues.Contains(i))
			{
				PossibleValues.Remove(i);
				change = true;
			}

			EvaluatePossibleValues();

			return change;
		}

		//Provides a String representation of this Cell for easy printing
		public override string ToString()
        {
            return Value.ToString();
        }
    }
}