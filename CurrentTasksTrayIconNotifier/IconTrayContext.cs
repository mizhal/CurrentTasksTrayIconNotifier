using CurrentTasksTrayIconNotifier.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CurrentTasksTrayIconNotifier
{
    internal class IconTrayContext : ApplicationContext
    {
        private CurrentTasksBackend Backend;
        private Runner Watchdog;
        private NotifyIcon Icon {get; set;}
        
        public IconTrayContext()
        {
            InitializeContext();
            Backend = new CurrentTasksBackend();
            var watchdog_process = new CurrentTasksWatchdog(Backend);

            watchdog_process.UpdateProgressEvent += UpdateUI;
            watchdog_process.ProgressCompleteEvent += ProgressCompleteNotification;

            Watchdog = new Runner(watchdog_process);
            Watchdog.Execute();
        }

        private Bars View()
        {
            return (Bars)MainForm;
        }

        private void InitializeContext()
        {
            var filename = "..\\..\\Process.ico";
            var component = new Container();
            Icon = new NotifyIcon()
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = new Icon(filename),
                Text = "Current tasks",
                Visible = true
            };
            Icon.ContextMenuStrip.Opening += MenuOpen;
            Icon.Click += OnClick;

            Icon.ContextMenuStrip.Items.Add(UITexts.Exit);
        }

        #region Event handlers
        private void ProgressCompleteNotification(object sender, EventArgs e)
        {
            ProgressCompleteEventArgs ev = (ProgressCompleteEventArgs)e;

            var task = Backend.GetById(ev.TaskId);
            var message = String.Format("La tarea '{0}' ha terminado", task.Name);
            NotificationManager.GetInstance().SendNotification(message);
        }

        private void UpdateUI(object sender, EventArgs args)
        {
            if (MainForm != null)
                MainForm.Invoke(new MethodInvoker(UpdateUI_delegate));
        }

        private void UpdateUI_delegate()
        {
            foreach (var task in Backend.GetTasks())
            {
                View().Update(task.Id, task);
            }
            foreach (var task in Backend.DeletedTasks())
            {
                View().Delete(task.Id, task);
            }
            View().UpdateHeight();
        }

        private void OnClick(object sender, EventArgs e)
        {
            MouseEventArgs ev = (MouseEventArgs)e;
            if (ev.Button == MouseButtons.Right)
                return;

            if (MainForm == null)
                MainForm = new Bars();

            var width = Screen.PrimaryScreen.WorkingArea.Width;
            var excess = Cursor.Position.X + MainForm.Width;

            MainForm.Show();
            MainForm.Activate();
            MainForm.BringToFront();
            MainForm.Top = Screen.PrimaryScreen.WorkingArea.Top;
            MainForm.Left = (excess < width) ? Cursor.Position.X : width - MainForm.Width;
        }

        private void MenuOpen(object sender, CancelEventArgs e)
        {
            e.Cancel = false;

            
        }
        #endregion
    }
}