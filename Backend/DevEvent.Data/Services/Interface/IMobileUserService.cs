using DevEvent.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Services
{
    public interface IMobileUserService
    {
        string AddMobileUser(string sid, string provider);

        MobileUser GetMobileUser(string sid);
    }
}
