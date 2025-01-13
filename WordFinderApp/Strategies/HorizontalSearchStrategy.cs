public class HorizontalSearchStrategy : ISearchStrategy
{
    public bool Search(char[,] matrix, string word, int row, int col)
    {
        for (int k = 0; k < word.Length; k++)
        {
            int newCol = col + k;

            if (newCol >= matrix.GetLength(1) || matrix[row, newCol] != word[k])
            {
                return false;
            }
        }
        return true;
    }
}
