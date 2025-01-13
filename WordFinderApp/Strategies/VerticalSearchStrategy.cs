public class VerticalSearchStrategy : ISearchStrategy
{
    public bool Search(char[,] matrix, string word, int row, int col)
    {
        for (int k = 0; k < word.Length; k++)
        {
            int newRow = row + k;

            if (newRow >= matrix.GetLength(0) || matrix[newRow, col] != word[k])
            {
                return false;
            }
        }
        return true;
    }
}
