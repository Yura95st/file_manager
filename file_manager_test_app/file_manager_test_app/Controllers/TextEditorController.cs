using System;
using System.Collections.Generic;
using file_manager_test_app.Models;
using file_manager_test_app.Views;

namespace file_manager_test_app.Controllers
{
    public class TextEditorController
    {
        private TextEditorModel _model;
        private TextEditorView _view;

        public TextEditorController(TextEditorModel model, TextEditorView view)
        {
            _model = model;
            _view = view;
            _model.AddObserver(_view);
            _view.AddModel(_model);
            _view.AddListener(this);
        }

        public void Init(string path)
        {
            try
            {
                _model.SetFile(path);
            }
            catch (Exception e)
            { }
        }
    }
}
