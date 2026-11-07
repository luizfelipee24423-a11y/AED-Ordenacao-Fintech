
public class QuickSort
{
    public static void Ordenar(Transacao[] array, int esq, int dir)
    {
        int i = esq;
        int j = dir;

        decimal pivo = array[dir].Valor;

        while (i <= j)
        {
            while (array[i].Valor < pivo)
            {
                i++;
            }

            while (array[j].Valor > pivo)
            {
                j--;
            }

            if (i <= j)
            {
                Transacao temp = array[i];
                array[i] = array[j];
                array[j] = temp;

                i++;
                j--;
            }
        }

        if (i < dir)
        {
            Ordenar(array, i, dir);
        }

        if (esq < j)
        {
            Ordenar(array, esq, j);
        }
    }
}
