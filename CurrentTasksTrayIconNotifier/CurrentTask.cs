using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CurrentTasksTrayIconNotifier
{
    public class CurrentTask: IProgressBarSource
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime ETA { get; set; }
        public DateTime Start { get; set; }
        public decimal Percent { get; set; }

        public int Order { get; set; }

        public decimal Progress { get { return Percent; } }
        public decimal Max { get { return 100M; } }

        private bool complete;

        internal void IncProgress(int v)
        {
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