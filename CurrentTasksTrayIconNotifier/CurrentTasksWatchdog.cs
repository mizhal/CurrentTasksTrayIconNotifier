using System;
using System.Threading;

namespace CurrentTasksTrayIconNotifier
{
    internal class CurrentTasksWatchdog : IRunable
    {
        private CurrentTasksBackend Backend;

        public event EventHandler UpdateProgressEvent;
        public event EventHandler ProgressCompleteEvent;

        public CurrentTasksWatchdog(CurrentTasksBackend backend)
        {
            Backend = backend;
        }

        public void Run()
        {
            var die = new Random();
            while (true)
            {
                foreach(var task in Backend.GetTasks())
                {
                    task.IncProgress(die.Next(1, 10));
                    UpdateProgress();
                    if (task.IsComplete())
                        ProgressComplete(task);
                    Thread.Sleep(300);
                }
            }
        }

        public void SetParam(string name, object value)
        {
        }

        #region Event notification

        private void UpdateProgress()
        {
            if(UpdateProgressEvent != null)
            {
                UpdateProgressEvent(this, null);
            }
        }

        private void ProgressComplete(CurrentTask task)
        {
            if(ProgressCompleteEvent != null)
            {
                ProgressCompleteEvent(this, new ProgressCompleteEventArgs() {
                    TaskId = task.Id
                });
            }
        }

        #endregion
    }

    public class ProgressCompleteEventArgs : EventArgs
    {
        public string TaskId { get; set; }
    }
}