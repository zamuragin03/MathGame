using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace MathGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double num1 { get; set; }
        public double num2 { get; set; }
        public double result { get; set; }
        public Signs sign { get; set; }

        private int totalcounter;
        private int trueCounter;
        private int falseCounter;

        private readonly Random rnd = new();
        bool isWork;
        private Mather task;


        private readonly DispatcherTimer timer1;
        private TimeSpan SpanTime1;

        private readonly DispatcherTimer timer2;
        private TimeSpan SpanTime2;

        private readonly DispatcherTimer timer3;
        private TimeSpan SpanTime3;

        private readonly DispatcherTimer timer4;
        private TimeSpan SpanTime4;

        private readonly List<string> textBoxes = new();
        private readonly List<string> answers = new();


        private bool iswordgamestarted = true;

        NumericGame game = new();

        public MainWindow()
        {
            InitializeComponent();
            Domath();

            timer1 = new DispatcherTimer();
            timer1.Tick += timer1_Tick;
            timer1.Interval = new TimeSpan(0, 0, 0, 1, 0);


            timer2 = new DispatcherTimer();
            timer2.Tick += timer2_Tick;
            timer2.Interval = new TimeSpan(0, 0, 0, 1, 0);


            timer3 = new DispatcherTimer();
            timer3.Tick += timer3_Tick;
            timer3.Interval = new TimeSpan(0, 0, 0, 1, 0);

            timer4 = new DispatcherTimer();
            timer4.Tick += timer4_Tick;
            timer4.Interval = new TimeSpan(0, 0, 0, 1, 0);

            foreach (var Stackitems in Grid.Children)
            {
                if (Stackitems is StackPanel panel)
                {
                    foreach (var item in panel.Children)
                    {
                        if (item is TextBox box)
                        {
                            box.SpellCheck.IsEnabled = true;
                            box.Language = XmlLanguage.GetLanguage("ru-Ru");
                            box.Background = new SolidColorBrush(Color.FromRgb(5,203 , 255));

                        }
                    }

                }
            }
            int i = 0;
            int[] temp = game.GetNumbers();

            foreach (var Stackitems in Numbers.Children)
            {
                if (Stackitems is StackPanel panel)
                {
                    foreach (var item in panel.Children)
                    {
                        if (item is Button button)
                        {
                            button.Click += Click;
                            button.Content = temp[i];
                            button.FontSize = 30;
                            i++;
                        }
                    }
                }
            }
            string[] list = File.ReadAllLines("bestresult.txt");
            BestScore.Content = $"Лучший: {list[0]}:{list[1]}";
            answelabel.Content = $" Следующее число — 1";

        }

        

        private void timer3_Tick(object sender, EventArgs e)
        {
            SpanTime3 = SpanTime3.Add(timer3.Interval);

            timerlabel.Content = $"Время {SpanTime3.Minutes}:{SpanTime3.Seconds}";
        }
        private void Click(object sender, RoutedEventArgs e)
        {

            if (game.CurrentNumber < 25) 
            {
                if (!timer3.IsEnabled)
                {
                    timer3.Start();
                }
                int NextValue = (int)((Button)e.OriginalSource).Content;
                game.SetNumber(NextValue);


                if (game.IsNumbersSequent())
                {
                    answelabel.Content = $" Следующее число — {game.CurrentNumber + 1}";
                    answelabel.Foreground = new SolidColorBrush(Colors.Black);

                }
                else
                    answelabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            if(game.CurrentNumber>=25)
            {
                game = new NumericGame();
                timer3.Stop();

                MessageBox.Show("Игра окончена", "Помощник", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                answelabel.Content = "";
                using var temp = new StreamWriter("result.txt", true);
                temp.WriteLine(new string('—', 16) + "Игра 25" + new string('—', 17));
                temp.WriteLine($"Дата выполнения — {DateTime.Now}");
                temp.WriteLine(new string('—', 40));
                temp.WriteLine(SpanTime3.Seconds +" Секунд " + SpanTime3.Minutes+ " Минут");

                string[] list = File.ReadAllLines("bestresult.txt");

                if (int.Parse(list[0])>=SpanTime3.Minutes && int.Parse(list[1])>=SpanTime3.Seconds)
                {
                    using var besttemp = new StreamWriter("bestresult.txt", false);
                    besttemp.WriteLine(SpanTime3.Minutes);
                    besttemp.WriteLine(SpanTime3.Seconds);
                    BestScore.Content = $"Лучший: {SpanTime3.Minutes}:{SpanTime3.Seconds}";
                }
                SpanTime3 = new TimeSpan(0, 0, 0, 0);

            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            SpanTime2 = SpanTime2.Add(timer2.Interval);

            if (SpanTime2 != TimeSpan.FromSeconds(120))
            {
                Title = $"Math Game Время — {SpanTime2.Minutes}:{SpanTime2.Seconds}";
            }
            else
            {
                timer2.Stop();
                CheckButton.IsEnabled = true;
                foreach (var Stackitems in Grid.Children)
                {
                    if (Stackitems is StackPanel panel)
                    {
                        foreach (var item in panel.Children)
                        {
                            if (item is TextBox box)
                            {
                                box.Text = "";
                                box.IsEnabled = true;

                            }
                        }

                    }
                }

            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            SpanTime1 = SpanTime1.Add(timer1.Interval);
            timerlbl.Text = $"{ SpanTime1.Minutes}:{ SpanTime1.Seconds}";

        }
        public void AddResult()
        {
            string tempstring = $"{totalcounter}) {task.num1}{Mather.GetSignTranslation(sign)}{task.num2}={task.num3} -{task.GetResultTranslation()} \n";
            using var temp = new StreamWriter("result.txt", true);
            temp.Write(tempstring);
            resultlabel.Text += tempstring;
        }
        public void Domath()
        {
            int temp = rnd.Next(1,6);
            switch (temp)
            {
                case 2:
                    sign = Signs.plus;
                    sgn1.Content = "+";
                    num1 = rnd.Next(70);
                    num2 = rnd.Next(50);
                    Num1.Content = num1;
                    Num2.Content = num2;
                    break;
                case 3:
                    sign = Signs.minus;
                    sgn1.Content = "-";
                    num1 = rnd.Next(50);
                    num2 = rnd.Next(20);
                    Num1.Content = num1;
                    Num2.Content = num2;
                    break;
                case 4:
                    sign = Signs.divide;
                    sgn1.Content = ":";
                    bool torch = true;
                    while (torch)
                    {
                        num1 = rnd.Next(1, 10) * 10;
                        num2 = rnd.Next(10);
                        if (num1%num2 ==0 && num1> num2 && num2!=0)
                        {
                            Num1.Content = num1;
                            Num2.Content = num2;
                            torch = false;
                        }
                    }
                    break;
                default:
                    sign = Signs.muliply;
                    sgn1.Content = "*";
                    num1 = rnd.Next(10);
                    num2 = rnd.Next(10);
                    Num1.Content = num1;
                    Num2.Content = num2;
                    break;
            }
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var p = new Process
            {
                StartInfo = new ProcessStartInfo("result.txt")
                {
                    UseShellExecute = true,
                }
            };
            p.Start();
        }
        private void TimerButton(object sender, RoutedEventArgs e)
        {
            if (timer1.IsEnabled)
            {
                timerlbl.Foreground = new SolidColorBrush(Colors.DarkRed);
                timer1.Stop();
                TimerEableButton.Background = new SolidColorBrush(Colors.DarkRed);
                TimerEableButton.Foreground = new SolidColorBrush(Colors.White);

                timer1.IsEnabled = false;
            }
            else
            {
                timerlbl.Foreground = new SolidColorBrush(Colors.White);
                timer1.Start();
                TimerEableButton.Background = new SolidColorBrush(Colors.Teal);
                TimerEableButton.Foreground = new SolidColorBrush(Colors.Black);

                timer1.IsEnabled= true;
            }
        }
        public bool IsGameEnough()
        {
            return totalcounter < 90;

        }
        private void ResetResult(object sender, RoutedEventArgs e)
        {
            totalcounter = 0;
            falseCounter = 0;
            trueCounter = 0;
            TrueCounter.Content = "";
            FalseCounter.Content = "";
            resultlabel.Text = "";
        }
        private void ClearClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите очистить файл", "Помощник", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                    using var temp = new StreamWriter("result.txt");
                    temp.WriteLine();
                    }
                    
                    break;
                case MessageBoxResult.No:
                    break;
            }
            

        }
        private void GenerateWords(object sender, RoutedEventArgs e)
        {
            if (iswordgamestarted)
            {
                StartWordGame();
                iswordgamestarted = false;
            }
        }
        private void StartWordGame()
        {
            timer2.Start();
            CheckButton.IsEnabled = false;

            string[] list = File.ReadAllLines("words.txt");

            foreach (var Stackitems in Grid.Children)
            {
                if (Stackitems is StackPanel panel)
                {
                    foreach (var item in panel.Children)
                    {
                        if (item is TextBox box)
                        {
                            box.Text = list[rnd.Next(0, 400)];
                            textBoxes.Add(box.Text);

                            box.TextWrapping = TextWrapping.Wrap;
                            box.TextAlignment = TextAlignment.Center;
                            box.VerticalContentAlignment = VerticalAlignment.Center;
                            box.FontSize = 16;
                            box.IsEnabled = false;
                        }
                    }
                }
            }
        }
        private void ResetTimerButton(object sender, RoutedEventArgs e)
        {
            SpanTime1 = new TimeSpan(0, 0, 0, 0);
            timerlbl.Text = "";
        }
        private void TabItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (timer1.IsEnabled)
            {
                if (IsGameEnough())
                {
                    if (e.Key == Key.Enter)
                    {
                        try
                        {
                            result = double.Parse(Result.Text);
                            isWork = true;
                            totalcounter++;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Ошибка ввода");
                            isWork = false;
                            Result.Text = "";

                        }
                        if (isWork)
                        {
                            task = new Mather(num1, num2, result);
                            if (task.GetResult(sign))
                            {
                                trueCounter++;
                                TrueCounter.Content = trueCounter;
                                Result.Text = "";
                                AddResult();
                                Domath();
                            }
                            else
                            {
                                falseCounter++;
                                FalseCounter.Content = falseCounter;
                                Result.Text = "";
                                AddResult();
                                Domath();

                            }
                        }
                    }
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show("90 заданий выполнено");
                    using var temp = new StreamWriter("result.txt", true);
                    temp.WriteLine(new string('—', 15)+ "Игра Числа" + new string('—', 15));
                    temp.WriteLine($"Дата выполнения — {DateTime.Now}");
                    temp.WriteLine(new string('—', 40));

                    temp.WriteLine("Ваш результат " + SpanTime1);
                    temp.WriteLine($"Верных — {trueCounter} \nНеверных {falseCounter}");
                }
            }
            else
            {
                MessageBox.Show("Таймер не запущен", "Помощник", MessageBoxButton.OK);
            }
        }
        private void CheckWords(object sender, RoutedEventArgs e)
        {
            foreach (var Stackitems in Grid.Children)
            {
                if (Stackitems is StackPanel panel)
                {
                    foreach (var item in panel.Children)
                    {
                        if (item is TextBox box)
                        {
                            answers.Add(box.Text);

                        }
                    }
                }
            }

            var rez = textBoxes.Except(answers).ToList();
            
            string tempstr = "";
            foreach (var mistakeword in rez)
            {
                tempstr += mistakeword + "\n";
            }
            MessageBox.Show(tempstr, $"Пропущенные слова — {rez.Count}", MessageBoxButton.OK);


            using var temp = new StreamWriter("result.txt", true);

            temp.WriteLine(new string('—', 15) + "Игра слова" + new string('—', 15));
            temp.WriteLine($"Дата выполнения — {DateTime.Now}");
            temp.WriteLine(new string('—', 40));

            temp.WriteLine($"Пропущенные слова — {rez.Count} ");
            temp.Write(tempstr);
        }
        private void OpenDic(object sender, RoutedEventArgs e)
        {
            var p = new Process
            {
                StartInfo = new ProcessStartInfo("words.txt")
                {
                    UseShellExecute = true,
                }
            };
            p.Start();
        }

        private void GenerateColorizedWords(object sender, RoutedEventArgs e)
        {
            foreach (var Stackitems in Words.Children)
            {
                if (Stackitems is StackPanel panel)
                {
                    foreach (var item in panel.Children)
                    {
                        if (item is Label label)
                        {
                            WordsGame gm = new();
                            label.Foreground = new SolidColorBrush(gm.Colorize());
                            label.HorizontalContentAlignment = HorizontalAlignment.Center;
                            label.VerticalContentAlignment = VerticalAlignment.Center;
                            label.FontSize = 15;
                            label.Content = gm.Wordize();
                        }
                    }
                }
            }
        }

        private void StartTimer4(object sender, RoutedEventArgs e)
        {
            if (timer4.IsEnabled)
            {
                timer4.Stop();
            }
            else
            {
                timer4.Start();
            }
        }
        private void timer4_Tick(object sender, EventArgs e)
        {
            SpanTime4 = SpanTime4.Add(timer4.Interval);

            Timer4lbl.Content = $"Время {SpanTime4.Minutes} : {SpanTime4.Seconds}";
        }
    }

}
