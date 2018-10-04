namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
		class Nonet : House
        {

            public Nonet(Cell[] c, int n)
            {
                Goal = n;
                Cells = c;
            }
        }
	}
}