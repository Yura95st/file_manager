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

using file_manager_test_app.Models;
using file_manager_test_app.Controllers;
using file_manager_test_app.Views;

namespace file_manager_test_app.MyWindows
{
    /// <summary>
    /// Interaction logic for TextEditor.xaml
    /// </summary>
    public partial class TextEditorWindow : Window
    {
        public TextEditorWindow(string path)
        {
            InitializeComponent();

            TextEditorView view = new TextEditorView(this);
            TextEditorModel model = new TextEditorModel();
            TextEditorController controller = new TextEditorController(model, view);

            controller.Init(path);
        }
    }
}
