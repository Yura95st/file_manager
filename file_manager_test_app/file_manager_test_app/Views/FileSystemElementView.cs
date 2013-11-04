using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace file_manager_test_app.Views
{
    public class FileSystemElementView
    {
        //_type: 0 - left, 1 - right
        private int _type;
        private Window _mainWindow;      
        private Controllers.ColumnController _controller;
        private List<Model.MyFileSystemInfo> _elements;

        private string[] _elementListViewsNames = { "LeftElementList", "RightElementList" };
        private string[] _listBoxPathNames = { "LeftListBoxPath", "RightListBoxPath" };
        private string _textBoxPathLabelName = "TextBoxPathLabel";
        private string _textBoxPathName = "TextBoxPath";

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
            SetListBoxPath();
            SetTextBoxLabelPath();
        }

        public void SetElementsList()
        {
            ListView listView = (ListView)_mainWindow.FindName(_elementListViewsNames[_type]);
            if (listView.Items.Count == 0)
            {
                listView.SelectionChanged += SelectListViewElements;
            }
            listView.Items.Clear();            
           
            ListViewItem item = new ListViewItem();

            if (!_controller.isCurrentPathRoot())
            {
                item.Content = "[..]";
                item.Uid = "-1";

                item.MouseDoubleClick += FileSystemElement_DoubleClick;
                item.KeyDown += FileSystemElement_KeyDown;

                listView.Items.Add(item);
            }

            List<Model.MyFileSystemInfo> elementList = new List<Model.MyFileSystemInfo>();
            int i = 0;

            foreach (Model.MyFileSystemInfo element in _elements)
            {
                item = new ListViewItem();

                item.Content = "[" + element.Name + "]";
                item.Uid = "" + i++;

                item.MouseDoubleClick += FileSystemElement_DoubleClick;
                item.KeyDown += FileSystemElement_KeyDown;

                listView.Items.Add(item);
            }

            listView.SelectedIndex = 0;
        }

        public void SetListBoxPath()
        {
            TextBox textBox = (TextBox)_mainWindow.FindName(_listBoxPathNames[_type]);
            textBox.Text = _controller.GetCurrentPath().ToLower();
        }

        public void SetTextBoxLabelPath()
        {
            Label label = (Label)_mainWindow.FindName(_textBoxPathLabelName);
            label.Content = _controller.GetCurrentPath().ToLower() + ">";
        }

        public void SetAlertMessage(string msg)
        {
            MessageBoxResult result = MessageBox.Show(_mainWindow, msg, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                // Yes code here
            }
            else
            {
                // No code here
            } 
        }

        private void SelectListViewElements(object sender, RoutedEventArgs e)
        {
            ListView lv = sender as ListView;
            List<int> idList = new List<int>();

            foreach (ListViewItem element in lv.SelectedItems)
            {
                idList.Add(Convert.ToInt32(element.Uid));
            }

            _controller.SelectElements(idList);

            //active element is the last element in selection
            int count = lv.SelectedItems.Count;
            int lastSelectedElementId = 0;

            if (count > 0)
            {
                ListViewItem lastSelectedElement = (ListViewItem)lv.SelectedItem;
                lastSelectedElementId = Convert.ToInt32(lastSelectedElement.Uid);
            }
            _controller.SetActiveElement(lastSelectedElementId);
        }

        private void FileSystemElementRead(object sender)
        {
            ListBoxItem item = sender as ListBoxItem;
            int elementId = Convert.ToInt32(item.Uid);

            if (elementId == -1)
            {
                _controller.NavigationHistoryGoBack();
            }
            else
            {
                _controller.Read();
            }
        }

        private void ElementCopy()
        {
            _controller.WriteToBuffer();
        }

        private void ElementCut()
        {
            //_controller.WriteToBuffer();
        }

        private void ElementPaste()
        {
            _controller.CopyTo(1); // 1 - means buffered copying
        }

        private void ElementCopyTo()
        {
            _controller.CopyTo();
        }

        private void ElementMoveTo()
        {
            _controller.MoveTo();
        }

        private void ElementDelete()
        {
            _controller.Delete();
        }

        private void FileSystemElement_DoubleClick(object sender, RoutedEventArgs e)
        {
            FileSystemElementRead(sender);
        }

        private void FileSystemElement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FileSystemElementRead(sender);
            }
            else if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ElementCopy();
            }
            else if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ElementPaste();
            }
            else if (e.Key == Key.F5)
            {
                ElementCopyTo();
            }
            else if (e.Key == Key.F6)
            {
                ElementMoveTo();
            }
            else if (e.Key == Key.F8)
            {
                ElementDelete();
            }
        }
    }
}
