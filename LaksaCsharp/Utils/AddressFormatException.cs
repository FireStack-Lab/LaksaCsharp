using System;
using System.Collections.Generic;
using System.Text;

namespace LaksaCsharp.Utils
{
    public class AddressFormatException : ArgumentException
    {
        public AddressFormatException() : base()
        {
        }

        public AddressFormatException(string message) : base(message)
        {
        }
    }


    public class InvalidCharacter : AddressFormatException
    {
        public char character;
        public int position;

        public InvalidCharacter(char character, int position) : base("Invalid character '" + character.ToString() + "' at position " + position)
        {
            this.character = character;
            this.position = position;
        }
    }


    public class InvalidDataLength : AddressFormatException
    {
        public InvalidDataLength() : base()
        {
        }

        public InvalidDataLength(string message) : base(message)
        {
        }
    }


    public class InvalidChecksum : AddressFormatException
    {
        public InvalidChecksum() : base("Checksum does not validate")
        {
        }

        public InvalidChecksum(string message) : base(message)
        {
        }
    }


    public class InvalidPrefix : AddressFormatException
    {
        public InvalidPrefix() : base()
        {
        }

        public InvalidPrefix(string message) : base(message)
        {
        }
    }


    public class WrongNetwork : InvalidPrefix
    {
        public WrongNetwork(int versionHeader) : base("Version code of address did not match acceptable versions for network: " + versionHeader)
        {
        }

        public WrongNetwork(string hrp) : base("Human readable part of address did not match acceptable HRPs for network: " + hrp)
        {
        }
    }
}
