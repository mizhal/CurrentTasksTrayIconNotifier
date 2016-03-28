using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CurrentTasksTrayIconNotifier
{
    public enum CurrentTaskType { MANUAL, TIMER, SPEED };

    public class CurrentTask: IProgressBarSource
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime ETA { get; set; }
        public DateTime Start { get; set; }
        public decimal Percent { get; set; }
        public decimal SpeedProgressBySecond { get; set; }

        private CurrentTaskType TaskType { get; set; }

        public int Order { get; set; }

        public decimal Progress {
            get {
                decimal raw_value = 0;
                switch (TaskType)
                {
                    case CurrentTaskType.MANUAL:
                        raw_value = Percent;
                        break;
                    case CurrentTaskType.SPEED:
                        raw_value = (decimal) (DateTime.Now - Start).TotalSeconds * SpeedProgressBySecond;
                        break;
                    case CurrentTaskType.TIMER:
                        var now = DateTime.Now;
                        var div = (ETA - Start).TotalSeconds;
                        if(div != 0)
                            raw_value = 100M * (decimal)( (now - Start).TotalSeconds / div );
                        break;
                    default:
                        raw_value = 0M;
                        break;
                }

                return (raw_value > 100) ? 100 : raw_value;
            }
        }
        public decimal Max { get { return 100M; } }

        private bool complete;

        public CurrentTask()
        {
            TaskType = CurrentTaskType.MANUAL;
        }

        internal void IncProgress(int v)
        {
            if (TaskType != CurrentTaskType.MANUAL)
                throw new Exception("Non-manual tasks cannot be updated programmatically");

            if (Percent < 100 && Percent + v > 100)
                complete = true;

            Percent = (Percent + v > 100) ? 100 : Percent + v;
        }

        internal bool IsComplete()
        {
            var tmp = complete;
            complete = false;
            return tmp;
        }
    }
}