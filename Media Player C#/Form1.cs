using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;


namespace Media_Player_C_
{
    public partial class Form1 : Form
    {

        List<string> filteredFiles = new List<string>();
        FolderBrowserDialog browser = new FolderBrowserDialog();
        int currentFile = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoadFolderEvent(object sender, EventArgs e)
        {
            Player.Ctlcontrols.stop();

            if (filteredFiles.Count > 1) { 
                filteredFiles.Clear();
                filteredFiles = null;

                list.Items.Clear();

                currentFile = 0;
            }

            DialogResult result = browser.ShowDialog();

            if (result == DialogResult.OK)
            {
                filteredFiles = Directory.GetFiles(browser.SelectedPath, "*.*").Where(file => file.ToLower().EndsWith("webm")
                || file.ToLower().EndsWith("mp4") || file.ToLower().EndsWith("avi") || file.ToLower().EndsWith("wmv")
                || file.ToLower().EndsWith("mkv") || file.ToLower().EndsWith("mp3")).ToList();

                LoadPlaylist();
            }

        }

        private void ShowAboutEvent(object sender, EventArgs e)
        {
            MessageBox.Show("This app is made by Miranda Marques at https://github.com/OMirandinha" + Environment.NewLine + "Hope you have fun with my media player ;)");

        }

        private void PlayerStateChangeEvent(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 0)
            {
                Duration.Text = "Media Player is ready to be loaded";
            }
            else if (e.newState == 1)
            {
                Duration.Text = "Duration: " + Player.currentMedia.durationString;
            }
            else if (e.newState == 8)  
            {
                
                if (currentFile >= filteredFiles.Count - 1)
                {
                    currentFile = 0;  
                }
                else
                {
                    currentFile += 1;
                }

                list.SelectedIndex = currentFile;

                
                PlayFile(filteredFiles[currentFile]);  

                ShowFileName(fileName);
            }

            else if (e.newState == 9)
            {
                Duration.Text = "Loading new video";
            }

            else if (e.newState == 10)
            { 
                timer.Start();
            }
        }

        private void PlaylistChanged(object sender, EventArgs e)
        {
            currentFile = list.SelectedIndex;
         
            PlayFile(filteredFiles[currentFile]);  

            ShowFileName(fileName);
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            Player.Ctlcontrols.play();
            timer.Stop();
        }

        private void LoadPlaylist()
        {
            Player.currentPlaylist = Player.newPlaylist("Playlist", "");

            foreach (string videos in filteredFiles) 
            {
                Player.currentPlaylist.appendItem(Player.newMedia(videos));
                list.Items.Add(Path.GetFileName(videos));



            }

            if (filteredFiles.Count > 0)
            {
                fileName.Text = "Files Found" + filteredFiles.Count;

                list.SelectedIndex = currentFile;

                
                PlayFile(filteredFiles[currentFile]); 


            }

            else
            {
                MessageBox.Show("No Video Files Found in this folder");
            }


        }

        private void PlayFile(string url)
        {
            Player.URL = url;
        }

        private void ShowFileName(Label name)
        {
            string file = Path.GetFileName(list.SelectedItem.ToString());
            name.Text = "Currently Playing:" + file;
        }

      
        



    }
}
