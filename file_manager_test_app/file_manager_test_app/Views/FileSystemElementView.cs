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
        private Controllers.FileSystemController _controller;
        private List<Model.MyFileSystemInfo> _elements;

        private string[] _elementListBoxesNames = { "LeftElementListBox", "RightElementListBox" };

        public FileSystemElementView(int type, Window mainWindow)
        {
            _type = type;
            _mainWindow = mainWindow;
            _controller = new Controllers.FileSystemController();
            _elements = _controller.GetElementsList();
        }

        public void BuildList()
        {
            ListBox listBox = (ListBox)_mainWindow.FindName(_elementListBoxesNames[_type]);

            foreach (Model.MyFileSystemInfo element in _elements)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = "[" + element.Name + "]";

                listBox.Items.Add(item);
            }
        }

        public void SetActiveElement()
        { }

        public void SelectElements()
        { }

        private void GetElementsList()
        { }
    }
}
