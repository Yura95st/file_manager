using System;
using System.Collections.Generic;
using System.IO;

namespace file_manager_test_app.Models
{
    public class MyBuffer
    {                
        private static MyBuffer _singletonInstance = null;
        private List<FileSystemInfo> _elements = new List<FileSystemInfo>();
        private int _type = 0; //type: 0 - copy/paste, 1 - cut/paste

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

        public void SetCopyPasteType()
        {
            _type = 0;
        }

        public void SetCutPasteType()
        {
            _type = 1;
        }

        public bool IsCutPasteType()
        {
            return (_type == 1);
        }
    }
}
