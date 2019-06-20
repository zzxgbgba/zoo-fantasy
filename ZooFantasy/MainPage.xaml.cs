using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZooFantasy.Pages;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ZooFantasy
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string BackGroundImagePath;
        public static MainPage Current;
        public MainPage()
        {
            ApplicationView.PreferredLaunchViewSize = new Size(1366, 768); //起始分辨率1366*768
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize; //加载起始分辨率
            RandomBackGround();
            this.InitializeComponent();
            SecondFrame.Navigate(typeof(MainMenuPage));
            Current = this;
        }

        private void RandomBackGround()
        {
            Random r = new Random();
            int tmp = r.Next(1, 8);
            BackGroundImagePath = "Resource/BackGround/MenuBackGround (" + tmp.ToString() + ").jpg";
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuSplitView.IsPaneOpen = !MainMenuSplitView.IsPaneOpen;
        }

        private void SecondFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if(SecondFrame.CanGoBack)
            {
                BackButton.Visibility = Visibility.Visible;
            }
            else
            {
                BackButton.Visibility = Visibility.Collapsed;
            }
            SettingsBox.IsSelected = false;
            UserBox.IsSelected = false;
            if (SecondFrame.SourcePageType == typeof(SettingsPage))
            {
                SettingsBox.IsSelected = true;
            }
            if (SecondFrame.SourcePageType == typeof(UserPage))
            {
                UserBox.IsSelected = true;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.GoBack();

        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsBox.IsSelected)
            {
                SecondFrame.Navigate(typeof(SettingsPage));
                //SettingsBox.IsSelected = !SettingsBox.IsSelected;
            }
            if (UserBox.IsSelected)
            {
                SecondFrame.Navigate(typeof(UserPage));
                //UserBox.IsSelected = !UserBox.IsSelected;
            }
        }
    }
}
