using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public partial class frmCaroChess : Form
    {

        public static int cdStep = 100;
        public static int cdTime = 15000;
        public static int cdInterval = 100;
        private CaroChess caroChess;
        private Graphics grs;
        
        public frmCaroChess()
        {
            InitializeComponent();
            caroChess = new CaroChess();
            caroChess.CreateChessPieces();
            grs = pnlChessBoard.CreateGraphics();
            PvC.Click += PvC_Click;
            btnComputer.Click += PvC_Click;
            exitToolStripMenuItem.Click += btnExit_Click;
            prcbCoolDown.Step = cdStep;
            prcbCoolDown.Maximum = cdTime;
            prcbCoolDown.Value = 0;
            tmCoolDown.Interval = cdInterval;


        }

        private void PvC_Click(object sender, EventArgs e)
        {
            grs.Clear(pnlChessBoard.BackColor);
            caroChess.StartPvC(grs);
            prcbCoolDown.Value = 0;
            tmCoolDown.Start();
        }

        private void pnlGame_Paint(object sender, PaintEventArgs e)
        {
            caroChess.DrawChessBoard(grs);
            caroChess.RepaintChess(grs);
        }

        private void frmCaroChess_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(58, 58, 60);
            btnComputer.BackColor = this.BackColor;
            btnNewGame.BackColor = this.BackColor;
            

            btn2Player.BackColor = this.BackColor;
            btnExit.BackColor = this.BackColor;
            pnlChessBoard.BackColor = Color.White;

        }

        private void pnlChessBoard_MouseClick(object sender, MouseEventArgs e)
        {
            if (!caroChess.Ready)
                return;
            if (caroChess.PlayChess(e.X, e.Y, grs))
            {
                if (caroChess.Mode == 1)
                {
                    if (caroChess.CheckWin())
                    {
                        tmCoolDown.Stop();
                        caroChess.EndGame();
                        return;
                    }
                }
                else if (caroChess.Mode == 2)
                {
                    caroChess.LaunchComputer(grs);
                    if (caroChess.CheckWin())
                    {
                        tmCoolDown.Stop();
                        caroChess.EndGame();
                        return;
                    }
                }
              
            }
            tmCoolDown.Start();
            prcbCoolDown.Value = 0;
        }

        public void OtherPlayerMark(Point point)
        {
            if (!caroChess.Ready)
                return;
            if (caroChess.PlayChess(point.X, point.Y, grs))
            {
                pnlChessBoard.Enabled = true;
                if (caroChess.CheckWin())
                {
                    tmCoolDown.Stop();
                    caroChess.EndGame();
                }
            }
        }
        private void PvsP(object sender, EventArgs e)
        {
            grs.Clear(pnlChessBoard.BackColor);
            caroChess.StartPvP(grs);
            prcbCoolDown.Value = 0;
            tmCoolDown.Start();

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            caroChess.Undo(grs);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            caroChess.Redo(grs);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dlr = MessageBox.Show("Do you want to exit!", "Exit", MessageBoxButtons.YesNo); ;

            if (dlr == DialogResult.Yes)
            {        
                Application.Exit();
            }


        }
        private void NewGame()
        {
            grs.Clear(pnlChessBoard.BackColor);
            
            tmCoolDown.Start();
            prcbCoolDown.Value = 0;
        }
        private void btnNewGame_Click(object sender, EventArgs e)
        {

            if (caroChess.Mode == 0)
            {
                MessageBox.Show("Chưa chọn chế độ chơi!", "Thông báo");
            }
            else if (caroChess.Mode == 1)
            {
                grs.Clear(pnlChessBoard.BackColor);
                caroChess.StartPvP(grs);
            }
            else if(caroChess.Mode == 2)
            {
                grs.Clear(pnlChessBoard.BackColor);
                caroChess.StartPvC(grs);
            }
            tmCoolDown.Start();
            prcbCoolDown.Value = 0;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.Show();
        }

       

        private void frmCaroChess_Shown(object sender, EventArgs e)
        {
            
        }
        void Listen()
        {

           
        }

        

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();

            if (prcbCoolDown.Value >= prcbCoolDown.Maximum)
            {
                tmCoolDown.Stop();
                caroChess.EndGame();

            }
        }

    
        
    }
}
