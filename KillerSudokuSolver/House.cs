namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class House
        {
            public int Id { get; set; }
            public int Goal { get; set; }

            public Cell[] Cells { get; set; }
        }
    }
}