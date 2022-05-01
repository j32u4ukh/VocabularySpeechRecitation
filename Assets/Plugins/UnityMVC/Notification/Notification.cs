using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMVC
{
    public class Notification : INotification
    {
        string name = string.Empty;
        object body = null;
        string header = string.Empty;

        public Notification(string name, object body = null, string header = null)
        {
            setName(name: name);
            setBody(body: body);
            setHeader(header: header);
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return name;
        }

        public void setBody(object body)
        {
            this.body = body;
        }

        public object getBody()
        {
            return body;
        }

        public void setHeader(string header)
        {
            this.header = header;
        }

        public string getHeader()
        {
            return header;
        }

        public string toString()
        {
            string description = "Notification Name: " + name;
            description += "\nHeader:" + ((header == null) ? "null" : header);
            description += "\nBody:" + ((body == null) ? "null" : body.ToString());
            return description;
        }
    }
}