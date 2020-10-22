using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace KMiSOIB_lab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Task1()
        {
            var text = MessageBox1.Text.ToCharArray();
            var key = KeyBox1.Text.ToCharArray();
            var desKey = Encoding.ASCII.GetBytes(key); //?? "12345678");
            var input = Encoding.ASCII.GetBytes(text); //?? "HELLO");
            var output = DES_Encrypt(input, desKey);
            var encMsg = BitConverter.ToString(output);
            EncBox1.Text = string.Join("", encMsg);
            var reversed = DES_Decrypt(desKey, output);
            var decMsg = Encoding.ASCII.GetString(reversed);
            DecBox1.Text = string.Join("", decMsg);


        }

        private static byte[] DES_Encrypt(byte[] input, byte[] desKey)
        {
            var engine = new DesEngine();
            var cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(engine));
            cipher.Init(true, new KeyParameter(desKey));
            var text = new byte[cipher.GetOutputSize(input.Length)];
            var outputLen = cipher.ProcessBytes(input, 0, input.Length, text, 0);
            cipher.DoFinal(text, outputLen);
            return text;
        }

        private static byte[] DES_Decrypt(byte[] desKey, byte[] text)
        {
            var engine = new DesEngine();
            BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(engine));
            cipher.Init(false, new KeyParameter(desKey));
            var decMsg = new byte[cipher.GetOutputSize(text.Length)];
            var outputLen = cipher.ProcessBytes(text, 0, text.Length, decMsg, 0);
            cipher.DoFinal(decMsg, outputLen);
            return decMsg;
        }

        private void Proceed1_Button_Click(object sender, RoutedEventArgs e) { Task1(); }
        private void Exit_Click(object sender, RoutedEventArgs e) { this.Close(); }
    }
}
