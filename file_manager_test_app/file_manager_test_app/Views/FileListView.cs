using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using file_manager_test_app.Libs.ObserverPattern;
using file_manager_test_app.Models;
using file_manager_test_app.Controllers;

namespace file_manager_test_app.Views
{
    public class FileListView : IObserver
    {
        //_type: 0 - left, 1 - right
        private int _type;
        private Window _mainWindow;

        private ColumnController _controller;
        private FileListModel _model;

        private string[] _elementListViewsNames = { "LeftElementList", "RightElementList" };
        private string[] _listBoxPathNames = { "LeftListBoxPath", "RightListBoxPath" };
        //private string _textBoxPathLabelName = "TextBoxPathLabel";
        //private string _textBoxPathName = "TextBoxPath";

        public FileListView(int type, Window mainWindow)
        {
            _type = type;
            _mainWindow = mainWindow;
            BindListViewSelectionChangedEvent();
            //_mainWindow.SizeChanged += ;
        }

        public void AddListener(ColumnController controller)
        {
            _controller = controller;
        }

        public void AddModel(FileListModel model)
        {
            _model = model;
        }

        public void BuildFileList()
        {
            List<MyFileSystemInfo> list = _controller.GetElementsList();
            string path = _model.CurrentPath;

            SetList(list);
            SetListBoxPath(path);
        }

        public void Update(int notificationCode)
        {
            switch (notificationCode)
            {
                case 0:
                    BuildFileList();
                    break;
            }
        }

        public void SetList(List<MyFileSystemInfo> list)
        {
            ListView listView = (ListView)_mainWindow.FindName(_elementListViewsNames[_type]);
            listView.Items.Clear();
           
            ListViewItem item = new ListViewItem();

            for (int i = -1, count = list.Count; i < count; i++)
            {
                item = new ListViewItem();

                if (i == -1)
                {
                    if (_controller.isCurrentPathRoot())
                    {
                        continue;
                    }

                    item.Content = "[..]";
                }
                else
                {
                    item.Content = "[" + list[i].Name + "]";
                }

                item.Uid = i.ToString();
                item.MouseDoubleClick += FileSystemElement_OnDoubleClick;
                item.KeyDown += FileSystemElement_OnKeyDown;

                listView.Items.Add(item);
            }
        }

        public void SetListBoxPath(string path)
        {
            TextBox textBox = (TextBox)_mainWindow.FindName(_listBoxPathNames[_type]);
            textBox.Text = path.ToLower();
        }

        private void BindListViewSelectionChangedEvent()
        {
            ListView listView = (ListView)_mainWindow.FindName(_elementListViewsNames[_type]);
            listView.SelectionChanged += SelectListViewElements;
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

        private void Copy()
        {
            _controller.WriteToBuffer();
        }

        private void Cut()
        {
            _controller.WriteToBuffer(1);
        }

        private void Paste()
        {
            _controller.PasteFromBuffer();
        }

        private void FileSystemElement_OnDoubleClick(object sender, RoutedEventArgs e)
        {
            FileSystemElementRead(sender);
        }

        private void FileSystemElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FileSystemElementRead(sender);
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
                Copy();
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.X)
            {
                Cut();
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.V)
            {
                Paste();
            }
            else if (e.Key == Key.F2)
            {
                Rename();
            }
            else if (e.Key == Key.F5)
            {
                CopyTo();
            }
            else if (e.Key == Key.F6)
            {
                MoveTo();
            }
            else if (e.Key == Key.F7)
            {
                CreateNewFolder();
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift && (e.Key == Key.F8 || e.Key == Key.Delete))
            {
                Delete(true);
            }
            else if (e.Key == Key.F8 || e.Key == Key.Delete)
            {
                Delete(false);
            }
        }

        delegate void MyDelegate(string s);

        public void CopyTo()
        {
            string path = _controller.GetColumnController().GetCurrentPath();
            int counter = _controller.GetSelectedElements().Count;

            MyWindows.ConfirmationDialog _confirmationWindow =
                new MyWindows.ConfirmationDialog("Copy " + counter + " files/directories to:", path, new MyDelegate(_controller.CopyTo));

            _confirmationWindow.Show();
        }

        public void MoveTo()
        {
            string path = _controller.GetColumnController().GetCurrentPath();
            int counter = _controller.GetSelectedElements().Count;

            MyWindows.ConfirmationDialog _confirmationWindow =
                new MyWindows.ConfirmationDialog("Move " + counter + " files/directories to:", path, new MyDelegate(_controller.MoveTo));

            _confirmationWindow.Show();
        }

        public void Delete(bool delPermanent)
        {
            if (!delPermanent)
            {
                _controller.Delete(delPermanent);
            }
            else
            {
                string text = "";
                var elements = _controller.GetElementsList();
                List<int> selectedElements = _controller.GetSelectedElements();
                int counter = selectedElements.Count;

                foreach (int i in selectedElements)
                {
                    text += elements[i].Name + "\n";
                }

                string caption = "File Commander";
                string message = "Do you really want to delete the " + counter + " selected files/directories?\n\n" + text;
                MessageBoxButton buttons = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Question;
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    _controller.Delete(delPermanent);
                }
            }
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

        public void CreateNewFolder()
        {
            string name = "";
            try
            {
                name = _controller.GetElementsList()[_controller.GetActiveElement()].Name;
            }
            catch (Exception e)
            { }
            finally
            {
                MyWindows.ConfirmationDialog _confirmationWindow =
                    new MyWindows.ConfirmationDialog("New Folder(directory):", name, new MyDelegate(_controller.CreateNewDirectory));

                _confirmationWindow.Show();
            }
        }

        public void Rename()
        {
            string name = "";
            try
            {
                name = _controller.GetElementsList()[_controller.GetActiveElement()].Name;
            }
            catch (Exception e)
            { }
            finally
            {
                MyWindows.ConfirmationDialog _confirmationWindow =
                    new MyWindows.ConfirmationDialog("Rename:", name, new MyDelegate(_controller.Rename));

                _confirmationWindow.Show();
            }
        }
    }
}
