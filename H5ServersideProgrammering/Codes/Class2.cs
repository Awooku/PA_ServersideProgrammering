using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;

namespace H5ServersideProgrammering.Codes
{
    public class Class2
    {
        public string GetHashValue_MD5(string value)
        {
            byte[] valueAsBytes = ASCIIEncoding.ASCII.GetBytes(value);
            byte[] hashedValueAsBytes = MD5.HashData(valueAsBytes);
            string hashedValueAsString = Convert.ToBase64String(hashedValueAsBytes);
            // https://stackoverflow.com/questions/33125624/how-to-use-salt-with-md5-technique

            return hashedValueAsString;
        }

        public string GetHashValue_HMACSHA1(string value)
        {
            byte[] valueAsBytes = ASCIIEncoding.ASCII.GetBytes(value);
            byte[] hashedValueAsBytes = new HMACSHA1().ComputeHash(valueAsBytes);
            string hashedValueAsString = Convert.ToBase64String(hashedValueAsBytes);

            return hashedValueAsString;
        }

        public string GetHashValue_PBKDF2(string value)
        {
            byte[] valueAsBytes = ASCIIEncoding.ASCII.GetBytes(value);
            
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }

            Rfc2898DeriveBytes hashedValueAsBytes = new Rfc2898DeriveBytes(valueAsBytes, salt, 4);
            string hashedTekst_PBKDF2 = Convert.ToBase64String(hashedValueAsBytes.GetBytes(32));

            return hashedTekst_PBKDF2;
        }

        public string GetHashValue_BCrypt(string value) => 
            BCrypt.Net.BCrypt.HashPassword(value, BCrypt.Net.SaltRevision.Revision2Y);

        #region Verifiers

        public bool VerifyPBKDF2(string password, byte[] salt, string hashPass)
        {
            bool result = true;

            Rfc2898DeriveBytes tmpHashT = new Rfc2898DeriveBytes(password, salt, 100000);
            string hashedTekst_PBKDF2 = Convert.ToBase64String(tmpHashT.GetBytes(32));

            result = hashPass.Equals(hashedTekst_PBKDF2);
            return result;
        }

        public bool VerifyBCrypt(string originaltTekst, string hashedTekst_BCrypt_Revision2Y)
        {
            return BCrypt.Net.BCrypt.Verify(originaltTekst, hashedTekst_BCrypt_Revision2Y);
        }
        #endregion
    }
}
