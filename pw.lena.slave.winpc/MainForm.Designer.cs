namespace pw.lena.slave.winpc
{
    partial class MainForm
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
            this.label_count = new System.Windows.Forms.Label();
            this.button_codeA = new System.Windows.Forms.Button();
            this.textBox_codeA = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button_ListMasters = new System.Windows.Forms.Button();
            this.listBox_masters = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox_screenshots = new System.Windows.Forms.ListBox();
            this.button_synch_powertime = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker_to = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_from = new System.Windows.Forms.DateTimePicker();
            this.button_refreshPT = new System.Windows.Forms.Button();
            this.listBox_powertime = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label_synch = new System.Windows.Forms.Label();
            this.label_synch_result = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label_powertimetimer = new System.Windows.Forms.Label();
            this.label_savepowertime_result = new System.Windows.Forms.Label();
            this.button_hide = new System.Windows.Forms.Button();
            this.button_screenshot = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label_command = new System.Windows.Forms.Label();
            this.label_command_result = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_count
            // 
            this.label_count.AutoSize = true;
            this.label_count.Location = new System.Drawing.Point(6, 42);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(13, 13);
            this.label_count.TabIndex = 5;
            this.label_count.Text = "0";
            // 
            // button_codeA
            // 
            this.button_codeA.Location = new System.Drawing.Point(127, 16);
            this.button_codeA.Name = "button_codeA";
            this.button_codeA.Size = new System.Drawing.Size(124, 23);
            this.button_codeA.TabIndex = 4;
            this.button_codeA.Text = "get code A";
            this.button_codeA.UseVisualStyleBackColor = true;
            this.button_codeA.Click += new System.EventHandler(this.button_codeA_Click);
            // 
            // textBox_codeA
            // 
            this.textBox_codeA.Location = new System.Drawing.Point(7, 19);
            this.textBox_codeA.Name = "textBox_codeA";
            this.textBox_codeA.Size = new System.Drawing.Size(100, 20);
            this.textBox_codeA.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(801, 138);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "test 2";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_ListMasters
            // 
            this.button_ListMasters.Location = new System.Drawing.Point(10, 19);
            this.button_ListMasters.Name = "button_ListMasters";
            this.button_ListMasters.Size = new System.Drawing.Size(124, 23);
            this.button_ListMasters.TabIndex = 7;
            this.button_ListMasters.Text = "List masters";
            this.button_ListMasters.UseVisualStyleBackColor = true;
            this.button_ListMasters.Click += new System.EventHandler(this.button_ListMasters_Click);
            // 
            // listBox_masters
            // 
            this.listBox_masters.FormattingEnabled = true;
            this.listBox_masters.Location = new System.Drawing.Point(140, 19);
            this.listBox_masters.Name = "listBox_masters";
            this.listBox_masters.Size = new System.Drawing.Size(467, 56);
            this.listBox_masters.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox_masters);
            this.groupBox1.Controls.Add(this.button_ListMasters);
            this.groupBox1.Location = new System.Drawing.Point(12, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(617, 84);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "list masters";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_codeA);
            this.groupBox2.Controls.Add(this.button_codeA);
            this.groupBox2.Controls.Add(this.label_count);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 59);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pair new master";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBox_screenshots);
            this.groupBox3.Controls.Add(this.button_synch_powertime);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.dateTimePicker_to);
            this.groupBox3.Controls.Add(this.dateTimePicker_from);
            this.groupBox3.Controls.Add(this.button_refreshPT);
            this.groupBox3.Controls.Add(this.listBox_powertime);
            this.groupBox3.Location = new System.Drawing.Point(12, 173);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(793, 241);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Command";
            // 
            // listBox_screenshots
            // 
            this.listBox_screenshots.FormattingEnabled = true;
            this.listBox_screenshots.Location = new System.Drawing.Point(9, 137);
            this.listBox_screenshots.Name = "listBox_screenshots";
            this.listBox_screenshots.Size = new System.Drawing.Size(777, 82);
            this.listBox_screenshots.TabIndex = 7;
            // 
            // button_synch_powertime
            // 
            this.button_synch_powertime.Location = new System.Drawing.Point(589, 24);
            this.button_synch_powertime.Name = "button_synch_powertime";
            this.button_synch_powertime.Size = new System.Drawing.Size(94, 23);
            this.button_synch_powertime.TabIndex = 6;
            this.button_synch_powertime.Text = "Synch power";
            this.button_synch_powertime.UseVisualStyleBackColor = true;
            this.button_synch_powertime.Click += new System.EventHandler(this.button_synch_powertime_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "to";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "from";
            // 
            // dateTimePicker_to
            // 
            this.dateTimePicker_to.Location = new System.Drawing.Point(268, 24);
            this.dateTimePicker_to.Name = "dateTimePicker_to";
            this.dateTimePicker_to.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker_to.TabIndex = 3;
            this.dateTimePicker_to.ValueChanged += new System.EventHandler(this.dateTimePicker_to_ValueChanged);
            // 
            // dateTimePicker_from
            // 
            this.dateTimePicker_from.Location = new System.Drawing.Point(40, 24);
            this.dateTimePicker_from.Name = "dateTimePicker_from";
            this.dateTimePicker_from.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker_from.TabIndex = 2;
            this.dateTimePicker_from.ValueChanged += new System.EventHandler(this.dateTimePicker_from_ValueChanged);
            // 
            // button_refreshPT
            // 
            this.button_refreshPT.Location = new System.Drawing.Point(474, 24);
            this.button_refreshPT.Name = "button_refreshPT";
            this.button_refreshPT.Size = new System.Drawing.Size(109, 23);
            this.button_refreshPT.TabIndex = 1;
            this.button_refreshPT.Text = "Power Today Add";
            this.button_refreshPT.UseVisualStyleBackColor = true;
            this.button_refreshPT.Click += new System.EventHandler(this.button_refreshPT_Click);
            // 
            // listBox_powertime
            // 
            this.listBox_powertime.FormattingEnabled = true;
            this.listBox_powertime.Location = new System.Drawing.Point(9, 49);
            this.listBox_powertime.Name = "listBox_powertime";
            this.listBox_powertime.Size = new System.Drawing.Size(777, 82);
            this.listBox_powertime.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(801, 47);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "test 1";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox8);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Location = new System.Drawing.Point(290, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(505, 55);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "timers";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label_synch);
            this.groupBox6.Controls.Add(this.label_synch_result);
            this.groupBox6.Location = new System.Drawing.Point(166, 15);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(163, 32);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "synch powertime rest";
            // 
            // label_synch
            // 
            this.label_synch.AutoSize = true;
            this.label_synch.Location = new System.Drawing.Point(6, 16);
            this.label_synch.Name = "label_synch";
            this.label_synch.Size = new System.Drawing.Size(13, 13);
            this.label_synch.TabIndex = 0;
            this.label_synch.Text = "0";
            // 
            // label_synch_result
            // 
            this.label_synch_result.AutoSize = true;
            this.label_synch_result.Location = new System.Drawing.Point(78, 16);
            this.label_synch_result.Name = "label_synch_result";
            this.label_synch_result.Size = new System.Drawing.Size(10, 13);
            this.label_synch_result.TabIndex = 1;
            this.label_synch_result.Text = "-";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label_powertimetimer);
            this.groupBox5.Controls.Add(this.label_savepowertime_result);
            this.groupBox5.Location = new System.Drawing.Point(6, 15);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(154, 32);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "update powertime local";
            // 
            // label_powertimetimer
            // 
            this.label_powertimetimer.AutoSize = true;
            this.label_powertimetimer.Location = new System.Drawing.Point(6, 16);
            this.label_powertimetimer.Name = "label_powertimetimer";
            this.label_powertimetimer.Size = new System.Drawing.Size(13, 13);
            this.label_powertimetimer.TabIndex = 0;
            this.label_powertimetimer.Text = "0";
            // 
            // label_savepowertime_result
            // 
            this.label_savepowertime_result.AutoSize = true;
            this.label_savepowertime_result.Location = new System.Drawing.Point(67, 16);
            this.label_savepowertime_result.Name = "label_savepowertime_result";
            this.label_savepowertime_result.Size = new System.Drawing.Size(10, 13);
            this.label_savepowertime_result.TabIndex = 1;
            this.label_savepowertime_result.Text = "-";
            // 
            // button_hide
            // 
            this.button_hide.Location = new System.Drawing.Point(801, 18);
            this.button_hide.Name = "button_hide";
            this.button_hide.Size = new System.Drawing.Size(75, 23);
            this.button_hide.TabIndex = 14;
            this.button_hide.Text = "Hide";
            this.button_hide.UseVisualStyleBackColor = true;
            this.button_hide.Click += new System.EventHandler(this.button_hide_Click);
            // 
            // button_screenshot
            // 
            this.button_screenshot.Location = new System.Drawing.Point(10, 19);
            this.button_screenshot.Name = "button_screenshot";
            this.button_screenshot.Size = new System.Drawing.Size(75, 23);
            this.button_screenshot.TabIndex = 4;
            this.button_screenshot.Text = "Screenshot";
            this.button_screenshot.UseVisualStyleBackColor = true;
            this.button_screenshot.Click += new System.EventHandler(this.button_screenshot_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox7.Controls.Add(this.button_screenshot);
            this.groupBox7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox7.Location = new System.Drawing.Point(882, 18);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(91, 143);
            this.groupBox7.TabIndex = 15;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Command";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label_command);
            this.groupBox8.Controls.Add(this.label_command_result);
            this.groupBox8.Location = new System.Drawing.Point(336, 17);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(163, 32);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "synch command";
            // 
            // label_command
            // 
            this.label_command.AutoSize = true;
            this.label_command.Location = new System.Drawing.Point(6, 16);
            this.label_command.Name = "label_command";
            this.label_command.Size = new System.Drawing.Size(13, 13);
            this.label_command.TabIndex = 0;
            this.label_command.Text = "0";
            // 
            // label_command_result
            // 
            this.label_command_result.AutoSize = true;
            this.label_command_result.Location = new System.Drawing.Point(78, 16);
            this.label_command_result.Name = "label_command_result";
            this.label_command_result.Size = new System.Drawing.Size(10, 13);
            this.label_command_result.TabIndex = 1;
            this.label_command_result.Text = "-";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1093, 473);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.button_hide);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PC slave";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_count;
        private System.Windows.Forms.Button button_codeA;
        private System.Windows.Forms.TextBox textBox_codeA;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_ListMasters;
        private System.Windows.Forms.ListBox listBox_masters;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_refreshPT;
        private System.Windows.Forms.ListBox listBox_powertime;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DateTimePicker dateTimePicker_to;
        private System.Windows.Forms.DateTimePicker dateTimePicker_from;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label_powertimetimer;
        private System.Windows.Forms.Label label_savepowertime_result;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button_synch_powertime;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label_synch;
        private System.Windows.Forms.Label label_synch_result;
        private System.Windows.Forms.Button button_hide;
        private System.Windows.Forms.Button button_screenshot;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ListBox listBox_screenshots;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label_command;
        private System.Windows.Forms.Label label_command_result;
    }
}

