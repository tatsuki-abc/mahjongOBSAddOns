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

namespace mahjongOBSAddOns
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnCalculateClick(object sender, RoutedEventArgs e)
        {
            if (hanComboBox.SelectedItem is ComboBoxItem hanItem && fuComboBox.SelectedItem is ComboBoxItem fuItem && honbaComboBox.SelectedItem is ComboBoxItem honbaItem && kyoutakuComboBox.SelectedItem is ComboBoxItem kyoutakuItem)
            {
                int han = int.Parse(hanItem.Content.ToString());
                int fu = int.Parse(fuItem.Content.ToString());
                int honba = int.Parse(honbaItem.Content.ToString());
                int kyoutaku = int.Parse(kyoutakuItem.Content.ToString());
                bool isParent = ParentRadio.IsChecked == true;
                bool isTsumo = TsumoRadio.IsChecked == true;

                string? result = ScoreCalculator.Calculate(han, fu, isParent, isTsumo, honba, kyoutaku);
                ResultText.Text = result ?? "無効な入力です。";
            }
            else
            {
                ResultText.Text = "翻数と符数を選択してください。";
            }
        }
    }
}
