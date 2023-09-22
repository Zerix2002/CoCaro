using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NguyenNgocThach_Tuan1.GUI
{
    public partial class BaiTuLam1 : Form
    {
        /*
         * Bàn cờ là ma trận 2 chiều với kích thước n
         * Chưa được đánh là -1, 0 là 'O', 1 là 'X'
         * true là O, false là X
         */

        bool luotDiCuaNguoiChoi; // Lượt đi
        bool luotDiCuaMay; // Lượt đi của máy, ngược lại với lượt đi của người chơi
        int[,] banCo; // Bàn cờ (Mảng 2 chiều)
        int capMaTran = 0; // Kích thước ma trận

        static Size firstSize ;

        public BaiTuLam1()
        {
            InitializeComponent();
            CenterToScreen();
            firstSize = this.Size;
        }

        #region Tạo form + khởi tạo bàn cờ
        /// <summary>
        /// Chỉnh kích thước form theo panel
        /// </summary>
        /// <param name="ctr">Form hiện tại</param>
        /// <param name="panel">Bàn cờ</param>
        public void chinhFormTheoPanel(Control ctr, Panel panel)
        {
            // Chỉnh kích thước form và bàn cờ theo kích thước ma trận
            int width = (int)(panel.Size.Width + 130); // Chiều dài form
            int height = (int)(panel_BanCo.Size.Height + 65); // Chiều cao form
            
            if (width <= firstSize.Width && height <= firstSize.Height) // Kiểm tra chiều cao và chiều rộng tối thiểu
            {
                ctr.Size = firstSize;
            }
            else
            {
                ctr.Size = new Size(width, height);
            }
            CenterToScreen();
        }

        /// <summary>
        /// Tạo bàn cờ với kích thước nhập từ textbox cấp ma trận
        /// </summary>
        /// <param name="n">Cấp ma trận</param>
        /// <param name="kichThuoc">Kích thước của button</param>
        /// <param name="khoangTrong">Khoảng trống cho phần rìa</param>
        void taoBanCo(int n, int kichThuoc, int khoangTrong)
        {
            banCo = new int[n, n]; // Khởi tạo bàn cờ

            panel_BanCo.Controls.Clear(); // Xóa panel cũ
            panel_BanCo.Enabled = true;
            panel_BanCo.Size = new Size(n * kichThuoc + khoangTrong, n * kichThuoc + khoangTrong); // Chỉnh kích thước mới cho panel
            // Chỉnh lại form theo panel
            chinhFormTheoPanel(this, panel_BanCo);

            int khoangTrongPhiaTren = 10; // Khoảng trống cho rìa trên
            for (int i = 0; i < n; i++)
            {
                int khoangTrongBenTrai = 10; // Khoảng trống cho rìa trái

                for (int j = 0; j < n; j++)
                {
                    NuocDi nd = new NuocDi(i, j);

                    banCo[i, j] = -1; // Gán giá trị chưa có nước đi (-1) cho từng ô

                    Button btn = new Button();
                    btn.Size = new Size(kichThuoc, kichThuoc); // Chỉnh kích thước cho từng nút
                    btn.Left = khoangTrongBenTrai;
                    khoangTrongBenTrai += kichThuoc;
                    btn.Top = khoangTrongPhiaTren;

                    btn.Tag = new int[] { nd.X, nd.Y };
                    btn.Click += btn_Click; // Gán event cho các nút được tạo

                    panel_BanCo.Controls.Add(btn); // Thêm nút vào panel
                }
                khoangTrongPhiaTren += kichThuoc;
            }
        }

        void thayDoiButton(Button btn, bool luotDi)
        {
            // Thay đổi ký tự và màu trên button
            btn.Text = luotDi ? "O" : "X";
            btn.BackColor = luotDi ? Color.Green : Color.Red;
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int[] tag = (int[])btn.Tag;
            NuocDi nd = new NuocDi(tag[0], tag[1]);

            if (banCo[nd.X, nd.Y] == -1) // Kiểm tra ô chưa có nước đi trước khi thực hiện nước đi
            {
                thucHienNuocDi(banCo, nd, luotDiCuaNguoiChoi ? 0 : 1);
                thayDoiButton(btn, luotDiCuaNguoiChoi);

                if(kiemTraNguoiThang(banCo, luotDiCuaNguoiChoi?0:1))
                {
                    MessageBox.Show("Bạn thắng!");
                    panel_BanCo.Enabled = false;
                    return;
                }
                nd = GetBestMove((int[,])banCo.Clone(), 10, luotDiCuaMay ? 0 : 1);
                banCo[nd.X, nd.Y] = luotDiCuaMay ? 0 : 1;
                // Tìm và thay đổi văn bản của button AI
                foreach (Control control in panel_BanCo.Controls)
                {
                    if (control is Button aiButton)
                    {
                        int[] aiTag = (int[])aiButton.Tag;
                        if (aiTag[0] == nd.X && aiTag[1] == nd.Y)
                        {
                            thayDoiButton(aiButton, luotDiCuaMay);
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Đánh nước đi hiện tại và thay đổi lượt đi từ người sang máy (O sang X - X sang O)
        /// </summary>
        /// <param name="btn">Button để thay đổi text</param>
        /// <param name="nd">x, y của điểm đang được đánh</param>
        /// <returns>False nếu còn lượt đi hoặc hòa, true nếu có lượt đi thắng</returns>
        int[,] thucHienNuocDi(int[,] banCo, NuocDi nd, int nuocDiHienTai)
        {
            int[,] banCoMoi = (int[,])banCo.Clone();
            // Gán giá trị cho (ma trận) bàn cờ với lượt đi hiện tại
            banCo[nd.X, nd.Y] = nuocDiHienTai;

            return banCoMoi;
        }

        /// <summary>
        /// Tạo bàn cờ với lượt đi trước tùy thuộc vào nút đã chọn.
        /// Có kiểm tra nội dung nhập vào
        /// </summary>
        /// <param name="nuocDiTruoc">true là O, false là X</param>
        private void batDau(bool nuocDiTruoc)
        {// Event của nút xác nhận, khi nhấn sẽ lấy nội dung trên textbox CapMaTran để tạo bàn cờ và chọn nước được đi trước

            if (!kiemTraChuoiSo(txtCapMaTran.Text)) return;

            capMaTran = int.Parse(txtCapMaTran.Text); // Cấp ma trận nhập từ textbox
            int kichThuoc = 30; // Kích thước cho từng button
            int khoangTrong = (int)(kichThuoc * 2 / 3); // Khoảng trống cho phần rìa
            luotDiCuaNguoiChoi = nuocDiTruoc;
            luotDiCuaMay = !nuocDiTruoc;
            taoBanCo(capMaTran, kichThuoc, khoangTrong); // Gọi hàm tạo bàn cờ

        }

        /// <summary>
        ///  Kiểm tra bàn còn vị trí đánh hay không
        /// </summary>
        public bool kiemTraBanDay(int[,] banCo)
        {
            foreach (int x in banCo)
            {
                if (x == -1)
                {
                    return false;
                }
            }
            return true;
        }

        public bool kiemTraKetThuc(int[,] banCo, int luotDiHienTai)
        {
            if (kiemTraNguoiThang(banCo, luotDiHienTai))
                return true;
            if (kiemTraBanDay(banCo))
                return true;
            return false;
        }

        List<NuocDi> layDanhSachNuocChuaDi(int[,] banCo)
        {
            List<NuocDi> list = new List<NuocDi>();

            for (int i = 0; i < banCo.GetLength(0); i++)
            {
                for (int j = 0; j < banCo.GetLength(1); j++)
                    if (banCo[i, j] == -1)
                        list.Add(new NuocDi(i, j));
            }
            return list;
        }

        /// <summary>
        /// Kiểm tra nước đi hiện tại có thỏa điều kiện thắng hay không, với ma trận lớn hơn 5 thì kiểm tra 5 ô
        /// </summary>
        /// <param name="nuocDiHienTai"></param>
        /// <returns></returns>
        public bool kiemTraNguoiThang(int[,] banCo, int nuocDiHienTai)
        {
            int dieuKienThang = capMaTran > 5 ? 5 : capMaTran;
            // Kiểm tra hàng và cột
            for (int i = 0; i < capMaTran; i++)
            {
                int demHangNgang = 0;
                int demHangDoc = 0;

                for (int j = 0; j < capMaTran; j++)
                {
                    if (banCo[i, j] == nuocDiHienTai)
                    {
                        demHangNgang++;
                    }
                    else
                    {
                        demHangNgang = 0;
                    }
                    if (banCo[j, i] == nuocDiHienTai)
                    {
                        demHangDoc++;
                    }
                    else demHangDoc = 0;
                    if (demHangDoc == dieuKienThang
                        || demHangNgang == dieuKienThang)
                        return true;
                }
            }

            // Kiểm tra đường chéo chính và phụ
            for (int i = 0; i < capMaTran; i++)
            {

                if (capMaTran - i < dieuKienThang)
                    // Không cần kiểm tra trong góc
                    return false;

                int demCheoChinhTren = 0;
                int demCheoChinhDuoi = 0;

                int demCheoPhuTren = 0;
                int demCheoPhuDuoi = 0;

                int iFake = i;

                for (int j = 0; j < capMaTran - i; j++)
                {
                    // Đếm số ô phía trên đường chéo chính
                    /*
                     * \|1|1
                     * 0|\|1
                     * 0|0|\
                     */
                    if (banCo[j, iFake] == nuocDiHienTai)
                    {
                        demCheoChinhTren++;
                    }
                    else
                    {
                        demCheoChinhTren = 0;
                    }

                    // Đếm số ô phía dưới đường chéo chính
                    /*
                     * \|0|0
                     * 1|\|0
                     * 1|1|\
                     */
                    if (banCo[iFake, j] == nuocDiHienTai)
                    {
                        demCheoChinhDuoi++;
                    }
                    else
                    {
                        demCheoChinhDuoi = 0;
                    }

                    // Đếm số ô phía duới đường chéo phụ
                    /*
                     * 0|0|/
                     * 0|/|1
                     * /|1|1
                     */
                    if (banCo[((capMaTran - 1) - j), iFake] == nuocDiHienTai)
                    {
                        demCheoPhuDuoi++;
                    }
                    else demCheoPhuDuoi = 0;

                    // Đếm số ô phía trên đường chéo phụ
                    /*
                     * 1|1|/
                     * 1|/|0
                     * /|0|0
                     */
                    if (((capMaTran - 2) - iFake) >= 0)
                    {
                        if (banCo[j, ((capMaTran - 2) - iFake)] == nuocDiHienTai)
                        {
                            demCheoPhuTren++;
                        }
                        else demCheoPhuTren = 0;
                    }

                    if (demCheoChinhDuoi == dieuKienThang
                        || demCheoChinhTren == dieuKienThang
                        || demCheoPhuDuoi == dieuKienThang
                        || demCheoPhuTren == dieuKienThang)
                        return true;

                    iFake++;
                }
            }

            return false;
        }
        /// <summary>
        /// Kiểm tra chuỗi nhập vào là chuỗi số không rỗng (Chuỗi số 0 - 9)
        /// </summary>
        /// <param name="chuoi">Chuỗi cần kiểm tra</param>
        /// <returns>true - là chuỗi số, false - có chứa ký tự khác</returns>
        private bool kiemTraChuoiSo(string chuoi)
        {
            if (chuoi.Trim().Length != 0 && chuoi.All(c => c >= '0' && c <= '9'))
            {
                return true;
            }
            return false;
        }

        #region Các button chọn lượt

        private void btnO_Click(object sender, EventArgs e)
        {
            batDau(true); // O đi trước
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            batDau(false); // X đi trước
        }
        #endregion

        int danhGiaNuocDi(int[,] banCo, int luotDiHienTai)
        {
            int playerScore = CalculateScore(banCo, luotDiHienTai);
            int opponentScore = CalculateScore(banCo, GetOpponent(luotDiHienTai));

            return playerScore - opponentScore;
        }

        int GetOpponent(int player)
        {
            return (player == 0) ? 1 : 1;
        }

        static int GetScore(int playerCount, int emptyCount)
        {
            if (playerCount == 4) // Thắng
                return 1000;
            if (playerCount == 3 && emptyCount == 1) // 3 ô và 1 ô trống
                return 100;
            if (playerCount == 2 && emptyCount == 2) // 2 ô và 2 ô trống
                return 10;
            return 0;
        }

        int CalculateScore(int[,] board, int player)
        {
            int score = 0;

            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            // Đếm điểm từng dòng
            for (int row = 0; row < rows; row++)
            {
                int playerCount = 0; // Số lượng ô của người chơi trong dòng
                int emptyCount = 0;  // Số lượng ô trống trong dòng

                for (int col = 0; col < cols; col++)
                {
                    if (board[row, col] == player)
                        playerCount++;
                    else if (board[row, col] == ' ')
                        emptyCount++;
                }

                // Điểm tăng khi có nhiều ô của người chơi và nhiều ô trống
                score += GetScore(playerCount, emptyCount);
            }

            // Đếm điểm từng cột
            for (int col = 0; col < cols; col++)
            {
                int playerCount = 0; // Số lượng ô của người chơi trong cột
                int emptyCount = 0;  // Số lượng ô trống trong cột

                for (int row = 0; row < rows; row++)
                {
                    if (board[row, col] == player)
                        playerCount++;
                    else if (board[row, col] == ' ')
                        emptyCount++;
                }

                // Điểm tăng khi có nhiều ô của người chơi và nhiều ô trống
                score += GetScore(playerCount, emptyCount);
            }

            // Đếm điểm đường chéo chính (từ góc trên trái đến góc dưới phải)
            int mainDiagonalPlayerCount = 0;
            int mainDiagonalEmptyCount = 0;

            for (int i = 0; i < Math.Min(rows, cols); i++)
            {
                if (board[i, i] == player)
                    mainDiagonalPlayerCount++;
                else if (board[i, i] == ' ')
                    mainDiagonalEmptyCount++;
            }

            score += GetScore(mainDiagonalPlayerCount, mainDiagonalEmptyCount);

            // Đếm điểm đường chéo phụ (từ góc trên phải đến góc dưới trái)
            int antiDiagonalPlayerCount = 0;
            int antiDiagonalEmptyCount = 0;

            for (int i = 0; i < Math.Min(rows, cols); i++)
            {
                if (board[i, cols - 1 - i] == player)
                    antiDiagonalPlayerCount++;
                else if (board[i, cols - 1 - i] == ' ')
                    antiDiagonalEmptyCount++;
            }

            score += GetScore(antiDiagonalPlayerCount, antiDiagonalEmptyCount);

            return score;
        }

        int Minimax(int[,] board, int depth, int alpha, int beta, bool maximizingPlayer, int player)
        {
            if (depth == 0 || kiemTraKetThuc(board, player))
            {
                return danhGiaNuocDi(board, player);
            }

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                foreach (var move in layDanhSachNuocChuaDi(board))
                {
                    var childBoard = thucHienNuocDi(board, move, player);
                    int eval = Minimax(childBoard, depth - 1, alpha, beta, false, player);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                        break;
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (var move in layDanhSachNuocChuaDi(board))
                {
                    var childBoard = thucHienNuocDi(board, move, player);
                    int eval = Minimax(childBoard, depth - 1, alpha, beta, true, player);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                        break;
                }
                return minEval;
            }
        }

        NuocDi GetBestMove(int[,] board, int depth, int player)
        {
            NuocDi bestMove = null;
            int bestEval = int.MinValue;
            int alpha = int.MinValue;
            int beta = int.MaxValue;


            foreach (var move in layDanhSachNuocChuaDi(board))
            {
                var childBoard = thucHienNuocDi(board, move, player);
                int eval = Minimax(childBoard, depth - 1, alpha, beta, false, player);

                if (eval > bestEval)
                {
                    bestEval = eval;
                    bestMove = move;
                }
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha)
                    break; // Cắt tỉa Alpha-Beta
            }

            return bestMove;
        }

    }
}
