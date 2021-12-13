using AutoMapper;

using FOREL.Extensions;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FOREL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += HandleException;
        }

        public void HandleException(Object sender, UnhandledExceptionEventArgs eventArgs)
        {
            if(eventArgs.ExceptionObject is Exception exception)
            {
                WindowExtensions.HandleException(exception, showStackTrace: true);
            }
            else
            {
                MessageBox.Show(eventArgs.ExceptionObject.ToString(), caption: "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
