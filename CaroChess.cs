using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{

    class CaroChess
    {
        public enum END
        {
            Draw,
            Player1,
            Player2
        }
        private END _end;
        public static Pen pen;
        public static SolidBrush sbPnl;
        private ChessPiece[,] arrChessPiece;
        private ChessBoard chessBoard;
        private Stack<ChessPiece> stkChessUsed;
        private Stack<ChessPiece> stkChessUndo;

        private int turn;
        private bool ready;

        private int mode;
        // mode = 1 => PvP 
        // mode = 2 => PvC

        private Image ImageO = new Bitmap(Properties.Resources.o);
        private Image ImageX = new Bitmap(Properties.Resources.x);
        public bool Ready
        {
            get { return ready; }
            set { ready = value; }
        }

        public int Mode
        {
            get
            {
                return mode;
            }
        }

        public CaroChess()
        {
            pen = new Pen(Color.Black);
            sbPnl = new SolidBrush(Color.White);
            chessBoard = new ChessBoard(20, 20);
            arrChessPiece = new ChessPiece[chessBoard.NumOfLines, chessBoard.NumOfColumns];
            stkChessUsed = new Stack<ChessPiece>();
            stkChessUndo = new Stack<ChessPiece>();
            turn = 1;
        }
        // Vẽ bàn cờ
        public void DrawChessBoard(Graphics g)
        {
            chessBoard.DrawChessBoard(g);
        }

        // Tạo bàn cờ bằng mảng 2 chiều
        public void CreateChessPieces()
        {
            for (int i = 0; i < chessBoard.NumOfLines; i++)
            {
                for (int j = 0; j < chessBoard.NumOfColumns; j++)
                {
                    arrChessPiece[i, j] = new ChessPiece(i, j, new Point(j * ChessPiece._Width, i * ChessPiece._Height), 0);
                }
            }
        }
        // Phương thức đánh cờ
        public bool PlayChess(int mouseX, int mouseY, Graphics g)
        {
            if (mouseX % ChessPiece._Width == 0 || mouseY % ChessPiece._Height == 0)
                return false;
            int column = mouseX / ChessPiece._Width;
            int row = mouseY / ChessPiece._Height;

            if (arrChessPiece[row, column].Owner != 0)
                return false;
            switch (turn)
            {
                case 1:
                    arrChessPiece[row, column].Owner = 1;
                    chessBoard.DrawChess(g, arrChessPiece[row, column].Position, ImageX);
                    turn = 2;
                    break;
                case 2:
                    arrChessPiece[row, column].Owner = 2;
                    chessBoard.DrawChess(g, arrChessPiece[row, column].Position, ImageO);
                    turn = 1;
                    break;
                default:
                    MessageBox.Show("Error!!");
                    break;
            }
            stkChessUndo = new Stack<ChessPiece>();
            ChessPiece tmp = arrChessPiece[row, column];
            ChessPiece cp = new ChessPiece(tmp.Row, tmp.Column, tmp.Position, tmp.Owner);
            stkChessUsed.Push(cp);

            return true;
        }



        public void RepaintChess(Graphics g)
        {
            foreach (ChessPiece cp in stkChessUsed)
            {
                if (cp.Owner == 1)
                    chessBoard.DrawChess(g, cp.Position, ImageX);
                else if (cp.Owner == 2)
                    chessBoard.DrawChess(g, cp.Position, ImageO);

            }
        }

        public void StartPvP(Graphics g)
        {
            ready = true;
            stkChessUsed = new Stack<ChessPiece>();
            stkChessUndo = new Stack<ChessPiece>();
            turn = 1;
            mode = 1;
            CreateChessPieces();
            DrawChessBoard(g);
        }

        public void StartPvC(Graphics g)
        {
            ready = true;
            stkChessUsed = new Stack<ChessPiece>();
            stkChessUndo = new Stack<ChessPiece>();
            turn = 1;
            mode = 2;
            CreateChessPieces();
            DrawChessBoard(g);
            LaunchComputer(g);
        }


        #region Undo, Redo
        public void Undo(Graphics g)
        {
            if (mode == 1)
            {
                if (stkChessUsed.Count != 0)
                {
                    if (turn == 1)
                        turn = 2;
                    else if (turn == 2)
                        turn = 1;
                    ChessPiece cp = stkChessUsed.Pop();
                    stkChessUndo.Push(new ChessPiece(cp.Row, cp.Column, cp.Position, cp.Owner));
                    arrChessPiece[cp.Row, cp.Column].Owner = 0;
                    chessBoard.RemoveChess(g, cp.Position, sbPnl);
                }
                else
                    MessageBox.Show("Bạn chưa đánh nước cờ nào!!");
            }
            else if (mode == 2)
            {
                if (stkChessUsed.Count != 0)
                {
                    ChessPiece cpC = stkChessUsed.Pop();
                    ChessPiece cpP = stkChessUsed.Pop();
                    stkChessUndo.Push(new ChessPiece(cpP.Row, cpP.Column, cpP.Position, cpP.Owner));
                    stkChessUndo.Push(new ChessPiece(cpC.Row, cpC.Column, cpC.Position, cpC.Owner));
                    arrChessPiece[cpP.Row, cpP.Column].Owner = 0;
                    arrChessPiece[cpC.Row, cpC.Column].Owner = 0;
                    chessBoard.RemoveChess(g, cpP.Position, sbPnl);
                    chessBoard.RemoveChess(g, cpC.Position, sbPnl);
                }
                else
                    MessageBox.Show("Bạn chưa đánh nước cờ nào!!");
            }
        }
        public void Redo(Graphics g)
        {
            if (mode == 1)
            {
                if (stkChessUndo.Count != 0)
                {
                    if (turn == 1)
                        turn = 2;
                    else if (turn == 2)
                        turn = 1;
                    ChessPiece cp = stkChessUndo.Pop();
                    stkChessUsed.Push(new ChessPiece(cp.Row, cp.Column, cp.Position, cp.Owner));
                    arrChessPiece[cp.Row, cp.Column].Owner = cp.Owner;
                    chessBoard.DrawChess(g, cp.Position, cp.Owner == 1 ? ImageX : ImageO);
                }
            }
            else if (mode == 2)
            {
                if (stkChessUndo.Count != 0)
                {
                    ChessPiece cpC = stkChessUndo.Pop();
                    ChessPiece cpP = stkChessUndo.Pop();
                    stkChessUsed.Push(new ChessPiece(cpP.Row, cpP.Column, cpP.Position, cpP.Owner));
                    stkChessUsed.Push(new ChessPiece(cpC.Row, cpC.Column, cpC.Position, cpC.Owner));
                    arrChessPiece[cpC.Row, cpC.Column].Owner = cpC.Owner;
                    arrChessPiece[cpP.Row, cpP.Column].Owner = cpP.Owner;
                    chessBoard.DrawChess(g, cpP.Position, cpP.Owner == 1 ? ImageX : ImageO);
                    chessBoard.DrawChess(g, cpC.Position, cpC.Owner == 1 ? ImageX : ImageO);
                }
            }
        }
        #endregion

        #region Check Winner
        public void EndGame()
        {
            switch (mode)
            {
                case 1:
                    switch (_end)
                    {
                        case END.Draw:
                            MessageBox.Show("Time out");
                            break;
                        case END.Player1:
                            MessageBox.Show("Player 1 win!!");
                            break;
                        case END.Player2:
                            MessageBox.Show("Player 2 win!!");
                            break;


                    }
                    break;
                case 2:
                    switch (_end)
                    {
                        case END.Draw:
                            MessageBox.Show("Time out");
                            break;
                        case END.Player1:
                            MessageBox.Show("Computer win!!");
                            break;
                        case END.Player2:
                            MessageBox.Show("You win!!");
                            break;

                    }
                    break;
                case 3:
                    switch (_end)
                    {
                        case END.Draw:
                            MessageBox.Show("Time out");
                            break;
                        case END.Player1:
                            MessageBox.Show("Player 1 win!!");
                            break;
                        case END.Player2:
                            MessageBox.Show("Player 2 win!!");
                            break;
                        default:
                            MessageBox.Show("End Game");
                            break;

                    }
                    break;
            }

            ready = false;
        }
        public bool CheckWin()
        {
            if (stkChessUsed.Count == chessBoard.NumOfColumns * chessBoard.NumOfLines)
            {
                _end = END.Draw;
                return true;
            }
            foreach (ChessPiece cp in stkChessUsed)
            {
                if (CheckVertical(cp.Row, cp.Column, cp.Owner) || CheckHorizontal(cp.Row, cp.Column, cp.Owner) || CheckCross(cp.Row, cp.Column, cp.Owner) || CheckCrossBackwards(cp.Row, cp.Column, cp.Owner))
                {
                    _end = cp.Owner == 1 ? END.Player1 : END.Player2;
                    return true;
                }
            }
            return false;
        }

        private bool CheckVertical(int currRow, int currColumn, int currOwner)
        {
            if (currRow > chessBoard.NumOfLines - 5)
                return false;
            int count;
            for (count = 1; count < 5; count++)
            {
                if (arrChessPiece[currRow + count, currColumn].Owner != currOwner)
                    return false;
            }
            if (currRow == 0 || currRow + count == chessBoard.NumOfLines)
                return true;
            if (arrChessPiece[currRow - 1, currColumn].Owner == 0 || arrChessPiece[currRow + count, currColumn].Owner == 0)
                return true;
            return false;
        }

        private bool CheckHorizontal(int currRow, int currColumn, int currOwner)
        {
            if (currColumn > chessBoard.NumOfColumns - 5)
                return false;
            int count;
            for (count = 1; count < 5; count++)
            {
                if (arrChessPiece[currRow, currColumn + count].Owner != currOwner)
                    return false;
            }
            if (currColumn == 0 || currColumn + count == chessBoard.NumOfColumns)
                return true;
            if (arrChessPiece[currRow, currColumn - 1].Owner == 0 || arrChessPiece[currRow, currColumn + count].Owner == 0)
                return true;
            return false;
        }

        private bool CheckCross(int currRow, int currColumn, int currOwner)
        {
            if (currRow > chessBoard.NumOfLines - 5 || currColumn > chessBoard.NumOfColumns - 5)
                return false;
            int count;
            for (count = 1; count < 5; count++)
            {
                if (arrChessPiece[currRow + count, currColumn + count].Owner != currOwner)
                    return false;
            }
            if (currColumn == 0 || currColumn + count == chessBoard.NumOfColumns || currRow == 0 || currRow + count == chessBoard.NumOfLines)
                return true;
            if (arrChessPiece[currRow - 1, currColumn - 1].Owner == 0 || arrChessPiece[currRow + count, currColumn + count].Owner == 0)
                return true;
            return false;
        }

        private bool CheckCrossBackwards(int currRow, int currColumn, int currOwner)
        {
            if (currRow < 4 || currColumn > chessBoard.NumOfColumns - 5)
                return false;
            int count;
            for (count = 1; count < 5; count++)
            {
                if (arrChessPiece[currRow - count, currColumn + count].Owner != currOwner)
                    return false;
            }
            if (currRow == 4 || currRow == chessBoard.NumOfLines - 1 || currColumn == 0 || currColumn + count == chessBoard.NumOfColumns)
                return true;
            if (arrChessPiece[currRow + 1, currColumn - 1].Owner == 0 || arrChessPiece[currRow - count, currColumn + count].Owner == 0)
                return true;
            return false;
        }

        #endregion

        #region AI Computer
        private long[] AttackPoint = new long[7] { 0, 9, 54, 162, 1458, 13112, 118008 };
        private long[] DefensePoint = new long[7] { 0, 3, 27, 99, 729, 6561, 59049 };
        public void LaunchComputer(Graphics g)
        {
            if (stkChessUsed.Count == 0)
            {
                PlayChess(chessBoard.NumOfColumns / 2 * ChessPiece._Height + 1, chessBoard.NumOfLines / 2 * ChessPiece._Width + 1, g);
            }
            else
            {

                ChessPiece cp = FindMove();
                PlayChess(cp.Position.X + 1, cp.Position.Y + 1, g);

            }
        }
        private ChessPiece FindMove()
        {
            ChessPiece cpResult = new ChessPiece();
            long maxPoint = 0;
            for (int i = 0; i < chessBoard.NumOfLines; i++)
            {
                for (int j = 0; j < chessBoard.NumOfColumns; j++)
                {
                    if (arrChessPiece[i, j].Owner == 0)
                    {
                        long attackkPoint = AtkPoint_CheckHorizontal(i, j) + AtkPoint_CheckVertical(i, j) + AtkPoint_CheckCross(i, j) + AtkPoint_CheckCrossBackward(i, j);
                        long defensePoint = DefPoint_CheckHorizontal(i, j) + DefPoint_CheckVertical(i, j) + DefPoint_CheckCross(i, j) + DefPoint_CheckCrossBackward(i, j);
                        long tmpPoint = attackkPoint > defensePoint ? attackkPoint : defensePoint;
                        if (maxPoint < tmpPoint)
                        {
                            maxPoint = tmpPoint;
                            cpResult = new ChessPiece(arrChessPiece[i, j].Row, arrChessPiece[i, j].Column, arrChessPiece[i, j].Position, arrChessPiece[i, j].Owner);

                        }
                    }
                }
            }

            return cpResult;
        }

        #region Attack
        // Duyệt dọc
        private long AtkPoint_CheckVertical(int currRow, int currColumn)
        {
            long total = 0;
            int ally = 0;
            int enemy = 0;
            for (int count = 1; count < 6 && currRow + count < chessBoard.NumOfLines; count++)
            {
                if (arrChessPiece[currRow + count, currColumn].Owner == 1)
                    ally++;
                else if (arrChessPiece[currRow + count, currColumn].Owner == 2)
                {
                    enemy++;
                    total -= 9;
                    break;
                }
                else
                {
                    break;
                }
            }
            for (int count = 1; count < 6 && currRow - count >= 0; count++)
            {
                if (arrChessPiece[currRow - count, currColumn].Owner == 1)
                    ally++;
                else if (arrChessPiece[currRow - count, currColumn].Owner == 2)
                {
                    enemy++;
                    total -= 9;
                    break;
                }
                else
                {
                    break;
                }
            }
            if (enemy == 2) return 0;
            total -= DefensePoint[enemy];
            total += AttackPoint[ally];
            return total;
        }
        // Duyệt ngang
        private long AtkPoint_CheckHorizontal(int currRow, int currColumn)
        {
            long total = 0;
            int ally = 0;
            int enemy = 0;
            for (int count = 1; count < 6 && currColumn + count < chessBoard.NumOfColumns; count++)
            {
                if (arrChessPiece[currRow, currColumn + count].Owner == 1)
                    ally++;
                else if (arrChessPiece[currRow, currColumn + count].Owner == 2)
                {
                    enemy++;
                    total -= 9;
                    break;
                }
                else
                {
                    break;
                }
            }
            for (int count = 1; count < 6 && currColumn - count >= 0; count++)
            {
                if (arrChessPiece[currRow, currColumn - count].Owner == 1)
                    ally++;
                else if (arrChessPiece[currRow, currColumn - count].Owner == 2)
                {
                    enemy++;
                    total -= 9;
                    break;
                }
                else
                {
                    break;
                }
            }
            if (enemy == 2) return 0;
            total -= DefensePoint[enemy];
            total += AttackPoint[ally];
            return total;
        }
        // Duyệt chéo
        private long AtkPoint_CheckCross(int currRow, int currColumn)
        {
            long total = 0;
            int ally = 0;
            int enemy = 0;
            for (int count = 1; count < 6 && currRow + count < chessBoard.NumOfLines && currColumn + count < chessBoard.NumOfColumns; count++)
            {
                if (arrChessPiece[currRow + count, currColumn + count].Owner == 1)
                    ally++;
                else if (arrChessPiece[currRow + count, currColumn + count].Owner == 2)
                {
                    enemy++;
                    total -= 9;
                    break;
                }
                else
                {
                    break;
                }
            }
            for (int count = 1; count < 6 && currRow - count >= 0 && currColumn - count >= 0; count++)
            {
                if (arrChessPiece[currRow - count, currColumn - count].Owner == 1)
                    ally++;
                else if (arrChessPiece[currRow - count, currColumn - count].Owner == 2)
                {
                    enemy++;
                    total -= 9;
                    break;
                }
                else
                {
                    break;
                }
            }
            if (enemy == 2) return 0;
            total -= DefensePoint[enemy];
            total += AttackPoint[ally];
            return total;
        }

        // Duyệt chéo ngược
        private long AtkPoint_CheckCrossBackward(int currRow, int currColumn)
        {
            long total = 0;
            int ally = 0;
            int enemy = 0;
            for (int count = 1; count < 6 && currRow - count >= 0 && currColumn + count < chessBoard.NumOfColumns; count++)
            {
                if (arrChessPiece[currRow - count, currColumn + count].Owner == 1)
                    ally++;
                else if (arrChessPiece[currRow - count, currColumn + count].Owner == 2)
                {
                    enemy++;
                    total -= 9;
                    break;
                }
                else
                {
                    break;
                }
            }
            for (int count = 1; count < 6 && currRow + count < chessBoard.NumOfLines && currColumn - count >= 0; count++)
            {
                if (arrChessPiece[currRow + count, currColumn - count].Owner == 1)
                    ally++;
                else if (arrChessPiece[currRow + count, currColumn - count].Owner == 2)
                {
                    enemy++;
                    total -= 9;
                    break;
                }
                else
                {
                    break;
                }
            }
            if (enemy == 2) return 0;
            total -= DefensePoint[enemy];
            total += AttackPoint[ally];
            return total;
        }
        #endregion

        #region Defense
        private long DefPoint_CheckVertical(int currRow, int currColumn)
        {
            long total = 0;
            int ally = 0;
            int enemy = 0;
            for (int count = 1; count < 6 && currRow + count < chessBoard.NumOfLines; count++)
            {
                if (arrChessPiece[currRow + count, currColumn].Owner == 1)
                {
                    ally++;
                    break;
                }
                else if (arrChessPiece[currRow + count, currColumn].Owner == 2)
                {
                    enemy++;
                }
                else
                {
                    break;
                }
            }
            for (int count = 1; count < 6 && currRow - count >= 0; count++)
            {
                if (arrChessPiece[currRow - count, currColumn].Owner == 1)
                {
                    ally++;
                    break;
                }
                else if (arrChessPiece[currRow - count, currColumn].Owner == 2)
                {
                    enemy++;
                }
                else
                {
                    break;
                }
            }
            if (ally == 2) return 0;
            total += DefensePoint[enemy];
            if (enemy > 0)
                total -= AttackPoint[ally] * 2;
            return total;
        }
        private long DefPoint_CheckHorizontal(int currRow, int currColumn)
        {
            long total = 0;
            int ally = 0;
            int enemy = 0;
            for (int count = 1; count < 6 && currColumn + count < chessBoard.NumOfColumns; count++)
            {
                if (arrChessPiece[currRow, currColumn + count].Owner == 1)
                {
                    ally++;
                    break;
                }
                else if (arrChessPiece[currRow, currColumn + count].Owner == 2)
                {
                    enemy++;
                }
                else
                {
                    break;
                }
            }
            for (int count = 1; count < 6 && currColumn - count >= 0; count++)
            {
                if (arrChessPiece[currRow, currColumn - count].Owner == 1)
                {
                    ally++;
                    break;
                }
                else if (arrChessPiece[currRow, currColumn - count].Owner == 2)
                {
                    enemy++;
                }
                else
                {
                    break;
                }
            }
            if (ally == 2) return 0;
            total += DefensePoint[enemy];
            if (enemy > 0)
                total -= AttackPoint[ally] * 2;

            return total;
        }
        private long DefPoint_CheckCross(int currRow, int currColumn)
        {
            long total = 0;
            int ally = 0;
            int enemy = 0;
            for (int count = 1; count < 6 && currRow + count < chessBoard.NumOfLines && currColumn + count < chessBoard.NumOfColumns; count++)
            {
                if (arrChessPiece[currRow + count, currColumn + count].Owner == 1)
                {
                    ally++;
                    break;
                }
                else if (arrChessPiece[currRow + count, currColumn + count].Owner == 2)
                {
                    enemy++;

                }
                else
                {
                    break;
                }
            }
            for (int count = 1; count < 6 && currRow - count >= 0 && currColumn - count >= 0; count++)
            {
                if (arrChessPiece[currRow - count, currColumn - count].Owner == 1)
                {
                    ally++;
                    break;
                }
                else if (arrChessPiece[currRow - count, currColumn - count].Owner == 2)
                {
                    enemy++;

                }
                else
                {
                    break;
                }
            }
            if (ally == 2) return 0;
            total += DefensePoint[enemy];
            if (enemy > 0)
                total -= AttackPoint[ally] * 2;

            return total;
        }
        private long DefPoint_CheckCrossBackward(int currRow, int currColumn)
        {
            long total = 0;
            int ally = 0;
            int enemy = 0;
            for (int count = 1; count < 6 && currRow - count >= 0 && currColumn + count < chessBoard.NumOfColumns; count++)
            {
                if (arrChessPiece[currRow - count, currColumn + count].Owner == 1)
                {
                    ally++;
                    break;
                }
                else if (arrChessPiece[currRow - count, currColumn + count].Owner == 2)
                {
                    enemy++;

                }
                else
                {
                    break;
                }
            }
            for (int count = 1; count < 6 && currRow + count < chessBoard.NumOfLines && currColumn - count >= 0; count++)
            {
                if (arrChessPiece[currRow + count, currColumn - count].Owner == 1)
                {
                    ally++;
                    break;
                }
                else if (arrChessPiece[currRow + count, currColumn - count].Owner == 2)
                {
                    enemy++;

                }
                else
                {
                    break;
                }
            }
            if (ally == 2) return 0;
            total += DefensePoint[enemy];
            if (enemy > 0)
                total -= AttackPoint[ally] * 2;

            return total;
        }
        #endregion

        #endregion

    }
}
