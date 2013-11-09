using System;
using System.Collections.Generic;

namespace file_manager_test_app.Models
{
    public class NavigationHistoryModel
    {
        private List<string> _navigationHistory = new List<string>();
        private int _pointer = -1;

        public NavigationHistoryModel()
        { }

        public string GetLastItem()
        {
            return (_pointer >= 0)? _navigationHistory[_pointer] : "";
        }

        public void GoBack()
        {
            if (_pointer >= 1)
            {
                _pointer--;
            }
        }

        public void AddItem(string item)
        {
            _pointer++;
            _navigationHistory.Insert(_pointer, item);
        }
    }
}
