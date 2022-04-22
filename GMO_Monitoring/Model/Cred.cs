using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace GMO_Monitoring.Model
{
    public class Cred
    {
        private SecureString Pass_ss_Var;

        public SecureString Pass_ss_Property
        {
            get { return Pass_ss_Var; }
            set { Pass_ss_Var = value; }
        }


    }
}
