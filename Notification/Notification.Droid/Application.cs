using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Notification.Droid;
using Caliburn.Micro;
using System.Reflection;

[assembly: Permission(Name = Notification.Droid.Application.JPUSH_MESSAGE_PERMISSION, ProtectionLevel = Protection.Signature)]
[assembly: UsesPermission(Name = Notification.Droid.Application.JPUSH_MESSAGE_PERMISSION)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.WakeLock)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.Vibrate)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.ReadPhoneState)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.ReadExternalStorage)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.MountUnmountFilesystems)]
[assembly: UsesPermission(Name = Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Name = "android.permission.RECEIVE_USER_PRESENT")]
[assembly: UsesPermission(Name = Android.Manifest.Permission.WriteSettings)]
namespace Notification.Droid {

    [Application]
    [MetaData("JPUSH_CHANNEL", Value = "developer-default")]
    [MetaData("JPUSH_APPKEY", Value = "02d25228d7321a1092991455")]
    public class Application : CaliburnApplication {
        //notification.Droid must same as Package Name
        public const string JPUSH_MESSAGE_PERMISSION = "notification.Droid.permission.JPUSH_MESSAGE";


        private SimpleContainer container;

        public Application(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) {
        }

        public override void OnCreate() {
            base.OnCreate();

            Initialize();
        }

        protected override void Configure() {
            container = new SimpleContainer();
            container.Instance(container);
        }

        protected override IEnumerable<Assembly> SelectAssemblies() {
            return new[]
            {
                GetType().Assembly,
                typeof (App).Assembly
            };
        }

        protected override void BuildUp(object instance) {
            container.BuildUp(instance);
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return container.GetAllInstances(service);
        }

        protected override object GetInstance(Type service, string key) {
            return container.GetInstance(service, key);
        }
    }
}