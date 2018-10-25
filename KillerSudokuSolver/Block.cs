using System.Collections.Generic;

namespace KillerSudokuSolver
{
	class Block : House
    {
        public Block(int id, Cell[] c)
        {
			Id = id;
			Cells = new Cell[c.Length];

			for (int i = 0; i < c.Length; i++)
			{
				Cells[i] = c[i];
			}
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

			return new Block(Id, incompleteCells.ToArray());
		}
	}
}