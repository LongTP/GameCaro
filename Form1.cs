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

        //public static int cdStep = 100;
        //public static int cdTime = 15000;
        //public static int cdInterval = 100;
        public static int ChieuRongBanCo;
        public static int ChieuCaoBanCo;
        private Graphics grs;
        private CaroChess caroChess;
        

        public frmCaroChess()
        {
            InitializeComponent();
            //vẽ nên pnlBanCo
            grs = pnlChessBoard.CreateGraphics();

            //lấy chiều rộng và chiều cao pnBanCo để vẽ bàn cờ
            ChieuCaoBanCo = pnlChessBoard.Height;
            ChieuRongBanCo = pnlChessBoard.Width;

            //khởi tạo đối tượng điều khiển trò chơi
            caroChess = new CaroChess();
            //PvC.Click += PvC_Click;
            //btnComputer.Click += PvC_Click;
            exitToolStripMenuItem.Click += btnExit_Click;
           



        }

        private void PvC_Click(object sender, EventArgs e)
        {
            grs.Clear(pnlChessBoard.BackColor);
            caroChess.choiVoiMay(grs);
        }

        private void pnlGame_Paint(object sender, PaintEventArgs e)
        {
            if (caroChess.SanSang)
            {
                //vẽ bàn cờ
                caroChess.veBanCo(grs);
                //vẽ lại các quân cờ trong stack
                caroChess.veLaiQuanCo(grs);
            }
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
            if (caroChess.SanSang)
            {
                //chơi với người
                if (caroChess.CheDoChoi == 1)
                {
                    //đánh cờ với tọa độ chuột khi lick vào panel bàn cờ
                    caroChess.danhCo(grs, e.Location.X, e.Location.Y);
                    //sau khi đánh cờ thì kiểm tra chiến thắng luôn
                    caroChess.kiemTraChienThang(grs);
                }
                //chơi với máy
                else
                {
                    //người chơi đánh
                    caroChess.danhCo(grs, e.Location.X, e.Location.Y);
                    //kiểm tra người chơi chưa chiến thắng thì cho máy đánh
                    if (!caroChess.kiemTraChienThang(grs))
                    {
                        //máy đánh
                        caroChess.mayDanh(grs);
                        caroChess.kiemTraChienThang(grs);
                    }
                }
            }
            
        }

        //public void OtherPlayerMark(Point point)
        //{
        //    if (!caroChess.Ready)
        //        return;
        //    if (caroChess.PlayChess(point.X, point.Y, grs))
        //    {
        //        pnlChessBoard.Enabled = true;
        //        if (caroChess.CheckWin())
        //        {
        //            tmCoolDown.Stop();
        //            caroChess.EndGame();
        //        }
        //    }
        //}
        private void PvsP(object sender, EventArgs e)
        {
            grs.Clear(pnlChessBoard.BackColor);
            caroChess.choiVoiNguoi(grs);
           

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //caroChess.Undo(grs);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //caroChess.Redo(grs);
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
            
            
        }
        private void btnNewGame_Click(object sender, EventArgs e)
        {

            if (caroChess.CheDoChoi == 0)
            {
                MessageBox.Show("Chưa chọn chế độ chơi!", "Thông báo");
            }
            else if (caroChess.CheDoChoi == 1)
            {
                grs.Clear(pnlChessBoard.BackColor);
                caroChess.choiVoiNguoi(grs);
            }
            else if(caroChess.CheDoChoi == 2)
            {
                grs.Clear(pnlChessBoard.BackColor);
                caroChess.choiVoiMay(grs);
            }
            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.Show();
        }

       

        private void frmCaroChess_Shown(object sender, EventArgs e)
        {
            
        }

    }
}
