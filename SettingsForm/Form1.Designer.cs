namespace SettingsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.apply_button = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.alarm1 = new System.Windows.Forms.Label();
            this.close_window = new System.Windows.Forms.Label();
            this.background = new System.Windows.Forms.Panel();
            this.minimized = new System.Windows.Forms.Label();
            this.trayicon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.background.SuspendLayout();
            this.SuspendLayout();
            // 
            // apply_button
            // 
            this.apply_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.apply_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.apply_button.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.apply_button.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.apply_button.Location = new System.Drawing.Point(54, 134);
            this.apply_button.Name = "apply_button";
            this.apply_button.Size = new System.Drawing.Size(77, 28);
            this.apply_button.TabIndex = 0;
            this.apply_button.Text = "Apply";
            this.apply_button.UseVisualStyleBackColor = false;
            this.apply_button.Click += new System.EventHandler(this.apply_button_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.numericUpDown1.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.numericUpDown1.Location = new System.Drawing.Point(32, 76);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(38, 20);
            this.numericUpDown1.TabIndex = 1;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.numericUpDown2.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.numericUpDown2.Location = new System.Drawing.Point(76, 76);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(38, 20);
            this.numericUpDown2.TabIndex = 2;
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(85)))), ((int)(((byte)(111)))));
            this.numericUpDown3.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.numericUpDown3.Location = new System.Drawing.Point(120, 76);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(38, 20);
            this.numericUpDown3.TabIndex = 3;
            // 
            // alarm1
            // 
            this.alarm1.AutoSize = true;
            this.alarm1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alarm1.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.alarm1.Location = new System.Drawing.Point(57, 38);
            this.alarm1.Name = "alarm1";
            this.alarm1.Size = new System.Drawing.Size(79, 16);
            this.alarm1.TabIndex = 7;
            this.alarm1.Text = "Set the time";
            // 
            // close_window
            // 
            this.close_window.AutoSize = true;
            this.close_window.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.close_window.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.close_window.Location = new System.Drawing.Point(165, 2);
            this.close_window.Name = "close_window";
            this.close_window.Size = new System.Drawing.Size(15, 17);
            this.close_window.TabIndex = 9;
            this.close_window.Text = "x";
            this.close_window.Click += new System.EventHandler(this.close_window_Click);
            // 
            // background
            // 
            this.background.Controls.Add(this.minimized);
            this.background.Controls.Add(this.numericUpDown1);
            this.background.Controls.Add(this.alarm1);
            this.background.Controls.Add(this.numericUpDown2);
            this.background.Controls.Add(this.numericUpDown3);
            this.background.Location = new System.Drawing.Point(0, 0);
            this.background.Name = "background";
            this.background.Size = new System.Drawing.Size(184, 177);
            this.background.TabIndex = 10;
            this.background.Paint += new System.Windows.Forms.PaintEventHandler(this.background_Paint);
            // 
            // minimized
            // 
            this.minimized.AutoSize = true;
            this.minimized.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.minimized.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.minimized.Location = new System.Drawing.Point(148, 0);
            this.minimized.Name = "minimized";
            this.minimized.Size = new System.Drawing.Size(15, 18);
            this.minimized.TabIndex = 11;
            this.minimized.Text = "_";
            this.minimized.Click += new System.EventHandler(this.minimized_Click);
            // 
            // trayicon
            // 
            this.trayicon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayicon.Icon")));
            this.trayicon.Text = "Alarm";
            this.trayicon.Visible = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(46)))), ((int)(((byte)(69)))));
            this.ClientSize = new System.Drawing.Size(184, 177);
            this.Controls.Add(this.close_window);
            this.Controls.Add(this.apply_button);
            this.Controls.Add(this.background);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Alarm";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.background.ResumeLayout(false);
            this.background.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button apply_button;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label alarm1;
        private System.Windows.Forms.Label close_window;
        private System.Windows.Forms.Panel background;
        private System.Windows.Forms.Label minimized;
        private System.Windows.Forms.NotifyIcon trayicon;
    }
}

