using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vkGroupWall
{
    class TotalCounter
    {
        public static int SuccessNum;
        public static int FailNum;
        public static int Valid;
        public static int Invalid;

        public static int SuccessMessages()
        {
            SuccessNum++;
            return SuccessNum;
        }

        public static int FailMessages()
        {
            FailNum++;
            return FailNum;
        }

        public static int ValidAccs()
        {
            Valid++;
            return Valid;
        }

        public static int InvalidAccs()
        {
            Invalid++;
            return Invalid;
        } 
    }
}
