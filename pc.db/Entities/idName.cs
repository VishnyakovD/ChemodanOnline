using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.db.Entities
{
    public class idName
    {
        public virtual long id { get; set; }
        public virtual string name { get; set; }
        public virtual int sortPriority { get; set; }
        public virtual bool isUse { get; set; }
    }

    public enum DisplayType
    {
        MainCarusel=0,
        MainInfoBlock=1,
        Favorite=2,
        Default=999
    }

    public static class TypesStreet
    {
        public static string Ylit { get; }= "улица";
        public static string Pros { get; }= "проспект";
        public static string Bulv { get; }= "бульвар";
        public static string Pere { get; }= "переулок";
        public static string Plos { get; }= "площадь";
        public static string Typi { get; }= "тупик";

        public static string[] Types { get; } = { Ylit, Pros, Bulv, Pere, Plos, Typi };
    }


}
