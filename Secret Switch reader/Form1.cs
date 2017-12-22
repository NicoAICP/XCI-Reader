using DamienG.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Secret_Switch_reader
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(this.button1, "Choose your .nci rom");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DB.xml")))
            {

            }
            else
            {
                MessageBox.Show("DB is missing, downloading DB");
                using (var client = new WebClient())
                {

                    client.DownloadFile("https://raw.githubusercontent.com/Hotbrawl20/db/master/NSWreleases.xml", "DB.xml");
                    string NEWDB_location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DB.xml");
                    string OLDDB_locarion = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DB.xml");
                    File.Move(OLDDB_locarion, NEWDB_location);
                }
            }
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "XCI Files (*.xci)|*.xci";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string rom = openFileDialog1.FileName;
                textBox1.Text = rom;

                textBox5.Text = new FileInfo(openFileDialog1.FileName).Length.ToString();
                BinaryReader reader = new BinaryReader(new FileStream(openFileDialog1.FileName, FileMode.Open));
                reader.BaseStream.Position = 0x10D;
                textBox10.Text = BitConverter.ToString(reader.ReadBytes(1)).Replace("-", null);
                if (textBox10.Text == "F8")
                {
                    textBox10.Text = "2 GB";
                }
                else
                {
                    if (textBox10.Text == "F0")
                    {
                        textBox10.Text = "4 GB";

                    }
                    else
                    {
                        if (textBox10.Text == "E0")
                        {
                            textBox10.Text = "8 GB";
                        }
                        else
                        {
                            if (textBox10.Text == "E1")
                            {
                                textBox10.Text = "16 GB";
                            }
                            else
                            {
                                MessageBox.Show("Error");

                            }
                        }
                    }
                }
                reader.Close();

                if (textBox10.Text == "2 GB")
                {
                    if (textBox5.Text == "1996488704")
                    {
                        textBox5.BackColor = Color.Green;
                        textBox5.Text = "1996488704 Bytes";
                    }
                    else { textBox5.BackColor = Color.DarkRed; }
                } else if(textBox10.Text == "4 GB")
                {
                    if (textBox5.Text == "4294967296")
                    {
                        textBox5.BackColor = Color.Green;
                        textBox5.Text = "4294967296 Bytes";
                    }
                    else
                    {
                        textBox5.BackColor = Color.DarkRed;
                    }
                } else if(textBox10.Text == "8 GB")
                {
                    if(textBox5.Text == "8589934592")
                    {
                        textBox5.BackColor = Color.Green;
                        textBox5.Text = "8589934592 Bytes";
                    } else
                    {
                        textBox5.BackColor = Color.DarkRed;
                    }
                } else if(textBox10.Text == "16 GB")
                {
                    if (textBox5.Text == "17179869184") {
                        textBox5.BackColor = Color.Green;
                        textBox5.Text = "17179869184 Bytes";
                    }else
                    {
                        textBox5.BackColor = Color.DarkRed;
                    }
                }
                
                    Crc32 crc32 = new Crc32();
                    String hash = String.Empty;

                    using (FileStream fs = File.Open(textBox1.Text, FileMode.Open))
                        foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();

                    textBox6.Text = hash;
                    XmlDocument doc = new XmlDocument();
                    doc.Load(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DB.xml"));
                    XmlNode node = doc.SelectSingleNode("releases/release1/imgcrc");
                    if (textBox6.Text == node.InnerText)
                    {
                        XmlNode id = doc.SelectSingleNode("releases/release1/titleid");
                        XmlNode serial = doc.SelectSingleNode("releases/release1/serial");
                        XmlNode r = doc.SelectSingleNode("releases/release1/region");
                        XmlNode fw = doc.SelectSingleNode("releases/release1/firmware");

                        textBox8.Text = id.InnerText;
                        textBox4.Text = r.InnerText;
                        if (textBox4.Text == "WLD")
                        {
                            textBox4.Text = "Worldwide";
                        }
                        textBox3.Text = fw.InnerText;
                        if (textBox3.Text == "")
                        {
                            textBox3.Text = "The required FW wasn't found yet";
                        }
                        textBox2.Text = serial.InnerText;
                    }
                    else
                    {
                        XmlNode node1 = doc.SelectSingleNode("releases/release2/imgcrc");
                        if (textBox6.Text == node1.InnerText)
                        {
                            XmlNode id = doc.SelectSingleNode("releases/release2/titleid");
                            XmlNode serial = doc.SelectSingleNode("releases/release2/serial");
                            XmlNode r = doc.SelectSingleNode("releases/release2/region");
                            XmlNode fw = doc.SelectSingleNode("releases/release2/firmware");

                            textBox8.Text = id.InnerText;
                            textBox4.Text = r.InnerText;
                            if (textBox4.Text == "WLD")
                            {
                                textBox4.Text = "Worldwide";
                            }
                            textBox3.Text = fw.InnerText;
                            if (textBox3.Text == "")
                            {
                                textBox3.Text = "The required FW wasn't found yet";
                            }
                            textBox2.Text = serial.InnerText;

                        }
                        else
                        {
                            XmlNode node2 = doc.SelectSingleNode("releases/release3/imgcrc");
                            if (textBox6.Text == node2.InnerText)
                            {
                                XmlNode id = doc.SelectSingleNode("releases/release3/titleid");
                                XmlNode serial = doc.SelectSingleNode("releases/release3/serial");
                                XmlNode r = doc.SelectSingleNode("releases/release3/region");
                                XmlNode fw = doc.SelectSingleNode("releases/release3/firmware");

                                textBox8.Text = id.InnerText;
                                textBox4.Text = r.InnerText;
                                if (textBox4.Text == "WLD")
                                {
                                    textBox4.Text = "Worldwide";
                                }
                                textBox3.Text = fw.InnerText;
                                if (textBox3.Text == "")
                                {
                                    textBox3.Text = "The required FW wasn't found yet";
                                }
                                textBox2.Text = serial.InnerText;
                            }
                            else
                            {
                                XmlNode node3 = doc.SelectSingleNode("releases/release4/imgcrc");
                                if (textBox6.Text == node3.InnerText)
                                {
                                    XmlNode id = doc.SelectSingleNode("releases/release4/titleid");
                                    XmlNode serial = doc.SelectSingleNode("releases/release4/serial");
                                    XmlNode r = doc.SelectSingleNode("releases/release4/region");
                                    XmlNode fw = doc.SelectSingleNode("releases/release4/firmware");

                                    textBox8.Text = id.InnerText;
                                    textBox4.Text = r.InnerText;
                                    if (textBox4.Text == "WLD")
                                    {
                                        textBox4.Text = "Worldwide";
                                    }
                                    textBox3.Text = fw.InnerText;
                                    if (textBox3.Text == "")
                                    {
                                        textBox3.Text = "The required FW wasn't found yet";
                                    }
                                    textBox2.Text = serial.InnerText;
                                }
                                else
                                {
                                    XmlNode node4 = doc.SelectSingleNode("releases/release3/imgcrc");
                                    if (textBox6.Text == node4.InnerText)
                                    {
                                        XmlNode id = doc.SelectSingleNode("releases/release5/titleid");
                                        XmlNode serial = doc.SelectSingleNode("releases/release5/serial");
                                        XmlNode r = doc.SelectSingleNode("releases/release5/region");
                                        XmlNode fw = doc.SelectSingleNode("releases/release5/firmware");

                                        textBox8.Text = id.InnerText;
                                        textBox4.Text = r.InnerText;
                                        if (textBox4.Text == "WLD")
                                        {
                                            textBox4.Text = "Worldwide";
                                        }
                                        textBox3.Text = fw.InnerText;
                                        if (textBox3.Text == "")
                                        {
                                            textBox3.Text = "The required FW wasn't found yet";
                                        }
                                        textBox2.Text = serial.InnerText;
                                    }
                                    else
                                    {
                                        XmlNode node5 = doc.SelectSingleNode("releases/release6/imgcrc");
                                        if (textBox6.Text == node5.InnerText)
                                        {
                                            XmlNode id = doc.SelectSingleNode("releases/release6/titleid");
                                            XmlNode serial = doc.SelectSingleNode("releases/release6/serial");
                                            XmlNode r = doc.SelectSingleNode("releases/release6/region");
                                            XmlNode fw = doc.SelectSingleNode("releases/release6/firmware");

                                            textBox8.Text = id.InnerText;
                                            textBox4.Text = r.InnerText;
                                            if (textBox4.Text == "WLD")
                                            {
                                                textBox4.Text = "Worldwide";
                                            }
                                            textBox3.Text = fw.InnerText;
                                            if (textBox3.Text == "")
                                            {
                                                textBox3.Text = "The required FW wasn't found yet";
                                            }
                                            textBox2.Text = serial.InnerText;
                                        }
                                        else
                                        {
                                            XmlNode node6 = doc.SelectSingleNode("releases/release7/imgcrc");
                                            if (textBox6.Text == node6.InnerText)
                                            {
                                                XmlNode id = doc.SelectSingleNode("releases/release7/titleid");
                                                XmlNode serial = doc.SelectSingleNode("releases/release7/serial");
                                                XmlNode r = doc.SelectSingleNode("releases/release7/region");
                                                XmlNode fw = doc.SelectSingleNode("releases/release7/firmware");

                                                textBox8.Text = id.InnerText;
                                                textBox4.Text = r.InnerText;
                                                if (textBox4.Text == "WLD")
                                                {
                                                    textBox4.Text = "Worldwide";
                                                }
                                                textBox3.Text = fw.InnerText;
                                                if (textBox3.Text == "")
                                                {
                                                    textBox3.Text = "The required FW wasn't found yet";
                                                }
                                                textBox2.Text = serial.InnerText;
                                            }
                                            else
                                            {
                                                XmlNode node7 = doc.SelectSingleNode("releases/release8/imgcrc");
                                                if (textBox6.Text == node7.InnerText)
                                                {
                                                    XmlNode id = doc.SelectSingleNode("releases/release8/titleid");
                                                    XmlNode serial = doc.SelectSingleNode("releases/release8/serial");
                                                    XmlNode r = doc.SelectSingleNode("releases/release8/region");
                                                    XmlNode fw = doc.SelectSingleNode("releases/release8/firmware");

                                                    textBox8.Text = id.InnerText;
                                                    textBox4.Text = r.InnerText;
                                                    if (textBox4.Text == "WLD")
                                                    {
                                                        textBox4.Text = "Worldwide";
                                                    }
                                                    textBox3.Text = fw.InnerText;
                                                    if (textBox3.Text == "")
                                                    {
                                                        textBox3.Text = "The required FW wasn't found yet";
                                                    }
                                                    textBox2.Text = serial.InnerText;
                                                }
                                                else
                                                {
                                                    XmlNode node8 = doc.SelectSingleNode("releases/release9/imgcrc");
                                                    if (textBox6.Text == node8.InnerText)
                                                    {
                                                        XmlNode id = doc.SelectSingleNode("releases/release9/titleid");
                                                        XmlNode serial = doc.SelectSingleNode("releases/release9/serial");
                                                        XmlNode r = doc.SelectSingleNode("releases/release9/region");
                                                        XmlNode fw = doc.SelectSingleNode("releases/release9/firmware");

                                                        textBox8.Text = id.InnerText;
                                                        textBox4.Text = r.InnerText;
                                                        if (textBox4.Text == "WLD")
                                                        {
                                                            textBox4.Text = "Worldwide";
                                                        }
                                                        textBox3.Text = fw.InnerText;
                                                        if (textBox3.Text == "")
                                                        {
                                                            textBox3.Text = "The required FW wasn't found yet";
                                                        }
                                                        textBox2.Text = serial.InnerText;
                                                    }
                                                    else
                                                    {
                                                        XmlNode node9 = doc.SelectSingleNode("releases/release10/imgcrc");
                                                        if (textBox6.Text == node9.InnerText)
                                                        {
                                                            XmlNode id = doc.SelectSingleNode("releases/release10/titleid");
                                                            XmlNode serial = doc.SelectSingleNode("releases/release10/serial");
                                                            XmlNode r = doc.SelectSingleNode("releases/release10/region");
                                                            XmlNode fw = doc.SelectSingleNode("releases/release10/firmware");

                                                            textBox8.Text = id.InnerText;
                                                            textBox4.Text = r.InnerText;
                                                            if (textBox4.Text == "WLD")
                                                            {
                                                                textBox4.Text = "Worldwide";
                                                            }
                                                            textBox3.Text = fw.InnerText;
                                                            if (textBox3.Text == "")
                                                            {
                                                                textBox3.Text = "The required FW wasn't found yet";
                                                            }
                                                            textBox2.Text = serial.InnerText;
                                                        }
                                                        else
                                                        {
                                                            XmlNode node10 = doc.SelectSingleNode("releases/release11/imgcrc");
                                                            if (textBox6.Text == node10.InnerText)
                                                            {
                                                                XmlNode id = doc.SelectSingleNode("releases/release11/titleid");
                                                                XmlNode serial = doc.SelectSingleNode("releases/release11/serial");
                                                                XmlNode r = doc.SelectSingleNode("releases/release11/region");
                                                                XmlNode fw = doc.SelectSingleNode("releases/release11/firmware");

                                                                textBox8.Text = id.InnerText;
                                                                textBox4.Text = r.InnerText;
                                                                if (textBox4.Text == "WLD")
                                                                {
                                                                    textBox4.Text = "Worldwide";
                                                                }
                                                                textBox3.Text = fw.InnerText;
                                                                if (textBox3.Text == "")
                                                                {
                                                                    textBox3.Text = "The required FW wasn't found yet";
                                                                }
                                                                textBox2.Text = serial.InnerText;
                                                            }
                                                            else
                                                            {
                                                                XmlNode node11 = doc.SelectSingleNode("releases/release12/imgcrc");
                                                                if (textBox6.Text == node11.InnerText)
                                                                {
                                                                    XmlNode id = doc.SelectSingleNode("releases/release12/titleid");
                                                                    XmlNode serial = doc.SelectSingleNode("releases/release12/serial");
                                                                    XmlNode r = doc.SelectSingleNode("releases/release12/region");
                                                                    XmlNode fw = doc.SelectSingleNode("releases/release12/firmware");

                                                                    textBox8.Text = id.InnerText;
                                                                    textBox4.Text = r.InnerText;
                                                                    if (textBox4.Text == "WLD")
                                                                    {
                                                                        textBox4.Text = "Worldwide";
                                                                    }
                                                                    textBox3.Text = fw.InnerText;
                                                                    if (textBox3.Text == "")
                                                                    {
                                                                        textBox3.Text = "The required FW wasn't found yet";
                                                                    }
                                                                    textBox2.Text = serial.InnerText;
                                                                }
                                                                else
                                                                {
                                                                    XmlNode node12 = doc.SelectSingleNode("releases/release13/imgcrc");
                                                                    if (textBox6.Text == node12.InnerText)
                                                                    {
                                                                        XmlNode id = doc.SelectSingleNode("releases/release13/titleid");
                                                                        XmlNode serial = doc.SelectSingleNode("releases/release13/serial");
                                                                        XmlNode r = doc.SelectSingleNode("releases/release13/region");
                                                                        XmlNode fw = doc.SelectSingleNode("releases/release13/firmware");

                                                                        textBox8.Text = id.InnerText;
                                                                        textBox4.Text = r.InnerText;
                                                                        if (textBox4.Text == "WLD")
                                                                        {
                                                                            textBox4.Text = "Worldwide";
                                                                        }
                                                                        textBox3.Text = fw.InnerText;
                                                                        if (textBox3.Text == "")
                                                                        {
                                                                            textBox3.Text = "The required FW wasn't found yet";
                                                                        }
                                                                        textBox2.Text = serial.InnerText;
                                                                    }
                                                                    else
                                                                    {
                                                                        XmlNode node13 = doc.SelectSingleNode("releases/release14/imgcrc");
                                                                        if (textBox6.Text == node13.InnerText)
                                                                        {
                                                                            XmlNode id = doc.SelectSingleNode("releases/release14/titleid");
                                                                            XmlNode serial = doc.SelectSingleNode("releases/release14/serial");
                                                                            XmlNode r = doc.SelectSingleNode("releases/release14/region");
                                                                            XmlNode fw = doc.SelectSingleNode("releases/release14/firmware");

                                                                            textBox8.Text = id.InnerText;
                                                                            textBox4.Text = r.InnerText;
                                                                            if (textBox4.Text == "WLD")
                                                                            {
                                                                                textBox4.Text = "Worldwide";
                                                                            }
                                                                            textBox3.Text = fw.InnerText;
                                                                            if (textBox3.Text == "")
                                                                            {
                                                                                textBox3.Text = "The required FW wasn't found yet";
                                                                            }
                                                                            textBox2.Text = serial.InnerText;
                                                                        }
                                                                        else
                                                                        {
                                                                            XmlNode node14 = doc.SelectSingleNode("releases/release15/imgcrc");
                                                                            if (textBox6.Text == node14.InnerText)
                                                                            {
                                                                                XmlNode id = doc.SelectSingleNode("releases/release15/titleid");
                                                                                XmlNode serial = doc.SelectSingleNode("releases/release15/serial");
                                                                                XmlNode r = doc.SelectSingleNode("releases/release15/region");
                                                                                XmlNode fw = doc.SelectSingleNode("releases/release15/firmware");

                                                                                textBox8.Text = id.InnerText;
                                                                                textBox4.Text = r.InnerText;
                                                                                if (textBox4.Text == "WLD")
                                                                                {
                                                                                    textBox4.Text = "Worldwide";
                                                                                }
                                                                                textBox3.Text = fw.InnerText;
                                                                                if (textBox3.Text == "")
                                                                                {
                                                                                    textBox3.Text = "The required FW wasn't found yet";
                                                                                }
                                                                                textBox2.Text = serial.InnerText;
                                                                            }
                                                                            else
                                                                            {
                                                                                XmlNode node15 = doc.SelectSingleNode("releases/release16/imgcrc");
                                                                                if (textBox6.Text == node15.InnerText)
                                                                                {
                                                                                    XmlNode id = doc.SelectSingleNode("releases/release16/titleid");
                                                                                    XmlNode serial = doc.SelectSingleNode("releases/release16/serial");
                                                                                    XmlNode r = doc.SelectSingleNode("releases/release16/region");
                                                                                    XmlNode fw = doc.SelectSingleNode("releases/release16/firmware");

                                                                                    textBox8.Text = id.InnerText;
                                                                                    textBox4.Text = r.InnerText;
                                                                                    if (textBox4.Text == "WLD")
                                                                                    {
                                                                                        textBox4.Text = "Worldwide";
                                                                                    }
                                                                                    textBox3.Text = fw.InnerText;
                                                                                    if (textBox3.Text == "")
                                                                                    {
                                                                                        textBox3.Text = "The required FW wasn't found yet";
                                                                                    }
                                                                                    textBox2.Text = serial.InnerText;
                                                                                }
                                                                                else
                                                                                {
                                                                                    XmlNode node16 = doc.SelectSingleNode("releases/release17/imgcrc");
                                                                                    if (textBox6.Text == node16.InnerText)
                                                                                    {
                                                                                        XmlNode id = doc.SelectSingleNode("releases/release17/titleid");
                                                                                        XmlNode serial = doc.SelectSingleNode("releases/release17/serial");
                                                                                        XmlNode r = doc.SelectSingleNode("releases/release17/region");
                                                                                        XmlNode fw = doc.SelectSingleNode("releases/release17/firmware");

                                                                                        textBox8.Text = id.InnerText;
                                                                                        textBox4.Text = r.InnerText;
                                                                                        if (textBox4.Text == "WLD")
                                                                                        {
                                                                                            textBox4.Text = "Worldwide";
                                                                                        }
                                                                                        textBox3.Text = fw.InnerText;
                                                                                        if (textBox3.Text == "")
                                                                                        {
                                                                                            textBox3.Text = "The required FW wasn't found yet";
                                                                                        }
                                                                                        textBox2.Text = serial.InnerText;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        XmlNode node17 = doc.SelectSingleNode("releases/release18/imgcrc");
                                                                                        if (textBox6.Text == node17.InnerText)
                                                                                        {
                                                                                            XmlNode id = doc.SelectSingleNode("releases/release18/titleid");
                                                                                            XmlNode serial = doc.SelectSingleNode("releases/release18/serial");
                                                                                            XmlNode r = doc.SelectSingleNode("releases/release18/region");
                                                                                            XmlNode fw = doc.SelectSingleNode("releases/release18/firmware");

                                                                                            textBox8.Text = id.InnerText;
                                                                                            textBox4.Text = r.InnerText;
                                                                                            if (textBox4.Text == "WLD")
                                                                                            {
                                                                                                textBox4.Text = "Worldwide";
                                                                                            }
                                                                                            textBox3.Text = fw.InnerText;
                                                                                            if (textBox3.Text == "")
                                                                                            {
                                                                                                textBox3.Text = "The required FW wasn't found yet";
                                                                                            }
                                                                                            textBox2.Text = serial.InnerText;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            XmlNode node18 = doc.SelectSingleNode("releases/release19/imgcrc");
                                                                                            if (textBox6.Text == node18.InnerText)
                                                                                            {
                                                                                                XmlNode id = doc.SelectSingleNode("releases/release19/titleid");
                                                                                                XmlNode serial = doc.SelectSingleNode("releases/release19/serial");
                                                                                                XmlNode r = doc.SelectSingleNode("releases/release19/region");
                                                                                                XmlNode fw = doc.SelectSingleNode("releases/release19/firmware");

                                                                                                textBox8.Text = id.InnerText;
                                                                                                textBox4.Text = r.InnerText;
                                                                                                if (textBox4.Text == "WLD")
                                                                                                {
                                                                                                    textBox4.Text = "Worldwide";
                                                                                                }
                                                                                                textBox3.Text = fw.InnerText;
                                                                                                if (textBox3.Text == "")
                                                                                                {
                                                                                                    textBox3.Text = "The required FW wasn't found yet";
                                                                                                }
                                                                                                textBox2.Text = serial.InnerText;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                XmlNode node19 = doc.SelectSingleNode("releases/release20/imgcrc");
                                                                                                if (textBox6.Text == node19.InnerText)
                                                                                                {
                                                                                                    XmlNode id = doc.SelectSingleNode("releases/release20/titleid");
                                                                                                    XmlNode serial = doc.SelectSingleNode("releases/release20/serial");
                                                                                                    XmlNode r = doc.SelectSingleNode("releases/release20/region");
                                                                                                    XmlNode fw = doc.SelectSingleNode("releases/release20/firmware");

                                                                                                    textBox8.Text = id.InnerText;
                                                                                                    textBox4.Text = r.InnerText;
                                                                                                    if (textBox4.Text == "WLD")
                                                                                                    {
                                                                                                        textBox4.Text = "Worldwide";
                                                                                                    }
                                                                                                    textBox3.Text = fw.InnerText;
                                                                                                    if (textBox3.Text == "")
                                                                                                    {
                                                                                                        textBox3.Text = "The required FW wasn't found yet";
                                                                                                    }
                                                                                                    textBox2.Text = serial.InnerText;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    XmlNode node20 = doc.SelectSingleNode("releases/release21/imgcrc");
                                                                                                    if (textBox6.Text == node20.InnerText)
                                                                                                    {
                                                                                                        XmlNode id = doc.SelectSingleNode("releases/release21/titleid");
                                                                                                        XmlNode serial = doc.SelectSingleNode("releases/release21/serial");
                                                                                                        XmlNode r = doc.SelectSingleNode("releases/release21/region");
                                                                                                        XmlNode fw = doc.SelectSingleNode("releases/release21/firmware");

                                                                                                        textBox8.Text = id.InnerText;
                                                                                                        textBox4.Text = r.InnerText;
                                                                                                        if (textBox4.Text == "WLD")
                                                                                                        {
                                                                                                            textBox4.Text = "Worldwide";
                                                                                                        }
                                                                                                        textBox3.Text = fw.InnerText;
                                                                                                        if (textBox3.Text == "")
                                                                                                        {
                                                                                                            textBox3.Text = "The required FW wasn't found yet";
                                                                                                        }
                                                                                                        textBox2.Text = serial.InnerText;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        XmlNode node21 = doc.SelectSingleNode("releases/release22/imgcrc");
                                                                                                        if (textBox6.Text == node21.InnerText)
                                                                                                        {
                                                                                                            XmlNode id = doc.SelectSingleNode("releases/release22/titleid");
                                                                                                            XmlNode serial = doc.SelectSingleNode("releases/release22/serial");
                                                                                                            XmlNode r = doc.SelectSingleNode("releases/release22/region");
                                                                                                            XmlNode fw = doc.SelectSingleNode("releases/release22/firmware");

                                                                                                            textBox8.Text = id.InnerText;
                                                                                                            textBox4.Text = r.InnerText;
                                                                                                            if (textBox4.Text == "WLD")
                                                                                                            {
                                                                                                                textBox4.Text = "Worldwide";
                                                                                                            }
                                                                                                            textBox3.Text = fw.InnerText;
                                                                                                            if (textBox3.Text == "")
                                                                                                            {
                                                                                                                textBox3.Text = "The required FW wasn't found yet";
                                                                                                            }
                                                                                                            textBox2.Text = serial.InnerText;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            XmlNode node22 = doc.SelectSingleNode("releases/release23/imgcrc");
                                                                                                            if (textBox6.Text == node22.InnerText)
                                                                                                            {
                                                                                                                XmlNode id = doc.SelectSingleNode("releases/release23/titleid");
                                                                                                                XmlNode serial = doc.SelectSingleNode("releases/release23/serial");
                                                                                                                XmlNode r = doc.SelectSingleNode("releases/release23/region");
                                                                                                                XmlNode fw = doc.SelectSingleNode("releases/release23/firmware");

                                                                                                                textBox8.Text = id.InnerText;
                                                                                                                textBox4.Text = r.InnerText;
                                                                                                                if (textBox4.Text == "WLD")
                                                                                                                {
                                                                                                                    textBox4.Text = "Worldwide";
                                                                                                                }
                                                                                                                textBox3.Text = fw.InnerText;
                                                                                                                if (textBox3.Text == "")
                                                                                                                {
                                                                                                                    textBox3.Text = "The required FW wasn't found yet";
                                                                                                                }
                                                                                                                textBox2.Text = serial.InnerText;
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                XmlNode node23 = doc.SelectSingleNode("releases/release24/imgcrc");
                                                                                                                if (textBox6.Text == node23.InnerText)
                                                                                                                {
                                                                                                                    XmlNode id = doc.SelectSingleNode("releases/release24/titleid");
                                                                                                                    XmlNode serial = doc.SelectSingleNode("releases/release24/serial");
                                                                                                                    XmlNode r = doc.SelectSingleNode("releases/release24/region");
                                                                                                                    XmlNode fw = doc.SelectSingleNode("releases/release24/firmware");

                                                                                                                    textBox8.Text = id.InnerText;
                                                                                                                    textBox4.Text = r.InnerText;
                                                                                                                    if (textBox4.Text == "WLD")
                                                                                                                    {
                                                                                                                        textBox4.Text = "Worldwide";
                                                                                                                    }
                                                                                                                    textBox3.Text = fw.InnerText;
                                                                                                                    if (textBox3.Text == "")
                                                                                                                    {
                                                                                                                        textBox3.Text = "The required FW wasn't found yet";
                                                                                                                    }
                                                                                                                    textBox2.Text = serial.InnerText;
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    XmlNode node24 = doc.SelectSingleNode("releases/release25/imgcrc");
                                                                                                                    if (textBox6.Text == node24.InnerText)
                                                                                                                    {
                                                                                                                        XmlNode id = doc.SelectSingleNode("releases/release25/titleid");
                                                                                                                        XmlNode serial = doc.SelectSingleNode("releases/release25/serial");
                                                                                                                        XmlNode r = doc.SelectSingleNode("releases/release25/region");
                                                                                                                        XmlNode fw = doc.SelectSingleNode("releases/release25/firmware");

                                                                                                                        textBox8.Text = id.InnerText;
                                                                                                                        textBox4.Text = r.InnerText;
                                                                                                                        if (textBox4.Text == "WLD")
                                                                                                                        {
                                                                                                                            textBox4.Text = "Worldwide";
                                                                                                                        }
                                                                                                                        textBox3.Text = fw.InnerText;
                                                                                                                        if (textBox3.Text == "")
                                                                                                                        {
                                                                                                                            textBox3.Text = "The required FW wasn't found yet";
                                                                                                                        }
                                                                                                                        textBox2.Text = serial.InnerText;
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        XmlNode node25 = doc.SelectSingleNode("releases/release26/imgcrc");
                                                                                                                        if (textBox6.Text == node25.InnerText)
                                                                                                                        {
                                                                                                                            XmlNode id = doc.SelectSingleNode("releases/release26/titleid");
                                                                                                                            XmlNode serial = doc.SelectSingleNode("releases/release26/serial");
                                                                                                                            XmlNode r = doc.SelectSingleNode("releases/release26/region");
                                                                                                                            XmlNode fw = doc.SelectSingleNode("releases/release26/firmware");

                                                                                                                            textBox8.Text = id.InnerText;
                                                                                                                            textBox4.Text = r.InnerText;
                                                                                                                            if (textBox4.Text == "WLD")
                                                                                                                            {
                                                                                                                                textBox4.Text = "Worldwide";
                                                                                                                            }
                                                                                                                            textBox3.Text = fw.InnerText;
                                                                                                                            if (textBox3.Text == "")
                                                                                                                            {
                                                                                                                                textBox3.Text = "The required FW wasn't found yet";
                                                                                                                            }
                                                                                                                            textBox2.Text = serial.InnerText;
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            XmlNode node26 = doc.SelectSingleNode("releases/release27/imgcrc");
                                                                                                                            if (textBox6.Text == node26.InnerText)
                                                                                                                            {
                                                                                                                                XmlNode id = doc.SelectSingleNode("releases/release27/titleid");
                                                                                                                                XmlNode serial = doc.SelectSingleNode("releases/release27/serial");
                                                                                                                                XmlNode r = doc.SelectSingleNode("releases/release27/region");
                                                                                                                                XmlNode fw = doc.SelectSingleNode("releases/release27/firmware");

                                                                                                                                textBox8.Text = id.InnerText;
                                                                                                                                textBox4.Text = r.InnerText;
                                                                                                                                if (textBox4.Text == "WLD")
                                                                                                                                {
                                                                                                                                    textBox4.Text = "Worldwide";
                                                                                                                                }
                                                                                                                                textBox3.Text = fw.InnerText;
                                                                                                                                if (textBox3.Text == "")
                                                                                                                                {
                                                                                                                                    textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                }
                                                                                                                                textBox2.Text = serial.InnerText;
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                XmlNode node27 = doc.SelectSingleNode("releases/release28/imgcrc");
                                                                                                                                if (textBox6.Text == node27.InnerText)
                                                                                                                                {
                                                                                                                                    XmlNode id = doc.SelectSingleNode("releases/release28/titleid");
                                                                                                                                    XmlNode serial = doc.SelectSingleNode("releases/release28/serial");
                                                                                                                                    XmlNode r = doc.SelectSingleNode("releases/release28/region");
                                                                                                                                    XmlNode fw = doc.SelectSingleNode("releases/release28/firmware");

                                                                                                                                    textBox8.Text = id.InnerText;
                                                                                                                                    textBox4.Text = r.InnerText;
                                                                                                                                    if (textBox4.Text == "WLD")
                                                                                                                                    {
                                                                                                                                        textBox4.Text = "Worldwide";
                                                                                                                                    }
                                                                                                                                    textBox3.Text = fw.InnerText;
                                                                                                                                    if (textBox3.Text == "")
                                                                                                                                    {
                                                                                                                                        textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                    }
                                                                                                                                    textBox2.Text = serial.InnerText;
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    XmlNode node28 = doc.SelectSingleNode("releases/release29/imgcrc");
                                                                                                                                    if (textBox6.Text == node28.InnerText)
                                                                                                                                    {
                                                                                                                                        XmlNode id = doc.SelectSingleNode("releases/release29/titleid");
                                                                                                                                        XmlNode serial = doc.SelectSingleNode("releases/release29/serial");
                                                                                                                                        XmlNode r = doc.SelectSingleNode("releases/release29/region");
                                                                                                                                        XmlNode fw = doc.SelectSingleNode("releases/release29/firmware");

                                                                                                                                        textBox8.Text = id.InnerText;
                                                                                                                                        textBox4.Text = r.InnerText;
                                                                                                                                        if (textBox4.Text == "WLD")
                                                                                                                                        {
                                                                                                                                            textBox4.Text = "Worldwide";
                                                                                                                                        }
                                                                                                                                        textBox3.Text = fw.InnerText;
                                                                                                                                        if (textBox3.Text == "")
                                                                                                                                        {
                                                                                                                                            textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                        }
                                                                                                                                        textBox2.Text = serial.InnerText;
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        XmlNode node29 = doc.SelectSingleNode("releases/release30/imgcrc");
                                                                                                                                        if (textBox6.Text == node29.InnerText)
                                                                                                                                        {
                                                                                                                                            XmlNode id = doc.SelectSingleNode("releases/release30/titleid");
                                                                                                                                            XmlNode serial = doc.SelectSingleNode("releases/release30/serial");
                                                                                                                                            XmlNode r = doc.SelectSingleNode("releases/release30/region");
                                                                                                                                            XmlNode fw = doc.SelectSingleNode("releases/release30/firmware");

                                                                                                                                            textBox8.Text = id.InnerText;
                                                                                                                                            textBox4.Text = r.InnerText;
                                                                                                                                            if (textBox4.Text == "WLD")
                                                                                                                                            {
                                                                                                                                                textBox4.Text = "Worldwide";
                                                                                                                                            }
                                                                                                                                            textBox3.Text = fw.InnerText;
                                                                                                                                            if (textBox3.Text == "")
                                                                                                                                            {
                                                                                                                                                textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                            }
                                                                                                                                            textBox2.Text = serial.InnerText;
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            XmlNode node30 = doc.SelectSingleNode("releases/release31/imgcrc");
                                                                                                                                            if (textBox6.Text == node30.InnerText)
                                                                                                                                            {
                                                                                                                                                XmlNode id = doc.SelectSingleNode("releases/release31/titleid");
                                                                                                                                                XmlNode serial = doc.SelectSingleNode("releases/release31/serial");
                                                                                                                                                XmlNode r = doc.SelectSingleNode("releases/release31/region");
                                                                                                                                                XmlNode fw = doc.SelectSingleNode("releases/release31/firmware");

                                                                                                                                                textBox8.Text = id.InnerText;
                                                                                                                                                textBox4.Text = r.InnerText;
                                                                                                                                                if (textBox4.Text == "WLD")
                                                                                                                                                {
                                                                                                                                                    textBox4.Text = "Worldwide";
                                                                                                                                                }
                                                                                                                                                textBox3.Text = fw.InnerText;
                                                                                                                                                if (textBox3.Text == "")
                                                                                                                                                {
                                                                                                                                                    textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                                }
                                                                                                                                                textBox2.Text = serial.InnerText;
                                                                                                                                            }
                                                                                                                                            else
                                                                                                                                            {
                                                                                                                                                XmlNode node31 = doc.SelectSingleNode("releases/release32/imgcrc");
                                                                                                                                                if (textBox6.Text == node31.InnerText)
                                                                                                                                                {
                                                                                                                                                    XmlNode id = doc.SelectSingleNode("releases/release32/titleid");
                                                                                                                                                    XmlNode serial = doc.SelectSingleNode("releases/release32/serial");
                                                                                                                                                    XmlNode r = doc.SelectSingleNode("releases/release32/region");
                                                                                                                                                    XmlNode fw = doc.SelectSingleNode("releases/release32/firmware");

                                                                                                                                                    textBox8.Text = id.InnerText;
                                                                                                                                                    textBox4.Text = r.InnerText;
                                                                                                                                                    if (textBox4.Text == "WLD")
                                                                                                                                                    {
                                                                                                                                                        textBox4.Text = "Worldwide";
                                                                                                                                                    }
                                                                                                                                                    textBox3.Text = fw.InnerText;
                                                                                                                                                    if (textBox3.Text == "")
                                                                                                                                                    {
                                                                                                                                                        textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                                    }
                                                                                                                                                    textBox2.Text = serial.InnerText;
                                                                                                                                                }
                                                                                                                                                else
                                                                                                                                                {
                                                                                                                                                    XmlNode node32 = doc.SelectSingleNode("releases/release33/imgcrc");
                                                                                                                                                    if (textBox6.Text == node32.InnerText)
                                                                                                                                                    {
                                                                                                                                                        XmlNode id = doc.SelectSingleNode("releases/release33/titleid");
                                                                                                                                                        XmlNode serial = doc.SelectSingleNode("releases/release33/serial");
                                                                                                                                                        XmlNode r = doc.SelectSingleNode("releases/release33/region");
                                                                                                                                                        XmlNode fw = doc.SelectSingleNode("releases/release33/firmware");

                                                                                                                                                        textBox8.Text = id.InnerText;
                                                                                                                                                        textBox4.Text = r.InnerText;
                                                                                                                                                        if (textBox4.Text == "WLD")
                                                                                                                                                        {
                                                                                                                                                            textBox4.Text = "Worldwide";
                                                                                                                                                        }
                                                                                                                                                        textBox3.Text = fw.InnerText;
                                                                                                                                                        if (textBox3.Text == "")
                                                                                                                                                        {
                                                                                                                                                            textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                                        }
                                                                                                                                                        textBox2.Text = serial.InnerText;
                                                                                                                                                    }
                                                                                                                                                    else
                                                                                                                                                    {
                                                                                                                                                        XmlNode node33 = doc.SelectSingleNode("releases/release34/imgcrc");
                                                                                                                                                        if (textBox6.Text == node33.InnerText)
                                                                                                                                                        {
                                                                                                                                                            XmlNode id = doc.SelectSingleNode("releases/release34/titleid");
                                                                                                                                                            XmlNode serial = doc.SelectSingleNode("releases/release34/serial");
                                                                                                                                                            XmlNode r = doc.SelectSingleNode("releases/release34/region");
                                                                                                                                                            XmlNode fw = doc.SelectSingleNode("releases/release34/firmware");

                                                                                                                                                            textBox8.Text = id.InnerText;
                                                                                                                                                            textBox4.Text = r.InnerText;
                                                                                                                                                            if (textBox4.Text == "WLD")
                                                                                                                                                            {
                                                                                                                                                                textBox4.Text = "Worldwide";
                                                                                                                                                            }
                                                                                                                                                            textBox3.Text = fw.InnerText;
                                                                                                                                                            if (textBox3.Text == "")
                                                                                                                                                            {
                                                                                                                                                                textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                                            }
                                                                                                                                                            textBox2.Text = serial.InnerText;
                                                                                                                                                        }
                                                                                                                                                        else
                                                                                                                                                        {
                                                                                                                                                            XmlNode node34 = doc.SelectSingleNode("releases/release35/imgcrc");
                                                                                                                                                            if (textBox6.Text == node34.InnerText)
                                                                                                                                                            {
                                                                                                                                                                XmlNode id = doc.SelectSingleNode("releases/release35/titleid");
                                                                                                                                                                XmlNode serial = doc.SelectSingleNode("releases/release35/serial");
                                                                                                                                                                XmlNode r = doc.SelectSingleNode("releases/release35/region");
                                                                                                                                                                XmlNode fw = doc.SelectSingleNode("releases/release35/firmware");

                                                                                                                                                                textBox8.Text = id.InnerText;
                                                                                                                                                                textBox4.Text = r.InnerText;
                                                                                                                                                                if (textBox4.Text == "WLD")
                                                                                                                                                                {
                                                                                                                                                                    textBox4.Text = "Worldwide";
                                                                                                                                                                }
                                                                                                                                                                textBox3.Text = fw.InnerText;
                                                                                                                                                                if (textBox3.Text == "")
                                                                                                                                                                {
                                                                                                                                                                    textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                                                }
                                                                                                                                                                textBox2.Text = serial.InnerText;
                                                                                                                                                            }
                                                                                                                                                            else
                                                                                                                                                            {
                                                                                                                                                                XmlNode node35 = doc.SelectSingleNode("releases/release36/imgcrc");
                                                                                                                                                                if (textBox6.Text == node35.InnerText)
                                                                                                                                                                {
                                                                                                                                                                    XmlNode id = doc.SelectSingleNode("releases/release36/titleid");
                                                                                                                                                                    XmlNode serial = doc.SelectSingleNode("releases/release36/serial");
                                                                                                                                                                    XmlNode r = doc.SelectSingleNode("releases/release36/region");
                                                                                                                                                                    XmlNode fw = doc.SelectSingleNode("releases/release36/firmware");

                                                                                                                                                                    textBox8.Text = id.InnerText;
                                                                                                                                                                    textBox4.Text = r.InnerText;
                                                                                                                                                                    if (textBox4.Text == "WLD")
                                                                                                                                                                    {
                                                                                                                                                                        textBox4.Text = "Worldwide";
                                                                                                                                                                    }
                                                                                                                                                                    textBox3.Text = fw.InnerText;
                                                                                                                                                                    if (textBox3.Text == "")
                                                                                                                                                                    {
                                                                                                                                                                        textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                                                    }
                                                                                                                                                                    textBox2.Text = serial.InnerText;
                                                                                                                                                                }else
                                                                                                                                                                {
                                                                                                                                                                    XmlNode node36 = doc.SelectSingleNode("releases/release37/imgcrc");
                                                                                                                                                                    if (textBox6.Text == node36.InnerText)
                                                                                                                                                                    {
                                                                                                                                                                        XmlNode id = doc.SelectSingleNode("releases/release37/titleid");
                                                                                                                                                                        XmlNode serial = doc.SelectSingleNode("releases/release37/serial");
                                                                                                                                                                        XmlNode r = doc.SelectSingleNode("releases/release37/region");
                                                                                                                                                                        XmlNode fw = doc.SelectSingleNode("releases/release37/firmware");

                                                                                                                                                                        textBox8.Text = id.InnerText;
                                                                                                                                                                        textBox4.Text = r.InnerText;
                                                                                                                                                                        if (textBox4.Text == "WLD")
                                                                                                                                                                        {
                                                                                                                                                                            textBox4.Text = "Worldwide";
                                                                                                                                                                        }
                                                                                                                                                                        textBox3.Text = fw.InnerText;
                                                                                                                                                                        if (textBox3.Text == "")
                                                                                                                                                                        {
                                                                                                                                                                            textBox3.Text = "The required FW wasn't found yet";
                                                                                                                                                                        }
                                                                                                                                                                        textBox2.Text = serial.InnerText;
                                                                                                                                                                    }else
                                                                                                                                                                    {
                                                                                                                                                                        textBox2.Text = "Game is not in the DB or you modified it";
                                                                                                                                                                        textBox3.Text = "Game is not in the DB or you modified it";
                                                                                                                                                                        textBox4.Text = "Game is not in the DB or you modified it";
                                                                                                                                                                        textBox8.Text = "Game is not in the DB or you modified it";
                                                                                                                                                                    }
                                                                                                                                                                }
                                                                                                                                                            }

                                                                                                                                                        }
                                                                                                                                                    }


                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            BinaryReader reader = new BinaryReader(new FileStream(textBox1.Text, FileMode.Open));
                reader.BaseStream.Position = 0x7000;
                var part = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Cert.cert");
                byte[] dataArray = reader.ReadBytes(512);
                reader.Close();
                using (var stream = new FileStream(part, FileMode.Create))
                {
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        stream.WriteByte(dataArray[i]);
                    }
                }
                string FF = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"FF");
            BinaryWriter writer = new BinaryWriter(new FileStream(textBox1.Text, FileMode.Open));
            writer.BaseStream.Position = 0x7000;
            for (int i = 0; i < 0x84; i++)
                writer.Write(0);
            writer.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "CERT Files (*.cert)|*.cert";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                BinaryReader reader = new BinaryReader(new FileStream(openFileDialog1.FileName, FileMode.Open));
                reader.BaseStream.Position = 0x0;
                byte[] dataArray = reader.ReadBytes(512);
                reader.Close();
                    BinaryWriter writer = new BinaryWriter(new FileStream(textBox1.Text, FileMode.Open));
                    writer.BaseStream.Position = 0x7000;
                    writer.Write(dataArray);
                    writer.Close();
                
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

         
            Crc32 crc32 = new Crc32();
            String hash = String.Empty;

            using (FileStream fs = File.Open(textBox1.Text, FileMode.Open))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();

            textBox6.Text = hash;
            BinaryReader reader = new BinaryReader(new FileStream(textBox1.Text, FileMode.Open));
            reader.BaseStream.Position = 0x140;
            textBox7.Text = BitConverter.ToString(reader.ReadBytes(32)).Replace("-", null);
            reader.BaseStream.Position = 0x160;
            textBox11.Text = BitConverter.ToString(reader.ReadBytes(32)).Replace("-", null);
            reader.Close();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://raw.githubusercontent.com/Hotbrawl20/db/master/NSWreleases.xml", "DB.xml");
                string NEWDB_location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DB.xml");
                string OLDDB_locarion = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DB.xml");
                File.Delete(NEWDB_location);
                File.Move(OLDDB_locarion, NEWDB_location);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            
                BinaryReader reader = new BinaryReader(new FileStream(textBox1.Text, FileMode.Open));
                reader.BaseStream.Position = 0x0;
                byte[] dataArray = reader.ReadBytes(512);
                reader.Close();
                BinaryWriter writer = new BinaryWriter(new FileStream(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Header.header"), FileMode.Create));
                writer.BaseStream.Position = 0x0;
                writer.Write(dataArray);
                writer.Close();
            }

        private void button5_Click_1(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://raw.githubusercontent.com/Hotbrawl20/db/master/NSWreleases.xml", "DB.xml");
                string NEWDB_location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DB.xml");
                string OLDDB_locarion = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DB.xml");
                File.Delete(NEWDB_location);
                File.Move(OLDDB_locarion, NEWDB_location);
            }
        }
    }
        }
    


