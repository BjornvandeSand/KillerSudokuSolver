using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    class RemoveHighLow : Rule
    {
        //Removes Values that are too high or low to be possible with the Cage's sum and returns affected Cells
        public RemoveHighLow(Cage target, float priority)
        {
            this.Target = target;
            this.Priority = priority;
        }

        public override List<Cell> Execute() {
			Cage Target = this.Target as Cage;

            List<Cell> output = new List<Cell>(Target.Cells.Length);

			int tempSum = 0; //Accumulator for values as we approach the Goal

			for (int i = 0; i < Target.Cells.Length - 1; i++)
			{
				tempSum += Target.PossibleValues.ElementAt(i);
			}

			int max = Target.Goal - tempSum;

			tempSum = 0;

			for (int i = Target.PossibleValues.Count - 1; i > Target.PossibleValues.Count - Target.Cells.Length; i--)
			{
				tempSum += Target.PossibleValues.ElementAt(i);
			}

			int min = Target.Goal - tempSum;

			foreach (Cell cell in Target.Cells)
            {
                //This part removes possible Values that are are too high
                if (max < Target.Cells[0].Block.Cells.Length) //Determines if there's any possible Values low enough to cull
                {
                    for (int i = max + 1; i <= Target.Cells[0].Block.Cells.Length; i++)
                    {
						//Check if this Value is still listed as possible for the Cell and remove it if so
						if (cell.RemovePossibleValueIfPresent(i))
						{
                            if (!output.Contains(cell)) //Check if this Cell isn't already listed as one that should be re-evaluated
                            {
                                output.Add(cell); //Add it to the list of upcoming evaluations
                            }
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
                            if (!output.Contains(cell)) //Check if this Cell isn't already listed as one that should be re-evaluated
                            {
                                output.Add(cell); //Add it to the list of upcoming evaluations
                            }
                        }
                    }
                }
			}

			return output; //Return the list of upcoming Cells to evaluate
        }
    }
}