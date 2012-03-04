namespace Lextm.TouchMouseMate
{
    public struct TouchPoint
    {
        internal int X;
        internal int Y;
        internal int Pixel;
        internal double CenterX;
        internal double CenterY;

        public void Reset()
        {
            X = 0;
            Y = 0;
            Pixel = 0;
            CenterX = 0F;
            CenterY = 0F;
        }

        public void Consume(int x, int y, int pixel)
        {
            X += x * pixel;
            Y += y * pixel;
            Pixel += pixel;
        }

        public void ComputeCenter()
        {
            if (Pixel > 0)
            {
                CenterX = X/(double) Pixel;
                CenterY = Y/(double) Pixel;
            }
        }
    }
}