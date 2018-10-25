using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    class RemoveHighLow : Rule
    {
        //Removes Values that are too high or low to be possible with the Cage's sum and returns affected Cells
        public RemoveHighLow(Cage target, float priority)
        {
            Target = target;
            Priority = priority;
        }

        public override HashSet<Cell> Execute()
		{
			Cage target = (Cage) Target.GenerateSuccessor();

			HashSet<Cell> changedCells = new HashSet<Cell>();

			int tempSum = 0; //Accumulator for values as we approach the Goal

			SortedSet<int> possibleValues = target.PossibleValues();

			for (int i = 0; i < target.Cells.Length - 1; i++)
			{
				tempSum += possibleValues.ElementAt(i);
			}

			int max = target.Goal - tempSum;

			tempSum = 0;

			for (int i = possibleValues.Count - 1; i > possibleValues.Count - target.Cells.Length; i--)
			{
				tempSum += possibleValues.ElementAt(i);
			}

			int min = target.Goal - tempSum;

			foreach (Cell cell in target.Cells)
            {
                //This part removes possible Values that are are too high
                if (max < target.Cells[0].Block.Cells.Length) //Determines if there's any possible Values low enough to cull
                {
                    for (int i = max + 1; i <= target.Cells[0].Block.Cells.Length; i++)
                    {
						//Check if this Value is still listed as possible for the Cell and remove it if so
						if (cell.RemovePossibleValueIfPresent(i))
						{
							changedCells.Add(cell); //Add it to the list of upcoming evaluations, it won't be added if the Cell is already in the HashSet
                        }
                    }
                }

                //This part removes possible Values that are too low
                if (min > 0) //Determines if there's any possible Values high enough to cull
                {
                    for (int i = min - 1; i > 0; i--)
                    {
                        //Check if this Value is still listed as possible for the Cell and remove it if so
                        if (cell.RemovePossibleValueIfPresent(i))
                        {
                            if (!changedCells.Contains(cell)) //Check if this Cell isn't already listed as one that should be re-evaluated
                            {
								changedCells.Add(cell); //Add it to the list of upcoming evaluations
                            }
                        }
                    }
                }
			}

			return changedCells; //Return the list of upcoming Cells to evaluate
        }
    }
}