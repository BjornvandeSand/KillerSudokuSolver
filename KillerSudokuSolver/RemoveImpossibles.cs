using System;
using System.Collections.Generic;

namespace KillerSudokuSolver
{
    class RemoveImpossibles : Rule
    {
        //Removes Values that are could not possibly be used to form the Goal
        public RemoveImpossibles(Cage target, float priority)
        {
            Target = target;
            Priority = priority;
        }

		public override HashSet<Cell> Execute()
		{
			Cage Target = this.Target as Cage;
			Target = Target.GenerateSuccessor();

			int[] possibilities = new int[Target.Cells.Length];

			HashSet<Cell> changedCells = new HashSet<Cell>();

			Recursion(possibilities, 0);

			return changedCells;
		}

		public void Recursion(int[] gatheredValues, int progress)
		{
			Cage Target = this.Target as Cage;

			if (progress == Target.Cells.Length) //We've arrived at the end
			{
				int counter = 0;

				foreach (int j in gatheredValues)
				{
					counter += j;
				}

				Console.WriteLine("Counter/Goal: " + counter + "/" + Target.Goal);

				if (counter == Target.Goal)
				{
					foreach (int i in gatheredValues)
					{
						Console.Write(i + "");
						//SUCCESS
						//ALL VALUES IN GATHERED VALUES ARE PART OF THE NEW LIST OF POSSIBLE VALUES
					}

					Console.WriteLine();
				}
			}
			else //There's more steps to go		
			{
				int[] possibilitiesCopy = new int[Target.Cells.Length];

				foreach (int i in Target.Cells[progress].PossibleValues)
				{
					Array.Copy(gatheredValues, possibilitiesCopy, Target.Cells.Length);
					possibilitiesCopy[progress] = i;
					Recursion(possibilitiesCopy, progress + 1);
				}
			}
		}
    }
}