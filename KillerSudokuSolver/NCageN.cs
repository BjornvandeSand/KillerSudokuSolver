using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    class NCageN : Rule
    {
        public NCageN(Cage target, float priority)
        {
            Target = target;
            Priority = priority;
        }

        public override HashSet<Cell> Execute()
		{
			Cage target = (Cage) Target.GenerateSuccessor();

			HashSet<Cell> changedCells = new HashSet<Cell>();

			SortedSet<int> possibleValues = target.PossibleValues(); //Get a set of all Possible Values in this Cage

			if(target.Cells.Length != 0 && target.AllPossibleValuesKnown())
			{
				//System.Console.WriteLine("CANDIDATE DETECTED!!!");

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
						//System.Console.WriteLine("SAME HOUSE DETECTED!!!");
						//System.Console.WriteLine(currentHouse.GetType() + " " + currentHouse.Id);

						//So remove their possible values elsewhere
						foreach (Cell cell in currentHouse.Cells)
						{
							//Provided they're not part of the Cage we're evaluating
							if (!target.Cells.Contains(cell))
							{
								foreach (int possibleValue in possibleValues)
								{
									//System.Console.WriteLine(possibleValue);
									if (cell.RemovePossibleValueIfPresent(possibleValue))
									{
										//System.Console.WriteLine(possibleValue + " removed in " + cell.Column.Id + "" + cell.Row.Id);
										changedCells.Add(cell); //Add it to the list of upcoming evaluations
									}
								}
							}
						}
					}
				}

				//DOESN'T TAKE KILLX INTO CONSIDERATION ATM
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