using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FOREL.Extensions
{
    static class WindowExtensions
    {
        public static void HandleException(Exception ex, Boolean showStackTrace = true)
        {
            String text = showStackTrace ? ex.ToString() : ex.Message;
            MessageBox.Show(text, caption: "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
