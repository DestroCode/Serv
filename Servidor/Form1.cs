using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Servidor
{
    public partial class Form1 : Form
    {
        
        delegate void SetTextCallBack(string text);
        TcpListener listener;
        TcpClient cliente;
        NetworkStream ns;
        Thread tarefa = null;

        public void execucao()
        {
            byte[] bytes = new byte[1024];
            while (true)
            {
                int bytesLidos = ns.Read(bytes, 0, bytes.Length);
                this.SetText(Encoding.ASCII.GetString(bytes, 0, bytesLidos));

            }
        }
        private void SetText(string texto)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(SetText);
                this.Invoke(d, new object[] { texto });
            }
            else
            {
                this.textBox1.Text = this.textBox1.Text + "\n";
                this.textBox1.AppendText("Cliente:> ");
                this.textBox1.AppendText(texto);
                this.textBox1.AppendText(Environment.NewLine);
            }
        }

        public Form1()
        {
            //listener.Stop();
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String dado = textBox2.Text;
            byte[] mensagem = Encoding.ASCII.GetBytes(dado);
            ns.Write(mensagem, 0, mensagem.Length);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress enderecoIP = IPAddress.Parse("192.168.0.2");
            label1.Text = "Aguardando Conexão...";
            label1.ForeColor = System.Drawing.Color.Yellow;
            button1.Enabled = false;
            listener = new TcpListener(enderecoIP, 4540);
            listener.Start();
            cliente = listener.AcceptTcpClient();
            ns = cliente.GetStream();
            label1.Text = "Cliente conectado!";
            label1.ForeColor = System.Drawing.Color.Green;
            button2.Enabled = true;
            tarefa = new Thread(execucao);
            tarefa.Start();
            
        }
    }
}
