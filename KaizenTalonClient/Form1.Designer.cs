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
            this.SuspendLayout();
            // 
            // btn_main
            // 
            this.btn_main.ForeColor = System.Drawing.Color.Green;
            this.btn_main.Location = new System.Drawing.Point(12, 12);
            this.btn_main.Name = "btn_main";
            this.btn_main.Size = new System.Drawing.Size(260, 62);
            this.btn_main.TabIndex = 0;
            this.btn_main.Text = "Запустить";
            this.btn_main.UseVisualStyleBackColor = true;
            this.btn_main.Click += new System.EventHandler(this.btn_main_Click);
            // 
            // lbl_info
            // 
            this.lbl_info.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_info.Location = new System.Drawing.Point(12, 88);
            this.lbl_info.Name = "lbl_info";
            this.lbl_info.Size = new System.Drawing.Size(260, 164);
            this.lbl_info.TabIndex = 1;
            this.lbl_info.Text = "Остановлен";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 261);
            this.Controls.Add(this.lbl_info);
            this.Controls.Add(this.btn_main);
            this.Name = "Form1";
            this.Text = "Kaizen | Талон";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_main;
        private System.Windows.Forms.Label lbl_info;
    }
}

