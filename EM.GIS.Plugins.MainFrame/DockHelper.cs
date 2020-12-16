using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Themes;
using EM.GIS.WPFControls;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.GIS.Plugins.MainFrame
{
    public class DockHelper
    {
        public DockHelper(IWpfAppManager app)
        {
            AppManager = app;
        }

        public IWpfAppManager AppManager { get; }

        private LayoutAnchorablePaneGroup GetLeftLayoutAnchorablePaneGroup()
        {
            var layoutAnchorablePaneGroup = new LayoutAnchorablePaneGroup()
            {
                DockMinWidth = 200,
                FloatingWidth = 200
            };
            LayoutAnchorablePane layoutAnchorablePane = new LayoutAnchorablePane();
            LayoutAnchorable layoutAnchorable = new LayoutAnchorable()
            {
                Title = "图例",
                CanHide = false
            };
            layoutAnchorablePane.Children.Add(layoutAnchorable);
            layoutAnchorablePaneGroup.Children.Add(layoutAnchorablePane);
            return layoutAnchorablePaneGroup;
        }
        private LayoutDocumentPaneGroup GetFillLayoutDocumentPaneGroup()
        {
            LayoutDocumentPaneGroup layoutDocumentPaneGroup = new LayoutDocumentPaneGroup();
            Map map = new Map();
            AppManager.Map = map;
            LayoutDocument layoutDocument = new LayoutDocument()
            {
                Title = "地图",
                Content = map,
                CanClose = false
            };
            LayoutDocumentPane layoutDocumentPane = new LayoutDocumentPane();
            layoutDocumentPane.Children.Add(layoutDocument);
            layoutDocumentPaneGroup.Children.Add(layoutDocumentPane);
            return layoutDocumentPaneGroup;
        }

        public DockingManager GetDockingManager()
        {
            LayoutPanel layoutPanel = new LayoutPanel()
            {
                Orientation = System.Windows.Controls.Orientation.Horizontal
            };
            layoutPanel.Children.Add(GetLeftLayoutAnchorablePaneGroup());
            layoutPanel.Children.Add(GetFillLayoutDocumentPaneGroup());
            LayoutRoot layoutRoot = new LayoutRoot()
            {
                RootPanel = layoutPanel
            };
            DockingManager dockingManager = new DockingManager()
            {
                Layout = layoutRoot,
                //Theme = new MetroTheme()
            };
            return dockingManager;
        }
    }
}
