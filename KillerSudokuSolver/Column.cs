namespace KillerSudokuSolver
{
    class Column : House
    {
        public Column(int x, Cell[] cells)
        {
            Id = x;
            Cells = cells;
        }
    }
}