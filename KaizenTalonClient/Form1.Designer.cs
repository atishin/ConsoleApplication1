namespace KaizenTalonClient
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_main = new System.Windows.Forms.Button();
            this.lbl_info = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_log = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_main
            // 
            this.btn_main.Location = new System.Drawing.Point(12, 12);
            this.btn_main.Name = "btn_main";
            this.btn_main.Size = new System.Drawing.Size(260, 62);
            this.btn_main.TabIndex = 0;
            this.btn_main.Text = "button1";
            this.btn_main.UseVisualStyleBackColor = true;
            this.btn_main.Click += new System.EventHandler(this.btn_main_Click);
            // 
            // lbl_info
            // 
            this.lbl_info.Location = new System.Drawing.Point(12, 88);
            this.lbl_info.Name = "lbl_info";
            this.lbl_info.Size = new System.Drawing.Size(260, 164);
            this.lbl_info.TabIndex = 1;
            this.lbl_info.Text = "label1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_log);
            this.groupBox1.Location = new System.Drawing.Point(278, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 237);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Логи";
            // 
            // tb_log
            // 
            this.tb_log.Enabled = false;
            this.tb_log.Location = new System.Drawing.Point(6, 22);
            this.tb_log.Multiline = true;
            this.tb_log.Name = "tb_log";
            this.tb_log.Size = new System.Drawing.Size(270, 209);
            this.tb_log.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 261);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbl_info);
            this.Controls.Add(this.btn_main);
            this.Name = "Form1";
            this.Text = "Kaizen | Талон";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_main;
        private System.Windows.Forms.Label lbl_info;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_log;
    }
}

