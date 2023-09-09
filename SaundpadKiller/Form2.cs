
using NAudio.Wave;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave.SampleProviders;
using System.Data.SQLite;
using NAudio.MediaFoundation;

namespace SaundpadKiller
{
    public partial class Form2 : Form
    {


        private void button_add_sound_Click(object sender, EventArgs e)
        {
            try
            {
                waveOut.Stop();
                waveOut.Dispose();
            }
            catch { }

            if (button1.Text == "Обрати звук")
            {
                button1.FlatAppearance.BorderSize = 1;
                button1.FlatAppearance.BorderColor = Color.Red;
                return;
            }
            if (name_sound.Text == "" && button1.Text != "Обрати звук")
            {
                string file_Name = Path.GetFileName(selectedFile);
                file_Name = file_Name.Replace(".mp3", "");
                file_Name = file_Name.Replace(".wav", "");
                name_sound.Text = file_Name;
                return;
            }

            int busyKeys = 0;

            string connectionString = "Data Source=database.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM sounds;"; // Запрос как строка

                using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["ID"]);
                            string key = reader["Key"].ToString();
                            
                            if (key == textbox_keys.Text && textbox_keys.Text != "")
                            {
                                textbox_keys.ForeColor = Color.Red;

                                busyKeys = 1;
                                break;
                            }
                        }
                    }
                }
            }





            if (button1.Text != "Обрати звук" && busyKeys == 0)
            {
                button1.FlatAppearance.BorderSize = 0;

                MoveFile();

                if (button1.Text == "Файл вже існує")
                {
                    ;
                }
                else
                {

                    if (selectedFile3.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                    {

                        if (audioFileReader != null)
                        {
                            audioFileReader.Dispose();
                        }

                        File.Delete(selectedFile3);
                    }

                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        { 
                            command.CommandText = "INSERT INTO sounds (Path, Name, Key, Key_tester, Volume, favourite) VALUES (@Path, @Name, @Key, @Key_tester, @Volume, 0);";
                            command.Parameters.AddWithValue("@Path", destinationFilePath);
                            command.Parameters.AddWithValue("@Name", name_sound.Text);
                            command.Parameters.AddWithValue("@Key", textbox_keys.Text);
                            command.Parameters.AddWithValue("@Key_tester", pressedKeys_tester.ToString());
                            command.Parameters.AddWithValue("@Volume", trackbar_sound_volume.Value);

                            command.ExecuteNonQuery();
                        }
                    }
                        

                    button1.Text = "Обрати звук";
                    name_sound.Text = "";
                    textbox_keys.Text = "";

                    this.Close();
                    Hide();
                }
            }
        }

        private bool Drag;
        private int MouseX;
        private int MouseY;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;
        }
        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }
        private void PanelMove_MouseUp(object sender, MouseEventArgs e) { Drag = false; }


        private string selectedFile;
        private string selectedFile2 = "1";
        private string selectedFile3 = "1";
        private string destinationPath;

        private int output_device_form2;

        public Form2(int device_id_output)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            output_device_form2 = device_id_output;


            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form2_DragEnter);
            this.DragDrop += new DragEventHandler(Form2_DragDrop);

            destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sounds");
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = "Додати звук";

            trackbar_sound_volume.Value = 100;

            text_sound_volume.KeyPress += text_sound_volume_KeyPress;
            textbox_keys.KeyDown += textbox_keys_KeyDown;
            //textbox_keys.KeyUp += textbox_keys_KeyUp;


            GC.Collect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files (*.mp3;*.wav;*.mp4)|*.mp3;*.wav;*.mp4";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                }
                catch { }

                if (selectedFile3.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                {

                    if (audioFileReader != null)
                    {
                        audioFileReader.Dispose();
                    }
                    
                    File.Delete(selectedFile3);
                }

                selectedFile = openFileDialog.FileName;
                string file_Name = Path.GetFileName(selectedFile);
                button1.Text = file_Name;
                file_Name = file_Name.Replace(".mp3", "");
                file_Name = file_Name.Replace(".wav", "");
                file_Name = file_Name.Replace(".mp4", "");
                name_sound.Text = file_Name;

                if (selectedFile.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                {
                    selectedFile2 = selectedFile;
                }
                if (selectedFile.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    selectedFile2 = selectedFile.Replace(".mp3", ".wav");
                    ConvertMp3ToWav(selectedFile, selectedFile2);
                    selectedFile3 = selectedFile2;
                }
                if (selectedFile.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    selectedFile2 = selectedFile.Replace(".mp4", ".wav");
                    ConvertMp4ToWav(selectedFile, selectedFile2);
                    selectedFile3 = selectedFile2;
                }
            }

            openFileDialog.Dispose();
        }

        private void Form2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form2_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                if (IsFileValid(file))
                {
                    try
                    {
                        waveOut.Stop();
                        waveOut.Dispose();
                    }
                    catch { }

                    if (selectedFile3.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                    {

                        if (audioFileReader != null)
                        {
                            audioFileReader.Dispose();
                        }

                        File.Delete(selectedFile3);
                    }

                    selectedFile = file;
                    string file_Name = Path.GetFileName(selectedFile);
                    button1.Text = file_Name;
                    file_Name = file_Name.Replace(".mp3", "");
                    file_Name = file_Name.Replace(".wav", "");
                    file_Name = file_Name.Replace(".mp4", "");
                    name_sound.Text = file_Name;

                    if (selectedFile.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                    {
                        selectedFile2 = selectedFile;
                    }
                    if (selectedFile.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                    {
                        selectedFile2 = selectedFile.Replace(".mp3", ".wav");
                        ConvertMp3ToWav(selectedFile, selectedFile2);
                        selectedFile3 = selectedFile2;
                    }
                    if (selectedFile.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
                    {
                        selectedFile2 = selectedFile.Replace(".mp4", ".wav");
                        ConvertMp4ToWav(selectedFile, selectedFile2);
                        selectedFile3 = selectedFile2;
                    }

                    break;
                }
            }
        }

        private string destinationFilePath;
        string sourceFilePath;
        private void MoveFile()
        {
            try
            {
                string newDirectoryName = "sounds";
                sourceFilePath = selectedFile2;

                string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                string newDirectoryPath = Path.Combine(applicationPath, newDirectoryName);
                Directory.CreateDirectory(newDirectoryPath);

                string fileName = Path.GetFileName(sourceFilePath);
                destinationFilePath = Path.Combine(newDirectoryPath, fileName);

                File.Copy(sourceFilePath, destinationFilePath);
            }
            catch
            {
                button1.Text = "Файл вже існує";
                name_sound.Text = "Файл вже існує";
                button1.FlatAppearance.BorderSize = 1;
                button1.FlatAppearance.BorderColor = Color.Red;
            }
        }


        private bool IsFileValid(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".mp3" || extension == ".wav" || extension == ".mp4";
        }

        private void trackbar_sound_volume_Scroll(object sender)
        {
            text_sound_volume.Text = trackbar_sound_volume.Value.ToString();

            ChangeVolume(trackbar_sound_volume.Value / 100.0f);
        }

        private void text_sound_volume_TextChanged(object sender, EventArgs e)
        {
            if (text_sound_volume.Text == "")
            {
                text_sound_volume.Text = "0";
            }
            if (Convert.ToInt32(text_sound_volume.Text) > 500)
            {
                text_sound_volume.Text = "500";
            }

            trackbar_sound_volume.Value = Convert.ToInt32(text_sound_volume.Text);
        }
        private void text_sound_volume_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private StringBuilder pressedKeys = new StringBuilder();
        private StringBuilder pressedKeys_tester = new StringBuilder();

        private void textbox_keys_KeyDown(object sender, KeyEventArgs e)
        {
            // Добавляем нажатую клавишу в список нажатых клавиш
            if (!pressedKeys_tester.ToString().Contains(e.KeyCode.ToString()))
            {
                if (pressedKeys_tester.Length > 0)
                {
                    pressedKeys.Append("+");
                    pressedKeys_tester.Append("+");
                }

                if (e.KeyCode.ToString() == "ControlKey")
                {
                    pressedKeys.Append("Ctrl");
                }
                else if (e.KeyCode.ToString() == "Escape")
                {
                    pressedKeys.Append("Esc");
                }
                else if (e.KeyCode.ToString() == "Insert")
                {
                    pressedKeys.Append("Ins");
                }
                else if (e.KeyCode.ToString() == "Oemtilde")
                {
                    pressedKeys.Append("`");
                }
                else if (e.KeyCode.ToString() == "D1")
                {
                    pressedKeys.Append("1");
                }
                else if (e.KeyCode.ToString() == "D2")
                {
                    pressedKeys.Append("2");
                }
                else if (e.KeyCode.ToString() == "D3")
                {
                    pressedKeys.Append("3");
                }
                else if (e.KeyCode.ToString() == "D4")
                {
                    pressedKeys.Append("4");
                }
                else if (e.KeyCode.ToString() == "D5")
                {
                    pressedKeys.Append("5");
                }
                else if (e.KeyCode.ToString() == "D6")
                {
                    pressedKeys.Append("6");
                }
                else if (e.KeyCode.ToString() == "D7")
                {
                    pressedKeys.Append("7");
                }
                else if (e.KeyCode.ToString() == "D8")
                {
                    pressedKeys.Append("8");
                }
                else if (e.KeyCode.ToString() == "D9")
                {
                    pressedKeys.Append("9");
                }
                else if (e.KeyCode.ToString() == "D0")
                {
                    pressedKeys.Append("0");
                }
                else if (e.KeyCode.ToString() == "OemMinus")
                {
                    pressedKeys.Append("-");
                }
                else if (e.KeyCode.ToString() == "Oemplus")
                {
                    pressedKeys.Append("+");
                }
                else if (e.KeyCode.ToString() == "Return")
                {
                    pressedKeys.Append("Enter");
                }
                else if (e.KeyCode.ToString() == "Delete")
                {
                    pressedKeys.Append("Dlt");
                }
                else if (e.KeyCode.ToString() == "Next")
                {
                    pressedKeys.Append("PageDown");
                }
                else if (e.KeyCode.ToString() == "Divide")
                {
                    pressedKeys.Append("Num/");
                }
                else if (e.KeyCode.ToString() == "Multiply")
                {
                    pressedKeys.Append("Num*");
                }
                else if (e.KeyCode.ToString() == "Subtract")
                {
                    pressedKeys.Append("Num-");
                }
                else if (e.KeyCode.ToString() == "OemOpenBrackets")
                {
                    pressedKeys.Append("[");
                }
                else if (e.KeyCode.ToString() == "Oem6")
                {
                    pressedKeys.Append("]");
                }
                else if (e.KeyCode.ToString() == "NumPad7")
                {
                    pressedKeys.Append("Num7");
                }
                else if (e.KeyCode.ToString() == "NumPad9")
                {
                    pressedKeys.Append("Num9");
                }
                else if (e.KeyCode.ToString() == "NumPad8")
                {
                    pressedKeys.Append("Num8");
                }
                else if (e.KeyCode.ToString() == "NumPad6")
                {
                    pressedKeys.Append("Num6");
                }
                else if (e.KeyCode.ToString() == "NumPad5")
                {
                    pressedKeys.Append("Num5");
                }
                else if (e.KeyCode.ToString() == "NumPad4")
                {
                    pressedKeys.Append("Num4");
                }
                else if (e.KeyCode.ToString() == "NumPad3")
                {
                    pressedKeys.Append("Num3");
                }
                else if (e.KeyCode.ToString() == "NumPad2")
                {
                    pressedKeys.Append("Num2");
                }
                else if (e.KeyCode.ToString() == "NumPad2")
                {
                    pressedKeys.Append("Num7");
                }
                else if (e.KeyCode.ToString() == "NumPad1")
                {
                    pressedKeys.Append("Num1");
                }
                else if (e.KeyCode.ToString() == "NumPad0")
                {
                    pressedKeys.Append("Num0");
                }
                else if (e.KeyCode.ToString() == "Add")
                {
                    pressedKeys.Append("Num+");
                }
                else if (e.KeyCode.ToString() == "Decimal")
                {
                    pressedKeys.Append("Num.");
                }
                else if (e.KeyCode.ToString() == "Capital")
                {
                    pressedKeys.Append("CapsLock");
                }
                else if (e.KeyCode.ToString() == "Oem1")
                {
                    pressedKeys.Append(";");
                }
                else if (e.KeyCode.ToString() == "Oem7")
                {
                    pressedKeys.Append("'");
                }
                else if (e.KeyCode.ToString() == "Oem5")
                {
                    pressedKeys.Append("\\");
                }
                else if (e.KeyCode.ToString() == "ShiftKey")
                {
                    pressedKeys.Append("Shift");
                }
                else if (e.KeyCode.ToString() == "OemBackslash")
                {
                    pressedKeys.Append("Backs\\");
                }
                else if (e.KeyCode.ToString() == "Oemcomma")
                {
                    pressedKeys.Append(",");
                }
                else if (e.KeyCode.ToString() == "OemPeriod")
                {
                    pressedKeys.Append(".");
                }
                else if (e.KeyCode.ToString() == "OemQuestion")
                {
                    pressedKeys.Append("/");
                }
                else if (e.KeyCode.ToString() == "LWin")
                {
                    pressedKeys.Append("Win");
                }
                else if (e.KeyCode.ToString() == "Menu")
                {
                    pressedKeys.Append("Alt");
                }
                else if (e.KeyCode.ToString() == "LaunchMail")
                {
                    pressedKeys.Append("Mail");
                }
                else if (e.KeyCode.ToString() == "MediaPlayPause")
                {
                    pressedKeys.Append("PlayPause");
                }
                else if (e.KeyCode.ToString() == "MediaPreviousTrack")
                {
                    pressedKeys.Append("PreviousTrack");
                }
                else if (e.KeyCode.ToString() == "MediaNextTrack")
                {
                    pressedKeys.Append("NextTrack");
                }
                else if (e.KeyCode.ToString() == "LaunchApplication2")
                {
                    pressedKeys.Append("Application2");
                }
                else
                {
                    pressedKeys.Append(e.KeyCode.ToString());
                }

                pressedKeys_tester.Append(e.KeyCode.ToString());
            }

            // Обновляем текстовое поле с нажатыми клавишами
            textbox_keys.Text = pressedKeys.ToString();
        }

        //private void textbox_keys_KeyUp(object sender, KeyEventArgs e)
        //{
        //    // Удаляем отпущенную клавишу из списка нажатых клавиш
        //    string keyToRemove = e.KeyCode.ToString();
        //    int index = pressedKeys.ToString().IndexOf(keyToRemove);
        //    if (index >= 0)
        //    {
        //        pressedKeys.Remove(index, keyToRemove.Length);
        //        if (index > 0 && index < pressedKeys.Length - 2 && pressedKeys[index - 2] == ' ' && pressedKeys[index - 1] == '+' && pressedKeys[index] == ' ')
        //        {
        //            pressedKeys.Remove(index - 2, 3);
        //        }
        //        else if (index > 0 && pressedKeys[index - 1] == ' ' && pressedKeys[index] == '+')
        //        {
        //            pressedKeys.Remove(index - 1, 2);
        //        }
        //        else if (index < pressedKeys.Length - 1 && pressedKeys[index] == '+' && pressedKeys[index + 1] == ' ')
        //        {
        //            pressedKeys.Remove(index, 2);
        //        }
        //    }

        //    // Обновляем текстовое поле с нажатыми клавишами
        //    textbox_keys.Text = pressedKeys.ToString();
        //}





        private void delete_keys_Click(object sender, EventArgs e)
        {
            textbox_keys.Text = "";
            textbox_keys.ForeColor = Color.Gainsboro;
            pressedKeys.Clear();
            pressedKeys_tester.Clear();
        }


        public void ConvertMp3ToWav(string inputFilePath, string outputFilePath)
        {
            using (var mp3Reader = new Mp3FileReader(inputFilePath))
            {
                using (var waveWriter = new WaveFileWriter(outputFilePath, mp3Reader.WaveFormat))
                {
                    mp3Reader.CopyTo(waveWriter);
                    waveWriter.Dispose();
                    mp3Reader.Dispose();
                }
            }
        }

        public void ConvertMp4ToWav(string inputFilePath, string outputFilePath)
        {
            MediaFoundationApi.Startup();

            try
            {
                using (var reader = new MediaFoundationReader(inputFilePath))
                {
                    var pcmStream = WaveFormatConversionStream.CreatePcmStream(reader);
                    WaveFileWriter.CreateWaveFile(outputFilePath, pcmStream);
                    pcmStream.Close();
                }
            }
            finally
            {
                MediaFoundationApi.Shutdown();
            }
        }


        private void ChangeVolume(float volume)
        {
            currentVolume = volume;
            if (volumeProvider != null)
            {
                volumeProvider.Volume = currentVolume;
            }
        }


        private WaveOutEvent waveOut;
        private WaveStream audioFileReader;
        private int a = 0;
        private float currentVolume = 1.0f;

        private VolumeSampleProvider volumeProvider;

        private void play()
        {
            string audioFilePath;

            audioFilePath = selectedFile2;

            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            int deviceNumber = output_device_form2;

            var selectedDevice = devices[deviceNumber];

            waveOut = new WaveOutEvent();

            int waveOutDeviceNumber = 0;
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                string substring2 = selectedDevice.FriendlyName.Substring(0, 15);
                if (substring1.Equals(substring2))
                {
                    waveOutDeviceNumber = i;
                    break;
                }
            }

            waveOut.DeviceNumber = waveOutDeviceNumber;

            audioFileReader = new WaveFileReader(audioFilePath);

            volumeProvider = new VolumeSampleProvider(audioFileReader.ToSampleProvider());

            volumeProvider.Volume = currentVolume;

            volumeProvider.Volume = Math.Min(volumeProvider.Volume * trackbar_sound_volume.Value / 100, 5.0f);

            waveOut.Init(volumeProvider);

            waveOut.Play();
        }

        private void listen_sound_Click(object sender, EventArgs e)
        {
            if (button1.Text != "Обрати звук" & button1.Text != "Файл вже існує") 
            {
                if (a > 0)
                {
                    if (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        waveOut.Stop();
                        waveOut.Dispose();
                        audioFileReader.Dispose();
                    }
                    else
                    {
                        ChangeVolume(trackbar_sound_volume.Value / 100.0f);
                        play();
                    }
                }
                else
                {
                    ChangeVolume(trackbar_sound_volume.Value / 100.0f);
                    play();
                }

                ++a;
            }
        }

        private void button_close_MouseEnter(object sender, EventArgs e)
        {
            button_close.BackColor = Color.FromArgb(158, 24, 24);
        }

        private void button_close_MouseLeave(object sender, EventArgs e)
        {
            button_close.BackColor = Color.FromArgb(76, 76, 76);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            try
            {
                waveOut.Stop();
                waveOut.Dispose();
            }
            catch {}

            if (selectedFile3.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            {

                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                }

                File.Delete(selectedFile3);
            }

            button1.Text = "Обрати звук";
            name_sound.Text = "";
            textbox_keys.Text = "";

            this.Close();
            Hide();
        }

        private void button_close_sound_Click(object sender, EventArgs e)
        {
            try
            {
                waveOut.Stop();
                waveOut.Dispose();
            }
            catch { }

            if (selectedFile3.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            {

                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                }

                File.Delete(selectedFile3);
            }

            button1.Text = "Обрати звук";
            name_sound.Text = "";
            textbox_keys.Text = "";

            this.Close();
            Hide();
        }
    }
}