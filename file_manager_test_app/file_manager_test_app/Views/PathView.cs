using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace file_manager_test_app.Views
{
    public static class PathView
    {
        public static Window _mainWindow;
        public static Controllers.ColumnController _controller;

        public static string[] _listBoxPathNames = { "LeftListBoxPath", "RightListBoxPath" };
        public static string _textBoxPathLabelName = "TextBoxPathLabel";
        public static string _textBoxPathName = "TextBoxPath";

        public static void Init(Window mainWindow, Controllers.ColumnController controller)
        {
            _mainWindow = mainWindow;
            _controller = controller;
            TextBoxPath_BindKeyDown();
        }

        private static void TextBoxPath_BindKeyDown()
        {
            TextBox textBox = (TextBox)_mainWindow.FindName(_textBoxPathName);
            textBox.KeyDown += TextBoxPath_KeyDown;
        }

        private static void TextBoxPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = sender as TextBox;
                try
                {
                    _controller.SetCurrentPath(_controller.GetCurrentPath() + textBox.Text);
                    _controller.Read();
                }
                catch (Exception exception)
                {
                    //SetAlertMessage(exception.Message);
                }
            }
        }

    }
}
