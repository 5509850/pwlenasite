namespace MLDBUtils
{
    partial class ChPwd
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
            this.button1 = new System.Windows.Forms.Button();
            this.tOld = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tNew1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tNew2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(178, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Сменить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tOld
            // 
            this.tOld.Location = new System.Drawing.Point(153, 6);
            this.tOld.Name = "tOld";
            this.tOld.PasswordChar = '*';
            this.tOld.Size = new System.Drawing.Size(100, 20);
            this.tOld.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Старый пароль";
            // 
            // tNew1
            // 
            this.tNew1.Location = new System.Drawing.Point(153, 32);
            this.tNew1.Name = "tNew1";
            this.tNew1.PasswordChar = '*';
            this.tNew1.Size = new System.Drawing.Size(100, 20);
            this.tNew1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Новый пароль";
            // 
            // tNew2
            // 
            this.tNew2.Location = new System.Drawing.Point(153, 58);
            this.tNew2.Name = "tNew2";
            this.tNew2.PasswordChar = '*';
            this.tNew2.Size = new System.Drawing.Size(100, 20);
            this.tNew2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Повторить новый пароль";
            // 
            // ChPwd
            // 
            this.ClientSize = new System.Drawing.Size(260, 127);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tNew2);
            this.Controls.Add(this.tNew1);
            this.Controls.Add(this.tOld);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChPwd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Смена пароля";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tOld;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tNew1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tNew2;
        private System.Windows.Forms.Label label3;
    }
}