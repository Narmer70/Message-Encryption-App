using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        Socket sck;
        EndPoint epLocal, epRemote;
        List<string> array = new List<string>();
        string key;
        string text;
        string alfa = "ABCDEFGHIGKLMNOPQRSTUVWXYZ";
        int temp;
        private int StrNum(char t)
        {
            int count = 0;
            foreach (string z in array)
            {
                if (z.IndexOf(t) != -1)
                {
                    temp = count;
                    return count;
                }
                count++;
            }
            return 1;
        }

        private int StbNum(char p)
        {
            int x;
            foreach (string z in array)
            {
                x = z.IndexOf(p);
                if (x != -1)
                {
                    temp = x;
                    return x;
                }
            }
            return 0;
        }
        public Form1()
        {
            InitializeComponent();

            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            txtIPClient1.Text = GetLocalIP();
            txtIPClient2.Text = GetLocalIP();
        }
        private string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        private void MessageCallBack(IAsyncResult aResult)
        {
          try
              {
                int size = sck.EndReceiveFrom(aResult, ref epRemote);
                if (size > 0)
                {
                    byte[] receiveData = new byte[1464];
                    receiveData = (byte[])aResult.AsyncState;

                    ASCIIEncoding eEncoding = new ASCIIEncoding();
                    string receivedMessage = eEncoding.GetString(receiveData);

                    txtDecrypt.Text += receivedMessage;
                }

                byte[] buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, 
                    new AsyncCallback(MessageCallBack), buffer);
          }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                epLocal = new IPEndPoint(IPAddress.Parse(txtIPClient1.Text),
                    Convert.ToInt32(txtPORTClient1.Text));
                sck.Bind(epLocal);
                epRemote = new IPEndPoint(IPAddress.Parse(txtIPClient2.Text),
                    Convert.ToInt32(txtPORTClient2.Text));
                sck.Connect(epRemote);

                byte[] buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote,
                   new AsyncCallback(MessageCallBack), buffer);

                btnStart.Text = "Connected";
                btnStart.Enabled = false;
                btnSend.Enabled = true;
                txtEncrypt.Focus();

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                byte[] msg = new byte[1500];
                msg = enc.GetBytes(txtEncryptSend.Text);
                sck.Send(msg);
                txtEncryptSend.Clear();
                
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string name = txtEncrypt.Text;
            string temp = name;
            name = Trisemus.Encrypt(temp);
            txtEncryptSend.Text = name;
            listMessages.Items.Add("You:" + txtEncrypt.Text);
            txtEncrypt.Clear();
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            string name = txtDecrypt.Text;
            string temp = name;
            name = Trisemusde.Decrypt(temp);
            txtDecryptRead.Text = name;
            txtDecrypt.Clear();
            
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
           listMessages.Items.Add("Freind:" + txtDecryptRead.Text);
            txtDecryptRead.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}