using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;

namespace Glass.UserControls
{
    public class MapContextMenu:ContextMenu
    {
        //RichTextBox rtb;
        //string placeName = string.Empty;

        public MapContextMenu()
        {
           //this.rtb = rtb;
            //this.placeName = name;
        }

        //构造菜单按钮并返回一个FrameworkElement对象
        protected override FrameworkElement GetContent()
        {
            Border border = new Border() { BorderBrush = new SolidColorBrush(Color.FromArgb(255, 167, 171, 176)), BorderThickness = new Thickness(1), Background = new SolidColorBrush(Colors.White) };
            border.Effect = new DropShadowEffect() { BlurRadius = 3, Color = Color.FromArgb(255, 230, 227, 236) };
            Grid grid = new Grid() { Margin = new Thickness(1) };
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(105) });
            grid.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromArgb(255, 233, 238, 238)) });
            grid.Children.Add(new Rectangle() { Fill = new SolidColorBrush(Color.FromArgb(255, 226, 228, 231)), HorizontalAlignment = HorizontalAlignment.Right, Width = 1 });


            Button tjspButton = new Button() { Height = 22, Margin = new Thickness(0, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Top, HorizontalContentAlignment = HorizontalAlignment.Left };
            tjspButton.Style = Application.Current.Resources["ContextMenuButton"] as Style;
            Grid.SetColumnSpan(tjspButton, 2);
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock tjspText = new TextBlock() { Text = "菜单项1", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(16, 0, 0, 0) };
            sp.Children.Add(tjspText);
            tjspButton.Content = sp;
            grid.Children.Add(tjspButton);


            Button zwxpButton = new Button() { Height = 22, Margin = new Thickness(0, 24, 0, 0), HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Top, HorizontalContentAlignment = HorizontalAlignment.Left };
   
            Grid.SetColumnSpan(zwxpButton, 2);
            sp = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock zwxpText = new TextBlock() { Text = "菜单项2", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(16, 0, 0, 0) };
            sp.Children.Add(zwxpText);
            zwxpButton.Content = sp;
            grid.Children.Add(zwxpButton);


            Button zjhyButton = new Button() { Height = 22, Margin = new Thickness(0, 48, 0, 0), HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Top, HorizontalContentAlignment = HorizontalAlignment.Left };
            zjhyButton.Style = Application.Current.Resources["ContextMenuButton"] as Style;
            Grid.SetColumnSpan(zjhyButton, 2);
            sp = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock zjhyText = new TextBlock() { Text = "菜单项3", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(16, 0, 0, 0) };
            sp.Children.Add(zjhyText);
            zjhyButton.Content = sp;
            grid.Children.Add(zjhyButton);

            border.Child = grid;
            return border;
        }

    }
}
