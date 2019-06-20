using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZooFantasy.Stage;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZooFantasy.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            this.InitializeComponent();
        }

        private async void StoryModeButton_Click(object sender, RoutedEventArgs e)
        {
            await StageManager.CreatCacheMode("StoryMode");
            this.Frame.Navigate(typeof(StoryModeStageChoosePage));
        }

        private async void ChallengeModeButton_Click(object sender, RoutedEventArgs e)
        {
            await StageManager.CreatCacheMode("ChallengeMode");
            this.Frame.Navigate(typeof(ChallengeModeStageChoosePage));
        }

        private void CardsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CardsPage));
        }
    }
}
