using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageManager
{
    public interface ISendEmail
    {
        Task SendMail(string mailbody, EnumEmailType emailType);
    }
}
