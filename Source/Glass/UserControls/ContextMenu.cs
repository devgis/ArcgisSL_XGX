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
using System.Windows.Controls.Primitives;

namespace Glass.UserControls
{
    public abstract class ContextMenu
    {
        private Point _location;
        private bool _isShowing;
        private Popup _popup;
        private Grid _grid;
        private Canvas _canvas;
        private FrameworkElement _content;

        //初始化并显示弹出窗体.公共方法在显示菜单项时调用
        public void Show(Point location)
        {
            if (_isShowing)
                throw new InvalidOperationException();
            _isShowing = true;
            _location = location;
            ConstructPopup();
            _popup.IsOpen = true;
        }

        //关闭弹出窗体
        public void Close()
        {
            _isShowing = false;
            if (_popup != null)
            {
                _popup.IsOpen = false;
            }
        }

        //abstract function that the child class needs to implement to return the framework element that needs to be displayed in the popup window.
        protected abstract FrameworkElement GetContent();

        //Default behavior for OnClickOutside() is to close the context menu when there is a mouse click event outside the context menu
        protected virtual void OnClickOutside()
        {
            Close();
        }

        // 用Grid来布局，初始化弹出窗体
        //在Grid里面添加一个Canvas，用来监测菜单项外面的鼠标点击事件
        //
        // Add the Framework Element returned by GetContent() to the grid and position it at _location
        private void ConstructPopup()
        {
            if (_popup != null)
                return;
            _popup = new Popup();
            _grid = new Grid();
            _popup.Child = _grid;
            _canvas = new Canvas();
            _canvas.MouseLeftButtonDown += (sender, args) => { OnClickOutside(); };
            _canvas.MouseRightButtonDown += (sender, args) => { args.Handled = true; OnClickOutside(); };
            _canvas.Background = new SolidColorBrush(Colors.Transparent);
            _grid.Children.Add(_canvas);
            _content = GetContent();
            _content.HorizontalAlignment = HorizontalAlignment.Left;
            _content.VerticalAlignment = VerticalAlignment.Top;
            _content.Margin = new Thickness(_location.X, _location.Y, 0, 0);
            _grid.Children.Add(_content);
            UpdateSize();
        }

        /// <summary>
        /// 更新大小
        /// </summary>
        private void UpdateSize()
        {
            _grid.Width = Application.Current.Host.Content.ActualWidth;
            _grid.Height = Application.Current.Host.Content.ActualHeight;
            if (_canvas != null)
            {
                _canvas.Width = _grid.Width;
                _canvas.Height = _grid.Height;
            }
        }
    }
}
