using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dict.utils
{
    using System.Drawing;
    using System.Windows.Forms;

    public static class WindowPositionHelper
    {
        /// <summary>
        /// 检查并修正窗口位置，确保窗口显示在屏幕内
        /// </summary>
        /// <param name="form">目标窗体</param>
        /// <param name="savedLocation">保存的位置</param>
        /// <param name="savedSize">保存的大小</param>
        public static void EnsureVisible(Form form, Point savedLocation, Size savedSize)
        {
            // 获取所有屏幕
            Screen[] screens = Screen.AllScreens;
            bool isInAnyScreen = false;

            // 检查保存的位置是否在任何一个屏幕的工作区内
            foreach (var screen in screens)
            {
                Rectangle workingArea = screen.WorkingArea;
                // 只要窗口的左上角在某个屏幕的工作区内，或者窗口的一部分在屏幕内，就算有效
                // 这里我们做一个稍微宽松的检查：只要中心点在屏幕内即可，或者左上角在屏幕内
                if (
                    workingArea.Contains(savedLocation)
                    || workingArea.Contains(
                        new Point(
                            savedLocation.X + savedSize.Width / 2,
                            savedLocation.Y + savedSize.Height / 2
                        )
                    )
                )
                {
                    isInAnyScreen = true;
                    break;
                }
            }

            if (isInAnyScreen)
            {
                // 位置有效，应用保存的位置和大小
                form.StartPosition = FormStartPosition.Manual;
                form.Location = savedLocation;
                form.Size = savedSize;
            }
            else
            {
                // 位置无效（例如上次是在断开的副屏上），则居中显示
                form.StartPosition = FormStartPosition.CenterScreen;
                // 即使位置无效，我们通常还是想保留用户设置的大小
                form.Size = savedSize;
            }
        }
    }
}
