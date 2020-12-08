using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TextMerge
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public List<string> Paths;
        public StringBuilder Sb;

        private void btnChoose_Click(object sender, EventArgs e)
        {
            Paths = new List<string>();
            Sb = new StringBuilder();
            lblLog.Text = string.Empty;
            btnStart.Enabled = true;
            var dialog = folderBrowserDialog1.ShowDialog();
            if (dialog == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog1.SelectedPath;
                GetDirectory(txtPath.Text);
                lblLog.Text = $"解析成功，共找到{Paths.Count}个文件。";
            }
        }

        public void GetFileName(string path)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (FileInfo f in root.GetFiles("*.txt"))
            {
                Paths.Add(f.FullName);
            }
        }

        public void GetDirectory(string path)
        {
            GetFileName(path);
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                GetDirectory(d.FullName);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            btnStart.Enabled = false;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= Paths.Count; i++)
            {
                var path = Paths[i - 1];
                backgroundWorker1.ReportProgress(i);
                Sb.Append(File.ReadAllText(path, Encoding.UTF8));
            }
            File.WriteAllText("D:\\output.txt", Sb.ToString(), Encoding.UTF8);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblLog.Text = $"正在导出第{e.ProgressPercentage}/{Paths.Count}个， D:\\output.txt。";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblLog.Text = $"导出完成，路径在 D:\\output.txt。";
            btnStart.Enabled = true;
        }
    }
}
