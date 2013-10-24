using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace file_manager_test_app.Views
{
    public class FileSystemElementView
    {
        //_type: 0 - left, 1 - right
        private int _type;
        private Window _mainWindow;      
        private Controllers.ColumnController _controller;
        private List<Model.MyFileSystemInfo> _elements;

        private string[] _elementListBoxesNames = { "LeftElementListBox", "RightElementListBox" };

        public FileSystemElementView(int type, Window mainWindow, Controllers.ColumnController controller)
        {
            _type = type;
            _mainWindow = mainWindow;
            _controller = controller;
            _elements = _controller.GetElementsList();
        }

        public void Refresh()
        {
            _elements.Clear();
            _elements = _controller.GetElementsList();
            SetElementsList();
        }

        public void SetElementsList()
        {
            ListBox listBox = (ListBox)_mainWindow.FindName(_elementListBoxesNames[_type]);
            listBox.Items.Clear();

            int i = 0;
            foreach (Model.MyFileSystemInfo element in _elements)
            {
                ListBoxItem item = new ListBoxItem();

                if (!_controller.isCurrentPathRoot() && i == 0)
                {
                    item.Content = "[..]";
                    item.Uid = "-1";
                }
                else
                {
                    item.Content = "[" + element.Name + "]";
                    item.Uid = "" + i;
                }
                item.MouseDoubleClick += fileSystemElement_Click;
                item.KeyDown += fileSystemElement_KeyDown;

                listBox.Items.Add(item);
                i++;
            }
        }

        public void SetActiveElement()
        { }

        public void SelectElements()
        { }

        private void fileSystemElement_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = sender as ListBoxItem;
            int elementId = Convert.ToInt32(item.Uid);

            _controller.SetActiveElement(elementId);
            _controller.Read();
        }

        private void fileSystemElement_KeyDown(object sender, RoutedEventArgs e)
        {
        }
    }
}
