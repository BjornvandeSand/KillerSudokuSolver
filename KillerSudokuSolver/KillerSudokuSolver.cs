using System;
using System.IO;

namespace KillerSudokuSolver
{
	class KillerSudokuSolver
	{
		static void Main(string[] args)
		{ 
			Console.WriteLine("*Killer Sudoku solver*" + Environment.NewLine);

			string path = "Puzzles/Warmup01.txt";
			KillerSudoku puzzle = Parse(path);
			Console.WriteLine("Puzzle loaded");

			puzzle.Verify();
			Console.WriteLine("Puzzle verified" + Environment.NewLine);

			puzzle.Solve();

			Console.WriteLine(Environment.NewLine + "Name: " + Path.GetFileNameWithoutExtension(path) + Environment.NewLine + Environment.NewLine + puzzle + Environment.NewLine);

			if (puzzle.Solved())
			{
				Console.WriteLine("Puzzle solved");
			}
			else
			{
				Console.WriteLine(Environment.NewLine + "Puzzle unsolved, but no more improving rules found." + Environment.NewLine);
			}

			int valuesLeft = 0;
			Console.WriteLine("Possible values left:");

			foreach (Cell cell in puzzle.grid)
			{
				Console.Write("Cell[" + (cell.Column.Id + 1) + "," + (cell.Row.Id + 1) + "] ");

				for(int i = 1; i <= cell.Row.Cells.Length; i++ )
				{
					if (cell.PossibleValues.Contains(i))
					{
						valuesLeft++;
						Console.Write(i + " ");
					}
					else
					{
						Console.Write("  ");
					}
				}

				Console.WriteLine();
			}

			int numberOfValues = puzzle.numberOfCells * puzzle.maxCellValue;
			float valuesElimated = numberOfValues - valuesLeft;

			Console.WriteLine(Environment.NewLine + valuesElimated + " possible values were elimated.");
			Console.WriteLine(valuesLeft + " possible values are left to eliminate.");
			Console.WriteLine(valuesElimated / numberOfValues  * 100 + "% solved.");

			Console.Read();

			//TESTLINES



			//END TESTLINES

			Console.Read();
		}

		//Parses all the required info to build a Killer Sudoku and hands it to the constructor
		static KillerSudoku Parse(string file)
		{
			KillerSudoku output = null;

			try
			{
				using (StreamReader sr = new StreamReader(file))
				{
					const int housesPerCell = 4; //The amount of Houses each Cell is a part of. This is static, except for Diagonals
					const int housesPerXCell = 5; //The amount of Houses for a Cell that is part of a Diagonal in a KillerX

					int cellCounter = 0; //The total amount of Cells parsed at any point
					int tempX; //The X coordinate for the current Cell
					int tempY; //The Y coordinate for the current Cell
					int counter; //Keeps track of where on the parsed line we are
					int cellsInLine; //The amount of Cells on the current line

					Cell tempCell = null; //The Cell currently being evaluated
					Cell[] cageCells; //The list of Cells prepared for a Cage

					string line = sr.ReadLine();
					string[] splitLine = line.Split(' ');

					int dimension = int.Parse(splitLine[0]);
					int maxValue = dimension * dimension;
					int numberOfCells = maxValue * maxValue;
					int extremeSum = maxValue + 1; //The value of the lowest and highest possible Values combined

					Cell[,] grid = new Cell[maxValue, maxValue];

					int cagesAmount = int.Parse(splitLine[1]);
					Cage[] cages = new Cage[cagesAmount];

					bool killerX = bool.Parse(splitLine[2]);

					//Make all the Cells and Cages
					for (int i = 0; i < cagesAmount; i++)
					{
						splitLine = sr.ReadLine().Split(' ');
						cellsInLine = splitLine.Length - 2;
						cageCells = new Cell[cellsInLine / 2];
						cellCounter += cageCells.Length;

						counter = 0;

						//Handles the Cell info on the line
						while (counter * 2 < cellsInLine)
						{
							tempX = int.Parse(splitLine[counter * 2]) - 1;
							tempY = int.Parse(splitLine[counter * 2 + 1]) - 1;

							//Check if this Cell is part of a Diagonal if this is a killerX Sudoku
							if (killerX)
							{
								//If this Cell is in either of the Diagonals
								if (tempX == tempY || tempX + tempY == extremeSum)
								{
									tempCell = new Cell(maxValue, housesPerXCell);
								}
							}
							else
							{
								tempCell = new Cell(maxValue, housesPerCell);
							}

							cageCells[counter] = tempCell;
							grid[tempX, tempY] = tempCell;
							counter++;
						}

						//Handles the Cage parameters on the line
						cages[i] = new Cage(i, cageCells, int.Parse(splitLine[counter * 2]), splitLine[counter * 2 + 1][0]);

						foreach (Cell cell in cageCells)
						{
							cell.Cage = cages[i];
						}
					}

					if (sr.ReadLine() != null)
					{
						Console.WriteLine("The parsed file contains more than the stated " + cagesAmount + " cages.");
					}

					if (cellCounter != numberOfCells)
					{
						Console.WriteLine("The amount of Cells in the parsed file doesn't match the required amount: " + cellCounter);
					}

					//Build the Killer Sudoku based on the parsed input
					output = new KillerSudoku(grid, dimension, maxValue, cages, killerX, numberOfCells, extremeSum);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("There was a problem in the attempt to parse the KillerSudoku file:" + Environment.NewLine + e.Message);
			}

			return output;
		}
	}
}