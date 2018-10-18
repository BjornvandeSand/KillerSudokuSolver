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
			Console.WriteLine("Puzzle verified");

			puzzle.Solve();

			Console.WriteLine(Environment.NewLine + "Name: " + Path.GetFileNameWithoutExtension(path) + Environment.NewLine + Environment.NewLine + puzzle + Environment.NewLine);

			if (puzzle.Solved())
			{
				Console.WriteLine("Puzzle solved");
			}
			else
			{
				Console.WriteLine("Puzzle unsolved, but no more improving rules found.");
			}

			//TESTLINES
			Console.WriteLine("Possible values left:");

			foreach (Cell cell in puzzle.grid)
			{
				Console.Write("Cell[" + cell.Column.Id + "," + cell.Row.Id + "] ");

				foreach (int i in cell.PossibleValues)
				{
					Console.Write(i + " ");
				}

				Console.WriteLine();
			}
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
					int housesPerCell = 4; //The amount of Houses each Cell is a part of. This is static, except for Diagonals
					int housesPerXCell = 5; //The amount of Houses for a Cell that is part of a Diagonal in a KillerX
					int totalCellCounter = 0; //The total amount of Cells parsed at any point

					int tempX; //The X coordinate for the current Cell
					int tempY; //The Y coordinate for the current Cell
					int counter; //Keeps track of where on the parsed line we are
					int cellsInLine; //The amount of Cells on the current line
					Cell tempCell = null; //The Cell currently being evaluated
					Cell[] cageCells; //The list of Cells prepared for a Cage

					string line = sr.ReadLine();
					string[] splitLine = line.Split(' ');

					int dimension = Int32.Parse(splitLine[0]);
					int maxValue = dimension * dimension;
					int extremeSum = maxValue + 1; //The value of the lowest and highest possible values combined
					Cell[,] grid = new Cell[maxValue, maxValue];

					int cagesAmount = Int32.Parse(splitLine[1]);
					Cage[] cages = new Cage[cagesAmount];

					bool killerX = Boolean.Parse(splitLine[2]);

					//Make all the Cells and Cages
					for (int i = 0; i < cagesAmount; i++)
					{
						splitLine = sr.ReadLine().Split(' ');
						cellsInLine = splitLine.Length - 2;
						totalCellCounter += cellsInLine;
						cageCells = new Cell[cellsInLine / 2];

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

					totalCellCounter /= 2;

					if (totalCellCounter != dimension * dimension * dimension * dimension)
					{
						Console.WriteLine("The amount of Cells in the parsed file doesn't match the required amount: " + totalCellCounter);
					}

					//Build the Killer Sudoku based on the parsed input
					output = new KillerSudoku(grid, dimension, maxValue, cages, killerX, extremeSum);
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