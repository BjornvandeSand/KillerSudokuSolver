using System;
using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
	class RemoveImpossibles : Rule
    {
		SortedSet<int>[] AllHousePossibleValues;

		//Removes Values that are could not possibly be used to form the Goal by running all viable permutations and creating a new set of PossibleValues.
		public RemoveImpossibles(House target, float priority)
		{
			Target = target;
			Priority = priority;
			AllHousePossibleValues = new SortedSet<int>[Target.Cells.Length];
		}

		public override HashSet<Cell> Execute()
		{
			Successor = Target.GenerateSuccessor();

			AllHousePossibleValues = new SortedSet<int>[Successor.Cells.Length];

			for (int i = 0; i < Successor.Cells.Length; i++)
			{
				AllHousePossibleValues[i] = new SortedSet<int>();
			}

			int[] possibilities = new int[Successor.Cells.Length];

			HashSet<Cell> changedCells = new HashSet<Cell>();

			Recursion(possibilities, 0);

			for (int i = 0; i < Successor.Cells.Length; i++)
			{
				if (Successor.Cells[i].SwapPossibleValues(AllHousePossibleValues[i]))
				{
					changedCells.Add(Successor.Cells[i]);
				}
			}

			return changedCells;
		}

		public void Recursion(int[] gatheredValues, int progress)
		{
			if (progress == Successor.Cells.Length) //We've arrived at the end
			{
				if (Successor is Cage)
				{
					int counter = 0;

					foreach (int j in gatheredValues)
					{
						counter += j;
					}

					Cage CastTarget = Successor as Cage;

					if (counter != CastTarget.Goal)
					{
						return;
					}
				}

				for (int i = 0; i < gatheredValues.Length; i++)
				{
					AllHousePossibleValues[i].Add(gatheredValues[i]);
				}
			}
			else //There's more steps to go		
			{
				int[] possibilitiesCopy;

				//Start new branches with a copy of the progress for each Possible Value that wasn't included yet
				foreach (int i in Successor.Cells[progress].PossibleValues)
				{
					if (!gatheredValues.Contains(i)) //Check if the new Possible Value isn't already used in the progress so far
					{
						possibilitiesCopy = new int[Successor.Cells.Length];
						Array.Copy(gatheredValues, possibilitiesCopy, Successor.Cells.Length); //Copy the progress so far
						possibilitiesCopy[progress] = i; //Add the PossibleValue for the latest Cell
						Recursion(possibilitiesCopy, progress + 1); //Run the branch
					}
				}
			}
		}
	}
}