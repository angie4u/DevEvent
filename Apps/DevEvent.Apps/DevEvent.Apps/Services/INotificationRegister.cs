using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Apps
{
    public interface INotificationRegister
    {
        Task Register(string tag);
    }
}
