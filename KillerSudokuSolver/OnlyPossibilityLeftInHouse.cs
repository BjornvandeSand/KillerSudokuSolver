using System.Collections.Generic;

namespace KillerSudokuSolver
{
    partial class KillerSudokuSolver
    {
        class OnlyPossibilityLeftInHouse : Rule
        {
            public OnlyPossibilityLeftInHouse(House target, float priority)
            {
                Target = target;
                Priority = priority;
            }

			//Checks if a possible value is found on only 1 Cell in a House
            public override List<Cell> Execute() {
                List<Cell> output = new List<Cell>();

				int valueCounter;
				Cell cellTracker;

				for (int i = 1; i <= 9; i++)
				{
					valueCounter = 0;
					cellTracker = null;

					foreach (Cell cell in Target.Cells) {
						if (cell.PossibleValues.Contains(i))
						{
							valueCounter++;
							cellTracker = cell;
						}

						if (valueCounter == 1)
						{
							for (int j = 1; j <= Target.Cells.Length; j++)
							{
								if (i != j)
								{
									cell.RemovePossibleValueIfPresent(i);
								}
							}

							output.Add(cell);
						}
					}
				}

				return output; //Return the list of upcoming Cells to evaluate
            }
        }
    }
}