using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Glass.UserControls
{
    public partial class SearchForm : ChildWindow
    {
        public SearchForm()
        {
            InitializeComponent();
            cbLayer.Items.Add("井");
            cbLayer.Items.Add("信息点");
            cbLayer.SelectedIndex = 0;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(cbLayer.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择图层！");
                cbLayer.Focus();
            }

            if (String.IsNullOrEmpty(tbWords.Text.Trim()))
            {
                MessageBox.Show("请输入关键词！");
                tbWords.Focus();
            }
            
            MainPage.bSearch = true;
            MainPage.SearchLayer = cbLayer.SelectedItem.ToString();
            MainPage.SearchWords = tbWords.Text;
            ;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.bSearch = false;
            MainPage.SearchLayer = String.Empty;
            MainPage.SearchWords = String.Empty;
            this.DialogResult = false;
        }
    }
}

