namespace Core.Models
{
    public readonly struct CellNeighbors
    {
        public CellNeighbors(int leftTop, int leftBot, int rightTop, int rightBot)
        {
            LeftTop = leftTop;
            LeftBot = leftBot;
            RightTop = rightTop;
            RightBot = rightBot;
        }

        public int LeftBot { get; }

        public int LeftTop { get; }

        public int RightBot { get; }

        public int RightTop { get; }

        public bool IsNeighbor(int cellId) =>
            cellId != -1 && (LeftBot == cellId || LeftTop == cellId || RightBot == cellId || RightTop == cellId);

        public int GetIntersection(CellNeighbors other)
        {
            if      (LeftTop == other.RightBot && LeftTop != -1)  return LeftTop;
            else if (LeftBot == other.RightTop && LeftBot != -1)  return LeftBot;
            else if (RightTop == other.LeftBot && RightTop != -1) return RightTop;
            else if (RightBot == other.LeftTop && RightBot != -1) return RightBot;

            return -1;
        }

    }
}
