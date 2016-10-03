using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using TaxiApp.Core.Repository;
using TaxiApp.Core.Messages;
using GalaSoft.MvvmLight.Messaging;
using TaxiApp.Core.Entities;

namespace TaxiApp.Core.DataModel
{
    public class UserInfo
    {
        //public string Name { get; set; }
        //public string Surname {get; set;}
        //public string Lastname {get; set;}
        //public string Email { get; set; }


        private UserRepository _userRepository = null;


        public UserInfo(UserRepository userRepository)
        {
            this._userRepository = userRepository;

            //this.Name = string.Empty;
            //this.Surname = string.Empty;
            //this.Lastname = string.Empty;
            //this.Email = string.Empty;

        }


    }
}
