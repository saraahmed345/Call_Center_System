using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueModels;
using MultiQueueTesting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace MultiQueueSimulation
{
    public partial class Form1 : Form
    {
        static string path;
        
        List<SimulationCase> simulationtable;
        static SimulationSystem system2;
        public Form1(string pathinp)
        {
            path = pathinp;
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //dataGridView1.DataSource = simulationtable;
            if (path == null)

            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                button3.Enabled = false;

            }
            else 
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled= false;
               
                button3.Enabled = true;
                setPath(path);
                int nOServ = system2.NumberOfServers;
                
                
                for (int i = 0; i < nOServ; i++)
                {
                    dataGridView1.Rows.Add("Server " + (i + 1));


                }


            }
            
        }
        public static void setSystem(SimulationSystem sys)
        {
            system2 = sys;

            //dataGridView1.DataSource = simulationtable;

        }

        public static string getPath() 
        {
            return path;
        }
        public static void setPath(string pa)
        {
            path = pa;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4(system2);
            frm4.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            button3.Enabled = true;
            path = "..\\localManualTest.txt";
            
            
            
            this.Close();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                simulationtable = system2.return_data_of_server(e.RowIndex + 1);
                Form2 frm = new Form2(simulationtable);


                frm.ShowDialog();
            }
            //dataGridView1.Rows[e.RowIndex]
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select File";
            fileDialog.Filter = "ALl files (*.*)|*.*|Text file (*.txt)|*.txt";
            fileDialog.FilterIndex = 1;
            fileDialog.ShowDialog();

            if (fileDialog.FileName == null || fileDialog.FileName == "")
            {

                return;

            }
            else
            {

                path = fileDialog.FileName;
                this.Close();


            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select File";
            fileDialog.Filter = "ALl files (*.*)|*.*|Text file (*.txt)|*.txt";
            fileDialog.FilterIndex = 1;
            fileDialog.ShowDialog();

            if (fileDialog.FileName == null || fileDialog.FileName == "")
            {

                return;

            }
            else
            {

                path = fileDialog.FileName;
                this.Close();


            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select File";
            fileDialog.Filter = "ALl files (*.*)|*.*|Text file (*.txt)|*.txt";
            fileDialog.FilterIndex = 1;
            fileDialog.ShowDialog();

            if (fileDialog.FileName == null || fileDialog.FileName == "")
            {

                return;

            }
            else
            {

                path = fileDialog.FileName;
                this.Close();


            }
        }
    }
}
