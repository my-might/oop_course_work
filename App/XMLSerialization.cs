using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

namespace App
{
    public class XMLSerialization
    {
        public static void ExportData(List<Observer> observers)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Observer>));
            StreamWriter sw = new System.IO.StreamWriter("./../data/observers.xml");
            ser.Serialize(sw, observers);
            sw.Close();
        }

        public static List<Observer> ImportData()
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Observer>));
            string xmlData = File.ReadAllText("./../data/observers.xml");
            StringReader reader = new StringReader(xmlData);
            List<Observer> observers = (List<Observer>)ser.Deserialize(reader);
            reader.Close();
            return observers;
        }
    }
}