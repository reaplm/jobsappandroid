using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace JobsAppAndroid
{
    public class RandomColorGenerator
    {
        public List<string> colors;
        Random r;

        public RandomColorGenerator()
        {
            colors = new List<string>
            {
                "#f48fb1", "#9fa8da", "#b388ff", "#90caf9", "#84ffff",
                "#ce93d8", "#ff80ab", "#8c9eff", "#81d4fa", "#a7ffeb",
                "#b39ddb", "#ea80fc", "#82b1ff", "#80deea", "#80d8ff"
            };

            r = new Random();
        }

        public Color GetColor(int i)
        {
            return Color.ParseColor(colors[i]);

        }
        public Color GetColor()
        {
            return Color.ParseColor(colors[r.Next(15)]);

        }
        public string GetColorString()
        {
            return (colors[r.Next(15)]);

        }
    }
}