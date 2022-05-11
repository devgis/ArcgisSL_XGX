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
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.SilverlightMapApp;
using ESRI.ArcGIS.Client.Tasks;
using System.Windows.Data;
using Glass.DBWS;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using Glass.UserControls;

namespace Glass
{
    public partial class MainPage : UserControl
    {
        bool usePlaneProjection = false;
        double marginLRFactor = 0.25;
        double marginTBFactor = 0.5;

        #region 文字标注
        Brush b = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
         
        #endregion

        public MainPage()
        {
            InitializeComponent();

            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
            };

            #region 地图初始化参数
            
            #endregion

            string strvalue = Application.Current.Resources["UsePlaneProjection"] as string;
            if (strvalue != null)
                usePlaneProjection = bool.Parse(strvalue);
            if (usePlaneProjection)
            {
                strvalue = Application.Current.Resources["MapLeftRightMarginFactor"] as string;
                if (strvalue != null)
                    marginLRFactor = double.Parse(strvalue);
                strvalue = Application.Current.Resources["MapTopBottomMarginFactor"] as string;
                if (strvalue != null)
                    marginTBFactor = double.Parse(strvalue);
            }

            _drawSurface = new Draw(Map)
            {
                LineSymbol = DefaultLineSymbol,
                FillSymbol = DefaultFillSymbol
            };
            _drawSurface.DrawComplete += MyDrawSurface_DrawComplete;

            _queryTask = new QueryTask("PointGraphicsLayer");//http://serverapps.esri.com/SDS/databases/Demo/dbo.USStates_Geographic
            _queryTask.ExecuteCompleted += QueryTask_ExecuteCompleted;
            _queryTask.Failed += QueryTask_Failed;

            #region 异步加载数据
            DBWS.DBServiceSoapClient dbsClient = new DBWS.DBServiceSoapClient();
            dbsClient.GetPointsCompleted += new EventHandler<DBWS.GetPointsCompletedEventArgs>(dbsClient_GetPointsCompleted);
            dbsClient.GetPointsAsync();

            dbsClient.GetResourcesCompleted += new EventHandler<DBWS.GetResourcesCompletedEventArgs>(dbsClient_GetResourcesCompleted);
            dbsClient.GetResourcesAsync();
            #endregion

            
        }

        #region 异步加载点和资源绘制到地图
        private static ESRI.ArcGIS.Client.Projection.WebMercator mercator =
            new ESRI.ArcGIS.Client.Projection.WebMercator();

        void dbsClient_GetResourcesCompleted(object sender, DBWS.GetResourcesCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<GResources> listGResources = e.Result as List<GResources>;
                //MessageBox.Show("Resources数量:"+listGResources.Count.ToString());
                GraphicsLayer graphicsLayer = Map.Layers["ResourcesGraphicsLayer"] as GraphicsLayer;
                GraphicsLayer labelLayer = Map.Layers["LabelGraphicsLayer"] as GraphicsLayer;

                for (int i = 0; i < listGResources.Count; i++)
                {
                    Graphic graphic = new Graphic()
                    {
                        Geometry = mercator.FromGeographic(new MapPoint((double)listGResources[i].X, (double)listGResources[i].Y)),
                        Symbol = LayoutRoot.Resources["Symbol_Resources"] as Symbol
                    };
                    graphic.Attributes["ID"] = listGResources[i].ID;
                    graphic.Attributes["NAME"] = listGResources[i].NAME;
                    graphic.Attributes["TYPE"] = listGResources[i].TYPE;
                    graphic.Attributes["X"] = listGResources[i].X;
                    graphic.Attributes["Y"] = listGResources[i].Y;
                    graphic.MouseRightButtonDown += new MouseButtonEventHandler(graphic_MouseRightButtonDown);
                    graphicsLayer.Graphics.Add(graphic);
                    //graphic.MouseEnter += new MouseEventHandler(graphic_MouseEnter2);
                    //graphic.MouseLeave += new MouseEventHandler(graphic_MouseLeave2);
                    //ContextMenuService.SetContextMenu(graphic, cm);  

                    TextSymbol txtSymbol = new TextSymbol()
                    {
                        Text = listGResources[i].NAME,
                        FontFamily = new System.Windows.Media.FontFamily("Arial"),
                        Foreground = new System.Windows.Media.SolidColorBrush(Colors.Purple),
                        OffsetY = 5,
                        OffsetX = -10,
                        FontSize = 12
                    }; 
                    Graphic labelGraphic = new Graphic()
                    {
                        Geometry = mercator.FromGeographic(new MapPoint((double)listGResources[i].X, (double)listGResources[i].Y)),
                        Symbol = txtSymbol
                    };
                    labelLayer.Graphics.Add(labelGraphic);

                    //SpatialReference sr = new SpatialReference();
                    
                }
            }
            else
            {
                System.Windows.MessageBox.Show("发生错误" + e.Error.Message.ToString());
            }
        }

        void dbsClient_GetPointsCompleted(object sender, DBWS.GetPointsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<GPoint> listGPoint = e.Result as List<GPoint>;
                //MessageBox.Show("Point数量:" + listGPoint.Count.ToString());
                GraphicsLayer graphicsLayer = Map.Layers["PointGraphicsLayer"] as GraphicsLayer;
                GraphicsLayer labelLayer = Map.Layers["LabelGraphicsLayer"] as GraphicsLayer;

                for (int i = 0; i < listGPoint.Count; i++)
                {
                    Graphic graphic = new Graphic()
                    {
                        Geometry = mercator.FromGeographic(new MapPoint((double)listGPoint[i].X, (double)listGPoint[i].Y)),
                        Symbol = new MySymbol(listGPoint[i].ID) //LayoutRoot.Resources["Symbol_Point"] as Symbol
                    };
                    graphic.Attributes["ID"] = listGPoint[i].ID;
                    graphic.Attributes["PDATE"] = listGPoint[i].PDATE;
                    graphic.Attributes["UNIT"] = listGPoint[i].UNIT;
                    graphic.Attributes["VALUE1"] = listGPoint[i].VALUE1;
                    graphic.Attributes["VALUE2"] = listGPoint[i].VALUE2;
                    graphic.Attributes["VALUE3"] = listGPoint[i].VALUE3;
                    graphic.Attributes["ZONE"] = listGPoint[i].ZONE;
                    graphic.Attributes["X"] = listGPoint[i].X;
                    graphic.Attributes["Y"] = listGPoint[i].Y;
                    graphic.MouseRightButtonDown += new MouseButtonEventHandler(graphic_MouseRightButtonDown);
                    //graphic.MouseEnter += new MouseEventHandler(graphic_MouseEnter);
                    //graphic.MouseLeave += new MouseEventHandler(graphic_MouseLeave);
                    graphicsLayer.Graphics.Add(graphic);
                    //ContextMenuService.SetContextMenu(graphic, cm);  

                    ////Brush b = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
                    //TextSymbol txtSymbol = new TextSymbol()
                    //{
                    //    Text = listGPoint[i].ID,
                    //    FontFamily = new System.Windows.Media.FontFamily("Arial"),
                    //    Foreground = new System.Windows.Media.SolidColorBrush(Colors.Purple),
                    //    OffsetY = 5,
                    //    OffsetX = -10,
                    //    FontSize = 12
                    //};

                    //Graphic labelGraphic = new Graphic()
                    //{
                    //    Geometry = mercator.FromGeographic(new MapPoint((double)listGPoint[i].X, (double)listGPoint[i].Y)),
                    //    Symbol = txtSymbol
                    //};
                    //labelLayer.Graphics.Add(labelGraphic);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("发生错误" + e.Error.Message.ToString());
            }
        }

        void graphic_MouseLeave(object sender, MouseEventArgs e)
        {
            //(sender as Graphic).Symbol = LayoutRoot.Resources["Symbol_Point"] as Symbol;
            Graphic g= sender as Graphic;
            (sender as Graphic).Symbol = new MySymbol(g.Attributes["ID"].ToString());
        }

        void graphic_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Graphic).Symbol = LayoutRoot.Resources["Symbol_Point_Select"] as Symbol;
            //(sender as Graphic).Symbol = new MySymbol("新样式");
            
        }

        void graphic_MouseLeave2(object sender, MouseEventArgs e)
        {
            (sender as Graphic).Symbol = LayoutRoot.Resources["Symbol_Resources"] as Symbol;
            
        }

        void graphic_MouseEnter2(object sender, MouseEventArgs e)
        {
            //(sender as Graphic).Symbol = LayoutRoot.Resources["Symbol_Point_Select"] as Symbol;
            (sender as Graphic).Symbol = new MySymbol("新样式");
        }
        #endregion

        private void nav_Loaded(object sender, RoutedEventArgs e)
        {
            nav.MapProjection = mapPlaneProjection;
            //nav.OverviewMap2 = OverView;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (usePlaneProjection)
            {
                double mwidth = MapBorder.ActualWidth * marginLRFactor * -1;
                double mheight = MapBorder.ActualHeight * marginTBFactor * -1;
                Map.Margin = new Thickness(mwidth, mheight, mwidth, mheight);
            }
        }
        #region 查询相关
        private Draw _drawSurface;
        QueryTask _queryTask;
        
        public static bool bSearch = false;
        public static String SearchLayer = "";
        public static String SearchWords = "";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                switch ((sender as Button).Name)
                {
                    case "BtnMuiltSelect":
                        _drawSurface.DrawMode = DrawMode.Polygon;
                        break;
                    case "BtnSearch":
                        SearchForm frmSearch = new SearchForm();
                        frmSearch.Closed += new EventHandler(frmSearch_Closed);
                        frmSearch.Show();
                        
                        break;
                }
            }
            _drawSurface.IsEnabled = (_drawSurface.DrawMode != DrawMode.None);
        }

        void frmSearch_Closed(object sender, EventArgs e)
        {
            if (bSearch)
            {
                double MinX = 0;
                double MaxX = 0;
                double MinY = 0;
                double MaxY = 0;
                Graphic gTemp = new Graphic();
                int iCount = 0;
                switch (SearchLayer)
                {
                    case "井":
                        GraphicsLayer gResourcesLayer0 = Map.Layers["ResourcesGraphicsLayer"] as GraphicsLayer;
                        foreach (Graphic graphic in gResourcesLayer0.Graphics)
                        {
                            graphic.Symbol = LayoutRoot.Resources["Symbol_Resources"] as Symbol;
                        }

                        GraphicsLayer graphicsLayer = Map.Layers["PointGraphicsLayer"] as GraphicsLayer;
                        foreach (Graphic graphic in graphicsLayer.Graphics)
                        {
                            if (graphic.Attributes["ID"].ToString().Contains(SearchWords)
                                || graphic.Attributes["UNIT"].ToString().Contains(SearchWords)
                                || graphic.Attributes["ZONE"].ToString().Contains(SearchWords))
                            {
                                
                                graphic.Symbol = LayoutRoot.Resources["Symbol_Point_Select"] as Symbol;
                                gTemp = graphic;
                                double TempX = 0;
                                double TempY = 0;
                                try
                                {
                                    TempX = Convert.ToDouble(graphic.Attributes["X"]);
                                }
                                catch
                                { }
                                try
                                {
                                    TempY = Convert.ToDouble(graphic.Attributes["X"]);
                                }
                                catch
                                { }
                                if (TempX < MinX)
                                {
                                    MinX = TempX;
                                }
                                if (TempX > MaxX)
                                {
                                    MaxX = TempX;
                                }
                                if (TempY < MinY)
                                {
                                    MinY = TempY;
                                }
                                if (TempY > MaxY)
                                {
                                    MaxY = TempY;
                                }
                                iCount++;
                            }
                            else
                            {
                                graphic.Symbol = LayoutRoot.Resources["Symbol_Point"] as Symbol;
                            }
                        }
                        break;
                    case "信息点":
                        GraphicsLayer graphicsLayer0 = Map.Layers["PointGraphicsLayer"] as GraphicsLayer;
                        foreach (Graphic graphic in graphicsLayer0.Graphics)
                        {
                            graphic.Symbol = LayoutRoot.Resources["Symbol_Point"] as Symbol;
                        }

                        GraphicsLayer gResourcesLayer = Map.Layers["ResourcesGraphicsLayer"] as GraphicsLayer;
                        foreach (Graphic graphic in gResourcesLayer.Graphics)
                        {
                            if (graphic.Attributes["NAME"].ToString().Contains(SearchWords))
                            {
                                graphic.Symbol = LayoutRoot.Resources["Symbol_Point_Select"] as Symbol;
                                gTemp = graphic;
                                double TempX = 0;
                                double TempY = 0;
                                try
                                {
                                    TempX = Convert.ToDouble(graphic.Attributes["X"]);
                                }
                                catch
                                { }
                                try
                                {
                                    TempY = Convert.ToDouble(graphic.Attributes["X"]);
                                }
                                catch
                                { }
                                if (TempX < MinX)
                                {
                                    MinX = TempX;
                                }
                                if (TempX > MaxX)
                                {
                                    MaxX = TempX;
                                }
                                if (TempY < MinY)
                                {
                                    MinY = TempY;
                                }
                                if (TempY > MaxY)
                                {
                                    MaxY = TempY;
                                }
                                iCount++;
                            }
                            else
                            {
                                graphic.Symbol = LayoutRoot.Resources["Symbol_Resources"] as Symbol;
                            }
                        }
                        break;
                }
                
                MessageBox.Show("查询到 " + iCount.ToString() + " 个符合条件的" + SearchLayer+"!");
                try
                {
                    if (iCount == 0)
                    {
                        Map.ZoomTo(gTemp.Geometry);
                    }
                    else if (iCount >= 1)
                    {
                        Envelope locatorExtent = new Envelope(MinX - 0.001, MinY - 0.001, MaxX + 0.001, MaxY + 0.001);
                        Map.ZoomDuration = new TimeSpan(0, 0, 2);
                        this.Map.ZoomTo(locatorExtent);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("发生错误："+ex.Message);
                }
            }
        }

        void graphic_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MapContextMenu mcm = new MapContextMenu();
            mcm.Show(e.GetPosition(LayoutRoot));  


            //MessageBox.Show("邮件点我了！" + (sender as Graphic).Attributes["ID"].ToString());
            //MessageBox.Show("邮件点我了！" +e.OriginalSource.GetType().ToString());
            //MessageBox.Show("邮件点我了！" + (e.OriginalSource as Graphic).Attributes["ID"].ToString());
        }

        

        private void MyDrawSurface_DrawComplete(object sender, ESRI.ArcGIS.Client.DrawEventArgs args)
        {
            //List<MapPoint> lstMapPoint = (args.Geometry as ESRI.ArcGIS.Client.Geometry.Polygon).Rings[0].ToList<MapPoint>();
            List<Point> region = new List<Point>();
            foreach (MapPoint mp in (args.Geometry as ESRI.ArcGIS.Client.Geometry.Polygon).Rings[0])
            {
                Point p = new Point(mp.X, mp.Y);
                region.Add(p);
            }

            GraphicsLayer graphicsLayer = Map.Layers["PointGraphicsLayer"] as GraphicsLayer;
            Decimal value1 = 0;
            foreach (Graphic graphic in graphicsLayer.Graphics)
            {
                Point ptemp = new Point(Convert.ToDouble(graphic.Attributes["X"]), Convert.ToDouble(graphic.Attributes["Y"]));
                if (isInRegion(ptemp, region))
                {
                    value1 += Convert.ToDecimal(graphic.Attributes["VALUE1"]);
                    graphic.Symbol = LayoutRoot.Resources["Symbol_Point_Select"] as Symbol;

                }
                else
                {
                    graphic.Symbol = LayoutRoot.Resources["Symbol_Point"] as Symbol;
                }
            }
            if (value1>0m)
            {
                MessageBox.Show("选中VALUE1总值：" + value1.ToString());
            }

            //GraphicsLayer selectionGraphicslayer = Map.Layers["MyGraphicsLayer"] as GraphicsLayer;
            //selectionGraphicslayer.ClearGraphics();

            //// Bind data grid to query results
            //Binding resultFeaturesBinding = new Binding("LastResult.Features");
            //resultFeaturesBinding.Source = _queryTask;
            //QueryDetailsDataGrid.SetBinding(DataGrid.ItemsSourceProperty, resultFeaturesBinding);

            //Query query = new ESRI.ArcGIS.Client.Tasks.Query();
            //query.OutFields.AddRange(new string[] { "STATE_NAME", "POP2008", "SUB_REGION" });
            //query.OutSpatialReference = Map.SpatialReference;
            //query.Geometry = args.Geometry;
            //query.SpatialRelationship = SpatialRelationship.esriSpatialRelIntersects;
            //query.ReturnGeometry = true;
            //_queryTask.ExecuteAsync(query);

            _drawSurface.DrawMode = DrawMode.None;
            //_drawSurface.IsEnabled = false;
        }
        private void QueryTask_ExecuteCompleted(object sender, ESRI.ArcGIS.Client.Tasks.QueryEventArgs args)
        {
            FeatureSet featureSet = args.FeatureSet;

            if (featureSet == null || featureSet.Features.Count < 1)
            {
                MessageBox.Show("No features retured from query");
                return;
            }

            GraphicsLayer graphicsLayer = Map.Layers["MyGraphicsLayer"] as GraphicsLayer;

            if (featureSet != null && featureSet.Features.Count > 0)
            {
                foreach (Graphic feature in featureSet.Features)
                {
                    feature.Symbol = ResultsFillSymbol;
                    graphicsLayer.Graphics.Insert(0, feature);
                }
            }

            //ResultsDisplay.Visibility = Visibility.Visible;
            ResultsDisplay.IsExpanded = true;

            _drawSurface.IsEnabled = false;
        }

        private void QueryTask_Failed(object sender, TaskFailedEventArgs args)
        {
            MessageBox.Show("Query failed: " + args.Error);
        }


        private void QueryDetailsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Graphic g in e.AddedItems)
                g.Select();

            foreach (Graphic g in e.RemovedItems)
                g.UnSelect();
        }

        private void QueryDetailsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseEnter += Row_MouseEnter;
            e.Row.MouseLeave += Row_MouseLeave;
        }

        void Row_MouseEnter(object sender, MouseEventArgs e)
        {
            (((System.Windows.FrameworkElement)(sender)).DataContext as Graphic).Select();
        }

        void Row_MouseLeave(object sender, MouseEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            Graphic g = ((System.Windows.FrameworkElement)(sender)).DataContext as Graphic;

            if (!QueryDetailsDataGrid.SelectedItems.Contains(g))
                g.UnSelect();
        }

        private void GraphicsLayer_MouseEnter(object sender, GraphicMouseEventArgs args)
        {
            QueryDetailsDataGrid.Focus();
            QueryDetailsDataGrid.SelectedItem = args.Graphic;
            QueryDetailsDataGrid.CurrentColumn = QueryDetailsDataGrid.Columns[0];
            QueryDetailsDataGrid.ScrollIntoView(QueryDetailsDataGrid.SelectedItem, QueryDetailsDataGrid.Columns[0]);
        }

        private void GraphicsLayer_MouseLeave(object sender, GraphicMouseEventArgs args)
        {
            QueryDetailsDataGrid.Focus();
            QueryDetailsDataGrid.SelectedItem = null;
        }
        #endregion

        #region 判断点在选择的范围内
        //判断点在线的一边  
        private int isLeft(Point P0, Point P1, Point P2)
        {
            int abc = (int)((P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y));
            return abc;

        }   

        //判断点pnt是否在region内主程序   
        private bool isInRegion(Point pnt, List<Point> region)
        {
            int wn = 0, j = 0; //wn 计数器 j第二个点指针   
            for (int i = 0; i < region.Count; i++)
            {
                //开始循环   
                if (i == region.Count - 1)
                {
                    j = 0;//如果 循环到最后一点 第二个指针指向第一点   
                }
                else
                {
                    j = j + 1; //如果不是 ，则找下一点   
                }

                if (region[i].Y <= pnt.Y) // 如果多边形的点 小于等于 选定点的 Y 坐标   
                {
                    if (region[j].Y > pnt.Y) // 如果多边形的下一点 大于于 选定点的 Y 坐标   
                    {
                        if (isLeft(region[i], region[j], pnt) > 0)
                        {
                            wn++;
                        }
                    }
                }
                else
                {
                    if (region[j].Y <= pnt.Y)
                    {
                        if (isLeft(region[i], region[j], pnt) < 0)
                        {
                            wn--;
                        }
                    }
                }
            }
            if (wn == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
