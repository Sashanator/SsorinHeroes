using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SsorinHeroes
{
    struct GridPosition //Координаты
    {
        public int row; //СТРОКА
        public int col; //СТОЛБЕЦ


        public override bool Equals(Object obj)
        {
            return obj is GridPosition && this == (GridPosition)obj;
        }
        public override int GetHashCode()
        {
            return row.GetHashCode() ^ col.GetHashCode();
        }
        public static bool operator ==(GridPosition x, GridPosition y)
        {
            return x.row == y.row && x.col == y.col;
        }
        public static bool operator !=(GridPosition x, GridPosition y)
        {
            return !(x == y);
        }
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        bool isYourTurn; //При true - ход игрока, при false - ход ИИ

        int TurnN = 1000; //Константа, определяющая частоту ходов дружеского мага 

        string ButtonMovingName; //Имя клетки, с которой УШЕЛ моб
        bool isActive; //Активирован ли моб
        int NumberTurn;
        List<Mob> friendlyMobs = new List<Mob>(); //Список союзных мобов
        List<Mob> enemyMobs = new List<Mob>(); //Список вражеских мобов
        List<Mob> friendlyMobsMadeTurn = new List<Mob>(); //Список дружеских мобов которые походили
        List<Mob> enemyMobsMadeTurn = new List<Mob>(); //Список вражеских мобов которые походили
        List<GridPosition> friednlyRngPositions = new List<GridPosition>(); //список позиций для дружеских мобов
        List<GridPosition> enemyRngPositions = new List<GridPosition>(); //список позиций для вражеских мобов

        Hero friendlyWizard;

        GridPosition gr_wizard = new GridPosition();
        
        public GameWindow(int enemyMobsCount, int enemyMobsLevel, int friendlyMobsCount, int friendlyMobsLevel, int gameDifficulty)
        {
            InitializeComponent();
            //В зависимости от уровня сложности определяется как часто чародейка будет помогать игроку
            switch (gameDifficulty)
            {
                case 1:
                    TurnN = 2;
                    break;
                case 2:
                    TurnN = 5;
                    break;
                case 3:
                    TurnN = 10;
                    break;
            }
            
            NumberTurn = 0;

            isYourTurn = true;

            Random rng = new Random();
            
            //Сборка дружеского мага
            gr_wizard.row = rng.Next(0, 10);
            gr_wizard.col = rng.Next(0, 2);
            
            string name_hero = "Чародейка";
            int hp_hero = rng.Next(6, 12);
            int attack_hero = rng.Next(0, 5);
            int defense_hero = rng.Next(3, 6);
            int luck_hero = rng.Next(30, 100);
            int move_hero = rng.Next(2, 4);
            int accuracy_hero = rng.Next(15, 100);
            int leader_hero = rng.Next(0, 1000);
            friendlyWizard = new Hero(name_hero, hp_hero, attack_hero, defense_hero, luck_hero, move_hero, accuracy_hero, leader_hero);
            friendlyWizard.setAP(move_hero);
            friendlyWizard.SetPosition(gr_wizard);
            friendlyWizard.setFriend();

            int spellDamage = rng.Next(1, 4);
            int spellDistance = rng.Next(2, 6);
            int spellMana = rng.Next(0, 3);
            int spellMagicError = rng.Next(0, 100);
            string spellName = "Dragon hit";
            friendlyWizard.SetSpell(spellDamage, spellDistance, spellMana, spellMagicError, spellName);

            //
            lb_leader_wizard.Content = "Лидерство: " + friendlyWizard.getLeader().ToString();
            lb_ap_wizard.Content = "Мана: " + friendlyWizard.ap.ToString();
            lb_hp_wizard.Content = "Здоровье: " + friendlyWizard.getHP().ToString();
            lb_attack_wizard.Content = "Атака: " + friendlyWizard.getAttack().ToString();
            lb_defense_wizard.Content = "Защита: " + friendlyWizard.getDefense().ToString();
            lb_luck_wizard.Content = "Удача: " + friendlyWizard.getLuck().ToString();
            lb_accuracy_wizard.Content = "Точность: " + friendlyWizard.getAccuracy().ToString();
            lb_move_wizard.Content = "Перемещение: " + friendlyWizard.getMove().ToString();
            lb_name_wizard.Content = friendlyWizard.name;

            damageSpell.Content = $"Урон: {spellDamage.ToString()}";
            distanceSpell.Content = $"Дистанция: {spellDistance.ToString()}";
            manaSpell.Content = $"Мана: {spellMana.ToString()}";
            errorSpell.Content = $"Ошибка: {spellMagicError.ToString()}";

            //Конец сборки дружеского мага

            for (int i = 0; i < friendlyMobsCount; i++)
            {//Автоматизированная сборка дружеских мобов!
                GridPosition gr;
                gr.row = rng.Next(0, 10);
                gr.col = rng.Next(0, 2);
                while (friednlyRngPositions.Contains(gr)) 
                {
                    gr.row = rng.Next(0, 10);
                    gr.col = rng.Next(0, 2);
                }
                while (gr == gr_wizard)//Убираем мага
                {
                    gr.row = rng.Next(0, 10);
                    gr.col = rng.Next(0, 2);
                }
                friednlyRngPositions.Add(gr);

                if (friendlyMobsLevel == 1)
                {
                    string name = "EZ";
                    int hp = rng.Next(1, 5);
                    int attack = rng.Next(1, 3);
                    int defense = rng.Next(0, 3);
                    int luck = rng.Next(0, 100);
                    int move = rng.Next(1, 4);
                    int accuracy = rng.Next(0, 100);
                    int leader = rng.Next(0, 1000);
                    Mob newMob = new Mob(name, hp, attack, defense, luck, move, accuracy, leader);
                    newMob.setAP(move);
                    newMob.SetPosition(gr);
                    newMob.setFriend();
                    friendlyMobs.Add(newMob);
                    Console.WriteLine(newMob.gr.row + " " + newMob.gr.col);
                }
                else if (friendlyMobsLevel == 2)
                {
                    string name = "MID";
                    int hp = rng.Next(3, 7);
                    int attack = rng.Next(2, 4);
                    int defense = rng.Next(1, 5);
                    int luck = rng.Next(25, 100);
                    int move = rng.Next(2, 6);
                    int accuracy = rng.Next(25, 100);
                    int leader = rng.Next(0, 1000);
                    Mob newMob = new Mob(name, hp, attack, defense, luck, move, accuracy, leader);
                    newMob.setAP(move);
                    newMob.SetPosition(gr);
                    newMob.setFriend();
                    friendlyMobs.Add(newMob);
                    Console.WriteLine(newMob.gr.row + " " + newMob.gr.col);
                }
                else
                {
                    string name = "HARD";
                    int hp = rng.Next(5, 10);
                    int attack = rng.Next(4, 7);
                    int defense = rng.Next(3, 7);
                    int luck = rng.Next(50, 100);
                    int move = rng.Next(4, 9);
                    int accuracy = rng.Next(50, 100);
                    int leader = rng.Next(0, 1000);
                    Mob newMob = new Mob(name, hp, attack, defense, luck, move, accuracy, leader);
                    newMob.setAP(move);
                    newMob.SetPosition(gr);
                    newMob.setFriend();
                    friendlyMobs.Add(newMob);
                    Console.WriteLine(newMob.gr.row + " " + newMob.gr.col);
                }
            }

            for (int i = 0; i < enemyMobsCount; i++)
            {//Автоматизированная сборка вражеских мобов!
                GridPosition gr;
                gr.row = rng.Next(0, 10);
                gr.col = rng.Next(8, 10);
                while (enemyRngPositions.Contains(gr))
                {
                    gr.row = rng.Next(0, 10);
                    gr.col = rng.Next(8, 10);
                }
                enemyRngPositions.Add(gr);

                if (enemyMobsLevel == 1)
                {
                    string name = "EZ";
                    int hp = rng.Next(1, 4);
                    int attack = rng.Next(1, 4);
                    int defense = rng.Next(0, 2);
                    int luck = rng.Next(0, 50);
                    int move = rng.Next(1, 4);
                    int accuracy = rng.Next(0, 100);
                    int leader = rng.Next(0, 1000);
                    Mob newMob = new Mob(name, hp, attack, defense, luck, move, accuracy, leader);
                    newMob.SetPosition(gr);
                    newMob.setEnemy();
                    enemyMobs.Add(newMob);
                    Console.WriteLine(newMob.gr.row + " " + newMob.gr.col);
                }
                else if (enemyMobsLevel == 2)
                {
                    string name = "MID";
                    int hp = rng.Next(2, 4);
                    int attack = rng.Next(2, 4);
                    int defense = rng.Next(1, 3);
                    int luck = rng.Next(25, 100);
                    int move = rng.Next(2, 4);
                    int accuracy = rng.Next(25, 100);
                    int leader = rng.Next(0, 1000);
                    Mob newMob = new Mob(name, hp, attack, defense, luck, move, accuracy, leader);
                    newMob.SetPosition(gr);
                    newMob.setEnemy();
                    enemyMobs.Add(newMob);
                    Console.WriteLine(newMob.gr.row + " " + newMob.gr.col);
                }
                else
                {
                    string name = "HARD";
                    int hp = rng.Next(3, 6);
                    int attack = rng.Next(3, 6);
                    int defense = rng.Next(3, 6);
                    int luck = rng.Next(50, 100);
                    int move = rng.Next(3, 6);
                    int accuracy = rng.Next(50, 100);
                    int leader = rng.Next(0, 1000);
                    Mob newMob = new Mob(name, hp, attack, defense, luck, move, accuracy, leader);
                    newMob.SetPosition(gr);
                    newMob.setEnemy();
                    enemyMobs.Add(newMob);
                    Console.WriteLine(newMob.gr.row + " " + newMob.gr.col);
                }
            }

            isActive = false;

            ButtonMovingName = "";

            for (int i = 0; i < 10; i++)
            {//Автоматизированная сборка кнопочек!
                for (int j = 0; j < 10; j++)
                {
                    Button bt = new Button();
                    bt.Name = "r" + i.ToString() + "_c" + j.ToString();
                    bt.Background = new SolidColorBrush(Colors.Transparent);
                    bt.BorderBrush = Brushes.LimeGreen;
                    bt.BorderThickness = new Thickness(0);
                    bt.Click += (s, e) => InteractCharacter(s, e);
                    bt.Width = 51;
                    bt.Height = 51;
                    MainGrid.Children.Add(bt);
                    Grid.SetRow(bt, i);
                    Grid.SetColumn(bt, j);
                }
            }

            //Обозначаем мага
            var brushHero = new ImageBrush();
            brushHero.ImageSource = new BitmapImage(new Uri("img/Wizard.png", UriKind.Relative));
            string btName_hero = $"r{friendlyWizard.gr.row}_c{friendlyWizard.gr.col}";
            Button btWanted_hero = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, btName_hero); //Берём кнопку по названию
            btWanted_hero.Background = brushHero;
            btWanted_hero.IsEnabled = false;
            //Конец обозначения мага
            
            for (int i = 0; i < friendlyMobs.Count; i++)
            {//Находим и обозначаем дружеских мобов
                var brushFriendly = new ImageBrush();
                brushFriendly.ImageSource = new BitmapImage(new Uri($"heroes/Hero{rng.Next(1, 18)}.png", UriKind.Relative));
                
                string btName = $"r{friendlyMobs[i].gr.row}_c{friendlyMobs[i].gr.col}";
                Button btWanted = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, btName); //Берём кнопку по названию
                btWanted.Background = brushFriendly;
                
            }

            var brushEnemy= new ImageBrush();
            brushEnemy.ImageSource = new BitmapImage(new Uri("img/dragon.png", UriKind.Relative));

            for (int i = 0; i < enemyMobs.Count; i++)
            {//Находим и обозначаем вражеских мобов
                string btName = $"r{enemyMobs[i].gr.row}_c{enemyMobs[i].gr.col}";
                Button btWanted = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, btName); //Берём кнопку по названию
                btWanted.BorderBrush = Brushes.Red;
                btWanted.Background = brushEnemy;
                btWanted.GotFocus += (s, e) =>
                {
                    btWanted.BorderThickness = new Thickness(1);
                };
                btWanted.LostFocus += (s, e) =>
                {
                    btWanted.BorderThickness = new Thickness(0);
                };
            }

            //Определение кто будет ходить первым
            //PerformClick(EndTurn);
        }

        private string TrueSubString(string sub, int start, int finish)
        {
            string substr = "";
            for (int i = start; i < finish; i++)
            {
                substr += sub[start++];
            }
            return substr;
        }

        /// <summary>
        /// Главный метод
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InteractCharacter(object sender, RoutedEventArgs e)
        {
            Button bt = (sender as Button); //Кнопка на которую сейчас нажали

            int rowWantedBt = Grid.GetRow(bt); //Строка в гриде на которой находится кнопка
            int colWantedBt = Grid.GetColumn(bt); //Столбец в гриде на которой находится кнопка
            
            if ((bt.Background as ImageBrush) != null) //Нажали на героя
            {
                int x = -1; //Переменная, по которой индентифицируется моб
                for (int i = 0; i < friendlyMobs.Count; i++)
                {//Находим моба с координатами клетки, на которую нажали
                    if (friendlyMobs[i].gr.row == rowWantedBt && friendlyMobs[i].gr.col == colWantedBt)
                    {
                        x = i;
                        break;
                    }
                }
                if (x != -1) //Значит моб дружеский
                {
                    Mob thisMob = friendlyMobs[x]; //Определяем, что за моба мы активировали

                    //Выводим статы на экран
                    lb_leader.Content = "Лидерство: " + thisMob.getLeader().ToString();
                    lb_ap.Content = "Действия: " + thisMob.ap.ToString();
                    lb_hp.Content = "Здоровье: " + thisMob.getHP().ToString();
                    lb_attack.Content = "Атака: " + thisMob.getAttack().ToString();
                    lb_defense.Content = "Защита: " + thisMob.getDefense().ToString();
                    lb_luck.Content = "Удача: " + thisMob.getLuck().ToString();
                    lb_accuracy.Content = "Точность: " + thisMob.getAccuracy().ToString();
                    lb_move.Content = "Перемещение: " + thisMob.getMove().ToString();

                    bool isThisHero = false; //Нажали ли на уже выбранного героя

                    if (isActive == true) //Если уже выбран герой
                    {
                        Button btWanted = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, ButtonMovingName); //Ищем кнопку по имени
                        if (Grid.GetRow(bt) == Grid.GetRow(btWanted) && Grid.GetColumn(bt) == Grid.GetColumn(btWanted))
                        {//Если клетки совпадают то
                            isThisHero = true; //Ставим тру
                        }
                        else
                        {
                            btWanted.BorderThickness = new Thickness(0); //Убираем границы
                            foreach (Button elem in MainGrid.Children) //Убираем цвета мува
                            {
                                if (elem.Background == Brushes.Khaki)
                                {
                                    elem.Background = new SolidColorBrush(Colors.Transparent);
                                }
                            }
                        }
                    }
                    
                    if (isThisHero) //Если нажали на уже выбранного героя
                    {
                        bt.BorderThickness = new Thickness(0); //Убираем границы
                        isActive = false; //Убираем из активных
                        foreach (Button elem in MainGrid.Children) //Убираем цвета мува
                        {
                            if (elem.Background == Brushes.Khaki)
                            {
                                elem.Background = new SolidColorBrush(Colors.Transparent);
                            }
                            if (elem.BorderThickness == new Thickness(1))
                            {
                                elem.BorderThickness = new Thickness(0);
                            }
                        }
                    }
                    else
                    {
                        foreach (Button elem in MainGrid.Children) //Определяем цвета куда может сходить герой
                        {
                            int rowBt = Grid.GetRow(elem); //Строка каждого элемента
                            int colBt = Grid.GetColumn(elem); //Столбец каждого элемента
                            if (Math.Abs(rowWantedBt - rowBt) + Math.Abs(colWantedBt - colBt) <= thisMob.ap) //Было move вместо ap, если чо поменять
                            { //Если сумма модулей разности строк и столбцов меньше или равно муву героя то
                                if (rowBt == rowWantedBt && colBt == colWantedBt) { } //Отсеиваем героев
                                else if (rowBt == gr_wizard.row && colBt == gr_wizard.col) { } //Отсеиваем мага???
                                else
                                {
                                    bool isColor = true; //По дефолту закраска на тру
                                    foreach (GridPosition gr in friednlyRngPositions)
                                    {
                                        if (rowBt == gr.row && colBt == gr.col) //Отсеиваем позиции героев на доске
                                        {
                                            isColor = false; //И ставим им фолс
                                        }
                                    }
                                    foreach (GridPosition gr in enemyRngPositions)
                                    {
                                        if (rowBt == gr.row && colBt == gr.col) //Отсеиваем позиции врагов на доске
                                        {
                                            Button btEnemy = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{rowBt}_c{colBt}"); //Ищем кнопку по имени
                                            btEnemy.BorderThickness = new Thickness(1); //ГРАНИЦЫ МОБА! МОЖНО УБРАТь!
                                            isColor = false; //И ставим им фолс
                                        }
                                    }
                                    if (isColor)
                                    {
                                        elem.Background = Brushes.Khaki; //Закрашиваем
                                    }
                                }
                            }
                        }

                        isActive = true; //Ставим активным т.к. мы выбрали героя
                        bt.BorderThickness = new Thickness(1); //Выделяем границы 
                        ButtonMovingName = bt.Name; //Запоминаем название кнопки 
                    }
                    
                }
                else //Значит моб вражеский
                {
                    int enemy_id = -1; //id вражеского моба
                    for (int i = 0; i < enemyMobs.Count; i++)
                    {//Находим вражеского моба на которого тыкнули
                        if (enemyMobs[i].gr.row == rowWantedBt && enemyMobs[i].gr.col == colWantedBt)
                        {
                            enemy_id = i;
                            break;
                        }
                    }
                    Mob enemyMob = enemyMobs[enemy_id]; //Нашли выбранного вражеского моба

                    if (isActive) //Если активирован дружеский юнит
                    {
                        int friend_id = -1;
                        Button btWanted = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, ButtonMovingName); //Берём кнопку по названию
                        for (int i = 0; i < friendlyMobs.Count; i++)
                        {//Находим активарованного дружеского моба
                            if (friendlyMobs[i].gr.row == Grid.GetRow(btWanted) && friendlyMobs[i].gr.col == Grid.GetColumn(btWanted))
                            {
                                friend_id = i;
                                break;
                            }
                        }
                        Mob friendlyMob = friendlyMobs[friend_id]; //Нашли выбранного дружеского моба
                        if (Math.Abs(enemyMob.gr.row - friendlyMob.gr.row) + Math.Abs(enemyMob.gr.col - friendlyMob.gr.col) == 1)
                        {//Если расстояние между вражеским и дружеским мобом не более одного (не может быть меньше 1 (прим. Санька))
                            if (friendlyMob.ap >= 2)
                            {
                                MessageBox.Show("За короля!"); //Лог
                                friendlyMobs[friend_id].AttackMob(enemyMobs[enemy_id]); //Атака

                                //Работает правильно вроде как
                                if (enemyMobs[enemy_id].isDead()) //Если вражеский моб мёртв после атаки
                                {// Удаляем координаты из списков, закрашиваем клетку
                                    Mob tmpMob = enemyMobs[enemy_id];
                                    enemyMobs.Remove(tmpMob);
                                    enemyMobsMadeTurn.Remove(tmpMob);
                                    GridPosition gr4del = new GridPosition();
                                    gr4del.row = tmpMob.gr.row;
                                    gr4del.col = tmpMob.gr.col;
                                    enemyRngPositions.Remove(gr4del);

                                    //Закрашиваем клетку
                                    Button btWasEnemyMob = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{tmpMob.gr.row}_c{tmpMob.gr.col}"); //Берём кнопку по названию
                                    btWasEnemyMob.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                                }

                                foreach (Button elem in MainGrid.Children)
                                {
                                    if (elem.Background == Brushes.Khaki)
                                    {
                                        elem.Background = new SolidColorBrush(Colors.Transparent);
                                    }
                                }
                                foreach (Button elem in MainGrid.Children) //Определяем цвета куда может сходить герой
                                {
                                    int rowBt = Grid.GetRow(elem); //Строка каждого элемента
                                    int colBt = Grid.GetColumn(elem); //Столбец каждого элемента

                                    if (Math.Abs(friendlyMob.gr.row - rowBt) + Math.Abs(friendlyMob.gr.col - colBt) <= friendlyMob.ap) //Было move вместо ap, если чо поменять
                                    { //Если сумма модулей разности строк и столбцов меньше или равно муву героя то
                                        if (rowBt == friendlyMob.gr.row && colBt == friendlyMob.gr.col) { } //Отсеиваем героев
                                        else if (rowBt == gr_wizard.row && colBt == gr_wizard.col) { } //Отсеиваем мага???
                                        else
                                        {
                                            bool isColor = true; //По дефолту закраска на тру
                                            foreach (GridPosition gr in friednlyRngPositions)
                                            {
                                                if (rowBt == gr.row && colBt == gr.col) //Отсеиваем позиции героев на доске
                                                {
                                                    isColor = false; //И ставим им фолс
                                                }
                                            }
                                            foreach (GridPosition gr in enemyRngPositions)
                                            {
                                                if (rowBt == gr.row && colBt == gr.col) //Отсеиваем позиции врагов на доске
                                                {
                                                    Button btEnemy = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{rowBt}_c{colBt}"); //Ищем кнопку по имени
                                                    btEnemy.BorderThickness = new Thickness(1); //ГРАНИЦЫ МОБА! МОЖНО УБРАТь!
                                                    isColor = false; //И ставим им фолс
                                                }
                                            }
                                            if (isColor)
                                            {
                                                elem.Background = Brushes.Khaki; //Закрашиваем
                                            }
                                        }
                                    }
                                }

                                //Выводим статы на экран (ОБНОВЛЯЕМ)
                                lb_leader.Content = "Лидерство: " + friendlyMob.getLeader().ToString();
                                lb_ap.Content = "Действия: " + friendlyMob.ap.ToString();
                                lb_hp.Content = "Здоровье: " + friendlyMob.getHP().ToString();
                                lb_attack.Content = "Атака: " + friendlyMob.getAttack().ToString();
                                lb_defense.Content = "Защита: " + friendlyMob.getDefense().ToString();
                                lb_luck.Content = "Удача: " + friendlyMob.getLuck().ToString();
                                lb_accuracy.Content = "Точность: " + friendlyMob.getAccuracy().ToString();
                                lb_move.Content = "Перемещение: " + friendlyMob.getMove().ToString();
                            }
                            else
                            {
                                MessageBox.Show("Нет...сил...");
                            }
                        }
                    }
                    else //Выводим инфу о враж. мобе
                    {
                        lb_leader.Content = "Лидерство: " + enemyMobs[enemy_id].getLeader().ToString();
                        lb_ap.Content = "Действия: " + enemyMobs[enemy_id].ap.ToString();
                        lb_hp.Content = "Здоровье: " + enemyMobs[enemy_id].getHP().ToString();
                        lb_attack.Content = "Атака: " + enemyMobs[enemy_id].getAttack().ToString();
                        lb_defense.Content = "Защита: " + enemyMobs[enemy_id].getDefense().ToString();
                        lb_luck.Content = "Удача: " + enemyMobs[enemy_id].getLuck().ToString();
                        lb_accuracy.Content = "Точность: " + enemyMobs[enemy_id].getAccuracy().ToString();
                        lb_move.Content = "Перемещение: " + enemyMobs[enemy_id].getMove().ToString();
                        //MessageBox.Show(enemyMobs[enemy_id].name);
                    }
                }
            }
            else //Нажали на пустую клетку
            {
                
                if (ButtonMovingName != "" && bt.Background == Brushes.Khaki) 
                {
                    Button btWanted = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, ButtonMovingName); //Берём кнопку по названию
                    string nameToPic = ((btWanted as Button).Background as ImageBrush).ImageSource.ToString(); //Название картинки которая СТОЯЛА
                    btWanted.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                    btWanted.BorderThickness = new Thickness(0); //Убираем границы
                    isActive = false; //Актив на ноль, т.к. мы переместили героя

                    // Вычитаем пройденные клетки из очкой действий (ар)

                    // 1. Находим дружского моба по координатам

                    Mob thisMob = null; //Берём моба

                    for (int i = 0; i < friendlyMobs.Count; i++) //Обновление координат
                    {
                        if (friendlyMobs[i].gr.row == Grid.GetRow(btWanted) && friendlyMobs[i].gr.col == Grid.GetColumn(btWanted))
                        {//Находим координаты клетки где БЫЛ моб
                            GridPosition tmpGr; //Новая тмп переменная
                            tmpGr.row = Grid.GetRow(bt); //Находим текущую строку
                            tmpGr.col = Grid.GetColumn(bt); //Находим текущий столбец

                            for (int j = 0; j < friednlyRngPositions.Count; j++)
                            { //Находим нужные координаты в списке координат чтобы их изменить
                                if (friednlyRngPositions[i].row == friendlyMobs[i].gr.row && friednlyRngPositions[i].col == friendlyMobs[i].gr.col)
                                { //Сравниваем координаты из списка с координатами моба
                                    friednlyRngPositions[i] = tmpGr; //Перезаписываем инфу в списке
                                    break;
                                }
                            }
                            thisMob = friendlyMobs[i]; // Запоминаем моба
                            friendlyMobs[i].gr = tmpGr; //Перезаписываем инфу у моба
                            friendlyMobs[i].ap -= Math.Abs(Grid.GetRow(btWanted) - Grid.GetRow(bt)) + Math.Abs(Grid.GetColumn(btWanted) - Grid.GetColumn(bt));
                        }
                    }
                    
                    ButtonMovingName = ""; //Убираем имя кнопки т.к. переместились

                    var brush = new ImageBrush(); 
                    brush.ImageSource = new BitmapImage(new Uri(nameToPic, UriKind.Relative)); //Меняем картинку
                    bt.Background = brush;

                    foreach (Button elem in MainGrid.Children) //Убираем краску
                    {
                        if (elem.Background == Brushes.Khaki)
                        {
                            elem.Background = new SolidColorBrush(Colors.Transparent);
                        }
                        if (elem.BorderThickness == new Thickness(1))
                        {
                            elem.BorderThickness = new Thickness(0);
                        }
                        
                    }
                    //Выводим статы на экран (ОБНОВЛЯЕМ)
                    lb_leader.Content = "Лидерство: " + thisMob.getLeader().ToString();
                    lb_ap.Content = "Действия: " + thisMob.ap.ToString();
                    lb_hp.Content = "Здоровье: " + thisMob.getHP().ToString();
                    lb_attack.Content = "Атака: " + thisMob.getAttack().ToString();
                    lb_defense.Content = "Защита: " + thisMob.getDefense().ToString();
                    lb_luck.Content = "Удача: " + thisMob.getLuck().ToString();
                    lb_accuracy.Content = "Точность: " + thisMob.getAccuracy().ToString();
                    lb_move.Content = "Перемещение: " + thisMob.getMove().ToString();
                }
                else
                {
                    //MessageBox.Show("Test!");
                }
            }
        }

        private void MoveCharacterTest(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, "r9_c9");
            //object bt = (Button)this.FindName("r9_c9");
            if (bt is Button)
            {
                //MessageBox.Show("hello");
                Button needBt = bt as Button;
                needBt.Background = Brushes.Red;
            }
        }

        /// <summary>
        /// Метод определяет, победил игрок на этому ходу или проиграл
        /// </summary>
        private bool isVictoryDefeat()
        {
            if (enemyMobs.Count <= 0)
            {
                MessageBox.Show("Вы победили!");
                this.Close();
            }
            if (friendlyMobs.Count <= 0)
            {
                MessageBox.Show("Вы проиграли!");
                this.Close();
            }
            return (friendlyMobs.Count <= 0) || (enemyMobs.Count <= 0);
        }

        private void Reload(object sender, RoutedEventArgs e)
        {
            //GameWindow win = new GameWindow();
            //win.Show();
            //this.Close();
        }

        /// <summary>
        /// МЕТОД ЗАВЕРШЕНИЯ ХОДА
        /// ОБРАБАТЫВАЕТ ВСЁ ЧТО МОЖНО
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinishTurn(object sender, RoutedEventArgs e)
        {
            if (isVictoryDefeat()) { ; }
            else
            {
                isYourTurn = isYourTurn ? false : true; //Меняем ход
                if (isYourTurn) //Если ход игрока
                {
                    MessageBox.Show("Ваш ход!");
                    Mob mob = null; //Объявляем моба
                    int maxLeader = -1;
                    foreach (Mob m in friendlyMobs)
                    {//Ищем моба с максимальным лидерством
                        m.ap = m.move;
                        if (m.getLeader() > maxLeader && friendlyMobsMadeTurn.Contains(m) == false)
                        { maxLeader = m.getLeader(); mob = m; }
                    }
                    friendlyMobsMadeTurn.Add(mob); //Добавляем его в список
                    if (friendlyMobsMadeTurn.Count == friendlyMobs.Count)
                    {//Если список полон, т.е. все мобы походили, то список обновляется
                        friendlyMobsMadeTurn.Clear();
                    }

                    //Находим моба с максимальным лидерством и даём ему право походить
                    Button btWanted = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{mob.gr.row}_c{mob.gr.col}"); //Берём кнопку по названию
                    btWanted.IsEnabled = true;

                    //Логика дружеского ИИ:
                    //1. Если на момент НАЧАЛА ХОДА ДРУЖЕСКОГО МАГА враг находится на расстоянии ОДНОЙ клетки, то ударить его мечом
                    //2. Если враг в зоне действия магии - плюнуть фаерболом в лицо
                    //3. В противном случае переместиться в точку, наиболее близкую к ближайшему противнику
                    //В конце каждого N хода игрока будет ходить дружеский маг
                    if ((NumberTurn / 2) % TurnN == 0)
                    {
                        MessageBox.Show("Чародейка ходит!"); //Лог для мага
                                                             //Кнопка, на которой БЫЛ маг
                        Button btWasWizard = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{friendlyWizard.gr.row}_c{friendlyWizard.gr.col}"); //Берём кнопку по названию
                        int rowWizard = Grid.GetRow(btWasWizard); //Строка где БЫЛ маг
                        int colWizard = Grid.GetColumn(btWasWizard); //Столбец где БЫЛ маг

                        List<Button> enemiesInArea = new List<Button>();//Находится ли враг в поле зрения

                        foreach (Button elem in MainGrid.Children) //Определяем цвета куда может сходить маг
                        {
                            int rowBt = Grid.GetRow(elem); //Строка каждого элемента
                            int colBt = Grid.GetColumn(elem); //Столбец каждого элемента
                            if (Math.Abs(rowWizard - rowBt) + Math.Abs(colWizard - colBt) <= friendlyWizard.ap) //Было move вместо ap, если чо поменять
                            { //Если сумма модулей разности строк и столбцов меньше или равно муву мага то
                                if (rowBt == rowWizard && colBt == colWizard) { } //Отсеиваем героев
                                else if (rowBt == gr_wizard.row && colBt == gr_wizard.col) { } //Отсеиваем мага???
                                else
                                {
                                    bool isColor = true; //По дефолту закраска на тру
                                    foreach (GridPosition gr in friednlyRngPositions)
                                    {
                                        if (rowBt == gr.row && colBt == gr.col) //Отсеиваем позиции друзей на доске
                                        {
                                            isColor = false; //И ставим им фолс
                                        }
                                    }
                                    foreach (GridPosition gr in enemyRngPositions)
                                    {
                                        if (rowBt == gr.row && colBt == gr.col) //Отсеиваем позиции врагов на доске
                                        {
                                            Button btEnemy = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{rowBt}_c{colBt}"); //Ищем кнопку по имени
                                            btEnemy.BorderThickness = new Thickness(1); //ГРАНИЦЫ МОБА! МОЖНО УБРАТь!
                                            enemiesInArea.Add(btEnemy); //Дружеский моб находится в области действия
                                            isColor = false; //И ставим им фолс
                                        }
                                    }
                                    if (isColor)
                                    {
                                        elem.Background = Brushes.HotPink; //Закрашиваем
                                    }
                                }
                            }
                        }
                        bool canWizardAttack = false; //Может ли вражеский моб атаковать на данный ход
                        Button targetAttack = null; //Цель для атаки (кнопка)

                        if (enemiesInArea.Count > 0) //Если в радиусе действия есть враги (для нас друж. мобы)
                        {//Проверяем, можем ли мы атаковать их прямо сейчас
                            foreach (Button elem in enemiesInArea)
                            {//Проверяем для каждого элемента в радиусе действия дистанцию
                                if (Math.Abs(Grid.GetRow(elem) - Grid.GetRow(btWasWizard)) +
                                    Math.Abs(Grid.GetColumn(elem) - Grid.GetColumn(btWasWizard)) == 1)
                                {//Если дистанция равна единице, то отмечаем цель для атаки и возможность атаки на тру
                                    targetAttack = elem;
                                    canWizardAttack = true;
                                }
                            }
                        }
                        if (canWizardAttack) //Может ли маг дать шашкой по лицу прям щас
                        {
                            foreach (Button elem in MainGrid.Children)
                            {//Убираем выделение цвета для ИИ куда он мог сходить
                                if (elem.Background == Brushes.HotPink)
                                {
                                    elem.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                                }
                            }

                            Mob enemyMob4Attack = null; //Объявляем переменную для идентификации враж. моба
                            foreach (Mob m in enemyMobs)
                            {//Находим вражеского моба, которого атаковал маг
                                if (m.gr.row == Grid.GetRow(targetAttack) &&
                                    m.gr.col == Grid.GetColumn(targetAttack))
                                {
                                    enemyMob4Attack = m;
                                    //break;
                                }
                            }
                            while (friendlyWizard.ap >= 2 && enemyMob4Attack != null)
                            {//Если у мага больше 2 ОД то враг атакует
                                MessageBox.Show("Чародейка атакует врага!"); //Лог
                                friendlyWizard.AttackMob(enemyMob4Attack);

                                //Работает правильно вроде как
                                if (enemyMob4Attack.isDead()) //Если вражеский моб мёртв после атаки
                                {// Удаляем координаты из списков, закрашиваем клетку
                                    Mob tmpMob = enemyMob4Attack;
                                    enemyMobs.Remove(tmpMob);
                                    enemyMobsMadeTurn.Remove(tmpMob);
                                    GridPosition gr4del = new GridPosition();
                                    gr4del.row = tmpMob.gr.row;
                                    gr4del.col = tmpMob.gr.col;
                                    enemyRngPositions.Remove(gr4del);

                                    //Закрашиваем клетку
                                    Button btWasEnemyMob = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{tmpMob.gr.row}_c{tmpMob.gr.col}"); //Берём кнопку по названию
                                    btWasEnemyMob.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                                }
                            }
                        }
                        else //Если не может, то смотрим может ли плюнуть фаерболом
                        {
                            bool canWizardSpell = false;
                            Mob targetSpell = null;
                            foreach (Mob eMob in enemyMobs)
                            {
                                int distance = Math.Abs(eMob.gr.row - friendlyWizard.gr.row) +
                                    Math.Abs(eMob.gr.col - friendlyWizard.gr.col);
                                if (distance <= friendlyWizard.heroSpell.distance)
                                {
                                    canWizardSpell = true;
                                    targetSpell = eMob;
                                    break;
                                }
                            }
                            if (canWizardSpell && targetSpell != null) //Значит может плюнуть фаерболом
                            {
                                MessageBox.Show("Кидаю фаербол!"); //Log
                                friendlyWizard.CastSpell(targetSpell);

                                //Работает правильно вроде как
                                if (targetSpell.isDead()) //Если вражеский моб мёртв после атаки
                                {// Удаляем координаты из списков, закрашиваем клетку
                                    Mob tmpMob = targetSpell;
                                    enemyMobs.Remove(tmpMob);
                                    enemyMobsMadeTurn.Remove(tmpMob);
                                    GridPosition gr4del = new GridPosition();
                                    gr4del.row = tmpMob.gr.row;
                                    gr4del.col = tmpMob.gr.col;
                                    enemyRngPositions.Remove(gr4del);

                                    //Закрашиваем клетку
                                    Button btWasEnemyMob = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{tmpMob.gr.row}_c{tmpMob.gr.col}"); //Берём кнопку по названию
                                    btWasEnemyMob.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                                }

                                foreach (Button elem in MainGrid.Children)
                                {//Закрашиваем выделенные клетки
                                    if (elem.Background == Brushes.HotPink)
                                    {
                                        elem.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                                    }
                                }
                            }
                            else //Тогда просто перемещаемся на ближайшую к врагу клетку
                            {
                                MessageBox.Show("Чародейка бежит!");
                                GridPosition bestPosition = new GridPosition(); //находим лучшую позицию для перемещения
                                int minPos = 100; //Условно 100 
                                foreach (Button elem in MainGrid.Children)
                                {
                                    if (elem.Background == Brushes.HotPink)
                                    {//Смотрим на выделенные клетки
                                        foreach (Mob enemyMob in enemyMobs)
                                        {
                                            int d_row = Grid.GetRow(elem);
                                            int d_col = Grid.GetColumn(elem);
                                            int f_row = enemyMob.gr.row;
                                            int f_col = enemyMob.gr.col;

                                            int distance = Math.Abs(d_row - f_row) + Math.Abs(d_col - f_col);
                                            if (distance < minPos)
                                            {//Находим минимальную дистанцию до ближайшего дружеского (нам) моба
                                                minPos = distance;
                                                bestPosition.row = d_row;
                                                bestPosition.col = d_col;
                                            }
                                        }
                                    }
                                }
                                Button btWizardMove = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{bestPosition.row}_c{bestPosition.col}"); //Берём кнопку по названию
                                var brush = new ImageBrush();
                                brush.ImageSource = new BitmapImage(new Uri("img/Wizard.png", UriKind.Relative)); //Меняем картинку
                                btWizardMove.Background = brush; //Как бы перемещаемся
                                btWasWizard.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                                btWasWizard.BorderThickness = new Thickness(0); //Убираем границы
                                                                                //Allo
                                                                                //Обвноляем координаты у мага
                                friendlyWizard.gr.row = Grid.GetRow(btWizardMove);
                                friendlyWizard.gr.col = Grid.GetColumn(btWizardMove);
                                gr_wizard.row = Grid.GetRow(btWizardMove);
                                gr_wizard.col = Grid.GetColumn(btWizardMove);
                                //Заканчиваем обновлять координаты у мага
                                btWasWizard.IsEnabled = true;
                                btWizardMove.IsEnabled = false;

                                foreach (Button elem in MainGrid.Children)
                                {//Закрашиваем выделенные клетки
                                    if (elem.Background == Brushes.HotPink)
                                    {
                                        elem.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                                    }
                                }
                            }
                        }
                    }
                }
                else //Если ход ИИ
                {
                    //1. Найти клетку, которая будет ближайшей к любому из дружеских мобов
                    //2. Встать на неё
                    //3. Если остались ОД - то ударить дружеского моба
                    MessageBox.Show("Ход оппонента!");
                    Mob enemyMob = null;
                    int maxLeader = -1;
                    foreach (Mob m in enemyMobs)
                    {//Находим вражеского моба с наибольшим лидерством
                        m.ap = m.move;
                        if (m.getLeader() > maxLeader && enemyMobsMadeTurn.Contains(m) == false)
                        { maxLeader = m.getLeader(); enemyMob = m; }
                    }
                    enemyMobsMadeTurn.Add(enemyMob); //Добавляем его в список
                    if (enemyMobsMadeTurn.Count == enemyMobs.Count)
                    {//Если список полон, т.е. все мобы походили, то список обновляется
                        enemyMobsMadeTurn.Clear();
                    }
                    //Кнопка, на которой БЫЛ монстр
                    Button btWasEnemyMob = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{enemyMob.gr.row}_c{enemyMob.gr.col}"); //Берём кнопку по названию
                    int rowWantedBt = Grid.GetRow(btWasEnemyMob); //Строка где БЫЛ монстр
                    int colWantedBt = Grid.GetColumn(btWasEnemyMob); //Столбец где БЫЛ монстр

                    List<Button> enemiesInArea = new List<Button>();//Находится ли враг в поле зрения

                    foreach (Button elem in MainGrid.Children) //Определяем цвета куда может сходить враг
                    {
                        int rowBt = Grid.GetRow(elem); //Строка каждого элемента
                        int colBt = Grid.GetColumn(elem); //Столбец каждого элемента
                        if (Math.Abs(rowWantedBt - rowBt) + Math.Abs(colWantedBt - colBt) <= enemyMob.ap) //Было move вместо ap, если чо поменять
                        { //Если сумма модулей разности строк и столбцов меньше или равно муву героя то
                            if (rowBt == rowWantedBt && colBt == colWantedBt) { } //Отсеиваем героев
                            else if (rowBt == gr_wizard.row && colBt == gr_wizard.col) { } //Отсеиваем мага???
                            else
                            {
                                bool isColor = true; //По дефолту закраска на тру
                                foreach (GridPosition gr in enemyRngPositions)
                                {
                                    if (rowBt == gr.row && colBt == gr.col) //Отсеиваем позиции врагов на доске
                                    {
                                        isColor = false; //И ставим им фолс
                                    }
                                }
                                foreach (GridPosition gr in friednlyRngPositions)
                                {
                                    if (rowBt == gr.row && colBt == gr.col) //Отсеиваем позиции героев на доске
                                    {
                                        Button btEnemy = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{rowBt}_c{colBt}"); //Ищем кнопку по имени
                                        btEnemy.BorderThickness = new Thickness(1); //ГРАНИЦЫ МОБА! МОЖНО УБРАТь!
                                        enemiesInArea.Add(btEnemy); //Дружеский моб находится в области действия
                                        isColor = false; //И ставим им фолс
                                    }
                                }
                                if (isColor)
                                {
                                    elem.Background = Brushes.DeepSkyBlue; //Закрашиваем
                                }
                            }
                        }
                    }

                    bool canMobAttack = false; //Может ли вражеский моб атаковать на данный ход
                    Button targetAttack = null; //Цель для атаки (кнопка)

                    if (enemiesInArea.Count > 0) //Если в радиусе действия есть враги (для нас друж. мобы)
                    {//Проверяем, можем ли мы атаковать их прямо сейчас
                        foreach (Button elem in enemiesInArea)
                        {//Проверяем для каждого элемента в радиусе действия дистанцию
                            if (Math.Abs(Grid.GetRow(elem) - Grid.GetRow(btWasEnemyMob)) +
                                Math.Abs(Grid.GetColumn(elem) - Grid.GetColumn(btWasEnemyMob)) == 1)
                            {//Если дистанция арвна единице, то отмечаем цель для атаки и возможность атаки на тру
                                targetAttack = elem;
                                canMobAttack = true;
                            }
                        }
                    }
                    if (canMobAttack) //Если враг может атаковать прямо сейчас
                    {
                        foreach (Button elem in MainGrid.Children)
                        {//Убираем выделение цвета для ИИ куда он мог сходить
                            if (elem.Background == Brushes.DeepSkyBlue)
                            {
                                elem.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                            }
                        }

                        Mob friendlyMob4Attack = null; //Объявляем переменную для идентификации друж. моба
                        foreach (Mob m in friendlyMobs)
                        {//Находим дружеского моба, которого атаковал враг
                            if (m.gr.row == Grid.GetRow(targetAttack) &&
                                m.gr.col == Grid.GetColumn(targetAttack))
                            {
                                friendlyMob4Attack = m;
                                //break;
                            }
                        }
                        while (enemyMob.ap >= 2 && friendlyMob4Attack != null)
                        {//Если у врага больше 2 ОД то враг атакует
                            MessageBox.Show("Вражеский монстр ударил вашего героя!"); //Лог
                            enemyMob.AttackMob(friendlyMob4Attack); //

                            if (friendlyMob4Attack.isDead()) //Если вражеский моб мёртв после атаки
                            {// Удаляем координаты из списков, закрашиваем клетку
                                Mob tmpMob = friendlyMob4Attack;
                                friendlyMobs.Remove(tmpMob);
                                friendlyMobsMadeTurn.Remove(tmpMob);
                                GridPosition gr4del = new GridPosition();
                                gr4del.row = tmpMob.gr.row;
                                gr4del.col = tmpMob.gr.col;
                                friednlyRngPositions.Remove(gr4del);

                                //Закрашиваем клетку
                                Button btWasFriendlyMob = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{tmpMob.gr.row}_c{tmpMob.gr.col}"); //Берём кнопку по названию
                                btWasFriendlyMob.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                            }
                        }
                    }
                    else
                    {//Если враг не может атаковать ПРЯМ ЩАС
                        GridPosition bestPosition = new GridPosition(); //находим лучшую позицию для перемещения
                        int minPos = 100; //Условно 100 
                        foreach (Button elem in MainGrid.Children)
                        {
                            if (elem.Background == Brushes.DeepSkyBlue)
                            {//Смотрим на выделенные клетки
                                foreach (Mob friendlyMob in friendlyMobs)
                                {
                                    int d_row = Grid.GetRow(elem);
                                    int d_col = Grid.GetColumn(elem);
                                    int f_row = friendlyMob.gr.row;
                                    int f_col = friendlyMob.gr.col;

                                    int distance = Math.Abs(d_row - f_row) + Math.Abs(d_col - f_col);
                                    if (distance < minPos)
                                    {//Находим минимальную дистанцию до ближайшего дружеского (нам) моба
                                        minPos = distance;
                                        bestPosition.row = d_row;
                                        bestPosition.col = d_col;
                                    }
                                }
                            }
                        }

                        Button btEnemyMove = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{bestPosition.row}_c{bestPosition.col}"); //Берём кнопку по названию
                        var brush = new ImageBrush();
                        brush.ImageSource = new BitmapImage(new Uri("img/dragon.png", UriKind.Relative)); //Меняем картинку
                        btEnemyMove.Background = brush; //Как бы перемещаемся
                        btWasEnemyMob.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                        btWasEnemyMob.BorderThickness = new Thickness(0); //Убираем границы

                        //Обвноляем координаты у моба
                        enemyMob.gr.row = Grid.GetRow(btEnemyMove);
                        enemyMob.gr.col = Grid.GetColumn(btEnemyMove);
                        for (int i = 0; i < enemyRngPositions.Count; i++)
                        {//Обновляем координаты в изначальном списке (так надо!)
                            if (enemyRngPositions[i].row == Grid.GetRow(btWasEnemyMob) &&
                                enemyRngPositions[i].col == Grid.GetColumn(btWasEnemyMob))
                            {
                                GridPosition tmp;
                                tmp.row = enemyMob.gr.row;
                                tmp.col = enemyMob.gr.col;
                                enemyRngPositions[i] = tmp;
                            }
                        }

                        foreach (Button elem in MainGrid.Children)
                        {//Закрашиваем выделенные клетки
                            if (elem.Background == Brushes.DeepSkyBlue)
                            {
                                elem.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                            }
                        }

                        //После того как переместился
                        if (enemiesInArea.Count > 0)
                        {//Идёт опять проверка на атаку
                            foreach (Button elem in enemiesInArea)
                            {//Если дистанция равна единице то враг может атаковать
                                if (Math.Abs(Grid.GetRow(elem) - Grid.GetRow(btEnemyMove)) +
                                    Math.Abs(Grid.GetColumn(elem) - Grid.GetColumn(btEnemyMove)) == 1)
                                {
                                    targetAttack = elem;
                                    canMobAttack = true;
                                }
                            }
                        }
                        if (canMobAttack)
                        {//Если враг может атаковать после того как сходил то он... атакует!
                            Mob friendlyMob4Attack = null;
                            foreach (Mob m in friendlyMobs)
                            {//Идентифицируем дружеского (нам) моба
                                if (m.gr.row == Grid.GetRow(targetAttack) &&
                                    m.gr.col == Grid.GetColumn(targetAttack))
                                {
                                    friendlyMob4Attack = m;
                                    //break;
                                }
                            }
                            while (enemyMob.ap >= 2 && friendlyMob4Attack != null)
                            {//Бьём пока есть силы
                                MessageBox.Show("Вражеский монстр атаковал!!");
                                enemyMob.AttackMob(friendlyMob4Attack);

                                if (friendlyMob4Attack.isDead()) //Если вражеский моб мёртв после атаки
                                {// Удаляем координаты из списков, закрашиваем клетку
                                    Mob tmpMob = friendlyMob4Attack;
                                    friendlyMobs.Remove(tmpMob);
                                    friendlyMobsMadeTurn.Remove(tmpMob);
                                    GridPosition gr4del = new GridPosition();
                                    gr4del.row = tmpMob.gr.row;
                                    gr4del.col = tmpMob.gr.col;
                                    friednlyRngPositions.Remove(gr4del);

                                    //Закрашиваем клетку
                                    Button btWasFriendlyMob = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{tmpMob.gr.row}_c{tmpMob.gr.col}"); //Берём кнопку по названию
                                    btWasFriendlyMob.Background = new SolidColorBrush(Colors.Transparent); //Ставим цвет обычный
                                }
                            }
                        }
                    }

                    //Не трогать!
                    foreach (Mob m in friendlyMobs)
                    {//Блочим все кнопки во время вражеского хода
                        Button bts = (Button)LogicalTreeHelper.FindLogicalNode(MainGrid, $"r{m.gr.row}_c{m.gr.col}"); //Берём кнопку по названию
                        bts.IsEnabled = false;
                    }
                    PerformClick(EndTurn);
                }
                Turn.Content = $"Ход: {++NumberTurn}";//Следующий ход
            }
        }

        private void btExiot(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public static void PerformClick(Button btn)
        {
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
