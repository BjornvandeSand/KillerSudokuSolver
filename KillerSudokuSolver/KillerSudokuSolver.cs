using System;
using System.Collections.Generic;
using System.IO;

namespace KillerSudokuSolver
{
	class KillerSudokuSolver
	{
		static void Main(string[] args)
		{
			while (true)
			{
				string path = "Puzzles/";

				Console.WriteLine("*Killer Sudoku solver*");
				Console.WriteLine("Which category of puzzle would you like to solve?");
				Console.WriteLine("Press the key associated with a difficulty to continue or any other key to quit:");
				Console.WriteLine("(w)armup, (e)asy, (m)edium, (t)ricky or (d)ifficult? Alternatively (c)alcudoku.org");

				ConsoleKeyInfo input = Console.ReadKey();

				string difficulty = "";

				switch (input.KeyChar)
				{
					case 'w':
						difficulty = "Warmup";
						break;
					case 'e':
						difficulty = "Easy";
						break;
					case 'm':
						difficulty = "Medium";
						break;
					case 't':
						difficulty = "Tricky";
						break;
					case 'd':
						difficulty = "Difficult";
						break;
					case 'c':
						difficulty = "Calcudoku.org";
						break;
					default:
						Console.WriteLine("Goodbye!");
						Environment.Exit(0);
						break;
				}

				path += difficulty + '/';
				Console.WriteLine(Environment.NewLine + difficulty + " selected" + Environment.NewLine);

				Console.WriteLine("Which puzzle would you like to solve? Enter a number:");

				int input2 = int.Parse(Console.ReadLine());

				Console.WriteLine(input2 + " selected." + Environment.NewLine);

				path += input2 + ".txt";

				KillerSudoku puzzle = Parser(path);
				Console.WriteLine("Puzzle loaded");

				puzzle.Verify();
				Console.WriteLine("Puzzle verified" + Environment.NewLine);

				bool solved = Solve(puzzle);

				Console.WriteLine(Environment.NewLine + puzzle + Environment.NewLine);

				if (solved)
				{
					Console.WriteLine("Puzzle solved!");
				}
				else
				{
					Console.WriteLine("Puzzle unsolved, but no more improving rules found." + Environment.NewLine);

					int valuesLeft = 0;
					Console.WriteLine("Possible values left:");

					foreach (Cell cell in puzzle.grid)
					{
						Console.Write("Cell[" + (cell.Column.Id + 1) + "," + (cell.Row.Id + 1) + "] ");

						for (int i = 1; i <= cell.Row.Cells.Length; i++)
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
					float valuesEliminated = numberOfValues - valuesLeft;

					Console.WriteLine(Environment.NewLine + valuesEliminated + " possible values were elimated.");
					Console.WriteLine(valuesLeft + " possible values are left to eliminate.");
					Console.WriteLine(valuesEliminated / numberOfValues * 100 + "% solved.");
				}

				Console.WriteLine(Environment.NewLine + "Press any key to return the menu.");
				Reset();
				Console.ReadLine();
			}
		}

		//Parses all the required info to build a Killer Sudoku and hands it to the constructor
		static KillerSudoku Parser(string file)
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

		//Solves this puzzle through a Priority Queue and several possible steps
		static bool Solve(KillerSudoku puzzle)
		{
			PriorityQueue<Rule> rulesQueue = new PriorityQueue<Rule>();
			HashSet<Cell> improvedCells = new HashSet<Cell>();
			HashSet<Cage> improvedCages = new HashSet<Cage>();
			HashSet<House> improvedHouses = new HashSet<House>();

			foreach (Cage cage in puzzle.cages)
			{
				rulesQueue.Enqueue(new RemoveHighLow(cage, 1));
			}

			int rulesEvaluated = 0;

			while (rulesQueue.Count() != 0)
			{
				rulesEvaluated++;
				improvedCages.Clear();
				improvedHouses.Clear();
				improvedCells = rulesQueue.Dequeue().Execute();

				foreach (Cell cell in improvedCells)
				{
					rulesQueue.Enqueue(new RemoveDuplicatePossibilities(cell, 2));

					foreach (House house in cell.Houses)
					{
						if (improvedHouses.Add(house))
						{
							if (house is Cage)
							{
								if (improvedCages.Add(cell.Cage))
								{
									rulesQueue.Enqueue(new RemoveHighLow(cell.Cage, 1));
									rulesQueue.Enqueue(new NCageN(cell.Cage, 1));
								}
							}

							rulesQueue.Enqueue(new OnlyPossibilityLeftInHouse(house, 1));
							rulesQueue.Enqueue(new RemoveImpossibles(house, 0));
						}
					}
				}
			}

			int totalValuesEliminated = RemoveDuplicatePossibilities.PossibleValuesEliminated + 
				RemoveHighLow.PossibleValuesEliminated + 
				OnlyPossibilityLeftInHouse.PossibleValuesEliminated + 
				NCageN.PossibleValuesEliminated +
				RemoveImpossibles.PossibleValuesEliminated +
				Cell.PossibleValuesEliminated;

			Console.WriteLine("A total of " + rulesEvaluated + " rules were evaluated.");

			Console.WriteLine(Environment.NewLine + "The following Rules were executed this number of times:");
			Console.WriteLine(RemoveDuplicatePossibilities.Executions + " by RemoveDuplicatePossibilities Rules");
			Console.WriteLine(RemoveHighLow.Executions + " by RemoveHighLow Rules");
			Console.WriteLine(OnlyPossibilityLeftInHouse.Executions + " by OnlyPossibilityLeftInHouse Rules");
			Console.WriteLine(NCageN.Executions + " by NCageN Rules");
			Console.WriteLine(RemoveImpossibles.Executions + " by RemoveImpossibles Rules");

			Console.WriteLine(Environment.NewLine + "The following Rules were responsible for this number of Possible Value eliminations:");
			Console.WriteLine(RemoveDuplicatePossibilities.PossibleValuesEliminated + " by RemoveDuplicatePossibilities Rules");
			Console.WriteLine(RemoveHighLow.PossibleValuesEliminated + " by RemoveHighLow Rules");
			Console.WriteLine(OnlyPossibilityLeftInHouse.PossibleValuesEliminated + " by OnlyPossibilityLeftInHouse Rules");
			Console.WriteLine(NCageN.PossibleValuesEliminated + " by NCageN Rules");
			Console.WriteLine(RemoveImpossibles.PossibleValuesEliminated + " by RemoveImpossibles Rules");

			Console.WriteLine(Environment.NewLine + Cell.PossibleValuesEliminated + " by " + Cell.PossibleValuesEliminated + " automatic removal of the last Possible Value in Cell calls");

			Console.WriteLine("In total " + totalValuesEliminated + " Possible Values were eliminated.");

			return puzzle.Solved();
		}

		static void Reset()
		{
			Cell.PossibleValuesEliminated = 0;
			RemoveDuplicatePossibilities.PossibleValuesEliminated = 0;
			RemoveHighLow.PossibleValuesEliminated = 0;
			OnlyPossibilityLeftInHouse.PossibleValuesEliminated = 0;
			NCageN.PossibleValuesEliminated = 0;
			RemoveImpossibles.PossibleValuesEliminated = 0;
		}
	}
}