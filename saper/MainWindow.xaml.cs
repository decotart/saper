using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace saper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int[] pattern = { -11, -10, -9, -1, 1, 9, 10, 11 };
        Button[] map;
        bool[] bombMap;
        Random rnd = new();
        bool isDiging = true;
        int score,
            bombCount;

        private void btnMap_Click(object sender, RoutedEventArgs e)
        {
            int index = Convert.ToInt32(((Button)sender).Name.Trim('q')),
                count;
            
            if (isDiging)
            {
                if (bombMap[index])
                {
                    EndGame(false);
                }
                else
                {
                    count = GetCountOfBombs(index);

                    if (count != 0)
                    {
                        ((Button)sender).Content = count.ToString();
                        ((Button)sender).Background = Brushes.White;
                        ((Button)sender).Foreground = Brushes.BlueViolet;
                        ((Button)sender).IsEnabled = false;
                    }
                    else
                    {
                        CheckIfCellEmpty(index);
                    }
                }
            }
            else
            {
                ((Button)sender).Content = "Ф";
                ((Button)sender).Background = Brushes.LightCoral;

                if (bombMap[index])
                {
                    score++;
                }
            }
            CheckForWinning();
        }

        public void CheckIfCellEmpty(int index)
        {
            bool foo = true;
            map[index].IsEnabled = false;

            List<int> emptyIndexList = new List<int>();
            emptyIndexList.Add(index);

            //while (foo)
            //{
                int counter = 0;

                for (int i = 0; i < pattern.Length; i++)
                {
                    if (GetCountOfBombs(emptyIndexList[counter] + pattern[i]) == 0)
                    {
                        emptyIndexList.Add(emptyIndexList[counter] + pattern[i]);

                        map[emptyIndexList[counter] + pattern[i]].IsEnabled = false;
                        map[emptyIndexList[counter] + pattern[i]].Background = Brushes.White;
                    }
                }

                counter++;
                if (counter == emptyIndexList.Count)
                {
                    foo = false;
                }
            //}
        }

        public void CheckForWinning()
        {
            int counter = 0;

            foreach (Button btn in map)
            {
                if (!btn.IsEnabled)
                {
                    counter++;
                }
            }

            if (score == bombCount && counter == (100 - bombCount))
            {
                EndGame(true);
            }
        }

        public void EndGame(bool f)
        {
            if (f)
            {
                MessageBox.Show("Вы выиграли!");
                foreach (Button b in map)
                {
                    b.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                MessageBox.Show("Вы проиграли!");
                foreach (Button b in map)
                {
                    b.Visibility = Visibility.Hidden;
                }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bombCount = Convert.ToInt32(tbBombCount.Text);

                score = 0;

                if (bombCount > 100)
                {
                    throw new Exception();
                }

                map = new Button[100];
                bombMap = new bool[100];

                Button btn;

                for (int i = 0; i < 100; i++)
                {
                    btn = new();

                    if (i < 10)
                    {
                        Grid.SetRow(btn, 0);
                        Grid.SetColumn(btn, i);
                    }
                    else
                    {
                        Grid.SetRow(btn, i / 10);
                        Grid.SetColumn(btn, i % 10);
                    }

                    btn.Name = $"q{i}";
                    btn.Content = $"{i}";
                    btn.FontSize = 30;
                    btn.Click += btnMap_Click;

                    gridMap.Children.Add(btn);
                    map[i] = btn;
                }

                for (int i = 0; i < 100; i++)
                {
                    bombMap[i] = false;
                }

                int row;

                

                for (int i = 0; i < bombCount; i++)
                {
                    row = rnd.Next(0, 100);

                    if (bombMap[row] != true)
                    {
                        bombMap[row] = true;
                    }
                    else
                    {
                        i--;
                    }
                }
            }
            catch
            {
                Echo();
            }
        }

        private void btnDigFlag_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isDiging)
                {
                    isDiging = !isDiging;
                    btnDigFlag.Content = "Флаг";
                }
                else
                {
                    isDiging = !isDiging;
                    btnDigFlag.Content = "Лопата";
                }
            }
            catch
            {
                Echo();
            }
        }

        public void Echo()
        {
            MessageBox.Show("Что-то пошло не так");
        }

        public int GetCountOfBombs(int index)
        {

            int counter = 0,
                temp;

            for (int i = 0; i < pattern.Length; i++)
            {
                temp = index + pattern[i];

                if (temp >= 0 && temp <= 99)
                {
                    if (bombMap[temp])
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }
    }
}