using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CurrentTasksTrayIconNotifier
{
    class Runner
    {
        private Thread ThreadHandle;
        private IRunable Runable;

        public Runner(IRunable runable)
        {
            Runable = runable;
        }

        public void Execute()
        {
            ThreadHandle = new Thread(new ParameterizedThreadStart(Runner.ThreadMain));
            ThreadHandle.Start(Runable);
        }

        public static void ThreadMain(object param)
        {
            IRunable runable = (IRunable)param;
            runable.Run();
        }
    }
}
