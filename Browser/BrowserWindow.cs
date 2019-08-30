﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Browser.Core;

namespace Browser
{
    public class BrowserWindow : NativeWindow
    {
        public enum WindowsMessage
        {
            WM_NCLBUTTONDOWN = 0x00A1,
            WM_MOUSEACTIVATE = 0x0021,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEHWHEEL = 0x020E,
            WM_MOUSELEAVE = 0x02A3,
            WM_ACTIVATE = 0x0006
        }

        public BrowserWindow(Control browser, IntPtr handle)
        {
            AssignHandle(handle);
            browser.HandleDestroyed += BrowserOnHandleDestroyed;
        }

        public bool BlockUserInput { get; set; }

        private void BrowserOnHandleDestroyed(object sender, EventArgs e)
        {
            ReleaseHandle();
            ((Control)sender).HandleDestroyed -= BrowserOnHandleDestroyed;
        }
        protected override void WndProc(ref Message m)
        {
            var allow = OnInput(m);
            if (allow) base.WndProc(ref m);
        }

        private bool OnInput(Message m)
        {
            if (BlockUserInput)
            {
                var msg = m.Msg;
                if (msg > (int)WindowsMessage.WM_NCLBUTTONDOWN)
                {
                    if (msg - (int)WindowsMessage.WM_MOUSEMOVE <= 10) return false;
                    if (msg == (int)WindowsMessage.WM_MOUSEHWHEEL) return false;
                    if (msg != (int)WindowsMessage.WM_MOUSELEAVE)
                    {
                        return true;
                    }
                }
                else if (msg != (int)WindowsMessage.WM_MOUSEACTIVATE)
                {
                    if (msg != (int)WindowsMessage.WM_NCLBUTTONDOWN)
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

    }
}
