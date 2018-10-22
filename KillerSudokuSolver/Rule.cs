using System;
using System.Collections.Generic;

namespace KillerSudokuSolver
{
    abstract class Rule : IComparable<Rule>
    {
        public House Target { get; set; }
        public float Priority { get; set; }

        //Executes the Step this object contains on the House it contains and returns a list of affected Cells
        public abstract HashSet<Cell> Execute();

		public int CompareTo(Rule other)
		{
			if (Priority == other.Priority)
			{
				return 0;
			}

			else if (Priority > other.Priority)
			{
				return -1;
			}

			else
			{
				return 1;
			}
		}
	}
}