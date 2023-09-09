namespace SaundpadKiller
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.button_add_sound = new System.Windows.Forms.Button();
            this.button_close_sound = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.text_add_sound = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.name_sound = new System.Windows.Forms.TextBox();
            this.trackbar_sound_volume = new MetroSet_UI.Controls.MetroSetTrackBar();
            this.text_sound_volume = new System.Windows.Forms.TextBox();
            this.text_sound = new System.Windows.Forms.Label();
            this.textbox_keys = new System.Windows.Forms.TextBox();
            this.text_keys = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.delete_keys = new System.Windows.Forms.Button();
            this.listen_sound = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_add_sound
            // 
            this.button_add_sound.BackColor = System.Drawing.Color.Gray;
            this.button_add_sound.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.button_add_sound.FlatAppearance.BorderSize = 0;
            this.button_add_sound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_add_sound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_add_sound.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button_add_sound.Location = new System.Drawing.Point(413, 322);
            this.button_add_sound.Name = "button_add_sound";
            this.button_add_sound.Size = new System.Drawing.Size(120, 34);
            this.button_add_sound.TabIndex = 0;
            this.button_add_sound.Text = "Додати";
            this.button_add_sound.UseVisualStyleBackColor = false;
            this.button_add_sound.Click += new System.EventHandler(this.button_add_sound_Click);
            // 
            // button_close_sound
            // 
            this.button_close_sound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.button_close_sound.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button_close_sound.FlatAppearance.BorderSize = 0;
            this.button_close_sound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_close_sound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_close_sound.ForeColor = System.Drawing.Color.Gainsboro;
            this.button_close_sound.Location = new System.Drawing.Point(294, 322);
            this.button_close_sound.Name = "button_close_sound";
            this.button_close_sound.Size = new System.Drawing.Size(104, 34);
            this.button_close_sound.TabIndex = 1;
            this.button_close_sound.Text = "Відмінити";
            this.button_close_sound.UseVisualStyleBackColor = false;
            this.button_close_sound.Click += new System.EventHandler(this.button_close_sound_Click);
            // 
            // button_close
            // 
            this.button_close.FlatAppearance.BorderSize = 0;
            this.button_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_close.ForeColor = System.Drawing.Color.White;
            this.button_close.Location = new System.Drawing.Point(488, -2);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(64, 35);
            this.button_close.TabIndex = 2;
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
            this.text_add_sound.Location = new System.Drawing.Point(201, 11);
            this.text_add_sound.Name = "text_add_sound";
            this.text_add_sound.Size = new System.Drawing.Size(144, 18);
            this.text_add_sound.TabIndex = 3;
            this.text_add_sound.Text = "Додовання звуку";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.Color.Gainsboro;
            this.button1.Location = new System.Drawing.Point(15, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(242, 89);
            this.button1.TabIndex = 4;
            this.button1.Text = "Обрати звук";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Gainsboro;
            this.label2.Location = new System.Drawing.Point(277, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Назва звуку";
            // 
            // name_sound
            // 
            this.name_sound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.name_sound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.name_sound.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.name_sound.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.name_sound.Location = new System.Drawing.Point(281, 110);
            this.name_sound.Name = "name_sound";
            this.name_sound.Size = new System.Drawing.Size(254, 29);
            this.name_sound.TabIndex = 7;
            // 
            // trackbar_sound_volume
            // 
            this.trackbar_sound_volume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.trackbar_sound_volume.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.trackbar_sound_volume.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackbar_sound_volume.DisabledBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.trackbar_sound_volume.DisabledBorderColor = System.Drawing.Color.Empty;
            this.trackbar_sound_volume.DisabledHandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.trackbar_sound_volume.DisabledValueColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.trackbar_sound_volume.HandlerColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.trackbar_sound_volume.IsDerivedStyle = true;
            this.trackbar_sound_volume.Location = new System.Drawing.Point(304, 210);
            this.trackbar_sound_volume.Maximum = 500;
            this.trackbar_sound_volume.Minimum = 0;
            this.trackbar_sound_volume.Name = "trackbar_sound_volume";
            this.trackbar_sound_volume.Size = new System.Drawing.Size(223, 16);
            this.trackbar_sound_volume.Style = MetroSet_UI.Enums.Style.Light;
            this.trackbar_sound_volume.StyleManager = null;
            this.trackbar_sound_volume.TabIndex = 8;
            this.trackbar_sound_volume.Text = "metroSetTrackBar1";
            this.trackbar_sound_volume.ThemeAuthor = "Narwin";
            this.trackbar_sound_volume.ThemeName = "MetroLite";
            this.trackbar_sound_volume.Value = 0;
            this.trackbar_sound_volume.ValueColor = System.Drawing.Color.LightSkyBlue;
            this.trackbar_sound_volume.Scroll += new MetroSet_UI.Controls.MetroSetTrackBar.ScrollEventHandler(this.trackbar_sound_volume_Scroll);
            // 
            // text_sound_volume
            // 
            this.text_sound_volume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.text_sound_volume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.text_sound_volume.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.text_sound_volume.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.text_sound_volume.ForeColor = System.Drawing.Color.Gainsboro;
            this.text_sound_volume.Location = new System.Drawing.Point(451, 173);
            this.text_sound_volume.MaxLength = 3;
            this.text_sound_volume.Name = "text_sound_volume";
            this.text_sound_volume.Size = new System.Drawing.Size(40, 22);
            this.text_sound_volume.TabIndex = 13;
            this.text_sound_volume.Text = "100";
            this.text_sound_volume.TextChanged += new System.EventHandler(this.text_sound_volume_TextChanged);
            // 
            // text_sound
            // 
            this.text_sound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.text_sound.AutoSize = true;
            this.text_sound.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.text_sound.ForeColor = System.Drawing.Color.Gainsboro;
            this.text_sound.Location = new System.Drawing.Point(300, 173);
            this.text_sound.Name = "text_sound";
            this.text_sound.Size = new System.Drawing.Size(115, 20);
            this.text_sound.TabIndex = 14;
            this.text_sound.Text = "Гучність звуку";
            // 
            // textbox_keys
            // 
            this.textbox_keys.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.textbox_keys.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textbox_keys.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textbox_keys.ForeColor = System.Drawing.Color.Gainsboro;
            this.textbox_keys.Location = new System.Drawing.Point(16, 206);
            this.textbox_keys.Name = "textbox_keys";
            this.textbox_keys.ReadOnly = true;
            this.textbox_keys.Size = new System.Drawing.Size(254, 29);
            this.textbox_keys.TabIndex = 15;
            // 
            // text_keys
            // 
            this.text_keys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.text_keys.AutoSize = true;
            this.text_keys.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.text_keys.ForeColor = System.Drawing.Color.Gainsboro;
            this.text_keys.Location = new System.Drawing.Point(12, 173);
            this.text_keys.Name = "text_keys";
            this.text_keys.Size = new System.Drawing.Size(99, 20);
            this.text_keys.TabIndex = 16;
            this.text_keys.Text = "Бінд клавіш";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(13, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 26);
            this.label1.TabIndex = 17;
            this.label1.Text = "Натисніть на поле та після чого, на клавіатурі,\nнатисніть потрібні клавіші";
            // 
            // delete_keys
            // 
            this.delete_keys.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.delete_keys.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.delete_keys.FlatAppearance.BorderSize = 0;
            this.delete_keys.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delete_keys.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.delete_keys.ForeColor = System.Drawing.Color.Gainsboro;
            this.delete_keys.Location = new System.Drawing.Point(16, 276);
            this.delete_keys.Name = "delete_keys";
            this.delete_keys.Size = new System.Drawing.Size(129, 34);
            this.delete_keys.TabIndex = 18;
            this.delete_keys.Text = "Видалити бінд";
            this.delete_keys.UseVisualStyleBackColor = false;
            this.delete_keys.Click += new System.EventHandler(this.delete_keys_Click);
            // 
            // listen_sound
            // 
            this.listen_sound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.listen_sound.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.listen_sound.FlatAppearance.BorderSize = 0;
            this.listen_sound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.listen_sound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listen_sound.ForeColor = System.Drawing.Color.Gainsboro;
            this.listen_sound.Location = new System.Drawing.Point(353, 250);
            this.listen_sound.Name = "listen_sound";
            this.listen_sound.Size = new System.Drawing.Size(129, 34);
            this.listen_sound.TabIndex = 19;
            this.listen_sound.Text = "Прослухати";
            this.listen_sound.UseVisualStyleBackColor = false;
            this.listen_sound.Click += new System.EventHandler(this.listen_sound_Click);
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(427, -2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 35);
            this.button2.TabIndex = 20;
            this.button2.Text = "—";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(547, 368);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listen_sound);
            this.Controls.Add(this.delete_keys);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.text_keys);
            this.Controls.Add(this.textbox_keys);
            this.Controls.Add(this.text_sound);
            this.Controls.Add(this.text_sound_volume);
            this.Controls.Add(this.trackbar_sound_volume);
            this.Controls.Add(this.name_sound);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.text_add_sound);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_close_sound);
            this.Controls.Add(this.button_add_sound);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(547, 368);
            this.MinimumSize = new System.Drawing.Size(547, 368);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_add_sound;
        private System.Windows.Forms.Button button_close_sound;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Label text_add_sound;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox name_sound;
        private MetroSet_UI.Controls.MetroSetTrackBar trackbar_sound_volume;
        private System.Windows.Forms.TextBox text_sound_volume;
        private System.Windows.Forms.Label text_sound;
        private System.Windows.Forms.TextBox textbox_keys;
        private System.Windows.Forms.Label text_keys;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button delete_keys;
        private System.Windows.Forms.Button listen_sound;
        private System.Windows.Forms.Button button2;
    }
}