using Caliburn.Micro;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMTest.ViewModels {
    public class UserInfoViewModel : Screen {

        private string _account = "";
        public string Account {
            get {
                return this._account;
            }
            set {
                this._account = value;
                this.NotifyOfPropertyChange(() => this._account);
            }
        }

        public DateTime LoginedOn {
            get;
            set;
        }

        public UserInfoViewModel() {
        
        }
    }
}
