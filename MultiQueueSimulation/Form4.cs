using MultiQueueModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiQueueSimulation
{
    public partial class Form4 : Form
    {
        SimulationSystem system1;
        public Form4(SimulationSystem sys)
        {
            system1 = sys;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 f3 = new Form5(system1);
            f3.ShowDialog();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            textBox1.Text = system1.PerformanceMeasures.AverageWaitingTime.ToString();
            textBox2.Text = system1.PerformanceMeasures.MaxQueueLength.ToString();
            textBox3.Text = system1.PerformanceMeasures.WaitingProbability.ToString();

            richTextBox1.Font = new Font("Tahoma", 11, FontStyle.Bold);
            richTextBox1.ForeColor = Color.Black;
            richTextBox1.AppendText("   Server       average service time      probabilty of idle      utilization");

            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText(Environment.NewLine);

            for (int i = 0; i < system1.Servers.Count(); i++)
            {



                richTextBox1.AppendText("      " + system1.Servers[i].ID.ToString() + "                      " + system1.Servers[i].AverageServiceTime + "                     " + system1.Servers[i].IdleProbability + "                 " + system1.Servers[i].Utilization.ToString() + Environment.NewLine + Environment.NewLine);


            }
            this.Controls.Add(richTextBox1);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
