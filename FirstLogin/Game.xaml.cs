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
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using Microsoft.Win32;

namespace FirstLogin
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        private MediaPlayer mp1 = new MediaPlayer();
        Random rnd = new Random();

        DispatcherTimer tmMoveBtnShoot  = new DispatcherTimer();
        DispatcherTimer tmBulletMove    = new DispatcherTimer();
        DispatcherTimer tmLeftRight     = new DispatcherTimer();
        DispatcherTimer tmLeftRightMove = new DispatcherTimer();
        DispatcherTimer tmUpDown        = new DispatcherTimer();
        DispatcherTimer tmUpDownMove    = new DispatcherTimer();
        DispatcherTimer tmTime          = new DispatcherTimer();

        int iTime = 30;
        int iUserId = 0;
        int iFollowUpWisp = 0;
        int iFollowUpHeal = 0;
        int iBullet = 0;
        clsDb dbConn = new clsDb();

        Uri uri = new Uri(@"pack://siteoforigin:,,,/sound/roblox.mp3");

        public Game(int iUserID)
        {
            InitializeComponent();

            tmMoveBtnShoot.Interval = TimeSpan.FromMilliseconds(3);
            tmMoveBtnShoot.Tick += MoveButton;

            tmBulletMove.Interval = TimeSpan.FromMilliseconds(1);
            tmBulletMove.Tick += TmBulletMove_Tick;

            tmLeftRight.Interval = TimeSpan.FromMilliseconds(1000);
            tmLeftRight.Tick += TmLeftRight_Tick;

            tmLeftRightMove.Interval = TimeSpan.FromMilliseconds(2);
            tmLeftRightMove.Tick += TmLeftRightMove_Tick;

            tmUpDown.Interval = TimeSpan.FromMilliseconds(2000);
            tmUpDown.Tick += TmUpDown_Tick;

            tmUpDownMove.Interval = TimeSpan.FromMilliseconds(2);
            tmUpDownMove.Tick += TmUpDownMove_Tick;

            tmTime.Interval = TimeSpan.FromSeconds(1);
            tmTime.Tick += TmTime_Tick;

            iUserId = iUserID;
            lblTime.Content = iTime;
            GetUserInfo();
            GetHighscore();

            Uri uri = new Uri(@"pack://siteoforigin:,,,/sound/roblox.mp3");
            mp1.Open(uri);
            //mp1.MediaEnded += Mp1_MediaEnded;
            //mp1.Play();

        }

        private void Mp1_MediaEnded(object sender, EventArgs e)
        {

            //replace with hearthstone theme
            mp1.Position = TimeSpan.Zero;
            mp1.Play();
        }

        private void TmTime_Tick(object sender, EventArgs e)
        {       
            iTime--;
            lblTime.Content = iTime;

            if (iTime == 0)
            {
                tmMoveBtnShoot.Stop();
                tmBulletMove.Stop();
                tmLeftRight.Stop();
                tmLeftRightMove.Stop();
                tmUpDown.Stop();
                tmUpDownMove.Stop();
                tmTime.Stop();

                int iScore = Convert.ToInt32(lblScore.Content);
                int iHighscore = Convert.ToInt32(lblHighscore.Content);

                if (iScore > iHighscore)
                {
                    if (MessageBox.Show("Gefeliciteerd! Je hebt een nieuwe highscore van: " + lblScore.Content + ". Wil je nog een keer?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        dbConn.AddHighscore(iScore, iUserId);
                        Close();
                    }
                    else
                    {
                        dbConn.AddHighscore(iScore, iUserId);
                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    if (MessageBox.Show("Je Score is: " + lblScore.Content + ". Wil je nog een keer?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        Close();
                    }
                    else
                    {
                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                }
                
            }
        }

        private void TmUpDownMove_Tick(object sender, EventArgs e)
        {
            foreach (Image item in cvGame.Children.OfType<Image>())
            {
                if (item.Name.Contains("imgUpDown"))
                {

                    double dImgUp = item.Margin.Top;
                    dImgUp += 15;
                    item.Margin = new Thickness(item.Margin.Left, dImgUp, 0, 0);

                    if (item.Margin.Top >= cvGame.ActualHeight)
                    {
                        cvGame.Children.Remove(item);
                        RemoveScore(5);
                        return;
                    }
                }
            }
        }

        private void TmUpDown_Tick(object sender, EventArgs e)
        {
            double xLength = cvGame.ActualWidth - 100;
            int X = Convert.ToInt32(xLength);

            double xPos = rnd.Next(0, X);

            iFollowUpHeal++;
            Image img = new Image();
            img.Name = "imgUpDown" + iFollowUpHeal;
            img.Margin = new Thickness(xPos, 0, 0, 0);
            img.Width = 150;
            img.Height = 250;
            img.Source = new BitmapImage(new Uri("pack://application:,,,/img/LesserHeal.png"));
            cvGame.Children.Add(img);
        }

        private void TmLeftRightMove_Tick(object sender, EventArgs e)
        {
            foreach (Image item in cvGame.Children.OfType<Image>())
            {
                if (item.Name.Contains("imgLeftRight"))
                {
                    double dImagLeft = item.Margin.Left;
                    dImagLeft += 15;
                    item.Margin = new Thickness(dImagLeft, 0, 0, 0);

                    if (item.Margin.Left >= cvGame.ActualWidth)
                    {
                        cvGame.Children.Remove(item);
                        RemoveScore(2);
                        return;
                    }
                }
            }
        }

        private void TmLeftRight_Tick(object sender, EventArgs e)
        {
            iFollowUpWisp++;
            Image img = new Image();
            img.Name = "imgLeftRight" + iFollowUpWisp;
            img.Margin = new Thickness(0, 0, 0, 0);
            img.Width = 150;
            img.Height = 250;
            img.Source = new BitmapImage(new Uri("pack://application:,,,/img/Wisp.png"));
            cvGame.Children.Add(img);
        }

        private void TmBulletMove_Tick(object sender, EventArgs e)
        {
            foreach (Button btnBullet in cvGame.Children.OfType<Button>())
            {
                if (btnBullet.Tag != null && btnBullet.Tag.ToString() == "btnBullet")
                {
                    double dImgTop = btnBullet.Margin.Top;
                    dImgTop -= 30;
                    btnBullet.Margin = new Thickness(btnBullet.Margin.Left, dImgTop, 0, 0);
                }
                if (btnBullet.Margin.Top <= 0)
                {
                    cvGame.Children.Remove(btnBullet);
                    iBullet--;
                    return;
                }

                Point relLocBullet = btnBullet.TranslatePoint(new Point(0, 0), cvGame);
                double btnBulletX = relLocBullet.X;
                double btnBulletY = relLocBullet.Y;
                Rect rectBullet = new Rect(btnBulletX, btnBulletY, btnBullet.ActualWidth, btnBullet.ActualHeight);

                foreach (Image imgGame in cvGame.Children.OfType<Image>())
                {
                    if (imgGame.Name.Contains("imgLeftRight"))
                    {

                        Point relLocLeftRight = imgGame.TranslatePoint(new Point(0, 0), cvGame);
                        double imgLeftRightX = relLocLeftRight.X;
                        double imgLeftRightY = relLocLeftRight.Y;
                        Rect rectLeftRight = new Rect(imgLeftRightX, imgLeftRightY, imgGame.ActualWidth, imgGame.ActualHeight);

                        if (rectLeftRight.IntersectsWith(rectBullet))
                        {

                            cvGame.Children.Remove(imgGame);
                            cvGame.Children.Remove(btnBullet);
                            iBullet--;
                            AddScore(1);

                            
                            return;
                        }
                    }

                    if (imgGame.Name.Contains("imgUpDown"))
                    {
                        Point relLocUpDown = imgGame.TranslatePoint(new Point(0, 0), cvGame);
                        double imgUpDownX = relLocUpDown.X;
                        double imgUpDownY = relLocUpDown.Y;
                        Rect rectUpDown = new Rect(imgUpDownX, imgUpDownY, imgGame.ActualWidth, imgGame.ActualHeight);

                        Point relLocShoot = btnShoot.TranslatePoint(new Point(0, 0), cvGame);
                        double btnShootX = relLocShoot.X;
                        double btnShootY = relLocShoot.Y;
                        Rect rectShoot = new Rect(btnShootX, btnShootY, btnShoot.ActualWidth, btnShoot.ActualHeight);

                        if (rectUpDown.IntersectsWith(rectShoot))
                        {
                            

                            cvGame.Children.Remove(imgGame);
                            AddScore(10);
                            return;
                        }

                        if (rectUpDown.IntersectsWith(rectBullet))
                        {
                            //replace with hearthstone death sound!!!!
                            mp1.Open(uri);
                            mp1.Play();

                            cvGame.Children.Remove(imgGame);
                            cvGame.Children.Remove(btnBullet);
                            iBullet--;
                            RemoveScore(5);
                            return;
                        }
                                                                       
                    }
                }
            }
        }

        private void GetUserInfo()
        {
            string sUsername = dbConn.GetUsername(iUserId);
            lblUsername.Content = "Hi, " + sUsername;
        }

        private void GetHighscore()
        {
            string sHighscore = dbConn.GetHighscore(iUserId);
            lblHighscore.Content = sHighscore;
        }

        private void MoveButton(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A)) 
            {
                if (btnShoot.Margin.Left > 0)
                {
                    btnShoot.Margin = new Thickness(btnShoot.Margin.Left - 15, btnShoot.Margin.Top, 0, 0);
                }
            }
            if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
            {
                if (btnShoot.Margin.Left < cvGame.ActualWidth - btnShoot.ActualWidth)
                {
                    btnShoot.Margin = new Thickness(btnShoot.Margin.Left + 15, btnShoot.Margin.Top, 0, 0);
                }
            }
            if (Keyboard.IsKeyDown(Key.Space))
            {
                FireBullet();
            }
        }

        private void FireBullet()
        {
            if (iBullet < 1)
            {
                //Uri uri = new Uri(@"pack://siteoforigin:,,,/sound/roblox.mp3");
                //mp1.Open(uri);
                //mp1.Play();

                Point relativeLocation = btnShoot.TranslatePoint(new Point(0, 0), cvGame);
                double bulletX = (relativeLocation.X + (btnShoot.ActualWidth / 2));
                double bulletY = relativeLocation.Y;

                Button btnBullet = new Button();
                btnBullet.Tag = "btnBullet";
                btnBullet.Width = 30;
                btnBullet.Height = 90;
                btnBullet.Background = Brushes.Red;
                btnBullet.Margin = new Thickness(bulletX, bulletY, 0, 0);
                cvGame.Children.Add(btnBullet);

                iBullet++;
            }
            
        }

        private void AddScore(int iPoints)
        {
            int iScore = int.Parse(lblScore.Content.ToString());

            iScore += iPoints;
            lblScore.Content = iScore.ToString();
        }

        private void RemoveScore(int iPoints)
        {
            int iScore = int.Parse(lblScore.Content.ToString());

            iScore -= iPoints;
            lblScore.Content = iScore.ToString();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            tmMoveBtnShoot.Start();
            tmBulletMove.Start();
            tmLeftRight.Start();
            tmLeftRightMove.Start();
            tmUpDown.Start();
            tmUpDownMove.Start();
            tmTime.Start();
            btnStart.IsEnabled = false;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            tmMoveBtnShoot.Stop();
            tmBulletMove.Stop();
            tmLeftRight.Stop();
            tmLeftRightMove.Stop();
            tmUpDown.Stop();
            tmUpDownMove.Stop();
            tmTime.Stop();

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
