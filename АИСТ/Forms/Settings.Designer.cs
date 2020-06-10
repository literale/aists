namespace АИСТ
{
    partial class Settings
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
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_emai = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.gb_lb = new System.Windows.Forms.GroupBox();
            this.gb_tb = new System.Windows.Forms.GroupBox();
            this.gb_bt = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(303, 375);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(126, 23);
            this.btn_save.TabIndex = 1;
            this.btn_save.Text = "Сохранить";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_emai
            // 
            this.btn_emai.Location = new System.Drawing.Point(19, 375);
            this.btn_emai.Name = "btn_emai";
            this.btn_emai.Size = new System.Drawing.Size(126, 23);
            this.btn_emai.TabIndex = 77;
            this.btn_emai.Text = "email";
            this.btn_emai.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Location = new System.Drawing.Point(151, 375);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(145, 23);
            this.label13.TabIndex = 77;
            this.label13.Text = "shop@mail.com";
            // 
            // gb_lb
            // 
            this.gb_lb.Location = new System.Drawing.Point(19, 12);
            this.gb_lb.Name = "gb_lb";
            this.gb_lb.Size = new System.Drawing.Size(126, 343);
            this.gb_lb.TabIndex = 78;
            this.gb_lb.TabStop = false;
            this.gb_lb.Text = "Таблицы";
            // 
            // gb_tb
            // 
            this.gb_tb.Location = new System.Drawing.Point(151, 12);
            this.gb_tb.Name = "gb_tb";
            this.gb_tb.Size = new System.Drawing.Size(145, 343);
            this.gb_tb.TabIndex = 79;
            this.gb_tb.TabStop = false;
            this.gb_tb.Text = "Реальные имена ";
            // 
            // gb_bt
            // 
            this.gb_bt.Location = new System.Drawing.Point(303, 12);
            this.gb_bt.Name = "gb_bt";
            this.gb_bt.Size = new System.Drawing.Size(126, 343);
            this.gb_bt.TabIndex = 80;
            this.gb_bt.TabStop = false;
            this.gb_bt.Text = "Поля";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 403);
            this.Controls.Add(this.gb_bt);
            this.Controls.Add(this.gb_tb);
            this.Controls.Add(this.gb_lb);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btn_emai);
            this.Controls.Add(this.btn_save);
            this.Name = "Settings";
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Settings_FormClosed);
            this.Load += new System.EventHandler(this.Settings_Load);
            this.Shown += new System.EventHandler(this.Settings_Shown);
            this.Click += new System.EventHandler(this.Settings_Click);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_emai;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox gb_lb;
        private System.Windows.Forms.GroupBox gb_tb;
        private System.Windows.Forms.GroupBox gb_bt;
    }
}