using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro
{
    class ChessPiece
    {
        public const int _Width = 25;
        public const int _Height = 25;

        private int _Row;

        public int Row
        {
            get
            {
                return _Row;
            }

            set
            {
                _Row = value;
            }
        }

        private int _Column;
        public int Column
        {
            get
            {
                return _Column;
            }

            set
            {
                _Column = value;
            }
        }

        private Point _Position;
        public Point Position
        {
            get
            {
                return _Position;
            }

            set
            {
                _Position = value;
            }
        }

        public int Owner
        {
            get
            {
                return _Owner;
            }

            set
            {
                _Owner = value;
            }
        }

        private int _Owner;

        public ChessPiece(int row, int column, Point pos, int owner)
        {
            _Row = row;
            _Column = column;
            _Position = pos;
            _Owner = owner;
        }

        public ChessPiece()
        {

        }
    }
}
