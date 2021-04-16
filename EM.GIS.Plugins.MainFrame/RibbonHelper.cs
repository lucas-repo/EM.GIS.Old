using EM.GIS.Controls;
using EM.GIS.Data;
using EM.GIS.WPFControls;
using Fluent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace EM.GIS.Plugins.MainFrame
{
    public class RibbonHelper
    {
        public IWpfAppManager AppManager { get; }
        public IMap Map => AppManager.Map;
        public RibbonHelper(IWpfAppManager appManager)
        {
            AppManager = appManager;
        }
        #region StartScreen
        private object GetLeftContent()
        {
            Image image = new Image()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/EM.GIS.Resources;Component/Images/Home32.png", UriKind.RelativeOrAbsolute))
            };
            TextBlock textBlock = new TextBlock()
            {
                Text = "首页",
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Center
            };
            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.Children.Add(image);
            wrapPanel.Children.Add(textBlock);
            return wrapPanel;
        }
        private object GetRightContent()
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = "欢迎，现在开始您的易图世界！",
                FontSize = 14
            };
            Fluent.Button button = new Fluent.Button()
            {
                Header = "进入",
                Content = "进入",
                FontSize = 14,
                IsDefinitive = true
            };
            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(button);
            return stackPanel;
        }
        private StartScreen GetStartScreen()
        {
            StartScreenTabControl startScreenTabControl = new StartScreenTabControl()
            {
                LeftContent = GetLeftContent(),
                RightContent = GetRightContent()
            };
            StartScreen startScreen = new StartScreen()
            {
                IsOpen = true,
                Content = startScreenTabControl
            };
            return startScreen;
        }
        #endregion

        #region Menu
        private BackstageTabItem GetNewMapBackstageTabItem()
        {
            var button = new Fluent.Button()
            {
                Content = "空白地图",
                IsDefinitive = true,
                Icon = new BitmapImage(new Uri("pack://application:,,,/EM.GIS.Resources;Component/Images/File128.png", UriKind.RelativeOrAbsolute))
            };
            button.Click += NewMap;
            BackstageTabItem backstageTabItem = new BackstageTabItem()
            {
                Header = "新建",
                Icon = new BitmapImage(new Uri("pack://application:,,,/EM.GIS.Resources;Component/Images/New32.png", UriKind.RelativeOrAbsolute)),
                Content = button
            };
            return backstageTabItem;
        }
        private Backstage GetMenu()
        {
            BackstageTabControl backstageTabControl = new BackstageTabControl();
            backstageTabControl.Items.Add(GetNewMapBackstageTabItem());
            backstageTabControl.Items.Add(GetOpenMapBackstageTabItem());
            backstageTabControl.Items.Add(GetSaveMapBackstageTabItem());
            Backstage menu = new Backstage()
            {
                Content = backstageTabControl
            };
            return menu;
        }

        private BackstageTabItem GetSaveMapBackstageTabItem()
        {
            BackstageTabItem backstageTabItem = new BackstageTabItem()
            {
                Header = "保存",
                Icon = new BitmapImage(new Uri("pack://application:,,,/EM.GIS.Resources;Component/Images/Save32.png", UriKind.RelativeOrAbsolute)),
                Content = ControlExtensions.GetFluentButton("保存",GetSaveProjectCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Folder16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Folder32.png","保存工程")
            };
            return backstageTabItem;
        }

        private object GetOpenMapBackstageTabItem()
        {
            var button = new Fluent.Button()
            {
                Content = "浏览",
                IsDefinitive = true,
                Icon = new BitmapImage(new Uri("pack://application:,,,/EM.GIS.Resources;Component/Images/Folder16.png", UriKind.RelativeOrAbsolute)),
                LargeIcon = new BitmapImage(new Uri("pack://application:,,,/EM.GIS.Resources;Component/Images/Folder32.png", UriKind.RelativeOrAbsolute))
            };
            button.Click += OpenMap;
            BackstageTabItem backstageTabItem = new BackstageTabItem()
            {
                Header = "打开",
                Icon = new BitmapImage(new Uri("pack://application:,,,/EM.GIS.Resources;Component/Images/Open32.png", UriKind.RelativeOrAbsolute)),
                Content = button
            };
            return backstageTabItem;
        }

        private void OpenMap(object sender, RoutedEventArgs e)
        {
        }

        private void NewMap(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        #region QuickAccessItems

        private void AddQuickAccessItems(Ribbon ribbon)
        {
            ribbon.QuickAccessItems.Add(GetQuickAccessMenuItem("保存", GetSaveProjectCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Save16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Save32.png", "保存工程"));
            ribbon.QuickAccessItems.Add(GetQuickAccessMenuItem("撤销", GetUndoCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Undo16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Undo32.png", "撤销"));
            ribbon.QuickAccessItems.Add(GetQuickAccessMenuItem("重做", GetRedoCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Redo16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Redo32.png", "重做"));
        }

        private ICommand GetRedoCommand()
        {
            return GetSimpleCommand("redo");
        }

        private QuickAccessMenuItem GetQuickAccessMenuItem(object header, ICommand command, string iconPath, string largeIconPath, object toolTip = null, RibbonControlSize size = RibbonControlSize.Large)
        {
            QuickAccessMenuItem quickAccessMenuItem = new QuickAccessMenuItem()
            {
                IsChecked = true,
                Target = ControlExtensions.GetFluentButton(header,  command,  iconPath,  largeIconPath,  toolTip ,  size )
            };
            return quickAccessMenuItem;
        }
       
        private ICommand GetUndoCommand()
        {
            return GetSimpleCommand("undo");
        }

        private ICommand GetSaveProjectCommand()
        {
            return GetSimpleCommand("saveProject");
        }
        #endregion

        #region Tabs
        private void AddTabs(Ribbon ribbon)
        {
            ribbon.Tabs.Add(GetMapRibbonTabItem());
        }

        private RibbonTabItem GetMapRibbonTabItem()
        {
            RibbonTabItem ribbonTabItem = new RibbonTabItem()
            {
                Header = "地图",

            };
            ribbonTabItem.Groups.Add(GetNavigateRibbonGoupBox());
            ribbonTabItem.Groups.Add(GetLayerRibbonGoupBox());
            return ribbonTabItem;
        }

        private RibbonGroupBox GetLayerRibbonGoupBox()
        {
            RibbonGroupBox ribbonGroupBox = new RibbonGroupBox()
            {
                Header = "图层"
            };
            ribbonGroupBox.Items.Add(ControlExtensions.GetFluentButton("添加", GetAddLayersCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Add16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Add32.png", "添加图层"));
            ribbonGroupBox.Items.Add(ControlExtensions.GetFluentButton("移除", GetRemoveLayersCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Remove16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Remove32.png", "移除图层"));
            return ribbonGroupBox;
        }

        private ICommand GetRemoveLayersCommand()
        {
            return GetSimpleCommand("removeLayers");
        }

        private ICommand GetAddLayersCommand()
        {
            var commandName = "addLayers";
            if (!CommandDic.ContainsKey(commandName))
            {
                CommandDic[commandName] = new DelegateCommand((obj) => Map?.AddLayers());
            }
            return CommandDic[commandName];
        }

        private ICommand GetSimpleCommand(string commandName, Func<ICommand> getCommandFunc = null)
        {
            ICommand command = null;
            if (!string.IsNullOrEmpty(commandName))
            {
                if (!CommandDic.ContainsKey(commandName))
                {
                    if (getCommandFunc == null)
                    {
                        CommandDic[commandName] = new DelegateCommand();
                    }
                    else
                    {
                        CommandDic[commandName] = getCommandFunc();
                    }
                }
                command = CommandDic[commandName];
            }
            return command;
        }
        private ICommand GetActivePanToolCommand()
        {
            return GetSimpleCommand("activePanTool");
        }
        private RibbonGroupBox GetNavigateRibbonGoupBox()
        {
            RibbonGroupBox ribbonGroupBox = new RibbonGroupBox()
            {
                Header = "导航"
            };
            ribbonGroupBox.Items.Add(ControlExtensions.GetFluentButton("平移", GetActivePanToolCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Pan16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Pan32.png", "平移工具"));
            ribbonGroupBox.Items.Add(ControlExtensions.GetFluentButton("全图", GetZoomToMaxExtentCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Global16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Global32.png", "缩放至全图", RibbonControlSize.Middle));
            ribbonGroupBox.Items.Add(ControlExtensions.GetFluentButton("放大", GetActiveZoomInToolCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/ZoomIn16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/ZoomIn32.png", "放大工具", RibbonControlSize.Middle));
            ribbonGroupBox.Items.Add(ControlExtensions.GetFluentButton("后退", GetZoomToPreviousViewCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Pre16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Pre32.png", "后退至前一视图", RibbonControlSize.Middle));
            ribbonGroupBox.Items.Add(ControlExtensions.GetFluentButton("识别", GetActiveIdentifyToolCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Identify16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Identify32.png", "识别工具", RibbonControlSize.Middle));
            ribbonGroupBox.Items.Add(ControlExtensions.GetFluentButton("缩小", GetActiveZoomOutToolCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/ZoomOut16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/ZoomOut32.png", "缩小工具", RibbonControlSize.Middle));
            ribbonGroupBox.Items.Add(ControlExtensions.GetFluentButton("前进", GetZoomToNextViewCommand(), "pack://application:,,,/EM.GIS.Resources;Component/Images/Next16.png", "pack://application:,,,/EM.GIS.Resources;Component/Images/Next32.png", "前进至后一视图", RibbonControlSize.Middle));
            return ribbonGroupBox;
        }

        private ICommand GetZoomToNextViewCommand()
        {
            return GetSimpleCommand("zoomToNextView");
        }

        private ICommand GetActiveZoomOutToolCommand()
        {
            return GetSimpleCommand("activeZoomOutTool");
        }

        private ICommand GetActiveIdentifyToolCommand()
        {
            return GetSimpleCommand("activeIdentifyTool");
        }

        private ICommand GetZoomToPreviousViewCommand()
        {
            return GetSimpleCommand("zoomToPreviousView");
        }

        Dictionary<string, ICommand> CommandDic = new Dictionary<string, ICommand>();
        private ICommand GetActiveZoomInToolCommand()
        {
            return GetSimpleCommand("activeZoomInTool");
        }

        private ICommand GetZoomToMaxExtentCommand()
        {
            var commandName = "zoomToMaxExtent";
            if (!CommandDic.ContainsKey(commandName))
            {
                CommandDic[commandName] = new DelegateCommand((obj) => Map?.ZoomToMaxExtent());
            }
            return CommandDic[commandName];
        }

        #endregion
        public Ribbon GetRibbon()
        {
            Ribbon ribbon = new Ribbon()
            {
                //StartScreen = GetStartScreen(),
                Menu = GetMenu()
            };
            AddQuickAccessItems(ribbon);
            AddTabs(ribbon);
            return ribbon;
        }
        public StatusBar GetStatusBar()
        {
            StatusBar statusBar = new StatusBar();
            statusBar.Items.Add(GetProgressStatusBarItem());
            statusBar.Items.Add(GetCoordStatusBarItem());
            return statusBar;
        }

        #region StatusBar
        private object GetCoordStatusBarItem()
        {
            StatusBarItem statusBarItem = new StatusBarItem()
            {
                Title = "坐标",
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(5, 0, 0, 0)
            };
            Map.GeoMouseMove += (sender, e) =>
            {
                statusBarItem.Content = $"{Math.Round(e.GeographicLocation.X, 3)},{Math.Round(e.GeographicLocation.Y, 3)}";
            };
            return statusBarItem;
        }
        private StatusBarItem GetProgressStatusBarItem()
        {
            WrapPanel wrapPanel = new WrapPanel();
            ProgressBar progressBar = new ProgressBar()
            {
                Width = 100,
                Margin = new Thickness(5, 0, 0, 0)
            };
            wrapPanel.Children.Add(progressBar);
            TextBlock textBlock = new TextBlock()
            {
                Margin = new Thickness(5, 0, 0, 0)
            };
            wrapPanel.Children.Add(textBlock);
            ProgressHandler progressHandler = new ProgressHandler()
            {
                Handler = (percent, message) =>
                {
                    var action = new Action(() =>
                    {
                        progressBar.Value = percent;
                        textBlock.Text = message;
                    });
                    AppManager.Window.Dispatcher.BeginInvoke(action);
                }
            };
            AppManager.ProgressHandler = progressHandler;
            StatusBarItem statusBarItem = new StatusBarItem()
            {
                Title = "进度",
                Content = wrapPanel,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            return statusBarItem;
        }
        #endregion

    }
}
