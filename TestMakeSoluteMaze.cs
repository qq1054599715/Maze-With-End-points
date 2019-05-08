using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZycSolute;

public class TestMakeSoluteMaze : MonoBehaviour
{
    private SoluteMaze maze;
    private List<Cell> cells;
    public float width;
    public Cell stepCell;

    public ExpectPosition startPosition;

    public List<ExpectPosition> endPositions;

    private void OnDrawGizmos()
    {
        if (maze == null || cells == null)
        {
            return;
            // maze = new Maze(10,10);
            // cells = maze.RemakeMaze();
        }


        Gizmos.color = Color.red;
        if (stacks != null)
        {
            for (int i = 0; i < stacks.Count; i++)
            {
                var tstack = stacks[i];
                for (int j = 0; j < tstack.Count; j++)
                {
                    Gizmos.DrawSphere(new Vector3(tstack[j].x, tstack[j].y, 0) * width, width * 0.1f);
                }
            }
        }

        Gizmos.color = Color.white;

        Gizmos.DrawSphere(new Vector3(stepCell.x, stepCell.y, 0) * width, width * 0.5f);
        for (int i = 0; i < cells.Count; i++)
        {
            Cell currentCell = cells[i];
            if (currentCell.right == false)
            {
                Vector3 wall1 = new Vector3(currentCell.x + 0.5f, currentCell.y - 0.5f, 0) * width;
                Vector3 wall2 = new Vector3(currentCell.x + 0.5f, currentCell.y + 0.5f, 0) * width;
                Gizmos.DrawLine(wall1, wall2);
            }

            if (currentCell.left == false)
            {
                Vector3 wall1 = new Vector3(currentCell.x - 0.5f, currentCell.y - 0.5f, 0) * width;
                Vector3 wall2 = new Vector3(currentCell.x - 0.5f, currentCell.y + 0.5f, 0) * width;
                Gizmos.DrawLine(wall1, wall2);
            }

            if (currentCell.up == false)
            {
                Vector3 wall1 = new Vector3(currentCell.x - 0.5f, currentCell.y + 0.5f, 0) * width;
                Vector3 wall2 = new Vector3(currentCell.x + 0.5f, currentCell.y + 0.5f, 0) * width;
                Gizmos.DrawLine(wall1, wall2);
            }

            if (currentCell.down == false)
            {
                Vector3 wall1 = new Vector3(currentCell.x - 0.5f, currentCell.y - 0.5f, 0) * width;
                Vector3 wall2 = new Vector3(currentCell.x + 0.5f, currentCell.y - 0.5f, 0) * width;
                Gizmos.DrawLine(wall1, wall2);
            }
        }

        Gizmos.color = Color.blue;
        for (int i = 0; i < endPositions.Count; i++)
        {
            Gizmos.DrawSphere(new Vector3(endPositions[i].x, endPositions[i].y, 0) * width, width * 0.5f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(startPosition.x, startPosition.y, 0) * width, width * 0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(stepBystepRandom());
        StartCoroutine(stepBystepSearch());
    }

    //public List<Cell> stacks = new List<Cell>();
    public int mWidth, mHeight;
    private List<List<Cell>> stacks;

    public IEnumerator stepBystepSearch()
    {
        stacks = new List<List<Cell>>();
        maze = new SoluteMaze(mWidth, mHeight);
        maze.BeginSearchMaze(startPosition, endPositions);
        do
        {
            bool isOver = false;
            List<Cell> singleStack = new List<Cell>();
            bool getSolution = false;
            cells = maze.NextSearch(ref isOver, out stepCell, ref getSolution, ref singleStack);
            yield return new WaitForSeconds(0.01f);
            if (getSolution == true)
            {
                stacks.Add(singleStack);
            }

            if (isOver == true)
            {
                break;
            }
        } while (true);

        yield return 0;
    }
}
