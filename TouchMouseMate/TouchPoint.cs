namespace Lextm.TouchMouseMate
{
    public struct TouchPoint
    {
        internal int X;
        internal int Y;
        internal int Time;
        internal int Pixel;
        internal double CenterX;
        internal double CenterY;
        public int PreviousPixel;
        public int PreviousPreviousPixel;

        public bool KeyUpDetected
        {
            get { return Pixel == 0 && PreviousPixel > 0; }
        }

        public bool KeyDownDetected
        {
            get { return PreviousPixel > 0 && PreviousPreviousPixel == 0; }
        }

        public void Reset()
        {
            X = 0;
            Y = 0;
            Time = 0;
            Pixel = 0;
            CenterX = 0;
            CenterY = 0;
        }

        public void Consume(int x, int y, int pixel)
        {
            X += x * pixel;
            Y += y * pixel;
            Pixel += pixel;
        }

        public void ComputeCenter()
        {
            CenterX = X / (double)Pixel;
            CenterY = Y / (double)Pixel;
        }

        public void Prepare()
        {
            PreviousPreviousPixel = PreviousPixel;
            PreviousPixel = Pixel;
            Reset();
        }
    }
}