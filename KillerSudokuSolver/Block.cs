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
    }
}