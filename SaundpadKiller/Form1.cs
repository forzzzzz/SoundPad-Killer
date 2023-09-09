using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using NAudio.Wave;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data.SQLite;
using System.Drawing.Drawing2D;
using NAudio.Wave.SampleProviders;
using System.IO;
using XComponent.SliderBar;

namespace SaundpadKiller
{
    public partial class Form1 : Form
    {
        private Form2 form2;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private LowLevelKeyboardProc _proс;
        private static IntPtr _hookID = IntPtr.Zero;
        private static List<Keys> pressedKeys = new List<Keys>();
        private static string targetKeyString; /* pressedKeys_tester2.ToString();*/ // Здесь задайте вашу целевую строку

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private DateTime lastKeyPressTime = DateTime.MinValue;

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
                {
                    if (!pressedKeys.Contains(key))
                        pressedKeys.Add(key);

                    CheckKeys();
                }
                else if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
                {
                    pressedKeys.Remove(key);
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private bool isThread;

        private void CheckKeys()
        {

            string pressedKeyString = string.Join("+", pressedKeys.OrderBy(k => k).Select(k => k.ToString()));

            bool areEqual = AreStringsEqualBySortedChars(pressedKeyString, targetKeyString);

            if (areEqual && audioFileReader2 != null)
            {
                if (waveOut2.PlaybackState == PlaybackState.Stopped)
                {
                    waveOut2.Dispose();
                    waveOutput2.Dispose();

                    if (audioFileReader2 != null)
                    {
                        audioFileReader2.Dispose();
                        audioFileReaderOutput2.Dispose();
                    }

                    audioFileReader2 = new WaveFileReader(audioFilePath2);
                    audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);

                    TimeSpan desiredTime = TimeSpan.FromMilliseconds(macTrackBar1.Value);
                    if (macTrackBar1.Value != macTrackBar1.Maximum)
                    {
                        audioFileReader2.CurrentTime = desiredTime;
                        audioFileReaderOutput2.CurrentTime = desiredTime;
                    }

                    volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                    volumeProvider.Volume = Math.Min(volumeProvider.Volume * volume_play * trackbar_Main_volume.Value / 10000, 25.0f);

                    waveOut2.Init(volumeProvider);


                    waveOut2.Play();



                    volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                    volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * volume_play * track_bar_Output.Value / 10000, 25.0f);

                    waveOutput2.Init(volumeOutputProvider);


                    waveOutput2.Play();
                }

                else if (waveOut2.PlaybackState == PlaybackState.Playing)
                {
                    waveOut2.Pause();
                    waveOutput2.Pause();
                }
                else if (waveOut2.PlaybackState == PlaybackState.Paused)
                {
                    waveOut2.Play();
                    waveOutput2.Play();
                }
            }



            else
            {
                string connectionString = "Data Source=database.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM sounds;";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["ID"]);
                                string name = reader["Name"].ToString();
                                string path = reader["Path"].ToString();
                                string key = reader["Key"].ToString();
                                string key_tester = reader["Key_tester"].ToString();
                                int volume = Convert.ToInt32(reader["Volume"]);

                                areEqual = AreStringsEqualBySortedChars(pressedKeyString, key_tester);

                                if (areEqual)
                                {


                                    DateTime currentTime = DateTime.Now;
                                    if ((currentTime - lastKeyPressTime).TotalMilliseconds >= 150)
                                    {
                                        WaveOutEvent waveOut;
                                        WaveStream audioFileReader;

                                        WaveOutEvent waveOutput;
                                        WaveStream audioFileReaderOutput;

                                        waveOut = new WaveOutEvent();

                                        waveOutput = new WaveOutEvent();

                                        if (checkBox2.Switched == true)
                                        {
                                            string audioFilePath;
                                            string audioFilePathOutput;

                                            int waveOutDeviceNumber = 0;
                                            for (int i = 0; i < WaveOut.DeviceCount; i++)
                                            {
                                                string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                                                string substring2 = "CABLE Input (VB-Audio Virtual Cable)".Substring(0, 15);
                                                if (substring1.Equals(substring2))
                                                {
                                                    waveOutDeviceNumber = i;
                                                    break;
                                                }
                                            }

                                            int waveOutputDeviceNumber = 0;
                                            for (int i = 0; i < WaveOut.DeviceCount; i++)
                                            {
                                                string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                                                string substring2 = comboBox1_Output_devices.Text.Substring(0, 15);
                                                if (substring1.Equals(substring2))
                                                {
                                                    waveOutputDeviceNumber = i;
                                                    break;
                                                }
                                            }

                                            waveOut.DeviceNumber = waveOutDeviceNumber;

                                            waveOutput.DeviceNumber = waveOutputDeviceNumber;

                                            audioFilePath = path;
                                            audioFilePathOutput = path;


                                            audioFileReader = new WaveFileReader(audioFilePath);
                                            audioFileReaderOutput = new WaveFileReader(audioFilePathOutput);


                                            if (waveOut.PlaybackState == PlaybackState.Stopped)
                                            {

                                                var volumeProvider1 = new VolumeSampleProvider(audioFileReader.ToSampleProvider());
                                                volumeProvider1.Volume = Math.Min(volumeProvider1.Volume * volume * trackbar_Main_volume.Value / 10000, 25.0f);

                                                waveOut.Init(volumeProvider1);


                                                waveOut.Play();



                                                var volumeOutputProvider1 = new VolumeSampleProvider(audioFileReaderOutput.ToSampleProvider());
                                                volumeOutputProvider1.Volume = Math.Min(volumeOutputProvider1.Volume * volume * track_bar_Output.Value / 10000, 25.0f);

                                                waveOutput.Init(volumeOutputProvider1);


                                                waveOutput.Play();
                                            }

                                            else if (waveOut.PlaybackState == PlaybackState.Playing)
                                            {
                                                waveOut.Stop();
                                                waveOut.Dispose();

                                                var volumeProvider1 = new VolumeSampleProvider(audioFileReader.ToSampleProvider());
                                                volumeProvider1.Volume = Math.Min(volumeProvider1.Volume * volume * trackbar_Main_volume.Value / 10000, 25.0f);

                                                waveOut.Init(volumeProvider1);


                                                waveOut.Play();

                                                waveOutput.Stop();
                                                waveOutput.Dispose();

                                                var volumeOutputProvider1 = new VolumeSampleProvider(audioFileReaderOutput.ToSampleProvider());
                                                volumeOutputProvider1.Volume = Math.Min(volumeOutputProvider1.Volume * volume * track_bar_Output.Value / 10000, 25.0f);

                                                waveOutput.Init(volumeOutputProvider1);


                                                waveOutput.Play();
                                            }
                                        }

                                        else
                                        {

                                            textBox1.Text = name;
                                            textBox2.Text = key;

                                            if (panel2.Visible == false)
                                            {
                                                panel2.Visible = true;
                                                flowLayoutPanel1.Size = new Size(flowLayoutPanel1.Size.Width, flowLayoutPanel1.Size.Height - 81);
                                            }

                                            int waveOutDeviceNumber = 0;
                                            for (int i = 0; i < WaveOut.DeviceCount; i++)
                                            {
                                                string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                                                string substring2 = "CABLE Input (VB-Audio Virtual Cable)".Substring(0, 15);
                                                if (substring1.Equals(substring2))
                                                {
                                                    waveOutDeviceNumber = i;
                                                    break;
                                                }
                                            }

                                            int waveOutputDeviceNumber = 0;
                                            for (int i = 0; i < WaveOut.DeviceCount; i++)
                                            {
                                                string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                                                string substring2 = comboBox1_Output_devices.Text.Substring(0, 15);
                                                if (substring1.Equals(substring2))
                                                {
                                                    waveOutputDeviceNumber = i;
                                                    break;
                                                }
                                            }

                                            waveOut2.DeviceNumber = waveOutDeviceNumber;

                                            waveOutput2.DeviceNumber = waveOutputDeviceNumber;

                                            audioFilePath2 = path;
                                            audioFilePathOutput2 = path;

                                            if (waveOut2.PlaybackState == PlaybackState.Stopped)
                                            {

                                                waveOut2.Dispose();
                                                waveOutput2.Dispose();

                                                if (audioFileReader2 != null)
                                                {
                                                    audioFileReader2.Dispose();
                                                    audioFileReaderOutput2.Dispose();
                                                }

                                                audioFileReader2 = new WaveFileReader(audioFilePath2);
                                                audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);
                                                TimeSpan desiredTime = TimeSpan.FromMilliseconds(macTrackBar1.Value);
                                                if (macTrackBar1.Value != macTrackBar1.Maximum && audioFilePath2_tester == audioFilePath2)
                                                {
                                                    audioFileReader2.CurrentTime = desiredTime;
                                                    audioFileReaderOutput2.CurrentTime = desiredTime;
                                                }

                                                audioFilePath2_tester = audioFilePath2;

                                                TimeSpan duration2 = audioFileReader2.TotalTime;
                                                label8.Text = $"{duration2.Minutes}:{duration2.Seconds:00}";
                                                macTrackBar1.Maximum = Convert.ToInt32(duration2.TotalMilliseconds);

                                                volume_play = volume;

                                                volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                                                volumeProvider.Volume = Math.Min(volumeProvider.Volume * volume * trackbar_Main_volume.Value / 10000, 25.0f);

                                                waveOut2.Init(volumeProvider);


                                                waveOut2.Play();
                                                if (updateTimer == null)
                                                {
                                                    updateTimer = new System.Threading.Timer(UpdateTimer_Tick, null, 0, 100);
                                                }

                                                volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                                                volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * volume * track_bar_Output.Value / 10000, 25.0f);

                                                waveOutput2.Init(volumeOutputProvider);


                                                waveOutput2.Play();
                                            }

                                            else if (waveOut2.PlaybackState == PlaybackState.Playing)
                                            {
                                                audioFilePath2_tester = audioFilePath2;

                                                waveOut2.Stop();
                                                waveOut2.Dispose();

                                                if (audioFileReader2 != null)
                                                {
                                                    audioFileReader2.Dispose();
                                                    audioFileReaderOutput2.Dispose();
                                                }

                                                audioFileReader2 = new WaveFileReader(audioFilePath2);
                                                audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);

                                                textBox1.Text = name;
                                                textBox2.Text = key;

                                                audioFileReader2 = new WaveFileReader(audioFilePath2);
                                                audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);
                                                TimeSpan duration2 = audioFileReader2.TotalTime;
                                                label8.Text = $"{duration2.Minutes}:{duration2.Seconds:00}";
                                                macTrackBar1.Maximum = Convert.ToInt32(duration2.TotalMilliseconds);

                                                volume_play = volume;

                                                volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                                                volumeProvider.Volume = Math.Min(volumeProvider.Volume * volume * trackbar_Main_volume.Value / 10000, 25.0f);

                                                waveOut2.Init(volumeProvider);


                                                waveOut2.Play();

                                                waveOutput2.Stop();
                                                waveOutput2.Dispose();

                                                volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                                                volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * volume * track_bar_Output.Value / 10000, 25.0f);

                                                waveOutput2.Init(volumeOutputProvider);


                                                waveOutput2.Play();
                                            }
                                            else if (waveOut2.PlaybackState == PlaybackState.Paused && audioFilePath2_tester == audioFilePath2)
                                            {
                                                waveOut2.Play();

                                                waveOutput2.Play();
                                            }
                                            else if (waveOut2.PlaybackState == PlaybackState.Paused && audioFilePath2_tester != audioFilePath2)
                                            {
                                                audioFilePath2_tester = audioFilePath2;

                                                waveOut2.Dispose();
                                                waveOutput2.Dispose();

                                                if (audioFileReader2 != null)
                                                {
                                                    audioFileReader2.Dispose();
                                                    audioFileReaderOutput2.Dispose();
                                                }

                                                audioFileReader2 = new WaveFileReader(audioFilePath2);
                                                audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);
                                                TimeSpan duration2 = audioFileReader2.TotalTime;
                                                label8.Text = $"{duration2.Minutes}:{duration2.Seconds:00}";
                                                macTrackBar1.Maximum = Convert.ToInt32(duration2.TotalMilliseconds);

                                                volume_play = volume;

                                                volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                                                volumeProvider.Volume = Math.Min(volumeProvider.Volume * volume * trackbar_Main_volume.Value / 10000, 25.0f);

                                                waveOut2.Init(volumeProvider);


                                                waveOut2.Play();


                                                volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                                                volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * volume * track_bar_Output.Value / 10000, 25.0f);

                                                waveOutput2.Init(volumeOutputProvider);

                                                waveOutput2.Play();
                                            }
                                        }


                                        lastKeyPressTime = currentTime;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool AreStringsEqualBySortedChars(string str1, string str2)
        {

            if (str1.Contains("LControlKey") || str1.Contains("RControlKey"))
            {
                str1 = str1.Replace("L", "");
                str1 = str1.Replace("R", "");
            }
            if (str1.Contains("LMenu") || str1.Contains("RMenu"))
            {
                str1 = str1.Replace("L", "");
                str1 = str1.Replace("R", "");
            }
            if (str1.Contains("LShiftKey") || str1.Contains("RShiftKey"))
            {
                str1 = str1.Replace("L", "");
                str1 = str1.Replace("R", "");
            }

            // Преобразовать строки в массивы символов, отсортировать их и сравнить отсортированные строки.
            string sortedStr1 = new string(str1.ToCharArray().OrderBy(c => c).ToArray());
            string sortedStr2 = new string(str2.ToCharArray().OrderBy(c => c).ToArray());

            return sortedStr1 == sortedStr2;
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(module.ModuleName), 0);
            }
        }


        CoreAudioController Controller;
        CoreAudioDevice defaultInputDevice;
        CoreAudioDevice defaultOutputDevice;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            form2 = new Form2(device_id_output);
            form2.FormClosing += Form2_FormClosing;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.PerformLayout();
            Activated += Form1_Activated;
            name_sound.KeyDown += name_sound_KeyDown;
            name_sound.KeyPress += name_sound_KeyPress;
            textbox_keys.KeyDown += textbox_keys_KeyDown;

            _proс = new LowLevelKeyboardProc(HookCallback);
            _hookID = SetHook(_proс);
        }

        private void OpenForm2()
        {
            form2.Show();
        }

        protected void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            sounds_load();
        }

        private List<CoreAudioDevice> devices;
        private List<CoreAudioDevice> devices_output;

        private string connectionString = "Data Source=database.db;Version=3;";
        private void Form1_Load(object sender, EventArgs e)
        {

            Controller = new CoreAudioController();


            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS sounds (ID INTEGER PRIMARY KEY, Path TEXT, Name TEXT, Key TEXT, Key_tester TEXT, Volume INTEGER, favourite INTEGER);";
                    command.ExecuteNonQuery();
                }
            }

            comboBox1.Text = "Від старих до нових";

            sounds_load();

            devices = Controller.GetDevices(DeviceType.Capture, AudioSwitcher.AudioApi.DeviceState.Active).ToList();
            devices_output = Controller.GetDevices(DeviceType.Playback, AudioSwitcher.AudioApi.DeviceState.Active).ToList();
            UpdateDropdowns();
            Load_settings();

            text_input_volume.KeyPress += text_input_volume_KeyPress;
            text_output_volume.KeyPress += text_output_volume_KeyPress;
            text_main_volume.KeyPress += text_main_volume_KeyPress;

            GraphicsPath path7 = new GraphicsPath();
            path7.AddEllipse(-1, -1, pictureBox1.Width + 2, pictureBox1.Height + 2);
            pictureBox1.Region = new Region(path7);

            GraphicsPath path8 = new GraphicsPath();
            path8.AddEllipse(-3, -3, pictureBox2.Width + 3, pictureBox2.Height + 3);
            pictureBox2.Region = new Region(path8);

            GraphicsPath path9 = new GraphicsPath();
            path9.AddEllipse(-2, -2, pictureBox5.Width + 3, pictureBox5.Height + 3);
            pictureBox5.Region = new Region(path9);

            GraphicsPath path10 = new GraphicsPath();
            path10.AddEllipse(-2, -2, pictureBox4.Width + 3, pictureBox4.Height + 3);
            pictureBox4.Region = new Region(path10);

            teep1();

            GC.Collect();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            int totalHeightOfControls = 0;

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                totalHeightOfControls += control.Height;
            }

            flowLayoutPanel1.AutoScrollMinSize = new Size(flowLayoutPanel1.Width - 17, totalHeightOfControls);

        }

        private void teep1()
        {
            if (Properties.Settings.Default.teep == false)
            {
                Process.Start("control", $"mmsys.cpl,,1");
                Form3 form3 = new Form3();
                form3.ShowDialog();
            }
        }

        private void teep2()
        {
            if (Properties.Settings.Default.teep == false)
            {
                Form3 form3 = new Form3();
                form3.ShowDialog();
            }
        }

        private void UpdateDropdowns()
        {
            var InputDevices = Controller.GetPlaybackDevices(AudioSwitcher.AudioApi.DeviceState.Active);
            var OutputDevices = Controller.GetCaptureDevices(AudioSwitcher.AudioApi.DeviceState.Active);

            comboBox1_Input_devices.Items.Clear();
            comboBox1_Output_devices.Items.Clear();

            foreach (var device in OutputDevices)
            {
                comboBox1_Input_devices.Items.Add(device.FullName);
            }
            foreach (var device in InputDevices)
            {
                comboBox1_Output_devices.Items.Add(device.FullName);
            }

            defaultInputDevice = Controller.DefaultCaptureDevice;
            defaultOutputDevice = Controller.DefaultPlaybackDevice;

            comboBox1_Input_devices.Text = defaultInputDevice.FullName;
            comboBox1_Output_devices.Text = defaultOutputDevice.FullName;

            track_bar_Output.Value = 100;
            trackbar_Main_volume.Value = 100;

            darktheme_input_on = Properties.Resources.darktheme_input_on;
            darktheme_input_off = Properties.Resources.darktheme_input_off;
            darktheme_output_on = Properties.Resources.darktheme_output_on;
            darktheme_output_off = Properties.Resources.darktheme_output_off;
            //lighttheme_input_on = Properties.Resources.lighttheme_input_on;
            //lighttheme_input_off = Properties.Resources.lighttheme_input_off;
            //lighttheme_output_on = Properties.Resources.lighttheme_output_on;
            //lighttheme_output_off = Properties.Resources.lighttheme_output_off;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
            Save_settings();
        }







        private void track_bar_Input_Scroll(object sender)
        {
            text_input_volume.Text = track_bar_Input.Value.ToString();

            if (track_bar_Input.Value == 0)
            {
                switch_input_volume.Image = darktheme_input_off;
            }
            if (track_bar_Input.Value >= 1 && track_bar_Input.Value <= 9 || track_bar_Input.Value == 10 || track_bar_Input.Value == 20 || track_bar_Input.Value == 30 || track_bar_Input.Value == 40 || track_bar_Input.Value == 50 || track_bar_Input.Value == 60 || track_bar_Input.Value == 70 || track_bar_Input.Value == 80 || track_bar_Input.Value == 90 || track_bar_Input.Value == 100)
            {
                switch_input_volume.Image = darktheme_input_on;
            }

            var device = devices[device_id];
            device.SetVolumeAsync(track_bar_Input.Value);
        }

        private void track_bar_Output_Scroll(object sender)
        {
            text_output_volume.Text = track_bar_Output.Value.ToString();

            if (track_bar_Output.Value == 0)
            {
                switch_output_volume.Image = darktheme_output_off;
            }
            if (track_bar_Output.Value >= 1 && track_bar_Output.Value <= 9 || track_bar_Output.Value == 10 || track_bar_Output.Value == 20 || track_bar_Output.Value == 30 || track_bar_Output.Value == 40 || track_bar_Output.Value == 50 || track_bar_Output.Value == 60 || track_bar_Output.Value == 70 || track_bar_Output.Value == 80 || track_bar_Output.Value == 90 || track_bar_Output.Value == 100 || track_bar_Output.Value == 200 || track_bar_Output.Value == 300 || track_bar_Output.Value == 400 || track_bar_Output.Value == 500)
            {
                switch_output_volume.Image = darktheme_output_on;
            }

            if (checkBox1.Switched == true)
            {
                trackbar_Main_volume.Value = track_bar_Output.Value;
            }

            if (volumeProvider != null)
            {
                volumeProvider.Volume = Math.Min(Convert.ToSingle(volume_play) * trackbar_Main_volume.Value / 10000, 25.0f);
                volumeOutputProvider.Volume = Math.Min(Convert.ToSingle(volume_play) * track_bar_Output.Value / 10000, 25.0f);
            }
        }

        private void trackbar_Main_volume_Scroll(object sender)
        {
            text_main_volume.Text = trackbar_Main_volume.Value.ToString();

            if (trackbar_Main_volume.Value == 0)
            {
                switch_main_volume.Image = darktheme_output_off;
            }
            if (trackbar_Main_volume.Value >= 1 && trackbar_Main_volume.Value <= 9 || trackbar_Main_volume.Value == 10 || trackbar_Main_volume.Value == 20 || trackbar_Main_volume.Value == 30 || trackbar_Main_volume.Value == 40 || trackbar_Main_volume.Value == 50 || trackbar_Main_volume.Value == 60 || trackbar_Main_volume.Value == 70 || trackbar_Main_volume.Value == 80 || trackbar_Main_volume.Value == 90 || trackbar_Main_volume.Value == 100 || trackbar_Main_volume.Value == 200 || trackbar_Main_volume.Value == 300 || trackbar_Main_volume.Value == 400 || trackbar_Main_volume.Value == 500)
            {
                switch_main_volume.Image = darktheme_output_on;
            }

            if (checkBox1.Switched == true)
            {
                track_bar_Output.Value = trackbar_Main_volume.Value;
            }

            if (volumeProvider != null)
            {
                volumeProvider.Volume = Math.Min(Convert.ToSingle(volume_play) * trackbar_Main_volume.Value / 10000, 25.0f);
                volumeOutputProvider.Volume = Math.Min(Convert.ToSingle(volume_play) * track_bar_Output.Value / 10000, 25.0f);
            }
        }

        private void text_input_volume_TextChanged(object sender, EventArgs e)
        {
            if (text_input_volume.Text == "")
            {
                text_input_volume.Text = "0";
            }
            if (Convert.ToInt32(text_input_volume.Text) > 100)
            {
                text_input_volume.Text = "100";
            }

            track_bar_Input.Value = Convert.ToInt32(text_input_volume.Text);
        }
        private void text_output_volume_TextChanged(object sender, EventArgs e)
        {
            if (text_output_volume.Text == "")
            {
                text_output_volume.Text = "0";
            }
            if (Convert.ToInt32(text_output_volume.Text) > 500)
            {
                text_output_volume.Text = "500";
            }

            track_bar_Output.Value = Convert.ToInt32(text_output_volume.Text);
        }

        private void text_main_volume_TextChanged(object sender, EventArgs e)
        {
            if (text_main_volume.Text == "")
            {
                text_main_volume.Text = "0";
            }
            if (Convert.ToInt32(text_main_volume.Text) > 500)
            {
                text_main_volume.Text = "500";
            }

            trackbar_Main_volume.Value = Convert.ToInt32(text_main_volume.Text);
        }



        private void text_input_volume_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void text_output_volume_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void text_main_volume_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void switch_input_volume_MouseEnter(object sender, EventArgs e)
        {
            switch_input_volume.BackColor = Color.DimGray;
        }

        private void switch_input_volume_MouseLeave(object sender, EventArgs e)
        {
            switch_input_volume.BackColor = Color.FromArgb(77, 77, 77);
        }

        private void switch_input_volume_Click(object sender, EventArgs e)
        {
            switch_input_volume.BackColor = Color.Gray;
            if (track_bar_Input.Value > 0)
            {
                switch_input_volume.Image = darktheme_input_off;

                Properties.Settings.Default.Input_volume = track_bar_Input.Value;
                track_bar_Input.Value = 0;
            }
            else
            {
                switch_input_volume.Image = darktheme_input_on;
                track_bar_Input.Value = Properties.Settings.Default.Input_volume;
            }
        }
        private void switch_output_volume_MouseEnter(object sender, EventArgs e)
        {
            switch_output_volume.BackColor = Color.DimGray;
        }

        private void switch_output_volume_MouseLeave(object sender, EventArgs e)
        {
            switch_output_volume.BackColor = Color.FromArgb(77, 77, 77);
        }

        private void switch_output_volume_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch_output_volume.BackColor = Color.Gray;
            if (track_bar_Output.Value > 0)
            {
                switch_output_volume.Image = darktheme_output_off;

                Properties.Settings.Default.Output_volume = track_bar_Output.Value;
                track_bar_Output.Value = 0;
            }
            else
            {
                switch_output_volume.Image = darktheme_output_on;
                track_bar_Output.Value = Properties.Settings.Default.Output_volume;

                if (checkBox1.Switched == true)
                {
                    switch_main_volume.Image = darktheme_output_on;
                }
            }
        }
        private void switch_main_volume_MouseEnter(object sender, EventArgs e)
        {
            switch_main_volume.BackColor = Color.DimGray;
        }

        private void switch_main_volume_MouseLeave(object sender, EventArgs e)
        {
            switch_main_volume.BackColor = Color.FromArgb(77, 77, 77);
        }

        private void switch_main_volume_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch_main_volume.BackColor = Color.Gray;
            if (trackbar_Main_volume.Value > 0)
            {
                switch_main_volume.Image = darktheme_output_off;

                Properties.Settings.Default.Main_volume = trackbar_Main_volume.Value;
                trackbar_Main_volume.Value = 0;
            }
            else
            {
                switch_main_volume.Image = darktheme_output_on;
                trackbar_Main_volume.Value = Properties.Settings.Default.Main_volume;

                if (checkBox1.Switched == true)
                {
                    switch_output_volume.Image = darktheme_output_on;
                }
            }
        }
        private void button_add_sounds_Click(object sender, EventArgs e)
        {
            form2.ShowDialog();
        }

        public static bool IsSoundWindowOpen()
        {
            Process[] processes = Process.GetProcessesByName("sndvol");
            return processes.Length > 0;
        }

        private string a;
        private int device_id = 0;
        private void comboBox1_Input_devices_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (a != null && a != comboBox1_Input_devices.Text && a == Properties.Settings.Default.Input_device)
            {
                Process.Start("control", $"mmsys.cpl,,1");

                teep2();

                Properties.Settings.Default.Input_device = comboBox1_Input_devices.Text;
                Properties.Settings.Default.Save();
            }
            a = comboBox1_Input_devices.Text;

            if (Properties.Settings.Default.Input_device == "")
            {
                Properties.Settings.Default.Input_device = comboBox1_Input_devices.Text;
                Properties.Settings.Default.Save();
            }

            device_id = 0;
            foreach (var device in devices)
            {
                if (device.FullName == comboBox1_Input_devices.Text)
                {
                    track_bar_Input.Value = Convert.ToInt32(device.Volume);
                    break;
                }
                device_id++;
            }
        }
        private void button_teep_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private int device_id_output = 0;
        WaveOutEvent waveOut2;
        WaveStream audioFileReader2;

        WaveOutEvent waveOutput2;
        WaveStream audioFileReaderOutput2;
        private void comboBox1_Output_devices_SelectedIndexChanged(object sender, EventArgs e)
        {
            device_id_output = 0;
            foreach (var device in devices_output)
            {
                if (device.FullName == comboBox1_Output_devices.Text)
                {
                    break;
                }
                device_id_output++;
            }

            int waveOutDeviceNumber = 0;
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                string substring2 = "CABLE Input (VB-Audio Virtual Cable)".Substring(0, 15);
                if (substring1.Equals(substring2))
                {
                    waveOutDeviceNumber = i;
                    break;
                }
            }

            int waveOutputDeviceNumber = 0;
            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                string substring2 = comboBox1_Output_devices.Text.Substring(0, 15);
                if (substring1.Equals(substring2))
                {
                    waveOutputDeviceNumber = i;
                    break;
                }
            }

            if (waveOut2 != null)
            {
                waveOut2.DeviceNumber = waveOutDeviceNumber;
                waveOutput2.DeviceNumber = waveOutputDeviceNumber;

                if (waveOut2.PlaybackState == PlaybackState.Stopped && audioFilePath2 != null)
                {
                    waveOut2.Dispose();
                    waveOutput2.Dispose();

                    if (audioFileReader2 != null)
                    {
                        audioFileReader2.Dispose();
                        audioFileReaderOutput2.Dispose();
                    }

                    audioFileReader2 = new WaveFileReader(audioFilePath2);
                    audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);

                    TimeSpan desiredTime = TimeSpan.FromMilliseconds(macTrackBar1.Value * 100);
                    audioFileReader2.CurrentTime = desiredTime;
                    audioFileReaderOutput2.CurrentTime = desiredTime;

                    volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                    volumeProvider.Volume = Math.Min(volumeProvider.Volume * volume_play * trackbar_Main_volume.Value / 10000, 25.0f);

                    waveOut2.Init(volumeProvider);


                    waveOut2.Play();



                    volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                    volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * volume_play * track_bar_Output.Value / 10000, 25.0f);

                    waveOutput2.Init(volumeOutputProvider);


                    waveOutput2.Play();
                }

                else if (waveOut2.PlaybackState == PlaybackState.Playing && audioFilePath2 != null)
                {
                    waveOut2.Stop();
                    waveOut2.Dispose();

                    if (audioFileReader2 != null)
                    {
                        audioFileReader2.Dispose();
                        audioFileReaderOutput2.Dispose();
                    }

                    audioFileReader2 = new WaveFileReader(audioFilePath2);
                    audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);

                    TimeSpan desiredTime = TimeSpan.FromMilliseconds(macTrackBar1.Value * 100);
                    audioFileReader2.CurrentTime = desiredTime;
                    audioFileReaderOutput2.CurrentTime = desiredTime;

                    volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                    volumeProvider.Volume = Math.Min(volumeProvider.Volume * volume_play * trackbar_Main_volume.Value / 10000, 25.0f);

                    waveOut2.Init(volumeProvider);


                    waveOut2.Play();

                    waveOutput2.Stop();
                    waveOutput2.Dispose();

                    volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                    volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * volume_play * track_bar_Output.Value / 10000, 25.0f);

                    waveOutput2.Init(volumeOutputProvider);


                    waveOutput2.Play();
                }
                else if (waveOut2.PlaybackState == PlaybackState.Paused && audioFilePath2 != null)
                {
                    waveOut2.Play();
                    waveOutput2.Play();
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            track_bar_Output.Value = trackbar_Main_volume.Value;

            if (track_bar_Output.Value == 0)
            {
                switch_output_volume.Image = darktheme_output_off;
            }
            else
            {
                switch_output_volume.Image = darktheme_output_on;
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.FromArgb(105, 105, 105);
        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.FromArgb(96, 96, 96);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.FromArgb(110, 110, 110);

            sounds_load();
        }

        private void name_sound_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sounds_load();

                e.Handled = true;
            }
        }
        private void name_sound_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(105, 105, 105);
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(96, 96, 96);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(110, 110, 110);

            comboBox1.Text = "Від старих до нових";
            name_sound.Text = "";

            sounds_load();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sounds_load();
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.BackColor = Color.FromArgb(104, 104, 104);
            panel1.BackColor = Color.FromArgb(104, 104, 104);
            pictureBox3.BackColor = Color.FromArgb(104, 104, 104);
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            label4.BackColor = Color.FromArgb(104, 104, 104);
            panel1.BackColor = Color.FromArgb(104, 104, 104);
            pictureBox3.BackColor = Color.FromArgb(104, 104, 104);
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            label4.BackColor = Color.FromArgb(104, 104, 104);
            panel1.BackColor = Color.FromArgb(104, 104, 104);
            pictureBox3.BackColor = Color.FromArgb(104, 104, 104);
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.BackColor = Color.FromArgb(96, 96, 96);
            panel1.BackColor = Color.FromArgb(96, 96, 96);
            pictureBox3.BackColor = Color.FromArgb(96, 96, 96);
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            label4.BackColor = Color.FromArgb(96, 96, 96);
            panel1.BackColor = Color.FromArgb(96, 96, 96);
            pictureBox3.BackColor = Color.FromArgb(96, 96, 96);
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            label4.BackColor = Color.FromArgb(96, 96, 96);
            panel1.BackColor = Color.FromArgb(96, 96, 96);
            pictureBox3.BackColor = Color.FromArgb(96, 96, 96);
        }

        int active = 0;
        private void label4_Click(object sender, EventArgs e)
        {
            if (active == 0)
            {
                pictureBox3.Image = heart_darktheme_active;
                active = 1;
            }
            else
            {
                pictureBox3.Image = heart_darktheme;
                active = 0;
            }

            sounds_load();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (active == 0)
            {
                pictureBox3.Image = heart_darktheme_active;
                active = 1;
            }
            else
            {
                pictureBox3.Image = heart_darktheme;
                active = 0;
            }

            sounds_load();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (active == 0)
            {
                pictureBox3.Image = heart_darktheme_active;
                active = 1;
            }
            else
            {
                pictureBox3.Image = heart_darktheme;
                active = 0;
            }

            sounds_load();
        }

        private void metroSetSwitch1_SwitchedChanged(object sender)
        {
            if (checkBox1.Switched == true)
            {
                track_bar_Output.Value = trackbar_Main_volume.Value;
            }

            if (track_bar_Output.Value == 0)
            {
                switch_output_volume.Image = darktheme_output_off;
            }
            else
            {
                switch_output_volume.Image = darktheme_output_on;
            }
        }





        

        private string audioFilePath2;
        private string audioFilePathOutput2;

        private int volume_play;

        private string audioFilePath2_tester;

        private System.Threading.Timer updateTimer;

        private VolumeSampleProvider volumeProvider;
        private VolumeSampleProvider volumeOutputProvider;

        private void UpdateLabel7(int value)
        {
            macTrackBar1.Invoke((MethodInvoker)delegate { macTrackBar1.Value = value; });
        }
        private void UpdateTimer_Tick(object state)
        {
            if (isThumbHeld == false)
            {
                try
                {
                    TimeSpan positionInMilliseconds = TimeSpan.FromMilliseconds(audioFileReader2.CurrentTime.TotalMilliseconds);
                    double totalMilliseconds = positionInMilliseconds.TotalMilliseconds;
                    int milliseconds = (int)Math.Round(totalMilliseconds);
                    UpdateLabel7(milliseconds);
                }
                catch { }
            }
        }

        private Panel CreatePanel(int id, string path_sound, string name, string key, int volume, int favourite)
        {

            waveOut2 = new WaveOutEvent();

            waveOutput2 = new WaveOutEvent();

            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);


            Panel panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Size = new Size(flowLayoutPanel1.Width - 24, 60);
            panel.BackColor = Color.FromArgb(82, 82, 82);
            panel.BorderStyle = BorderStyle.None;

            System.Windows.Forms.TextBox txtName = new System.Windows.Forms.TextBox();
            txtName.Text = name;
            txtName.Location = new Point(20, 18);
            txtName.Width = 400;
            txtName.BackColor = Color.FromArgb(82, 82, 82);
            txtName.BorderStyle = BorderStyle.None;
            txtName.ForeColor = Color.Gainsboro;
            txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            Size newMaxSize = new Size(700, txtName.Size.Height);
            txtName.MaximumSize = newMaxSize;

            System.Windows.Forms.TextBox txtKey = new System.Windows.Forms.TextBox();
            txtKey.Text = key;
            txtKey.Location = new Point(497, 19);
            txtKey.Width = 140;
            txtKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            txtKey.BackColor = Color.FromArgb(82, 82, 82);
            txtKey.ForeColor = Color.Gainsboro;
            txtKey.BorderStyle = BorderStyle.None;
            txtKey.ReadOnly = true;

            System.Windows.Forms.PictureBox resetKeyButton = new System.Windows.Forms.PictureBox();
            resetKeyButton.Size = new Size(22, 22);
            resetKeyButton.BackColor = Color.FromArgb(82, 82, 82);
            resetKeyButton.ForeColor = Color.White;
            resetKeyButton.Image = reset_darktheme;
            resetKeyButton.Location = new Point(462, 19);
            resetKeyButton.SizeMode = PictureBoxSizeMode.Zoom;

            GraphicsPath path1 = new GraphicsPath();
            path1.AddEllipse(-3, -3, resetKeyButton.Width + 3, resetKeyButton.Height + 3);
            resetKeyButton.Region = new Region(path1);

            resetKeyButton.MouseEnter += (sender, e) =>
            {
                resetKeyButton.BackColor = Color.FromArgb(95, 95, 95);
            };
            resetKeyButton.MouseLeave += (sender, e) =>
            {
                resetKeyButton.BackColor = Color.FromArgb(82, 82, 82);
            };
            resetKeyButton.Click += (sender, e) =>
            {
                resetKeyButton.BackColor = Color.FromArgb(110, 110, 110);
            };

            //System.Windows.Forms.PictureBox SaveButton = new System.Windows.Forms.PictureBox();
            //SaveButton.Size = new Size(22, 22);
            //SaveButton.BackColor = Color.FromArgb(82, 82, 82);
            //SaveButton.ForeColor = Color.White;
            //SaveButton.Image = save_darktheme;
            //SaveButton.Location = new Point(632, 19);
            //SaveButton.SizeMode = PictureBoxSizeMode.Zoom;

            //GraphicsPath path11 = new GraphicsPath();
            //path11.AddEllipse(-3, -3, resetKeyButton.Width + 3, resetKeyButton.Height + 3);
            //resetKeyButton.Region = new Region(path11);

            //SaveButton.MouseEnter += (sender, e) =>
            //{
            //    SaveButton.BackColor = Color.FromArgb(95, 95, 95);
            //};
            //SaveButton.MouseLeave += (sender, e) =>
            //{
            //    SaveButton.BackColor = Color.FromArgb(82, 82, 82);
            //};
            //SaveButton.Click += (sender, e) =>
            //{
            //    SaveButton.BackColor = Color.FromArgb(110, 110, 110);
            //    panel.Controls.Remove(SaveButton);

            //    command.CommandText = "UPDATE sounds SET Key = @Key WHERE ID = @id;";
            //    command.Parameters.AddWithValue("@Key", txtKey.Text);
            //    command.Parameters.AddWithValue("@id", id);

            //    command.ExecuteNonQuery();

            //    sounds_load();
            //};

            MetroSet_UI.Controls.MetroSetTrackBar trackBarVolume = new MetroSet_UI.Controls.MetroSetTrackBar();
            trackBarVolume.Minimum = 0;
            trackBarVolume.Maximum = 500;
            trackBarVolume.Value = volume;
            trackBarVolume.Location = new Point(760, 22);
            trackBarVolume.Width = 180;

            System.Windows.Forms.TextBox txtVolume = new System.Windows.Forms.TextBox();
            txtVolume.Text = trackBarVolume.Value.ToString();
            txtVolume.Location = new Point(950, 18);
            txtVolume.Width = 40;
            txtVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            txtVolume.BackColor = Color.FromArgb(82, 82, 82);
            txtVolume.ForeColor = Color.Gainsboro;
            txtVolume.BorderStyle = BorderStyle.None;

            System.Windows.Forms.PictureBox roundButton = new System.Windows.Forms.PictureBox();
            roundButton.Size = new Size(40, 40);
            roundButton.BackColor = Color.FromArgb(82, 82, 82);
            roundButton.ForeColor = Color.White;
            roundButton.Image = play_darkTheme;
            roundButton.Location = new Point(680, 10);
            roundButton.SizeMode = PictureBoxSizeMode.Zoom;

            GraphicsPath path2 = new GraphicsPath();
            path2.AddEllipse(-2, -2, roundButton.Width + 3, roundButton.Height + 3);
            roundButton.Region = new Region(path2);

            roundButton.MouseEnter += (sender, e) =>
            {
                roundButton.BackColor = Color.FromArgb(95, 95, 95);
            };
            roundButton.MouseLeave += (sender, e) =>
            {
                roundButton.BackColor = Color.FromArgb(82, 82, 82);
            };
            roundButton.Click += (sender, e) =>
            {
                roundButton.BackColor = Color.FromArgb(110, 110, 110);
            };

            System.Windows.Forms.PictureBox deleteButton = new System.Windows.Forms.PictureBox();
            deleteButton.Size = new Size(32, 32);
            deleteButton.BackColor = Color.FromArgb(82, 82, 82);
            deleteButton.ForeColor = Color.White;
            deleteButton.Image = delete_darktheme;
            deleteButton.Location = new Point(1050, 14);
            deleteButton.SizeMode = PictureBoxSizeMode.Zoom;

            GraphicsPath path3 = new GraphicsPath();
            path3.AddEllipse(0, 0, deleteButton.Width, deleteButton.Height);
            deleteButton.Region = new Region(path3);

            deleteButton.MouseEnter += (sender, e) =>
            {
                deleteButton.BackColor = Color.FromArgb(95, 95, 95);
            };
            deleteButton.MouseLeave += (sender, e) =>
            {
                deleteButton.BackColor = Color.FromArgb(82, 82, 82);
            };

            System.Windows.Forms.PictureBox favouriteButton = new System.Windows.Forms.PictureBox();
            favouriteButton.Size = new Size(32, 32);
            favouriteButton.BackColor = Color.FromArgb(82, 82, 82);
            favouriteButton.ForeColor = Color.White;
            if (favourite == 0)
            {
                favouriteButton.Image = heart_darktheme;
            }
            else
            {
                favouriteButton.Image = heart_darktheme_active;
            }
            favouriteButton.Location = new Point(1000, 14);
            favouriteButton.SizeMode = PictureBoxSizeMode.Zoom;

            GraphicsPath path6 = new GraphicsPath();
            path6.AddEllipse(-1, -2, favouriteButton.Width + 1, favouriteButton.Height);
            favouriteButton.Region = new Region(path6);

            favouriteButton.MouseEnter += (sender, e) =>
            {
                favouriteButton.BackColor = Color.FromArgb(95, 95, 95);
            };
            favouriteButton.MouseLeave += (sender, e) =>
            {
                favouriteButton.BackColor = Color.FromArgb(82, 82, 82);
            };







            favouriteButton.Click += (sender, e) =>
            {
                if (favourite == 0)
                {
                    favouriteButton.Image = heart_darktheme_active;

                    command.CommandText = "UPDATE sounds SET favourite = @favourite WHERE ID = @id;";
                    command.Parameters.AddWithValue("@favourite", 1);
                    command.Parameters.AddWithValue("@id", id);
                    favourite = 1;
                }
                else
                {
                    favouriteButton.Image = heart_darktheme;

                    command.CommandText = "UPDATE sounds SET favourite = @favourite WHERE ID = @id;";
                    command.Parameters.AddWithValue("@favourite", 0);
                    command.Parameters.AddWithValue("@id", id);
                    favourite = 0;
                }

                command.ExecuteNonQuery();
            };


            deleteButton.Click += (sender, e) =>
            {

                deleteButton.BackColor = Color.FromArgb(110, 110, 110);

                command.CommandText = "DELETE FROM sounds WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                waveOut2.Stop();
                waveOut2.Dispose();

                if (audioFileReader2 != null)
                {
                    audioFileReader2.Dispose();
                    audioFileReaderOutput2.Dispose();
                }

                File.Delete(path_sound);

                if (panel2.Visible == true)
                {
                    panel2.Visible = false;
                    flowLayoutPanel1.Size = new Size(flowLayoutPanel1.Size.Width, flowLayoutPanel1.Size.Height + 81);
                }

                Panel panelToRemove = panel;
                panelToRemove.Parent.Controls.Remove(panelToRemove);
                panelToRemove.Dispose();

                int totalHeightOfControls = 0;

                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    totalHeightOfControls += control.Height;
                }

                flowLayoutPanel1.AutoScrollMinSize = new Size(flowLayoutPanel1.Width - 17, totalHeightOfControls);
            };

            trackBarVolume.Scroll += (e) =>
            {
                txtVolume.Text = trackBarVolume.Value.ToString();
                volume_play = trackBarVolume.Value;
                if (audioFilePath2 == path_sound)
                {
                    volumeProvider.Volume = Math.Min(Convert.ToSingle(trackBarVolume.Value) * trackbar_Main_volume.Value / 10000, 25.0f);
                    volumeOutputProvider.Volume = Math.Min(Convert.ToSingle(trackBarVolume.Value) * track_bar_Output.Value / 10000, 25.0f);
                }
            };



            txtVolume.TextChanged += (sender, e) =>
            {
                if (txtVolume.Text == "")
                {
                    txtVolume.Text = "0";
                }
                if (Convert.ToInt32(txtVolume.Text) > 500)
                {
                    txtVolume.Text = "500";
                }

                trackBarVolume.Value = Convert.ToInt32(txtVolume.Text);
            };

            txtVolume.KeyUp += (sender, e) =>
            {
                command.CommandText = "UPDATE sounds SET Volume = @Volume WHERE ID = @id;";
                command.Parameters.AddWithValue("@Volume", trackBarVolume.Value);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            };

            trackBarVolume.MouseUp += (sender, e) =>
            {
                command.CommandText = "UPDATE sounds SET Volume = @Volume WHERE ID = @id;";
                command.Parameters.AddWithValue("@Volume", trackBarVolume.Value);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            };



            txtVolume.KeyPress += (object sender, KeyPressEventArgs e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            StringBuilder pressedKeys = new StringBuilder();
            StringBuilder pressedKeys_tester = new StringBuilder();

            txtKey.KeyDown += (object sender, System.Windows.Forms.KeyEventArgs e) =>
            {
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

                txtKey.Text = pressedKeys.ToString();
            };

            //txtKey.KeyUp += (object sender, System.Windows.Forms.KeyEventArgs e) =>
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
            //    txtKey.Text = pressedKeys.ToString();
            //};

            resetKeyButton.Click += (sender, e) =>
            {
                txtKey.Text = "";
                pressedKeys.Clear();
                pressedKeys_tester.Clear();
            };

            txtName.TextChanged += (sender, e) =>
            {
                if (txtName.Text != "")
                {
                    command.CommandText = "UPDATE sounds SET Name = @Name WHERE ID = @id;";
                    command.Parameters.AddWithValue("@Name", txtName.Text);
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                } 
            };

            txtName.Leave += (sender, e) =>
            {
                if (txtName.Text == "")
                {
                    txtName.Text = name;
                }
            };

            txtKey.TextChanged += (sender, e) =>
            {
                if (txtKey.Text != "")
                {
                    panel.Controls.Add(resetKeyButton);
                }
                else
                {
                    panel.Controls.Remove(resetKeyButton);
                }

                //panel.Controls.Remove(SaveButton);
                //panel.Controls.Add(SaveButton);

                if (textbox_keys.Text == txtKey.Text)
                {
                    txtKey.ForeColor = Color.Red;
                    textbox_keys.ForeColor = Color.Red;
                }
                else
                {
                    txtKey.ForeColor = Color.Gainsboro;
                    textbox_keys.ForeColor = Color.Gainsboro;

                    command.CommandText = "UPDATE sounds SET Key_tester = @Key_tester WHERE ID = @id;";
                    command.Parameters.AddWithValue("@Key_tester", pressedKeys_tester.ToString());
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();

                    command.CommandText = "UPDATE sounds SET Key = @Key WHERE ID = @id;";
                    command.Parameters.AddWithValue("@Key", txtKey.Text);
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }


                

                
            };

            txtKey.MouseEnter += (sender, e) =>
            {
                txtKey.BackColor = Color.FromArgb(90, 90, 90);
            };
            txtKey.MouseLeave += (sender, e) =>
            {
                txtKey.BackColor = Color.FromArgb(82, 82, 82);
            };

            txtName.MouseEnter += (sender, e) =>
            {
                txtName.BackColor = Color.FromArgb(90, 90, 90);
            };
            txtName.MouseLeave += (sender, e) =>
            {
                txtName.BackColor = Color.FromArgb(82, 82, 82);
            };


            //panel.Width = flowLayoutPanel1.Width - 24;
            //txtName.Width = flowLayoutPanel1.Width - 685;
            //txtKey.Location = new Point(flowLayoutPanel1.Width - 608, txtKey.Location.Y);
            //resetKeyButton.Location = new Point(flowLayoutPanel1.Width - 643, resetKeyButton.Location.Y);
            //roundButton.Location = new Point(flowLayoutPanel1.Width - 425, roundButton.Location.Y);
            //trackBarVolume.Location = new Point(flowLayoutPanel1.Width - 345, trackBarVolume.Location.Y);
            //txtVolume.Location = new Point(flowLayoutPanel1.Width - 155, txtVolume.Location.Y);
            //favouriteButton.Location = new Point(flowLayoutPanel1.Width - 105, favouriteButton.Location.Y);
            //deleteButton.Location = new Point(flowLayoutPanel1.Width - 55, deleteButton.Location.Y);
            //if (txtKey.Location.X >= 930)
            //{
            //    txtKey.Location = new Point(930, txtKey.Location.Y);
            //    resetKeyButton.Location = new Point(895, resetKeyButton.Location.Y);
            //}
            //if (roundButton.Location.X >= 1230)
            //{
            //    roundButton.Location = new Point(1230, roundButton.Location.Y);
            //}
            //if (trackBarVolume.Location.X >= 1430)
            //{
            //    trackBarVolume.Location = new Point(1430, trackBarVolume.Location.Y);
            //    txtVolume.Location = new Point(1620, txtVolume.Location.Y);
            //}
            //if (favouriteButton.Location.X >= 1730)
            //{
            //    favouriteButton.Location = new Point(1730, favouriteButton.Location.Y);
            //}
            //if (deleteButton.Location.X >= 1840)
            //{
            //    deleteButton.Location = new Point(1840, deleteButton.Location.Y);
            //}


            //FlowLayoutPanel1.SizeChanged += (sender, e) =>
            //{
            //    panel.Width = FlowLayoutPanel1.Width - 24;
            //    txtName.Width = FlowLayoutPanel1.Width - 685;
            //    txtKey.Location = new Point(FlowLayoutPanel1.Width - 608, txtKey.Location.Y);
            //    resetKeyButton.Location = new Point(FlowLayoutPanel1.Width - 643, resetKeyButton.Location.Y);
            //    roundButton.Location = new Point(FlowLayoutPanel1.Width - 425, roundButton.Location.Y);
            //    trackBarVolume.Location = new Point(FlowLayoutPanel1.Width - 345, trackBarVolume.Location.Y);
            //    txtVolume.Location = new Point(FlowLayoutPanel1.Width - 155, txtVolume.Location.Y);
            //    favouriteButton.Location = new Point(FlowLayoutPanel1.Width - 105, favouriteButton.Location.Y);
            //    deleteButton.Location = new Point(FlowLayoutPanel1.Width - 55, deleteButton.Location.Y);
            //    if (txtKey.Location.X >= 930)
            //    {
            //        txtKey.Location = new Point(930, txtKey.Location.Y);
            //        resetKeyButton.Location = new Point(895, resetKeyButton.Location.Y);
            //    }
            //    if (roundButton.Location.X >= 1230)
            //    {
            //        roundButton.Location = new Point(1230, roundButton.Location.Y);
            //    }
            //    if (trackBarVolume.Location.X >= 1430)
            //    {
            //        trackBarVolume.Location = new Point(1430, trackBarVolume.Location.Y);
            //        txtVolume.Location = new Point(1620, txtVolume.Location.Y);
            //    }
            //    if (favouriteButton.Location.X >= 1730)
            //    {
            //        favouriteButton.Location = new Point(1730, favouriteButton.Location.Y);
            //    }
            //    if (deleteButton.Location.X >= 1840)
            //    {
            //        deleteButton.Location = new Point(1840, deleteButton.Location.Y);
            //    }
            //};

            //flowLayoutPanel1.SizeChanged += (sender, e) =>
            //{ 
            //    if (flowLayoutPanel1.VerticalScroll.Visible == false)
            //    {
            //        panel.Size = new Size(flowLayoutPanel1.Width - 7, 60);
            //    }
            //    else
            //    {
            //        panel.Size = new Size(flowLayoutPanel1.Width - 24, 60);
            //    }
            //};









            roundButton.Click += (sender, e) =>
            {
                WaveOutEvent waveOut;
                WaveStream audioFileReader;

                WaveOutEvent waveOutput;
                WaveStream audioFileReaderOutput;

                waveOut = new WaveOutEvent();

                waveOutput = new WaveOutEvent();

                if (checkBox2.Switched == true)
                {
                    string audioFilePath;
                    string audioFilePathOutput;

                    int waveOutDeviceNumber = 0;
                    for (int i = 0; i < WaveOut.DeviceCount; i++)
                    {
                        string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                        string substring2 = "CABLE Input (VB-Audio Virtual Cable)".Substring(0, 15);
                        if (substring1.Equals(substring2))
                        {
                            waveOutDeviceNumber = i;
                            break;
                        }
                    }

                    int waveOutputDeviceNumber = 0;
                    for (int i = 0; i < WaveOut.DeviceCount; i++)
                    {
                        string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                        string substring2 = comboBox1_Output_devices.Text.Substring(0, 15);
                        if (substring1.Equals(substring2))
                        {
                            waveOutputDeviceNumber = i;
                            break;
                        }
                    }

                    waveOut.DeviceNumber = waveOutDeviceNumber;

                    waveOutput.DeviceNumber = waveOutputDeviceNumber;

                    audioFilePath = path_sound;
                    audioFilePathOutput = path_sound;


                    audioFileReader = new WaveFileReader(audioFilePath);
                    audioFileReaderOutput = new WaveFileReader(audioFilePathOutput);


                    if (waveOut.PlaybackState == PlaybackState.Stopped)
                    {

                        var volumeProvider1 = new VolumeSampleProvider(audioFileReader.ToSampleProvider());
                        volumeProvider1.Volume = Math.Min(volumeProvider1.Volume * trackBarVolume.Value * trackbar_Main_volume.Value / 10000, 25.0f);

                        waveOut.Init(volumeProvider1);


                        waveOut.Play();



                        var volumeOutputProvider1 = new VolumeSampleProvider(audioFileReaderOutput.ToSampleProvider());
                        volumeOutputProvider1.Volume = Math.Min(volumeOutputProvider1.Volume * trackBarVolume.Value * track_bar_Output.Value / 10000, 25.0f);

                        waveOutput.Init(volumeOutputProvider1);


                        waveOutput.Play();
                    }

                    else if (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        waveOut.Stop();
                        waveOut.Dispose();

                        var volumeProvider1 = new VolumeSampleProvider(audioFileReader.ToSampleProvider());
                        volumeProvider1.Volume = Math.Min(volumeProvider1.Volume * trackBarVolume.Value * trackbar_Main_volume.Value / 10000, 25.0f);

                        waveOut.Init(volumeProvider1);


                        waveOut.Play();

                        waveOutput.Stop();
                        waveOutput.Dispose();

                        var volumeOutputProvider1 = new VolumeSampleProvider(audioFileReaderOutput.ToSampleProvider());
                        volumeOutputProvider1.Volume = Math.Min(volumeOutputProvider1.Volume * trackBarVolume.Value * track_bar_Output.Value / 10000, 25.0f);

                        waveOutput.Init(volumeOutputProvider1);


                        waveOutput.Play();
                    }
                }

                else
                {

                    textBox1.Text = name;
                    textBox2.Text = key;

                    if (panel2.Visible == false)
                    {
                        panel2.Visible = true;
                        flowLayoutPanel1.Size = new Size(flowLayoutPanel1.Size.Width, flowLayoutPanel1.Size.Height - 81);
                    }

                    int waveOutDeviceNumber = 0;
                    for (int i = 0; i < WaveOut.DeviceCount; i++)
                    {
                        string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                        string substring2 = "CABLE Input (VB-Audio Virtual Cable)".Substring(0, 15);
                        if (substring1.Equals(substring2))
                        {
                            waveOutDeviceNumber = i;
                            break;
                        }
                    }

                    int waveOutputDeviceNumber = 0;
                    for (int i = 0; i < WaveOut.DeviceCount; i++)
                    {
                        string substring1 = WaveOut.GetCapabilities(i).ProductName.Substring(0, 15);
                        string substring2 = comboBox1_Output_devices.Text.Substring(0, 15);
                        if (substring1.Equals(substring2))
                        {
                            waveOutputDeviceNumber = i;
                            break;
                        }
                    }

                    waveOut2.DeviceNumber = waveOutDeviceNumber;

                    waveOutput2.DeviceNumber = waveOutputDeviceNumber;

                    audioFilePath2 = path_sound;
                    audioFilePathOutput2 = path_sound;

                    if (waveOut2.PlaybackState == PlaybackState.Stopped)
                    {

                        waveOut2.Dispose();
                        waveOutput2.Dispose();

                        if (audioFileReader2 != null)
                        {
                            audioFileReader2.Dispose();
                            audioFileReaderOutput2.Dispose();
                        }

                        audioFileReader2 = new WaveFileReader(audioFilePath2);
                        audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);
                        TimeSpan desiredTime = TimeSpan.FromMilliseconds(macTrackBar1.Value);
                        if (macTrackBar1.Value != macTrackBar1.Maximum && audioFilePath2_tester == audioFilePath2)
                        {
                            audioFileReader2.CurrentTime = desiredTime;
                            audioFileReaderOutput2.CurrentTime = desiredTime;
                        }

                        audioFilePath2_tester = audioFilePath2;

                        TimeSpan duration2 = audioFileReader2.TotalTime;
                        label8.Text = $"{duration2.Minutes}:{duration2.Seconds:00}";
                        macTrackBar1.Maximum = Convert.ToInt32(duration2.TotalMilliseconds);

                        volume_play = trackBarVolume.Value;

                        volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                        volumeProvider.Volume = Math.Min(volumeProvider.Volume * trackBarVolume.Value * trackbar_Main_volume.Value / 10000, 25.0f);

                        waveOut2.Init(volumeProvider);


                        waveOut2.Play();
                        if (updateTimer == null)
                        {
                            updateTimer = new System.Threading.Timer(UpdateTimer_Tick, null, 0, 100);
                        }

                        volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                        volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * trackBarVolume.Value * track_bar_Output.Value / 10000, 25.0f);

                        waveOutput2.Init(volumeOutputProvider);


                        waveOutput2.Play();
                    }

                    else if (waveOut2.PlaybackState == PlaybackState.Playing)
                    {
                        audioFilePath2_tester = audioFilePath2;

                        waveOut2.Stop();
                        waveOut2.Dispose();

                        if (audioFileReader2 != null)
                        {
                            audioFileReader2.Dispose();
                            audioFileReaderOutput2.Dispose();
                        }

                        audioFileReader2 = new WaveFileReader(audioFilePath2);
                        audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);

                        textBox1.Text = name;
                        textBox2.Text = key;

                        audioFileReader2 = new WaveFileReader(audioFilePath2);
                        audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);
                        TimeSpan duration2 = audioFileReader2.TotalTime;
                        label8.Text = $"{duration2.Minutes}:{duration2.Seconds:00}";
                        macTrackBar1.Maximum = Convert.ToInt32(duration2.TotalMilliseconds);

                        volume_play = trackBarVolume.Value;

                        volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                        volumeProvider.Volume = Math.Min(volumeProvider.Volume * trackBarVolume.Value * trackbar_Main_volume.Value / 10000, 25.0f);

                        waveOut2.Init(volumeProvider);


                        waveOut2.Play();

                        waveOutput2.Stop();
                        waveOutput2.Dispose();

                        volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                        volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * trackBarVolume.Value * track_bar_Output.Value / 10000, 25.0f);

                        waveOutput2.Init(volumeOutputProvider);


                        waveOutput2.Play();
                    }
                    else if (waveOut2.PlaybackState == PlaybackState.Paused && audioFilePath2_tester == audioFilePath2)
                    {
                        waveOut2.Play();

                        waveOutput2.Play();
                    }
                    else if (waveOut2.PlaybackState == PlaybackState.Paused && audioFilePath2_tester != audioFilePath2)
                    {
                        audioFilePath2_tester = audioFilePath2;

                        waveOut2.Dispose();
                        waveOutput2.Dispose();

                        if (audioFileReader2 != null)
                        {
                            audioFileReader2.Dispose();
                            audioFileReaderOutput2.Dispose();
                        }

                        audioFileReader2 = new WaveFileReader(audioFilePath2);
                        audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);
                        TimeSpan duration2 = audioFileReader2.TotalTime;
                        label8.Text = $"{duration2.Minutes}:{duration2.Seconds:00}";
                        macTrackBar1.Maximum = Convert.ToInt32(duration2.TotalMilliseconds);

                        volume_play = trackBarVolume.Value;

                        volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                        volumeProvider.Volume = Math.Min(volumeProvider.Volume * trackBarVolume.Value * trackbar_Main_volume.Value / 10000, 25.0f);

                        waveOut2.Init(volumeProvider);


                        waveOut2.Play();


                        volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                        volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * trackBarVolume.Value * track_bar_Output.Value / 10000, 25.0f);

                        waveOutput2.Init(volumeOutputProvider);

                        waveOutput2.Play();
                    }
                }
            };



            panel.Controls.Add(txtName);
            panel.Controls.Add(txtKey);
            panel.Controls.Add(trackBarVolume);
            panel.Controls.Add(txtVolume);
            panel.Controls.Add(roundButton);
            if (txtKey.Text != "")
            {
                panel.Controls.Add(resetKeyButton);
            }
            panel.Controls.Add(deleteButton);
            panel.Controls.Add(favouriteButton);

            return panel;
        }

        private void sounds_load()
        {
            flowLayoutPanel1.Controls.Clear();

            GC.Collect();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string select = "SELECT * FROM sounds WHERE Name LIKE '%' || @searchText || '%';";

                if (pictureBox3.Image == heart_darktheme_active)
                {
                    select = "SELECT * FROM sounds WHERE Name LIKE '%' || @searchText || '%' AND favourite = 1;";
                }

                if (comboBox1.Text == "Від нових до старих")
                {
                    if (pictureBox3.Image == heart_darktheme_active)
                    {
                        select = "SELECT * FROM sounds WHERE Name LIKE '%' || @searchText || '%' AND favourite = 1 ORDER BY ID DESC;";
                    }
                    else
                    {
                        select = "SELECT * FROM sounds WHERE Name LIKE '%' || @searchText || '%' ORDER BY ID DESC;";
                    }
                }

                if (comboBox1.Text == "За алфавітом")
                {
                    if (pictureBox3.Image == heart_darktheme_active)
                    {
                        select = "SELECT * FROM sounds WHERE Name LIKE '%' || @searchText || '%' AND favourite = 1 ORDER BY Name;";
                    }
                    else
                    {
                        select = "SELECT * FROM sounds WHERE Name LIKE '%' || @searchText || '%' ORDER BY Name;";
                    }
                }

                if (comboBox1.Text == "Проти алфавіту")
                {
                    if (pictureBox3.Image == heart_darktheme_active)
                    {
                        select = "SELECT * FROM sounds WHERE Name LIKE '%' || @searchText || '%' AND favourite = 1 ORDER BY Name DESC;";
                    }
                    else
                    {
                        select = "SELECT * FROM sounds WHERE Name LIKE '%' || @searchText || '%' ORDER BY Name DESC;";
                    }
                }


                using (SQLiteCommand command = new SQLiteCommand(select, connection))
                {
                    command.Parameters.AddWithValue("@searchText", name_sound.Text);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["ID"]);
                            string path_sound = reader["Path"].ToString();
                            string name = reader["Name"].ToString();
                            string key = reader["Key"].ToString();
                            int volume = Convert.ToInt32(reader["Volume"]);
                            int favourite = Convert.ToInt32(reader["favourite"]);

                            Panel panel = CreatePanel(id, path_sound, name, key, volume, favourite);

                            flowLayoutPanel1.Controls.Add(panel);
                        }
                    }
                }
            }


            int totalHeightOfControls = 0;

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                totalHeightOfControls += control.Height;
            }

            flowLayoutPanel1.AutoScrollMinSize = new Size(flowLayoutPanel1.Width - 17, totalHeightOfControls);
        }








        private Image darktheme_input_on;
        private Image darktheme_input_off;
        private Image darktheme_output_on;
        private Image darktheme_output_off;
        //private Image lighttheme_input_on;
        //private Image lighttheme_input_off;
        //private Image lighttheme_output_on;
        //private Image lighttheme_output_off;

        private Image pause_darkTheme = Properties.Resources.pause_darktheme;
        private Image play_darkTheme = Properties.Resources.play_darktheme;

        //private Image pause_lightTheme = Properties.Resources.pause_lighttheme;
        //private Image play_lightTheme = Properties.Resources.pause_lighttheme;

        private Image reset_darktheme = Properties.Resources.reset_darktheme;
        //private Image reset_lighttheme = Properties.Resources.reset_lighttheme;

        private Image delete_darktheme = Properties.Resources.delete_darktheme;
        //private Image delete_lighttheme = Properties.Resources.delete_lighttheme;

        private Image stop_darktheme = Properties.Resources.stop_darktheme;
        //private Image stop_lighttheme = Properties.Resources.stop_lighttheme;

        private Image search_darktheme = Properties.Resources.search_darktheme;
        //private Image search_lighttheme = Properties.Resources.search_lighttheme;

        private Image heart_darktheme = Properties.Resources.heart_darktheme;
        //private Image heart_lighttheme = Properties.Resources.heart_lighttheme;

        private Image heart_darktheme_active = Properties.Resources.heart_darktheme_active;

        //private Image save_darktheme = Properties.Resources.save_darktheme;
        //private Image save_lighttheme = Properties.Resources.save_lighttheme;
        private void Save_settings()
        {
            Properties.Settings.Default.Input_device = comboBox1_Input_devices.Text;
            Properties.Settings.Default.Output_device = comboBox1_Output_devices.Text;

            Properties.Settings.Default.Output_volume = track_bar_Output.Value;
            Properties.Settings.Default.Main_volume = trackbar_Main_volume.Value;

            Properties.Settings.Default.lock_volume = checkBox1.Switched;

            Properties.Settings.Default.slide = checkBox2.Switched;

            Properties.Settings.Default.key_pause = textbox_keys.Text;
            Properties.Settings.Default.key_pause_tester = pressedKeys_tester2.ToString();

            Properties.Settings.Default.Save();
        }
        private void Load_settings()
        {
            comboBox1_Input_devices.Text = Properties.Settings.Default.Input_device;
            comboBox1_Output_devices.Text = Properties.Settings.Default.Output_device;

            track_bar_Output.Value = Properties.Settings.Default.Output_volume;
            trackbar_Main_volume.Value = Properties.Settings.Default.Main_volume;

            checkBox1.Switched = Properties.Settings.Default.lock_volume;

            checkBox2.Switched = Properties.Settings.Default.slide;

            pressedKeys_tester2.Clear();
            pressedKeys_tester2.Append(Properties.Settings.Default.key_pause_tester);
            targetKeyString = Properties.Settings.Default.key_pause_tester;

            pressedKeys2.Clear();
            pressedKeys2.Append(Properties.Settings.Default.key_pause);
            textbox_keys.Text = Properties.Settings.Default.key_pause;

            if (track_bar_Input.Value > 0) { switch_input_volume.Image = darktheme_input_on; }
            if (track_bar_Input.Value == 0) { switch_input_volume.Image = darktheme_input_off; }
            if (track_bar_Output.Value > 0) { switch_output_volume.Image = darktheme_output_on; }
            if (track_bar_Output.Value == 0) { switch_output_volume.Image = darktheme_output_off; }
            if (trackbar_Main_volume.Value > 0) { switch_main_volume.Image = darktheme_output_on; }
            if (trackbar_Main_volume.Value == 0) { switch_main_volume.Image = darktheme_output_off; }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            name_sound.Width = filter_panel.Width - 797;
            pictureBox1.Location = new Point(filter_panel.Width - 763, pictureBox1.Location.Y);
            comboBox1.Location = new Point(filter_panel.Width - 704, comboBox1.Location.Y);
            pictureBox2.Location = new Point(filter_panel.Width - 509, pictureBox2.Location.Y);
            
            if (pictureBox1.Location.X >= 650)
            {
                pictureBox1.Location = new Point(650, pictureBox1.Location.Y);
                comboBox1.Location = new Point(709, comboBox1.Location.Y);
                pictureBox2.Location = new Point(904, pictureBox2.Location.Y);
            }  
        }

        private void macTrackBar1_MouseEnter(object sender, EventArgs e)
        {
            macTrackBar1.TrackerSize = new Size(16, 16);
            macTrackBar1.Location = new Point(macTrackBar1.Location.X, macTrackBar1.Location.Y - 8);
        }

        private void macTrackBar1_MouseLeave(object sender, EventArgs e)
        {
            macTrackBar1.Location = new Point(macTrackBar1.Location.X, macTrackBar1.Location.Y + 8);
            macTrackBar1.TrackerSize = new Size(0, 0);
        }

        public string ConvertToTimeString(int valueInSeconds)
        {
            valueInSeconds = valueInSeconds / 10;
            int minutes = valueInSeconds / 60;
            int seconds = valueInSeconds % 60;
            return $"{minutes}:{seconds.ToString("00")}";
        }
        private void macTrackBar1_ValueChanged(object sender, decimal value)
        {
            label7.Text = ConvertToTimeString(macTrackBar1.Value/100);
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.FromArgb(105, 105, 105);
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.FromArgb(96, 96, 96);
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.FromArgb(105, 105, 105);
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.FromArgb(96, 96, 96);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.FromArgb(110, 110, 110);

            if (waveOut2.PlaybackState == PlaybackState.Stopped)
            {
                waveOut2.Dispose();
                waveOutput2.Dispose();

                if (audioFileReader2 != null)
                {
                    audioFileReader2.Dispose();
                    audioFileReaderOutput2.Dispose();
                }

                audioFileReader2 = new WaveFileReader(audioFilePath2);
                audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);

                TimeSpan desiredTime = TimeSpan.FromMilliseconds(macTrackBar1.Value);
                if (macTrackBar1.Value != macTrackBar1.Maximum)
                {
                    audioFileReader2.CurrentTime = desiredTime;
                    audioFileReaderOutput2.CurrentTime = desiredTime;
                }

                volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                volumeProvider.Volume = Math.Min(volumeProvider.Volume * volume_play * trackbar_Main_volume.Value / 10000, 25.0f);

                waveOut2.Init(volumeProvider);


                waveOut2.Play();



                volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * volume_play * track_bar_Output.Value / 10000, 25.0f);

                waveOutput2.Init(volumeOutputProvider);


                waveOutput2.Play();
            }

            else if (waveOut2.PlaybackState == PlaybackState.Playing)
            {
                waveOut2.Stop();
                waveOut2.Dispose();

                if (audioFileReader2 != null)
                {
                    audioFileReader2.Dispose();
                    audioFileReaderOutput2.Dispose();
                }

                audioFileReader2 = new WaveFileReader(audioFilePath2);
                audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);

                volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                volumeProvider.Volume = Math.Min(volumeProvider.Volume * volume_play * trackbar_Main_volume.Value / 10000, 25.0f);

                waveOut2.Init(volumeProvider);


                waveOut2.Play();

                waveOutput2.Stop();
                waveOutput2.Dispose();

                volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * volume_play * track_bar_Output.Value / 10000, 25.0f);

                waveOutput2.Init(volumeOutputProvider);


                waveOutput2.Play();
            }
            else if (waveOut2.PlaybackState == PlaybackState.Paused)
            {
                waveOut2.Play();
                waveOutput2.Play();
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.FromArgb(110, 110, 110);

            waveOut2.Pause();

            waveOutput2.Pause();
        }

        private void checkBox2_SwitchedChanged(object sender)
        {
            if (panel2.Visible == true)
            {
                panel2.Visible = false;
                flowLayoutPanel1.Size = new Size(flowLayoutPanel1.Size.Width, flowLayoutPanel1.Size.Height + 81);
            }

            if (waveOut2 != null)
            {
                if (waveOut2.PlaybackState == PlaybackState.Playing)
                {
                    waveOut2.Stop();
                    waveOut2.Dispose();

                    waveOutput2.Stop();
                    waveOutput2.Dispose();

                    if (audioFileReader2 != null)
                    {
                        audioFileReader2.Dispose();
                        audioFileReaderOutput2.Dispose();
                    }
                }
            }
        }

        private bool isThumbHeld = false;
        private void macTrackBar1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            isThumbHeld = true;
        }

        private TimeSpan desiredTime;
        private bool isPlay = false;
        private void macTrackBar1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            isThumbHeld = false;
            macTrackBar1.BorderStyle = MACBorderStyle.None;
            if (waveOut2.PlaybackState == PlaybackState.Playing)
            {
                waveOut2.Stop();
                waveOut2.Dispose();
                waveOutput2.Stop();
                waveOutput2.Dispose();


                if (audioFileReader2 != null)
                {
                    audioFileReader2.Dispose();
                    audioFileReaderOutput2.Dispose();
                }

                audioFileReader2 = new WaveFileReader(audioFilePath2);
                audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);

                TimeSpan desiredTime = TimeSpan.FromMilliseconds(macTrackBar1.Value);
                audioFileReader2.CurrentTime = desiredTime;
                audioFileReaderOutput2.CurrentTime = desiredTime;

                volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                volumeProvider.Volume = Math.Min(volumeProvider.Volume * volume_play * trackbar_Main_volume.Value / 10000, 25.0f);

                waveOut2.Init(volumeProvider);


                volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * volume_play * track_bar_Output.Value / 10000, 25.0f);

                waveOutput2.Init(volumeOutputProvider);


                waveOut2.Play();
                waveOutput2.Play();
            }
            else
            {
                waveOut2.Stop();
                waveOut2.Dispose();
                waveOutput2.Stop();
                waveOutput2.Dispose();

                isPlay = true;

                if (audioFileReader2 != null)
                {
                    audioFileReader2.Dispose();
                    audioFileReaderOutput2.Dispose();
                }

                audioFileReader2 = new WaveFileReader(audioFilePath2);
                audioFileReaderOutput2 = new WaveFileReader(audioFilePathOutput2);

                desiredTime = TimeSpan.FromMilliseconds(macTrackBar1.Value);
                audioFileReader2.CurrentTime = desiredTime;
                audioFileReaderOutput2.CurrentTime = desiredTime;

                volumeProvider = new VolumeSampleProvider(audioFileReader2.ToSampleProvider());
                volumeProvider.Volume = Math.Min(volumeProvider.Volume * volume_play * trackbar_Main_volume.Value / 10000, 25.0f);

                waveOut2.Init(volumeProvider);


                volumeOutputProvider = new VolumeSampleProvider(audioFileReaderOutput2.ToSampleProvider());
                volumeOutputProvider.Volume = Math.Min(volumeOutputProvider.Volume * volume_play * track_bar_Output.Value / 10000, 25.0f);

                waveOutput2.Init(volumeOutputProvider);
            }
            
        }

        private StringBuilder pressedKeys2 = new StringBuilder();
        private static StringBuilder pressedKeys_tester2 = new StringBuilder();
        private void textbox_keys_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(!pressedKeys_tester2.ToString().Contains(e.KeyCode.ToString()))
            {
                if (pressedKeys_tester2.Length > 0)
                {
                    pressedKeys2.Append("+");
                    pressedKeys_tester2.Append("+");
                }

                if (e.KeyCode.ToString() == "ControlKey")
                {
                    pressedKeys2.Append("Ctrl");
                }
                else if (e.KeyCode.ToString() == "Escape")
                {
                    pressedKeys2.Append("Esc");
                }
                else if (e.KeyCode.ToString() == "Insert")
                {
                    pressedKeys2.Append("Ins");
                }
                else if (e.KeyCode.ToString() == "Oemtilde")
                {
                    pressedKeys2.Append("`");
                }
                else if (e.KeyCode.ToString() == "D1")
                {
                    pressedKeys2.Append("1");
                }
                else if (e.KeyCode.ToString() == "D2")
                {
                    pressedKeys2.Append("2");
                }
                else if (e.KeyCode.ToString() == "D3")
                {
                    pressedKeys2.Append("3");
                }
                else if (e.KeyCode.ToString() == "D4")
                {
                    pressedKeys2.Append("4");
                }
                else if (e.KeyCode.ToString() == "D5")
                {
                    pressedKeys2.Append("5");
                }
                else if (e.KeyCode.ToString() == "D6")
                {
                    pressedKeys2.Append("6");
                }
                else if (e.KeyCode.ToString() == "D7")
                {
                    pressedKeys2.Append("7");
                }
                else if (e.KeyCode.ToString() == "D8")
                {
                    pressedKeys2.Append("8");
                }
                else if (e.KeyCode.ToString() == "D9")
                {
                    pressedKeys2.Append("9");
                }
                else if (e.KeyCode.ToString() == "D0")
                {
                    pressedKeys2.Append("0");
                }
                else if (e.KeyCode.ToString() == "OemMinus")
                {
                    pressedKeys2.Append("-");
                }
                else if (e.KeyCode.ToString() == "Oemplus")
                {
                    pressedKeys2.Append("+");
                }
                else if (e.KeyCode.ToString() == "Return")
                {
                    pressedKeys2.Append("Enter");
                }
                else if (e.KeyCode.ToString() == "Delete")
                {
                    pressedKeys2.Append("Dlt");
                }
                else if (e.KeyCode.ToString() == "Next")
                {
                    pressedKeys2.Append("PageDown");
                }
                else if (e.KeyCode.ToString() == "Divide")
                {
                    pressedKeys2.Append("Num/");
                }
                else if (e.KeyCode.ToString() == "Multiply")
                {
                    pressedKeys2.Append("Num*");
                }
                else if (e.KeyCode.ToString() == "Subtract")
                {
                    pressedKeys2.Append("Num-");
                }
                else if (e.KeyCode.ToString() == "OemOpenBrackets")
                {
                    pressedKeys2.Append("[");
                }
                else if (e.KeyCode.ToString() == "Oem6")
                {
                    pressedKeys2.Append("]");
                }
                else if (e.KeyCode.ToString() == "NumPad7")
                {
                    pressedKeys2.Append("Num7");
                }
                else if (e.KeyCode.ToString() == "NumPad9")
                {
                    pressedKeys2.Append("Num9");
                }
                else if (e.KeyCode.ToString() == "NumPad8")
                {
                    pressedKeys2.Append("Num8");
                }
                else if (e.KeyCode.ToString() == "NumPad6")
                {
                    pressedKeys2.Append("Num6");
                }
                else if (e.KeyCode.ToString() == "NumPad5")
                {
                    pressedKeys2.Append("Num5");
                }
                else if (e.KeyCode.ToString() == "NumPad4")
                {
                    pressedKeys2.Append("Num4");
                }
                else if (e.KeyCode.ToString() == "NumPad3")
                {
                    pressedKeys2.Append("Num3");
                }
                else if (e.KeyCode.ToString() == "NumPad2")
                {
                    pressedKeys2.Append("Num2");
                }
                else if (e.KeyCode.ToString() == "NumPad2")
                {
                    pressedKeys2.Append("Num7");
                }
                else if (e.KeyCode.ToString() == "NumPad1")
                {
                    pressedKeys2.Append("Num1");
                }
                else if (e.KeyCode.ToString() == "NumPad0")
                {
                    pressedKeys2.Append("Num0");
                }
                else if (e.KeyCode.ToString() == "Add")
                {
                    pressedKeys2.Append("Num+");
                }
                else if (e.KeyCode.ToString() == "Decimal")
                {
                    pressedKeys2.Append("Num.");
                }
                else if (e.KeyCode.ToString() == "Capital")
                {
                    pressedKeys2.Append("CapsLock");
                }
                else if (e.KeyCode.ToString() == "Oem1")
                {
                    pressedKeys2.Append(";");
                }
                else if (e.KeyCode.ToString() == "Oem7")
                {
                    pressedKeys2.Append("'");
                }
                else if (e.KeyCode.ToString() == "Oem5")
                {
                    pressedKeys2.Append("\\");
                }
                else if (e.KeyCode.ToString() == "ShiftKey")
                {
                    pressedKeys2.Append("Shift");
                }
                else if (e.KeyCode.ToString() == "OemBackslash")
                {
                    pressedKeys2.Append("Backs\\");
                }
                else if (e.KeyCode.ToString() == "Oemcomma")
                {
                    pressedKeys2.Append(",");
                }
                else if (e.KeyCode.ToString() == "OemPeriod")
                {
                    pressedKeys2.Append(".");
                }
                else if (e.KeyCode.ToString() == "OemQuestion")
                {
                    pressedKeys2.Append("/");
                }
                else if (e.KeyCode.ToString() == "LWin")
                {
                    pressedKeys2.Append("Win");
                }
                else if (e.KeyCode.ToString() == "Menu")
                {
                    pressedKeys2.Append("Alt");
                }
                else if (e.KeyCode.ToString() == "LaunchMail")
                {
                    pressedKeys2.Append("Mail");
                }
                else if (e.KeyCode.ToString() == "MediaPlayPause")
                {
                    pressedKeys2.Append("PlayPause");
                }
                else if (e.KeyCode.ToString() == "MediaPreviousTrack")
                {
                    pressedKeys2.Append("PreviousTrack");
                }
                else if (e.KeyCode.ToString() == "MediaNextTrack")
                {
                    pressedKeys2.Append("NextTrack");
                }
                else if (e.KeyCode.ToString() == "LaunchApplication2")
                {
                    pressedKeys2.Append("Application2");
                }
                else
                {
                    pressedKeys2.Append(e.KeyCode.ToString());
                }

                pressedKeys_tester2.Append(e.KeyCode.ToString());
            }

            textbox_keys.Text = pressedKeys2.ToString();
        }

        private void textbox_keys_TextChanged(object sender, EventArgs e)
        {
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

                            if (key == textbox_keys.Text)
                            {
                                textbox_keys.ForeColor = Color.Red;

                                Properties.Settings.Default.key_pause_tester = pressedKeys_tester2.ToString();
                                targetKeyString = Properties.Settings.Default.key_pause_tester;

                                break;
                            }
                            else
                            {
                                Properties.Settings.Default.key_pause_tester = pressedKeys_tester2.ToString();
                                targetKeyString = Properties.Settings.Default.key_pause_tester;

                                textbox_keys.ForeColor = Color.Gainsboro;
                            }
                        }
                    }
                }
            }
        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            pictureBox6.BackColor = Color.FromArgb(84, 84, 84);
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            pictureBox6.BackColor = Color.FromArgb(77, 77, 77);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            pictureBox6.BackColor = Color.FromArgb(90, 90, 90);

            textbox_keys.Clear();

            pressedKeys2.Clear();
            pressedKeys_tester2.Clear();

            Properties.Settings.Default.key_pause_tester = pressedKeys_tester2.ToString();
        }
    }
}
