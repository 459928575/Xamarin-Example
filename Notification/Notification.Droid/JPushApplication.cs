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

[assembly: Permission(Name = JPushApplication.JPUSH_MESSAGE_PERMISSION, ProtectionLevel = Protection.Signature)]
[assembly: UsesPermission(Name = JPushApplication.JPUSH_MESSAGE_PERMISSION)]
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
    public class JPushApplication : Application {
        //notification.Droid must same as Package Name
        public const string JPUSH_MESSAGE_PERMISSION = "notification.Droid.permission.JPUSH_MESSAGE";


        public JPushApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) {
        }
    }
}