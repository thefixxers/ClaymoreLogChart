
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;

namespace ClaymoreLogChart.VisualStylers
{
    class MenuStripCustomRenderer : ToolStripRenderer
    {
        protected override void InitializeItem(ToolStripItem item)
        {
            base.InitializeItem(item);
            item.ForeColor = ThemeColors.ToolbarStripForegroundColor;
        }
        protected override void Initialize(ToolStrip toolStrip)
        {
            base.Initialize(toolStrip);
            toolStrip.ForeColor = ThemeColors.ToolbarStripForegroundColor;
        }
        protected override void InitializeContentPanel(ToolStripContentPanel contentPanel)
        {
            base.InitializeContentPanel(contentPanel);
            contentPanel.ForeColor = ThemeColors.ToolbarStripForegroundColor;
        }
        protected override void InitializePanel(ToolStripPanel toolStripPanel)
        {
            base.InitializePanel(toolStripPanel);
            toolStripPanel.ForeColor = ThemeColors.ToolbarStripForegroundColor;
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBackground(e);

            if (e.ToolStrip.IsDropDown)
            {
                using (Brush brush = new SolidBrush(ThemeColors.ToolbarStripWindowColor))
                {
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
                }
            }
        }
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderMenuItemBackground(e);
            if (e.Item.Enabled)
            {
                //Item selected but not opened
                if (e.Item.IsOnDropDown == false && e.Item.Selected)
                    using (Brush b = new SolidBrush(ThemeColors.UnfocusedSelectedBackgroundColor))
                    {
                        e.Graphics.FillRectangle(b, e.Item.ContentRectangle);
                        e.Item.ForeColor = ThemeColors.ToolbarStripForegroundColor;
                    }
                //Item selected and opened
                else if (e.Item.IsOnDropDown && e.Item.Selected)
                    using (Brush b = new SolidBrush(ThemeColors.UnfocusedSelectedBackgroundColor))
                    {
                        e.Graphics.FillRectangle(b, e.Item.ContentRectangle);
                        e.Item.ForeColor = ThemeColors.ToolbarStripForegroundColor;
                    }
                // If item is MenuHeader and menu is dropped down; 
                // selection rectangle is now darker
                if ((e.Item as ToolStripMenuItem).DropDown.Visible && e.Item.IsOnDropDown == false)
                {
                    Rectangle rect = new Rectangle(0, 0, e.Item.Width, e.Item.Height);
                    using (Brush b = new SolidBrush(ThemeColors.ToolbarStripWindowColor))
                    {
                        e.Graphics.FillRectangle(b, rect);
                    }
                    using (Pen pen = new Pen(ThemeColors.ToolbarStripBorderColor))
                    {
                        e.Graphics.DrawLine(pen, 0, 0, 0, e.Item.Height);
                        e.Graphics.DrawLine(pen, 0, 0, e.Item.Width, 0);
                        e.Graphics.DrawLine(pen, e.Item.Width - 1, 0, e.Item.Width - 1, e.Item.Height);
                    }
                    e.Item.ForeColor = ThemeColors.ToolbarStripForegroundColor;
                }
            }
        }
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);
            using (Pen pen = new Pen(ThemeColors.ToolbarStripBorderColor))
                e.Graphics.DrawLine(pen, e.Item.ContentRectangle.X + 30, e.Item.ContentRectangle.Y + 1, e.Item.ContentRectangle.Right, e.Item.ContentRectangle.Y + 1);
        }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            base.OnRenderToolStripBorder(e);
            if (e.ToolStrip.IsDropDown)
            {
                Rectangle rect = new Rectangle(e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 2, e.AffectedBounds.Height - 2);
                using (Pen pen = new Pen(ThemeColors.ToolbarStripBorderColor))
                    e.Graphics.DrawRectangle(pen, rect);
                using (Brush brush = new SolidBrush(ThemeColors.ToolbarStripWindowColor))
                    e.Graphics.FillRectangle(brush, e.ConnectedArea);
            }
        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.Item.ForeColor = e.Item.Enabled ? ThemeColors.ToolbarStripForegroundColor : ThemeColors.ToolbarStripDisabledColor;
            base.OnRenderItemText(e);
        }
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            using (Brush brush = new SolidBrush(ThemeColors.ToolbarStripBackgroundColor))
                e.Graphics.FillRectangle(brush, e.ImageRectangle);

            ImageAttributes attrib = new ImageAttributes();
            attrib.SetColorMatrix(imageNegateMatrix);
            e.Graphics.DrawImage(e.Image, e.ImageRectangle, 0, 0, e.Image.Width, e.Image.Height, GraphicsUnit.Pixel, attrib);
        }


        #region-- Matrix to negate the colors of "check icon"
        protected static ColorMatrix imageNegateMatrix = new ColorMatrix(
            new float[][]
            {
                new float[]{ -1,  0,  0,  0,  0},
                new float[]{  0, -1,  0,  0,  0},
                new float[]{  0,  0, -1,  0,  0},
                new float[]{  0,  0,  0,  1,  0},
                new float[]{  1,  1,  1,  0,  1},
            }
            );
        #endregion
    }
}
