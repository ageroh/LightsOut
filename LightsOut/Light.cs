using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOut
{
    public class Light
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public Switch Switch { get; set; }

        public Light Left { get; set; }

        public Light Right { get; set; }

        public Light Top { get; set; }

        public Light Bottom { get; set; }


        internal bool GetSwitchState()
        {
            return (Switch == Switch.On ? true : false );
        }
    }
}
