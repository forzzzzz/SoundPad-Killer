namespace SaundpadKiller
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.button_close = new System.Windows.Forms.Button();
            this.text_add_sound = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_close_sound = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // button_close
            // 
            this.button_close.FlatAppearance.BorderSize = 0;
            this.button_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_close.ForeColor = System.Drawing.Color.White;
            this.button_close.Location = new System.Drawing.Point(408, -3);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(64, 35);
            this.button_close.TabIndex = 3;
            this.button_close.Text = "×";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            this.button_close.MouseEnter += new System.EventHandler(this.button_close_MouseEnter);
            this.button_close.MouseLeave += new System.EventHandler(this.button_close_MouseLeave);
            // 
            // text_add_sound
            // 
            this.text_add_sound.AutoSize = true;
            this.text_add_sound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.text_add_sound.ForeColor = System.Drawing.Color.Gainsboro;
            this.text_add_sound.Location = new System.Drawing.Point(113, 9);
            this.text_add_sound.Name = "text_add_sound";
            this.text_add_sound.Size = new System.Drawing.Size(211, 18);
            this.text_add_sound.TabIndex = 4;
            this.text_add_sound.Text = "Налаштування мікрофону";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox1);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox2);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox3);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(28, 53);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(428, 577);
            this.flowLayoutPanel1.TabIndex = 5;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.LightCoral;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 50, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.label1.Size = new System.Drawing.Size(369, 74);
            this.label1.TabIndex = 5;
            this.label1.Text = "Для того, щоб інші люди могли чути ваш голос без затримок, будь ласка, встановіть" +
    " наступні налаштування вручну:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Gainsboro;
            this.label2.Location = new System.Drawing.Point(3, 74);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 40, 0, 0);
            this.label2.Size = new System.Drawing.Size(367, 76);
            this.label2.TabIndex = 6;
            this.label2.Text = "1. У щойно відкритому вікні знайдіть вами обраний мікрофон:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SaundpadKiller.Properties.Resources._11;
            this.pictureBox1.Location = new System.Drawing.Point(10, 153);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.pictureBox1.Size = new System.Drawing.Size(381, 410);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Gainsboro;
            this.label3.Location = new System.Drawing.Point(3, 566);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 20, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label3.Size = new System.Drawing.Size(404, 46);
            this.label3.TabIndex = 8;
            this.label3.Text = "2. Клацніть на мікрофон, а потім знизу ліворуч натисніть на \"Властивості\":";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SaundpadKiller.Properties.Resources._2;
            this.pictureBox2.Location = new System.Drawing.Point(45, 632);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(45, 20, 3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(322, 417);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.Gainsboro;
            this.label4.Location = new System.Drawing.Point(3, 1052);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 0, 30, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label4.Size = new System.Drawing.Size(356, 100);
            this.label4.TabIndex = 10;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::SaundpadKiller.Properties.Resources._3;
            this.pictureBox3.Location = new System.Drawing.Point(45, 1172);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(45, 20, 3, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(322, 417);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 11;
            this.pictureBox3.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.Color.DarkOrange;
            this.label5.Location = new System.Drawing.Point(3, 1592);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 40, 0);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.label5.Size = new System.Drawing.Size(385, 74);
            this.label5.TabIndex = 12;
            this.label5.Text = "Якщо прапорець вже встановлений або ви виконуєте цю дію повторно, спочатку зніміт" +
    "ь прапорець зі старого пристрою!";
            // 
            // button_close_sound
            // 
            this.button_close_sound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.button_close_sound.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button_close_sound.FlatAppearance.BorderSize = 0;
            this.button_close_sound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_close_sound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_close_sound.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button_close_sound.Location = new System.Drawing.Point(345, 649);
            this.button_close_sound.Name = "button_close_sound";
            this.button_close_sound.Size = new System.Drawing.Size(104, 34);
            this.button_close_sound.TabIndex = 6;
            this.button_close_sound.Text = "Закрити";
            this.button_close_sound.UseVisualStyleBackColor = false;
            this.button_close_sound.Click += new System.EventHandler(this.button_close_sound_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox1.ForeColor = System.Drawing.Color.Gainsboro;
            this.checkBox1.Location = new System.Drawing.Point(169, 658);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(170, 20);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Більше не показувати";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(345, -3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 35);
            this.button2.TabIndex = 21;
            this.button2.Text = "—";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(467, 695);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button_close_sound);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.text_add_sound);
            this.Controls.Add(this.button_close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form3";
            this.Text = "Налаштування мікрофону";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Label text_add_sound;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_close_sound;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button2;
    }
}