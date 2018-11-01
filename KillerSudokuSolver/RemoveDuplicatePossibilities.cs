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

        public override HashSet<Cell> Execute()
		{
			HashSet<Cell> changedCells = new HashSet<Cell>();

			Cell target = Target as Cell;

			if (target.Value != 0 || true) //Means the Cell is finalized
			{
				foreach (House house in target.Houses)
				{
					changedCells.UnionWith(house.RemovePossibleValue(target.Value));
				}
			}

			return changedCells; //Return the list of upcoming Cells to evaluate
        }
    }
}