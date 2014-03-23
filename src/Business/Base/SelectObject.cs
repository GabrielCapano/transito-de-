using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    public class SelectObject
    {
        public SelectObject(string text, string value, bool selected)
        {
            Text = text;
            Value = value;
            Selected = selected;
        }

        public SelectObject(string text, string value)
        {
            
            Text = text;
            Value = value;
            Selected = false;
        }

        public String Value { get; set; }
        public String Text { get; set; }

        public bool Selected { get; set; }
    }
}
