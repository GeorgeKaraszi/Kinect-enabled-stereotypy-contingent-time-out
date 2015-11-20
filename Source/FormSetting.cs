using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WesternMichgian.SeniorDesign.KinectProject
{
    class FormSetting
    {
        public static int framesTimer = 1;
        public static Boolean timerOrWindow = true;

        public FormSetting(int val)
        {
            framesTimer = val;
        }

        public FormSetting(bool val)
        {
            timerOrWindow= val;
        }

        public FormSetting()

        {
        }

        public int getTimer()
        {
            return framesTimer;
        }

        public bool getOption()
        {
            return timerOrWindow;
        }

   




    }

}
