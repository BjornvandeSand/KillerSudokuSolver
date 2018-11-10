using System;
using System.Linq;

namespace KillerSudokuSolver
{
	class KillerSudoku
	{
		readonly bool killerX; //Whether this is a KillerX Sudoku
		readonly int dimension; //Dimension of the puzzle
		readonly public int maxCellValue; //Maximum Value of any Cell
		readonly public int numberOfCells;//Total number of Cells

		readonly public Cell[,] grid;
		readonly public Row[] rows;
		readonly public Column[] columns;
		readonly Diagonal[] diagonals;
		readonly Block[,] blocks;
		readonly public Cage[] cages;
		readonly House[] houses;

		//The constructor not only builds the KillerSudoko itself, but initializes and interconnects all its components
		public KillerSudoku(Cell[,] grid, int dimension, int maxCellValue, Cage[] cages, bool killerX, int numberOfCells, int extremeSum)
		{
			this.dimension = dimension;
			this.maxCellValue = maxCellValue;
			this.grid = grid;
			this.numberOfCells = numberOfCells;
			rows = new Row[maxCellValue];
			columns = new Column[maxCellValue];
			diagonals = new Diagonal[2];
			blocks = new Block[dimension, dimension];
			this.cages = cages;
			this.killerX = killerX;

			//All Rows, Columns and Blocks
			int housesAmount = maxCellValue * 3 + cages.Length;

			//KillerX Sudokus include the two Diagonals
			if (killerX)
			{
				housesAmount += 2;
			}

			houses = new House[housesAmount];

			int counter = 0;
			Cell[] tempRow;
			Cell[] tempColumn;

			Cell[] tempDiagonal1 = new Cell[maxCellValue];
			Cell[] tempDiagonal2 = new Cell[maxCellValue];

			foreach (Cage cage in cages)
			{
				houses[counter] = cage;
				counter++;
			}

			for (int y = 0; y < maxCellValue; y++)
			{
				tempRow = new Cell[maxCellValue];

				for (int x = 0; x < maxCellValue; x++)
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

				rows[y] = new Row(y, tempRow, dimension);

				foreach (Cell cell in tempRow)
				{
					cell.Row = rows[y];
				}

				houses[counter] = rows[y];
				counter++;
			}

			//Build the Column objects
			for (int x = 0; x < maxCellValue; x++)
			{
				tempColumn = new Cell[maxCellValue];

				for (int y = 0; y < maxCellValue; y++)
				{
					tempColumn[y] = grid[x, y];
				}

				columns[x] = new Column(x, tempColumn);

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
				Diagonal tempDiagonal = new Diagonal(0, tempDiagonal1);

				foreach (Cell cell in tempDiagonal.Cells)
				{
					cell.Diagonal = tempDiagonal;
				}

				diagonals[0] = tempDiagonal;
				houses[counter] = tempDiagonal;
				counter++;

				tempDiagonal = new Diagonal(1, tempDiagonal2);

				foreach (Cell cell in tempDiagonal.Cells)
				{
					cell.Diagonal = tempDiagonal;
				}

				diagonals[1] = tempDiagonal;
				houses[counter] = tempDiagonal;
				counter++;
			}

			Cell[] tempCells = new Cell[maxCellValue];
			Block tempBlock;
			int cellCounter = 0;
			int blockCounter = 0;

			//Walk through all Cells in the Grid Block by Block and create these Block objects
			for (int xFactor = 0; xFactor < maxCellValue; xFactor += dimension)
			{
				for (int yFactor = 0; yFactor < maxCellValue; yFactor += dimension)
				{
					for (int x = 0 + xFactor; x < dimension + xFactor; x++)
					{
						for (int y = 0 + yFactor; y < dimension + yFactor; y++)
						{
							tempCells[cellCounter] = grid[x, y];
							cellCounter++;
						}
					}

					tempBlock = new Block(blockCounter, tempCells);
					blockCounter++;
					blocks[xFactor / dimension, yFactor / dimension] = tempBlock;
					cellCounter = 0;

					foreach (Cell cell in tempCells)
					{
						cell.Block = tempBlock;
					}

					houses[counter] = tempBlock;
					counter++;
				}
			}

			//Collect all of each Cell's Houses in their House list
			foreach (Cell cell in grid)
			{
				cell.Houses.Add(cell.Cage);
				cell.Houses.Add(cell.Row);
				cell.Houses.Add(cell.Column);
				cell.Houses.Add(cell.Block);

				if (cell.Diagonal != null)
				{
					cell.Houses.Add(cell.Diagonal);
				}
			}
		}

		//Does a number of basic checks to see if there are likely errors in the parsed puzzle
		public void Verify()
		{
			//Checks if any position in the Grid has  an uninitialized Cell
			foreach (Cell cell in grid)
			{
				if (cell == null)
				{
					Console.WriteLine("Unitialized Cell in parsed puzzle");
				}
			}

			int sum = 0;

			//Generates the sum of all Cages
			foreach (Cage cage in cages)
			{
				sum += cage.Goal;
			}

			//Checks if it equals what the sum of all possible Goals should be
			if (sum != numberOfCells * (maxCellValue + 1) / 2)
			{
				Console.WriteLine("Sum of Cages: " + sum + " doesn't add up to required sum of Grid");
			}

			//Checks if any Houses are too large
			foreach (House house in houses)
			{
				if (house.Cells.Length > maxCellValue)
				{
					Console.WriteLine("There is a House too large for Dimension");
				}
			}
		}

		public bool Solved()
		{
			bool output = true;

			foreach(Cell cell in grid)
			{
				if (cell.Value == 0)
				{
					return false;
				}
			}

			return output;
		}

		//Creates a String representation of the puzzle for easy printing
		public override string ToString()
		{
			int whiteSpaceLength = (maxCellValue - 1).ToString().Length;
			string whiteSpace = string.Concat(Enumerable.Repeat(" ", whiteSpaceLength));
			string lineSpace = string.Concat(Enumerable.Repeat("-", whiteSpaceLength)) + "-";
			string output = whiteSpace + "  ";

			string numberLine = ""; //The lines displaying the Grid coordinates

			//Builds the lines displaying the Grid coordinates
			for (int i = 1; i <= maxCellValue; i++)
			{
				numberLine += " " + i;

				if (i % dimension == 0)
				{
					numberLine += "  ";
				}
			}

			output += numberLine + Environment.NewLine + whiteSpace + " /-";

			string lineLine = ""; //The lines above and below the Grid
			string intermediateLineLine = ""; //The lines between Blocks

			int stopper = maxCellValue - 1; //Makes sure the lines between Blocks don't run on too far

			//Builds the lines above, below and between Blocks
			for (int i = 0; i < maxCellValue; i++)
			{
				lineLine += lineSpace;

				if ((i + 1) % dimension == 0 && i < stopper)
				{
					intermediateLineLine += lineSpace + "+-";
				}
				else
				{
					intermediateLineLine += lineSpace;
				}
			}

			lineLine += string.Concat(Enumerable.Repeat("-", (dimension - 1) * 2));

			output += lineLine + "\\" + Environment.NewLine;

			//Builds the lines
			for (int y = maxCellValue - 1; y >= 0; y--)
			{
				output += rows[y];

				if (y != 0 && y % dimension == 0)
				{
					output += whiteSpace + " |-" + intermediateLineLine + "|" + Environment.NewLine;
				}
			}

			output += whiteSpace + " \\-" + lineLine + "/" + Environment.NewLine + whiteSpace + "  " + numberLine;

			return output;
		}
	}
}