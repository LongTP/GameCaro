using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro
{
    class ChessBoard
    {
        //khai báo 2 ảnh để vẽ ảnh lên bàn cờ
        Image ImageO = new Bitmap(Properties.Resources.o);
        Image ImageX = new Bitmap(Properties.Resources.x);

        private int _soDong;

        public int SoDong
        {
            get { return _soDong; }
            set { _soDong = value; }
        }
        private int _soCot;

        public int SoCot
        {
            get { return _soCot; }
            set { _soCot = value; }
        }

        public ChessBoard()
        {
            _soCot = 0;
            _soDong = 0;
        }

        public ChessBoard(int SoDong, int SoCot)
        {
            _soCot = SoCot;
            _soDong = SoDong;
        }

        //vẽ bàn cờ
        public void veBanCo(Graphics g)
        {
            //vẽ cột
            for (int i = 0; i <= _soCot; i++)
            {
                g.DrawLine(CaroChess.pen, i * ChessPiece.CHIEU_RONG, 0, i * ChessPiece.CHIEU_RONG, _soDong * ChessPiece.CHIEU_CAO);
            }
            //vẽ dòng
            for (int i = 0; i <= _soDong; i++)
            {
                g.DrawLine(CaroChess.pen, 0, i * ChessPiece.CHIEU_CAO, _soCot * ChessPiece.CHIEU_RONG, i * ChessPiece.CHIEU_CAO);
            }
        }

        //vẽ quân cờ
        public void veQuanCo(Graphics g, int X, int Y, int SoHuu)
        {
            //quân xanh
            if (SoHuu == 1)
            {
                g.DrawImage(ImageO, X, Y);
                //g.FillEllipse(C_DieuKhien.sbBlack, X+2, Y+2, C_OCo.CHIEU_RONG-4, C_OCo.CHIEU_CAO-4);

            }
            else//quân đỏ
            {
                //g.FillEllipse(C_DieuKhien.sbWhite, X+2, Y+2, C_OCo.CHIEU_RONG-4, C_OCo.CHIEU_CAO-4);
                g.DrawImage(ImageX, X + 2, Y + 2);
            }
        }

        // Xóa quân cờ
        public void RemoveChess(Graphics g, Point point, SolidBrush sb)
        {
            g.FillRectangle(sb, point.X + 1, point.Y + 1, ChessPiece.CHIEU_CAO - 2, ChessPiece.CHIEU_CAO - 2);
        }
    }
}
