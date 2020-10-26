using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        TcpClient client;//kullanılacak bağlantı nesnesi

        NetworkStream stream;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            client = new TcpClient(textBox1.Text,1453);
            stream = client.GetStream();// client dan Stream üretip bu Stream ı stream a atıyoruz
            Thread dinleyici = new Thread(BaglantiDinle);
            dinleyici.Start();
        }

        BinaryFormatter bf = new BinaryFormatter();
        void BaglantiDinle() {
            while (true) {
                Server.Mesaj mesaj = (Server.Mesaj)bf.Deserialize(stream);
                listBox1.Items.Add(mesaj);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Server.Mesaj msg = new Server.Mesaj();
            msg.Gonderen = textBox2.Text;
            msg.Gonderim = DateTime.Now;
            msg.Mesaji = textBox3.Text;
            listBox1.Items.Add(msg);

            bf.Serialize(stream,msg);
            stream.Flush();
            textBox3.Clear();
            textBox3.Focus();
        }
    }
}
