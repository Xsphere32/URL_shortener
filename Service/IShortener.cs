using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URL_shortener.Service
{
    public interface IShortener
    {
        string Encrypt(string Url);
        string Decrypt(string EncryptedUrl);
    }
}
