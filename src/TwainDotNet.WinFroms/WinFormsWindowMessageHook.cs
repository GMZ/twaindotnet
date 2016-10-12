using System;
using System.Windows.Forms;

namespace TwainDotNet.WinFroms
{
    /// <summary>
    /// A windows message hook for WinForms applications.
    /// </summary>
    public class WinFormsWindowMessageHook : IWindowsMessageHook, IMessageFilter
    {
        readonly IntPtr _windowHandle;
        bool _usingFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinFormsWindowMessageHook"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        public WinFormsWindowMessageHook(IWin32Window window)
        {
            _windowHandle = window.Handle;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (FilterMessageCallback != null)
            {
                bool handled = false;
                FilterMessageCallback(m.HWnd, m.Msg, m.WParam, m.LParam, ref handled);
                return handled;
            }

            return false;
        }

        /// <summary>
        /// The handle to the window that is performing the scanning.
        /// </summary>
        public IntPtr WindowHandle { get { return _windowHandle; } }

        /// <summary>
        /// Gets or sets if the message filter is in use.
        /// </summary>
        public bool UseFilter
        {
            get
            {
                return _usingFilter;
            }
            set
            {
                if (!_usingFilter && value)
                {
                    Application.AddMessageFilter(this);
                    _usingFilter = true;
                }

                if (_usingFilter && value == false)
                {
                    Application.RemoveMessageFilter(this);
                    _usingFilter = false;
                }
            }
        }

        /// <summary>
        /// The delegate to call back then the filter is in place and a message arrives.
        /// </summary>
        public FilterMessage FilterMessageCallback { get; set; }        
    }
}
