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
                "#ce93d8", "#90caf9", "#b64fc8", "#4d82cb", "#ff80ab","#9d46ff",
                "#b39ddb", "#81d4fa", "#805acb", "#49a7cc", "#ea80fc","#bc477b",
                "#9fa8da", "#80cbc4", "#5870cb", "#75ccb9", "#e254ff","#ff4081"
            };

            r = new Random();
        }

        public Color GetColor(int i)
        {
            return Color.ParseColor(colors[i]);

        }
        public Color GetColor()
        {
            return Color.ParseColor(colors[r.Next(18)]);

        }
        public string GetColorString()
        {
            return (colors[r.Next(18)]);

        }
    }
}