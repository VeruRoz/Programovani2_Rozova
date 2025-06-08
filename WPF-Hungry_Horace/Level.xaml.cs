using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace WPF_Hungry_Horace
{
    /// <summary>
    /// Interaction logic for Level.xaml
    /// </summary>
    /// 


    public partial class Level : UserControl
    {
        private int horaceRow = 8;
        private int horaceCol = 1;

        private int _skore;
        public int Skore
        {
            get => _skore;
            set
            {
                _skore = value;
                SkoreChanged?.Invoke(this, _skore);
            }
        }

        public event EventHandler<int> SkoreChanged;

        private DispatcherTimer priserkaTimer;



        public Level()
        {
            InitializeComponent();
            this.Loaded += Level_Loaded;
            Skore = 0;

            priserkaTimer = new DispatcherTimer();
            priserkaTimer.Interval = TimeSpan.FromMilliseconds(500);
            priserkaTimer.Tick += (s, e) => Monster_Move();
            priserkaTimer.Start();
        }

        private void Level_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focusable = true;
            this.Focus();
            this.KeyDown += Level_KeyDown;
        }

        private Random rng = new Random();

        private void Monster_Move()
        {
            int radek = Grid.GetRow(priserka);
            int sloupec = Grid.GetColumn(priserka);

            var smery = new List<(int dRow, int dCol)>
            {
        (-1,0), (1,0), (0,-1), (0,1)
             };


            smery = smery.OrderBy(x => rng.Next()).ToList();

            foreach (var smer in smery)
            {
                int newRow = radek + smer.dRow;
                int newCol = sloupec + smer.dCol;

                bool Wall = IsThereWall(newRow, newCol);

                int HoraceRow = Grid.GetRow(Horace);
                int HoraceCol = Grid.GetColumn(Horace);
                if (newRow == HoraceRow && newCol == HoraceCol)
                {
                    Horace.Visibility = Visibility.Hidden;
                    Skore = -1;

                }

                if (!Wall)
                {
                    Grid.SetRow(priserka, newRow);
                    Grid.SetColumn(priserka, newCol);
                    break;
                }
                
            }
        }


        private void Level_KeyDown(object sender, KeyEventArgs e)
        {
            int newRow = horaceRow;
            int newCol = horaceCol;



            switch (e.Key)
            {
                case Key.Up:
                    newRow--;
                    break;
                case Key.Down:
                    newRow++;
                    break;
                case Key.Left:
                    newCol--;
                    break;
                case Key.Right:
                    newCol++;
                    break;
            }



            //okraje + vnitřní stěny
            if (newRow >= 1 && newRow < 9 && newCol >= 1 && newCol < 9)
            {
                foreach (var shape in GameBoard.Children.OfType<Shape>())
                {
                    if ((string)shape.Tag == "wall")
                    {
                        int row = Grid.GetRow(shape);
                        int col = Grid.GetColumn(shape);

                        if (row == newRow && col == newCol)
                        {
                            newCol = horaceCol;
                            newRow = horaceRow;
                        }
                    }


                } 
                //je na příšrce?
                int priserkaRow = Grid.GetRow(priserka);
                int priserkaCol = Grid.GetColumn(priserka);
                if (horaceRow == priserkaRow && horaceCol == priserkaCol)
                {
                    Horace.Visibility = Visibility.Collapsed;
                    Skore = -1;

                }

                horaceRow = newRow;
                horaceCol = newCol;

                Grid.SetRow(Horace, horaceRow);
                Grid.SetColumn(Horace, horaceCol);
            }
            foreach (var shape in GameBoard.Children.OfType<Shape>())
            {

                if ((string)shape.Tag == "food")
                {
                    //když jsou na stejných souzadnicích tak jidlo zmizí a navýší skore

                    int row = Grid.GetRow(shape);
                    int col = Grid.GetColumn(shape);

                    if (row == horaceRow && col == horaceCol && shape.Visibility == Visibility.Visible && Horace.Visibility == Visibility.Visible)
                    {
                        shape.Visibility = Visibility.Hidden;
                        Skore++;
                    }
                }
            }


        }


        private bool IsThereWall(int newRow, int newCol)
        {
            // Mimo herní plochu se chová jako zeď
            if (newRow < 1 || newRow > 8 || newCol < 1 || newCol > 8)
                return true;

            foreach (var shape in GameBoard.Children.OfType<Shape>())
            {
                if (shape.Tag is string tag && tag == "wall")
                {
                    int row = Grid.GetRow(shape);
                    int col = Grid.GetColumn(shape);

                    if (row == newRow && col == newCol)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
