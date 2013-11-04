using System;
using System.Collections.Generic;
using System.IO;

namespace file_manager_test_app.Model
{
    class MyBuffer
    {                
        private static MyBuffer _singletonInstance = null;
        private List<FileSystemInfo> _elements = new List<FileSystemInfo>();

        private MyBuffer() 
        { }

        public static MyBuffer GetInstance()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new MyBuffer();
            }
            return _singletonInstance;
        }

        public List<FileSystemInfo> GetElements()
        {
            return _elements;
        }

        public void AddElement(FileSystemInfo element)
        {
            _elements.Add(element);
        }

        public void Clear()
        {
            _elements.Clear();
        }
    }
}
