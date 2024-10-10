namespace Utilities
{
    public static class CoordinateConverter
    {
        public static int Convert(int row, int column, int columnCount) => row * columnCount + column;
        
        public static (int row, int column) Convert (int index, int columnCount) => (index / columnCount, index % columnCount);
    }
}