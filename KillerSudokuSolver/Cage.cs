namespace KillerSudokuSolver
{
    partial class Program
    {
        class Cage : House
        {
            char type;

            public Cage(Cell[] x, int n, char o)
            {
                Cells = x;
                Goal = n;
                type = o;
            }
        }
    }
}