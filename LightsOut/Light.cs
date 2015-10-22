using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOut
{
    /// <summary>
    /// Model representing a Light switch on the array of Lights
    /// </summary>
    public class Light
    {
        public int Row { get; set; }
        public int Column { get; set; }
        /// <summary>
        /// Is switched on or off 
        /// </summary>
        public Switch Switch { get; set; }

        public Light Left { get; set; }

        public Light Right { get; set; }

        public Light Top { get; set; }

        public Light Bottom { get; set; }

        // the performance would be better if it was binary, instead of a class type.
        internal bool GetSwitchState()
        {
            return (Switch == Switch.On ? true : false );
        }
    }
}
