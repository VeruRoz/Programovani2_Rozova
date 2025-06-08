using System.Diagnostics;
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

namespace WPF_Hungry_Horace;
enum GameStage { Intro, Game, OutroWin, OutroLosse }
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private GameStage Stage;
    private Level LevelControl;
    public MainWindow()
    {
        InitializeComponent();
        Stage = GameStage.Intro;

        LevelControl = new Level();
        LevelControl.SkoreChanged += LevelControl_SkoreChanged;

        Grid.SetRow(LevelControl, 1);
        Grid.SetColumn(LevelControl, 1);
        GameLevel_1.Children.Add(LevelControl);
    }
    private void LevelControl_SkoreChanged(object sender, int noveSkore)
    {
        if (Stage == GameStage.Game)
        {
            if (noveSkore == 6)
            {
                Stage = GameStage.OutroWin;
                TogglePanels();
            }
            else if (noveSkore < 0)
            {
                Stage = GameStage.OutroLosse;
                TogglePanels();
            }
        }

        Skore.Text = noveSkore.ToString();

    }

    private void StartBTN_Click(object sender, RoutedEventArgs e)
    {
        Stage = GameStage.Game;
        TogglePanels();
    }

    private void TogglePanels()
    {
        Start.Visibility = Visibility.Hidden;
        GameLevel_1.Visibility = Visibility.Hidden;
        Win.Visibility = Visibility.Hidden;
        lost.Visibility = Visibility.Hidden;

        switch (Stage)
        {
            case GameStage.Intro:
                Start.Visibility = Visibility.Visible;
                break;

            case GameStage.Game:
                GameLevel_1.Visibility = Visibility.Visible;
                LevelControl.Focusable = true;
                LevelControl.Focus();
                break;

            case GameStage.OutroWin:
                Win.Visibility = Visibility.Visible;
                break;

            case GameStage.OutroLosse:
                lost.Visibility = Visibility.Visible;
                break;
        }
    }
}

    
