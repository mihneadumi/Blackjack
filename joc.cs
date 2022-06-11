using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack
{
    public partial class joc : Form
    {
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        bool muted;

        Random random = new Random();
        string[] cartea = new string[] {"2", "3", "4", "5", "6", "7", "8",
                                        "9", "10", "J", "Q", "K", "A",};
        string[] culoarea = new string[] { "C", "D", "H", "S" };

        int scorJucator = 0, scorAI = 0;
        int valJucator = 0, valAI = 0;
        int val;

        string hidden=""; //cartea ascunsa a computerului

        bool As;
        int AsJuc = 0, AsAI = 0;
        
        int nrCarteJuc = 3;
        int nrCarteAI = 3;//contoare sa vad care carte urmeaza sa primeasca
        
        //buton iesire
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        public joc()
        {
            InitializeComponent();
            player.SoundLocation = "jazz.wav";
        }

        private void PlusCarte_Click(object sender, EventArgs e)///Valabil doar la jucator
        {
            if (LoseCondition() == 0)
            {
                mutaCartiJuc();
                string c1 = "";
                c1 = CarteRandom();

                if (nrCarteJuc == 3) { carte3.Load(c1); carte3.Visible = true; }
                else if (nrCarteJuc == 4) { carte4.Load(c1); carte4.Visible = true; }
                else if (nrCarteJuc == 5) { carte5.Load(c1); carte5.Visible = true; }

                nrCarteJuc++;
                valJucator += val;
                if (As) AsJuc++;

                valManaJuc.Text = valJucator.ToString();
            }

            if(LoseCondition()!=0)
            {
                EndGame();
            }
        }

        void EndGame()
        {
            dealer1.Load(hidden);//apare cartea ai-ului si se updateaza valoarea
            valManaAi.Text = valAI.ToString();
            button1.Show();//apare butonul pt urm runda
            PlusCarte.Visible = false;
            EndTurn.Visible = false;
            Mesaj.Visible = true;


            if (valJucator < 22 && (valJucator > valAI || valAI > 21))
            {
                Mesaj.Text = "Bravo! Ai castigat runda asta.";
                scorJucator++;
            }
            else if (valAI == valJucator)
            {
                Mesaj.Text = "Egalitate, nu se primesc puncte.";
            }
            else
            {
                Mesaj.Text = "De data asta n-ai avut noroc...";
                scorAI++;
            }

            scorAItext.Text = scorAI.ToString();
            scorJuctext.Text = scorJucator.ToString();

            if (scorJucator == 10)
            {
                MessageBox.Show("Felicitari, ai batut o conserva de robot, foarte satisfacator, nu?",
                                "Bravo");
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();
            }
            else if(scorAI==10)
            {
                MessageBox.Show("Faptul ca nu ai reusit sa castigi e foarte ingrijorator sincer...",
                                "Nu pot sa cred!");
                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();
            }
        }
        /// Astea is pt mutat panel-urile cu carti
        /// 
        void mutaCartiJuc()
        {
            Cartijuc.Location = new Point(Cartijuc.Location.X + 60, Cartijuc.Location.Y);
        }
        void JucMutaLaloc()
        {
            Cartijuc.Location = new Point(108, 350);
        }

        void mutaCartiAI()
        {
            CartiAI.Location = new Point(CartiAI.Location.X + 60, CartiAI.Location.Y);
        }
        void AIMutaLaLoc()
        {
            CartiAI.Location = new Point(108, 12);
        }
        /// 
        /// se termina astea de mutat
        bool condAI()//Conditia de baza a jocului
        {
            if (valAI <17 && valAI<=valJucator) return true;
            else return false;
        }

        void AiTurn()
        {
            while (condAI())//Computerul joaca dupa reguli predefinite, deci nu e in tocmai "AI" propriu zis
            {
                System.Threading.Thread.Sleep(1000);
                mutaCartiAI();
                string c1 = "";//string aux
                c1 = CarteRandom();

                if (nrCarteAI == 3) { dealer3.Load(c1); dealer3.Visible = true; }
                else if (nrCarteAI == 4) { dealer4.Load(c1); dealer4.Visible = true; }
                else if (nrCarteAI == 5) { dealer5.Load(c1); dealer5.Visible = true; }
                
                nrCarteAI++;
                valAI += val;
                if (As) AsAI++;
                valManaAi.Text = valAI.ToString();               
            }
            EndGame();
        }

        private void EndTurn_Click(object sender, EventArgs e)
        {
            dealer1.Load(hidden);
            valManaAi.Text = valAI.ToString();

            EndTurn.Visible = false;
            PlusCarte.Visible = false;
            AiTurn();
        }

        private void mute_Click(object sender, EventArgs e)///Se cam explica singura functia asta
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

        int LoseCondition()///Valabil doar la jucator
        {
            if (valJucator == 21) return 1;
            else if (valJucator > 21)
            {
                if (AsJuc > 0)
                {
                    AsJuc--;//Daca mana depaseste 21 transforma ca valoare
                    valJucator -= 10; // asul din 11 in 1 punct (dupa reguli)
                }
                else return -1; //daca mana depaseste 21 si nu are un as de transformat, jucatorul pierde
            }
            return 0;//valoare returnata daca jucatorul poate continua jocul
        }

        string CarteRandom()
        {
            As = false;

            int card = random.Next(0, 12);// ia o carte random din cele 13 posibile
            
            int color = random.Next(0, 3); //ia o culoare random din cele 4 posibile
            string carte = "imagini/" + cartea[card] + culoarea[color] + ".jpg";//formez adresa pt poza cartii
            val = card+2;//valoarea care iese ca sa nu o scot din numele cartii
            if (val > 10 && val != 14) val = 10;
            else if (val == 14)
            {
                val = 11;//calcule nebune pt valoarea cartilor
                As = true;
            }
            return (carte);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ///
            carte1.Visible = true;
            carte2.Visible = true;
            carte3.Visible = false;
            carte4.Visible = false;
            carte5.Visible = false;
            //apar chestii pe aici cand incepe jocul
            dealer1.Visible = true;
            dealer2.Visible = true;
            dealer3.Visible = false;
            dealer4.Visible = false;
            dealer5.Visible = false;
            dealer1.Load("imagini/Green_back.jpg");
            valManaAi.Text = "?";
            pachet.Visible = true;
            if (tabel.Visible == false)
            {
                tabel.Visible = true;//apare scorul
            }
            TextValJuc.Visible = true;
            valManaJuc.Visible = true;
            TextValAi.Visible = true;
            valManaAi.Visible = true;
            ///

            Mesaj.Visible = false;//dispare mesajul de win/lose
            button1.Visible = false;//dispare butonul de start joc
            button1.Text = "Incepe a runda noua";

            PlusCarte.Visible = true;
            EndTurn.Visible = true;
            JucMutaLaloc();
            AIMutaLaLoc();
            //Astea reseteaza interfata dupa fiecare runda
            nrCarteJuc = 3;
            nrCarteAI = 3;

            valJucator = 0;
            valAI = 0;
            string c1 = "";///string uri auxiliare pt carti
            string c2 = "";

            ///Initializare Jucator
            c1 = CarteRandom();
            carte1.Load(c1);
            valJucator += val;
            if (As) AsJuc++;

            c2 = CarteRandom();
            carte2.Load(c2);
            valJucator += val;
            if (As) AsJuc++;

            valManaJuc.Text = valJucator.ToString();
            ///

            ///Initializare AI
            hidden = CarteRandom(); ///Prima carte e generata dar nu se va incarca
            valAI += val;///decat la finalul turei jucatorului (dupa regulile jocului)
            if (As) AsAI++;

            c2 = CarteRandom();
            dealer2.Load(c2);
            valAI += val;
            if (As) AsAI++;
            ///
        }
    }
}
