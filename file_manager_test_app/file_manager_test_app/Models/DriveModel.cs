using System;
using System.IO;
using System.Collections.Generic;

using file_manager_test_app.Libs.ObserverPattern;
using file_manager_test_app.Models;

namespace file_manager_test_app.Models
{
    public class DriveModel : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();

        private List<DriveInfo> _drives = new List<DriveInfo>();
        private int _activeDrive = -1;

        public DriveModel()
        {  }

        public int ActiveDrive
        {
            get
            {
                return _activeDrive;
            }
            set
            {
                _activeDrive = value;
                NotifyObservers(2);
            }
        }

        public void BuildDrives()
        {
            _drives.Clear();
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            try
            {
                foreach (DriveInfo d in allDrives)
                {
                    _drives.Add(d);
                }
                NotifyObservers(1);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MyDriveInfo> GetDrivesList()
        {
            List<MyDriveInfo> list = new List<MyDriveInfo>();

            foreach (DriveInfo d in _drives)
            {
                MyDriveInfo myDrive = new MyDriveInfo(d.AvailableFreeSpace, d.DriveFormat, d.DriveType,
                    d.Name, d.TotalFreeSpace, d.TotalSize, d.VolumeLabel, d.RootDirectory.Name);

                list.Add(myDrive);
            }

            return list;
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
    }
}
