using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        Socket socket;
        NetworkStream stream;
        TcpListener listener;

        private void Form1_Load(object sender, EventArgs e)
        { //listener üret ve listener ı dinlemeye başla
            Int32 port = 1453;
            IPAddress localAddr = IPAddress.Parse("192.168.1.42");
            listener = new TcpListener(localAddr,port);//Tcp listener 1453 portunu sürekli olaran dinler 
            listener.Start();

            //bu listener dan bir tane socket üret
            socket = listener.AcceptSocket();//TcpListener dan bir adet soket üretilir ,geriye soket döndürür
            // ve bu socket den de bir tane stream üret
            stream = new NetworkStream(socket); // bu socket den de NetworkStream üretiriz

            //dinleme işni dişarıdan yapmamız gerek
            //bunu için Thread oluşturmamız gerek
            Thread dinle = new Thread(SoketDinle);
            //Thread e SoketDinle metodunu çalıştırmasını söylüyoruz
            dinle.Start();
        }
        BinaryFormatter bf = new BinaryFormatter();
        void SoketDinle()
        {
            while (socket.Connected)//socket bağlı olduğu sürece sürekli döner
            {
                //Socket conected sa git stream in içindeki veriyi al o veriyi Serileştir(Deserialize)
                Mesaj alinan = (Mesaj)bf.Deserialize(stream);
                listBox1.Items.Add(alinan);//aldığını listBox a yaz
                
                Mesaj msg = new Mesaj();
                msg.Gonderen = "Server";
                msg.Gonderim = DateTime.Now;
                msg.Mesaji = Convert.ToString(alinan);
                listBox1.Items.Add(msg);
                bf.Serialize(stream, msg);//stream networkStream dır , networkStream içerisine konan socket e verilir socket de gönderir
                stream.Flush();//göderim yapıldı
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Mesaj msg = new Mesaj();
            msg.Gonderen = "Server";
            msg.Gonderim = DateTime.Now;
            msg.Mesaji = textBox1.Text;
            listBox1.Items.Add(msg);
            bf.Serialize(stream,msg);//stream networkStream dır , networkStream içerisine konan socket e verilir socket de gönderir
            stream.Flush();//göderim yapıldı

            textBox1.Clear();
            textBox1.Focus();
        }

    }
}
