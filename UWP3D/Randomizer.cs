using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace UWP3D
{
    /// <summary>
    /// Потокобезопасный статичесский рандомизатор
    /// </summary>
    public static class Randomizer
    {
        private static Random _random;
        private static object _lock;

        static Randomizer()
        {
            _random = new Random();
            _lock = new object();
        }

        public static double NextDouble()
        {
            lock(_lock)
            {
                return _random.NextDouble();
            }
        }

        public static int Next(int from, int to)
        {
            lock(_lock)
            {
                return _random.Next(from, to);
            }
        }

        public static byte NextByte(byte from, byte to)
        {
            lock(_lock)
            {
                return (byte)_random.Next(from, to);
            }
        }

        public static Color RandomColor()
        {
            lock(_lock)
            {
                return Color.FromArgb(255,
                    (byte)_random.Next(0, 255),
                    (byte)_random.Next(0, 255),
                    (byte)_random.Next(0, 255));
            }
        }
    }
}
