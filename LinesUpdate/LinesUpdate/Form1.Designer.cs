using System.Drawing;
using System.Drawing.Drawing2D;

namespace LinesUpdate
{
	public class RoundButton : System.Windows.Forms.Button
	{
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			GraphicsPath grPath = new GraphicsPath();
			grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
			this.Region = new System.Drawing.Region(grPath);
			base.OnPaint(e);
		}
	}

	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.startButton = new System.Windows.Forms.Button();
			this.scoreLabel = new System.Windows.Forms.Label();
			this.gameOverLabel = new System.Windows.Forms.Label();
			this.loadButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// startButton
			// 
			this.startButton.Font = new System.Drawing.Font("Arial", 8F);
			this.startButton.Location = new System.Drawing.Point(750, 36);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(100, 30);
			this.startButton.TabIndex = 0;
			this.startButton.Text = "New Game";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.startButtonClick);
			// 
			// scoreLabel
			// 
			this.scoreLabel.BackColor = this.BackColor;
			this.scoreLabel.Font = new System.Drawing.Font("Arial", 9F);
			this.scoreLabel.ForeColor = System.Drawing.Color.Black;
			this.scoreLabel.Location = new System.Drawing.Point(750, 150);
			this.scoreLabel.Name = "scoreLabel";
			this.scoreLabel.Size = new System.Drawing.Size(120, 30);
			this.scoreLabel.TabIndex = 3;
			// 
			// gameOverLabel
			// 
			this.gameOverLabel.AutoSize = true;
			this.gameOverLabel.BackColor = System.Drawing.Color.White;
			this.gameOverLabel.Font = new System.Drawing.Font("Arial", 20F);
			this.gameOverLabel.ForeColor = System.Drawing.Color.Black;
			this.gameOverLabel.Location = new System.Drawing.Point(200, 200);
			this.gameOverLabel.Name = "gameOverLabel";
			this.gameOverLabel.Size = new System.Drawing.Size(100, 23);
			this.gameOverLabel.TabIndex = 4;
			this.gameOverLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// loadButton
			// 
			this.loadButton.Font = new System.Drawing.Font("Arial", 8F);
			this.loadButton.Location = new System.Drawing.Point(750, 72);
			this.loadButton.Name = "loadButton";
			this.loadButton.Size = new System.Drawing.Size(100, 30);
			this.loadButton.TabIndex = 1;
			this.loadButton.Text = "Load Game";
			this.loadButton.UseVisualStyleBackColor = true;
			this.loadButton.Click += new System.EventHandler(this.loadButtonClick);
			// 
			// saveButton
			// 
			this.saveButton.Font = new System.Drawing.Font("Arial", 8F);
			this.saveButton.Location = new System.Drawing.Point(750, 108);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(100, 30);
			this.saveButton.TabIndex = 2;
			this.saveButton.Text = "Save Game";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButtonClick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(900, 630);
			this.Controls.Add(this.scoreLabel);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.loadButton);
			this.Controls.Add(this.startButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Form1";
			this.Text = "Lines";
			this.ResumeLayout(false);

		}
		#endregion

		private	System.Windows.Forms.Button	startButton;
		private System.Windows.Forms.Label	gameOverLabel;
		private System.Windows.Forms.Label	scoreLabel;
		private System.Windows.Forms.Button loadButton;
		private System.Windows.Forms.Button saveButton;
	}
}

