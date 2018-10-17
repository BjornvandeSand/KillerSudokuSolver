using System.Collections.Generic;

namespace KillerSudokuSolver
{
    class Cage : House
    {
        readonly char type;
		public int Goal { get; }

		public Cage(int id, Cell[] cells, int goal, char type, SortedSet<int> possibleValues)
		{
			Id = id;
			Cells = cells;
			Goal = goal;
			this.type = type;
			PossibleValues = possibleValues;

			PossibleValues = new SortedSet<int>();

			foreach (int value in Cells[0].PossibleValues)
			{
				PossibleValues.Add(value);
			}
		}

		public Cage GenerateSuccessor()
		{
			int updatedGoal = Goal;
			List<Cell> incompleteCells = new List<Cell>(Cells.Length);

			foreach(Cell cell in Cells)
			{
				if(cell.Value != 0)
				{
					updatedGoal -= cell.Value;
				}

				else
				{
					incompleteCells.Add(cell);
				}
			}

			return new Cage(Id,incompleteCells.ToArray(),updatedGoal,type,PossibleValues);
		}
    }
}