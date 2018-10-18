using System.Collections.Generic;

namespace KillerSudokuSolver
{
    class RemoveDuplicatePossibilities : Rule
    {
        //Removes PossibleValues that are no longer possible in connected Houses, because they're already fulfilled by the Value for this Cell
        public RemoveDuplicatePossibilities(Cell target, float priority)
        {
			Target = target;
            Priority = priority;
        }

        public override List<Cell> Execute() {
			List<Cell> changedCells = new List<Cell>();

			Cell target = Target as Cell;

			if(target.Value != 0) //Means the Cell is finalized
			{
				foreach (House house in target.Houses)
				{
					changedCells.AddRange(house.RemovePossibleValue(target.Value));
				}
			}

			return changedCells; //Return the list of upcoming Cells to evaluate
        }
    }
}