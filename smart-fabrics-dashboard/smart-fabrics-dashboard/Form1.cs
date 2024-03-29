﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Media;


namespace smart_fabrics_dashboard
{

    public partial class Form1 : Form
    {
        const int row = 4;
        const int col = 7;
        Thread serialThread;
        Boolean comport = false;
        int[] mpr = new int[12];
        int[] mprbase = new int[12];
        PictureBox[,] box = new PictureBox[col, row];
        PictureBox[] bar = new PictureBox[12];
        string[] ports;
        Label[,] lbl = new Label[col, row];
        Boolean setBase = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ports = SerialPort.GetPortNames();
            serialThread = new Thread(ReadSerial);
            timer1.Interval = 120;

            checkSerial();

            listView1.Columns.Add("ID");
            listView1.Columns[0].Width = 30;
            listView1.Columns.Add("Base");
            listView1.Columns.Add("Value");

            for (int i = 0; i < 12; i++)
            {
                mpr[i] = 0;
                mprbase[i] = 0;
                listView1.Items.Add(i.ToString());
                listView1.Items[i].SubItems.Add("-");
                listView1.Items[i].SubItems.Add("-");
            }
            createGrid();
        }

        public void ReadSerial()
        {
            int id;
            int val;
            int baseline;
            int count = 0;

            while (comport)
            {
                string s = serialPort1.ReadLine();
                if(s == "MPR121 not found, check wiring?")
                {
                    btnCOM.Text = "MPR!";
                    comport = false;
                    break ;
                }

                string[] spa = s.Split(':');
           
                {
                    if (spa[0] == "x")
                    {
                        id = Convert.ToInt32(spa[1]);
                        int b = Convert.ToInt32(spa[2]);
                        baseline = b;
                        int v = Convert.ToInt32(spa[3]);
                        val = v;


                        if (setBase)
                        {
                            mprbase[id] = baseline;
                            if (++count > 80)
                            {
                                count = 0;
                                setBase = false;
                            }
                        }
                        mpr[id] = val;

                    }
                }

            }
        }

        private void checkSerial()
        {
            foreach (string port in ports)
            {
                comList.Items.Add(port);
            }

            if (comList.Items.Count == 0)
            {
                comList.Items.Add("NO DEVICES FOUND");
            }
            else
            {
                comList.SetSelected(0, true);
                btnCOM.Enabled = true;
            }
        }

        private void btnCOM_Click(object sender, EventArgs e)
        {
            if (!comport)
            {
                comport = true;
                serialPort1.Open();
                serialThread.Start();
                btnCOM.Text = "EXIT";
                timer1.Enabled = true;

            }
            else
            {
                comport = false;
                Application.Exit();
            }
        }

        private void comList_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = comList.Items[0].ToString();
        }

        private void txtBaudrate_TextChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = txtBaudrate.Text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            for (int i = 0; i < 12; i++)
            {
                listView1.BeginUpdate();
                try
                {
                    if (setBase) listView1.Items[i].SubItems[1].Text = mprbase[i].ToString();
                    listView1.Items[i].SubItems[2].Text = mpr[i].ToString();
                }
                finally
                {
                    listView1.EndUpdate();
                }

                bar[i].Width = mpr[i];
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    int c = mpr[i] + mpr[j + row];
                    c = Math.Min(255, Convert.ToInt32(map(c, 0, mprbase[i] + mprbase[j + row], 0, 255)));
                    box[j, i].BackColor = Color.FromArgb(c, c, c);
                    lbl[j, i].Text = "(" + i + "," + j + ") " + c + " ["+ i+"/"+ (j+row) +"]";
                }
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < 12; i++)
            {
                listView1.Items[i].SubItems[1].Text = mprbase[i].ToString();
            }
        }


        void createGrid()
        {
            Point pos = new Point(175, 120);
            int padding = 8;
            int size = 11;

            for (int i = 0; i < 12; i++)
            {
                bar[i] = new PictureBox
                {
                    BackColor = Color.FromArgb(0, 173, 239),
                    Height = size,
                    Width = 100,
                    Left = pos.X,
                    Top = pos.Y + (i * (size + padding)),
                };
                this.Controls.Add(bar[i]);
            }

            pos = new Point(500, 15);
            padding = 5;
            size = 100;

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    box[i, j] = new PictureBox
                    {
                        Width = size,
                        Height = size,
                        BackColor = Color.Gray,
                        Left = (pos.X + (i * (size + padding))),
                        Top = (pos.Y + (j * (size + padding))),
                    };
                    this.Controls.Add(box[i, j]);

                    lbl[i, j] = new Label
                    {
                        AutoSize = true,
                        Left = box[i, j].Left + 2,
                        Top = box[i, j].Top + 2,
                        ForeColor = Color.White,
                        Font = new Font("Cascadia Mono", 7, FontStyle.Regular),
                        Text = "",
                        BackColor = Color.Black,
                        Visible = false,
                    };

                    this.Controls.Add(lbl[i, j]);
                    lbl[i, j].BringToFront();

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkSerial();
        }

        float map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            setBase = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Label l in lbl)
            {
                l.Visible = checkBox2.Checked;
            }
        }
    }
}
