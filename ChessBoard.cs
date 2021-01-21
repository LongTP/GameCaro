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
        private int _NumOfLines;
        private int _NumOfColumns;

        public int NumOfLines
        {
            get
            {
                return _NumOfLines;
            }

            set
            {
                _NumOfLines = value;
            }
        }

        public int NumOfColumns
        {
            get
            {
                return _NumOfColumns;
            }

            set
            {
                _NumOfColumns = value;
            }
        }

        public ChessBoard()
        {
            NumOfColumns = 0;
            NumOfLines = 0;
        }
        public ChessBoard(int numOfLines, int numOFColumns)
        {
            NumOfColumns = numOFColumns;
            NumOfLines = numOfLines;
        }

        // Vẽ bàn cờ
        public void DrawChessBoard(Graphics g)
        {
            for (int i = 0; i <= NumOfColumns; i++)
            {
                g.DrawLine(CaroChess.pen, i * ChessPiece._Width, 0, i * ChessPiece._Width, ChessPiece._Height * NumOfLines);
            }
            for (int j = 0; j <= NumOfLines; j++)
            {
                g.DrawLine(CaroChess.pen, 0, j * ChessPiece._Height, ChessPiece._Width * NumOfColumns, j * ChessPiece._Height);
            }
        }

        // Vẽ quân cờ
        public void DrawChess(Graphics g, Point point, Image img)
        {
            g.DrawImage(img, point.X + 1, point.Y + 1, ChessPiece._Width - 2, ChessPiece._Height - 2);
        }

        // Xóa quân cờ
        public void RemoveChess(Graphics g, Point point, SolidBrush sb)
        {
            g.FillRectangle(sb, point.X + 1, point.Y + 1, ChessPiece._Width - 2, ChessPiece._Height - 2);
        }
    }
}
