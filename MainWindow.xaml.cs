using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace mahjongOBSAddOns
{
    public partial class MainWindow : Window
    {
        private int[] playerScores = Array.Empty<int>();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int initialPlayerCount = 4;
            InitializePlayers(initialPlayerCount);

            PlayerCountComboBox.SelectionChanged += (s, ev) =>
            {
                int playerCount = PlayerCountComboBox.SelectedIndex == 0 ? 4 : 3;
                InitializePlayers(playerCount);
            };
        }

        private void InitializePlayers(int count)
        {
            playerScores = new int[count];
            for (int i = 0; i < count; i++) playerScores[i] = 25000;

            UpdatePlayerSelections(count);
            UpdateScoreDisplay();
        }

        private void UpdatePlayerSelections(int count)
        {
            WinnerComboBox.Items.Clear();
            LoserComboBox.Items.Clear();

            for (int i = 0; i < count; i++)
            {
                string label = $"プレイヤー{i + 1}";
                WinnerComboBox.Items.Add(label);
                LoserComboBox.Items.Add(label);
            }

            WinnerComboBox.SelectedIndex = 0;
            if (count > 1) LoserComboBox.SelectedIndex = 1;
        }

        private void UpdateScoreDisplay()
        {
            Player1ScoreText.Text = $"{playerScores[0]}点";
            Player2ScoreText.Text = $"{playerScores[1]}点";
            Player3ScoreText.Text = $"{playerScores[2]}点";

            if (playerScores.Length == 4)
            {
                Player4Label.Visibility = Visibility.Visible;
                Player4ScoreText.Visibility = Visibility.Visible;
                Player4ScoreText.Text = $"{playerScores[3]}点";
            }
            else
            {
                Player4Label.Visibility = Visibility.Collapsed;
                Player4ScoreText.Visibility = Visibility.Collapsed;
            }
        }

        private void ApplyScoreChange(bool isTsumo, int winnerIndex, int? loserIndex, int[] pointChanges)
        {
            if (isTsumo)
            {
                for (int i = 0; i < playerScores.Length; i++)
                {
                    if (i == winnerIndex) continue;

                    playerScores[i] -= pointChanges[i];
                    playerScores[winnerIndex] += pointChanges[i];
                }
            }
            else if (loserIndex.HasValue)
            {
                int loser = loserIndex.Value;
                playerScores[loser] -= pointChanges[loser];
                playerScores[winnerIndex] += pointChanges[loser];
            }

            UpdateScoreDisplay();
        }

        private void ApplyScoreButton_Click(object sender, RoutedEventArgs e)
        {
            int winnerIndex = WinnerComboBox.SelectedIndex;
            int loserIndex = LoserComboBox.SelectedIndex;
            bool isTsumo = TsumoRadio.IsChecked == true;
            bool isDealer = ParentRadio.IsChecked == true;

            if (hanComboBox.SelectedItem is not ComboBoxItem hanItem || fuComboBox.SelectedItem is not ComboBoxItem fuItem)
            {
                MessageBox.Show("翻数と符数を選択してください。");
                return;
            }

            int han = int.Parse(hanItem.Content.ToString());
            int fu = int.Parse(fuItem.Content.ToString());

            int playerCount = playerScores.Length;
            int[] pointChanges = new int[playerCount];

            var result = MahjongScoreTable.GetScore(han, fu, isDealer, isTsumo);

            if (result == null)
            {
                MessageBox.Show("無効な翻・符の組み合わせです。");
                return;
            }

            int selfScore = result.WinScore;
            int[] paymentScores = result.PaymentScores;

            if (isTsumo)
            {
                for (int i = 0; i < playerCount; i++)
                {
                    if (i != winnerIndex)
                        pointChanges[i] = paymentScores[i];
                }
            }
            else
            {
                pointChanges[loserIndex] = paymentScores[loserIndex];
            }

            ApplyScoreChange(isTsumo, winnerIndex, isTsumo ? null : loserIndex, pointChanges);
        }
    }
}
