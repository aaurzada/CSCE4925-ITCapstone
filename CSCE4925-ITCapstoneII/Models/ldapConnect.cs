using System;
using System.Collections.Generic;
using System.Linq;
using Novell.Directory.Ldap;
using System.Web;

namespace SQLSolutions.Models
{
    public class ldapConnect
    {
        static void main(string[] args)
        {
            string ldapHost = args[0];
            int ldapPort = System.Convert.ToInt32(args[1]);
            String login = args[2];
            String password = args[3];
            String objectDN = args[3];
            String testPassword = args[4];

            try
            {
                // Creating an LdapConnection instance 
                LdapConnection ldapConn = new LdapConnection();

                //Connect function will create a socket connection to the server
                ldapConn.Connect(ldapHost, ldapPort);

                //Bind function with null user dn and password value will perform anonymous bind
                //to LDAP server 
                ldapConn.Bind(login, password);


                LdapAttribute attr = new LdapAttribute("employeeID", login);
                bool correct = ldapConn.Compare(objectDN, attr);
            }
            catch
            { 
                //display error
            }
        }
    }
}
       