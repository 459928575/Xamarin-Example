using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMTest.ViewModels {
    public class ControlViewModel : Screen {

        public void T() {
            App.Current.MainPage.DisplayAlert("ttt", "ttt", "ok");
        }

    }
}
