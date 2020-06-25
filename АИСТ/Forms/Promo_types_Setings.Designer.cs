namespace АИСТ.Forms
{
    partial class Promo_types_Setings
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pr_client_ABC = new System.Windows.Forms.ComboBox();
            this.comboBox8 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.pr_client_XYZ = new System.Windows.Forms.ComboBox();
            this.client_XYZ = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.client_ABC = new System.Windows.Forms.ComboBox();
            this.pers_prod = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.disc_size = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cl_prod_ABC = new System.Windows.Forms.ComboBox();
            this.prod_ABC = new System.Windows.Forms.ComboBox();
            this.cl_prod_XYZ = new System.Windows.Forms.ComboBox();
            this.prod_XYZ = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox13 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label31 = new System.Windows.Forms.Label();
            this.prior_cl = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.prior_client = new System.Windows.Forms.ComboBox();
            this.prior_prod = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cl_intresting = new System.Windows.Forms.ComboBox();
            this.prod_intresting = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.импортToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pers_prod)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pr_client_ABC);
            this.groupBox1.Controls.Add(this.comboBox8);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.pr_client_XYZ);
            this.groupBox1.Controls.Add(this.client_XYZ);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.client_ABC);
            this.groupBox1.Controls.Add(this.pers_prod);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.disc_size);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label29);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 104);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Клиент";
            // 
            // pr_client_ABC
            // 
            this.pr_client_ABC.DisplayMember = "0";
            this.pr_client_ABC.FormattingEnabled = true;
            this.pr_client_ABC.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.pr_client_ABC.Location = new System.Drawing.Point(11, 59);
            this.pr_client_ABC.Name = "pr_client_ABC";
            this.pr_client_ABC.Size = new System.Drawing.Size(38, 21);
            this.pr_client_ABC.TabIndex = 33;
            this.pr_client_ABC.Text = "A";
            this.pr_client_ABC.ValueMember = "0";
            this.pr_client_ABC.SelectedIndexChanged += new System.EventHandler(this.pr_client_ABC_SelectedIndexChanged);
            // 
            // comboBox8
            // 
            this.comboBox8.Enabled = false;
            this.comboBox8.FormattingEnabled = true;
            this.comboBox8.Items.AddRange(new object[] {
            "Сет 1"});
            this.comboBox8.Location = new System.Drawing.Point(11, 99);
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Size = new System.Drawing.Size(141, 21);
            this.comboBox8.TabIndex = 32;
            this.comboBox8.Text = "Сет 1";
            this.comboBox8.Visible = false;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(158, 99);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 31;
            this.button2.Text = "Добавить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // pr_client_XYZ
            // 
            this.pr_client_XYZ.DisplayMember = "0";
            this.pr_client_XYZ.FormattingEnabled = true;
            this.pr_client_XYZ.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.pr_client_XYZ.Location = new System.Drawing.Point(54, 59);
            this.pr_client_XYZ.Name = "pr_client_XYZ";
            this.pr_client_XYZ.Size = new System.Drawing.Size(38, 21);
            this.pr_client_XYZ.TabIndex = 24;
            this.pr_client_XYZ.Text = "X";
            this.pr_client_XYZ.ValueMember = "0";
            this.pr_client_XYZ.SelectedIndexChanged += new System.EventHandler(this.pr_client_XYZ_SelectedIndexChanged);
            // 
            // client_XYZ
            // 
            this.client_XYZ.DisplayMember = "0";
            this.client_XYZ.FormattingEnabled = true;
            this.client_XYZ.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.client_XYZ.Location = new System.Drawing.Point(54, 19);
            this.client_XYZ.Name = "client_XYZ";
            this.client_XYZ.Size = new System.Drawing.Size(38, 21);
            this.client_XYZ.TabIndex = 27;
            this.client_XYZ.Text = "X";
            this.client_XYZ.ValueMember = "0";
            this.client_XYZ.SelectedIndexChanged += new System.EventHandler(this.client_XYZ_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 43);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "Тип Клиента";
            // 
            // client_ABC
            // 
            this.client_ABC.DisplayMember = "1";
            this.client_ABC.FormattingEnabled = true;
            this.client_ABC.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.client_ABC.Location = new System.Drawing.Point(10, 19);
            this.client_ABC.Name = "client_ABC";
            this.client_ABC.Size = new System.Drawing.Size(38, 21);
            this.client_ABC.TabIndex = 25;
            this.client_ABC.Text = "А";
            this.client_ABC.ValueMember = "0";
            this.client_ABC.SelectedIndexChanged += new System.EventHandler(this.client_ABC_SelectedIndexChanged);
            // 
            // pers_prod
            // 
            this.pers_prod.Location = new System.Drawing.Point(98, 59);
            this.pers_prod.Name = "pers_prod";
            this.pers_prod.Size = new System.Drawing.Size(54, 20);
            this.pers_prod.TabIndex = 15;
            this.pers_prod.ValueChanged += new System.EventHandler(this.pers_prod_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(155, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Размер скидки";
            // 
            // disc_size
            // 
            this.disc_size.FormattingEnabled = true;
            this.disc_size.Items.AddRange(new object[] {
            "Низкий",
            "Средний",
            "Высокий"});
            this.disc_size.Location = new System.Drawing.Point(158, 59);
            this.disc_size.Name = "disc_size";
            this.disc_size.Size = new System.Drawing.Size(75, 21);
            this.disc_size.TabIndex = 3;
            this.disc_size.Text = "Низкий";
            this.disc_size.SelectedIndexChanged += new System.EventHandler(this.disc_size_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "% в предл";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(7, 83);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(64, 13);
            this.label29.TabIndex = 19;
            this.label29.Text = "Тип товара";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cl_prod_ABC);
            this.groupBox4.Controls.Add(this.prod_ABC);
            this.groupBox4.Controls.Add(this.cl_prod_XYZ);
            this.groupBox4.Controls.Add(this.prod_XYZ);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.comboBox13);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.label31);
            this.groupBox4.Controls.Add(this.prior_cl);
            this.groupBox4.Location = new System.Drawing.Point(282, 30);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(237, 101);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Товар";
            // 
            // cl_prod_ABC
            // 
            this.cl_prod_ABC.DisplayMember = "0";
            this.cl_prod_ABC.FormattingEnabled = true;
            this.cl_prod_ABC.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.cl_prod_ABC.Location = new System.Drawing.Point(7, 56);
            this.cl_prod_ABC.Name = "cl_prod_ABC";
            this.cl_prod_ABC.Size = new System.Drawing.Size(38, 21);
            this.cl_prod_ABC.TabIndex = 32;
            this.cl_prod_ABC.Text = "A";
            this.cl_prod_ABC.ValueMember = "0";
            this.cl_prod_ABC.SelectedIndexChanged += new System.EventHandler(this.cl_prod_ABC_SelectedIndexChanged);
            // 
            // prod_ABC
            // 
            this.prod_ABC.DisplayMember = "0";
            this.prod_ABC.FormattingEnabled = true;
            this.prod_ABC.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.prod_ABC.Location = new System.Drawing.Point(7, 19);
            this.prod_ABC.Name = "prod_ABC";
            this.prod_ABC.Size = new System.Drawing.Size(38, 21);
            this.prod_ABC.TabIndex = 31;
            this.prod_ABC.Text = "A";
            this.prod_ABC.ValueMember = "0";
            this.prod_ABC.SelectedIndexChanged += new System.EventHandler(this.prod_ABC_SelectedIndexChanged);
            // 
            // cl_prod_XYZ
            // 
            this.cl_prod_XYZ.DisplayMember = "0";
            this.cl_prod_XYZ.FormattingEnabled = true;
            this.cl_prod_XYZ.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.cl_prod_XYZ.Location = new System.Drawing.Point(51, 56);
            this.cl_prod_XYZ.Name = "cl_prod_XYZ";
            this.cl_prod_XYZ.Size = new System.Drawing.Size(38, 21);
            this.cl_prod_XYZ.TabIndex = 30;
            this.cl_prod_XYZ.Text = "X";
            this.cl_prod_XYZ.ValueMember = "0";
            this.cl_prod_XYZ.SelectedIndexChanged += new System.EventHandler(this.cl_prod_XYZ_SelectedIndexChanged);
            // 
            // prod_XYZ
            // 
            this.prod_XYZ.DisplayMember = "0";
            this.prod_XYZ.FormattingEnabled = true;
            this.prod_XYZ.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z"});
            this.prod_XYZ.Location = new System.Drawing.Point(51, 19);
            this.prod_XYZ.Name = "prod_XYZ";
            this.prod_XYZ.Size = new System.Drawing.Size(38, 21);
            this.prod_XYZ.TabIndex = 30;
            this.prod_XYZ.Text = "X";
            this.prod_XYZ.ValueMember = "0";
            this.prod_XYZ.SelectedIndexChanged += new System.EventHandler(this.prod_XYZ_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Тип Клиента";
            // 
            // comboBox13
            // 
            this.comboBox13.Enabled = false;
            this.comboBox13.FormattingEnabled = true;
            this.comboBox13.Items.AddRange(new object[] {
            "Сет 1"});
            this.comboBox13.Location = new System.Drawing.Point(6, 98);
            this.comboBox13.Name = "comboBox13";
            this.comboBox13.Size = new System.Drawing.Size(106, 21);
            this.comboBox13.TabIndex = 23;
            this.comboBox13.Text = "Сет 1";
            this.comboBox13.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Тип товара";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(118, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "Добавить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(118, 79);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(61, 13);
            this.label31.TabIndex = 19;
            this.label31.Text = "Приоритет";
            // 
            // prior_cl
            // 
            this.prior_cl.FormattingEnabled = true;
            this.prior_cl.Items.AddRange(new object[] {
            "Низкий",
            "Средний",
            "Высокий",
            "Запрещен"});
            this.prior_cl.Location = new System.Drawing.Point(118, 55);
            this.prior_cl.Name = "prior_cl";
            this.prior_cl.Size = new System.Drawing.Size(82, 21);
            this.prior_cl.TabIndex = 18;
            this.prior_cl.Text = "Низкий";
            this.prior_cl.SelectedIndexChanged += new System.EventHandler(this.prior_cl_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Приоритет для клиента";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Приоритет для товара";
            // 
            // prior_client
            // 
            this.prior_client.FormattingEnabled = true;
            this.prior_client.Items.AddRange(new object[] {
            "Объем закупок",
            "Затраченная сумма"});
            this.prior_client.Location = new System.Drawing.Point(145, 137);
            this.prior_client.Name = "prior_client";
            this.prior_client.Size = new System.Drawing.Size(171, 21);
            this.prior_client.TabIndex = 31;
            this.prior_client.Text = "Объем закупок";
            this.prior_client.SelectedIndexChanged += new System.EventHandler(this.prior_client_SelectedIndexChanged);
            // 
            // prior_prod
            // 
            this.prior_prod.FormattingEnabled = true;
            this.prior_prod.Items.AddRange(new object[] {
            "Популярность",
            "Доходность",
            "Остаток на складе"});
            this.prior_prod.Location = new System.Drawing.Point(145, 164);
            this.prior_prod.Name = "prior_prod";
            this.prior_prod.Size = new System.Drawing.Size(171, 21);
            this.prior_prod.TabIndex = 32;
            this.prior_prod.Text = "Популярность";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(323, 140);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Порядок";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(324, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Порядок";
            // 
            // cl_intresting
            // 
            this.cl_intresting.FormattingEnabled = true;
            this.cl_intresting.Items.AddRange(new object[] {
            "Сначала интересные",
            "Сначала не интересные"});
            this.cl_intresting.Location = new System.Drawing.Point(380, 137);
            this.cl_intresting.Name = "cl_intresting";
            this.cl_intresting.Size = new System.Drawing.Size(143, 21);
            this.cl_intresting.TabIndex = 35;
            this.cl_intresting.Text = "Сначала интересные";
            // 
            // prod_intresting
            // 
            this.prod_intresting.FormattingEnabled = true;
            this.prod_intresting.Items.AddRange(new object[] {
            "Сначала интересные",
            "Сначала не интересные"});
            this.prod_intresting.Location = new System.Drawing.Point(380, 164);
            this.prod_intresting.Name = "prod_intresting";
            this.prod_intresting.Size = new System.Drawing.Size(143, 21);
            this.prod_intresting.TabIndex = 36;
            this.prod_intresting.Text = "Сначала не интересные";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьToolStripMenuItem,
            this.импортToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(532, 24);
            this.menuStrip1.TabIndex = 37;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // импортToolStripMenuItem
            // 
            this.импортToolStripMenuItem.Name = "импортToolStripMenuItem";
            this.импортToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.импортToolStripMenuItem.Text = "Импорт";
            this.импортToolStripMenuItem.Click += new System.EventHandler(this.импортToolStripMenuItem_Click_1);
            // 
            // Promo_types_Setings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 193);
            this.Controls.Add(this.prod_intresting);
            this.Controls.Add(this.cl_intresting);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.prior_prod);
            this.Controls.Add(this.prior_client);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Promo_types_Setings";
            this.Text = "Promo_types_Setings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Promo_types_Setings_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Promo_types_Setings_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pers_prod)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox disc_size;
        private System.Windows.Forms.NumericUpDown pers_prod;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.ComboBox prior_cl;
        public System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.ComboBox comboBox13;
        public System.Windows.Forms.ComboBox pr_client_XYZ;
        public System.Windows.Forms.ComboBox client_XYZ;
        public System.Windows.Forms.Label label10;
        public System.Windows.Forms.ComboBox client_ABC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cl_prod_XYZ;
        public System.Windows.Forms.ComboBox prod_XYZ;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox comboBox8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox prior_client;
        private System.Windows.Forms.ComboBox prior_prod;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cl_intresting;
        private System.Windows.Forms.ComboBox prod_intresting;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem импортToolStripMenuItem;
        public System.Windows.Forms.ComboBox pr_client_ABC;
        public System.Windows.Forms.ComboBox cl_prod_ABC;
        public System.Windows.Forms.ComboBox prod_ABC;
    }
}