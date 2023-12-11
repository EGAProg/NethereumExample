using System.Windows;
using System.Windows.Controls;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;

namespace EtheriumWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    /*
    Го́споди Бо́же на́ш, Тво́рче не́ба и земли́, созда́вый челове́ка и дарова́вый ему́ ве́дение сокрове́нной прему́дрости, 
    во е́же прославля́ти и́мя Твое́, горе́ и до́лу Ты́ еси́, Твое́ е́сть ночь и Твое́ е́сть де́нь, не утая́тся пред Тобо́ю да́же мимолетные движе́ния мы́сли.
    И ны́не молю́ Тя́, от вся́каго греха́ сохрани́ мя́, приступа́ющаго к рабо́те с хитроумным творе́нием ру́к челове́ческих, проникающим во вся́ концы́ земли́.
    Огради́ о́чи мои́ и у́м мо́й от вся́ких нечи́стых и блу́дных образо́в, от пусты́х и негодных слове́с.
    Укрепи́ во́лю, се́рдце очи́сти, не да́ждь ми́ вотще́6 расточа́ти вре́мя жи́зни моея́, и от вся́каго расслабления и уны́ния изба́ви.
    Да бу́дут дела́ на́ши во сла́ву Твою́!
    Ами́нь.
     */

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        static readonly Web3 web3 = new("http://localhost:7545");
        //static readonly string contractAddr = "0xDA6156Aa691e564a93E52d11b631E38Fb836ace2";
        static readonly string contractAddr = "0x96028138Dac70F2219A4c14cc8112e842aFA820b";
        static readonly Contract contract = web3.Eth.GetContract(Connection.GetAbi(), contractAddr);
        public string userAddr = "";
        private string privateKey = "";

        public static void GetAccounts(string ACC, Label lb)
        {
            GetAccBalance(ACC, lb);
        }

        public static async void GetAccBalance(string publicKey, Label lb)
        {
            try
            {
                var balance = await web3.Eth.GetBalance.SendRequestAsync(publicKey);
                lb.Content = $"Addres: {publicKey} | Balance ETH: {Web3.Convert.FromWei(balance)}";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            accountAddrTextBox.Visibility = Visibility.Collapsed;
            LogInBtn.Visibility = Visibility.Collapsed;
            var account = new Nethereum.Web3.Accounts.Account(accountAddrTextBox.Text);
            privateKey = accountAddrTextBox.Text;
            userAddr = account.Address;
            try
            {
                Label lb = new()
                {
                    Name = $"lbUser",
                    Content = $"",
                    Height = 60,
                    FontSize = 18,
                    Margin = new(0, 0, 0, 0)
                };
                LoginGrid.Children.Add(lb);
                GetAccounts(userAddr, lb);

            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception");
            }
        }

        private async void SendTransactBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await contract.GetFunction("Deposit").SendTransactionAsync(
                    userAddr, 
                    new HexBigInteger(5000000), 
                    new HexBigInteger(Web3.Convert.ToWei(Convert.ToInt32(AmountTextBox.Text))), 
                    KeyKodeTextBox.Text, 
                    ResieverAddrTextBox.Text);
                lbSuc.Content = "Success!";

                Label lb = new()
                {
                    Name = $"lbHis",
                    Content = $"{userAddr} -> {ResieverAddrTextBox.Text} ({AmountTextBox.Text})",
                    Margin = new(10,10,0,0),
                    Width = 710

                };

                HistoryGrid.Children.Add(lb);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception");
            }
        }

        private async void Resieve_Button_Click(object sender, RoutedEventArgs e)
        {
            await contract.GetFunction("getTransact").SendTransactionAsync(
                    userAddr,
                    new HexBigInteger(5000000),
                    null,
                    ResieveTextBox.Text);
            lbSuc1.Content = "Success!";
            Label lb = new()
             {
                 Name = $"lbHis",
                 Content = $"{userAddr} -> {ResieverAddrTextBox.Text} ({AmountTextBox.Text})",
                 Margin = new(10, 10, 0, 0),
                 Width = 710

             };

            HistoryGrid.Children.Add(lb);
        }

        private async void Cansel_Button_Click(object sender, RoutedEventArgs e)
        {
            await contract.GetFunction("CancelTransaction").SendTransactionAsync(
                    userAddr,
                    new HexBigInteger(5000000),
                    null,
                    CanselTextBox.Text);
            Label lb = new()
            {
                Name = $"lbHis",
                Content = $"{userAddr} -> {ResieverAddrTextBox.Text} ({AmountTextBox.Text})",
                Margin = new(10, 10, 0, 0),
                Width = 710

            };

            HistoryGrid.Children.Add(lb);
            lbSuc2.Content = "Success!";
        }
    }
}