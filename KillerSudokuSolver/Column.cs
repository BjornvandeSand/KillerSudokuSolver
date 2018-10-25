using System.Collections.Generic;

namespace KillerSudokuSolver
{
    class Column : House
    {
        public Column(int x, Cell[] cells)
        {
            Id = x;
            Cells = cells;
        }

		public override House GenerateSuccessor()
		{
			List<Cell> incompleteCells = new List<Cell>(Cells.Length);

			foreach (Cell cell in Cells)
			{
				if (cell.Value != 0)
				{
					incompleteCells.Add(cell);
				}
			}

			return new Column(Id, incompleteCells.ToArray());
		}
	}
}