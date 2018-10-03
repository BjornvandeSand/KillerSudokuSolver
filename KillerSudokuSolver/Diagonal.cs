namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Diagonal : House
        {
            public Diagonal(int x, Cell[] z, int n)
            {
                Id = x;
                Cells = z;
                Goal = n;
            }
        }
    }
}