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
using System.Xml.Linq;

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

                    client.DownloadFile("http://nswdb.com/xml.php", "DB.xml");
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
                }
                else if (textBox10.Text == "4 GB")
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
                }
                else if (textBox10.Text == "8 GB")
                {
                    if (textBox5.Text == "8589934592")
                    {
                        textBox5.BackColor = Color.Green;
                        textBox5.Text = "8589934592 Bytes";
                    }
                    else
                    {
                        textBox5.BackColor = Color.DarkRed;
                    }
                }
                else if (textBox10.Text == "16 GB")
                {
                    if (textBox5.Text == "17179869184")
                    {
                        textBox5.BackColor = Color.Green;
                        textBox5.Text = "17179869184 Bytes";
                    }
                    else
                    {
                        textBox5.BackColor = Color.DarkRed;
                    }
                }

                Crc32 crc32 = new Crc32();
                String hash = String.Empty;

                using (FileStream fs = File.Open(textBox1.Text, FileMode.Open))
                    foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToUpper();

                textBox6.Text = hash;
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DB.xml"));
                var path = "releases/release[imgcrc = '" + textBox6.Text + "']";
                var node = doc.SelectSingleNode(path);
                if (node != null)
                {
                    var titleId = node["titleid"].InnerText;
                    var name = node["name"].InnerText;
                    var serial = node["serial"].InnerText;
                    var region = node["region"].InnerText;
                    var firmware = node["firmware"].InnerText;
                    textBox8.Text = titleId;
                    textBox2.Text = serial;
                    textBox3.Text = firmware;
                    if(textBox3.Text == "")
                    {
                        textBox3.Text = "The required Firmware isnt found yet";
                    }
                    textBox4.Text = region;
                    if(textBox4.Text == "WLD")
                    {
                        textBox4.Text = "Worldwide";
                    }
                    textBox9.Text = name;
                }
                else
                {
                    textBox2.Text = "Game is not in the DB or you modified it";
                    textBox3.Text = "Game is not in the DB or you modified it";
                    textBox4.Text = "Game is not in the DB or you modified it";
                    textBox8.Text = "Game is not in the DB or you modified it";
                    textBox9.Text = "Game is not in the DB or you modified it";
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
                try
                {
                    client.DownloadFile("http://nswdb.com/xml.php", "DB.xml");
                    string NEWDB_location =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DB.xml");
                    string OLDDB_locarion =
                        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"DB.xml");
                    File.Delete(NEWDB_location);
                    File.Move(OLDDB_locarion, NEWDB_location);
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
        }
    


