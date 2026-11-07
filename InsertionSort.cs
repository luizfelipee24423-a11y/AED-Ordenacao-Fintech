
public class InsertionSort
{
    public static void Ordenar(Transacao[] array)
    {
        int n = array.Length;

        for (int i = 1; i < n; i++)
        {
            Transacao temp = array[i];
            int j = i - 1;

            while (j >= 0 && array[j].Valor > temp.Valor)
            {
                array[j + 1] = array[j];
                j--;
            }

            array[j + 1] = temp;
        }
    }
}
