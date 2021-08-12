using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H5ServersideProgrammering.Codes
{
    public class CrytpExample
    {
        public string Encrypt(string payload, IDataProtector _protector)
        {
            return _protector.Protect(payload);
        }

        public string Decrypt(string protectedPayload, IDataProtector _protector)
        {
            return _protector.Unprotect(protectedPayload);
        }
    }
}
