using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameCaro
{
    class CaroChess
    {
        //3 loại bút vẽ
        public static Pen pen;
        public static SolidBrush sbBlack;
        public static SolidBrush sbWhite;


        private Random rd = new Random();//random ô cờ máy sẽ đánh đầu tiên, và random lượt đi đầu tiên
        private ChessBoard BanCo;
        private ChessPiece[,] MangOCo;
        private int _cheDoChoi;
        private int _luotDi;
        private bool _sanSang;

        public bool SanSang
        {
            get { return _sanSang; }
            set { _sanSang = value; }
        }
        private Stack<ChessPiece> _stkCacNuocDaDi;

        public int CheDoChoi
        {
            get { return _cheDoChoi; }
            set { _cheDoChoi = value; }
        }

        public CaroChess()
        {
            //khởi tạo bàn cờ với số dòng 20, số cột 20
            BanCo = new ChessBoard(frmCaroChess.ChieuCaoBanCo / ChessPiece.CHIEU_CAO, frmCaroChess.ChieuRongBanCo / ChessPiece.CHIEU_RONG);
            //khởi tạo 3 loại bút vẽ
            pen = new Pen(Color.DarkKhaki, 1);
            sbBlack = new SolidBrush(Color.Black);
            sbWhite = new SolidBrush(Color.White);

            _sanSang = false;
            //khai báo mảng các ô cờ 
            MangOCo = new ChessPiece[BanCo.SoDong, BanCo.SoCot];
        }

        //vẽ bàn cờ
        public void veBanCo(Graphics g)
        {
            BanCo.veBanCo(g);
        }

        //khởi tạo mảng ô cờ
        public void khoiTaoMangOCo()
        {
            for (int i = 0; i < BanCo.SoDong; i++)
                for (int j = 0; j < BanCo.SoCot; j++)
                {
                    MangOCo[i, j] = new ChessPiece(i, j, 0);
                }
        }

        // đánh cờ
        public void danhCo(Graphics g, int mouseX, int mouseY)
        {
            int dong = mouseY / ChessPiece.CHIEU_CAO;
            int cot = mouseX / ChessPiece.CHIEU_RONG;

            //loại bỏ trường hợp người chơi kích vào giữa đường kẻ vạch
            if (mouseY % ChessPiece.CHIEU_CAO != 0 && mouseX % ChessPiece.CHIEU_RONG != 0)
            {
                //chỉ đánh vào những ô trống
                if (MangOCo[dong, cot].SoHuu == 0)
                {
                    //quân đen đánh
                    if (_luotDi == 1)
                    {
                        BanCo.veQuanCo(g, cot * ChessPiece.CHIEU_CAO, dong * ChessPiece.CHIEU_RONG, _luotDi);
                        MangOCo[dong, cot].SoHuu = 1;

                        //sao chép o cờ ra một vùng nhớ mới để đẩy vào stack
                        ChessPiece OCo = new ChessPiece(MangOCo[dong, cot].Dong, MangOCo[dong, cot].Cot, MangOCo[dong, cot].SoHuu);
                        //sau khi đánh xong thì đẩy o cờ vào trong stack
                        _stkCacNuocDaDi.Push(OCo);

                        _luotDi = 2;
                    }
                    //quân trắng đánh
                    else
                    {
                        BanCo.veQuanCo(g, cot * ChessPiece.CHIEU_CAO, dong * ChessPiece.CHIEU_RONG, _luotDi);
                        MangOCo[dong, cot].SoHuu = 2;

                        //sao chép o cờ ra một vùng nhớ mới để đẩy vào stack
                        ChessPiece OCo = new ChessPiece(MangOCo[dong, cot].Dong, MangOCo[dong, cot].Cot, MangOCo[dong, cot].SoHuu);
                        //sau khi đánh xong thì đẩy o cờ vào trong stack
                        _stkCacNuocDaDi.Push(OCo);

                        _luotDi = 1;
                    }
                }
            }
        }

        //vẽ lại quân cờ
        public void veLaiQuanCo(Graphics g)
        {
            //trong stack có quân cờ thì thực hiện vẽ lại quân cờ
            if (_stkCacNuocDaDi.Count != 0)
            {
                foreach (ChessPiece oco in _stkCacNuocDaDi)
                {
                    BanCo.veQuanCo(g, oco.Cot * ChessPiece.CHIEU_RONG, oco.Dong * ChessPiece.CHIEU_CAO, oco.SoHuu);
                }
            }
        }

        //chơi với người
        public void choiVoiNguoi(Graphics g)
        {
            //chơi với người
            _cheDoChoi = 1;
            //random lượt đi
            _luotDi = rd.Next(0, 2);
            if (_luotDi == 1)
            {
                MessageBox.Show("Quân xanh đi trước");
            }
            else MessageBox.Show("Quân đỏ đi trước");
            _sanSang = true;
            //khởi tạo mảng ô cờ
            khoiTaoMangOCo();
            //khởi tạo lại stack
            _stkCacNuocDaDi = new Stack<ChessPiece>();
            //vẽ bàn cờ
            veBanCo(g);
        }

        //chơi với máy
        public void choiVoiMay(Graphics g)
        {
            //chơi với máy
            _cheDoChoi = 2;
            //random lượt đi
            _luotDi = rd.Next(0, 2);
            if (_luotDi == 1)
            {
                MessageBox.Show("Máy đi trước");
            }
            else MessageBox.Show("Người chơi đi trước");

            _sanSang = true;
            khoiTaoMangOCo();
            _stkCacNuocDaDi = new Stack<ChessPiece>();
            veBanCo(g);
            mayDanh(g);
        }

        //máy đánh
        public void mayDanh(Graphics g)
        {
            int DiemMax = 0;
            int DiemPhongNgu = 0;
            int DiemTanCong = 0;
            ChessPiece oco = new ChessPiece();

            if (_luotDi == 1)
            {
                //lượt đi đầu tiên sẽ đánh random trong khoảng chính giữa đến 3 nước trên bàn cờ
                if (_stkCacNuocDaDi.Count == 0)
                {
                    danhCo(g, rd.Next((BanCo.SoCot / 2 - 3) * ChessPiece.CHIEU_RONG + 1, (BanCo.SoCot / 2 + 3) * ChessPiece.CHIEU_RONG + 1), rd.Next((BanCo.SoDong / 2 - 3) * ChessPiece.CHIEU_CAO, (BanCo.SoDong / 2 + 3) * ChessPiece.CHIEU_CAO));
                }
                else
                {
                    //thuật toán minmax tìm điểm cao nhất để đánh
                    for (int i = 0; i < BanCo.SoDong; i++)
                    {
                        for (int j = 0; j < BanCo.SoCot; j++)
                        {
                            //nếu nước cờ chưa có ai đánh và không bị cắt tỉa thì mới xét giá trị MinMax
                            if (MangOCo[i, j].SoHuu == 0 && !catTia(MangOCo[i, j]))
                            {
                                int DiemTam;

                                DiemTanCong = duyetTC_Ngang(i, j) + duyetTC_Doc(i, j) + duyetTC_CheoXuoi(i, j) + duyetTC_CheoNguoc(i, j);
                                DiemPhongNgu = duyetPN_Ngang(i, j) + duyetPN_Doc(i, j) + duyetPN_CheoXuoi(i, j) + duyetPN_CheoNguoc(i, j);

                                if (DiemPhongNgu > DiemTanCong)
                                {
                                    DiemTam = DiemPhongNgu;
                                }
                                else
                                {
                                    DiemTam = DiemTanCong;
                                }

                                if (DiemMax < DiemTam)
                                {
                                    DiemMax = DiemTam;
                                    oco = new ChessPiece(MangOCo[i, j].Dong, MangOCo[i, j].Cot, MangOCo[i, j].SoHuu);
                                }
                            }
                        }
                    }

                    danhCo(g, oco.Cot * ChessPiece.CHIEU_RONG + 1, oco.Dong * ChessPiece.CHIEU_CAO + 1);

                }
            }
        }


        #region Cắt tỉa Alpha betal
        bool catTia(ChessPiece oCo)
        {
            //nếu cả 4 hướng đều không có nước cờ thì cắt tỉa
            if (catTiaNgang(oCo) && catTiaDoc(oCo) && catTiaCheoPhai(oCo) && catTiaCheoTrai(oCo))
                return true;

            //chạy đến đây thì 1 trong 4 hướng vẫn có nước cờ thì không được cắt tỉa
            return false;
        }

        bool catTiaNgang(ChessPiece oCo)
        {
            //duyệt bên phải
            if (oCo.Cot <= BanCo.SoCot - 5)
                for (int i = 1; i <= 4; i++)
                    if (MangOCo[oCo.Dong, oCo.Cot + i].SoHuu != 0)//nếu có nước cờ thì không cắt tỉa
                        return false;

            //duyệt bên trái
            if (oCo.Cot >= 4)
                for (int i = 1; i <= 4; i++)
                    if (MangOCo[oCo.Dong, oCo.Cot - i].SoHuu != 0)//nếu có nước cờ thì không cắt tỉa
                        return false;

            //nếu chạy đến đây tức duyệt 2 bên đều không có nước đánh thì cắt tỉa
            return true;
        }
        bool catTiaDoc(ChessPiece oCo)
        {
            //duyệt phía giưới
            if (oCo.Dong <= BanCo.SoDong - 5)
                for (int i = 1; i <= 4; i++)
                    if (MangOCo[oCo.Dong + i, oCo.Cot].SoHuu != 0)//nếu có nước cờ thì không cắt tỉa
                        return false;

            //duyệt phía trên
            if (oCo.Dong >= 4)
                for (int i = 1; i <= 4; i++)
                    if (MangOCo[oCo.Dong - i, oCo.Cot].SoHuu != 0)//nếu có nước cờ thì không cắt tỉa
                        return false;

            //nếu chạy đến đây tức duyệt 2 bên đều không có nước đánh thì cắt tỉa
            return true;
        }
        bool catTiaCheoPhai(ChessPiece oCo)
        {
            //duyệt từ trên xuống
            if (oCo.Dong <= BanCo.SoDong - 5 && oCo.Cot >= 4)
                for (int i = 1; i <= 4; i++)
                    if (MangOCo[oCo.Dong + i, oCo.Cot - i].SoHuu != 0)//nếu có nước cờ thì không cắt tỉa
                        return false;

            //duyệt từ giưới lên
            if (oCo.Cot <= BanCo.SoCot - 5 && oCo.Dong >= 4)
                for (int i = 1; i <= 4; i++)
                    if (MangOCo[oCo.Dong - i, oCo.Cot + i].SoHuu != 0)//nếu có nước cờ thì không cắt tỉa
                        return false;

            //nếu chạy đến đây tức duyệt 2 bên đều không có nước đánh thì cắt tỉa
            return true;
        }
        bool catTiaCheoTrai(ChessPiece oCo)
        {
            //duyệt từ trên xuống
            if (oCo.Dong <= BanCo.SoDong - 5 && oCo.Cot <= BanCo.SoCot - 5)
                for (int i = 1; i <= 4; i++)
                    if (MangOCo[oCo.Dong + i, oCo.Cot + i].SoHuu != 0)//nếu có nước cờ thì không cắt tỉa
                        return false;

            //duyệt từ giưới lên
            if (oCo.Cot >= 4 && oCo.Dong >= 4)
                for (int i = 1; i <= 4; i++)
                    if (MangOCo[oCo.Dong - i, oCo.Cot - i].SoHuu != 0)//nếu có nước cờ thì không cắt tỉa
                        return false;

            //nếu chạy đến đây tức duyệt 2 bên đều không có nước đánh thì cắt tỉa
            return true;
        }

        #endregion

        #region AI

        private int[] MangDiemTanCong = new int[7] { 0, 9, 54, 162, 1458, 13112, 118008 };
        private int[] MangDiemPhongNgu = new int[7] { 0, 3, 27, 99, 729, 6561, 59049 };
        //private int[] MangDiemPhongNgu = new int[7] { 0, 1, 9, 81, 729, 6561, 59049 };
        #region Tấn công
        //duyệt ngang
        public int duyetTC_Ngang(int dongHT, int cotHT)
        {
            int DiemTanCong = 0;
            int SoQuanTa = 0;
            int SoQuanDichPhai = 0;
            int SoQuanDichTrai = 0;
            int KhoangChong = 0;

            //bên phải
            for (int dem = 1; dem <= 4 && cotHT < BanCo.SoCot - 5; dem++)
            {

                if (MangOCo[dongHT, cotHT + dem].SoHuu == 1)
                {
                    if (dem == 1)
                        DiemTanCong += 37;

                    SoQuanTa++;
                    KhoangChong++;
                }
                else
                    if (MangOCo[dongHT, cotHT + dem].SoHuu == 2)
                {
                    SoQuanDichPhai++;
                    break;
                }
                else KhoangChong++;
            }
            //bên trái
            for (int dem = 1; dem <= 4 && cotHT > 4; dem++)
            {
                if (MangOCo[dongHT, cotHT - dem].SoHuu == 1)
                {
                    if (dem == 1)
                        DiemTanCong += 37;

                    SoQuanTa++;
                    KhoangChong++;

                }
                else
                    if (MangOCo[dongHT, cotHT - dem].SoHuu == 2)
                {
                    SoQuanDichTrai++;
                    break;
                }
                else KhoangChong++;
            }
            //bị chặn 2 đầu khoảng chống không đủ tạo thành 5 nước
            if (SoQuanDichPhai > 0 && SoQuanDichTrai > 0 && KhoangChong < 4)
                return 0;

            DiemTanCong -= MangDiemPhongNgu[SoQuanDichPhai + SoQuanDichTrai];
            DiemTanCong += MangDiemTanCong[SoQuanTa];
            return DiemTanCong;
        }

        //duyệt dọc
        public int duyetTC_Doc(int dongHT, int cotHT)
        {
            int DiemTanCong = 0;
            int SoQuanTa = 0;
            int SoQuanDichTren = 0;
            int SoQuanDichDuoi = 0;
            int KhoangChong = 0;

            //bên trên
            for (int dem = 1; dem <= 4 && dongHT > 4; dem++)
            {
                if (MangOCo[dongHT - dem, cotHT].SoHuu == 1)
                {
                    if (dem == 1)
                        DiemTanCong += 37;

                    SoQuanTa++;
                    KhoangChong++;

                }
                else
                    if (MangOCo[dongHT - dem, cotHT].SoHuu == 2)
                {
                    SoQuanDichTren++;
                    break;
                }
                else KhoangChong++;
            }
            //bên dưới
            for (int dem = 1; dem <= 4 && dongHT < BanCo.SoDong - 5; dem++)
            {
                if (MangOCo[dongHT + dem, cotHT].SoHuu == 1)
                {
                    if (dem == 1)
                        DiemTanCong += 37;

                    SoQuanTa++;
                    KhoangChong++;

                }
                else
                    if (MangOCo[dongHT + dem, cotHT].SoHuu == 2)
                {
                    SoQuanDichDuoi++;
                    break;
                }
                else KhoangChong++;
            }
            //bị chặn 2 đầu khoảng chống không đủ tạo thành 5 nước
            if (SoQuanDichTren > 0 && SoQuanDichDuoi > 0 && KhoangChong < 4)
                return 0;

            DiemTanCong -= MangDiemPhongNgu[SoQuanDichTren + SoQuanDichDuoi];
            DiemTanCong += MangDiemTanCong[SoQuanTa];
            return DiemTanCong;
        }

        //chéo xuôi
        public int duyetTC_CheoXuoi(int dongHT, int cotHT)
        {
            int DiemTanCong = 1;
            int SoQuanTa = 0;
            int SoQuanDichCheoTren = 0;
            int SoQuanDichCheoDuoi = 0;
            int KhoangChong = 0;

            //bên chéo xuôi xuống
            for (int dem = 1; dem <= 4 && cotHT < BanCo.SoCot - 5 && dongHT < BanCo.SoDong - 5; dem++)
            {
                if (MangOCo[dongHT + dem, cotHT + dem].SoHuu == 1)
                {
                    if (dem == 1)
                        DiemTanCong += 37;

                    SoQuanTa++;
                    KhoangChong++;

                }
                else
                    if (MangOCo[dongHT + dem, cotHT + dem].SoHuu == 2)
                {
                    SoQuanDichCheoTren++;
                    break;
                }
                else KhoangChong++;
            }
            //chéo xuôi lên
            for (int dem = 1; dem <= 4 && dongHT > 4 && cotHT > 4; dem++)
            {
                if (MangOCo[dongHT - dem, cotHT - dem].SoHuu == 1)
                {
                    if (dem == 1)
                        DiemTanCong += 37;

                    SoQuanTa++;
                    KhoangChong++;

                }
                else
                    if (MangOCo[dongHT - dem, cotHT - dem].SoHuu == 2)
                {
                    SoQuanDichCheoDuoi++;
                    break;
                }
                else KhoangChong++;
            }
            //bị chặn 2 đầu khoảng chống không đủ tạo thành 5 nước
            if (SoQuanDichCheoTren > 0 && SoQuanDichCheoDuoi > 0 && KhoangChong < 4)
                return 0;

            DiemTanCong -= MangDiemPhongNgu[SoQuanDichCheoTren + SoQuanDichCheoDuoi];
            DiemTanCong += MangDiemTanCong[SoQuanTa];
            return DiemTanCong;
        }

        //chéo ngược
        public int duyetTC_CheoNguoc(int dongHT, int cotHT)
        {
            int DiemTanCong = 0;
            int SoQuanTa = 0;
            int SoQuanDichCheoTren = 0;
            int SoQuanDichCheoDuoi = 0;
            int KhoangChong = 0;

            //chéo ngược lên
            for (int dem = 1; dem <= 4 && cotHT < BanCo.SoCot - 5 && dongHT > 4; dem++)
            {
                if (MangOCo[dongHT - dem, cotHT + dem].SoHuu == 1)
                {
                    if (dem == 1)
                        DiemTanCong += 37;

                    SoQuanTa++;
                    KhoangChong++;

                }
                else
                    if (MangOCo[dongHT - dem, cotHT + dem].SoHuu == 2)
                {
                    SoQuanDichCheoTren++;
                    break;
                }
                else KhoangChong++;
            }
            //chéo ngược xuống
            for (int dem = 1; dem <= 4 && cotHT > 4 && dongHT < BanCo.SoDong - 5; dem++)
            {
                if (MangOCo[dongHT + dem, cotHT - dem].SoHuu == 1)
                {
                    if (dem == 1)
                        DiemTanCong += 37;

                    SoQuanTa++;
                    KhoangChong++;

                }
                else
                    if (MangOCo[dongHT + dem, cotHT - dem].SoHuu == 2)
                {
                    SoQuanDichCheoDuoi++;
                    break;
                }
                else KhoangChong++;
            }
            //bị chặn 2 đầu khoảng chống không đủ tạo thành 5 nước
            if (SoQuanDichCheoTren > 0 && SoQuanDichCheoDuoi > 0 && KhoangChong < 4)
                return 0;

            DiemTanCong -= MangDiemPhongNgu[SoQuanDichCheoTren + SoQuanDichCheoDuoi];
            DiemTanCong += MangDiemTanCong[SoQuanTa];
            return DiemTanCong;
        }
        #endregion

        #region phòng ngự

        //duyệt ngang
        public int duyetPN_Ngang(int dongHT, int cotHT)
        {
            int DiemPhongNgu = 0;
            int SoQuanTaTrai = 0;
            int SoQuanTaPhai = 0;
            int SoQuanDich = 0;
            int KhoangChongPhai = 0;
            int KhoangChongTrai = 0;
            bool ok = false;


            for (int dem = 1; dem <= 4 && cotHT < BanCo.SoCot - 5; dem++)
            {
                if (MangOCo[dongHT, cotHT + dem].SoHuu == 2)
                {
                    if (dem == 1)
                        DiemPhongNgu += 9;

                    SoQuanDich++;
                }
                else
                    if (MangOCo[dongHT, cotHT + dem].SoHuu == 1)
                {
                    if (dem == 4)
                        DiemPhongNgu -= 170;

                    SoQuanTaTrai++;
                    break;
                }
                else
                {
                    if (dem == 1)
                        ok = true;

                    KhoangChongPhai++;
                }
            }

            if (SoQuanDich == 3 && KhoangChongPhai == 1 && ok)
                DiemPhongNgu -= 200;

            ok = false;

            for (int dem = 1; dem <= 4 && cotHT > 4; dem++)
            {
                if (MangOCo[dongHT, cotHT - dem].SoHuu == 2)
                {
                    if (dem == 1)
                        DiemPhongNgu += 9;

                    SoQuanDich++;
                }
                else
                    if (MangOCo[dongHT, cotHT - dem].SoHuu == 1)
                {
                    if (dem == 4)
                        DiemPhongNgu -= 170;

                    SoQuanTaPhai++;
                    break;
                }
                else
                {
                    if (dem == 1)
                        ok = true;

                    KhoangChongTrai++;
                }
            }

            if (SoQuanDich == 3 && KhoangChongTrai == 1 && ok)
                DiemPhongNgu -= 200;

            if (SoQuanTaPhai > 0 && SoQuanTaTrai > 0 && (KhoangChongTrai + KhoangChongPhai + SoQuanDich) < 4)
                return 0;

            DiemPhongNgu -= MangDiemTanCong[SoQuanTaPhai + SoQuanTaPhai];
            DiemPhongNgu += MangDiemPhongNgu[SoQuanDich];

            return DiemPhongNgu;
        }

        //duyệt dọc
        public int duyetPN_Doc(int dongHT, int cotHT)
        {
            int DiemPhongNgu = 0;
            int SoQuanTaTrai = 0;
            int SoQuanTaPhai = 0;
            int SoQuanDich = 0;
            int KhoangChongTren = 0;
            int KhoangChongDuoi = 0;
            bool ok = false;

            //lên
            for (int dem = 1; dem <= 4 && dongHT > 4; dem++)
            {
                if (MangOCo[dongHT - dem, cotHT].SoHuu == 2)
                {
                    if (dem == 1)
                        DiemPhongNgu += 9;

                    SoQuanDich++;

                }
                else
                    if (MangOCo[dongHT - dem, cotHT].SoHuu == 1)
                {
                    if (dem == 4)
                        DiemPhongNgu -= 170;

                    SoQuanTaPhai++;
                    break;
                }
                else
                {
                    if (dem == 1)
                        ok = true;

                    KhoangChongTren++;
                }
            }

            if (SoQuanDich == 3 && KhoangChongTren == 1 && ok)
                DiemPhongNgu -= 200;

            ok = false;
            //xuống
            for (int dem = 1; dem <= 4 && dongHT < BanCo.SoDong - 5; dem++)
            {
                //gặp quân địch
                if (MangOCo[dongHT + dem, cotHT].SoHuu == 2)
                {
                    if (dem == 1)
                        DiemPhongNgu += 9;

                    SoQuanDich++;
                }
                else
                    if (MangOCo[dongHT + dem, cotHT].SoHuu == 1)
                {
                    if (dem == 4)
                        DiemPhongNgu -= 170;

                    SoQuanTaTrai++;
                    break;
                }
                else
                {
                    if (dem == 1)
                        ok = true;

                    KhoangChongDuoi++;
                }
            }

            if (SoQuanDich == 3 && KhoangChongDuoi == 1 && ok)
                DiemPhongNgu -= 200;

            if (SoQuanTaPhai > 0 && SoQuanTaTrai > 0 && (KhoangChongTren + KhoangChongDuoi + SoQuanDich) < 4)
                return 0;

            DiemPhongNgu -= MangDiemTanCong[SoQuanTaTrai + SoQuanTaPhai];
            DiemPhongNgu += MangDiemPhongNgu[SoQuanDich];
            return DiemPhongNgu;
        }

        //chéo xuôi
        public int duyetPN_CheoXuoi(int dongHT, int cotHT)
        {
            int DiemPhongNgu = 0;
            int SoQuanTaTrai = 0;
            int SoQuanTaPhai = 0;
            int SoQuanDich = 0;
            int KhoangChongTren = 0;
            int KhoangChongDuoi = 0;
            bool ok = false;

            //lên
            for (int dem = 1; dem <= 4 && dongHT < BanCo.SoDong - 5 && cotHT < BanCo.SoCot - 5; dem++)
            {
                if (MangOCo[dongHT + dem, cotHT + dem].SoHuu == 2)
                {
                    if (dem == 1)
                        DiemPhongNgu += 9;

                    SoQuanDich++;
                }
                else
                    if (MangOCo[dongHT + dem, cotHT + dem].SoHuu == 1)
                {
                    if (dem == 4)
                        DiemPhongNgu -= 170;

                    SoQuanTaPhai++;
                    break;
                }
                else
                {
                    if (dem == 1)
                        ok = true;

                    KhoangChongTren++;
                }
            }

            if (SoQuanDich == 3 && KhoangChongTren == 1 && ok)
                DiemPhongNgu -= 200;

            ok = false;
            //xuống
            for (int dem = 1; dem <= 4 && dongHT > 4 && cotHT > 4; dem++)
            {
                if (MangOCo[dongHT - dem, cotHT - dem].SoHuu == 2)
                {
                    if (dem == 1)
                        DiemPhongNgu += 9;

                    SoQuanDich++;
                }
                else
                    if (MangOCo[dongHT - dem, cotHT - dem].SoHuu == 1)
                {
                    if (dem == 4)
                        DiemPhongNgu -= 170;

                    SoQuanTaTrai++;
                    break;
                }
                else
                {
                    if (dem == 1)
                        ok = true;

                    KhoangChongDuoi++;
                }
            }

            if (SoQuanDich == 3 && KhoangChongDuoi == 1 && ok)
                DiemPhongNgu -= 200;

            if (SoQuanTaPhai > 0 && SoQuanTaTrai > 0 && (KhoangChongTren + KhoangChongDuoi + SoQuanDich) < 4)
                return 0;

            DiemPhongNgu -= MangDiemTanCong[SoQuanTaPhai + SoQuanTaTrai];
            DiemPhongNgu += MangDiemPhongNgu[SoQuanDich];

            return DiemPhongNgu;
        }

        //chéo ngược
        public int duyetPN_CheoNguoc(int dongHT, int cotHT)
        {
            int DiemPhongNgu = 0;
            int SoQuanTaTrai = 0;
            int SoQuanTaPhai = 0;
            int SoQuanDich = 0;
            int KhoangChongTren = 0;
            int KhoangChongDuoi = 0;
            bool ok = false;

            //lên
            for (int dem = 1; dem <= 4 && dongHT > 4 && cotHT < BanCo.SoCot - 5; dem++)
            {

                if (MangOCo[dongHT - dem, cotHT + dem].SoHuu == 2)
                {
                    if (dem == 1)
                        DiemPhongNgu += 9;

                    SoQuanDich++;
                }
                else
                    if (MangOCo[dongHT - dem, cotHT + dem].SoHuu == 1)
                {
                    if (dem == 4)
                        DiemPhongNgu -= 170;

                    SoQuanTaPhai++;
                    break;
                }
                else
                {
                    if (dem == 1)
                        ok = true;

                    KhoangChongTren++;
                }
            }


            if (SoQuanDich == 3 && KhoangChongTren == 1 && ok)
                DiemPhongNgu -= 200;

            ok = false;

            //xuống
            for (int dem = 1; dem <= 4 && dongHT < BanCo.SoDong - 5 && cotHT > 4; dem++)
            {
                if (MangOCo[dongHT + dem, cotHT - dem].SoHuu == 2)
                {
                    if (dem == 1)
                        DiemPhongNgu += 9;

                    SoQuanDich++;
                }
                else
                    if (MangOCo[dongHT + dem, cotHT - dem].SoHuu == 1)
                {
                    if (dem == 4)
                        DiemPhongNgu -= 170;

                    SoQuanTaTrai++;
                    break;
                }
                else
                {
                    if (dem == 1)
                        ok = true;

                    KhoangChongDuoi++;
                }
            }

            if (SoQuanDich == 3 && KhoangChongDuoi == 1 && ok)
                DiemPhongNgu -= 200;

            if (SoQuanTaPhai > 0 && SoQuanTaTrai > 0 && (KhoangChongTren + KhoangChongDuoi + SoQuanDich) < 4)
                return 0;

            DiemPhongNgu -= MangDiemTanCong[SoQuanTaTrai + SoQuanTaPhai];
            DiemPhongNgu += MangDiemPhongNgu[SoQuanDich];

            return DiemPhongNgu;
        }

        #endregion

        #endregion

        #region duyệt chiến thắng theo 8 hướng
        //kiểm tra chiến thắng
        public bool kiemTraChienThang(Graphics g)
        {
            if (_stkCacNuocDaDi.Count != 0)
            {
                foreach (ChessPiece oco in _stkCacNuocDaDi)
                {
                    //duyệt theo 8 hướng mỗi quân cờ
                    if (duyetNgangPhai(g, oco.Dong, oco.Cot, oco.SoHuu) || duyetNgangTrai(g, oco.Dong, oco.Cot, oco.SoHuu)
                        || duyetDocTren(g, oco.Dong, oco.Cot, oco.SoHuu) || duyetDocDuoi(g, oco.Dong, oco.Cot, oco.SoHuu)
                        || duyetCheoXuoiTren(g, oco.Dong, oco.Cot, oco.SoHuu) || duyetCheoXuoiDuoi(g, oco.Dong, oco.Cot, oco.SoHuu)
                        || duyetCheoNguocTren(g, oco.Dong, oco.Cot, oco.SoHuu) || duyetCheoNguocDuoi(g, oco.Dong, oco.Cot, oco.SoHuu))
                    {
                        ketThucTroChoi(oco);
                        return true;
                    }
                }
            }

            return false;
        }

        //vẽ đường kẻ trên 5 nước thắng
        public void veDuongChienThang(Graphics g, int x1, int y1, int x2, int y2)
        {
            g.DrawLine(new Pen(Color.Blue, 3f), x1, y1, x2, y2);
        }

        public bool duyetNgangPhai(Graphics g, int dongHT, int cotHT, int SoHuu)
        {
            if (cotHT > BanCo.SoCot - 5)
                return false;
            for (int dem = 1; dem <= 4; dem++)
            {
                if (MangOCo[dongHT, cotHT + dem].SoHuu != SoHuu)
                {
                    return false;
                }

            }
            veDuongChienThang(g, (cotHT) * ChessPiece.CHIEU_RONG, dongHT * ChessPiece.CHIEU_CAO + ChessPiece.CHIEU_CAO / 2, (cotHT + 5) * ChessPiece.CHIEU_RONG, dongHT * ChessPiece.CHIEU_CAO + ChessPiece.CHIEU_CAO / 2);
            return true;
        }

        public bool duyetNgangTrai(Graphics g, int dongHT, int cotHT, int SoHuu)
        {
            if (cotHT < 4)
                return false;
            for (int dem = 1; dem <= 4; dem++)
            {
                if (MangOCo[dongHT, cotHT - dem].SoHuu != SoHuu)
                {
                    return false;
                }

            }
            veDuongChienThang(g, (cotHT + 1) * ChessPiece.CHIEU_RONG, dongHT * ChessPiece.CHIEU_CAO + ChessPiece.CHIEU_CAO / 2, (cotHT - 4) * ChessPiece.CHIEU_RONG, dongHT * ChessPiece.CHIEU_CAO + ChessPiece.CHIEU_CAO / 2);
            return true;
        }

        public bool duyetDocTren(Graphics g, int dongHT, int cotHT, int SoHuu)
        {
            if (dongHT < 4)
                return false;
            for (int dem = 1; dem <= 4; dem++)
            {
                if (MangOCo[dongHT - dem, cotHT].SoHuu != SoHuu)
                {
                    return false;
                }

            }
            veDuongChienThang(g, cotHT * ChessPiece.CHIEU_RONG + ChessPiece.CHIEU_RONG / 2, (dongHT + 1) * ChessPiece.CHIEU_CAO, cotHT * ChessPiece.CHIEU_RONG + ChessPiece.CHIEU_RONG / 2, (dongHT - 4) * ChessPiece.CHIEU_CAO);
            return true;
        }

        public bool duyetDocDuoi(Graphics g, int dongHT, int cotHT, int SoHuu)
        {
            if (dongHT > BanCo.SoDong - 5)
                return false;
            for (int dem = 1; dem <= 4; dem++)
            {
                if (MangOCo[dongHT + dem, cotHT].SoHuu != SoHuu)
                {
                    return false;
                }

            }
            veDuongChienThang(g, cotHT * ChessPiece.CHIEU_RONG + ChessPiece.CHIEU_RONG / 2, dongHT * ChessPiece.CHIEU_CAO, cotHT * ChessPiece.CHIEU_RONG + ChessPiece.CHIEU_RONG / 2, (dongHT + 5) * ChessPiece.CHIEU_CAO);
            return true;
        }

        public bool duyetCheoXuoiTren(Graphics g, int dongHT, int cotHT, int SoHuu)
        {
            if (dongHT < 4 || cotHT < 4)
                return false;
            for (int dem = 1; dem <= 4; dem++)
            {
                if (MangOCo[dongHT - dem, cotHT - dem].SoHuu != SoHuu)
                {
                    return false;
                }

            }
            veDuongChienThang(g, (cotHT + 1) * ChessPiece.CHIEU_RONG, (dongHT + 1) * ChessPiece.CHIEU_CAO, (cotHT - 4) * ChessPiece.CHIEU_RONG, (dongHT - 4) * ChessPiece.CHIEU_CAO);
            return true;
        }

        public bool duyetCheoXuoiDuoi(Graphics g, int dongHT, int cotHT, int SoHuu)
        {
            if (dongHT > BanCo.SoDong - 5 || cotHT > BanCo.SoCot - 5)
                return false;
            for (int dem = 1; dem <= 4; dem++)
            {
                if (MangOCo[dongHT + dem, cotHT + dem].SoHuu != SoHuu)
                {
                    return false;
                }

            }
            veDuongChienThang(g, cotHT * ChessPiece.CHIEU_RONG, dongHT * ChessPiece.CHIEU_CAO, (cotHT + 5) * ChessPiece.CHIEU_RONG, (dongHT + 5) * ChessPiece.CHIEU_CAO);
            return true;
        }

        public bool duyetCheoNguocDuoi(Graphics g, int dongHT, int cotHT, int SoHuu)
        {
            if (dongHT > BanCo.SoDong - 5 || cotHT < 4)
                return false;
            for (int dem = 1; dem <= 4; dem++)
            {
                if (MangOCo[dongHT + dem, cotHT - dem].SoHuu != SoHuu)
                {
                    return false;
                }

            }
            veDuongChienThang(g, (cotHT + 1) * ChessPiece.CHIEU_RONG, dongHT * ChessPiece.CHIEU_CAO, (cotHT - 4) * ChessPiece.CHIEU_RONG, (dongHT + 5) * ChessPiece.CHIEU_CAO);
            return true;
        }

        public bool duyetCheoNguocTren(Graphics g, int dongHT, int cotHT, int SoHuu)
        {
            if (dongHT < 4 || cotHT > BanCo.SoCot - 5)
                return false;
            for (int dem = 1; dem <= 4; dem++)
            {
                if (MangOCo[dongHT - dem, cotHT + dem].SoHuu != SoHuu)
                {
                    return false;
                }

            }
            veDuongChienThang(g, cotHT * ChessPiece.CHIEU_RONG, (dongHT + 1) * ChessPiece.CHIEU_CAO, (cotHT + 5) * ChessPiece.CHIEU_RONG, (dongHT - 4) * ChessPiece.CHIEU_CAO);
            return true;
        }

        #endregion

        //kết thúc trò chơi
        public void ketThucTroChoi(ChessPiece oco)
        {
            //chơi với người
            if (_cheDoChoi == 1)
            {
                if (oco.SoHuu == 1)
                    MessageBox.Show("Quân xanh thắng");
                else
                    MessageBox.Show("Quân đỏ thắng");
            }
            else//chơi với máy
            {
                if (oco.SoHuu == 1)
                    MessageBox.Show("Máy thắng");
                else
                    MessageBox.Show("Người chơi thắng");
            }

            _sanSang = false;
        }
    }
}
