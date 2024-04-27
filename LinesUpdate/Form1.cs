using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LinesUpdate
{
	public class RoundButton : Button
	{
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			GraphicsPath grPath = new GraphicsPath();
			grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
			this.Region = new System.Drawing.Region(grPath);
			base.OnPaint(e);
		}
	}
	public partial class Form1 : Form
	{
		private Stack<RoundButton>	pressed = new Stack<RoundButton>();
		private const int			buttonSize = 50, mapSize = 9;
		static int					pressCount = 0;
		private int					colorValue;
		private TextBox[,]			textBoxes;							
		private RoundButton[,]		buttons;
		private Color[]				colors;
		private int[,]				map;
		class myTuple
		{
			public int row;
			public int col;

			public myTuple(int row, int col) { this.row = row; this.col = col; }
		}

		public Form1()
		{
			InitializeComponent();
			colors = new Color[5];
			colors[0] = Color.FromArgb(255, 0, 0);
			colors[1] = Color.FromArgb(0, 255, 0);
			colors[2] = Color.FromArgb(0, 0, 255);
			colors[3] = Color.FromArgb(247, 0, 255);
			colors[4] = Color.FromArgb(255, 255, 0);
		}

		private void pickButtonIndexes(out int row, out int col, RoundButton button)
		{
			row = button.Name[0] - '0';
			col = button.Name[1] - '0';
		}

		public void printMap()
		{
			for (int i = 0; i < mapSize; i++)
			{
				for (int j = 0; j < mapSize; j++)
				{
					if (map[i, j] > 9 || map[i, j] < 0)
						Console.Write(this.map[i, j] + " ");
					else
						Console.Write(" " + this.map[i, j] + " ");
				}
				Console.WriteLine();
			}
			Console.WriteLine("---------------------------------");
		}

		private void deletePrevColors()
		{
			for (int i = 0; i < mapSize; i++)
			{
				for (int j = 0; j < mapSize; j++)
				{
					this.map[i, j] = 0;
					this.buttons[i, j].BackColor = Color.Gray;
				}
			}
		}

		private bool	rowsCheck(int row, int col, int color)
		{
			int		tmpRow = row;
			int		rowCount = 1;

			while (--tmpRow >= 0 && this.map[tmpRow, col] == color)
				rowCount++;
			tmpRow = row;
			while (++tmpRow < 9 && this.map[tmpRow, col] == color)
				rowCount++;
			if (rowCount >= 5)
			{
				while (rowCount != 0)
				{
					this.map[--tmpRow, col] = 0;
					this.buttons[tmpRow, col].BackColor = Color.Gray;
					rowCount--;
				}
				return (true);
			}
			return (false);
		}

		private bool columnsCheck(int row, int col, int color)
		{
			int		tmpCol = col;
			int		colCount = 1;

			while (--tmpCol >= 0 && this.map[row, tmpCol] == color)
				colCount++;
			tmpCol = col;
			while (++tmpCol < 9 && this.map[row, tmpCol] == color)
				colCount++;
			if (colCount >= 5)
			{
				while (colCount != 0)
				{
					this.map[row, --tmpCol] = 0;
					this.buttons[row, tmpCol].BackColor = Color.Gray;
					colCount--;
				}
				return (true);
			}
			return (false);
		}

		private bool	diagonalCheck(int row, int col, int color)
		{
			int tmpRow = row - 1, tmpCol = col - 1;
			int diagCount = 1;

			while (tmpRow >= 0 && tmpCol >= 0 && this.map[tmpRow, tmpCol] == color)
			{
				diagCount++;
				tmpRow--;
				tmpCol--;
			}
			tmpRow = row + 1; tmpCol = col + 1;
			while (tmpRow < 9 && tmpCol < 9 && this.map[tmpRow, tmpCol] == color)
			{
				diagCount++;
				tmpRow++;
				tmpCol++;
			}
			if (diagCount >= 5)
			{
				while (diagCount != 0)
				{
					this.map[--tmpRow, --tmpCol] = 0;
					this.buttons[tmpRow, tmpCol].BackColor = Color.Gray;
					diagCount--;
				}
				return (true);
			}

			tmpRow = row + 1; tmpCol = col - 1;
			diagCount = 1;

			while (tmpRow < 9 && tmpCol >= 0 && this.map[tmpRow, tmpCol] == color)
			{
				diagCount++;
				tmpRow++;
				tmpCol--;
			}
			tmpRow = row - 1; tmpCol = col + 1;
			while (tmpRow >= 0 && tmpCol < 9 && this.map[tmpRow, tmpCol] == color)
			{
				diagCount++;
				tmpRow--;
				tmpCol++;
			}
			if (diagCount >= 5)
			{
				while (diagCount != 0)
				{
					this.map[++tmpRow, --tmpCol] = 0;
					this.buttons[tmpRow, tmpCol].BackColor = Color.Gray;
					diagCount--;
				}
				return (true);
			}
			return (false);
		}

		private bool	colorsInLineCheck(int row, int col)
		{
			return (rowsCheck(row, col, this.map[row, col]) || columnsCheck(row, col, this.map[row, col])
				|| diagonalCheck(row, col, this.map[row, col]));
		}

		public static void getLine()
		{
			Console.WriteLine("LINE: {0}", ((new StackTrace(new StackFrame(true))).GetFrame(0)).GetFileLineNumber());
		}

		private bool mapIsFilled()
		{
			for (int i = 0; i < mapSize; ++i)
				for (int j = 0; j < mapSize; ++j)
					if (this.map[i, j] == 0)
						return (false);
			return (true);
		}

		public void addColors(int count)
		{

			Random rand = new Random();
			int c1, c3;
			for (int i = 0; i < count && !mapIsFilled(); ++i)
			{
				c3 = rand.Next(5);
				do
				{
					c1 = rand.Next(mapSize * mapSize);
				} while (this.map[c1 / mapSize, c1 % mapSize] != 0);
				this.buttons[c1 / mapSize, c1 % mapSize].BackColor = colors[c3];
				this.map[c1 / mapSize, c1 % mapSize] = -(c3 + 1);
				//Console.WriteLine("Added at " + c1 / mapSize + c1 % mapSize + " color " + -(c3 + 1));
				colorsInLineCheck(c1 / mapSize, c1 % mapSize);
			}
		}

		public void InitButtons()
		{
			this.buttons = new RoundButton[mapSize, mapSize];
			this.map = new int[mapSize, mapSize];

			this.SuspendLayout();
			for (int i = 0; i < mapSize; i++)
			{
				for (int j = 0; j < mapSize; j++)
				{
					this.map[i, j] = 0;
					this.buttons[i, j] = new RoundButton();
					this.buttons[i, j].Location =
						new System.Drawing.Point(buttonSize * j, buttonSize * i);
					this.buttons[i, j].Name = "" + i + j;
					this.buttons[i, j].Size = new System.Drawing.Size(buttonSize, buttonSize);
					this.buttons[i, j].TabIndex = 0;
					this.buttons[i, j].BackColor = Color.Gray;
					this.buttons[i, j].Click += new System.EventHandler(this.mapButtonClick);
					this.Controls.Add(this.buttons[i, j]);
				}
			}
			ResumeLayout();
		}

		private void findAllValidPaths(int row, int col)
		{
			Queue<myTuple> queue = new Queue<myTuple>();
			int start = 10;
			myTuple tmp;

			queue.Enqueue(new myTuple(row, col));
			this.map[row, col] = start;

			while (queue.Count > 0)
			{
				tmp = queue.Dequeue();
				row = tmp.row;
				col = tmp.col;

				if (row < 8 && this.map[row + 1, col] == 0)
				{
					this.map[row + 1, col] = this.map[row, col] + 1;
					queue.Enqueue(new myTuple(row + 1, col));
				}
				if (col < 8 && this.map[row, col + 1] == 0)
				{
					this.map[row, col + 1] = this.map[row, col] + 1;
					queue.Enqueue(new myTuple(row, col + 1));
				}
				if (row > 0 && this.map[row - 1, col] == 0)
				{
					this.map[row - 1, col] = this.map[row, col] + 1;
					queue.Enqueue(new myTuple(row - 1, col));
				}
				if (col > 0 && this.map[row, col - 1] == 0)
				{
					this.map[row, col - 1] = this.map[row, col] + 1;
					queue.Enqueue(new myTuple(row, col - 1));
				}
				//Console.WriteLine("-----------ITERACIAAAAAA-----------");
				//printMap();
				//Console.WriteLine("-----------ITERACIAAAAAA-----------");
			}
			Console.WriteLine("Posle cikla");
			printMap();
		}

		private void updateMap()
		{
			Console.WriteLine("IN FUNCTION RESTARTMAP()");
			//printMap();
			for (int i = 0; i < mapSize; ++i)
			{
				for (int j = 0; j < mapSize; ++j)
				{
					if (this.map[i, j] >= 10)
						this.map[i, j] = 0;
				}
			}
		}

		private void changeColor(ref Stack<myTuple> way, int srcRow, int srcCol)
		{
			myTuple tmp;

			while (way.Count > 0)
			{
				tmp = way.Pop();
				this.buttons[tmp.row, tmp.col].BackColor = this.buttons[srcRow, srcCol].BackColor;
				this.buttons[srcRow, srcCol].BackColor = Color.Gray;
				srcRow = tmp.row;
				srcCol = tmp.col;
			}
		}

		private Stack<myTuple> findShortestWay(int dstRow, int dstCol)
		{
			Console.WriteLine("-----------In function findshortway----------");
			//printMap();

			Stack<myTuple> way = new Stack<myTuple>();
			while (true)
			{
				if (dstRow + 1 < 9 && this.map[dstRow + 1, dstCol] == this.map[dstRow, dstCol] - 1)
				{
					way.Push(new myTuple(dstRow, dstCol));
					dstRow = dstRow + 1;
				}
				else if (dstRow - 1 >= 0 && this.map[dstRow - 1, dstCol] == this.map[dstRow, dstCol] - 1)
				{
					way.Push(new myTuple(dstRow, dstCol));
					dstRow = dstRow - 1;
				}
				else if (dstCol + 1 < 9 && this.map[dstRow, dstCol + 1] == this.map[dstRow, dstCol] - 1)
				{
					way.Push(new myTuple(dstRow, dstCol));
					dstCol = dstCol + 1;
				}
				else if (dstCol - 1 >= 0 && this.map[dstRow, dstCol - 1] == this.map[dstRow, dstCol] - 1)
				{
					way.Push(new myTuple(dstRow, dstCol));
					dstCol = dstCol - 1;
				}
				else
				{
					break;
				}
				//Console.WriteLine(srcRow + " " +  srcCol + " and " + dstRow + " " + dstCol);
			}
			Console.WriteLine("++++++++++++++++I BREAK++++++++++++++++");
			//printMap();
			return (way);
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			while (pressed.Count > 0)
				pressed.Pop();
			if (++pressCount == 1)
			{
				InitButtons();
				addColors(5);
			}
			else
			{
				deletePrevColors();
				addColors(5);
			}
		}

		private void mapButtonClick(object sender, EventArgs e)
		{
			int row, col;
			pickButtonIndexes(out row, out col, (RoundButton)sender);
			Console.Write("row = " + row);
			Console.WriteLine("col = " + col);
			Console.WriteLine("ahaaaa " + pressed.Count);
			Console.WriteLine("yani " + this.map[row, col]);
			if (this.map[row, col] < 0)
			{
				if (pressed.Count == 1)
				{
					RoundButton tmp = pressed.Pop();
					int tmpRow, tmpCol;

					pickButtonIndexes(out tmpRow, out tmpCol, tmp);
					this.map[tmpRow, tmpCol] = colorValue;
					updateMap();
				}
				this.colorValue = this.map[row, col];
				pressed.Push((RoundButton)sender);
				findAllValidPaths(row, col);
			}
			else if (this.map[row, col] > 0 && this.pressed.Count == 1)
			{
				RoundButton tmp = this.pressed.Peek();
				int srcRow, srcCol;

				pickButtonIndexes(out srcRow, out srcCol, tmp);
				if (srcRow == row && srcCol == col) { return; }
				pressed.Pop();
				Stack<myTuple> way = findShortestWay(row, col);
				Console.WriteLine("OSTOROJNO " + row + col + " PRCAV OSTOROJNON");
				// GUYNY TEXAPOXUM A heto restart map
				changeColor(ref way, srcRow, srcCol);
				updateMap();
				this.map[row, col] = colorValue;
				this.colorValue = 0;
				// TAZA GUYNER A AVELACNUM
				if (!colorsInLineCheck(row, col))
				{
					addColors(3);
					if (mapIsFilled())
						;
				}
				Console.WriteLine("++++++++++++++++++++AFTER ALL++++++++++++++++++++");
				printMap();
				Console.WriteLine("++++++++++++++++++++RESET STEPS++++++++++++++++++++");


			}
		}
	}
	//class Click
	//{

	//}
}