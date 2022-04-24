using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vts.mvc
{
    public class SpeakNorm
    {
        public string proxy_name;
        public int index;

        public SpeakNorm(string proxy_name, int index)
        {
            this.proxy_name = proxy_name;
            this.index = index;
        }
    }
}
