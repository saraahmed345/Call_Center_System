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
    
    public partial class Form5 : Form
    {
        SimulationSystem system1;
        public Form5(SimulationSystem sys)
        {
            InitializeComponent();
            system1 = sys;

            int numofServ = sys.NumberOfServers;//EDIT GET THE NUMBER OF SERVERS
            //=======================================================
            for (int i = 0; i < numofServ; i++)
            {

                comboBox1.Items.Add("Server " + (i + 1).ToString());

            }
        
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = comboBox1.SelectedIndex;

            for (int j = 0; j < system1.StoppingNumber; j++)
            {
                if (system1.SimulationTable[j].AssignedServer.ID == id + 1)
                {
                    int Start = system1.SimulationTable[j].StartTime;
                    int End = system1.SimulationTable[j].EndTime;


                    for (int c = Start; c <= End; c++)
                    {
                        this.chart1.Series["Series1"].Points.AddXY(c, 1);
                    }

                }
            }
        }
    }
}
