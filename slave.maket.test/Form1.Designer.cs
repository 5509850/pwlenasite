namespace slave.maket.test
{
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
            this.textBox_codeA = new System.Windows.Forms.TextBox();
            this.button_codeA = new System.Windows.Forms.Button();
            this.label_count = new System.Windows.Forms.Label();
            this.button_viewModel = new System.Windows.Forms.Button();
            this.label_code = new System.Windows.Forms.Label();
            this.button_exp = new System.Windows.Forms.Button();
            this.textBox_exp = new System.Windows.Forms.TextBox();
            this.textBox_data = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_codeA
            // 
            this.textBox_codeA.Location = new System.Drawing.Point(25, 49);
            this.textBox_codeA.Name = "textBox_codeA";
            this.textBox_codeA.Size = new System.Drawing.Size(100, 20);
            this.textBox_codeA.TabIndex = 0;
            this.textBox_codeA.TextChanged += new System.EventHandler(this.textBox_codeA_TextChanged);
            // 
            // button_codeA
            // 
            this.button_codeA.Location = new System.Drawing.Point(142, 49);
            this.button_codeA.Name = "button_codeA";
            this.button_codeA.Size = new System.Drawing.Size(124, 23);
            this.button_codeA.TabIndex = 1;
            this.button_codeA.Text = "get code A(rest + sql)";
            this.button_codeA.UseVisualStyleBackColor = true;
            this.button_codeA.Click += new System.EventHandler(this.button_codeA_Click);
            // 
            // label_count
            // 
            this.label_count.AutoSize = true;
            this.label_count.Location = new System.Drawing.Point(25, 85);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(13, 13);
            this.label_count.TabIndex = 2;
            this.label_count.Text = "0";
            this.label_count.Click += new System.EventHandler(this.label_count_Click);
            // 
            // button_viewModel
            // 
            this.button_viewModel.Location = new System.Drawing.Point(142, 108);
            this.button_viewModel.Name = "button_viewModel";
            this.button_viewModel.Size = new System.Drawing.Size(124, 23);
            this.button_viewModel.TabIndex = 3;
            this.button_viewModel.Text = "ViewModel";
            this.button_viewModel.UseVisualStyleBackColor = true;
            this.button_viewModel.Click += new System.EventHandler(this.button_viewModel_Click);
            // 
            // label_code
            // 
            this.label_code.AutoSize = true;
            this.label_code.Location = new System.Drawing.Point(28, 117);
            this.label_code.Name = "label_code";
            this.label_code.Size = new System.Drawing.Size(31, 13);
            this.label_code.TabIndex = 4;
            this.label_code.Text = "code";
            // 
            // button_exp
            // 
            this.button_exp.Location = new System.Drawing.Point(180, 192);
            this.button_exp.Name = "button_exp";
            this.button_exp.Size = new System.Drawing.Size(75, 23);
            this.button_exp.TabIndex = 5;
            this.button_exp.Text = "Exp";
            this.button_exp.UseVisualStyleBackColor = true;
            this.button_exp.Click += new System.EventHandler(this.button_exp_Click);
            // 
            // textBox_exp
            // 
            this.textBox_exp.Location = new System.Drawing.Point(25, 192);
            this.textBox_exp.Name = "textBox_exp";
            this.textBox_exp.Size = new System.Drawing.Size(100, 20);
            this.textBox_exp.TabIndex = 6;
            // 
            // textBox_data
            // 
            this.textBox_data.Location = new System.Drawing.Point(25, 217);
            this.textBox_data.Name = "textBox_data";
            this.textBox_data.Size = new System.Drawing.Size(230, 20);
            this.textBox_data.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textBox_data);
            this.Controls.Add(this.textBox_exp);
            this.Controls.Add(this.button_exp);
            this.Controls.Add(this.label_code);
            this.Controls.Add(this.button_viewModel);
            this.Controls.Add(this.label_count);
            this.Controls.Add(this.button_codeA);
            this.Controls.Add(this.textBox_codeA);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Slave Maket";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_codeA;
        private System.Windows.Forms.Button button_codeA;
        private System.Windows.Forms.Label label_count;
        private System.Windows.Forms.Button button_viewModel;
        private System.Windows.Forms.Label label_code;
        private System.Windows.Forms.Button button_exp;
        private System.Windows.Forms.TextBox textBox_exp;
        private System.Windows.Forms.TextBox textBox_data;
    }
}

