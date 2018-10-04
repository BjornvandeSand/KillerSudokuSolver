namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Diagonal : House
        {
            public Diagonal(int x, Cell[] z, int n)
            {
                Id = x;
                Goal = n;
                Cells = z;
            }
        }
    }
}