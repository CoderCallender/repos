using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Timers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Audio_Player
{
    public partial class Form1 : Form
    {
        SoundPlayer player = new SoundPlayer();
        private static System.Timers.Timer pauseTimer;
        private static System.Timers.Timer playTimer;

        public Form1()
        {
            InitializeComponent();
            waitUpDown.Minimum = 1;
            playUpDown.Minimum = 100;

            waitUpDown.Maximum = 120;
            playUpDown.Maximum = 1000;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\Users\Chase\Desktop",
                Title = "Browse wav Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "wav",
                Filter = "wav files (*.wav)|*.wav",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileTextBox.Text = openFileDialog1.FileName;
            }

            else
            {
                MessageBox.Show("uh - oh!");
                return;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            
            try
            {
                // Assign the selected file's path to 
                // the SoundPlayer object.  
                player.SoundLocation = fileTextBox.Text;

                // Load the .wav file.
                player.Load();
            }
            catch (Exception ex)
            {
                // ReportStatus(ex.Message);
                MessageBox.Show("File load error");
                return;
            }

            startButton.Enabled = false;
            stopButton.Enabled = true;
            waitUpDown.Enabled = false;
            playUpDown.Enabled = false;

            //load timer for play length
            playTimer = new System.Timers.Timer();
            playTimer.Interval = Convert.ToInt32(playUpDown.Value);  
            playTimer.Elapsed += playEndEvent;
            playTimer.AutoReset = false;

            //load timer for pause length
            pauseTimer = new System.Timers.Timer();
            pauseTimer.Interval = Convert.ToInt32(waitUpDown.Value) * 1000; //time is given in milliseconds
            pauseTimer.Elapsed += pauseEndEvent;
            pauseTimer.AutoReset = false;
            pauseTimer.Enabled = false;

            // Start the timer
            playTimer.Enabled = true;

            statusLabel.Text = "Status: Playing";
            player.PlayLooping();
        }

        //time has come to pause the audio 
        private void playEndEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("The play event was raised at {0}", e.SignalTime);
            //stop audio
            player.Stop();
            //start pause length timer
            pauseTimer.Enabled = true;
            playTimer.Enabled = false;
        }

        //time has come to play the audio 
        private void pauseEndEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("The pause event was raised at {0}", e.SignalTime);
            //play audio
            player.PlayLooping();
            //start play length timer
            pauseTimer.Enabled = false;
            playTimer.Enabled = true;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            pauseTimer.Enabled = false;
            playTimer.Enabled = false;
            player.Stop();
            startButton.Enabled = true;
            stopButton.Enabled = false;
            waitUpDown.Enabled = true;
            playUpDown.Enabled = true;
            statusLabel.Text = "Status: Paused";
        }
    }
}
