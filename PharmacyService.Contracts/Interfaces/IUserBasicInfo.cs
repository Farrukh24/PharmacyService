using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Interfaces
{
    public interface IUserBasicInfo
    {
        string Id { get; set; }
        string UserName { get; set; }
    }
}
