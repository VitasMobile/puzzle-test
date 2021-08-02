namespace PuzzleTest
{
    [System.Serializable]
    public class TileData
    {
        public float offsetX;
        public float offsetY;
        public float width;
        public float height;
        public string name;
        public string img;
        public float texX;
        public float texY;
    }

    [System.Serializable]
    public class TileList
    {
        public TileData[] tiles;
    }

}
