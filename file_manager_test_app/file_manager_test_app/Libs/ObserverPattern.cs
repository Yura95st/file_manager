using System;

namespace file_manager_test_app.Libs.ObserverPattern
{
    public interface IObserver
    {
        void Update(int notificationCode);
    }

    public interface ISubject
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers(int notificationCode);
    }
}
