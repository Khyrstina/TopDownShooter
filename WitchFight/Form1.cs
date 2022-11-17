
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
 
namespace WitchFight
    {
    public partial class Form1 : Form
        {

        bool goup; // allows player to go up
        bool godown; //allows player to go down screen
        bool goleft; // allows player to go left
        bool goright; //allows player to go right
        string facing = "up"; // this is used later to guide bullets
        double playerHealth = 100; // player health
        int heart = 0;//hearts start at zero
        int speed = 10; // this is player speed
        int ammo = 20; // starting amount of ammo
        int zombieSpeed = 1; // how fast the enemies move
        int score = 0; // how many "kills" player has accumulated
        bool gameOver = false; // false = game keeps going, true = game is finished
        Random rnd = new Random(); // random number generation


        public Form1()
            {
            InitializeComponent();
            button1.Enabled = false;
            button1.Visible = false;
            Thread.Sleep(2000);
            }

        private void keyisdown(object sender, KeyEventArgs e)
            {
            if (gameOver) return;

            // if the left key is pressed then...
            if (e.KeyCode == Keys.Left)
                {
                goleft = true; // change go left to true
                facing = "left"; //change facing to left
                player.Image = Properties.Resources.left; // change the player image to LEFT image

                }

            // if the right key is pressed then...
            if (e.KeyCode == Keys.Right)
                {
                goright = true; // change go right to true
                facing = "right"; // change facing to right
                player.Image = Properties.Resources.right; // change the player image to right

                }

            // if the up key is pressed then...
            if (e.KeyCode == Keys.Up)
                {
                facing = "up"; // change facing to up
                goup = true; // change go up to true
                player.Image = Properties.Resources.up; // change the player image to up

                }


            // if the down key is pressed then...
            if (e.KeyCode == Keys.Down)
                {
                facing = "down"; // change facing to down
                godown = true; // change go down to true
                player.Image = Properties.Resources.down; //change the player image to down

                }

            }

        private void keyisup(object sender, KeyEventArgs e)
            {
            if (gameOver) return; // only do this if gamover is false

            //when the key is released
            if (e.KeyCode == Keys.Left)
                {
                goleft = false; // change the go left boolean to false
                }

            // below is the key up selection for the right key
            if (e.KeyCode == Keys.Right)
                {
                goright = false; // change the go right boolean to false
                }
            // below is the key up selection for the up key
            if (e.KeyCode == Keys.Up)
                {
                goup = false; // change the go up boolean to false
                }
            // below is the key up selection for the down key
            if (e.KeyCode == Keys.Down)
                {
                godown = false; // change the go down boolean to false
                }

            //below is the key up selection for the space key
            if (e.KeyCode == Keys.Space && ammo > 0) // in this if statement we are checking if the space bar is up and ammo is more than 0
                {
                ammo--; // reduce ammo by 1 from the total number
                shoot(facing); // invoke the shoot function with the facing string inside it
                               //facing will transfer up, down, left or right to the function and that will shoot the bullet that way. 

                if (ammo < 1) // if ammo is less than 1
                    {
                    DropAmmo(); // invoke the drop ammo function
                    }
                }
            }

        private void gameEngine(object sender, EventArgs e)
            {
            if (playerHealth > 1) // if player health is greater than 1
                {
                progressBar1.Value = Convert.ToInt32(playerHealth); // assign the progress bar to the player health integer
                }
            else
                {
                // if the player health is below 1
                player.Image = Properties.Resources.dead; // show the player dead image
                timer1.Stop(); // stop the timer
                gameOver = true; // change game over to true
                button1.Enabled = true; // retry button appears
                button1.Visible = true;
                button1.BringToFront();
                }

            label1.Text = "   Ammo:  " + ammo; // show the ammo amount on label 1
            label2.Text = "Kills: " + score; // show the total kills on the score

            // if the player health is less than 20
            if (playerHealth > 50)
                {
                progressBar1.ForeColor = System.Drawing.Color.Green; // change the progress bar colour to red. 
                }
            else
                {
                progressBar1.ForeColor = System.Drawing.Color.DarkRed;
                }

            if (goleft && player.Left > 0)
                {
                player.Left -= speed;
                // if moving left is true AND pacman is 1 pixel more from the left 
                // then move the player to the LEFT
                }
            if (goright && player.Left + player.Width < 930)
                {
                player.Left += speed;
                // if moving RIGHT is true AND player left + player width is less than 930 pixels
                // then move the player to the RIGHT
                }
            if (goup && player.Top > 60)
                {
                player.Top -= speed;
                // if moving TOP is true AND player is 60 pixel more from the top 
                // then move the player to the UP
                }

            if (godown && player.Top + player.Height < 700)
                {
                player.Top += speed;
                // if moving DOWN is true AND player top + player height is less than 700 pixels
                // then move the player to the DOWN
                }

            // run the first for each loop below
            // X is a control and we will search for all controls in this loop
            foreach (Control x in this.Controls)
                {
                // if the X is a picture box and X has a tag AMMO
                if (x is PictureBox && x.Tag == "heart")
                    {
                    if (((PictureBox)x).Bounds.IntersectsWith(player.Bounds))
                        {
                        // once the player picks up the heart

                        this.Controls.Remove(((PictureBox)x)); // remove the heart picture box

                        ((PictureBox)x).Dispose(); // dispose the picture box completely from the program
                        playerHealth += 5; // add 5 health to the integer
                        }
                    }

                if (x is PictureBox && x.Tag == "ammo")
                    {
                    // check is X in hitting the player picture box

                    if (((PictureBox)x).Bounds.IntersectsWith(player.Bounds))
                        {
                        // once the player picks up the ammo

                        this.Controls.Remove(((PictureBox)x)); // remove the ammo picture box

                        ((PictureBox)x).Dispose(); // dispose the picture box completely from the program
                        ammo += 5; // add 5 ammo to the integer
                        }
                    }

                // if the bullets hits the 4 borders of the game
                // if x is a picture box and x has the tag of bullet

                if (x is PictureBox && x.Tag == "bullet")
                    {
                    // if the bullet is less the 1 pixel to the left
                    // if the bullet is more then 930 pixels to the right
                    // if the bullet is 10 pixels from the top
                    // if the bullet is 700 pixels to the buttom

                    if (((PictureBox)x).Left < 1 || ((PictureBox)x).Left > 930 || ((PictureBox)x).Top < 10 || ((PictureBox)x).Top > 700)
                        {
                        this.Controls.Remove(((PictureBox)x)); // remove the bullet from the display
                        ((PictureBox)x).Dispose(); // dispose the bullet from the program
                        }
                    }

                // below is the if statement which will be checking if the player hits a zombie

                if (x is PictureBox && x.Tag == "zombie")
                    {

                    // below is the if statament thats checking the bounds of the player and the zombie

                    if (((PictureBox)x).Bounds.IntersectsWith(player.Bounds))
                        {
                        playerHealth -= 1; // if the zombie hits the player then we decrease the health by 1
                        }

                    //move zombie towards the player picture box

                    if (((PictureBox)x).Left > player.Left)
                        {
                        ((PictureBox)x).Left -= zombieSpeed; // move zombie towards the left of the player
                        ((PictureBox)x).Image = Properties.Resources.zleft; // change the zombie image to the left
                        }

                    if (((PictureBox)x).Top > player.Top)
                        {
                        ((PictureBox)x).Top -= zombieSpeed; // move zombie upwards towards the players top
                        ((PictureBox)x).Image = Properties.Resources.zup; // change the zombie picture to the top pointing image
                        }
                    if (((PictureBox)x).Left < player.Left)
                        {
                        ((PictureBox)x).Left += zombieSpeed; // move zombie towards the right of the player
                        ((PictureBox)x).Image = Properties.Resources.zright; // change the image to the right image
                        }
                    if (((PictureBox)x).Top < player.Top)
                        {
                        ((PictureBox)x).Top += zombieSpeed; // move the zombie towards the bottom of the player
                        ((PictureBox)x).Image = Properties.Resources.zdown; // change the image to the down zombie
                        }
                    }

                // below is the second for loop, this is nexted inside the first one
                // the bullet and zombie needs to be different than each other
                // then we can use that to determine if the hit each other

                foreach (Control j in this.Controls)
                    {
                    // below is the selection thats identifying the bullet and zombie

                    if ((j is PictureBox && j.Tag == "bullet") && (x is PictureBox && x.Tag == "zombie"))
                        {
                        // below is the if statement thats checking if bullet hits the zombie
                        if (x.Bounds.IntersectsWith(j.Bounds))
                            {
                            score++; // increase the kill score by 1 
                            this.Controls.Remove(j); // this will remove the bullet from the screen
                            j.Dispose(); // this will dispose the bullet all together from the program
                            this.Controls.Remove(x); // this will remove the zombie from the screen
                            x.Dispose(); // this will dispose the zombie from the program
                            makeZombies(); // this function will invoke the make zombies function to add another zombie to the game
                            }
                        }
                    }
                }
            }

        private void DropAmmo()
            {
            // this function will make a ammo image for this game

            PictureBox ammo = new PictureBox(); // create a new instance of the picture box
            ammo.Image = Properties.Resources.ammo_image; // assignment the ammo image to the picture box
            ammo.SizeMode = PictureBoxSizeMode.AutoSize; // set the size to auto size
            ammo.Left = rnd.Next(10, 890); // set the location to a random left
            ammo.Top = rnd.Next(50, 600); // set the location to a random top
            ammo.Tag = "ammo"; // set the tag to ammo
            this.Controls.Add(ammo); // add the ammo picture box to the screen
            ammo.BringToFront(); // bring it to front
            player.BringToFront(); // bring the player to front
            }
        private void DropLife()
            {
            PictureBox heart = new PictureBox();
            heart.Image = Properties.Resources.heart;
            heart.SizeMode = PictureBoxSizeMode.AutoSize;
            heart.Left = rnd.Next(10, 890);
            heart.Top = rnd.Next(50, 600);
            heart.Tag = "heart";
            this.Controls.Add(heart);
            heart.BringToFront();
            player.BringToFront();
            }
        private void shoot(string direct)
            {
            // this is the function thats makes the new bullets in this game

            bullet shoot = new bullet(); // create a new instance of the bullet class
            shoot.direction = direct; // assignment the direction to the bullet
            shoot.bulletLeft = player.Left + (player.Width / 2); // place the bullet to left half of the player
            shoot.bulletTop = player.Top + (player.Height / 2); // place the bullet on top half of the player
            shoot.mkBullet(this); // run the function mkBullet from the bullet class. 
            }

        private void makeZombies()
            {
            // when this function is called it will make zombies in the game

            PictureBox zombie = new PictureBox(); // create a new picture box called zombie
            zombie.Tag = "zombie"; // renamed this to zombie
            zombie.Image = Properties.Resources.zdown; // the default picture for the zombie is zdown
            zombie.Left = rnd.Next(0, 900); // generate a number between 0 and 900 and assignment that to the new zombies left 
            zombie.Top = rnd.Next(0, 800); // generate a number between 0 and 800 and assignment that to the new zombies top
            zombie.SizeMode = PictureBoxSizeMode.AutoSize; 
            this.Controls.Add(zombie); // add the picture box to the screen
            player.BringToFront(); // bring the player to the front
            }

         void button1_Click(object sender, EventArgs e)
            {
            Application.Restart(); //Temporary fix for starting game over
            }
        }
    }