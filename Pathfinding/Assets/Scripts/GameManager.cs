using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GameManager : MonoBehaviour
{
    public GameObject token1, token2, token3, token4;
    private int[,] GameMatrix; //0 not chosen, 1 player, 2 enemy
    private int[] startPos = new int[2];
    private int[] objectivePos = new int[2];
    private void Awake()
    {
        GameMatrix = new int[Calculator.length, Calculator.length];

        for (int i = 0; i < Calculator.length; i++) //fila
            for (int j = 0; j < Calculator.length; j++) //columna
                GameMatrix[i, j] = 0;

        //randomitzar pos final i inicial;
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        startPos[0] = rand1;
        startPos[1] = rand2;
        SetObjectivePoint(startPos);

        GameMatrix[startPos[0], startPos[1]] = 1;
        GameMatrix[objectivePos[0], objectivePos[1]] = 2;

        InstantiateToken(token1, startPos);
        InstantiateToken(token2, objectivePos);
        ShowMatrix();

        //Seteamos el primer nodo 
        Node nodoInicial = new Node(Calculator.CheckDistanceToObj(startPos, objectivePos), new Vector2(startPos[0], startPos[1]), null);
        listaAbierta.Add(nodoInicial);
    }
    private void InstantiateToken(GameObject token, int[] position)
    {
        Instantiate(token, Calculator.GetPositionFromMatrix(position),
            Quaternion.identity);
    }
    private void SetObjectivePoint(int[] startPos) 
    {
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        if (rand1 != startPos[0] || rand2 != startPos[1])
        {
            objectivePos[0] = rand1;
            objectivePos[1] = rand2;
        }
    }

    private void ShowMatrix() //fa un debug log de la matriu
    {
        string matrix = "";
        for (int i = 0; i < Calculator.length; i++)
        {
            for (int j = 0; j < Calculator.length; j++)
            {
                matrix += GameMatrix[i, j] + " ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }
    //EL VOSTRE EXERCICI COMENÇA AQUI

    //Creamos las listas
    List<Node> listaAbierta = new List<Node>();
    List<Node> listaCerrada = new List<Node>();

    private void FixedUpdate()
    {
        if(!EvaluateWin())
        {
            //Ordenamos
            listaAbierta.Sort();
            //Añadimos los nodos alrededor del nodo actual
            listaAbierta.AddRange(CreaNodosAlrededor(listaAbierta[0]));
            //Añadimos el nodo a la lista cerrada
            listaCerrada.Add(listaAbierta[0]);
            //Lo eliminamos de la lista abierta porque ya lo hemos revisado
            listaAbierta.Remove(listaAbierta[0]);
        }
        else
        {
            MuestraCamino(listaCerrada[listaCerrada.Count - 1]);

        }
    }
    // Pone el camino más corto en verde
    private void MuestraCamino(Node nodo)
    {
        if (nodo.nodoPadre != null)
        {
            int[] position = { (int)nodo.posicion.x, (int)nodo.posicion.y };
            InstantiateToken(token4, position);
            MuestraCamino(nodo.nodoPadre);
        }
    }

    private bool EvaluateWin()
    {
        if (listaCerrada.Count == 0) return false;
        if (listaCerrada[listaCerrada.Count - 1].posicion == new Vector2(objectivePos[0], objectivePos[1])) return true;
        return false;
    }

    //Crea los nodos de alrededor del nodo actual
    private List<Node> CreaNodosAlrededor(Node nodoActual)
    {
        List<Node> nodos = new List<Node>();
        //Arriba
        if (nodoActual.posicion[0] > 0)
        {
            Vector2 posicionNodo = new Vector2(nodoActual.posicion.x - 1, nodoActual.posicion.y);
            int[] posicionCalculator = { (int)posicionNodo[0], (int)posicionNodo[1] };
            nodos.Add(new Node(Calculator.CheckDistanceToObj(posicionCalculator,objectivePos), posicionNodo, nodoActual));
            InstantiateToken(token3, posicionCalculator);

        }
        //Abajo
        if (nodoActual.posicion[0] < Calculator.length - 1)
        {
            Vector2 posicionNodo = new Vector2(nodoActual.posicion.x + 1, nodoActual.posicion.y);
            int[] posicionCalculator = { (int)posicionNodo[0], (int)posicionNodo[1] };
            nodos.Add(new Node(Calculator.CheckDistanceToObj(posicionCalculator, objectivePos), posicionNodo, nodoActual));
            InstantiateToken(token3, posicionCalculator);

        }
        //Izquierda
        if (nodoActual.posicion[1] > 0)
        {
            Vector2 posicionNodo = new Vector2(nodoActual.posicion.x, nodoActual.posicion.y -1);
            int[] posicionCalculator = { (int)posicionNodo[0], (int)posicionNodo[1] };
            nodos.Add(new Node(Calculator.CheckDistanceToObj(posicionCalculator, objectivePos), posicionNodo, nodoActual));
            InstantiateToken(token3, posicionCalculator);

        }
        //Derecha
        if (nodoActual.posicion[1] < Calculator.length - 1)
        {
            Vector2 posicionNodo = new Vector2(nodoActual.posicion.x, nodoActual.posicion.y + 1);
            int[] posicionCalculator = { (int)posicionNodo[0], (int)posicionNodo[1] };
            nodos.Add(new Node(Calculator.CheckDistanceToObj(posicionCalculator, objectivePos), posicionNodo, nodoActual));
            InstantiateToken(token3, posicionCalculator);

        }
        return nodos;
    }
}
