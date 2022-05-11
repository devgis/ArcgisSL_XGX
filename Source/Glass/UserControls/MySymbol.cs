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
using ESRI.ArcGIS.Client.Symbols;
using System.Windows.Resources;
using System.IO;
using System.Windows.Markup;

namespace Glass.UserControls
{
    public class MySymbol: MarkerSymbol
    {
        public MySymbol(string Name)
            : base()
        {
            //try
            //{
                //StreamResourceInfo info = App.GetResourceStream(new Uri("Glass.UserControls.ControlTemplate.xaml", UriKind.RelativeOrAbsolute));
                //StreamReader sr = new StreamReader(info.Stream);                
                //string template = sr.ReadToEnd();
                //template = template.Replace("mynumb", Name);
                //this.ControlTemplate = (ControlTemplate)XamlReader.Load(template);

            String Key = "Glass.UserControls.ControlTemplate.xaml";
            Stream stream = typeof(MySymbol).Assembly.GetManifestResourceStream(Key);
            string template = new StreamReader(stream).ReadToEnd();
            //template = template.Replace("force1.png", "force" + sType.ToString() + ".png");
            //template = template.Replace("comp1.png", "comp" + sComp.ToString() + ".png");
            template = template.Replace("mynumb", Name);
            this.ControlTemplate = (ControlTemplate)XamlReader.Load(template);

            //}
            //catch (Exception ex)
            //{
            //}
        }
    }
}
