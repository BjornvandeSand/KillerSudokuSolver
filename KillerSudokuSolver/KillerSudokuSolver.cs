using System;
using System.IO;
using System.Linq;

namespace KillerSudokuSolver
{
	partial class KillerSudokuSolver
	{
		static void Main(string[] args)
		{
			Console.WriteLine("*Killer Sudoku solver*");

			KillerSudoku puzzle = parser("2.txt");
			Console.WriteLine("Puzzle loaded");

			puzzle.Verify();
			Console.WriteLine("Puzzle verified");

			puzzle.solve();
		    Console.WriteLine("Puzzle solved");

			Console.WriteLine(puzzle);

            Console.Read();
		}

        //Parses all the required info to build a Killer Sudoku and hands it to the constructor
         static KillerSudoku parser(string file) {
			KillerSudoku output = null;

			try
			{
				using (StreamReader sr = new StreamReader(file))
				{
					string line = sr.ReadLine();
					string[] splitLine = line.Split(' ');

					int dimension = Int32.Parse(splitLine[0]);
					int maxValue = dimension * dimension;
					Cell[,] grid = new Cell[maxValue, maxValue];

					int cagesAmount = Int32.Parse(splitLine[1]);
					Cage[] cages = new Cage[cagesAmount];

					bool killerX = Boolean.Parse(splitLine[2]);

					int totalCellCounter = 0;

					int counter;
					int cellsInLine;
					Cell tempCell;
					Cell[] cageCells;

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
							tempCell = new Cell(maxValue);
							cageCells[counter] = tempCell;

							grid[int.Parse(splitLine[counter * 2]) - 1, int.Parse(splitLine[counter * 2 + 1]) - 1] = tempCell;
							counter++;
						}

						//Handles the Cage parameters on the line
						cages[i] = new Cage(i, cageCells, int.Parse(splitLine[counter * 2]), splitLine[counter * 2 + 1][0], maxValue);

                        foreach(Cell cell in cageCells)
                        {
                            cell.Cage = cages[i];
                        }
					}

					totalCellCounter /= 2;

					if (totalCellCounter != dimension * dimension * dimension * dimension)
					{
						Console.WriteLine("The amount of cells in the parsed file doesn't match the required amount." + totalCellCounter);
					}

					//Build the Killer Sudoku based on the parsed input
					output = new KillerSudoku(grid, dimension, maxValue, cages, killerX);
				}
			} catch(Exception e) {
				Console.WriteLine("There was a problem in the attempt to parse the KillerSudoku file:" + Environment.NewLine + e.Message);
			}

			return output;
		}

		class KillerSudoku {
			bool killerX; //Whether this is a KillerX Sudoku
			int maxValue; //Maximum value of any cell
            int houseSum; //Sum of the numbers in all the Houses but the Cages

			public Cell[,] grid;
			Row[] rows;
			Column[] columns;
			Diagonal[] diagonals;
			Nonet[,] nonets;
			House[] houses;

			int dimension;
			Cage[] cages;

			//The constructor not only builds the KillerSudoko itself, but initializes and interconnects all its components
			public KillerSudoku(Cell[,] g, int n, int nn, Cage[] c, bool b)
			{
				dimension = n;
				maxValue = nn;
                houseSum = maxValue * (maxValue + 1) / 2;
				grid = g;
				rows = new Row[maxValue];
				columns = new Column[maxValue];
				diagonals = new Diagonal[2];
				nonets = new Nonet[n, n];
				cages = c;
				killerX = b;

				//All rows, columns and nonets
				int housesAmount = maxValue * 3 + cages.Length;

				//KillerX Sudokus include the two diagonals
				if (killerX)
				{
					housesAmount += 2;
				}

				houses = new House[housesAmount];

				int counter = 0;
				int tempSum = maxValue + 1;
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
							if (x + y == tempSum)
							{
								tempDiagonal2[x] = grid[x, y];
							}
						}
					}
                    
					rows[y] = new Row(y, tempRow, houseSum, maxValue);

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

					columns[x] = new Column(x, tempColumn, houseSum, maxValue);

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
					Diagonal tempDiagonal = new Diagonal(0, tempDiagonal1, houseSum, maxValue);

                    foreach (Cell cell in tempDiagonal.Cells)
                    {
                        cell.Diagional = tempDiagonal;
                    }

                    diagonals[0] = tempDiagonal;
					houses[counter] = tempDiagonal;
					counter++;

					tempDiagonal = new Diagonal(1, tempDiagonal2, houseSum, maxValue);

                    foreach (Cell cell in tempDiagonal.Cells)
                    {
                        cell.Diagional = tempDiagonal;
                    }

                    diagonals[1] = tempDiagonal;
					houses[counter] = tempDiagonal;
					counter++;
				}

				Cell[] tempCells = new Cell[maxValue];
                Nonet tempNonet;
                int cellCounter = 0;

                //Walk through all Cells in the Grid Nonet by Nonet and create these Nonet objects
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

                        tempNonet = new Nonet(tempCells, houseSum, maxValue);
                        nonets[xFactor / dimension, yFactor / dimension] = tempNonet;
                        cellCounter = 0;

                        foreach(Cell cell in tempCells)
                        {
                            cell.Nonet = tempNonet;
                        }

                        houses[counter] = tempNonet;
                        counter++;
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
						Console.WriteLine("Unitialized cell in parsed puzzle");
					}
				}

				int sum = 0;

                //Generates the sum of all cages
				foreach (Cage cage in cages)
				{
					sum += cage.Goal;
				}

                //and checks if they equal what the sum of all PossibleValues should be
				if (sum != maxValue * houseSum)
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

			public void solve() {
                foreach (Cage cage in cages)
                {
                    removeHighLow(cage);
                }
			}

            public void removeHighLow(Cage cage)
            {
                int max; //The largest possible value for this Cage
                int min; //The smallest possible value for this Cage
            
                max = cage.Goal - cage.Cells.Length * (cage.Cells.Length - 1) / 2; //Determine the maximum Value allowed in this Cage
                min = cage.Goal - ((cage.Cells.Length - 1) * (maxValue - (cage.Cells.Length - 1) + 1 + maxValue) / 2); //Determine the minimum Value allowed in this Cage

                foreach (Cell cell in cage.Cells)
                {
                    //This part removes possible Values that are are too high
                    if (max < maxValue) //Determines if there's any possible Values low enough to cull
                    {
                        for (int i = max + 1; i <= maxValue; i++)
                        {
                            cell.removeOption(i);
                        }
                    }

                    //This part removes possible Values that are too low
                    if (min > 0) //Determines if there's any possible Values high enough to cull
                    {
                        for (int i = min - 1; i > 0; i--)
                        {
                            cell.removeOption(i);
                        }
                    }
                }
            }

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