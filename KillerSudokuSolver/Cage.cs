namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
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