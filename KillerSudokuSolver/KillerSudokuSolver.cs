using System;
using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
	partial class KillerSudokuSolver
	{
		static void Main(string[] args)
		{
			Console.WriteLine("*Killer Sudoku solver*");

			KillerSudoku puzzle = Parser("warmup01.txt");
			Console.WriteLine("Puzzle loaded");

			puzzle.Verify();
			Console.WriteLine("Puzzle verified");

			puzzle.Solve();
		    Console.WriteLine("Puzzle solved");

			Console.WriteLine(puzzle);

			//TESTLINES
			foreach (int i in puzzle.grid[0, 0].PossibleValues)
			{
				Console.Write(i + " ");
			}
			Console.WriteLine();
			//END TESTLINES

			Console.Read();
		}

		class KillerSudoku {
			readonly bool killerX; //Whether this is a KillerX Sudoku
			readonly int dimension;
			readonly int maxValue; //Maximum value of any cell

            readonly public Cell[,] grid;
			readonly Row[] rows;
            readonly Column[] columns;
            readonly Diagonal[] diagonals;
            readonly Block[,] blocks;
            readonly House[] houses;
            readonly Cage[] cages;

			//The constructor not only builds the KillerSudoko itself, but initializes and interconnects all its components
			public KillerSudoku(Cell[,] grid, int dimension, int maxValue, Cage[] cages, bool killerX, int extremeSum)
			{
				this.dimension = dimension;
				this.maxValue = maxValue;
				this.grid = grid;
				rows = new Row[maxValue];
				columns = new Column[maxValue];
				diagonals = new Diagonal[2];
				blocks = new Block[dimension, dimension];
				this.cages = cages;
				this.killerX = killerX;

				//All rows, columns and Blocks
				int housesAmount = maxValue * 3 + cages.Length;

				//KillerX Sudokus include the two diagonals
				if (killerX)
				{
					housesAmount += 2;
				}

				houses = new House[housesAmount];

				int counter = 0;
				Cell[] tempRow;
				Cell[] tempColumn;

				Cell[] tempDiagonal1 = new Cell[maxValue];
				Cell[] tempDiagonal2 = new Cell[maxValue];

                foreach (Cage cage in cages) {
                    houses[counter] = cage;
                    counter++;
                }

                for (int y = 0; y < maxValue; y++)
				{
					tempRow = new Cell[maxValue];

					for (int x = 0; x < maxValue; x++)
					{
						tempRow[x] = grid[x, y];

                        //Check if this Cell is part of a Diagonal if this is a killerX Sudoku
						if (killerX)
						{
							//If this coordinate is in the bottom left to top right Diagonal
							if (x == y)
							{
								tempDiagonal1[x] = grid[x, y];
							}

							//If this coordinate is in the top left to bottom right Diagonal
							if (x + y == extremeSum)
							{
								tempDiagonal2[x] = grid[x, y];
							}
						}
					}
                    
					rows[y] = new Row(y, tempRow, maxValue);

                    foreach(Cell cell in tempRow)
                    {
                        cell.Row = rows[y];
                    }

					houses[counter] = rows[y];
					counter++;
				}

                //Build the Column objects
				for (int x = 0; x < maxValue; x++)
				{
					tempColumn = new Cell[maxValue];

					for (int y = 0; y < maxValue; y++)
					{
						tempColumn[y] = grid[x, y];
					}

					columns[x] = new Column(x, tempColumn, maxValue);

                    foreach (Cell cell in tempColumn)
                    {
                        cell.Column = columns[x];
                    }

                    houses[counter] = columns[x];
					counter++;
				}

                //Adds the Diagonals if this is a KillerX Sudoku
				if (killerX)
				{
					Diagonal tempDiagonal = new Diagonal(0, tempDiagonal1, maxValue);

                    foreach (Cell cell in tempDiagonal.Cells)
                    {
                        cell.Diagonal = tempDiagonal;
                    }

                    diagonals[0] = tempDiagonal;
					houses[counter] = tempDiagonal;
					counter++;

					tempDiagonal = new Diagonal(1, tempDiagonal2, maxValue);

                    foreach (Cell cell in tempDiagonal.Cells)
                    {
                        cell.Diagonal = tempDiagonal;
                    }

                    diagonals[1] = tempDiagonal;
					houses[counter] = tempDiagonal;
					counter++;
				}

				Cell[] tempCells = new Cell[maxValue];
                Block tempBlock;
                int cellCounter = 0;

                //Walk through all Cells in the Grid Block by Block and create these Block objects
				for (int xFactor = 0; xFactor < maxValue; xFactor += dimension)
				{
					for (int yFactor = 0; yFactor < maxValue; yFactor += dimension)
					{
						for (int x = 0 + xFactor; x < dimension + xFactor; x++)
						{
							for (int y = 0 + yFactor; y < dimension + yFactor; y++)
							{
								tempCells[cellCounter] = grid[x, y];
								cellCounter++;
							}
						}

                        tempBlock = new Block(tempCells, maxValue);
                        blocks[xFactor / dimension, yFactor / dimension] = tempBlock;
                        cellCounter = 0;

                        foreach(Cell cell in tempCells)
                        {
                            cell.Block = tempBlock;
                        }

                        houses[counter] = tempBlock;
                        counter++;
                    }
				}

				//Collect all of each Cell's Houses in their House list
				foreach(Cell cell in grid)
				{
					cell.Houses.Add(cell.Row);
					cell.Houses.Add(cell.Column);
					cell.Houses.Add(cell.Cage);
					cell.Houses.Add(cell.Block);

					if(cell.Diagonal != null)
					{
						cell.Houses.Add(cell.Diagonal);
					}
				}
			}

            //Does a number of basic checks to see if there are likely errors in the parsed puzzle
			public void Verify()
			{
                //Checks if any position in the Grid doesn't have an initialized Cell
				foreach (Cell cell in grid)
				{
					if (cell == null)
					{
						Console.WriteLine("Unitialized Cell in parsed puzzle");
					}
				}

				int sum = 0;

                //Generates the sum of all cages
				foreach (Cage cage in cages)
				{
					sum += cage.Goal;
				}

                //and checks if they equal what the sum of all PossibleValues should be
                if (sum != maxValue * maxValue * (maxValue + 1) / 2)
				{
					Console.WriteLine("Sum of Cages doesn't add up to required sum of Grid");
				}

                //Checks if any Houses are too large
                foreach (House house in houses)
                {
                    if(house.Cells.Length > maxValue)
                    {
                        Console.WriteLine("There is a House too large for Dimension");
                    }
                }
            }

            //Solves this puzzle through a Priority Queue and several possible steps
			public void Solve() {
                Queue<Rule> priorityQueue = new Queue<Rule>();

                foreach (Cage cage in cages)
                {
                    priorityQueue.Enqueue(new RemoveHighLow(cage,0));
                }

                while(priorityQueue.Count != 0)
                {
                    List<Cell> improvedCells = priorityQueue.Dequeue().Execute();

					foreach (Cell cell in improvedCells)
					{
						priorityQueue.Enqueue(new RemoveDuplicatePossibilities(cell, 0));

						foreach (House house in cell.Houses)
						{
							Console.WriteLine(house.GetType());
							priorityQueue.Enqueue(new OnlyPossibilityLeftInHouse(house, 0));
						}
					}
				}
			}

            //Creates a String representation of the puzzle for easy printing
			public override string ToString()
			{
				int whiteSpaceLength = (maxValue - 1).ToString().Length;
				string whiteSpace = String.Concat(Enumerable.Repeat(" ", whiteSpaceLength));
				string lineSpace = String.Concat(Enumerable.Repeat("-", whiteSpaceLength)) + "-";
				string output = whiteSpace + "  ";

				for (int i = 1; i <= maxValue; i++)
				{
					output += " " + i;
				}

				output += Environment.NewLine + whiteSpace + " /-";

				for (int i = 0; i < maxValue; i++)
				{
					output += lineSpace;
				}

				output += "\\" + Environment.NewLine;

				for (int y = maxValue - 1; y >= 0; y--)
				{
					output += rows[y];
				}

				output += whiteSpace + " \\-";

				for (int i = 0; i < maxValue; i++)
				{
					output += lineSpace;
				}

				output += "/" + Environment.NewLine + whiteSpace + "  ";

				for (int i = 1; i <= maxValue; i++)
				{
					output += " " + i;
				}

				return output;
			}
		}
	}
}