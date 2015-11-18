using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Caliburn.Micro;
using Windows.UI.Core;
using System.Reflection;

namespace Notification.UWP {

    public sealed partial class App {
        private WinRTContainer _container;
        private IEventAggregator _eventAggregator;

        public App() {
            InitializeComponent();
        }

        protected override void Configure() {
            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

            //_container.PerRequest<ShellViewModel>();

            _eventAggregator = _container.GetInstance<IEventAggregator>();
        }


        protected override IEnumerable<Assembly> SelectAssemblies() {
            return new[]
            {
                GetType().GetTypeInfo().Assembly,
                typeof (Notification.App).GetTypeInfo().Assembly
            };
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args) {
            Xamarin.Forms.Forms.Init(args); // requires the `e` parameter

            //this.DisplayRootViewFor<ShellViewModel>();
            this.DisplayRootView<MainPage>();
            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                //_eventAggregator.PublishOnUIThread(new ResumeStateMessage());
            }
        }

        protected override void OnSuspending(object sender, SuspendingEventArgs e) {
            //_eventAggregator.PublishOnUIThread(new SuspendStateMessage(e.SuspendingOperation));
        }

        protected override object GetInstance(Type service, string key) {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance) {
            _container.BuildUp(instance);
        }
    }
}
