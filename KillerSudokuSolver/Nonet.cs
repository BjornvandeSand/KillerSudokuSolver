namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
		class Nonet : House
        {
            Cell[] cells;

            public Nonet(Cell[] c, int n)
            {
                cells = c;
                Goal = n;
            }
        }
	}
}