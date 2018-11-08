using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    class NCageN : Rule
    {
		public static int PossibleValuesEliminated = 0;
		public static int Executions = 0;

		public NCageN(Cage target, float priority)
        {
            Target = target;
            Priority = priority;
        }

        public override HashSet<Cell> Execute()
		{
			Executions++;

			Cage target = (Cage) Target.GenerateSuccessor();

			HashSet<Cell> changedCells = new HashSet<Cell>();

			SortedSet<int> possibleValues = target.PossibleValues(); //Get a set of all Possible Values in this Cage

			if(target.Cells.Length != 0 && target.AllPossibleValuesKnown())
			{
				//Go through all Houses, except the first as that's the Cage we're evaluating
				for (int i = 1; i < 4; i++)
				{
					bool sameHouse = true;
					House currentHouse = target.Cells[0].Houses[i];

					//Check if all Cells in this Cage share this House
					foreach (Cell cell in target.Cells)
					{
						sameHouse &= currentHouse == cell.Houses[i];
					}

					//All Cells in this Cage are part of this House
					if (sameHouse)
					{
						//So remove their possible values elsewhere
						foreach (Cell cell in currentHouse.Cells)
						{
							//Provided they're not part of the Cage we're evaluating
							if (!target.Cells.Contains(cell))
							{
								foreach (int possibleValue in possibleValues)
								{
									if (cell.RemovePossibleValueIfPresent(possibleValue))
									{
										PossibleValuesEliminated++;

										changedCells.Add(cell); //Add it to the list of upcoming evaluations
									}
								}
							}
						}
					}
				}

				//DOESN'T TAKE KILLERX INTO CONSIDERATION ATM
				//Diagonal is the potential 5th House
			}

			return changedCells; //Return the list of upcoming Cells to evaluate
        }

		public override int GetHashCode()
		{
			return 0;
		}
	}
}