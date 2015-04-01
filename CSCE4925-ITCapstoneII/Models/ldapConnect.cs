using System;
using System.Net;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.Permissions;
using System.Web.UI;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace ConnectLDAP
{
    [DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]

    public class LDAPConnect
    {
        // static variables used throughout the example
        static LdapConnection ldapConnection;
        static string ldapServer;
        static NetworkCredential credential;
        private const int LDAPError_InvalidCredentials = 0x31;
        static SecureString pass = new SecureString();

        public bool useLDAP(string userDN, string password)
        {
            try
            {
                GetParameters(userDN, password);  // Get the Command Line parameters
                
                // Create the new LDAP connection
                ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(ldapServer, true, false));
                LdapSessionOptions options = ldapConnection.SessionOptions;
                

                ldapConnection.Credential = new NetworkCredential(userDN, password); //save username and password of user which will be bound later
                ldapConnection.AuthType = AuthType.Basic;
                ldapConnection.SessionOptions.ProtocolVersion = 3; //change protocol to match unt ldap protocol version 3
                ldapConnection.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback((con, cer) => true);
                try //will only work if protocol version is 3
                {
                   
                    ldapConnection.SessionOptions.SecureSocketLayer = true; //requires ssl to connect to ldap
                }
                catch
                {
                    throw; //throw exception if startTransportLayerSecurity fails
                }
                try
                {
                    ldapConnection.Bind(); //bind with NetworkCredentials 
                    //bind succeeded, credentials matched
                    return true; //return true, user was found in ldap
                }
                catch 
                {
                    return false; //user not found in ldap system
                }
            }
            catch (LdapException ldapException)
            {
                //if ldap error is 49 meaning invalid credentials then return false
                //else throw error
                if (ldapException.ErrorCode.Equals(LDAPError_InvalidCredentials)) return false; //credentials did not match ldap system
                throw;
            }
        }

        static void GetParameters( string user, string password)
        {
            // When running: ConnectLDAP.exe <ldapServer> <user> <pwd> <domain> <targetOU>

            char[] passwordChars = password.ToCharArray();
           
            foreach (char c in passwordChars)
            {
                pass.AppendChar(c); //set string password to secureString pass
            }

            // test arguments to insure they are valid and secure

            // initialize variables
            ldapServer = "auth.ldap.untsystem.edu:636";
            credential = new NetworkCredential(user, pass);
           
        }
      
    }
}

