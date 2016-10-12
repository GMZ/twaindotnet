using System;
using System.Windows;
using System.Windows.Interop;

namespace TwainDotNet.Wpf
{
    /// <summary>
    /// A windows message hook for WPF applications.
    /// </summary>
    public class WpfWindowMessageHook : IWindowsMessageHook
    {
        readonly HwndSource _source;
        readonly WindowInteropHelper _interopHelper;
        bool _usingFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfWindowMessageHook"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        public WpfWindowMessageHook(Window window)
        {
            _source = (HwndSource)PresentationSource.FromDependencyObject(window);
            _interopHelper = new WindowInteropHelper(window);            
        }

        /// <summary>
        /// Filters the message.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <param name="handled">if set to <c>true</c> [handled].</param>
        /// <returns></returns>
        public IntPtr FilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (FilterMessageCallback != null)
            {
                return FilterMessageCallback(hwnd, msg, wParam, lParam, ref handled);
            }

            return IntPtr.Zero;
        }

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
                if (!_usingFilter && value == true)
                {
                    _source.AddHook(FilterMessage);
                    _usingFilter = true;
                }

                if (_usingFilter && value == false)
                {
                    _source.RemoveHook(FilterMessage);
                    _usingFilter = false;
                }
            }
        }

        /// <summary>
        /// The delegate to call back then the filter is in place and a message arrives.
        /// </summary>
        public FilterMessage FilterMessageCallback { get; set; }

        /// <summary>
        /// The handle to the window that is performing the scanning.
        /// </summary>
        public IntPtr WindowHandle { get { return _interopHelper.Handle; } }
    }
}
