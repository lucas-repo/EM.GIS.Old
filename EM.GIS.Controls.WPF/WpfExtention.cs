using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace EM.GIS.WpfControls
{
    public static class WpfExtention
    {
        public static TEnum1 GetAnotherEnum<TEnum0, TEnum1>(this TEnum0 enum0) where TEnum0 : Enum
            where TEnum1 : Enum
        {
            TEnum1 enum1 = default;
            var ret = Enum.TryParse(typeof(Controls.MouseButtons), enum0.ToString(), out object? obj);
            if (ret)
            {
                enum1 = (TEnum1)obj;
            }
            return enum1;
        }
        public static Controls.MouseButtons GetMouseButtons(this MouseEventArgs e)
        {
            Controls.MouseButtons mouseButtons = Controls.MouseButtons.None;
            if (e != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    mouseButtons = Controls.MouseButtons.Left;
                }
                else if (e.MiddleButton == MouseButtonState.Pressed)
                {
                    mouseButtons = Controls.MouseButtons.Middle;
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    mouseButtons = Controls.MouseButtons.Right;
                }
                else if (e.XButton1 == MouseButtonState.Pressed)
                {
                    mouseButtons = Controls.MouseButtons.XButton1;
                }
                else if (e.XButton2 == MouseButtonState.Pressed)
                {
                    mouseButtons = Controls.MouseButtons.XButton2;
                }
            }
            return mouseButtons;
        }
        public static Controls.MouseEventArgs ToMouseEventArgs(this MouseButtonEventArgs e, IInputElement inputElement)
        {
            Controls.MouseEventArgs mouseEventArgs = null;
            if (e == null)
            {
                return mouseEventArgs; 
            }
            var mouseButtons = e.ChangedButton.GetAnotherEnum<MouseButton, Controls.MouseButtons>();
            var position= e.GetPosition(inputElement);
            mouseEventArgs = new Controls.MouseEventArgs(mouseButtons,e.ClickCount, (int)position.X, (int)position.Y);
            return mouseEventArgs;
        }
        public static Controls.MouseEventArgs ToMouseEventArgs(this MouseEventArgs e, IInputElement inputElement)
        {
            Controls.MouseEventArgs mouseEventArgs = null;
            if (e == null)
            {
                return mouseEventArgs;
            }
            var mouseButtons = e.GetMouseButtons();
            int count = mouseButtons != Controls.MouseButtons.None ? 1 : 0;
            var position = e.GetPosition(inputElement);
            mouseEventArgs = new Controls.MouseEventArgs(mouseButtons, count, (int)position.X, (int)position.Y);
            return mouseEventArgs;
        }
        public static Controls.MouseEventArgs ToMouseEventArgs(this MouseWheelEventArgs e, IInputElement inputElement)
        {
            Controls.MouseEventArgs mouseEventArgs = null;
            if (e == null)
            {
                return mouseEventArgs;
            }
            var position = e.GetPosition(inputElement);
            mouseEventArgs = new Controls.MouseEventArgs( (int)position.X, (int)position.Y, e.Delta);
            return mouseEventArgs;
        }

        
    }
}
