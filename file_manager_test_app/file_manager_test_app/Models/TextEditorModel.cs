namespace file_manager_test_app.Models
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using Microsoft.VisualBasic.FileIO;
    using file_manager_test_app.Libs.ObserverPattern;

    public class TextEditorModel : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        private FileInfo _file;

        public TextEditorModel()
        { }

        public void SetFile(string path)
        {
            try
            {
                _file = new FileInfo(@path);
                NotifyObservers(0);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
 
        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }
 
        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }
 
        public void NotifyObservers(int notificationCode)
        {
            foreach (var observer in _observers)
            {
                observer.Update(notificationCode);
            }
        }

        public string GetFileContext()
        {
            string text = "";

            try
            {
                text = File.ReadAllText(_file.FullName);
            }
            catch (Exception e)
            { }

            return text;
        }
    }
}
