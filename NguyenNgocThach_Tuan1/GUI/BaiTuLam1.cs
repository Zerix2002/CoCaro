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
        int[,] banCo; // Bàn cờ (Mảng 2 chiều)
        int capMaTran = 0; // Kích thước ma trận
        bool luotDiCuaMay; // Lượt đi của máy, ngược lại với lượt đi của người chơi

        public bool LuotDiCuaMay { get => luotDiCuaMay; set => luotDiCuaMay = value; }

        public BaiTuLam1()
        {
            InitializeComponent();
            CenterToScreen();

        }

        /// <summary>
        /// Chỉnh kích thước form theo panel
        /// </summary>
        /// <param name="ctr">Form hiện tại</param>
        /// <param name="panel">Bàn cờ</param>
        public void chinhFormTheoPanel(Control ctr, Panel panel)
        {
            // Chỉnh kích thước form và bàn cờ theo kích thước ma trận
            int width = panel.Size.Width + 100; // Chiều dài form
            int height = (int)(panel_BanCo.Size.Height + 50); // Chiều cao form
            if (width <= 200 && height <= 140) // Kiểm tra chiều cao và chiều rộng tối thiểu
            {
                ctr.Size = new Size(200, 140);
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
            panel_BanCo.Size = new Size(n * kichThuoc + khoangTrong, n * kichThuoc + khoangTrong); // Chỉnh kích thước mới cho panel
            // Chỉnh lại form theo panel
            chinhFormTheoPanel(this, panel_BanCo);

            int khoangTrongPhiaTren = 10; // Khoảng trống cho rìa trên
            for (int i = 0; i < n; i++)
            {
                int khoangTrongBenTrai = 10; // Khoảng trống cho rìa trái

                for (int j = 0; j < n; j++)
                {
                    NuocDi td = new NuocDi(i, j);

                    banCo[i, j] = -1; // Gán giá trị chưa được đi cho từng ô (Chưa có nước đi => -1)

                    Button btn = new Button();
                    btn.Size = new Size(kichThuoc, kichThuoc); // Chỉnh kích thước cho từng nút
                    btn.Left = khoangTrongBenTrai;
                    khoangTrongBenTrai += kichThuoc;
                    btn.Top = khoangTrongPhiaTren;

                    btn.Tag = new int[] { td.X, td.Y };
                    btn.Click += btn_Click; // Gán event cho các nút được tạo

                    panel_BanCo.Controls.Add(btn); // Thêm nút vào panel
                }
                khoangTrongPhiaTren += kichThuoc;
            }
        }

        /// <summary>
        /// Thay đổi lượt đi hiện tại (O sang X - X sang O)
        /// </summary>
        /// <param name="btn">Button để thay đổi text</param>
        /// <param name="td">Tọa độ của điểm đang được đánh</param>
        /// <returns>False nếu còn lượt đi hoặc hòa, true nếu có lượt đi thắng</returns>
        bool thayDoiLuotDi(Button btn, NuocDi td)
        {
            int luotHienTai = luotDiCuaNguoiChoi ? 0 : 1; // lượt hiện tại là true = 0, false = 1
            btn.Text = luotDiCuaNguoiChoi ? "O" : "X";

            banCo[td.X, td.Y] = luotHienTai; // Gán giá trị cho ma trận với lượt đi hiện tại

            if (kiemTraNguoiThang(luotHienTai))
            {
                MessageBox.Show((luotDiCuaNguoiChoi ? "O" : "X") + " thắng");
                return true; // Nếu người chơi thắng
            }

            luotDiCuaNguoiChoi = !luotDiCuaNguoiChoi; // Thay đổi lượt đi hiện tại
            return false; // Nếu chưa hết nước/hòa
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int[] tag = (int[])btn.Tag;
            NuocDi td = new NuocDi(tag[0], tag[1]);
            if (btn.Text.Length == 0)
            {
                bool hoa = thayDoiLuotDi(btn, td);
                if (kiemTraBanDay() && !hoa)
                {
                    MessageBox.Show("Hòa");
                }
            }
        }

        bool kiemTraNuocDanhHopLe(NuocDi td, int n)
        {
            if (td.X >= n || td.Y >= n || td.X < 0 || td.Y < 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///  Kiểm tra bàn còn vị trí đánh hay không
        /// </summary>
        public bool kiemTraBanDay()
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

        public bool kiemTraKetThuc(int luotDiHienTai)
        {
            return kiemTraBanDay() || kiemTraNguoiThang(luotDiHienTai);
        }

        /// <summary>
        /// Kiểm tra nước đi hiện tại có thỏa điều kiện thắng hay không, với ma trận lớn hơn 5 thì kiểm tra 5 ô
        /// </summary>
        /// <param name="nuocDiHienTai"></param>
        /// <returns></returns>
        public bool kiemTraNguoiThang(int nuocDiHienTai)
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
                        if (demHangNgang == dieuKienThang)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        demHangNgang = 0;
                    }
                    if (banCo[j, i] == nuocDiHienTai)
                    {
                        demHangDoc++;
                        if (demHangDoc == dieuKienThang)
                        {
                            return true;
                        }
                    }
                    else demHangDoc = 0;
                }
            }

            // Kiểm tra đường chéo chính và phụ


            for (int i = 0; i < capMaTran; i++)
            {
                int demCheoChinh = 0;

                int demCheoPhu = 0;
                for (int j = 0; j < capMaTran; j++)
                {
                    if (banCo[j, j] == nuocDiHienTai)
                    {
                        demCheoChinh++;
                        if (demCheoChinh == dieuKienThang)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        demCheoChinh = 0;
                    }
                    if (banCo[j, capMaTran - 1 - j] == nuocDiHienTai)
                    {
                        demCheoPhu++;
                        if (demCheoPhu == dieuKienThang)
                        {
                            return true;
                        }
                    }
                    else demCheoPhu = 0;
                }
            }

            return false;
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
            taoBanCo(capMaTran, kichThuoc, khoangTrong); // Gọi hàm tạo bàn cờ

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

        int danhGiaNuocDi(int luotDiHienTai)
        {
            if (kiemTraNguoiThang(luotDiHienTai)) // Nếu máy thắng
            {
                return 10;
            }
            else if (kiemTraNguoiThang(luotDiHienTai)) // Nếu người chơi thắng
            {
                return -10;
            }
            return 0; // Hòa
        }

        /*
         def evaluate(board, player):
            # Tính toán giá trị đánh giá cho trạng thái hiện tại của bảng
            # Dựa trên cách bạn muốn đánh giá trạng thái cho người chơi hiện tại
            Máy thắng -> return 10
            Người chơi thắng -> return -10
            Hòa -> return 0

        def game_over(board):
            # Kiểm tra xem trò chơi đã kết thúc chưa (người thắng hoặc hết ô trống)
            Người hoặc máy hoặc bàn đầy -> return true
            Ngược lại false

        def get_possible_moves(board):
            # Trả về danh sách các ô trống trên bảng
            

        def make_move(board, move, player):
            # Thực hiện nước đi của người chơi tại vị trí `move` trên bảng
            banCo[move.x, move.y] = player
         */
    }
}
