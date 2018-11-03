using System.Collections.Generic;

namespace KillerSudokuSolver
{
    class RemoveDuplicatePossibilities : Rule
    {
		public static int PossibleValuesEliminated = 0;
		public static int Executions = 0;

		//Removes PossibleValues that are no longer possible in connected Houses, because they're already fulfilled by the Value for this Cell
		public RemoveDuplicatePossibilities(Cell target, float priority)
        {
			Target = target;
            Priority = priority;
        }

        public override HashSet<Cell> Execute()
		{
			Executions++;

			HashSet<Cell> changedCells = new HashSet<Cell>();

			Cell target = Target as Cell;

			if (target.Value != 0 || true) //Means the Cell is finalized
			{
				foreach (House house in target.Houses)
				{
					foreach (Cell cell in house.Cells)
					{
						if (cell.RemovePossibleValueIfPresent(target.Value))
						{
							PossibleValuesEliminated++;
							changedCells.Add(cell);
						}
					}
				}
			}

			return changedCells; //Return the list of upcoming Cells to evaluate
        }
    }
}