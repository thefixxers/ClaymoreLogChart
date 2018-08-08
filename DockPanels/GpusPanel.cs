using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BrightIdeasSoftware;
using WeifenLuo.WinFormsUI.Docking;
using Appccelerate.EventBroker;
using Appccelerate.Events;
using Appccelerate.EventBroker.Extensions;
using Appccelerate.EventBroker.Handlers;


using ClaymoreLogChart.DataModel;
using ClaymoreLogChart.VisualStylers;

namespace ClaymoreLogChart.DockPanels
{
    public partial class GpusPanel : DockContent
    {
        public GpusPanel()
        {
            InitializeComponent();

            BackColor = ThemeColors.WindowContentBackgroundDarkTheme;
            ForeColor = ThemeColors.WindowContentForegroundDarkTheme;

            //Jus Some Color Games
            ObjectListGpus.BackColor = BackColor;
            ObjectListGpus.ForeColor = ForeColor;
            ObjectListGpus.SelectedBackColor = ThemeColors.UnfocusedSelectedBackgroundColor;
            ObjectListGpus.UnfocusedSelectedBackColor = ThemeColors.UnfocusedSelectedBackgroundColor;
            ObjectListGpus.UnfocusedSelectedForeColor = ObjectListGpus.ForeColor;

            //Setting ObjectListView Aspect Handlers
            ColumnColor.AspectToStringConverter = (obj) => { return " "; };
            ColumnColor.RendererDelegate = ColorRender;
            ColumnName.AspectGetter = GpuNameGetter;
            ColumnName.AspectPutter = delegate (object gpu, object newValue) { GpuData gData = gpu as GpuData; gData.NickName = newValue.ToString(); GpuRenamed?.Invoke(this, new EventArgs<int>(gData.Index));  };

            //Wiring Events
            ObjectListGpus.ItemDrag += ObjectListGpus_ItemDrag;
            ObjectListGpus.GiveFeedback += ObjectListGpus_GiveFeedback;
            ObjectListGpus.SelectionChanged += ObjectListGpus_SelectionChanged;
            ObjectListGpus.MouseClick += ObjectListGpus_MouseClick;
        }

        private void ObjectListGpus_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Right)
            {
                GpuData clickeGpu = ObjectListGpus.SelectedObject as GpuData;
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                menu.Renderer = new MenuStripCustomRenderer();
                menu.Items.Add(new ToolStripMenuItem("Set Nickname", null, (s, ea) => { ObjectListGpus.EditSubItem(ObjectListGpus.SelectedItem, 1); }, "Rename"));
                Point cursorPos = System.Windows.Forms.Cursor.Position;
                Point menuPos = new Point(cursorPos.X - menu.Width / 2, cursorPos.Y);
                menu.Show(menuPos);
            }
        }

        private void ObjectListGpus_SelectionChanged(object sender, EventArgs e)
        {
            GpuData gpu = (GpuData) ObjectListGpus.SelectedObject;
            if(gpu!=null)
                GpuSelected?.Invoke(this, new EventArgs<int>(gpu.Index));
        }

        private void ObjectListGpus_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
            Cursor.Current = CursorManager.Instance.Current;
        }

        private void ObjectListGpus_ItemDrag(object sender, ItemDragEventArgs e)
        {
            GpuData gpu;
            List<GpuData> draggedGpus = new List<DataModel.GpuData>();
            string dragCursorText = "";

            foreach(OLVListItem item in ObjectListGpus.SelectedItems)
            {
                gpu = item.RowObject as GpuData;
                draggedGpus.Add(gpu);
                dragCursorText += GpuNameGetter(gpu) + "\n";
            }

            CursorManager.Instance.MakeTextCursor(dragCursorText, this.Font);
            ObjectListGpus.DoDragDrop(draggedGpus, DragDropEffects.Link | DragDropEffects.Scroll);
        }

        private string GpuNameGetter(object rowObject)
        {
            GpuData gpu = rowObject as GpuData;
            if(gpu.NickName!=null && gpu.NickName != "" && gpu.NickName != string.Empty)
                return gpu.NickName;
            return gpu.Name;
        }
        private bool ColorRender(EventArgs e, Graphics g, Rectangle r, Object rowObject)
        {
            GpuData gpu = rowObject as GpuData;
            Color color = ThemeColors.PresetColors[gpu.Index];

            if (ObjectListGpus.SelectedObject == rowObject)
                g.Clear(ObjectListGpus.Focused ? ObjectListGpus.SelectedBackColor : ObjectListGpus.UnfocusedSelectedBackColor);
            else
                g.Clear(ObjectListGpus.BackColor);

            g.FillRectangle(new SolidBrush(color), r.X + 2, r.Y + 2, r.Width - 5, r.Height - 5);

            return true;
        }

        public void SetObjects(List<GpuData> gpuObjects)
        {
            ObjectListGpus.ClearObjects();
            ObjectListGpus.AddObjects(gpuObjects);
            ObjectListGpus.Refresh();
            ObjectListGpus.Update();
        }

        #region -- Event Publications --
        [EventPublication("topic://Gpu/Selected")]
        public event EventHandler<EventArgs<int>> GpuSelected;
        [EventPublication("topic://Gpu/Renamed")]
        public event EventHandler<EventArgs<int>> GpuRenamed;
        #endregion

        #region -- Event Subscriptions --
        [EventSubscription("topic://Gpu/Renamed", typeof(OnUserInterface))]
        public void GpuSelectedHandler(object sender, EventArgs<int> ea)
        {
            if (sender == this)
                return;

            ObjectListGpus.UpdateObject(Globals.GPUs[ea.Value]);
        }
        #endregion
    }
}
