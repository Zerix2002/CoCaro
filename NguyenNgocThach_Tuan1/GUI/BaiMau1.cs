using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NguyenNgocThach_Tuan1.GUI
{
    public partial class BaiMau1 : Form
    {

        List<string> danhSachHienTai;

        public BaiMau1()
        {
            InitializeComponent();
            danhSachHienTai = new List<string>();
            List<string>ketQua= new List<string>();
            docFile(ketQua, "data.txt");
            loadCheckBox(ketQua);
        }

        void docFile(List<string> danhSach, string tenFile)
        {
            using (StreamReader sr = new StreamReader("../../"+tenFile))
            {
                string line;
                
                while ((line = sr.ReadLine()) != null)
                {
                    danhSach.Add(line);
                }
            }
        }

        void loadCheckBox(List<string> danhSach)
        {
            int topPosition = 10;
            foreach (string item in danhSach)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Left = 10;
                checkBox.Top = topPosition;
                topPosition += 30;
                checkBox.Text = item;
                checkBox.CheckedChanged += checkBox_CheckedChanged;
                Controls.Add(checkBox);
            }
        }


        void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            int topOfLabel = 10;
            foreach (Control item in Controls)
            {
                if (item.GetType() == typeof(CheckBox))
                {
                    CheckBox cb = (CheckBox)item;
                    if (cb.Checked)
                    {
                        Label label = new Label();
                        label.Left = 200;
                        label.Top = topOfLabel;
                        topOfLabel += 30;
                        label.Text = cb.Text;
                        Controls.Add(label);
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}
