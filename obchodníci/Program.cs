using System.Xml.Linq;
using System;

 
namespace OOP_obchodníci
{
    internal class Program
    {
        static void Main(string[] args)     
        {
            
            //string filename = "smalltree.json";
            string filename = "largetree.json";
            Salesman boss = Salesman.DeserializeTree(File.ReadAllText(filename));
            UI uI = new UI(boss);
            uI.Start();
        }
       
    }
}