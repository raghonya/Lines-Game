using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Runtime.CompilerServices;

namespace LinesUpdate
{
	public partial class Form1 : Form
	{
		private Map map = new Map();
		private Load load = new Load();
		private Colors colors = new Colors();
		private Stack<RoundButton> pressed = new Stack<RoundButton>();
		private RoundButton[,] buttons = new RoundButton[Map.size, Map.size];
		private int score = 0;
		private const int buttonSize = 50;
		private int colorValue = 0;
		private int highestScore = 0;
		private static int newGamePressed = 0;
		private bool gameInProcess = false;

		public class MyTuple
		{
			public int row;
			public int col;
			public MyTuple(int row, int col) { this.row = row; this.col = col; }
		}

		private void DisableControls(Control con)
		{

			foreach (Control c in con.Controls)
				DisableControls(c);
			con.Enabled = false;
		}

		private void EnableControls(Control con)
		{
			foreach (Control c in con.Controls)
				EnableControls(c);
			con.Enabled = true;
		}

		public Form1()
		{
			this.KeyPreview = true;
			this.KeyDown += new KeyEventHandler(this.Form_KeyDown);

			InitializeComponent();
			this.SuspendLayout();
			colors.initNextColors(this.Controls, buttonSize);
			this.ResumeLayout();
			colors.NextColors(ref load, 3, false);
		}

		private void pickButtonIndexes(out int row, out int col, RoundButton button)
		{
			row = button.Name[0] - '0';
			col = button.Name[1] - '0';
		}

		public bool gameOverCheck()
		{
			if (map.mapIsFilled())
			{
				for (int i = 0; i < Map.size; ++i)
					for (int j = 0; j < Map.size; ++j)
						this.buttons[i, j].Enabled = false;
				this.highestScore = highestScore >= score ? highestScore : score;
				this.gameOverLabel.Text = "Game over\nYour score: " + score
					+ "\nHighest score: " + highestScore;
				this.Controls.Add(this.gameOverLabel);
				this.gameOverLabel.BringToFront();
				return (true);
			}
			return (false);
		}

		public void InitButtons()
		{
			this.SuspendLayout();
			for (int i = 0; i < Map.size; i++)
			{
				for (int j = 0; j < Map.size; j++)
				{
					this.buttons[i, j] = new RoundButton();
					this.buttons[i, j].Location =
						new System.Drawing.Point(buttonSize * j, buttonSize * i);
					this.buttons[i, j].Name = "" + i + j;
					this.buttons[i, j].Size = new System.Drawing.Size(buttonSize, buttonSize);
					this.buttons[i, j].TabIndex = 0;
					this.buttons[i, j].FlatStyle = FlatStyle.Flat;
					this.buttons[i, j].Click += new System.EventHandler(this.mapButtonClick);
					this.buttons[i, j].MouseEnter += new System.EventHandler(this.mouseEnter);
					this.buttons[i, j].MouseLeave += new System.EventHandler(this.mouseLeave);
					this.Controls.Add(this.buttons[i, j]);
				}
			}
			this.ResumeLayout();
		}

		public void mouseEnter(object sender, EventArgs e)
		{
			RoundButton button = (RoundButton)sender;
			int row, col;

			pickButtonIndexes(out row, out col, button);
			if (map.values[row, col] == 0 || pressed.Count != 1 || button.BackColor != Color.Gray)
				return;
			map.findShortestWay(ref this.buttons, this.colors, row, col);
			this.Refresh();
		}

		public void mouseLeave(object sender, EventArgs e)
		{
			Button button = (Button)sender;

			for (int i = 0; i < Map.size; ++i)
				for (int j = 0; j < Map.size; ++j)
					if (buttons[i, j].BackColor.A == 100)
						buttons[i, j].BackColor = Color.Gray;
			this.Refresh();
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			gameInProcess = true;
			this.score = 0;
			this.scoreLabel.Text = "Score: 0";
			this.gameOverLabel.Text = "Game Over\nYour score: 0";
			this.Controls.Remove(this.gameOverLabel);
			while (pressed.Count > 0)
				pressed.Pop();
			if (++newGamePressed == 1)
				InitButtons();
			map.clearMap(ref this.buttons);
			for (int i = 0; i < Map.size; ++i)
			{
				if (i < 3)
					colors.nextColors[i].Visible = true;
				for (int j = 0; j < Map.size; ++j)
				{
					map.values[i, j] = 0;
					this.buttons[i, j].Enabled = true;
				}
			}
			colors.addColors(ref load, ref map, ref buttons, 3, false);
			gameOverCheck();
		}

		private void loadButtonClick(object sender, EventArgs e)
		{
			gameInProcess = true;
			load.checkFileExistence();
			while (pressed.Count > 0)
				pressed.Pop();
			if (++newGamePressed == 1)
			{
				InitButtons();
				colors.NextColors(ref load, 3, true);
			}
			map.clearMap(ref this.buttons);
			if (load.getFileExistence())
			{
				load.loadGame(ref this.score, ref map.values, ref this.buttons, colors.arr);
				this.scoreLabel.Text = "Score: " + this.score;
			}
			else
				; // PTI YLNI MESSAGE "CANT LOAD GAME, PLAY WITHOUT LOAD HEHE" 
			for (int i = 0; i < 3; ++i)
			{
				colors.nextColors[i].Visible = true;
				if (load.colorValue[i] != 0)
					colors.nextColors[i].BackColor = colors.arr[-load.colorValue[i] - 1];
			}
			if (!gameOverCheck())
				this.Controls.Remove(this.gameOverLabel);
		}

		public void saveButtonClick(object sender, EventArgs e)
		{
			if (gameInProcess/* && this.pressed.Count == 0*/)
				this.load.createLoad(this.colors, this.map, this.score);
		}

		private void mapButtonClick(object sender, EventArgs e)
		{
			int row, col;
			pickButtonIndexes(out row, out col, (RoundButton)sender);
			if (map.values[row, col] < 0)
			{
				//Console.WriteLine(("AAAAAAAAAAAAAAAAAAAAAAAAAAAABBASBSGD ASGDASBDA"));
				this.startButton.Enabled = false;
				this.loadButton.Enabled = false;
				this.saveButton.Enabled = false;
				if (pressed.Count == 1)
				{
					RoundButton tmp = pressed.Pop();
					int tmpRow, tmpCol;

					pickButtonIndexes(out tmpRow, out tmpCol, tmp);
					map.values[tmpRow, tmpCol] = colorValue;
					map.updateMap();
				}
				this.colorValue = map.values[row, col];
				pressed.Push((RoundButton)sender);
				map.findAllValidPaths(row, col);
			}
			else if (map.values[row, col] > 0 && this.pressed.Count == 1)
			{
				RoundButton tmp = this.pressed.Peek();
				int srcRow, srcCol;
				int exploded;

				pickButtonIndexes(out srcRow, out srcCol, tmp);
				if (srcRow == row && srcCol == col) { return; }
				pressed.Pop();
				Stack<MyTuple> way = map.findShortestWay(ref this.buttons, this.colors, row, col);
				colors.changeColor(this, ref this.buttons, ref way, srcRow, srcCol);
				map.updateMap();
				map.values[row, col] = colorValue;
				this.colorValue = 0;

				exploded = map.colorsInLineCheck(ref this.buttons, row, col);
				if (exploded == 0)
				{
					colors.addColors(ref load, ref map, ref buttons, 3, true);
					gameOverCheck();
				}
				else
				{
					this.score += (exploded == 5 ? 10 : 10 + ((exploded - 5) * 4));
					this.scoreLabel.Text = "Score: " + score;
				}
				this.startButton.Enabled = true;
				this.loadButton.Enabled = true;
				this.saveButton.Enabled = true;
				//Console.WriteLine("++++++++++++++++++++AFTER ALL++++++++++++++++++++");
				Map.printValues(map.values);
				//Console.WriteLine("++++++++++++++++++++RESET STEPS++++++++++++++++++++");
				//load.createLoad(this.colors, this.map, this.score);
			}
		}
		private void Form_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}
	}
}