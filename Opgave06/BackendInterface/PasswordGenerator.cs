using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalTools
{
    public class PasswordGenerator : IEnumerable<string>
    {
        private int passwordLength;

        public PasswordGenerator(int passwordLength)
        {
            this.passwordLength = passwordLength;
        }

        public ulong Count()
        {
            ulong result = 1;
            for (int i = 0; i < passwordLength; i++)
            {
                result *= 26;
            }
            return result;
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var password in GetPasswords(0,new char[passwordLength]))
            {
                yield return password;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerable<string> GetPasswords(int index, char[] password)
        {
            if (index == passwordLength)
            {
                yield return new string(password);
            }
            else
            {
                for (char c = 'A'; c <='Z'; c++)
                {
                    password[index] = c;
                    foreach (var result in GetPasswords(index+1, password))
                    {
                        yield return result;

                    }
                }              
            }
        }
    }
}
