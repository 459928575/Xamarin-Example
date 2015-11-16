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

//[assembly: Permission(Name = "Notification.Test.permission.JPUSH_MESSAGE", ProtectionLevel = Protection.Signature)]
//[assembly: UsesPermission(Name = "Notification.Test.permission.JPUSH_MESSAGE")]
//[assembly: UsesPermission(Name = Android.Manifest.Permission.Internet)]
//[assembly: UsesPermission(Name = Android.Manifest.Permission.WakeLock)]
namespace Notification.Droid {

    //[Application]
    //[MetaData("JPUSH_CHANNEL", Value = "developer-default")]
    //[MetaData("JPUSH_APPKEY", Value = "02d25228d7321a1092991455")]
    public class ApplicationWithJPush : Application {
        public ApplicationWithJPush() {
        }
    }
}