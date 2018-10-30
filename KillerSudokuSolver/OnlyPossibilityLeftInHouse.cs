using System.Collections.Generic;

namespace KillerSudokuSolver
{
    class OnlyPossibilityLeftInHouse : Rule
    {
        public OnlyPossibilityLeftInHouse(House target, float priority)
        {
            Target = target;
            Priority = priority;
        }

		//Checks if a possible value is found in only 1 Cell in a House
        public override HashSet<Cell> Execute()
		{
			SortedSet<int> possibleValues = Target.PossibleValues(); //Gathers all the values possible in this House
			HashSet<Cell> changedCells = new HashSet<Cell>(); //Contains all the Cells changed by this Rule application

			int valueCounter; //Keeps track of how many times the current possible Value is found
			Cell cellTracker; // Keeps track of the last Cell found to allow the possible Value

			//This rule is only correct for non-Cage Houses and Cages where the remaining possible Values are all certain to exist in the Cage
			if (!(Target is Cage) || possibleValues.Count == Target.Cells.Length)
			{
				//Goes through all the possible Values
				foreach (int i in possibleValues)
				{
					valueCounter = 0;
					cellTracker = null;

					//Goes through all the Cells in this House
					foreach (Cell cell in Target.Cells)
					{						
						//An instance of the current Value is found
						if (cell.PossibleValues.Contains(i))
						{
							valueCounter++;
							cellTracker = cell;
						}
					}

					//There was only one instance
					if (valueCounter == 1)
					{
						//Remove all other possible Values so only the correct one remains
						for (int j = 1; j <= cellTracker.Row.Cells.Length; j++)
						{
							if (j != i)
							{
								cellTracker.RemovePossibleValueIfPresent(j);
							}
						}

						changedCells.Add(cellTracker); //Add this Cell to the list of changed ones
					}
				}
			}

			return changedCells; //Return the list of changed Cells to evaluate
        }

		public override int GetHashCode()
		{
			return 0;
		}
	}
}