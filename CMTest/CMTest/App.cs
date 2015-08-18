using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using CMTest.ViewModels;
using CMTest.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CMTest {
    public class App : FormsApplication {

        private SimpleContainer Container = null;

        public App(SimpleContainer container) {

            this.Container = container;

            this.Container
                .PerRequest<LoginViewModel>()
                .Singleton<UserInfoViewModel>();
            //.PerRequest<UserInfoViewModel>();

            this.DisplayRootView<LoginView>();
        }

        protected override void PrepareViewFirst(NavigationPage navigationPage) {
            this.Container.Instance<INavigationService>(new NavigationPageAdapter(navigationPage));
        }
    }
}
