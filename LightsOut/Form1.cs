using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            // initialize the game plarform.
            logic = new LightsOut(5);
        }

        public LightsOut logic { get; set; }

        public bool WinnedTheGame { get; set; }

        // here we keep track of solutions that have been found for the game instance.
        public static List<Light[,]> notSolvedMatrixes = new List<Light[,]>();

        private void finishGame_Click(object sender, EventArgs e)
        {
            GameFinished();
            if(!WinnedTheGame)
                MessageBox.Show("Cmon! Just hit the lights!... Try harder!!!", "Try Again!");

            logic.ClearGame();

            ClearGame();
            
        }

        // we do initialize here because we dont want the winform to be accessible by the user when solution is calculated.
        private void Form1_Load(object sender, EventArgs e)
        {
            ClearGame();

            // attach event handlers for buttons on load, just once.
            foreach (var ctrl in tableLayoutPanel1.Controls.Cast<Button>())
            {
                // create handlers for each and every button found in tableLayoutPanel1
                ctrl.Click += new System.EventHandler(HandleClick);
            }
            

        }

        // Interaface only initialization
        private void ClearGame()
        {
            // switch all lights off!
            foreach (var ctrl in tableLayoutPanel1.Controls.Cast<Button>())
            {
                // set starting color switched off for lights
                ctrl.BackColor = Color.DarkOliveGreen;
            }
            WinnedTheGame = false;
        }


        private void HandleClick(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                var myControl = (sender as Control);
                var position = this.tableLayoutPanel1.GetPositionFromControl(myControl);
                textBox1.Text = position.ToString();
                Light light = logic.ToggleLight(position.Column, position.Row);
                HandleToggleInForm(light);
                if (IsSolved())
                {
                    Wins();
                }
            }

        }

        private void Wins()
        {
            MessageBox.Show("Congratulations! You 've made it! Thank you for playing.", "Winner!");
            this.startButton.Enabled = false;
            this.finishButton.Enabled = false;
            this.tableLayoutPanel1.Enabled = false;
        }


        /// <summary>
        /// After a toggle occurrs in the matrix of light, an appropriate mapping should occur in interaface of buttons.
        /// </summary>
        /// <param name="light"></param>
        protected void HandleToggleInForm(Light light)
        {
            // toggle self button
            ToggleLightToButton(light);

            // toggle adjacents
            ToggleLightToButton(light.Top);
            ToggleLightToButton(light.Bottom);
            ToggleLightToButton(light.Left);
            ToggleLightToButton(light.Right);

        }

        protected void ToggleLight(Button btn)
        {
            if (btn.BackColor == Color.DarkOliveGreen)
                btn.BackColor = Color.LightGreen;
            else
                btn.BackColor = Color.DarkOliveGreen;
        }

        protected void ToggleLight(Button btn, bool On)
        {
            if (On)
                btn.BackColor = Color.LightGreen;
            else
                btn.BackColor = Color.DarkOliveGreen;
        }

        // this is the way to map LightsAray to main Interface of winforms, GetControlFromPosition() gives the correct visible possition of
        // button in oder to match the Light in the Array to the Button in the interface.
        protected void ToggleLightToButton(Light light, bool init = false)
        {
            if (light == null)
                return;

            Control ctrl = tableLayoutPanel1.GetControlFromPosition(light.Row, light.Column);
            if (ctrl != null)
            {
                var btn = ctrl as Button;
                if (init)
                    ToggleLight(btn, (light.Switch == Switch.On ? true : false));
                else
                    ToggleLight(btn);
            }
        }

        protected void ToggleLightToButton(Light[,] Matrix)
        {
            foreach (var li in Matrix)
            {
                ToggleLightToButton(li, true);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!logic.Startup())
            {
                MessageBox.Show("Failed to propose a true solution for this super game...", "Game init failed!");
                ClearGame();
                return;
            }

            // toggle single one light to begin!
            ToggleLightToButton(logic.GetMatrix());

            GameStarted();
        }

        private void GameStarted()
        {
            this.startButton.Enabled = false;
            this.finishButton.Enabled = true;
            this.tableLayoutPanel1.Enabled = true;
        }

        private void GameFinished()
        {
            this.startButton.Enabled = true;
            this.finishButton.Enabled = false;
            this.tableLayoutPanel1.Enabled = false;
        }

        private bool IsSolved()
        {
            return logic.IsSolved();
        }

    }



}
