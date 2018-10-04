namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class Cage : House
        {
            char type;

            public Cage(int id, Cell[] x, int n, char o)
            {
                Id = id;
                Cells = x;
                Goal = n;
                type = o;
            }
        }
    }
}