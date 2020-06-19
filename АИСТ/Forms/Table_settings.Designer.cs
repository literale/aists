namespace АИСТ.Properties
{
    partial class Table_settings
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
            this.save = new System.Windows.Forms.Button();
            this.gb1 = new System.Windows.Forms.GroupBox();
            this.gb2 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(12, 176);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(126, 23);
            this.save.TabIndex = 43;
            this.save.Text = "Сохранить";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // gb1
            // 
            this.gb1.Location = new System.Drawing.Point(13, 13);
            this.gb1.Name = "gb1";
            this.gb1.Size = new System.Drawing.Size(130, 100);
            this.gb1.TabIndex = 44;
            this.gb1.TabStop = false;
            this.gb1.Text = "Поле";
            // 
            // gb2
            // 
            this.gb2.Location = new System.Drawing.Point(150, 13);
            this.gb2.Name = "gb2";
            this.gb2.Size = new System.Drawing.Size(130, 100);
            this.gb2.TabIndex = 45;
            this.gb2.TabStop = false;
            this.gb2.Text = "Реальное имя";
            // 
            // Table_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 211);
            this.Controls.Add(this.gb2);
            this.Controls.Add(this.gb1);
            this.Controls.Add(this.save);
            this.Name = "Table_settings";
            this.Text = "Table_settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Table_settings_FormClosing);
            this.Load += new System.EventHandler(this.Table_settings_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.GroupBox gb1;
        private System.Windows.Forms.GroupBox gb2;
    }
}