namespace MLDBUtils
{
    partial class AParamAdd
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.vGridControl1 = new DevExpress.XtraVerticalGrid.VGridControl();
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(320, 241);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // vGridControl1
            // 
            this.vGridControl1.Location = new System.Drawing.Point(0, 3);
            this.vGridControl1.Name = "vGridControl1";
            this.vGridControl1.OptionsView.AutoScaleBands = true;
            this.vGridControl1.OptionsView.ScaleRowHeaderPanel = true;
            this.vGridControl1.RowHeaderWidth = 260;
            this.vGridControl1.Size = new System.Drawing.Size(424, 232);
            this.vGridControl1.TabIndex = 3;
            // 
            // AParamAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vGridControl1);
            this.Controls.Add(this.button1);
            this.Name = "AParamAdd";
            this.Size = new System.Drawing.Size(427, 267);
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private DevExpress.XtraVerticalGrid.VGridControl vGridControl1;
    }
}
