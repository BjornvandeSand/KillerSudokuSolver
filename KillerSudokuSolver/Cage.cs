using System.Collections.Generic;

namespace KillerSudokuSolver
{
    class Cage : House
    {
		public int Goal;

		public Cage(int id, Cell[] cells, int goal, char operation)
		{
			Id = id;
			Cells = cells;
			Goal = goal;
			Operation = operation;
		}

		public override House GenerateSuccessor()
		{
			int updatedGoal = Goal;
			List<Cell> incompleteCells = new List<Cell>(Cells.Length);

			foreach (Cell cell in Cells)
			{
				if (cell.Value != 0)
				{
					updatedGoal -= cell.Value;
				}

				else
				{
					incompleteCells.Add(cell);
				}
			}

			return new Cage(Id, incompleteCells.ToArray(), updatedGoal, Operation);
		}
	}
}