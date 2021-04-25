using System.Windows;

namespace SsorinHeroes
{
    /// <summary>
    /// Логика взаимодействия для OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        int eMobsCount = 3;
        int eMobsLevel = 1;
        int fMobsCount = 3;
        int fMobsLevel = 1;
        int difficulty = 2;

        public OptionsWindow()
        {
            InitializeComponent();

            EnemyMobsLevel.Content = eMobsLevel;
            EnemyMobsCount.Content = eMobsCount;
            FriendlyMobsCount.Content = fMobsCount;
            FriendlyMobsLevel.Content = fMobsLevel;
        }

        private void btEasy(object sender, RoutedEventArgs e)
        {
            difficultyLb.Content = "Легко";
            difficulty = 1;
        }

        private void btMedium(object sender, RoutedEventArgs e)
        {
            difficultyLb.Content = "Средне";
            difficulty = 2;
        }

        private void btHard(object sender, RoutedEventArgs e)
        {
            difficultyLb.Content = "Сложно";
            difficulty = 3;
        }

        private void clickNext(object sender, RoutedEventArgs e)
        {
            GameWindow game = new GameWindow(eMobsCount, eMobsLevel, fMobsCount, fMobsLevel, difficulty);
            game.Show();
            this.Close();
        }

        private void eMobsCountUp(object sender, RoutedEventArgs e)
        {
            if (eMobsCount < 5)
            {
                EnemyMobsCount.Content = ++eMobsCount;
            }
        }

        private void eMobsLevelUp(object sender, RoutedEventArgs e)
        {
            if (eMobsLevel < 3)
            {
                EnemyMobsLevel.Content = ++eMobsLevel;
            }
        }

        private void fMobsCountUp(object sender, RoutedEventArgs e)
        {
            if (fMobsCount < 5)
                FriendlyMobsCount.Content = ++fMobsCount;
        }

        private void fMobsLevelUp(object sender, RoutedEventArgs e)
        {
            if (fMobsLevel < 3)
                FriendlyMobsLevel.Content = ++fMobsLevel;
        }

        private void fMobsLevelDown(object sender, RoutedEventArgs e)
        {
            if (fMobsLevel > 1)
                FriendlyMobsLevel.Content = --fMobsLevel;
        }

        private void fMobsCountDown(object sender, RoutedEventArgs e)
        {
            if (fMobsCount > 1)
                FriendlyMobsCount.Content = --fMobsCount;
        }

        private void eMobsLevelDown(object sender, RoutedEventArgs e)
        {
            if (eMobsLevel > 1)
            {
                EnemyMobsLevel.Content = --eMobsLevel;
            }
        }

        private void eMobsCountDown(object sender, RoutedEventArgs e)
        {
            if (eMobsCount > 1)
            {
                EnemyMobsCount.Content = --eMobsCount;
            }
        }
    }
}
