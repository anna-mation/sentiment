namespace sentiment_analysis_v3
{
    partial class Form6
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
            this.resultstextbox = new System.Windows.Forms.RichTextBox();
            this.confidencelabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.sentimentlabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // resultstextbox
            // 
            this.resultstextbox.BackColor = System.Drawing.SystemColors.Window;
            this.resultstextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultstextbox.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Bold);
            this.resultstextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(184)))), ((int)(((byte)(180)))));
            this.resultstextbox.Location = new System.Drawing.Point(451, 109);
            this.resultstextbox.Name = "resultstextbox";
            this.resultstextbox.ReadOnly = true;
            this.resultstextbox.Size = new System.Drawing.Size(322, 186);
            this.resultstextbox.TabIndex = 10;
            this.resultstextbox.Text = "";
            // 
            // confidencelabel
            // 
            this.confidencelabel.AutoSize = true;
            this.confidencelabel.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Bold);
            this.confidencelabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(184)))), ((int)(((byte)(180)))));
            this.confidencelabel.Location = new System.Drawing.Point(30, 109);
            this.confidencelabel.Name = "confidencelabel";
            this.confidencelabel.Size = new System.Drawing.Size(101, 21);
            this.confidencelabel.TabIndex = 9;
            this.confidencelabel.Text = "Confidence:";
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.Salmon;
            this.progressBar1.Location = new System.Drawing.Point(34, 132);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(381, 35);
            this.progressBar1.TabIndex = 8;
            // 
            // sentimentlabel
            // 
            this.sentimentlabel.AutoSize = true;
            this.sentimentlabel.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Bold);
            this.sentimentlabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(184)))), ((int)(((byte)(180)))));
            this.sentimentlabel.Location = new System.Drawing.Point(30, 76);
            this.sentimentlabel.Name = "sentimentlabel";
            this.sentimentlabel.Size = new System.Drawing.Size(93, 21);
            this.sentimentlabel.TabIndex = 7;
            this.sentimentlabel.Text = "Sentiment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.label3.Location = new System.Drawing.Point(447, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 28);
            this.label3.TabIndex = 6;
            this.label3.Text = "Results";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(215)))), ((int)(((byte)(208)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(163)))), ((int)(((byte)(160)))));
            this.button1.Location = new System.Drawing.Point(34, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 63);
            this.button1.TabIndex = 15;
            this.button1.Text = "results doc";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(215)))), ((int)(((byte)(208)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(163)))), ((int)(((byte)(160)))));
            this.button2.Location = new System.Drawing.Point(274, 205);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(141, 63);
            this.button2.TabIndex = 16;
            this.button2.Text = "main menu";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Gainsboro;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.Gray;
            this.button3.Location = new System.Drawing.Point(12, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 33);
            this.button3.TabIndex = 17;
            this.button3.Text = "← ";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(184)))), ((int)(((byte)(180)))));
            this.label1.Location = new System.Drawing.Point(496, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 21);
            this.label1.TabIndex = 18;
            this.label1.Text = "0% scanned";
            // 
            // progressBar2
            // 
            this.progressBar2.ForeColor = System.Drawing.Color.Salmon;
            this.progressBar2.Location = new System.Drawing.Point(109, 12);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(381, 35);
            this.progressBar2.TabIndex = 19;
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.resultstextbox);
            this.Controls.Add(this.confidencelabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.sentimentlabel);
            this.Controls.Add(this.label3);
            this.Name = "Form6";
            this.Text = "Form6";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form6_FormClosing);
            this.Load += new System.EventHandler(this.Form6_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox resultstextbox;
        private System.Windows.Forms.Label confidencelabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label sentimentlabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar2;
    }
}