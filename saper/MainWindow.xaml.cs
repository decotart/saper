using System.Reflection;
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
                    ViewLosingMap();
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
                        map[index].IsEnabled = false;
                        map[index].Background = Brushes.White;
                        map[index].Content = "";
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

        public void ViewLosingMap()
        {
            int index;

            foreach (Button btn in map)
            {
                index = Convert.ToInt32(btn.Name.Trim('q'));

                if (bombMap[index] == true)
                {
                    btn.Background = Brushes.Red;
                    btn.Content = "Б";
                }
                else
                {
                    btn.Background = Brushes.LightGray;
                    btn.Content = GetCountOfBombs(index).ToString();
                }
            }
        }

        List<int> loopList = new List<int>();

        public void CheckIfCellEmpty(int index)
        {
            loopList.Add(index);

            foreach (int i in pattern)
            {
                if (index + i >= 0 && index + i <= 99)
                {
                    if (GetCountOfBombs(index + i) == 0)
                    {
                        if (!loopList.Contains(index + i))
                        {
                            loopList.Add(index + i);

                            map[index + i].IsEnabled = false;
                            map[index + i].Background = Brushes.White;
                            map[index + i].Content = "";

                            Loop(loopList);
                        }
                    }
                    else
                    {
                        map[index + i].Content = GetCountOfBombs(index + i).ToString();
                        map[index + i].Background = Brushes.White;
                        map[index + i].Foreground = Brushes.BlueViolet;
                        map[index + i].IsEnabled = false;
                    }
                }
            }
        }

        public void Loop(List<int> list)
        {
            int[] temp = new int[list.Count];
            temp = list.ToArray();

            foreach (int i in temp)
            {
                CheckIfCellEmpty(i);
            }
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
                    //btn.Content = $"{i}";
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