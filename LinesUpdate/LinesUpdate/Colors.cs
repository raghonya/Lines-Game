using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LinesUpdate.Form1;

namespace LinesUpdate
{
	internal class Colors
	{
		public Color[]			arr = new Color[5];
		public Color[]			way = new Color[5];
		public RoundButton[]	nextColors = new RoundButton[3];

		public Colors()
		{
			this.arr[0] = Color.FromArgb(255, 0, 0);
			this.arr[1] = Color.FromArgb(0, 255, 0);
			this.arr[2] = Color.FromArgb(0, 0, 255);
			this.arr[3] = Color.FromArgb(247, 0, 255);
			this.arr[4] = Color.FromArgb(255, 255, 0);
			this.way[0] = Color.FromArgb(100, 255, 0, 0);
			this.way[1] = Color.FromArgb(100, 0, 255, 0);
			this.way[2] = Color.FromArgb(100, 0, 0, 255);
			this.way[3] = Color.FromArgb(100, 247, 0, 255);
			this.way[4] = Color.FromArgb(100, 255, 255, 0);
		}

		public void initNextColors(Control.ControlCollection Controls, int buttonSize)
		{
			for (int i = 0; i < 3; ++i)
			{
				this.nextColors[i] = new RoundButton();
				this.nextColors[i].Location =
					new System.Drawing.Point(500, 150 + (buttonSize * i));
				this.nextColors[i].Name = "next" + i + 1;
				this.nextColors[i].Size = new System.Drawing.Size(buttonSize, buttonSize);
				this.nextColors[i].TabIndex = 0;
				this.nextColors[i].FlatStyle = FlatStyle.Flat;
				this.nextColors[i].BackColor = Color.Gray;
				this.nextColors[i].Enabled = false;
				this.nextColors[i].Visible = false;
				Controls.Add(this.nextColors[i]);
			}
		}

		public void NextColors(ref Load load, int count, bool isLoad)
		{
			Random rand = new Random();
			int c1, c2;

			for (int i = 0; i < count /*&& !map.mapIsFilled()*/; ++i)
			{
				c2 = rand.Next(5);
				this.nextColors[i].BackColor = this.arr[c2];
				if (isLoad)
					load.colorValue[i] = -(c2 + 1);
			}
		}

		public void changeColor(Form1 form, ref RoundButton[,] buttons, ref Stack<MyTuple> way, int srcRow, int srcCol)
		{
			MyTuple tmp;

			while (way.Count > 0)
			{
				tmp = way.Pop();
				buttons[tmp.row, tmp.col].BackColor = buttons[srcRow, srcCol].BackColor;
				buttons[srcRow, srcCol].BackColor = Color.Gray;
				form.Refresh();
				srcRow = tmp.row;
				srcCol = tmp.col;
			}
		}

		public void addColors(ref Load load, ref Map map, ref RoundButton[,] buttons, int count, bool isLoad)
		{
			Random rand = new Random();
			int c1, c2;

			for (int i = 0; i < count && !map.mapIsFilled(); ++i)
			{
				do
				{
					c1 = rand.Next(Map.size * Map.size);
				} while (map.values[c1 / Map.size, c1 % Map.size] != 0);
				buttons[c1 / Map.size, c1 % Map.size].BackColor =
					this.nextColors[i].BackColor;
				int j = 0;
				for (; j < 5; ++j)
					if (this.nextColors[i].BackColor == this.arr[j])
						break;
				map.values[c1 / Map.size, c1 % Map.size] = -(j + 1);
				map.colorsInLineCheck(ref buttons, c1 / Map.size, c1 % Map.size);
			}
			this.NextColors(ref load, 3, isLoad);
			//gameOverCheck();
		}

		public int	getColorValue(Color color)
		{
			for (int i = 0; i < this.arr.Length; ++i)
				if (this.arr[i] == color)
					return (-i - 1);
			return (0);
		}

	}
}
