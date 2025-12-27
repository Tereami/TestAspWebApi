using System;
using System.Collections.Generic;

namespace TestAspWebApi.Models
{
    public class CabinetViewModel
    {
        public string UserName { get; set; }

        public DateTime RegisteredAt { get; set; }

        public List<UserSessionViewModel> Sessions { get; set; }
    }
}
