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
         * Chưa được đánh là -1
         * O = 0, 1 = X
         */

        bool luotDi = true; // Bước đi <=> lượt đánh
        int[,] banCo; // Bàn cờ (Mảng 2 chiều)
        int capMaTran = 0; // Kích thước ma trận

        public BaiTuLam1()
        {
            InitializeComponent();
            CenterToScreen();

        }

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

        void taoBanCo(int n, int kichThuoc, int khoangTrong)
        {
            banCo = new int[kichThuoc, kichThuoc]; // Khởi tạo bàn cờ

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
                    banCo[i, j] = -1; // Gán giá trị chưa được đi cho từng ô (Chưa có nước đi => -1)

                    Button btn = new Button();
                    btn.Size = new Size(kichThuoc, kichThuoc); // Chỉnh kích thước cho từng nút
                    btn.Left = khoangTrongBenTrai;
                    khoangTrongBenTrai += kichThuoc;
                    btn.Top = khoangTrongPhiaTren;
                    btn.Tag = new int[] { i, j };
                    btn.Click += btn_Click; // Gán event cho các nút được tạo

                    panel_BanCo.Controls.Add(btn); // Thêm nút vào panel
                }
                khoangTrongPhiaTren += kichThuoc;
            }
        }

        bool thayDoiBuocDi(Button btn, int dong, int cot)
        {
            int luotHienTai = luotDi ? 0 : 1; // lượt hiện tại là true = 0, false = 1
            btn.Text = luotDi ? "O" : "X";
            
            banCo[dong, cot] = luotHienTai; // Gán giá trị cho ma trận với lượt đi hiện tại
            if (kiemTraNguoiThang(luotHienTai))
            {
                MessageBox.Show((luotDi ? "O" : "X") + " thắng");
                return true; // Nếu không hoà
            }
            luotDi = !luotDi; // Thay đổi lượt đi hiện tại
            return false; // Nếu chưa hết nước/hòa
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int dong = ((int[])btn.Tag)[0];
            int cot = ((int[])btn.Tag)[1];
            bool hoa = false;
            if (btn.Text.Length == 0)
            {
                hoa = thayDoiBuocDi(btn, dong, cot);
                if (kiemTraHoa() && !hoa)
                {
                    MessageBox.Show("Hòa");
                }
            }
        }

        bool kiemTraHoa()
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

        bool kiemTraNguoiThang(int nuocDiHienTai)
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

        private void batDau(bool nuocDiTruoc)
        {// Event của nút xác nhận, khi nhấn sẽ lấy nội dung trên textbox CapMaTran để tạo bàn cờ và chọn nước được đi trước
            if (txtCapMaTran.Text.Trim().Length != 0 && txtCapMaTran.Text.All(c => c >= '0' && c <= '9'))
            { // Kiểm tra textbox không rỗng và là số
                capMaTran = int.Parse(txtCapMaTran.Text); // Cấp ma trận nhập từ textbox
                int kichThuoc = 30; // Kích thước cho từng button
                int khoangTrong = (int)(kichThuoc * 2 / 3); // Khoảng trống cho phần rìa
                luotDi = nuocDiTruoc;
                taoBanCo(capMaTran, kichThuoc, khoangTrong); // Gọi hàm tạo bàn cờ
            }
        }

        private void btnO_Click(object sender, EventArgs e)
        {
            batDau(true); // O đi trước
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            batDau(false); // X đi trước
        }


    }
}
