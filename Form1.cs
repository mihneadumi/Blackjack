using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace BlackJack
{
    public partial class Form1 : Form
    {
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        bool muted;

        public Form1()
        {
            InitializeComponent();
            player.SoundLocation = "jazz.wav";
            player.Play();
            muted = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult iesire;
            iesire = MessageBox.Show("Esti sigur ca vrei sa iesi?", "Iesire", MessageBoxButtons.YesNo);

            if (iesire==DialogResult.Yes) Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            joc joc = new joc();
            this.Hide();
            joc.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!muted)
            {
                player.Stop();
                mute.BackgroundImage = new Bitmap("imagini/sound off.png");
                muted = true;
            }
            else
            {
                player.Play();
                mute.BackgroundImage = new Bitmap("imagini/sound on.png");
                muted = false;
            }
        }
    }
}
