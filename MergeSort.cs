
public class MergeSort
{
    public static void Ordenar(Transacao[] array, int esq, int dir)
    {
        if (esq < dir)
        {
            int meio = esq + (dir - esq) / 2;

            Ordenar(array, esq, meio);
            Ordenar(array, meio + 1, dir);

            Intercalar(array, esq, meio, dir);
        }
    }

    public static void Intercalar(Transacao[] array, int esq, int meio, int dir)
    {
        int tamanhoEsquerda = meio - esq + 1;
        int tamanhoDireita = dir - meio;

        Transacao[] vetorEsquerda = new Transacao[tamanhoEsquerda];
        Transacao[] vetorDireita = new Transacao[tamanhoDireita];

        for (int i = 0; i < tamanhoEsquerda; i++)
        {
            vetorEsquerda[i] = array[esq + i];
        }

        for (int i = 0; i < tamanhoDireita; i++)
        {
            vetorDireita[i] = array[meio + 1 + i];
        }

        int indiceEsquerda = 0;
        int indiceDireita = 0;
        int indiceArray = esq;

        while (indiceEsquerda < tamanhoEsquerda &&
               indiceDireita < tamanhoDireita)
        {
            if (vetorEsquerda[indiceEsquerda].Valor <=
                vetorDireita[indiceDireita].Valor)
            {
                array[indiceArray] = vetorEsquerda[indiceEsquerda];
                indiceEsquerda++;
            }
            else
            {
                array[indiceArray] = vetorDireita[indiceDireita];
                indiceDireita++;
            }

            indiceArray++;
        }

        while (indiceEsquerda < tamanhoEsquerda)
        {
            array[indiceArray] = vetorEsquerda[indiceEsquerda];

            indiceEsquerda++;
            indiceArray++;
        }

        while (indiceDireita < tamanhoDireita)
        {
            array[indiceArray] = vetorDireita[indiceDireita];

            indiceDireita++;
            indiceArray++;
        }
    }
}
