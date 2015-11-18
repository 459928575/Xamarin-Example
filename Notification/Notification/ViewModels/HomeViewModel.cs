using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Notification.ViewModels {
    public class HomeViewModel : Screen {

        public MasterViewModel MasterPage { get; set; }

        public NavigationPage DetailPage { get; set; }

        public string T { get; set; }

        public HomeViewModel(SimpleContainer container) {
            this.MasterPage = container.GetInstance<MasterViewModel>();
            this.DetailPage = new NavigationPage();
            this.T = "BBB";
            this.NotifyOfPropertyChange(() => this.MasterPage);
            this.NotifyOfPropertyChange(() => this.DetailPage);
            this.NotifyOfPropertyChange(() => this.T);
        }

    }
}
