using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinesUpdate
{
	internal class Load
	{
		string[]			fileInfo;
		public int[]		colorValue = new int[3];
		bool				loadFileExistence = false;
		private string		loadFileName = "./load.txt";

		public bool getFileExistence()
		{ return (loadFileExistence); }
		public	Load() { }

		public void	checkFileExistence()
		{
			loadFileExistence = false;
			if (File.Exists(this.loadFileName))
			{
				loadFileExistence = true;
			}
		}

		public void loadGame(ref int score, ref int[,] map, ref Form1.RoundButton[,]	buttons, Color[] colors)
		{
			string tmp = File.ReadAllText("./load.txt");

			/* SCORE LOAD */
			score = atoi(tmp);
			tmp = tmp.Remove(0, score.ToString().Length + 1);
			/* SCORE LOAD */

			/* COLORS LOAD */
			this.colorValue[0] = (-(tmp[0] - '0') - 1);
			this.colorValue[1] = (-(tmp[1] - '0') - 1);
			this.colorValue[2] = (-(tmp[2] - '0') - 1);
			tmp = tmp.Remove(0, 4);
			/* COLORS LOAD */
			
			/* MAP LOAD */
			fileInfo = tmp.Split('\n');
			for (int i = 0; i < fileInfo.Length; ++i)
			{
				for (int j = 0; j < fileInfo[i].Length; ++j)
				{
					//Console.WriteLine((int)(fileInfo[i][j]));
					//Console.WriteLine("i = " + i + " j = " + j);
					map[i, j] = -(fileInfo[i][j] - '0');
					buttons[i, j].BackColor =
						map[i, j] != 0 ? colors[-map[i, j] - 1] : Color.Gray;
				}
				Console.WriteLine();
			}
			/* MAP LOAD */
		}

		public void createLoad(Colors color, Map map, int score)
		{
			string mapInfo = "";
			int k = 0;

			for (int i = 0; i < Map.size; ++i)
			{
				for (int j = 0; j < Map.size; ++j)
					mapInfo = mapInfo.Insert(k++, -map.values[i, j] + "");
				mapInfo = mapInfo.Insert(k++, "\n");
			}
			File.WriteAllText(this.loadFileName, score + "\n");
			for (int i = 0; i < 3; ++i)
				File.AppendAllText(this.loadFileName, 
					(-color.getColorValue(color.nextColors[i].BackColor) - 1) + "");
			File.AppendAllText(this.loadFileName, mapInfo);
		}
		public int	atoi(string s)
		{
			int i;
			int n;
			int sign;

			i = 0;
			n = 0;
			sign = 1;
			if (s == null)
				return (0);
			while (s[i] == '\t' || s[i] == '\f' || s[i] == '\r'
				|| s[i] == '\n' || s[i] == '\v' || s[i] == ' ')
				i++;
			if (s[i] == '-' || s[i] == '+')
				if (s[i++] == '-')
					sign = -1;
			while (s[i] > 47 && s[i] < 58)
				n = (n * 10) + (s[i++] - 48);
			return (sign * n);
		}
	}
}
