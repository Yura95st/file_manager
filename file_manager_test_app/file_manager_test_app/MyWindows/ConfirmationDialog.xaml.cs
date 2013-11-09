using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace file_manager_test_app.MyWindows
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationDialog : Window
    {
        private string _label = "";
        private string _text = "";
        private Delegate _function = null;

        private string _labelName = "Label";
        private string _textBoxName = "TextBox";
        private string _okButtonName = "OKButton";
        private string _cancelButtonName = "CancelButton";

        public ConfirmationDialog(string label, string text, Delegate function)
        {
            _label = label;
            _text = text;
            _function = function;

            InitializeComponent();

            Init();
        }

        private void Init()
        {
            Label lable = (Label)this.FindName(_labelName);
            lable.Content = _label;

            TextBox textBox = (TextBox)this.FindName(_textBoxName);
            textBox.Text = _text;
            textBox.KeyDown += TextBox_KeyDown;

            textBox.SelectAll();
            textBox.Focus();

            Button OkButton = (Button)this.FindName(_okButtonName);
            OkButton.Click += okButton_Click;

            Button CancelButton = (Button)this.FindName(_cancelButtonName);
            CancelButton.Click += CancelButton_Click;

            this.KeyDown += Window_KeyDown;
        }

        private void PerformBindedAction()
        {
            TextBox textBox = (TextBox)this.FindName(_textBoxName);
            string path = textBox.Text;
            _function.DynamicInvoke(path);
            this.Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            PerformBindedAction();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformBindedAction();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
