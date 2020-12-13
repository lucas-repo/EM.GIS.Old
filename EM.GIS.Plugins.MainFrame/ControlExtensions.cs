using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EM.GIS.Plugins.MainFrame
{
    public static class ControlExtensions
    {
        /// <summary>
        /// 获取按钮
        /// </summary>
        /// <param name="header"></param>
        /// <param name="command"></param>
        /// <param name="iconPath"></param>
        /// <param name="largeIconPath"></param>
        /// <param name="toolTip"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Fluent.Button GetFluentButton(object header,ICommand command, string iconPath, string largeIconPath, object toolTip = null, Fluent.RibbonControlSize size = Fluent.RibbonControlSize.Large)
        {
            var button = new Fluent.Button()
            {
                Header = header,
                ToolTip = toolTip,
                Size = size,
                Command= command
            };
            if (!string.IsNullOrEmpty(iconPath))
            {
                button.Icon = new BitmapImage(new Uri(iconPath, UriKind.RelativeOrAbsolute));
            }
            if (!string.IsNullOrEmpty(largeIconPath))
            {
                button.LargeIcon = new BitmapImage(new Uri(largeIconPath, UriKind.RelativeOrAbsolute));
            }
            return button;
        }

    }
}
