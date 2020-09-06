using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            DataTable dt = new DataTable();
            dataGridView1.DataSource = dt;
            toolStripProgressBar1.Value = 0;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Trim() != "")
            {
                Hometax hometax = new Hometax();
                string[] list = richTextBox1.Text.ToString().Replace("-", "").Split('\n');
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = list.Length;
                toolStripProgressBar1.Step = 1;

                DataTable dt = new DataTable();
                dt.Columns.Add("cnt");
                dt.Columns.Add("cname");
                dt.Columns.Add("name");
                dt.Columns.Add("status");
                dt.Columns.Add("addr");

                if (list.Length > 0)
                {
                    foreach(var arg in list)
                    {
                        var datas = hometax.postCRN(arg);
                        dt.Rows.Add(datas.cnt, datas.cname, datas.name, datas.status, datas.addr);

                        toolStripProgressBar1.PerformStep();
                        await this.SometingAsync();
                    }
                    toolStripProgressBar1.Value = 0;
                }
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].HeaderText = "사업자번호";
                dataGridView1.Columns[0].Width = 100;
                dataGridView1.Columns[1].HeaderText = "상호";
                dataGridView1.Columns[1].Width = 100;
                dataGridView1.Columns[2].HeaderText = "대표자명";
                dataGridView1.Columns[2].Width = 100;
                dataGridView1.Columns[3].HeaderText = "상태";
                dataGridView1.Columns[3].Width = 200;
                dataGridView1.Columns[4].HeaderText = "주소";
                dataGridView1.Columns[4].Width = 400;


            }
            else
            {
                MessageBox.Show("사업자번호를 입력하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private async Task SometingAsync()
        {
            await Task.Delay(1000);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "117-81-40065\n211-85-41856";
        }
    }
}
