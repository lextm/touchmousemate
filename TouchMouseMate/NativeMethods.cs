using System;
using System.Runtime.InteropServices;
using Microsoft.Research.TouchMouseSensor;

namespace Lextm.TouchMouseMate
{
    internal class NativeMethods
    {
        internal static int MaxClickTimeout = 250;
        internal static int MinClickTimeout = 50;
        
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private static readonly TouchPoint[] TouchPoints = new TouchPoint[3];

        private struct TouchPoint
        {
            internal int X;
            internal int Y;
            internal int Time;
            internal int Pixel;
            internal double CenterX;
            internal double CenterY;
            internal int Enabled;
            public int Temp;

            public void Reset()
            {
                X = 0;
                Y = 0;
                Time = 0;
                Pixel = 0;
                CenterX = 0;
                CenterY = 0;
                Enabled = 0;
            }

            public void ResetAll()
            {
                Reset();
                Temp = 0;
            }
        }

        internal static bool[,] TouchMap = new bool[16, 14];
        internal static int[] LeftBound = { 3, 7, 2, 12 };
        internal static int[] MiddleBound = { 6, 7, 2, 12 };
        internal static int[] RightBound = { 8, 11, 2, 12 };
        internal static bool ClickDetected;
        internal static int[] PixelFound = { 0, 0 };
        internal static int OpMode = 2; // old middle click detection approach and new approach switch.
        public static bool TouchOverClick = true;
        public static bool MiddleClick = true;

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static MousePoint GetCursorPosition()
        {
            MousePoint currentMousePoint;
            var gotPoint = GetCursorPos(out currentMousePoint);
            if (!gotPoint)
            {
                currentMousePoint = new MousePoint(0, 0);
            }

            return currentMousePoint;
        }

        public static void MouseEvent(MouseEventFlags value)
        {
            MousePoint position = GetCursorPosition();
            mouse_event((int)value, position.X, position.Y, 0, 0);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public int X;
            public int Y;

            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        /// <summary>
        /// Function receiving callback from mouse.
        /// </summary>
        /// <param name="pTouchMouseStatus">Values indicating status of mouse.</param>
        /// <param name="pabImage">Bytes forming image, 13 rows of 15 columns.</param>
        /// <param name="dwImageSize">Size of image, assumed to always be 195 (13x15).</param>
        internal static void TouchMouseCallbackFunction(ref TOUCHMOUSESTATUS pTouchMouseStatus,
                                                        byte[] pabImage,
                                                        int dwImageSize)
        {
            // Reinizialize Touched Zones
            TouchPoints[0].Enabled = 0;
            TouchPoints[1].Enabled = 0;
            TouchPoints[2].Enabled = 0;
            PixelFound[0] = 0;
            PixelFound[1] = 0;

            // Iterate over rows.
            for (Int32 y = 0; y < pTouchMouseStatus.m_dwImageHeight; y++)
            {
                // Iterate over columns.
                for (Int32 x = 0; x < pTouchMouseStatus.m_dwImageWidth; x++)
                {
                    if (pabImage[pTouchMouseStatus.m_dwImageWidth*y + x] != 0) // analyze captured image for touch gesture.
                    {
                        TouchMap[x, y] = true; // touch detected
                        int pixel = pabImage[pTouchMouseStatus.m_dwImageWidth*y + x]; // touch strength recorded
                        // Get the pixel value at current position.
                        if (x >= LeftBound[0] && x <= LeftBound[1] &&
                            y >= LeftBound[2] && y <= LeftBound[3])
                        {
                            // Increment values.
                            if (TouchPoints[0].Enabled == 0 && pixel != 0)
                            {
                                TouchPoints[0].X += x*pixel;
                                TouchPoints[0].Y += y*pixel;
                                TouchPoints[0].Time += pTouchMouseStatus.m_dwTimeDelta;
                                TouchPoints[0].Pixel += pixel;
                                TouchPoints[0].Enabled = 1;
                            }
                        }
                        else if (x >= MiddleBound[0] && x <= MiddleBound[1] &&
                                 y >= MiddleBound[2] && y <= MiddleBound[3])
                        {
                            // Increment values.
                            if (TouchPoints[1].Enabled == 0 && pixel != 0)
                            {
                                TouchPoints[1].X += x*pixel;
                                TouchPoints[1].Y += y*pixel;
                                TouchPoints[1].Time += pTouchMouseStatus.m_dwTimeDelta;
                                TouchPoints[1].Pixel += pixel;
                                TouchPoints[1].Enabled = 1;
                            }
                        }
                        else if (x >= RightBound[0] && x <= RightBound[1] &&
                                 y >= RightBound[2] && y <= RightBound[3])
                        {
                            // Increment values.
                            if (TouchPoints[2].Enabled == 0 && pixel != 0)
                            {
                                TouchPoints[2].X += x*pixel;
                                TouchPoints[2].Y += y*pixel;
                                TouchPoints[2].Time += pTouchMouseStatus.m_dwTimeDelta;
                                TouchPoints[2].Pixel += pixel;
                                TouchPoints[2] .Enabled= 1;
                            }
                        }
                    }
                }
            }

            // Calculate and display the center of mass for the touches present.
            for (int i = 0; i <= 2; i++)
            {
                if (TouchPoints[i].Time > MinClickTimeout && TouchPoints[i].Time < MaxClickTimeout)
                {
                    TouchPoints[i].CenterX = TouchPoints[i].X/(double)TouchPoints[i].Pixel;
                    TouchPoints[i].CenterY = TouchPoints[i].Y/(double)TouchPoints[i].Pixel;
                }
            }

            if (pTouchMouseStatus.m_dwTimeDelta == 0)
            {
                // If the time delta is zero then there has been an 
                // undetermined delta since the last report.
                Console.WriteLine("New touch detected");
            }

            if (OpMode == 1)
            {
                ApproachOne(pTouchMouseStatus);
            }
            else if (OpMode == 2)
            {
                ApproachTwo(pTouchMouseStatus);
            }

            if (pTouchMouseStatus.m_fDisconnect)
            {
                // The mouse is now disconnected, if we had created objects to track 
                // the mouse they would be destroyed here.
                Console.WriteLine("\nMouse #{0:X4}: Disconnected\n",
                                  (pTouchMouseStatus.m_dwID & 0xFFFF));
            }
        }

        private static void ApproachTwo(TOUCHMOUSESTATUS pTouchMouseStatus)
        {
            for (int y = LeftBound[2]; y <= LeftBound[3]; y++)
            {
                for (int x = LeftBound[0]; x <= LeftBound[1]; x++)
                {
                    if (TouchMap[x, y] == true) PixelFound[0]++;
                }
            }
            for (int y = RightBound[2]; y <= RightBound[3]; y++)
            {
                for (int x = RightBound[0]; x <= RightBound[1]; x++)
                {
                    if (TouchMap[x, y] == true) PixelFound[1]++;
                }
            }

            // Iterate over columns.
            for (int i = 0; i <= 2; i = i + 2)
            {
                if (TouchPoints[i].Enabled == 0 && TouchPoints[i].Time != 0)
                {
                    // Click in the "i" touch zone is finished, checking valid clicks
                    Console.WriteLine(
                        "Mouse #{0:X4}: T= {1,6}MS, (Center ({2:F2}, {3:F2}), Zone: {4}, Left: {5}, Right: {6}",
                        (pTouchMouseStatus.m_dwID & 0xFFFF),
                        TouchPoints[i].Time,
                        TouchPoints[i].CenterX,
                        TouchPoints[i].CenterY,
                        i,
                        PixelFound[0],
                        PixelFound[1]);
                    if (i == 0)
                    {
                        if (TouchPoints[i].Time > MinClickTimeout && TouchPoints[i].Time < MaxClickTimeout &&
                            PixelFound[0] > 5 && PixelFound[0] < 17)
                        {
                            // Click in the "left" touch zone is valid
                            TouchPoints[0].Temp = 1;
                            ClickDetected = TouchPoints[2].Enabled == 0 || TouchPoints[2].Time >= MaxClickTimeout;
                        }
                        
                        TouchPoints[i].Reset();
                        for (int y = LeftBound[2]; y <= LeftBound[3]; y++)
                        {
                            for (int x = LeftBound[0]; x <= LeftBound[1]; x++)
                            {
                                TouchMap[x, y] = false;
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        if (TouchPoints[i].Time > MinClickTimeout && TouchPoints[i].Time < MaxClickTimeout &&
                            PixelFound[1] > 5 && PixelFound[1] < 17)
                        {
                            // Click in the "right" touch zone is valid
                            TouchPoints[2].Temp = 1;
                            ClickDetected = TouchPoints[0].Enabled == 0 || TouchPoints[0].Time >= MaxClickTimeout;
                        }
                        
                        TouchPoints[i].Reset();
                        for (int y = RightBound[2]; y <= RightBound[3]; y++)
                        {
                            for (int x = RightBound[0]; x <= RightBound[1]; x++)
                            {
                                TouchMap[x, y] = false;
                            }
                        }
                    }
                }
            }
            if (TouchPoints[0].Temp == 1 && TouchPoints[2].Temp == 1 && ClickDetected)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("MiddleClick Detected");
                Console.ResetColor();
                if (MiddleClick)
                {
                    MouseEvent(MouseEventFlags.MiddleDown);
                    MouseEvent(MouseEventFlags.MiddleUp);
                }

                TouchPoints[0].ResetAll();
                TouchPoints[2].ResetAll();
                ClickDetected = false;
            }
            else if (TouchPoints[0].Temp == 1 && ClickDetected)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("LeftClick Detected");
                Console.ResetColor();
                if (TouchOverClick)
                {
                    MouseEvent(MouseEventFlags.LeftDown);
                    MouseEvent(MouseEventFlags.LeftUp);
                }

                TouchPoints[0].ResetAll();
                ClickDetected = false;
            }
            else if (TouchPoints[2].Temp == 1 && ClickDetected)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("RightClick Detected");
                Console.ResetColor();
                if (TouchOverClick)
                {
                    MouseEvent(MouseEventFlags.RightDown);
                    MouseEvent(MouseEventFlags.RightUp);
                }

                TouchPoints[2].ResetAll();
                ClickDetected = false;
            }
        }

        private static void ApproachOne(TOUCHMOUSESTATUS pTouchMouseStatus)
        {
// approach 1: divide mouse surface to left, middle, right zones.
            for (int i = 0; i <= 2; i++)
            {
                if (TouchPoints[i].Enabled == 0 && TouchPoints[i].Time != 0)
                {
                    Console.WriteLine("Mouse #{0:X4}: T= {1,6}MS, (Center ({2:F2}, {3:F2}), Zone: {4}",
                                      (pTouchMouseStatus.m_dwID & 0xFFFF),
                                      TouchPoints[i].Time,
                                      TouchPoints[i].CenterX,
                                      TouchPoints[i].CenterY,
                                      i);
                    if (TouchPoints[i].Time > MinClickTimeout && TouchPoints[i].Time < MaxClickTimeout)
                    {
                        const int leftZone = 0;
                        const int middleZone = 1;
                        const int rightZone = 2;
                        if (i == leftZone && ClickDetected == false)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Leftclick Detected");
                            Console.ResetColor();
                            if (TouchOverClick)
                            {
                                MouseEvent(MouseEventFlags.LeftDown);
                                MouseEvent(MouseEventFlags.LeftUp);
                            }

                            ClickDetected = true;
                        }
                        else if (i == middleZone && ClickDetected == false)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Middleclick Detected");
                            Console.ResetColor();
                            if (MiddleClick)
                            {
                                MouseEvent(MouseEventFlags.MiddleDown);
                                MouseEvent(MouseEventFlags.MiddleUp);
                            }

                            ClickDetected = true;
                        }
                        else if (i == rightZone && ClickDetected == false)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Rightclick Detected");
                            Console.ResetColor();
                            if (TouchOverClick)
                            {
                                MouseEvent(MouseEventFlags.RightDown);
                                MouseEvent(MouseEventFlags.RightUp);
                            }

                            ClickDetected = true;
                        }
                    }

                    TouchPoints[i].Reset();
                }
            }

            ClickDetected = false;
        }
    }
}
