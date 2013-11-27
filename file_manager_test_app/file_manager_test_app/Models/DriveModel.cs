namespace file_manager_test_app.Models
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    //using System.Management;

    using file_manager_test_app.Libs.ObserverPattern;

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

        public List<int> GetReadyDrives()
        {
            List<int> activeDrives = new List<int>();
            int i = 0;

            foreach (var d in _drives)
            {
                if (d.IsReady)
                {
                    activeDrives.Add(i);
                }
                i++;
            }

            return activeDrives;
        }

        public void SetFirstReadyActiveDrive()
        {
            List<int> activeDrives = new List<int>(GetReadyDrives());

            if (activeDrives.Count > 0)
            {
                ActiveDrive = activeDrives[0];
            }
            else 
            {
                throw new Exception("There are no ready drives.");
            }
        }

        public void BuildDrives()
        {
            _drives.Clear();

            try
            {
                BuildDrivesCore(_drives);
                NotifyObservers(1);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void BuildDrivesCore(List<DriveInfo> drives)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            try
            {
                foreach (DriveInfo d in allDrives)
                {
                    drives.Add(d);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ReBuildDrives()
        {
            List<DriveInfo> reBuildedDrives = new List<DriveInfo>();

            try
            {
                BuildDrivesCore(reBuildedDrives);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                throw new Exception("Drives list is chaged.");

                if (reBuildedDrives.Count != _drives.Count)
                {
                    throw new Exception("Drives list is chaged.");
                }

                for (int i = 0, count = reBuildedDrives.Count; i < count; i++)
                {
                    if (reBuildedDrives[i].Name != _drives[i].Name)
                    {
                        throw new Exception("Drives list is chaged.");
                    }
                }
            }
            catch (Exception e)
            {
                _drives.Clear();
                _drives = reBuildedDrives;
                NotifyObservers(1);

                SetFirstReadyActiveDrive();                
            }
        }

        public List<MyDriveInfo> GetDrivesList()
        {
            List<MyDriveInfo> list = new List<MyDriveInfo>();

            foreach (DriveInfo d in _drives)
            {
                MyDriveInfo myDrive;

                if (d.IsReady)
                {
                    myDrive = new MyDriveInfo(d.AvailableFreeSpace, d.DriveFormat, d.DriveType,
                        d.Name, d.TotalFreeSpace, d.TotalSize, d.VolumeLabel, d.RootDirectory.Name);
                }
                else
                {
                    myDrive = new MyDriveInfo(0, "", d.DriveType, d.Name, 0, 0, "", "");
                }

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

        //public void BindDriveMountation()
        //{
        //    ManagementEventWatcher watcher = new ManagementEventWatcher();
        //    WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2");
        //    watcher.Query = query;
        //    watcher.Start();
        //    watcher.EventArrived += new EventArrivedEventHandler(HandleDriveMountation);
        //    watcher.WaitForNextEvent();
        //}

        //private void HandleDriveMountation(object sender, EventArrivedEventArgs e)   
        //{
        //    try
        //    {
        //        BuildDrives();
        //    }
        //    catch (Exception exception)
        //    { }
        //}
    }
}
