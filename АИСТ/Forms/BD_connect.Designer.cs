namespace АИСТ.Forms
{
    partial class BD_connect
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
            this.tb_server = new System.Windows.Forms.TextBox();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.tb_login = new System.Windows.Forms.TextBox();
            this.tb_password = new System.Windows.Forms.MaskedTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_server
            // 
            this.tb_server.Location = new System.Drawing.Point(12, 10);
            this.tb_server.Name = "tb_server";
            this.tb_server.Size = new System.Drawing.Size(185, 20);
            this.tb_server.TabIndex = 0;
            this.tb_server.TabStop = false;
            this.tb_server.Text = "localhost";
            this.tb_server.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_server_MouseClick);
            this.tb_server.TextChanged += new System.EventHandler(this.tb_server_TextChanged);
            this.tb_server.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_server_KeyDown);
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(12, 36);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(185, 20);
            this.tb_name.TabIndex = 1;
            this.tb_name.TabStop = false;
            this.tb_name.Text = "bd_name";
            this.tb_name.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_name_MouseClick);
            this.tb_name.TextChanged += new System.EventHandler(this.tb_name_TextChanged);
            this.tb_name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_name_KeyDown);
            // 
            // tb_login
            // 
            this.tb_login.Location = new System.Drawing.Point(12, 63);
            this.tb_login.Name = "tb_login";
            this.tb_login.Size = new System.Drawing.Size(185, 20);
            this.tb_login.TabIndex = 2;
            this.tb_login.Text = "login";
            this.tb_login.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_login_MouseClick);
            this.tb_login.TextChanged += new System.EventHandler(this.tb_login_TextChanged);
            this.tb_login.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_login_KeyDown);
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(12, 90);
            this.tb_password.Name = "tb_password";
            this.tb_password.PasswordChar = '*';
            this.tb_password.Size = new System.Drawing.Size(185, 20);
            this.tb_password.TabIndex = 3;
            this.tb_password.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.tb_password_MaskInputRejected);
            this.tb_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_password_KeyDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 122);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Подключить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BD_connect
            // 
            this.ClientSize = new System.Drawing.Size(206, 157);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.tb_login);
            this.Controls.Add(this.tb_name);
            this.Controls.Add(this.tb_server);
            this.Name = "BD_connect";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BD_connect_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_server;
        private System.Windows.Forms.TextBox tb_name;
        private System.Windows.Forms.TextBox tb_login;
        private System.Windows.Forms.Button button1;
        protected System.Windows.Forms.MaskedTextBox tb_password;
    }
}