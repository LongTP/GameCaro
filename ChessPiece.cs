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
        public const int CHIEU_RONG = 25;
        public const int CHIEU_CAO = 25;

        private int _dong;

        public int Dong
        {
            get { return _dong; }
            set { _dong = value; }
        }
        private int _cot;

        public int Cot
        {
            get { return _cot; }
            set { _cot = value; }
        }
        private int _soHuu;

        public int SoHuu
        {
            get { return _soHuu; }
            set { _soHuu = value; }
        }

        public ChessPiece(int Dong, int Cot, int SoHuu)
        {
            _dong = Dong;
            _cot = Cot;
            _soHuu = SoHuu;
        }

        public ChessPiece()
        {

        }
    }
}
