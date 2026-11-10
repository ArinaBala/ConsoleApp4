
using System;
using System.Collections.Generic;
namespace ConsoleApp4
{
    public class DataObject
    {
        public byte[] Data { get; set; }

        public DataObject(int size)
        {
            Data = new byte[size];
        }
    }
}