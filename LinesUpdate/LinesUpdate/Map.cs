using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Runtime.CompilerServices;
using static LinesUpdate.Form1;
namespace LinesUpdate
{
	internal class Map
	{
		public int[,]		values = new int[9, 9];
		public const int	size = 9;
		
		public Map()
		{
			for (int i = 0; i < 9; ++i)
				for (int j = 0; j < 9; ++j)
					values[i, j] = 0;
		}

		public static void printValues(int[,] values)
		{
			Console.WriteLine("---------------------------------");
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (values[i, j] > 9 || values[i, j] < 0)
						Console.Write(values[i, j] + " ");
					else
						Console.Write(" " + values[i, j] + " ");
				}
				Console.WriteLine();
			}
			Console.WriteLine("---------------------------------");
		}

		public bool mapIsFilled()
		{
			for (int i = 0; i < size; ++i)
				for (int j = 0; j < size; ++j)
					if (values[i, j] == 0)
						return (false);
			return (true);
		}

		public void	updateMap()
		{
			Console.WriteLine("IN FUNCTION RESTARTMAP()");
			for (int i = 0; i < size; ++i)
				for (int j = 0; j < size; ++j)
					if (values[i, j] >= 10)
						values[i, j] = 0;
		}

		public void clearMap(ref Form1.RoundButton[,] buttons)
		{
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					values[i, j] = 0;
					buttons[i, j].BackColor = Color.Gray;
				}
			}
		}


		private int rowsCheck(ref Form1.RoundButton[,] buttons, int row, int col, int color)
		{
			int tmpRow = row;
			int rowCount = 1;
			int retValue = 0;

			while (--tmpRow >= 0 && values[tmpRow, col] == color)
				rowCount++;
			tmpRow = row;
			while (++tmpRow < 9 && values[tmpRow, col] == color)
				rowCount++;
			if (rowCount >= 5)
			{
				retValue = rowCount - 1;
				while (rowCount != 0)
				{
					--tmpRow;
					if (tmpRow != row)
					{
						values[tmpRow, col] = 0;
						buttons[tmpRow, col].BackColor = Color.Gray;
					}
					rowCount--;
				}
			}
			return (retValue);
		}

		private int columnsCheck(ref Form1.RoundButton[,] buttons, int row, int col, int color)
		{
			int tmpCol = col;
			int colCount = 1;
			int retValue = 0;

			while (--tmpCol >= 0 && values[row, tmpCol] == color)
				colCount++;
			tmpCol = col;
			while (++tmpCol < 9 && values[row, tmpCol] == color)
				colCount++;
			if (colCount >= 5)
			{
				retValue = colCount - 1;
				while (colCount != 0)
				{
					--tmpCol;
					if (tmpCol != col)
					{
						values[row, tmpCol] = 0;
						buttons[row, tmpCol].BackColor = Color.Gray;
					}
					colCount--;
				}
			}
			return (retValue);
		}

		private int diagonalCheck(ref Form1.RoundButton[,] buttons, int row, int col, int color)
		{
			int tmpRow = row - 1, tmpCol = col - 1;
			int diagCount = 1;
			int retValue = 0;

			while (tmpRow >= 0 && tmpCol >= 0 && values[tmpRow, tmpCol] == color)
			{
				diagCount++;
				tmpRow--;
				tmpCol--;
			}
			tmpRow = row + 1; tmpCol = col + 1;
			while (tmpRow < 9 && tmpCol < 9 && values[tmpRow, tmpCol] == color)
			{
				diagCount++;
				tmpRow++;
				tmpCol++;
			}
			if (diagCount >= 5)
			{
				retValue = retValue + diagCount - 1;
				while (diagCount != 0)
				{
					--tmpRow; --tmpCol;
					if (tmpRow != row && tmpCol != col)
					{
						values[tmpRow, tmpCol] = 0;
						buttons[tmpRow, tmpCol].BackColor = Color.Gray;
					}
					diagCount--;
				}
			}

			tmpRow = row + 1; tmpCol = col - 1;
			diagCount = 1;

			while (tmpRow < 9 && tmpCol >= 0 && values[tmpRow, tmpCol] == color)
			{
				diagCount++;
				tmpRow++;
				tmpCol--;
			}
			tmpRow = row - 1; tmpCol = col + 1;
			while (tmpRow >= 0 && tmpCol < 9 && values[tmpRow, tmpCol] == color)
			{
				diagCount++;
				tmpRow--;
				tmpCol++;
			}
			if (diagCount >= 5)
			{
				retValue = retValue + diagCount - 1;
				while (diagCount != 0)
				{
					++tmpRow; --tmpCol;
					if (tmpRow != row && tmpCol != col)
					{
						values[tmpRow, tmpCol] = 0;
						buttons[tmpRow, tmpCol].BackColor = Color.Gray;
					}
					diagCount--;
				}
			}
			return (retValue);
		}

		public int colorsInLineCheck(ref Form1.RoundButton[,] buttons, int row, int col)
		{
			int explodedCount;

			explodedCount = rowsCheck(ref buttons, row, col, values[row, col]) +
				columnsCheck(ref buttons, row, col, values[row, col]) +
				diagonalCheck(ref buttons, row, col, values[row, col]);
			if (explodedCount > 0)
			{
				values[row, col] = 0;
				buttons[row, col].BackColor = Color.Gray;
				explodedCount++;
			}
			return (explodedCount);
		}

		public void findAllValidPaths(int row, int col)
		{
			Queue<Form1.MyTuple> queue = new Queue<Form1.MyTuple>();
			int start = 10;
			Form1.MyTuple tmp;

			queue.Enqueue(new Form1.MyTuple(row, col));
			values[row, col] = start;

			while (queue.Count > 0)
			{
				tmp = queue.Dequeue();
				row = tmp.row;
				col = tmp.col;

				if (row < 8 && values[row + 1, col] == 0)
				{
					values[row + 1, col] = values[row, col] + 1;
					queue.Enqueue(new Form1.MyTuple(row + 1, col));
				}
				if (col < 8 && values[row, col + 1] == 0)
				{
					values[row, col + 1] = values[row, col] + 1;
					queue.Enqueue(new Form1.MyTuple(row, col + 1));
				}
				if (row > 0 && values[row - 1, col] == 0)
				{
					values[row - 1, col] = values[row, col] + 1;
					queue.Enqueue(new Form1.MyTuple(row - 1, col));
				}
				if (col > 0 && values[row, col - 1] == 0)
				{
					values[row, col - 1] = values[row, col] + 1;
					queue.Enqueue(new Form1.MyTuple(row, col - 1));
				}
			}
			Console.WriteLine("Posle cikla");
			Map.printValues(this.values);
		}

		public Stack<MyTuple> findShortestWay(ref RoundButton[,] buttons, Colors color,int dstRow, int dstCol)
		{
			Console.WriteLine("-----------In function findshortway----------");

			Stack<MyTuple> way = new Stack<MyTuple>();
			while (true)
			{
				Console.WriteLine("row: " + dstRow + " col: " + dstCol);
				if (dstRow + 1 < 9 && values[dstRow + 1, dstCol] == values[dstRow, dstCol] - 1)
					way.Push(new MyTuple(dstRow++, dstCol));
				else if (dstRow - 1 >= 0 && values[dstRow - 1, dstCol] == values[dstRow, dstCol] - 1)
					way.Push(new MyTuple(dstRow--, dstCol));
				else if (dstCol + 1 < 9 && values[dstRow, dstCol + 1] == values[dstRow, dstCol] - 1)
					way.Push(new MyTuple(dstRow, dstCol++));
				else if (dstCol - 1 >= 0 && values[dstRow, dstCol - 1] == values[dstRow, dstCol] - 1)
					way.Push(new MyTuple(dstRow, dstCol--));
				else
					break;
			}
			Map.printValues(this.values);
			foreach (MyTuple tmp in way)
			{
				//Console.WriteLine("row: " + tmp.row + " col: " + tmp.col);
				int i;
				for (i = 0; i < 5; ++i)
					if (color.arr[i] == buttons[dstRow, dstCol].BackColor)
						break ;
				buttons[tmp.row, tmp.col].BackColor = color.way[i];
			}
			//Console.WriteLine("++++++++++++++++FIND AND BREAK++++++++++++++++");
			//printValues();
			return (way);
		}

	}
}
