using System;
using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    partial class Program
    {
		class Nonet : House
        {
            Cell[] cells;

            public Nonet(Cell[] c, int n)
            {
                cells = c;
                Goal = n;
            }
        }
	}
}