using System.Collections.Generic;

namespace KillerSudokuSolver
{
	class Diagonal : House
	{
		public Diagonal(int id, Cell[] cells)
		{
			Id = id;
			Cells = cells;
		}

		public override House GenerateSuccessor()
		{
			List<Cell> incompleteCells = new List<Cell>(Cells.Length);

			foreach (Cell cell in Cells)
			{
				if (cell.Value == 0)
				{
					incompleteCells.Add(cell);
				}
			}

			return new Diagonal(Id, incompleteCells.ToArray());
		}
	}
}