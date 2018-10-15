using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class RemoveDuplicatePossibilities : Rule
        {
            //Removes PossibleValues that are no longer possible, because they're already the final Value for this Cell
            public RemoveDuplicatePossibilities(House target, float priority)
            {
                Target = target;
                Priority = priority;
            }

            public override List<Cell> Execute() {
				Cell Target = this.Target as Cell;

				List<Cell> output = new List<Cell>();

				if(Target.Value != 0)
				{
					foreach(House house in Target.Houses)
					{
						output.AddRange(house.RemovePossibleValue(Target.Value));
					}
				}

				return output; //Return the list of upcoming Cells to evaluate
            }
        }
    }
}