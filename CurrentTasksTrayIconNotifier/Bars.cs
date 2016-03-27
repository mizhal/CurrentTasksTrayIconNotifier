using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CurrentTasksTrayIconNotifier
{
    public partial class Bars : Form
    {
        #region Constants and definitions

        public const int PROGRESS_BAR_HEIGHT = 20;
        public const int MAX_LABEL_WIDTH = 300;
        public const int MTOP = 5;
        public const int MBOTTOM = 5;
        public Padding PADDING = new Padding(5);

        #endregion

        #region Data

        private int ContentHeight { get; set; }
        private int RowIndex { get; set; }
        private ICollection<ProgressBar> progressBars;
        private Dictionary<string, ProgressBar> index;

        #endregion

        public Bars()
        {
            this.progressBars = new List<ProgressBar>();
            this.index = new Dictionary<string, ProgressBar>();

            this.FormClosing += Bars_Closing;

            InitializeComponent();

            table.ColumnCount = 2;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }
        
        #region UI Manipulation

        public void Clear()
        {
            foreach (var pb in progressBars)
            {
                Controls.Remove(pb);
            }
            progressBars.Clear();
            index.Clear();
            ContentHeight = 0;
        }

        public void Update(string id, CurrentTask task)
        {
            if (index.ContainsKey(id))
            {
                var pg = index[id];
                pg.Value = (int) Math.Ceiling(task.Progress);
         
            } else
            {
                Add(id, task);
            }
        }

        public void Delete(string id, CurrentTask task)
        {
            var pg = index[id];
            Controls.Remove(pg);
        }

        public void Add(string name, IProgressBarSource data_source)
        {
            var progress_bar = new ProgressBar();
            
            progress_bar.Name = name;
            progress_bar.Height = PROGRESS_BAR_HEIGHT;
            progress_bar.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            progress_bar.TabIndex = RowIndex;
            progress_bar.MarqueeAnimationSpeed = 5000;
            IncContentHeight();

            table.Controls.Add(progress_bar, 1, RowIndex);
            table.Controls.Add(new Label() {
                Text = data_source.Name,
                Padding = PADDING,
                AutoSize = true,
                MaximumSize = new Size(MAX_LABEL_WIDTH, PROGRESS_BAR_HEIGHT + PADDING.Top)
            }, 0, RowIndex);
            RowIndex++;
            table.Height = ContentHeight;

            index[name] = progress_bar;
        }

        public void Remove(string name)
        {

        }

        private void IncContentHeight()
        {
            ContentHeight += PROGRESS_BAR_HEIGHT + MTOP + MBOTTOM;
        }

        private void DecHeight()
        {
            ContentHeight -= PROGRESS_BAR_HEIGHT + MTOP + MBOTTOM;
        }

        public void UpdateHeight()
        {
            ClientSize = new Size(ClientSize.Width, ContentHeight);
        }

        #endregion

        #region Events
        private void Bars_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            ((Bars)sender).Hide();
        }
        #endregion
    }
}
