using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace com.akoimeexx.utilities.AwayFarmingK {
    /**
     * This project is a one-off from my main afk program, intended to work 
     * around changes in Minecraft 1.13 that prevent users from auto-clicking 
     * the right mouse button while the window is inactive during afk sessions.
     * 
     * Normally I keep things a lot less cluttered, but this is ripped from an 
     * unfinished project to use as a stand-alone application.
     * 
     * NOTES:
     * Java system api calls howto: http://www.rgagnon.com/javadetails/java-0189.html
     */

    /// <summary>
    /// 
    /// </summary>
    public static class MinecraftHook {
        public enum MouseActions : int {
            RightButton_Down = 0x204,
            RightButton_Up = 0x205,
        }
        public enum WindowMessage {
            /// <summary>
            /// Hides the window and activates another window.
            /// </summary>
            Hide = 0,
            /// <summary>
            /// Displays the window in its current size and position. This value is 
            /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
            /// window is not activated.
            /// </summary>
            ShowNA = 8,
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport(
            "user32.dll",
            EntryPoint="PostMessageA",
            SetLastError=true
        )]
        private static extern bool PostMessage(
            IntPtr hwnd, 
            uint Msg, 
            IntPtr wParam, 
            IntPtr lParam
        );
        [DllImport("user32.dll")]
        private static extern bool PrintWindow(
            IntPtr hWnd, 
            IntPtr hdcBlt, 
            int nFlags
        );
        [DllImport(
            "user32.dll", 
            EntryPoint="ShowWindow", 
            SetLastError=true
        )]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        //public static Bitmap PrintWindow(IntPtr hwnd)    
        //{       
        //    RECT rc;        
        //    GetWindowRect(hwnd, out rc);

        //    Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);        
        //    Graphics gfxBmp = Graphics.FromImage(bmp);        
        //    IntPtr hdcBitmap = gfxBmp.GetHdc();        

        //    PrintWindow(hwnd, hdcBitmap, 0);  

        //    gfxBmp.ReleaseHdc(hdcBitmap);               
        //    gfxBmp.Dispose(); 

        //    return bmp;   
        //}

        //public static object GetInstanceScreenshot(IntPtr handle) {
        //    object o = default(object);
        //    try {
        //        RECT region = default(RECT);
        //        GetWindowRect(handle, out region);
        //        BitmapSource bmp = BitmapSource.Create(
        //            region.Right - region.Left,
        //            region.Bottom - region.Top, 
        //            96, 
        //            96, 
        //            System.Windows.Media.PixelFormats.Default 

        //        );
        //    } catch (Exception e) { Console.Error.WriteLineAsync(e.Message); }
        //    return o;
        //}

        public static IEnumerable<Process> GetMinecraftInstances() {
            IEnumerable<Process> i = default(IEnumerable<Process>);
            try {
                i = from p in Process.GetProcesses()
                    where p.MainWindowHandle != IntPtr.Zero
                    where p.ProcessName == "javaw"
                    where p.MainWindowTitle.Contains("Minecraft")
                    select p;
            } catch (Exception e) { Console.Error.WriteLineAsync(e.Message); }
            return i;
        }

        public static bool SendMouseEvent(
            IntPtr handle,
            MouseActions action
        ) {
            bool b = default(bool);
            try {
                b = PostMessage(
                    handle, 
                    (uint)action, 
                    (IntPtr)0x1, 
                    (IntPtr)((0 << 16) | (0 & 0xffff))
                );
            } catch (Exception e) { Console.Error.WriteLineAsync(e.Message); }
            return b;
        }

        public static bool SendWindowMessage(
            IntPtr handle, 
            WindowMessage command
        ) {
            bool b = default(bool);
            try {
                if (!handle.Equals(IntPtr.Zero))
                    b = ShowWindow(handle, (int)command);
            } catch (Exception e) { Console.Error.WriteLineAsync(e.Message); }
            return b;
        }

    }
}
