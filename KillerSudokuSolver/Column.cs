namespace KillerSudokuSolver
{
    partial class Program
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