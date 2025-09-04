namespace sentiment_analysis_v3
{
    partial class Form3
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
            this.label3 = new System.Windows.Forms.Label();
            this.sentimentlabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.confidencelabel = new System.Windows.Forms.Label();
            this.resultstextbox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.label3.Location = new System.Drawing.Point(244, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 32);
            this.label3.TabIndex = 0;
            this.label3.Text = "Results";
            // 
            // sentimentlabel
            // 
            this.sentimentlabel.AutoSize = true;
            this.sentimentlabel.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sentimentlabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(184)))), ((int)(((byte)(180)))));
            this.sentimentlabel.Location = new System.Drawing.Point(246, 289);
            this.sentimentlabel.Name = "sentimentlabel";
            this.sentimentlabel.Size = new System.Drawing.Size(93, 21);
            this.sentimentlabel.TabIndex = 1;
            this.sentimentlabel.Text = "Sentiment:";
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.Salmon;
            this.progressBar1.Location = new System.Drawing.Point(250, 351);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(320, 35);
            this.progressBar1.TabIndex = 2;
            // 
            // confidencelabel
            // 
            this.confidencelabel.AutoSize = true;
            this.confidencelabel.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confidencelabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(184)))), ((int)(((byte)(180)))));
            this.confidencelabel.Location = new System.Drawing.Point(246, 318);
            this.confidencelabel.Name = "confidencelabel";
            this.confidencelabel.Size = new System.Drawing.Size(101, 21);
            this.confidencelabel.TabIndex = 3;
            this.confidencelabel.Text = "Confidence:";
            // 
            // resultstextbox
            // 
            this.resultstextbox.BackColor = System.Drawing.SystemColors.Window;
            this.resultstextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultstextbox.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultstextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(184)))), ((int)(((byte)(180)))));
            this.resultstextbox.Location = new System.Drawing.Point(248, 90);
            this.resultstextbox.Name = "resultstextbox";
            this.resultstextbox.ReadOnly = true;
            this.resultstextbox.Size = new System.Drawing.Size(322, 186);
            this.resultstextbox.TabIndex = 5;
            this.resultstextbox.Text = "";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(215)))), ((int)(((byte)(208)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(163)))), ((int)(((byte)(160)))));
            this.button1.Location = new System.Drawing.Point(647, 351);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 63);
            this.button1.TabIndex = 17;
            this.button1.Text = "main menu";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Gainsboro;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.Gray;
            this.button2.Location = new System.Drawing.Point(12, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 33);
            this.button2.TabIndex = 18;
            this.button2.Text = "← ";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.resultstextbox);
            this.Controls.Add(this.confidencelabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.sentimentlabel);
            this.Controls.Add(this.label3);
            this.Name = "Form3";
            this.Text = "Form3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label sentimentlabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label confidencelabel;
        private System.Windows.Forms.RichTextBox resultstextbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}