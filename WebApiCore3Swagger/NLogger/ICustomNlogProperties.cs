using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.NLogger
{
    public interface ICustomNlogProperties
    {
        void LogProperty(CustomProperty property);
    }
}
