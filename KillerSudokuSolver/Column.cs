namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Column : House
        {
            public Column(int x, Cell[] z, int n)
            {
                Id = x;
                Cells = z;
                Goal = n;
            }
        }
    }
}