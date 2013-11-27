using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing;

using file_manager_test_app.Libs.ObserverPattern;
using file_manager_test_app.Models;
using file_manager_test_app.Controllers;
using System.Text.RegularExpressions;

namespace file_manager_test_app.Views
{
    public class TextEditorView : IObserver
    {
        private Window _mainWindow;

        private TextEditorController _controller;
        private TextEditorModel _model;

        private string fileContentTextBoxName = "FileContentTextBox";
        private string searchTextBoxName = "SearchTextBox";
        private string searchResultsLabelName = "SearchResultsLabel";

        public TextEditorView(Window mainWindow)
        {
            _mainWindow = mainWindow;

            TextBox searchTextBox = (TextBox)_mainWindow.FindName(searchTextBoxName);
            searchTextBox.TextChanged += searchTextBox_TextChanged;
        }

        public void AddListener(TextEditorController controller)
        {
            _controller = controller;
        }

        public void AddModel(TextEditorModel model)
        {
            _model = model;
        }

        public void Update(int notificationCode)
        {
            switch (notificationCode)
            {
                case 0:
                    InitTextBlock();
                    break;
            }
        }

        private void InitTextBlock()
        {
            TextBox textBox = (TextBox)_mainWindow.FindName(fileContentTextBoxName);
            string context = _model.GetFileContext();
            textBox.Text = context;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            string resultText = "Found matches: ";

            TextBox fileContentTextBox = (TextBox)_mainWindow.FindName(fileContentTextBoxName);
            TextBox searchTextBox = (TextBox)_mainWindow.FindName(searchTextBoxName);
            Label searchResultsLabel = (Label)_mainWindow.FindName(searchResultsLabelName);

            searchResultsLabel.Content = "";

            if (searchTextBox.Text.Length > 0)
            {
                MatchCollection matches = Regex.Matches(fileContentTextBox.Text, searchTextBox.Text);

                searchResultsLabel.Content = resultText + matches.Count;
            }
        }
    }
}
